using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLoop : MonoBehaviour
{
    public bool isRotation = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotation)
        {
            this.gameObject.transform.LookAt(Camera.main.transform);
        }
       
    }
}
