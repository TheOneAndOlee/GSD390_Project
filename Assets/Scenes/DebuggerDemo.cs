using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var random = Random.Range(0, 5);

        if (random == 0)
        {
            throw new System.Exception();
        }
    }
}
