using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraToPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cameraPos;

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPos.position;
    }
}
