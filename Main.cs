using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AnimeExporter.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace AnimeExporter {
    internal class Program {
        private static readonly string[] Scopes = {SheetsService.Scope.Spreadsheets};
        private const string ApplicationName = "Google Sheets API";

        public static void Main(string[] args) {
            List<Anime> topAnime = HtmlParser.GetTopAnime(1);
            Console.WriteLine(string.Join(Environment.NewLine, topAnime));

            List<IList<object>> table = GenerateTable(topAnime);
            GoogleSheetsRunner(table);
        }

        public static List<IList<object>> GenerateTable(List<Anime> animes) {
            return animes.Select(anime => anime.Data).Cast<IList<object>>().ToList();
        }

        public static void GoogleSheetsRunner(List<IList<object>> table) {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = SetupCredentials(),
                ApplicationName = ApplicationName,
            });
            
            const string sheetId = "17KQKFy9o1pPG0Yko2dTYZcRhNSTdNWyI3NLWsJyfqbI";
            const string updateRange = "A1";

            List<IList<object>> updateValues = table;
            
            ValueRange valueRange = new ValueRange {
                Range = updateRange,
                Values = updateValues
            };

            service.Spreadsheets.Values.Update(valueRange, sheetId, updateRange);
            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest =
                service.Spreadsheets.Values.Update(valueRange, sheetId, updateRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            UpdateValuesResponse response = updateRequest.Execute();

            const string baseSheetUri = "https://docs.google.com/spreadsheets/d/"; 
            Console.WriteLine($"All done, check Google Sheet {baseSheetUri}{response.SpreadsheetId}");
        }

        private static UserCredential SetupCredentials() {
            UserCredential credential;
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-anime-exporter.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            return credential;
        }
    }
}