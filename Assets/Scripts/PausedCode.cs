using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedCode : MonoBehaviour
{
    private KeyCode key = KeyCode.Escape;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            gameObject.SetActive(true);
        }
    }
}
