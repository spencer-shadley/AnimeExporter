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
        
        private static SheetsService _service = null;
        private static readonly string[] Scopes = {SheetsService.Scope.Spreadsheets};
        private const string ApplicationName = "Google Sheets API";
        private const string BaseSheetUri = "https://docs.google.com/spreadsheets/d/";
        private const string SheetId = "17KQKFy9o1pPG0Yko2dTYZcRhNSTdNWyI3NLWsJyfqbI";
        private const string TopAnimeSheetName = "Top Anime";
        private const string GenresSheetName = "Genres";
        
        private static string CredentialsPath {
            get {
                string baseFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                const string exporterPath = ".credentials/sheets.googleapis.com-anime-exporter.json";
                return Path.Combine(baseFolderPath, exporterPath);
            }
        }

        private static SheetsService Service {
            get {
                _service = _service ?? new SheetsService(new BaseClientService.Initializer() {
                    HttpClientInitializer = SetupCredentials(),
                    ApplicationName = ApplicationName,
                });
                return _service;
            }
        }

        public static void PublishDataToGoogleSheet(Animes animes) {
            PublishGoogleSheet(animes.ToDataTable(), TopAnimeSheetName);
        }

        public static void PublishGenresToGoogleSheet(Animes animes) {
            PublishGoogleSheet(animes.ToCollectionsTable(), GenresSheetName);
        }

        public static void BackupData() {
            BackupSheet(TopAnimeSheetName);
            BackupSheet(GenresSheetName);
        }

        private static void BackupSheet(string sheetName) {
            SpreadsheetsResource.ValuesResource.GetRequest getRequest = Service.Spreadsheets.Values.Get(SheetId, GetEntireRangeOfSheet(sheetName));
            ValueRange response = getRequest.Execute();
            
            PublishGoogleSheet(response.Values, sheetName + " (Backup)");
        }

        private static void ClearGoogleSheet(string sheetName) {
            ClearValuesRequest request = new ClearValuesRequest();
            SpreadsheetsResource.ValuesResource.ClearRequest clearRequest = Service.Spreadsheets.Values.Clear(request, SheetId, GetEntireRangeOfSheet(sheetName));
            ClearValuesResponse response = clearRequest.Execute();
            Console.WriteLine($"Cleared {response.ClearedRange}");
        }

        /// <summary>
        /// Publishes the <see cref="values"/> to a the Google Sheet at <see cref="SheetId"/>
        /// </summary>
        /// <param name="values">The data to publish</param>
        /// <param name="sheetName">The sheet to update</param>
        private static void PublishGoogleSheet(IList<IList<object>> values, string sheetName) {
            ClearGoogleSheet(sheetName);

            ValueRange updateValues = new ValueRange {
                Values = values,
                Range = GetEntireRangeOfSheet(sheetName)
            };
            
            BatchUpdateValuesRequest request = new BatchUpdateValuesRequest {
                Data = new[] {updateValues},
                ValueInputOption = "USER_ENTERED"
            };
            SpreadsheetsResource.ValuesResource.BatchUpdateRequest updateRequest = Service.Spreadsheets.Values.BatchUpdate(request, SheetId);
            BatchUpdateValuesResponse response = updateRequest.Execute();

            Console.WriteLine($"Updated {sheetName}, check out Google Sheet {BaseSheetUri}{response.SpreadsheetId}" + Environment.NewLine);
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

        private static string GetRangeToUpdate(string sheetName) {
            return $"'{sheetName}'!A1";
        }

        /// <summary>
        /// Tries to return A1 notation for the range of an entire sheet
        /// </summary>
        /// <param name="sheetName">The title of the sheet to select notation for</param>
        /// <remarks>Will fail if the sheet exceeds 26^3 columns or 1,000,000 rows</remarks>
        private static string GetEntireRangeOfSheet(string sheetName) {
            return $"'{sheetName}'!A1:ZZZ1000000";
        }
    }
}
