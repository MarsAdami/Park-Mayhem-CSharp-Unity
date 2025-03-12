using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public Transform player;
    public float mouseSens = 1000f;
    private float xRotation;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseXPos = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseYPos = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseYPos;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);


        player.Rotate(Vector3.up * mouseXPos);
    }
}
