using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform characterRoot;

    public float HorizontalAngle { get; set; }
    public float VerticalAngle { get; set; }

    private InputHandler m_input;

    public void Awake()
    {
        m_input = InputHandler.Instance;
        GameSystem.Instance.LockCursor();
    }

    void Update()
    {
        if (GameSystem.Instance.IsMatchOver || GameSystem.Instance.IsPause)
            return;

        HorizontalAngle += m_input.TurnPlayer;
        if (HorizontalAngle > 360) HorizontalAngle -= 360.0f;
        if (HorizontalAngle < 0) HorizontalAngle += 360.0f;

        Vector3 currentAngles = transform.localEulerAngles;
        currentAngles.y = HorizontalAngle;
        transform.localEulerAngles = currentAngles;

        VerticalAngle += m_input.TurnCamera;
        VerticalAngle = Mathf.Clamp(VerticalAngle, -89.0f, 89.0f);
        currentAngles = characterRoot.transform.localEulerAngles;
        currentAngles.x = VerticalAngle;
        characterRoot.transform.localEulerAngles = currentAngles;
    }
}
