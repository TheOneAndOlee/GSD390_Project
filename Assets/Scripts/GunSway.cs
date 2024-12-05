using System;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    [SerializeField]
    private float smoothFactor;

    [SerializeField]
    private float smoothSpeed;

    // Update is called once per frame
    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * smoothSpeed;
        float mouseY = Input.GetAxisRaw("Mouse Y") * smoothSpeed;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothFactor * Time.deltaTime);
    }
}
