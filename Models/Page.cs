using System.Diagnostics;
using HtmlAgilityPack;

namespace AnimeExporter.Models {
    public class Page {
        protected HtmlNode Doc;
        
        public Page(HtmlNode document) {
            Doc = document;
        }

        public static string GetValue(HtmlNode node, string xPath) {
            HtmlNodeCollection nodes = node.SelectNodes(xPath);
            Debug.Assert(nodes.Count == 1);
            return nodes[0].InnerText;
        }
        
        public static HtmlNodeCollection FindElementsWithClass(HtmlNode node, string className) {
            string xPath = $"//*[contains(@class,'{className}')]";
            return node.SelectNodes(xPath);
        }
    }
}