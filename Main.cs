using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
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
//            List<Anime> topAnime = HtmlParser.GetTopAnime(1);
//            Console.WriteLine(string.Join(Environment.NewLine, topAnime));

            GoogleSheetsRunner();
        }

        public static void GoogleSheetsRunner() {
            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = SetupCredentials(),
                ApplicationName = ApplicationName,
            });
            
            const string sheetId = "17KQKFy9o1pPG0Yko2dTYZcRhNSTdNWyI3NLWsJyfqbI";
            const string updateRange = "A1";

            var updateValues = new List<IList<object>>();

            var someRow = new List<object> {"pizza?"};
            updateValues.Add(someRow);
            
            ValueRange valueRange = new ValueRange {
                Range = updateRange,
                Values = updateValues
            };

            service.Spreadsheets.Values.Update(valueRange, sheetId, updateRange);
            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest =
                service.Spreadsheets.Values.Update(valueRange, sheetId, updateRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            updateRequest.Execute();
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
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            return credential;
        }
    }
}