using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private GameObject[] progressBars;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.progressBars = GameObject.FindGameObjectsWithTag("Progress");
        foreach (GameObject gameObj in this.progressBars)
        {
            if (gameObj.GetComponent<Countdown>().isActive == false)
            {
                StartCoroutine("DeleteCoroutine", gameObj);
            }
        }
    }

    private IEnumerator DeleteCoroutine(GameObject targetObj)
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(targetObj);
        yield return null;
    }
}
