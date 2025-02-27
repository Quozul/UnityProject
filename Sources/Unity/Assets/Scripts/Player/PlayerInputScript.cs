using Checkpoints;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ShipController))]
[RequireComponent(typeof(CheckpointController))]
public class PlayerInputScript : MonoBehaviour
{
    // Accessor
    [SerializeField] private GameObject _blaster;
    [SerializeField] private GameObject _uiHUD;
    [SerializeField] private GameObject waitingScreen;

    public GameObject completePlayer;

    private ShipController _shipController;
    private PlayerPauseMenu _pauseMenu;
    private CheckpointController _checkpointController;
    private PlayerStatsScript _playerStatsScript;
    private InputActionAsset _inputAsset;
    private InputActionMap _player;

    private void Awake()
    {
        _pauseMenu = GameObject.FindGameObjectWithTag("Pause").GetComponent<PlayerPauseMenu>();
        // find this solution not the best but i have no other idea
        
        _inputAsset = GetComponent<PlayerInput>().actions;
        _player = _inputAsset.FindActionMap("Player");
        _shipController = GetComponent<ShipController>();
        _checkpointController = gameObject.GetComponent<CheckpointController>();
        _playerStatsScript = gameObject.GetComponent<PlayerStatsScript>();
    }

    private void OnEnable()
    {
        _player.FindAction("Use").started += UseBonus;
        _player.FindAction("Boost").started += Boost;
        _player.FindAction("Boost").canceled += Boost;
        _player.FindAction("Shoot").started += Shoot;
        _player.FindAction("Pause").started += Pause;
        _player.FindAction("Movement").performed += Direction;
        _player.FindAction("Movement").canceled += Direction;
        _player.FindAction("Movement").started += Direction;
        _player.FindAction("Rotate").performed += Rotation;
        _player.FindAction("Rotate").canceled += Rotation;
        _player.FindAction("Rotate").started += Rotation;
        _player.FindAction("Respawn").started += Respawn;

        _player.Enable();
    }

    public void UnregisterEvents()
    {
        _player.Disable();
    }

    private void OnDisable()
    {
        _player.FindAction("Use").started -= UseBonus;
        _player.FindAction("Boost").started -= Boost;
        _player.FindAction("Boost").canceled -= Boost;
        _player.FindAction("Shoot").started -= Shoot;
        _player.FindAction("Pause").started -= Pause;
        _player.FindAction("Movement").performed -= Direction;
        _player.FindAction("Movement").canceled -= Direction;
        _player.FindAction("Movement").started -= Direction;
        _player.FindAction("Rotate").performed -= Rotation;
        _player.FindAction("Rotate").canceled -= Rotation;
        _player.FindAction("Respawn").started -= Respawn;
    }

    private void Respawn(InputAction.CallbackContext obj)
    {
        _checkpointController.RespawnEntity();
    }

    // Action function

    private void Rotation(InputAction.CallbackContext obj)
    {
        double x = System.Math.Tanh(obj.ReadValue<float>()) * 2;
        _shipController.SetYaw((float) x);
    }

    private void Direction(InputAction.CallbackContext obj)
    {
        Vector2 vec = obj.ReadValue<Vector2>();
        _shipController.Move(vec);
    }

    private void UseBonus(InputAction.CallbackContext obj)
    {
        _playerStatsScript.unableBonusUse();
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        _blaster.GetComponent<PlayerBlaster>().Shoot();
    }

    private void Boost(InputAction.CallbackContext obj)
    {
        var isBoosting = obj.ReadValueAsButton();
        _shipController.ActiveBoost(isBoosting);
    }

    private void Pause(InputAction.CallbackContext obj)
    {
        // If the game is not loading, allow pause
        if (!LoadSceneManager.loading)
        {
            _pauseMenu.PauseProcess(_uiHUD);
        }
    }

    public void DisableWaitingScreen()
    {
        waitingScreen.SetActive(true);
    }

    public void DisplayWaitingScreen()
    {
        waitingScreen.SetActive(true);
        _uiHUD.SetActive(false);
    }
}