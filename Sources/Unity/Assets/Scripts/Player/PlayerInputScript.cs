using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ShipController))]
public class PlayerInputScript : MonoBehaviour
{
    // Accessor
    [SerializeField] private GameObject _blaster;
    [SerializeField] private GameObject _uiHUD;

    private ShipController _shipController;
    private GameObject _pauseMenu;

    private InputActionAsset _inputAsset;
    private InputActionMap _player;

    private void Awake()
    {
        _pauseMenu = GameObject.FindGameObjectWithTag("Pause");
        // find this solution not the best but i have no other idea
        _pauseMenu.SetActive(false);
        
        _inputAsset = GetComponent<PlayerInput>().actions;
        _player = _inputAsset.FindActionMap("Player");
        _shipController = GetComponent<ShipController>();
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

        _player.Enable();
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
        _player.FindAction("Rotate").started -= Rotation;

        _player.Disable();
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
        gameObject.GetComponent<PlayerStatsScript>().unableBonusUse();
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
        if (_pauseMenu)
        {
            if (_pauseMenu.activeSelf)
            {
                _player.FindAction("Shoot").started += Shoot; // Enable shooting back
                Cursor.lockState = CursorLockMode.Confined;
                _uiHUD.SetActive(true);
                Time.timeScale = 1.0f;
                _pauseMenu.SetActive(false);
            }
            else
            {
                _player.FindAction("Shoot").started -= Shoot; // Disable shooting in pause menu
                Cursor.lockState = CursorLockMode.None;
                _uiHUD.SetActive(false);
                Time.timeScale = 0.0f;
                _pauseMenu.SetActive(true);
            }
        }
    }
}