using UnityEngine;
using UnityEngine.UIElements;

public class OrientationLayoutUpdater : IUILayoutUpdater
{
    public void UpdateUILayout(VisualElement element)
    {
        if (Screen.width > Screen.height)
        {
            element.RemoveFromClassList("portrait");
            element.AddToClassList("landscape");
        }
        else
        {
            element.RemoveFromClassList("landscape");
            element.AddToClassList("portrait");
        }
    }
}
