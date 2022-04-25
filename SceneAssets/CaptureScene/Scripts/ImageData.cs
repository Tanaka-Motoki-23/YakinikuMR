using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageData : MonoBehaviour
{
    private Texture2D texture;
    private Vector3 position;
    private DateTime timestamp;

    public void setTexture(Texture2D texture)
    {
        this.texture = texture;
    }
    public void setPosition(Vector3 position)
    {
        this.position = position;
    }
    public void setTimestamp(DateTime timestamp)
    {
        this.timestamp = timestamp;
    }

    public Texture2D getTexture()
    {
        return this.texture;
    }
    public Vector3 getPosition()
    {
       return this.position;
    }
    public DateTime getTimestamp()
    {
        return this.timestamp;
    }


}

