using Player;
using UnityEngine;
using UnityEngine.UI;

public class VehicleButton : MonoBehaviour
{
    public int vehicleIndex = 0;
    public MeshRenderer previewMeshRenderer;
    public Text buttonText;
    public Text previewPlaceholder;
    public Text previewText;
    public WaitForAll waitForAll;

    private Button _button;
    private Vehicle _vehicle;

    // Start is called before the first frame update
    void Start()
    {
        Vehicle.InitializeVehicles();
        _vehicle = Vehicle.Vehicles[vehicleIndex];

        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);

        HandleClick();

        buttonText.text = _vehicle.name;
    }

    private void HandleClick()
    {
        previewMeshRenderer.material = _vehicle.material;
        previewPlaceholder.text = _vehicle.name;
    
        previewText.text = $"{TranslateSelector.GetTranslation("throttle")}: {_vehicle.baseThrottle}\n" +
                           $"{TranslateSelector.GetTranslation("boost_multiplier")}: {_vehicle.boostMultiplier}\n" +
                           $"{TranslateSelector.GetTranslation("boost_duration")}: {_vehicle.boostDuration}\n" +
                           $"{TranslateSelector.GetTranslation("yaw_strength")}: {_vehicle.yawStrength}";

        waitForAll.ChosenVehicle = vehicleIndex;
    }
}