using UnityEngine;

public abstract class TimeModelBase : MonoBehaviour
{
    public abstract TimeAPIService.TimeInfo ParseTime(string jsonResponse);
}
