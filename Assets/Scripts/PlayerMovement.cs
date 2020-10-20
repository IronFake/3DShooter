using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float jumpSpeed = 1f;
    public float gravity = 9.8f; 

    private float vSpeed = 0;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        /*transform.Rotate(0, x * turnSpeed * Time.deltaTime, 0);
        Vector3 vel = transform.forward * z * speed;*/
        
        Vector3 vel = (transform.right * x + transform.forward * z) * speed;

        if (controller.isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKeyDown("space"))
            {
                vSpeed = jumpSpeed;
            }
        }

        vSpeed -= gravity * Time.deltaTime;
        vel.y = vSpeed;

        controller.Move(vel * Time.deltaTime);
    }
}
