using BepInEx.Logging;
using UitkForKsp2.API;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProbablyAskew.UI;

/// <summary>
/// Controller for the MyFirstWindow UI.
/// </summary>
public class AngleWindowController : MonoBehaviour
{
    
    private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("ProbablyAskew.AngleWindow");
    
    // The UIDocument component of the window game object
    private UIDocument _window;

    // The elements of the window that we need to access
    private VisualElement _rootElement;
    private VisualElement _lineParent;
    private VisualElement _line;
    private VisualElement _circle;
    private VisualElement _crossNorthSouth;
    private VisualElement _crossEastWest;

    // The backing field for the IsWindowOpen property
    private bool _isWindowOpen;

    // private VisualElement _line;
    private float _angle = 0;

    /// <summary>
    /// The state of the window. Setting this value will open or close the window.
    /// </summary>
    public bool IsWindowOpen
    {
        get => _isWindowOpen;
        set
        {
            _isWindowOpen = value;

            // Set the display style of the root element to show or hide the window
            _rootElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            
        }
    }

    /// <summary>
    /// Runs when the window is first created, and every time the window is re-enabled.
    /// </summary>
    private void OnEnable()
    {
        
        Logger.LogWarning("Angle window OnEnable");
        try
        {

            // Get the UIDocument component from the game object
            _window = GetComponent<UIDocument>();

            // Get the root element of the window.
            // Since we're cloning the UXML tree from a VisualTreeAsset, the actual root element is a TemplateContainer,
            // so we need to get the first child of the TemplateContainer to get our actual root VisualElement.
            _rootElement = _window.rootVisualElement[0];
            
            // _rootElement.style.width = new StyleLength(Length.(100));
            _rootElement.style.width = 500;
            _rootElement.style.height = 500;
            
            _lineParent = _rootElement.Q<VisualElement>("line-parent");
            _line = _rootElement.Q<VisualElement>("line");
            _circle =  _rootElement.Q<VisualElement>("circle");
            _crossNorthSouth =  _rootElement.Q<VisualElement>("north-south");
            _crossEastWest =  _rootElement.Q<VisualElement>("east-west");

            // Center the window by default
            Logger.LogWarning("centering overlay");
            _rootElement.CenterByDefault();
            
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }

    }

    public void SetAngle(float angle)
    {
        try
        {
            // Rotate the line
            // Logger.LogInfo("Setting angle to {angle}");
            
            _angle = (angle - 90) * -1;
            _lineParent.transform.rotation = Quaternion.Euler(0, 0, _angle);

        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }

    }

    public void SetSize(int size)
    {
        if (null != _rootElement)
        {
            _rootElement.style.width = size;
            _rootElement.style.height = size;
            _rootElement.CenterByDefault();
        }
        
    }

    public VisualElement getRootElement()
    {
        return _rootElement;
    }

    public void SetOpacity(float value)
    {
        _rootElement.style.opacity = new StyleFloat(value / 100);
    }

    public void SetLineColor(String unsafeColor)
    {
        Color  parsed;
        if (ColorUtility.TryParseHtmlString(unsafeColor, out parsed))
        {
            _line.style.backgroundColor = new StyleColor(parsed);
        }
    }

    public void SetCircleColor(String unsafeColor)
    {
        Color  parsed;
        if (ColorUtility.TryParseHtmlString(unsafeColor, out parsed))
        {
            _circle.style.borderTopColor = new StyleColor(parsed);
            _circle.style.borderRightColor = new StyleColor(parsed);
            _circle.style.borderBottomColor = new StyleColor(parsed);
            _circle.style.borderLeftColor = new StyleColor(parsed);
        }
    }

    public void SetCrossColor(String unsafeColor)
    {
        Color  parsed;
        if (ColorUtility.TryParseHtmlString(unsafeColor, out parsed))
        {
            _crossNorthSouth.style.backgroundColor = new StyleColor(parsed);
            _crossEastWest.style.backgroundColor = new StyleColor(parsed);
        }
    }


}
