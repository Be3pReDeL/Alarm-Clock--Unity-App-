using System;
using UnityEngine;

[Serializable]
public class TimeApiModel : TimeModel
{
    public string timeZone { get; private set; }
    public string date { get; private set; }
    public string time { get; private set; }

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
