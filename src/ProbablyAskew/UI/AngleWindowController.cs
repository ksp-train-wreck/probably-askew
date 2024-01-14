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
            
            Logger.LogWarning("centering during set angle");
            _rootElement.CenterByDefault();
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }

    }


}
