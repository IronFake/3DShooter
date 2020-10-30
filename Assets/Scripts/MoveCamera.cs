using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform CameraPosition;

    public float MouseSensitivity;

    private float m_HorizontalAngle;
    private float m_VerticalAngle;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float turnPlayer = Input.GetAxis("Mouse X") * MouseSensitivity;
        m_HorizontalAngle = m_HorizontalAngle + turnPlayer;

        if (m_HorizontalAngle > 360) m_HorizontalAngle -= 360.0f;
        if (m_HorizontalAngle < 0) m_HorizontalAngle += 360.0f;

        Vector3 currentAngles = transform.localEulerAngles;
        currentAngles.y = m_HorizontalAngle;
        transform.localEulerAngles = currentAngles;

        var turnCam = -Input.GetAxis("Mouse Y");
        turnCam = turnCam * MouseSensitivity;
        m_VerticalAngle = Mathf.Clamp(turnCam + m_VerticalAngle, -89.0f, 89.0f);
        currentAngles = CameraPosition.transform.localEulerAngles;
        currentAngles.x = m_VerticalAngle;
        CameraPosition.transform.localEulerAngles = currentAngles;

    }
}
