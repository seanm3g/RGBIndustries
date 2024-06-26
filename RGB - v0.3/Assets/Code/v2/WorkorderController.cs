using System.Collections.Generic;
using UnityEngine;

public class WorkOrderController
{
    private List<WorkOrder> workOrders;

    public WorkOrderController(int i)
    {
        workOrders = new List<WorkOrder>(i);
    }



    public void update()
    {
        CheckWorkOrders();  //this isn't written yet, but this is the infrastructure
    }

    public void AddWorkOrder(WorkOrder order)
    {
        workOrders.Add(order);
    }

    public void RemoveWorkOrder(WorkOrder order)
    {
        workOrders.Remove(order);
    }

    public void CheckWorkOrders()     //needs to be written
    {
        foreach (var order in workOrders)
        {
            // Perform checks on each work order
        }
    }

    public List<WorkOrder> GetActiveWorkOrders()
    {
        return workOrders.FindAll(order => order.IsActive());
    }

    public List<WorkOrder> GetWaitingWorkOrders()
    {
        return workOrders.FindAll(order => order.Waiting());
    }

    public void AssignWorkOrderToMachine(WorkOrder order, int machineIndex)
    {
        if (order.status == WorkOrder.STATUS_WAITING)
        {
            order.machineIndex = machineIndex;
            order.status = WorkOrder.STATUS_IN_PRODUCTION;
        }
    }

    public void ResetWorkOrder(WorkOrder order)
    {
        order.Reset();
    }

    public void UpdateWorkOrders()
    {
        // Update logic for work orders, if necessary
    }

    // Additional methods to manage the collection of work orders
}
