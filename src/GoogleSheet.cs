using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace AnimeExporter {
    public class GoogleSheet {
        
        private static readonly string[] Scopes = {SheetsService.Scope.Spreadsheets};
        private const string ApplicationName = "Google Sheets API";

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

        /// <summary>The Google Sheet which is published to requires credentials to access.</summary>
        /// <remarks>
        /// To publish to your own Google Sheet, update the sheetId in <see cref="GoogleSheetsRunner"/>
        /// and add your own client_secret.json. See https://developers.google.com/sheets/api/guides/authorizing
        /// for more details
        /// </remarks>
        /// <returns>The UserCredentials based on client_secret.json for accessing the sheet at sheetId</returns>
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

        private static string GetCredentialsPath() {
            string baseFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            const string exporterPath = ".credentials/sheets.googleapis.com-anime-exporter.json";
            return Path.Combine(baseFolderPath, exporterPath);
        }
    }
}
