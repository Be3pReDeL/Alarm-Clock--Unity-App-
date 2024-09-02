using System;
using UnityEngine;

public class TimeModel : MonoBehaviour
{
    public Action<bool> DataUpdated;

    protected void NotifyOnUpdate(bool isSuccessfully)
    {
        DataUpdated?.Invoke(isSuccessfully);
    }

    virtual public void UpdateTime(string jsonResponse)
    {
        NotifyOnUpdate(true);
    }
}
