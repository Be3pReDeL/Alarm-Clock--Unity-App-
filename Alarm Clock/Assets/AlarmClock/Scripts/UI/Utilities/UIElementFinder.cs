using UnityEngine.UIElements;

public class UIElementFinder : IUIElementFinder
{
    public VisualElement FindTargetElement(VisualElement root, string elementName, string elementClass)
    {
        if (!string.IsNullOrEmpty(elementName))
        {
            return root.Q<VisualElement>(elementName);
        }

        if (!string.IsNullOrEmpty(elementClass))
        {
            return root.Q<VisualElement>(className: elementClass);
        }

        return root;
    }
}
