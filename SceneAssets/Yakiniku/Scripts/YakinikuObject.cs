using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakinikuObject : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject prefab_gyu;
    private GameObject prefab_buta;

    private DetectObject detectObject;
    private GameObject progressBar;


    public YakinikuObject(DetectObject detectObject)
    {
        this.prefab_gyu = (GameObject)Resources.Load("cow");
        this.prefab_buta = (GameObject)Resources.Load("pig");

        this.detectObject = detectObject;
        this.putProgressBar();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void putProgressBar()
    {
        if (this.detectObject.getLabel() == "gyu-kata-roast")
        {
            this.progressBar = Instantiate(this.prefab_gyu, this.detectObject.getPosition(), Quaternion.identity);
        }
        else if (this.detectObject.getLabel() == "buta-bara")
        {
            this.progressBar = Instantiate(this.prefab_buta, this.detectObject.getPosition(), Quaternion.identity);
        }
        else
        {
            Debug.Log("Error Class:" + this.detectObject.getLabel());
        }
        this.progressBar.transform.Find("ProgressBar").gameObject.GetComponent<Countdown>().initStats();
        this.progressBar.transform.Find("ProgressBar").gameObject.GetComponent<Countdown>().StartCountDown();
    }

    public DetectObject getDetectObject()
    {
        return this.detectObject;
    }
    public GameObject getProgressBar()
    {
        return this.progressBar.transform.Find("ProgressBar").gameObject;
    }

    public GameObject getPrefab()
    {
        return this.progressBar;
    }

    public Vector3 getPosition()
    {
        this.detectObject.setPosition(this.progressBar.transform.Find("ProgressBar").gameObject.transform.position);
        return this.detectObject.getPosition();
    }
}
