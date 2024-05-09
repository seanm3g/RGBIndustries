using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class workOrderManager
{
    private List<workOrder> workOrders;
    
    public workOrderManager(List<workOrder> workOrders)
        {
            this.workOrders = workOrders;
        }
    private bool openOrders()
    {
            if (workOrders.Count > 0)
                return true;
            else return false;
    }

}
