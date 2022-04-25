using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class SetConfidence : MonoBehaviour
{
    public CaptureAndPredict captureAndPredict;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnSliderUpdated(SliderEventData eventData)
    {
        captureAndPredict.confidenceThreshold = eventData.NewValue;
        Debug.Log("OnValueUpdated Event:" + eventData.NewValue);
    }
}
