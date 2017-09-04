using AnimeExporter.Models;
using AnimeExporter.Utility;
using HtmlAgilityPack;

namespace AnimeExporter.Controllers {
    public class VideoController : Page {
        
        // TODO: add tests
        // example of no streaming: https://myanimelist.net/anime/1974/Glass_no_Kamen_2005/video
        // example of hulu only: https://myanimelist.net/anime/2927/Kimikiss_Pure_Rouge/video
        // example of hulu and crunchyroll: https://myanimelist.net/anime/30187/Sakurako-san_no_Ashimoto_ni_wa_Shitai_ga_Umatteiru/video

        private const string VideoProviderClasses = "video-providers";
        private const string PromoVideoClasses    = "iframe js-fancybox-video video-list di-ib po-r";
        
        private const string BaseImagePath    = "https://myanimelist.cdn-dena.com/images/episodes/videos/";
        private const string CrunchyRollImage = BaseImagePath + "icon_crunchyroll_small.png";
        private const string HuluImage        = BaseImagePath + "icon_hulu_small.png";
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public VideoController(string url) : base(url + "/video") { }

        private string PromoVideo {
            get {
                HtmlNode promoVideo = this.SelectElementWithClass(PromoVideoClasses);
                return promoVideo?.Attributes["href"] == null ?
                    string.Empty :
                    promoVideo.Attributes["href"].Value;
            }
        }
        
        private HtmlNode FindVideoProvider() {
            HtmlNode videoProvider = this.SelectElementWithClass(VideoProviderClasses);

            if (videoProvider == null) return null;
            
            if (videoProvider.ChildNodes.Count > 2) Log.Warn($"There are more video providers than expected for {this.Url}");
            
            return videoProvider;
        }

        /// <summary>
        /// Checks if streaming service is available by checking if the icon exists
        /// </summary>
        /// <param name="imageUrl">The url of the steaming service's icon</param>
        /// <param name="videoProvider">The root node to search from</param>
        /// <returns></returns>
        private bool IsServiceAvailable(string imageUrl, HtmlNode videoProvider) {
            return videoProvider != null && this.SelectByImage(imageUrl, videoProvider) != null;
        }

        protected override DataModel Scrape() {
            HtmlNode videoProvider = this.FindVideoProvider();
            bool isOnCrunchyRoll   = this.IsServiceAvailable(CrunchyRollImage, videoProvider);
            bool isOnHulu          = this.IsServiceAvailable(HuluImage, videoProvider);
            
            return new VideoModel {
                PromoVideo      = { Value = this.PromoVideo },
                IsOnCrunchyRoll = { Value = isOnCrunchyRoll.ToString()},
                IsOnHulu        = { Value = isOnHulu.ToString()},
                HasStreaming    = { Value = (isOnCrunchyRoll || isOnHulu).ToString()}
            };
        }
    }
}
