namespace AnimeExporter.Models {
    public class StatsModel : DataModel {

        // TODO: implement the below stats (https://myanimelist.net/anime/1794/Dirty_Pair_no_Ooshoubu__Nolandia_no_Nazo/stats)

        public AttributeModel Watching = "Number of people currently watching";
        public AttributeModel Completed = "Number of people completed watching";
        public AttributeModel OnHold = "Number of people on hold";
        public AttributeModel Dropped = "Number of people dropped";
        public AttributeModel PlanToWatch = "Number of people planning to watch";
        public AttributeModel Total = "Number of people interested (watching, on-hold, dropped, etc.)";

        public AttributeModel NumberTotalVotes  = "Number of votes";
        public AttributeModel NumberScoreTen    = "Number of users scored 10";
        public AttributeModel NumberScoreNine   = "Number of users scored 9";
        public AttributeModel NumberScoreEight  = "Number of users scored 8";
        public AttributeModel NumberScoreSeven  = "Number of users scored 7";
        public AttributeModel NumberScoreSix    = "Number of users scored 6";
        public AttributeModel NumberScoreFive   = "Number of users scored 5";
        public AttributeModel NumberScoreFour   = "Number of users scored 4";
        public AttributeModel NumberScoreThree  = "Number of users scored 3";
        public AttributeModel NumberScoreTwo    = "Number of users scored 2";
        public AttributeModel NumberScoreOne    = "Number of users scored 1";

        public AttributeModel PercentScoreTen   = "Percent of users scored 10";
        public AttributeModel PercentScoreNine  = "Percent of users scored 9";
        public AttributeModel PercentScoreEight = "Percent of users scored 8";
        public AttributeModel PercentScoreSeven = "Percent of users scored 7";
        public AttributeModel PercentScoreSix   = "Percent of users scored 6";
        public AttributeModel PercentScoreFive  = "Percent of users scored 5";
        public AttributeModel PercentScoreFour  = "Percent of users scored 4";
        public AttributeModel PercentScoreThree = "Percent of users scored 3";
        public AttributeModel PercentScoreTwo   = "Percent of users scored 2";
        public AttributeModel PercentScoreOne   = "Percent of users scored 1";
    }
}
