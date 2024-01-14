using BepInEx.Logging;
using KSP.UI.Binding;
using SpaceWarp.API.Assets;
using UitkForKsp2.API;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProbablyAskew.UI;

/// <summary>
/// Controller for the MyFirstWindow UI.
/// </summary>
public class MyFirstWindowController : MonoBehaviour
{
    private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("ProbablyAskew.Window");


    // The UIDocument component of the window game object
    private UIDocument _window;

    // The elements of the window that we need to access
    private VisualElement _rootElement;
    private TextField _angleTextfield;
    private Toggle _showToggle;

    // The backing field for the IsWindowOpen property
    private bool _isWindowOpen;

    private AngleWindowController _angleWindowController;

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
            GameObject.Find(ProbablyAskewPlugin.ToolbarFlightButtonID)
                ?.GetComponent<UIValue_WriteBool_Toggle>()
                ?.SetValue(value);

            // Update the OAB AppBar button state
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
        try
        {
            // Get the UIDocument component from the game object
            _window = GetComponent<UIDocument>();

            // Get the root element of the window.
            // Since we're cloning the UXML tree from a VisualTreeAsset, the actual root element is a TemplateContainer,
            // so we need to get the first child of the TemplateContainer to get our actual root VisualElement.
            _rootElement = _window.rootVisualElement[0];
            
            // _rootElement.style.width = new StyleLength(Length.(100));
            

            createAngleWindow();

            // Get the text field from the window
            _angleTextfield = _rootElement.Q<TextField>("angle-textfield");

            _angleTextfield.RegisterValueChangedCallback(evt =>
            {
                float angle;
                var parsed = float.TryParse(evt.newValue, out angle);
                if (parsed)
                {
                    // Logger.LogWarning("parsed {parsed} float, changing");
                    _angleWindowController.SetAngle(angle);
                }

                // Your logic here
            });


            {
            }


            // Get the toggle from the window
            _showToggle = _rootElement.Q<Toggle>("show-toggle");
            _showToggle.value = true;


            _showToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                Logger.LogWarning($"Toggle Clicked, new value: {evt.newValue}");
                try
                {
                    _angleWindowController.IsWindowOpen = evt.newValue;
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            });

            Logger.LogInfo("Centering...");
            // Center the window by default
            _rootElement.CenterByDefault();

            // Get the close button from the window
            var closeButton = _rootElement.Q<Button>("close-button");
            // Add a click event handler to the close button
            closeButton.clicked += () => IsWindowOpen = false;
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }
    }

    private void createAngleWindow()
    {
        // Load the UI from the asset bundle
        var angleWindowUxml = AssetManager.GetAsset<VisualTreeAsset>(
            // The case-insensitive path to the asset in the bundle is composed of:
            // - The mod GUID:
            $"{MyPluginInfo.PLUGIN_GUID}/" +
            // - The name of the asset bundle:
            "ProbablyAskew_ui/" +
            // - The path to the asset in your Unity project (without the "Assets/" part)
            "ui/myfirstwindow/AngleWindow.uxml"
        );


        var windowOptions = new WindowOptions
        {
            // The ID of the window. It should be unique to your mod.
            WindowId = "ProbablyAskew_AngleWindow",
            // The transform of parent game object of the window.
            // If null, it will be created under the main canvas.
            Parent = null,
            // Whether or not the window can be hidden with F2.
            IsHidingEnabled = true,
            // Whether to disable game input when typing into text fields.
            DisableGameInputForTextFields = true,
            MoveOptions = new MoveOptions
            {
                // Whether or not the window can be moved by dragging.
                IsMovingEnabled = false,
                // Whether or not the window can only be moved within the screen bounds.
                CheckScreenBounds = false
            }
        };

        var angleWindow = Window.Create(windowOptions, angleWindowUxml);

        // angleWindow.RegisterCallback<MouseDownEvent>(e => e.StopImmediatePropagation(), TrickleDown.TrickleDown);


        // Add a controller for the UI to the window's game object
        _angleWindowController = angleWindow.gameObject.AddComponent<AngleWindowController>();
        _angleWindowController.SetAngle(0);
    }
}