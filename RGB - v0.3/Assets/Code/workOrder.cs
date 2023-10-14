using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct workOrder
{
    // Start is called before the first frame update

    public int c1index;
    public int c2index;
    public int c3index;  //the index for the pixel value in the library.
    public int quantity;
    ColorLibrary colorLib;
    public bool isActive;

    public workOrder(int q, ColorRGB p1, ColorRGB p2, ColorRGB p3)
    {
        colorLib = new ColorLibrary();

        isActive = false;
        quantity = q;

        c1index = colorLib.getIndex(p1);
        c2index = colorLib.getIndex(p2);
        c3index = colorLib.getIndex(p3);   //gets the index for the color value, but saves the level
        
        
    }

    public void makeActive()
    {
        isActive = true;
    }
}
