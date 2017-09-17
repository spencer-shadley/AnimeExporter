namespace AnimeExporterTests.TestUtility {
    
    /// <summary>
    /// This represents known animes - essentially an "expected" file
    /// </summary>
    public class TestConstants {

        public class DefaultAttributes {
            public const string Title = "Title";
            public const string Url = "URL";
        }

        public class KimiNoNaWa {
            public const string EnglishTitle = "Your Name.";
            public const string Genres = "Supernatural; Drama; Romance";
            public const string ImageUrl = "https://myanimelist.cdn-dena.com/images/anime/5/87048.jpg";
            public const string Title = "Kimi no Na wa.";
            public const string Type = "Movie";
            public const string Url = "https://myanimelist.net/anime/32281/Kimi_no_Na_wa";
        }

        public class SteinsGate {
            public const string Adapatation = "Steins;Gate (https://myanimelist.net/manga/17517/Steins_Gate)";
            public const string AirFinishDate = "Sep 14, 2011";
            public const string AirStartDate = "Apr 6, 2011";
            public const string AlternativeSetting = "ChäoS;HEAd (https://myanimelist.net/anime/4975/ChäoS_HEAd); Robotics;Notes (https://myanimelist.net/anime/13599/Robotics_Notes); ChäoS;Child (https://myanimelist.net/anime/30485/ChäoS_Child); Occultic;Nine (https://myanimelist.net/anime/32962/Occultic_Nine)";
            public const string PartialBackground = "Steins;Gate";
            public const string Url = "https://myanimelist.net/anime/9253/Steins_Gate";
        }

        public class OwarimongatariSecondSeason {
            public const string Prequel = "Owarimonogatari (https://myanimelist.net/anime/31181/Owarimonogatari); Koyomimonogatari (https://myanimelist.net/anime/32268/Koyomimonogatari)";
            public const string Sequel = "Hanamonogatari (https://myanimelist.net/anime/21855/Hanamonogatari)";
            public const string Url = "https://myanimelist.net/anime/35247/Owarimonogatari_2nd_Season";
        }

        public class FMABrotherhood {
            public const string Director = "Irie, Yasuhiro";
            public const string ExecutiveProducer = "Fukunaga, Gen";
            public const bool IsInEnglish = true;
            public const bool IsInJapanese = true;
            public const bool IsInSpanish = true;
            public const string Producer = "Cook, Justin";
            public const string SoundDirector = "Mima, Masafumi";
            public const string Url = "https://myanimelist.net/anime/5114/Fullmetal_Alchemist__Brotherhood";
        }

        public class KimiNiTodoke {
            public const string Director = "Kaburagi, Hiro";
            public const string ExecutiveProducer = ""; // No Executive Producer
            public const bool IsInEnglish = false;
            public const bool IsInJapanese = true;
            public const bool IsInSpanish = false;
            public const string Producer = "Ishikawa, Michiru";
            public const string SoundDirector = "Yamada, Chiaki";
            public const string Url = "https://myanimelist.net/anime/6045/Kimi_ni_Todoke";
        }
    }
}