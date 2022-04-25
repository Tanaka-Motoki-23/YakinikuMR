using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchYakinikuDelete : MonoBehaviour, IMixedRealityPointerHandler
{

    // Start is called before the first frame update
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if (eventData.Pointer is SpherePointer)
        {
            Debug.Log($"Grab start from {eventData.Pointer.PointerName}");
        }
        if (eventData.Pointer is PokePointer)
        {
            Debug.Log($"Touch start from {eventData.Pointer.PointerName}");
        }
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData) {
        if (eventData.Pointer.PointerName == "Left_ShellHandRayPointer(Clone)" || eventData.Pointer.PointerName == "Left_ConicalGrabPointer(Clone)")
        {

            this.gameObject.transform.Find("ProgressBar").gameObject.GetComponent<Countdown>().isActive = false;

            Debug.Log($"Touch start from {eventData.Pointer.PointerName}");
        }
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
    public void OnPointerUp(MixedRealityPointerEventData eventData) { }
}
