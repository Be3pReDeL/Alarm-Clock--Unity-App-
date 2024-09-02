using UnityEngine.UIElements;

public interface IUIElementFinder
{
    VisualElement FindTargetElement(VisualElement root, string elementName, string elementClass);
}
