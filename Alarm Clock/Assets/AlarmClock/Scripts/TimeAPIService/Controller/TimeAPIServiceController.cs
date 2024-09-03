using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class TimeAPIServiceController : MonoBehaviour
{
    [SerializeField] private List<TimeAPIService> _services; // Список сервисов

    private async void Start()
    {
        TimeAPIService.TimeInfo timeInfo = await FetchTimeFromServicesAsync();
        Debug.Log($"Timezone: {timeInfo.Timezone}, Time: {timeInfo.Time}, Date: {timeInfo.Date}");
    }

    public async Task<TimeAPIService.TimeInfo> FetchTimeFromServicesAsync()
    {
        var tasks = _services.Select(service => service.FetchTimeAsync()).ToList();

        var results = await Task.WhenAll(tasks);

        var successfulResults = results.Where(r => !string.IsNullOrEmpty(r.Time)).ToList();

        if (successfulResults.Count == 0)
        {
            // Нет успешных ответов, используем системное время
            return new TimeAPIService.TimeInfo
            {
                Timezone = TimeZoneInfo.Local.StandardName,
                Time = DateTime.Now.ToString("HH:mm:ss"),
                Date = DateTime.Now.ToString("dd/MM/yy")
            };
        }
        else if (successfulResults.Count == 1)
        {
            // Есть только один успешный ответ
            return successfulResults.First();
        }
        else
        {
            // Если несколько сервисов вернули время, рассчитываем среднее время
            return CalculateAverageTime(successfulResults);
        }
    }

    private TimeAPIService.TimeInfo CalculateAverageTime(List<TimeAPIService.TimeInfo> timeInfos)
    {
        var timesInSeconds = timeInfos
            .Select(t => TimeAPIService.ParseTimeToSeconds(t.Time))
            .ToList();

        int averageSeconds = (int)timesInSeconds.Average();
        string averageTime = TimeAPIService.FormatSecondsToTime(averageSeconds);

        return new TimeAPIService.TimeInfo
        {
            Timezone = timeInfos.First().Timezone, // Берем первый успешный таймзон
            Time = averageTime,
            Date = timeInfos.First().Date // Предполагаем, что дата одинаковая для всех
        };
    }
}
