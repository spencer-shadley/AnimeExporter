namespace AnimeExporter {
    public static class MyAnimeListInfo {
        public const string TopAnimeBaseUrl = "https://myanimelist.net/topanime.php";
        public const string UrlClass = "hoverinfo_trigger fl-l ml12 mr8";

        public static string GetTopAnimeUrl(int page) {
            return TopAnimeBaseUrl + "?limit=" + page*50;
        }
    }
}