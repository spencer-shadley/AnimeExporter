using System;
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
        private const string BaseSheetUri = "https://docs.google.com/spreadsheets/d/";
        private const string SheetId = "17KQKFy9o1pPG0Yko2dTYZcRhNSTdNWyI3NLWsJyfqbI";

        private static string CredentialsPath {
            get {
                string baseFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                const string exporterPath = ".credentials/sheets.googleapis.com-anime-exporter.json";
                return Path.Combine(baseFolderPath, exporterPath);
            }
        }
        
        /// <summary>
        /// Publishes the <see cref="animes"/> to a the Google Sheet at <see cref="SheetId"/>
        /// </summary>
        /// <param name="animes">The <see cref="Animes"/> to publish</param>
        public static void PublishGoogleSheet(Animes animes) {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = SetupCredentials(),
                ApplicationName = ApplicationName,
            });
            
            const string updateRange = "A1";

            ValueRange valueRange = new ValueRange {
                Range = updateRange,
                Values = animes.ToTable()
            };

            service.Spreadsheets.Values.Update(valueRange, SheetId, updateRange);
            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest =
                service.Spreadsheets.Values.Update(valueRange, SheetId, updateRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            UpdateValuesResponse response = updateRequest.Execute();

            Console.WriteLine($"All done, check out Google Sheet {BaseSheetUri}{response.SpreadsheetId}");
        }

        /// <summary>The Google Sheet which is published to requires credentials to access.</summary>
        /// <remarks>
        /// To publish to your own Google Sheet, update the <see cref="SheetId"/> in <see cref="PublishGoogleSheet"/>
        /// and add your own client_secret.json. See https://developers.google.com/sheets/api/guides/authorizing
        /// for more details
        /// </remarks>
        /// <returns>The UserCredentials based on client_secret.json for accessing the sheet at <see cref="SheetId"/></returns>
        private static UserCredential SetupCredentials() {

            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(CredentialsPath, true)).Result;
            }
            Debug.Assert(credential != null);
            return credential;
        }
    }
}
