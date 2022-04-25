using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObject : MonoBehaviour
{
    private string label = "";
    private Vector3 cameraPosition = new Vector3(0, 0, 0);
    private Vector3 ray = new Vector3(0, 0, 0);
    private Vector3 position = new Vector3(0, 0, 0);

    private Vector3 calcWorldPos()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(
                this.cameraPosition,
                this.ray,
                out hitInfo,
                20.0f,
                Physics.DefaultRaycastLayers)) { }

        return hitInfo.point;
    }

    public DetectObject(string label, Vector3 cameraPosition, Vector3 ray)
    {
        this.label = label;
        this.cameraPosition = cameraPosition;
        this.ray = ray;
        this.position = this.calcWorldPos();
    }

    public void setPosition(Vector3 position)
    {
        this.position = position;
    }
    public Vector3 getPosition()
    {
        return this.position;
    }
    public string getLabel()
    {
        return this.label;
    }
}
