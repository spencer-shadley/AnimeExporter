﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace AnimeExporter {
    
    public class Page {
        
        public const string MyAnimeListBaseUrl = "https://myanimelist.net";
        
        public const int MaxRetryCount = 10;
        
        public static string Delimiter => "; ";

        protected readonly string Url;
        
        protected HtmlNode Node;
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public Page(string url) {
            this.Url = url;
            this.TryLoadPage();
        }

        /// <summary>
        /// Try to load the page at <see cref="Url"/>
        /// </summary>
        /// <param name="retriesLeft">Number of retries remaining</param>
        /// <remarks>MyAnimeList has a rate limiting system which frequently causes a 429 - Too Many Requests</remarks>
        /// <exception cref="HttpRequestException">Thrown whenever a non-200 response comes back</exception>
        private void TryLoadPage(int retriesLeft = 10) {
            try {
                var web = new HtmlWeb();
                HtmlDocument doc = web.Load(this.Url);

                if (web.StatusCode != HttpStatusCode.OK) {
                    string errorMessage = $"Received status of {web.StatusCode}, {this.Url} could not be loaded";
                    Log.Error(errorMessage);
                    throw new HttpRequestException(errorMessage);
                }

                this.Node = doc.DocumentNode;
            }
            catch (Exception e) {
                Log.Error($"Failed to load page at {this.Url}", e);
                BackOff(retriesLeft);
                this.TryLoadPage(retriesLeft - 1);
            }
        }

        /// <summary>
        // Exponentially wait
        /// </summary>
        /// <remarks>Combats rate throttling</remarks>
        /// <remarks>Rate: 2^x * 100ms</remarks>
        public static void BackOff(int retriesLeft) {
            Log.Debug($"{retriesLeft} retries are left");
            
            const int backOffRate = 100;
            
            double waitTime = Math.Pow(2, (MaxRetryCount - retriesLeft)) * backOffRate;
            
            Log.Info($"Waiting {waitTime/1000} seconds to retry..." + Environment.NewLine);
            
            Task.Delay((int) waitTime).Wait();
            
            Log.Info("Retrying...");
        }
        
        /// <summary>
        /// Builds a url schema
        /// </summary>
        /// <param name="baseString">The string to continue building on</param>
        /// <param name="anchorNode">The node to parse and append to <see cref="baseString"/></param>
        /// <example>
        /// Kimi no Na wa. (https://myanimelist.net/manga/99314/Kimi_no_Na_wa); Kimi no Na Wa. (https://myanimelist.net/manga/108011/Kimi_no_Na_Wa)
        /// </example>
        protected static void BuildUrls(ref string baseString, HtmlNode anchorNode) {
            if (!anchorNode.Name.Equals("a")) { return; }
            
            if (baseString != string.Empty) {
                baseString += Delimiter;
            }
            
            string text = anchorNode.InnerText;
            string url = MyAnimeListBaseUrl + anchorNode.Attributes["href"].Value;
            baseString += $"{text} ({url})";
        }

        protected static string SelectValue(HtmlNode node, string xPath) {
            HtmlNodeCollection nodes = node.SelectNodes(xPath);

            if (nodes == null) {
                Log.Warn($"No nodes were selected for {xPath}");
                return null;
            }

            if (nodes.Count != 1) {
                Log.Warn($"There were {nodes.Count} nodes selected");
            }
            
            return WebUtility.HtmlDecode(nodes[0].InnerText);
        }

        protected HtmlNode SelectElementByText(string text) {
            string xPath = $"//span[text() = '{text}']";
            HtmlNode selected = this.Node.SelectSingleNode(xPath);

            if (selected != null) return selected;
            
            Log.Warn($"No nodes were selected for {text}");
            return null;
        }

        protected HtmlNodeCollection SelectElementsByType(HtmlNode node, string type) {
            string xPath = $"{type}";
            HtmlNodeCollection selected = node.SelectNodes(xPath);

            if (selected != null && selected.Count > 0) return selected;
            
            Log.Warn($"No nodes were selected for {type}");
            return null;
        }

        protected string SelectAllSiblingAnchorElements(string text, string defaultText = "None found") {
            return this.SelectAllSiblingAnchorElements(this.SelectElementByText(text), defaultText);
        }

        protected string SelectAllSiblingAnchorElements(HtmlNode node, string defaultText = "None found") {
            var anchorTexts = new List<string>();

            // When there are no known anchors, MyAnimeList inserts "None found"
            if (node.NextSibling.InnerText.Contains("None found")) {
                return defaultText;
            }
            
            while (node.NextSibling != null) {
                if (node.Name == "a") {
                    anchorTexts.Add(WebUtility.HtmlDecode(node.InnerText));
                }
                node = node.NextSibling;
            }
            return string.Join(Delimiter, anchorTexts);
        }

        /// <summary>
        /// Much of the information on MyAnimeList is representing as plaintext after an element.
        /// This method allows grabbing that plaintext value by selecting the preceeding element by
        /// selecting based on the InnerText value
        /// </summary>
        /// <remarks>Assumes that the text is within a span element</remarks>
        /// <param name="text">The text to select</param>
        /// <returns>The trimmed InnerText of the element after the element selected by <see cref="text"/></returns>
        protected string SelectValueAfterText(string text) {
            string xPath = $"//span[text() = '{text}']";
            return this.SelectValueAfter(xPath);
        }
        
        /// <summary>
        /// Many of the statistics are stored as floating plaintext after an element. This method makes it
        /// easier to grab that floating text.
        /// </summary>
        /// <returns>The trimmed InnerText of the next child of the node at <see cref="xPath"/></returns>
        protected string SelectValueAfter(string xPath) {
            HtmlNodeCollection selectedNodes = this.Node.SelectNodes(xPath);

            if (selectedNodes == null) {
                Log.Warn($"No nodes were selected for {xPath}");
                return null;
            }
            if (selectedNodes.Count != 1) {
                Log.Warn($"There were {selectedNodes.Count} nodes selected");
            }

            HtmlNode valueNode = selectedNodes[0].NextSibling;
            return WebUtility.HtmlDecode(valueNode.InnerText.Trim());
        }

        protected HtmlNode SelectElementByItemProp(string itemProp) {
            string xPath = $"//span[@itemprop=\"{itemProp}\"]";
            return this.Node.SelectSingleNode(xPath);
        }

        /// <summary>
        /// Gets the InnerText of the element which has an itemprop of <see cref="itemProp"/>
        /// </summary>
        /// <remarks>Assumes the text is with in a span element</remarks>
        /// <param name="itemProp">The value of the itemprop to search for</param>
        /// <returns>The InnerText of the element with an itemprop equal to <see cref="itemProp"/></returns>
        protected string SelectValueOfItemProp(string itemProp) {
            string xPath = $"//span[@itemprop=\"{itemProp}\"]";
            return SelectValue(this.Node, xPath);
        }
        
        protected HtmlNodeCollection FindElementsWithClass(string className) {
            string xPath = $"//*[contains(@class,'{className}')]";
            return this.Node.SelectNodes(xPath);
        }
        
        protected HtmlNode FindElementWithClass(string className) {
            HtmlNodeCollection elements = this.FindElementsWithClass(className);
            
            if (elements == null) {
                Log.Warn($"No nodes were selected for {className}");
                return null;
            }
            
            if (elements.Count != 1) {
                Log.Warn($"There were {elements.Count} nodes selected");
            }
            
            return elements[0];
        }
    }
}