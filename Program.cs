using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            const int numPages = 100;

            // Add the schema as its own "anime" so that we get nice titling in our Google Sheet
            Animes topAnimes = new Animes {Anime.Schema()}; 
            for (var i = 0; i < numPages; ++i) {
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine($"==========   PAGE {i} ==========");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                topAnimes.Add(TopAnimePage.GetTopAnimes(i));
            }
            
//            Animes topAnime = TopAnimePage.GetTopAnimes(0);
            GoogleSheetsRunner(topAnimes.ToTable());
        }

        // TODO: this should be moved out to another file
        public static void GoogleSheetsRunner(List<IList<object>> table) {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = SetupCredentials(),
                ApplicationName = ApplicationName,
            });
            
            const string sheetId = "17KQKFy9o1pPG0Yko2dTYZcRhNSTdNWyI3NLWsJyfqbI";
            const string updateRange = "A1";

            ValueRange valueRange = new ValueRange {
                Range = updateRange,
                Values = table
            };

            service.Spreadsheets.Values.Update(valueRange, sheetId, updateRange);
            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest =
                service.Spreadsheets.Values.Update(valueRange, sheetId, updateRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            UpdateValuesResponse response = updateRequest.Execute();

            const string baseSheetUri = "https://docs.google.com/spreadsheets/d/"; 
            Console.WriteLine($"All done, check out Google Sheet {baseSheetUri}{response.SpreadsheetId}");
        }

        // TODO: this should be moved out to another file
        private static UserCredential SetupCredentials() {

            UserCredential credential;
            
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(GetCredentialsPath(), true)).Result;
            }
            Debug.Assert(credential != null);
            return credential;
        }

        // TODO: This should moved out to another file
        private static string GetCredentialsPath() {
            string baseFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            const string exporterPath = ".credentials/sheets.googleapis.com-anime-exporter.json";
            return Path.Combine(baseFolderPath, exporterPath);
        }
    }
}