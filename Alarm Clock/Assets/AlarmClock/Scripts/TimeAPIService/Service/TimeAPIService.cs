using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class TimeAPIService : MonoBehaviour
{
    [SerializeField] private TimeModelBase _timeModel;
    [SerializeField] private string _timeApiUrl;

    public struct TimeInfo
    {
        public string Timezone;
        public string Time;
        public string Date;
    }

    public async Task<TimeInfo> FetchTimeAsync()
    {
        if (_timeModel == null)
        {
            Debug.LogError("Time model is not assigned!");
            return new TimeInfo();
        }

        if (string.IsNullOrEmpty(_timeApiUrl))
        {
            Debug.LogError("Time API URL is not assigned!");
            return new TimeInfo();
        }

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(_timeApiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return _timeModel.ParseTime(jsonResponse);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error fetching time from server: " + ex.Message);
                return new TimeInfo(); // Возвращаем пустую структуру в случае ошибки
            }
        }
    }

    public static int ParseTimeToSeconds(string time)
    {
        var parts = time.Split(':');
        int hours = int.Parse(parts[0]);
        int minutes = int.Parse(parts[1]);
        int seconds = int.Parse(parts[2]);

        return hours * 3600 + minutes * 60 + seconds;
    }

    public static string FormatSecondsToTime(int totalSeconds)
    {
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}
