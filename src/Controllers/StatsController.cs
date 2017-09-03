using System;
using System.Globalization;
using AnimeExporter.Models;
using AnimeExporter.Utility;
using HtmlAgilityPack;

namespace AnimeExporter.Controllers {
    public class StatsController : Page {
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int _totalVotes;
        
        public StatsController(string url) : base(url + "/stats") { }

        private HtmlNodeCollection FindNumVotesNodes() {
            HtmlNodeCollection voteNodes = this.SelectElementsByTypeContainsText("small", " votes)");
            
            if (voteNodes.Count == 10) return voteNodes;
            
            Log.Error($"[StatsController.FindNumVotesNodes] found {voteNodes.Count} nodes");
            return null;
        }

        private string FindNumVotes(HtmlNodeCollection voteNodes, int score) {
            if (voteNodes == null) {
                return "N/A";
            }
            string text = voteNodes[10 - score].InnerText; 
            string votes = text.Substring(1, text.IndexOf(" ", StringComparison.CurrentCulture))
                .Replace(",", string.Empty);
            
            int numVotes; int.TryParse(votes, out numVotes);
            this._totalVotes += numVotes;

            return votes;
        }

        private string CalculatePercentOfTotal(int numVotes) {
            return (numVotes * 1.0 / this._totalVotes * 100).ToString(CultureInfo.CurrentCulture);
        }
        
        protected override DataModel Scrape() {
            HtmlNodeCollection voteNodes = this.FindNumVotesNodes();

            int numVotesTen;   int.TryParse(this.FindNumVotes(voteNodes, 10), out numVotesTen);
            int numVotesNine;  int.TryParse(this.FindNumVotes(voteNodes, 9),  out numVotesNine);
            int numVotesEight; int.TryParse(this.FindNumVotes(voteNodes, 8),  out numVotesEight);
            int numVotesSeven; int.TryParse(this.FindNumVotes(voteNodes, 7),  out numVotesSeven);
            int numVotesSix;   int.TryParse(this.FindNumVotes(voteNodes, 6),  out numVotesSix);
            int numVotesFive;  int.TryParse(this.FindNumVotes(voteNodes, 5),  out numVotesFive);
            int numVotesFour;  int.TryParse(this.FindNumVotes(voteNodes, 4),  out numVotesFour);
            int numVotesThree; int.TryParse(this.FindNumVotes(voteNodes, 3),  out numVotesThree);
            int numVotesTwo;   int.TryParse(this.FindNumVotes(voteNodes, 2),  out numVotesTwo);
            int numVotesOne;   int.TryParse(this.FindNumVotes(voteNodes, 1),  out numVotesOne);
            
            return new StatsModel {
                Watching         = { Value = this.SelectValueAfterText("Watching:", true)},
                Completed        = { Value = this.SelectValueAfterText("Completed:", true)},
                OnHold           = { Value = this.SelectValueAfterText("On-Hold:", true)},
                Dropped          = { Value = this.SelectValueAfterText("Dropped:", true)},
                PlanToWatch      = { Value = this.SelectValueAfterText("Plan to Watch:", true)},
                Total            = { Value = this.SelectValueAfterText("Total:", true)},
                
                NumberScoreTen   = { Value = numVotesTen.ToString()},
                NumberScoreNine  = { Value = numVotesNine.ToString()},
                NumberScoreEight = { Value = numVotesEight.ToString()},
                NumberScoreSeven = { Value = numVotesSeven.ToString()},
                NumberScoreSix   = { Value = numVotesSix.ToString()},
                NumberScoreFive  = { Value = numVotesFive.ToString()},
                NumberScoreFour  = { Value = numVotesFour.ToString()},
                NumberScoreThree = { Value = numVotesThree.ToString()},
                NumberScoreTwo   = { Value = numVotesTwo.ToString()},
                NumberScoreOne   = { Value = numVotesOne.ToString()},
                NumberTotalVotes = { Value = this._totalVotes.ToString()},
                
                PercentScoreTen   = { Value = this.CalculatePercentOfTotal(numVotesTen)},
                PercentScoreNine  = { Value = this.CalculatePercentOfTotal(numVotesNine)},
                PercentScoreEight = { Value = this.CalculatePercentOfTotal(numVotesEight)},
                PercentScoreSeven = { Value = this.CalculatePercentOfTotal(numVotesSeven)},
                PercentScoreSix   = { Value = this.CalculatePercentOfTotal(numVotesSix)},
                PercentScoreFive  = { Value = this.CalculatePercentOfTotal(numVotesFive)},
                PercentScoreFour  = { Value = this.CalculatePercentOfTotal(numVotesFour)},
                PercentScoreThree = { Value = this.CalculatePercentOfTotal(numVotesThree)},
                PercentScoreTwo   = { Value = this.CalculatePercentOfTotal(numVotesTwo)},
                PercentScoreOne   = { Value = this.CalculatePercentOfTotal(numVotesOne)}
            };
        }
    }
}
