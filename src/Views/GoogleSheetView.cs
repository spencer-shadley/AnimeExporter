using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using AnimeExporter.Models;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

using Table = System.Collections.Generic.IList<System.Collections.Generic.IList<object>>;

namespace AnimeExporter.Views {
    
    // TODO: Split into View and Controller
    
    public class GoogleSheetView {
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private static SheetsService _service;
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

        public static void PublishDataToGoogleSheet(AnimesModel animesModel) {
            PublishGoogleSheet(animesModel.ToDataTable(), TopAnimeSheetName);
        }

        public static void PublishGenresToGoogleSheet(AnimesModel animesModel) {
            PublishGoogleSheet(animesModel.ToCollectionsTable(), GenresSheetName);
        }

        /// <summary>
        /// Backs up the data found at "Sheet" to "Sheet (Backup)"
        /// </summary>
        /// <remarks>Useful for when a scraping goes wrong...</remarks>
        public static void BackupData() {
            BackupSheet(TopAnimeSheetName);
            BackupSheet(GenresSheetName);
        }

        private static void BackupSheet(string sheetName) {
            GetRequest getRequest = Service.Spreadsheets.Values.Get(SheetId, CalculateEntireRange(sheetName));
            ValueRange response = getRequest.Execute();
            
            PublishGoogleSheet(response.Values, sheetName + " (Backup)");
        }

        private static void ClearGoogleSheet(string sheetName) {
            var request = new ClearValuesRequest();
            ClearRequest clearRequest = Service.Spreadsheets.Values.Clear(request, SheetId, CalculateEntireRange(sheetName));
            ClearValuesResponse response = clearRequest.Execute();
            Log.Info($"Cleared {response.ClearedRange}");
        }

        /// <summary>
        /// Publishes the <see cref="table"/> to a the Google Sheet at <see cref="SheetId"/> with <param name="sheetName"></param>
        /// </summary>
        /// <param name="table">The data to publish</param>
        /// <param name="sheetName">The sheet to update</param>
        private static void PublishGoogleSheet(Table table, string sheetName) {
            BatchUpdateValuesResponse response;
            ClearGoogleSheet(sheetName);

            var updateValues = new ValueRange {
                Values = table,
                Range = CalculateEntireRange(sheetName)
            };

            var request = new BatchUpdateValuesRequest {
                Data = new[] { updateValues },
                ValueInputOption = "USER_ENTERED"
            };
            
            try {
                BatchUpdateRequest updateRequest = Service.Spreadsheets.Values.BatchUpdate(request, SheetId);
                response = updateRequest.Execute();
            }
            catch (GoogleApiException e) {
                if (e.HttpStatusCode == HttpStatusCode.RequestEntityTooLarge) {
                    // TODO: instead of truncating load, split into multiple partial update requests
                    Log.Warn($"Google Sheets quota was exceeded with {table.Count} rows. Trying again with fewer rows...", e);
                    
                    PublishGoogleSheet(TruncateTable(table, 10), sheetName);
                    return;
                }
                
                Log.Error("Unhandled Google API exception", e);
                throw;
            }

            Debug.Assert(response != null, "response != null");
            Log.Info($"Updated {sheetName}, check out Google Sheet {BaseSheetUri}{response.SpreadsheetId}" + Environment.NewLine);
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

        /// <summary>
        /// Tries to return A1 notation for the range of an entire sheet
        /// </summary>
        /// <param name="sheetName">The title of the sheet to select notation for</param>
        /// <remarks>Will fail if the sheet exceeds 26^3 columns or 1,000,000 rows</remarks>
        private static string CalculateEntireRange(string sheetName) {
            return $"'{sheetName}'!A1:ZZZ1000000";
        }

        /// <summary>
        /// Remove the last <param name="numRowsToTruncate"></param> from <param name="values"></param>
        /// </summary>
        private static Table TruncateTable(Table values, int numRowsToTruncate) {
            if (numRowsToTruncate > values.Count) {
                string errorMessage = $"Values of size {values.Count} is too small to keep truncating"; 
                Log.Error(errorMessage);
                throw new ArgumentOutOfRangeException(errorMessage);
            }
            
            for (int i = 0; i < numRowsToTruncate; ++i) {
                values.RemoveAt(values.Count);
            }
            return values;
        }
    }
}
