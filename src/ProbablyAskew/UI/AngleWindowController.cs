using BepInEx.Logging;
using KSP.UI.Binding;
using ProbablyAskew.Unity.Runtime;
using SpaceWarp.API.Assets;
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

    // The backing field for the IsWindowOpen property
    private bool _isWindowOpen;

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
            // Alternatively, you can deactivate the window game object to close the window and stop it from updating,
            // which is useful if you perform expensive operations in the window update loop. However, this will also
            // mean you will have to re-register any event handlers on the window elements when re-enabled in OnEnable.
            // gameObject.SetActive(value);

            // Update the Flight AppBar button state
            // GameObject.Find(ProbablyAskewPlugin.ToolbarFlightButtonID)
            //     ?.GetComponent<UIValue_WriteBool_Toggle>()
            //     ?.SetValue(value);
            //
            // // Update the OAB AppBar button state
            // GameObject.Find(ProbablyAskewPlugin.ToolbarOabButtonID)
            //     ?.GetComponent<UIValue_WriteBool_Toggle>()
            //     ?.SetValue(value);
        }
    }

    /// <summary>
    /// Runs when the window is first created, and every time the window is re-enabled.
    /// </summary>
    private void OnEnable()
    {
        
        Logger.LogWarning("Angle window OnEnable");
        
        // Get the UIDocument component from the game object
        _window = GetComponent<UIDocument>();

        // Get the root element of the window.
        // Since we're cloning the UXML tree from a VisualTreeAsset, the actual root element is a TemplateContainer,
        // so we need to get the first child of the TemplateContainer to get our actual root VisualElement.
        _rootElement = _window.rootVisualElement[0];

        // Center the window by default
        _rootElement.CenterByDefault();

    }

}
