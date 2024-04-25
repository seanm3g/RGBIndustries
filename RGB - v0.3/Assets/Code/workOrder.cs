using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class workOrder
{
    // Things like "checkWorkOrders" should be in this class as order.check(order) instead of ref order stuff.

    // A workorder becomes a work order when it has a Ingredient A and Ingredient B

    public (Pixel pixel,int quantity) ingredientA, ingredientB, product;
   


    public int status;  ///status 5 = waiting on input color

    public const int STATUS_INACTVE = 0;
    public const int STATUS_IN_PRODUCTION = 1;
    public const int STATUS_IDLE = 2;
    public const int STATUS_FINSHED = 3;
    public const int STATUS_WAITING = 5; 


    public int machineIndex;
    public string name;
    FlavorText fl;

    public int destination;

    //ADD CONSTANTS FOR EACH STATE

    private const int DESTINATION_NONE = 0;
    private const int DESTINATION_WORK_ORDER = 1;
    private const int DESTINATION_TRADE = 2;
    private const int DESTINATION_CONTRACT = 3;
    private const int DESTINATION_INVENTORY = 4;

    public workOrder((Pixel p, int q) a, (Pixel p, int q) b, (Pixel p, int q) pro)
    {
        fl = new FlavorText();

        name = fl.orderName();  //what does this do?  line 2038 in logicCenter

        destination = DESTINATION_NONE;

        status = STATUS_INACTVE;

        ingredientA = a;
        ingredientB = b;
        product = pro;

        machineIndex = -1;

    }

    public bool waiting()  //should this have the other statuses?
    {
        if (status == STATUS_WAITING)
            return true;
        else return false;
    }

    public bool isActive()
    {
        if (status != STATUS_INACTVE)
            return true;
        else return false;


    }
    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    public void reset()
    {
        machineIndex= -1;
        ingredientA.pixel.Clear();
        ingredientA.quantity = -1;
        
        ingredientB.pixel.Clear();
        ingredientB.quantity = -1;
        
        product.pixel.Clear();
        product.quantity = -1;
    }

}
