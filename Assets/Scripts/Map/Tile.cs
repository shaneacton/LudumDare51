using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int X = -1;
    public int Y = -1;

    public Renderer renderer;

    private void Awake()
    {
        renderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
    }

    public override string ToString()
    {
        return "Tile " + X + ", " + Y + " at pos: " + transform.position;
    }

    public void setColour(Color col)
    {
        renderer.material.SetColor("_Color", col);
    }
}
