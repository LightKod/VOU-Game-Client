using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows;

namespace Owlet
{
    public static class GoogleSheetReader 
    {
        private static readonly char[] TRIM_CHARS = { '\"', '\r' };
        private const string SPLIT_RE = ",";
        private const string LINE_SPLIT_RE = @"\n|\r";
        private const string comma = "\\n";
        // List of lists to store the data
        public static async UniTask<GoogleSheetData> FetchData(string sheet, string gid)
        {
            string url = $"https://docs.google.com/spreadsheets/d/{sheet}/export?format=csv&id={sheet}&gid={gid}";

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                await www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    return null;
                }
                else
                {
                    // Parse JSON response
                    string jsonResult = www.downloadHandler.text;
                    GoogleSheetData data = new();
                    data.values = ReadNew(jsonResult);
                    data.headers = data.values[0];
                    data.values.RemoveAt(0);
                    return data;
                }
            }
        }

        public static List<List<string>> ReadNew(string data)
        {
            List<List<string>> rows = new List<List<string>>();

            string[] lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                List<string> row = new List<string>();
                StringBuilder field = new StringBuilder();

                bool insideQuotes = false;

                foreach (char c in line)
                {
                    if (c == '"')
                    {
                        insideQuotes = !insideQuotes; // Toggle inside/outside quotes
                    }
                    else if (c == ',' && !insideQuotes)
                    {
                        row.Add(field.ToString().Trim());
                        field.Clear();
                    }
                    else
                    {
                        field.Append(c);
                    }
                }

                row.Add(field.ToString().Trim());
                rows.Add(row);
            }

            return rows;
        }
    }

    [Serializable]
    public class GoogleSheetData
    {
        public List<string> headers;
        public List<List<string>> values;
    }
}
