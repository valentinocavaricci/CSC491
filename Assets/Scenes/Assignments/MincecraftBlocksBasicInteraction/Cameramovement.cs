using UnityEngine;

public class Cameramovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float moveX = 0;
        float moveZ = 0;

        if (Input.GetKey(KeyCode.UpArrow)) moveZ = 1;
        if (Input.GetKey(KeyCode.DownArrow)) moveZ = -1;
        if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1;
        if (Input.GetKey(KeyCode.RightArrow)) moveX = 1;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        transform.position += move * speed * Time.deltaTime;
    }
}