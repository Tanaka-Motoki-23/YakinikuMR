using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgessBarManager : MonoBehaviour
{
    public GameObject progressBar;

    // Update is called once per frame
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.progressBar.GetComponent<Countdown>().StartCountDown();
        }
    }
}
