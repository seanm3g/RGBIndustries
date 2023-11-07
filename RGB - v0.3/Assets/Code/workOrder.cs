using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct workOrder
{
    // Start is called before the first frame update

    public int c1index;
    public int c2index;
    public int c3index;  //the index for the pixel value in the library.
    public int quantity;  //this is redundant
    ColorLibrary colorLib;
    public bool isActive;
    public int status;  ///status 5 = waiting on input color

    public int requiredAQuantity;
    public int requiredBQuantity;

    public int currentAQuantity;
    public int currentBQuantity;

    public int machineIndex;
    public string name;
    FlavorText fl;

    public int destination;  //1 =  WORK ORDER, 2 = TRADE, 3 = CONTRACT, 4 = INVENTORY

    //ADD CONSTANTS FOR EACH STATE

    private const int DESTINATION_WORK_ORDER = 1;
    private const int DESTINATION_TRADE = 2;
    private const int DESTINATION_CONTRACT = 3;
    private const int DESTINATION_INVENTORY = 4;

    public workOrder(int q, ColorRGB p1, ColorRGB p2, ColorRGB p3)
    {

        colorLib = new ColorLibrary();
        fl = new FlavorText();
        
        requiredAQuantity = q;
        requiredBQuantity = q;
        currentAQuantity = 0;
        currentBQuantity = 0;

        name = fl.orderName();
        isActive = false;
        quantity = q;
        machineIndex = -1;

        c1index = colorLib.getIndex(p1);
        c2index = colorLib.getIndex(p2);
        c3index = colorLib.getIndex(p3);   //gets the index for the color value, but saves the level 

        this.destination = 0;
        status = 0;
    }
    public workOrder(int q, int p1, int p2, int p3,int destination)
    {
        colorLib = new ColorLibrary();

        fl = new FlavorText();
        name = fl.orderName();
        isActive = false;
        quantity = q;

        machineIndex = -1;

        requiredAQuantity = q;
        requiredBQuantity = q;
        currentAQuantity = 0;
        currentBQuantity = 0;

        c1index = p1;
        c2index = p2;
        c3index = p3;   //gets the index for the color value, but saves the level

        this.destination = destination;
        status = 0;

    }
    public workOrder(workOrder w)
    {
        colorLib = new ColorLibrary();

        fl = new FlavorText();
        name = fl.orderName();

        isActive = false;
        quantity = w.quantity;  //this might be redundant now

        machineIndex = -1;

        c1index = w.c1index;
        c2index = w.c2index;
        c3index = w.c3index;

        requiredAQuantity = w.quantity;
        requiredBQuantity = w.quantity;
        currentAQuantity = 0;
        currentBQuantity = 0;

        this.destination = 0;
        status = 0;
    }
    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    public void reset()
    {
        isActive = false;
        machineIndex= -1;
        c1index= 0;
        c2index= 0;
        c3index= 0;
    }
}
