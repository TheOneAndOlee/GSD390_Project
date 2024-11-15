using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (illusionOfChoice() == 0)
        {
            throw new System.Exception();
        }
    }
    public int illusionOfChoice()
    {
        var num = Random.Range(0, 1);
        return num;
    }
}
