using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TimeAPIService : MonoBehaviour
{
    //https://api.ipgeolocation.io/timezone?apiKey=aa2e4cba4db443bbbfda8c1427234c26&ip=
    //https://timeapi.io/api/time/current/ip?ipAddress=

    [SerializeField] private TimeModel _timeModel;
    [SerializeField] private string _timeApiUrl;
    private const string _apiUrl = "https://api.ipify.org?format=json";

    private Action _gotIpAdress;
    static public Action<string, string, string> TimeReceivedCallback;

    static private Dictionary<bool, string> _updatedModels = new Dictionary<bool, string>();
    static private string _timezone, _date, _actualTime;

    private string _ipAddress;

    private void OnEnable() {
        _timeModel.DataUpdated += UpdateModelIncrease;
        _gotIpAdress += StartGetTimeUsingTimeAPI;
    }

    private void Awake()
    {
        StartCoroutine(GetPublicIPAddress());
    }

    public void FetchTime() {
        StartCoroutine(GetPublicIPAddress());
    }

    private IEnumerator GetPublicIPAddress()
    {
        UnityWebRequest request = UnityWebRequest.Get(_apiUrl + _ipAddress);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching IP address: " + request.error);
        }
        else
        {
            IPResponse response = JsonUtility.FromJson<IPResponse>(request.downloadHandler.text);
            _ipAddress = response.ip;

            _gotIpAdress?.Invoke();
        }
    }

    [Serializable]
    private class IPResponse
    {
        public string ip;
    }

    private IEnumerator GetTimeUsingTimeAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(_timeApiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            UpdateModelIncrease(false);

            Debug.LogError("Error fetching time from server" + request.error);
        }
        else
        {
            _timeModel.UpdateTime(request.downloadHandler.text);
        }
    } 

    private void StartGetTimeUsingTimeAPI() => StartCoroutine(GetTimeUsingTimeAPI());

    private void UpdateModelIncrease(bool isSuccessfully)
    {
        string time;

        if(isSuccessfully) {
            try
            {
                if (_timeModel is TimeApiModel timeApiModel){
                    time = timeApiModel.time;

                    _timezone = timeApiModel.timeZone;
                }
                else if (_timeModel is IpgeolocationAPIModel ipgeolocationAPIModel) {
                    time = ipgeolocationAPIModel.time_24;

                    _timezone = ipgeolocationAPIModel.timezone;
                }
                else
                    throw new Exception("Can't define class of a model");
            }
            catch
            {
                _updatedModels.Add(false, "");
                return;
            }

            _updatedModels.Add(isSuccessfully, time);
        }

        else
            _updatedModels.Add(false, "");        

        if (_updatedModels.Count >= 2)
        {
            _updatedModels = _updatedModels.Where(kv => kv.Key).ToDictionary(kv => kv.Key, kv => kv.Value);

            if (_updatedModels.Count == 0)
            {
                _actualTime = DateTime.Now.ToString("HH:mm");
                _timezone = TimeZoneInfo.Local.StandardName;
                _date = DateTime.Now.ToString("dd/MM/yy");
            }
            else
            {
                CalculateAverageTime();
            }

            _updatedModels = new Dictionary<bool, string>();

            TimeReceivedCallback?.Invoke(_timezone, _date, _actualTime);
        }
    }

    private void CalculateAverageTime()
    {
        var times = _updatedModels.Values
            .Where(time => !string.IsNullOrEmpty(time))
            .Select(time => ParseTimeToMinutes(time))
            .ToList();

        if (times.Count > 0)
        {
            int averageMinutes = (int)times.Average();
            string formattedTime = FormatMinutesToTime(averageMinutes);

            _actualTime = formattedTime;
        }
    }

    private int ParseTimeToMinutes(string time)
    {
        var parts = time.Split(':');
        int hours = int.Parse(parts[0]);
        int minutes = int.Parse(parts[1]);

        return hours * 60 + minutes;
    }

    private string FormatMinutesToTime(int totalMinutes)
    {
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        return $"{hours:D2}:{minutes:D2}";
    }

    private void OnDisable() {
        _timeModel.DataUpdated -= UpdateModelIncrease;
        _gotIpAdress -= StartGetTimeUsingTimeAPI;
    }
}
