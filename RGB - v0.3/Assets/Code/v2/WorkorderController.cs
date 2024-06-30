using System.Collections.Generic;

public class workOrderController
{
    private List<workOrder> workOrders;

    public workOrderController(int i,MachineController mc)
    {
        workOrders = new List<workOrder>(i);
    }



    public void update()
    {
        CheckworkOrders();  //this isn't written yet, but this is the infrastructure
    }

    public void AddworkOrder(workOrder order)
    {
        workOrders.Add(order);
    }

    public void RemoveworkOrder(workOrder order)
    {
        workOrders.Remove(order);
    }

    public void CheckworkOrders()     //needs to be written
    {
        foreach (var order in workOrders)
        {
            // Perform checks on each work order
        }
    }

    public workOrder getPendingWorkOrder()
    {
        return workOrders[3];  //this is gibberish

    }

    public List<workOrder> GetActiveworkOrders()
    {
        return workOrders.FindAll(order => order.IsActive());
    }

    public List<workOrder> GetPendingworkOrders()
    {
        return workOrders.FindAll(order => order.pending());
    }

    public void AssignworkOrderToMachine(workOrder order, int machineIndex)
    {
        if (order.status == workOrder.stat.PRODUCTION)
        {
            order.machineIndex = machineIndex;
            order.status = workOrder.stat.PRODUCTION;
        }
    }

    public void ResetworkOrder(workOrder order)
    {
        order.Reset();
    }

    public void UpdateworkOrders()
    {
        // Update logic for work orders, if necessary
    }

    // Additional methods to manage the collection of work orders
}
