using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class workOrder
{
    public (Pixel pixel, int quantity) ingredientA, ingredientB, product;
    

    public enum stat
    {
        INACTIVE = 0,
        INPRODUCTION = 1,
        IDLE = 2,
        FINISHED = 3,
        WAITING = 5
    }
    public enum dest
    {
        NONE = 0,
        WORKORDER = 1,
        TRADE = 2,
        CONTRACT = 3,
        INVENTORY = 4
    }

    public stat status;
    public dest destination;


    public int machineIndex;
    public string name;
    private FlavorText fl;

    public workOrder((Pixel p, int q) a, (Pixel p, int q) b, (Pixel p, int q) pro)
    {
        fl = new FlavorText();
        name = fl.orderName();
        destination = dest.NONE;
        status = stat.INACTIVE;
        ingredientA = a;
        ingredientB = b;
        product = pro;
        machineIndex = -1;
    }

    public bool Waiting()
    {
        return status == stat.WAITING;
    }

    public bool IsActive()
    {
        return status != stat.WAITING;
    }

    public void Reset()
    {
        machineIndex = -1;
        ingredientA.pixel.Clear();
        ingredientA.quantity = -1;
        ingredientB.pixel.Clear();
        ingredientB.quantity = -1;
        product.pixel.Clear();
        product.quantity = -1;
    }
}
