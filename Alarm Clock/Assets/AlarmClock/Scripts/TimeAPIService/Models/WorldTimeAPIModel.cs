using System;
using UnityEngine;


public class WorldTimeAPIModel : TimeModelBase
{
    override public TimeAPIService.TimeInfo ParseTime(string jsonResponse)
    {
        var data = JsonUtility.FromJson<WorldTimeAPIModelData>(jsonResponse);

        DateTime dateTime = DateTime.Parse(data.datetime);
        string time = dateTime.ToString("HH:mm:ss");
        string date = dateTime.ToString("dd/MM/yy");

        return new TimeAPIService.TimeInfo
        {
            Timezone = data.timezone,
            Time = time,
            Date = date
        };
    }
}

[Serializable]
public class WorldTimeAPIModelData
{
    public string datetime;
    public string timezone;
}
