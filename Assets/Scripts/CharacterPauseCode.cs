using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPauseCode : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    private KeyCode key = KeyCode.Escape;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Debug.Log("Escape Pressed");
            pauseMenu.SetActive(true);
        }

    }
}
