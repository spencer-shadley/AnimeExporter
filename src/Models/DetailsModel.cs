using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnimeExporter.Models {
    
    public class DetailsModel : DataModel {
        
        // TODO: External Links
        
        public AttributeModel Title                = "Title";
        public AttributeModel Url                  = "URL";
        public AttributeModel EnglishTitle         = "English Title";
        public AttributeModel JapaneseTitle        = "Japanese Title";
        public AttributeModel Synonyms             = "Synonyms";
        public AttributeModel Score                = "Score";
        public AttributeModel NumRatings           = "Number of ratings";
        public AttributeModel Rank                 = "Rank";
        public AttributeModel Popularity           = "Popularity";
        public AttributeModel NumMembers           = "Number of Members";
        public AttributeModel NumFavorites         = "Number of Favorites";
        public AttributeModel MediaType            = "Type of Media";
        public AttributeModel NumEpisodes          = "Number of Episodes";
        public AttributeModel Status               = "Status";
        public AttributeModel DateStarted          = "Date Started Airing";
        public AttributeModel DateFinished         = "Date Finished Airing";
        public AttributeModel Producers            = "Producers";
        public AttributeModel Licensors            = "Licensors";
        public AttributeModel Studios              = "Studios";
        public AttributeModel Genres               = "Genres";
        public AttributeModel Duration             = "Duration";
        public AttributeModel Rating               = "Rating";
        public AttributeModel Source               = "Source";
        public AttributeModel Synopsis             = "Synopsis";
        public AttributeModel Background           = "Background";
        public AttributeModel Image                = "Image";
        public AttributeModel Adaptation           = "Adaptation";
        public AttributeModel AlternativeSetting   = "Alternative Setting";
        public AttributeModel Prequel              = "Prequel";
        public AttributeModel Sequel               = "Sequel";
        public AttributeModel Summary              = "Summary";
        public AttributeModel ParentStory          = "Parent story";
        public AttributeModel FullStory            = "Full story";
        public AttributeModel SideStory            = "Side story";
        public AttributeModel SpinOff              = "Spin-off";
        public AttributeModel Character            = "Character";
        public AttributeModel Other                = "Other";
        public AttributeModel AlternativeVersion   = "Alternative Version";
    }
}
