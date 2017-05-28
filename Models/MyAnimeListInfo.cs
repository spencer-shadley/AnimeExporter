namespace AnimeExporter {
    public static class MyAnimeListInfo {
        public const string Url = "https://myanimelist.net/topanime.php";
        public const string SkipParam = "limit"; // the amount of anime to skip past; MyAnimeList always uses blocks of 50 anime
        public const int LimitAmount = 10;

        public static string TopAnimeUrl {
            get { return Url + "?" + SkipParam + "=" + LimitAmount; }
        }
    }
}