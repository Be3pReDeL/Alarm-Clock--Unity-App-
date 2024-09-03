using System;
using UnityEngine;

public class IpgeolocationAPIModel : TimeModelBase
{
    override public TimeAPIService.TimeInfo ParseTime(string jsonResponse)
    {
        var data = JsonUtility.FromJson<IpgeolocationAPIModelData>(jsonResponse);

        string time = data.time_24;
        string date = data.date;

        return new TimeAPIService.TimeInfo
        {
            Timezone = data.timezone,
            Time = time,
            Date = date
        };
    }
}

[Serializable]
public class IpgeolocationAPIModelData
{
    public string time_24;
    public string timezone;
    public string date;
}
