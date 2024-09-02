using System;
using UnityEngine;

[Serializable]
public class IpgeolocationAPIModel : TimeModel
{
    public string timezone { get; private set; }
    public string date { get; private set; }
    public string time_24 { get; private set; }

    override public void UpdateTime(string jsonResponse)
    {
        try {
            JsonUtility.FromJsonOverwrite(jsonResponse, this);
        }
        catch {
            NotifyOnUpdate(false);

            return;
        }

        base.UpdateTime(jsonResponse);
    }
}
