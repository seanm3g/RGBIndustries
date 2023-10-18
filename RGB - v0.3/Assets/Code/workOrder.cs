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
    public int machineIndex;
    public string name;
    FlavorText fl;

    public workOrder(int q, ColorRGB p1, ColorRGB p2, ColorRGB p3)
    {
        colorLib = new ColorLibrary();
        fl = new FlavorText();
        
        name = fl.orderName();
        isActive = false;
        quantity = q;

        machineIndex = -1;

        c1index = colorLib.getIndex(p1);
        c2index = colorLib.getIndex(p2);
        c3index = colorLib.getIndex(p3);   //gets the index for the color value, but saves the level 
    }
    public workOrder(int q, int p1, int p2, int p3)
    {
        colorLib = new ColorLibrary();

        fl = new FlavorText();
        name = fl.orderName();
        isActive = false;
        quantity = q;

        machineIndex = -1;

        c1index = p1;
        c2index = p2;
        c3index = p3;   //gets the index for the color value, but saves the level


    }
    public workOrder(workOrder w)
    {
        colorLib = new ColorLibrary();

        fl = new FlavorText();
        name = fl.orderName();

        isActive = false;
        quantity = w.quantity;

        machineIndex = -1;

        c1index = w.c1index;
        c2index = w.c2index;
        c3index = w.c3index;

    }
    public void reset()
    {
        isActive = false;
        machineIndex= -1;
        c1index= 0;
        c2index= 0;
        c3index= 0;
    }
}
