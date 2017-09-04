namespace AnimeExporter.Models {
    public class VideoModel : DataModel {
        public AttributeModel IsOnHulu        = "Available on Hulu";
        public AttributeModel IsOnCrunchyRoll = "Available on Crunchyroll";
        public AttributeModel HasStreaming    = "Available for streaming";

        public AttributeModel PromoVideo = "Promotional Video";
    }
}