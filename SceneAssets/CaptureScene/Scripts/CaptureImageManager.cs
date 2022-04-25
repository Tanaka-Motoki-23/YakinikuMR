using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureImageManager : MonoBehaviour
{
    public GameObject imageObject;
    public bool isLoop = false;
    private Material material;

    private DateTime lastTime;
    private CaptureAndPredict captureAndPredict;

    // Start is called before the first frame update
    void Start()
    {
        this.captureAndPredict = this.gameObject.GetComponent<CaptureAndPredict>();
        this.material = this.imageObject.GetComponent<Renderer>().material;
        this.lastTime = this.gameObject.GetComponent<CaptureAndPredict>().getImageData().getTimestamp();
    }

    void Update()
    {
        if (this.lastTime != this.gameObject.GetComponent<CaptureAndPredict>().getImageData().getTimestamp())
        {
            Debug.Log(this.lastTime);
            Debug.Log(this.gameObject.GetComponent<CaptureAndPredict>().getImageData().getTimestamp());
            this.setMaterial();
            this.lastTime = this.gameObject.GetComponent<CaptureAndPredict>().getImageData().getTimestamp();
        }
    }

    public void takePhoto()
    {
        Debug.Log("take Photo");
        this.captureAndPredict.CaptureImageData();
    }

    public void takePhotoLoop()
    {
        this.isLoop = true;
        StartCoroutine("TakePhotoLoopCoroutine");
    }
    public void stopPhotoLoop()
    {
        this.isLoop = false;
    }

    IEnumerator TakePhotoLoopCoroutine()
    {
        while (this.isLoop)
        {
            this.takePhoto();
            yield return new WaitForSeconds(5.0f);
        }
    }
    public void setMaterial()
    {
        this.material.SetTexture("_MainTex", this.gameObject.GetComponent<CaptureAndPredict>().getImageData().getTexture());
    }

    IEnumerator SetMaterialDelayCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        this.setMaterial();
    }
}
