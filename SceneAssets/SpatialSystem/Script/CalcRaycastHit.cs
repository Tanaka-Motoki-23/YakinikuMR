using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcRaycastHit : MonoBehaviour
{

    public GameObject hitPoint;
    public bool showCursor = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 CalcTargetPosition(Vector3 cameraPosition, Vector2 screenPosition)
    {
        Vector3 screenPosition3D = new Vector3(screenPosition.x, screenPosition.y, 1.0f);
        Vector3 targetRay = Camera.main.ScreenToWorldPoint(screenPosition3D);

        RaycastHit hitInfo;
        if (Physics.Raycast(
                cameraPosition,
                targetRay,
                out hitInfo,
                20.0f,
                Physics.DefaultRaycastLayers)){ }

        return hitInfo.point;
    }

}
