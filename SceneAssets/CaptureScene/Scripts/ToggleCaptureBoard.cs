using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCaptureBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject captureBoard;
    public CaptureAndPredict captureAndPredict;
    private MeshFilter meshFilter;
    void Start()
    {
        this.meshFilter = captureBoard.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleBoard()
    {
        this.meshFilter.mesh = new Mesh();
        if (this.captureAndPredict.showMarker == true)
        {
            this.captureAndPredict.showMarker = false;
            Debug.Log(this.captureAndPredict.showMarker);
        }
        else if (this.captureAndPredict.showMarker == false)
        {
            this.captureAndPredict.showMarker = true;
        }
        Debug.Log(this.captureAndPredict.showMarker);
    }
}
