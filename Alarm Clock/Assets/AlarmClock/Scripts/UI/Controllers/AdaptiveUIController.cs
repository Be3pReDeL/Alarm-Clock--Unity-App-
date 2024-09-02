using UnityEngine;
using UnityEngine.UIElements;

public class AdaptiveUIController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private string _elementName = "";
    [SerializeField] private string _elementClass = "";

    private VisualElement _targetElement;
    private IUILayoutUpdater _layoutUpdater;
    private IUIElementFinder _elementFinder;

    private void Awake()
    {
        _layoutUpdater = new OrientationLayoutUpdater();
        _elementFinder = new UIElementFinder();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        _targetElement = _elementFinder.FindTargetElement(root, _elementName, _elementClass);

        Debug.Log(_targetElement);

        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    private void Update()
    {
        UpdateUILayout();
    }

    private void UpdateUILayout()
    {
        if (_targetElement != null)
        {
            _layoutUpdater.UpdateUILayout(_targetElement);
        }
    }

    public void SetLayoutAdapter(IUILayoutUpdater customUpdater)
    {
        _layoutUpdater = customUpdater;
    }

    public void SetElementFinder(IUIElementFinder customFinder)
    {
        _elementFinder = customFinder;
    }
}
