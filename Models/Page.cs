using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using HtmlAgilityPack;

namespace AnimeExporter.Models {
    public class Page {
        protected HtmlNode Doc;
        
        public Page(HtmlNode document) {
            Doc = document;
        }

        public static string SelectValue(HtmlNode node, string xPath) {
            HtmlNodeCollection nodes = node.SelectNodes(xPath);
            Debug.Assert(nodes.Count == 1);
            return WebUtility.HtmlDecode(nodes[0].InnerText);
        }

        public HtmlNode SelectElementByText(string text) {
            string xPath = $"//span[text() = '{text}']";
            HtmlNode selected = Doc.SelectSingleNode(xPath);

            if (selected != null) return selected;
            
            Console.Error.WriteLine($"No nodes were selected for {text}");
            return null;
        }

        public string SelectAllSiblingAnchorElements(string text, string defaultText = "None found") {
            return this.SelectAllSiblingAnchorElements(this.SelectElementByText(text), defaultText);
        }

        public string SelectAllSiblingAnchorElements(HtmlNode node, string defaultText = "None found") {
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
            return string.Join(", ", anchorTexts);
        }

        /// <summary>
        /// Much of the information on MyAnimeList is representing as plaintext after an element.
        /// This method allows grabbing that plaintext value by selecting the preceeding element by
        /// selecting based on the InnerText value
        /// </summary>
        /// <remarks>Assumes that the text is within a span element</remarks>
        /// <param name="text">The text to select</param>
        /// <returns>The trimmed InnerText of the element after the element selected by <see cref="text"/></returns>
        public string SelectValueAfterText(string text) {
            string xPath = $"//span[text() = '{text}']";
            return this.SelectValueAfter(xPath);
        }

        public string SelectValuesAfterText(string text) {
            return null;
        }
        
        /// <summary>
        /// Many of the statistics are stored as floating plaintext after an element. This method makes it
        /// easier to grab that floating text.
        /// </summary>
        /// <returns>The trimmed InnerText of the next child of the node at <see cref="xPath"/></returns>
        public string SelectValueAfter(string xPath) {
            HtmlNodeCollection selectedNodes = Doc.SelectNodes(xPath);

            if (selectedNodes == null) {
                Console.Error.WriteLine($"No nodes were selected for {xPath}");
                return null;
            }
            if (selectedNodes.Count != 1) {
                Console.Error.WriteLine($"There were {selectedNodes.Count} nodes selected");
            }

            HtmlNode valueNode = selectedNodes[0].NextSibling;
            return WebUtility.HtmlDecode(valueNode.InnerText.Trim());
        }

        /// <summary>
        /// Gets the InnerText of the element which has an itemprop of <see cref="itemProp"/>
        /// </summary>
        /// <remarks>Assumes the text is with in a span element</remarks>
        /// <param name="itemProp">The value of the itemprop to search for</param>
        /// <returns>The InnerText of the element with an itemprop equal to <see cref="itemProp"/></returns>
        public string SelectValueOfItemProp(string itemProp) {
            string xPath = $"//span[@itemprop=\"{itemProp}\"]";
            return SelectValue(Doc, xPath);
        }
        
        public static HtmlNodeCollection FindElementsWithClass(HtmlNode node, string className) {
            string xPath = $"//*[contains(@class,'{className}')]";
            return node.SelectNodes(xPath);
        }
    }
}