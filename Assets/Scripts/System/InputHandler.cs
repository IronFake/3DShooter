using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    #region Player Movement
    //Keyboard
    public Vector2 Movement { get; private set; }
    public bool ToggleRun { get; private set; }
    public bool ToggleJump { get; private set; }
    
    //Mouse
    [Range(0.1f, 10.0f)]
    public float mouseSensitivity = 2f;
    public float TurnPlayer { get; private set; }
    public float TurnCamera { get; private set; }
    #endregion

    #region Weapon Controls
    public bool ToggleReload { get; private set; }
    public float ScrollWheelMovement { get; private set; } 
    public bool ToggleChangeWeapon { get; private set; }    
    public int NumberOfKey { get; private set; }
    public bool ToggleFire { get; private set; }
    #endregion
    
    //UI
    public bool ToggleShowingScoreboard { get; private set; }
    public bool TogglePause { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        PlayerMovement();
        Weapon();

        ToggleShowingScoreboard = Input.GetKey(KeyCode.Tab);
        TogglePause = Input.GetKeyDown(KeyCode.Escape);
    }

    private void PlayerMovement()
    {
        Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        ToggleRun = Input.GetButton("Run");
        ToggleJump = Input.GetButton("Jump");

        TurnPlayer = Input.GetAxis("Mouse X") * mouseSensitivity;
        TurnCamera = -Input.GetAxis("Mouse Y") * mouseSensitivity;
    }

    private void Weapon()
    {
        ToggleFire = Input.GetMouseButton(0);
        ToggleReload = Input.GetButtonDown("Reload");
        ScrollWheelMovement = Input.GetAxis("Mouse ScrollWheel");
        CheckAlphaKeys();
    }

    private void CheckAlphaKeys()
    {
        ToggleChangeWeapon = false;
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int num;
                if (i == 0)
                    num = 10;
                else
                    num = i - 1;
                ToggleChangeWeapon = true;
                NumberOfKey = num;
                return;
            }
        }
    }
}
