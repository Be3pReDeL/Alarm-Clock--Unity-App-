using System;
using System.Threading.Tasks;
using UnityEngine;

public class ClockPresenter : MonoBehaviour
{
    [SerializeField] private TimeAPIService _timeAPIService, _timeAPIService2;

    private async void Start()
    {
        try
        {
            TimeAPIService.TimeInfo timeInfo = await _timeAPIService.FetchTimeAsync();
            TimeAPIService.TimeInfo timeInfo2 = await _timeAPIService2.FetchTimeAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error fetching time: " + ex.Message);
        }
    }

    private void DisplayTimeInfo(TimeAPIService.TimeInfo timeInfo)
    {
        Debug.Log($"Time: {timeInfo.Time}");
        Debug.Log($"Timezone: {timeInfo.Timezone}");
        Debug.Log($"Date: {timeInfo.Date}");
    }
}
