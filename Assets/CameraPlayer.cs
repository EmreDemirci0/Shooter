using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    public Player player;
    public float moveSpeed = 5f; // Kameranın hareket hızı
    public float mouseSensitivity = 2f; // Fare hassasiyeti
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        // WASD ile hareket
        float moveX = Input.GetAxis("Horizontal"); // A ve D tuşları
        float moveZ = Input.GetAxis("Vertical");   // W ve S tuşları
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2f : moveSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.position += move * currentSpeed * Time.deltaTime;
    }

    void HandleMouseLook()
    {
        // Fare hareketi ile kamera döndürme
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Yukarı ve aşağı bakış sınırı

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
