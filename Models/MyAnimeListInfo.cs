namespace AnimeExporter.Models {
    public static class MyAnimeListInfo {
        public const string TopAnimeBaseUrl = "https://myanimelist.net/topanime.php";
        public const string UrlClass = "hoverinfo_trigger fl-l ml12 mr8";
        public const string AnimeTitleClass = "h1";

        public static string GetTopAnimeUrl(int page) {
            return TopAnimeBaseUrl + "?limit=" + page*50;
        }
    }
}