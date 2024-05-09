using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Machine;

public class MachineManager
{
    private List<Machine> machines;
    //private List<workOrder> workOrders;
    public MachineManager(List<Machine> machines)
    {
        this.machines = machines;
    }


    /*
    public void Update()
    {
        // Iterate through each machine and update its logic based on deltaTime
        bool workToDo=openOrders();
        
        foreach (var machine in machines)
        {
            machine.Update();
            
            if (workToDo)  //there are work orders to process
                AssignOrderToAvailableMachine(workOrders[0]);

            //HandleMachineStatus(machine);
        }


    }


    private bool openOrders()
    {

        
        if (workOrders.Count > 0)
            return true;
        else return false;
        
    }

    
    public void Update()
    {
        foreach (var machine in machines)
        {

            switch (machine.status)
            {
                case MachineStatus.Idle:
                    //check if a work order is in queue
                    //not sure if this is even needed
                    AssignOrderToAvailableMachine(order);
                    machine.Update();
                    break;

                case MachineStatus.Loading:

                    machine.Load();
                    break;

                case MachineStatus.Running:

                    machine.Run();
                    break;

                case MachineStatus.Unloading:

                    //findDestination()
                    machine.Unload();
                    break;
                // Add other cases as needed
                case Machine.MachineStatus.Completed:
                    completeOrder(machine);
                    break;
                case Machine.MachineStatus.Broken:
                    // Logic for handling machine repair or notification
                    notifyMaintenance(machine);
                    break;
                    // Add other status handling as needed
            }
        }
    }


    public void AssignOrderToAvailableMachine(workOrder order)
    {
        // Find the first available idle machine and assign the order
        foreach (var machine in machines)
        {
            if (machine.IsIdle())
            {
                machine.AssignOrder(order);  //this will be more complicated
                break;  // Stop the loop once an idle machine is found and assigned
            }
            else HandleMachineStatus(machine);
        }
    }


    private void HandleMachineStatus(Machine machine)   //this handles anything that requires a machine result to affect other things
    {
        // Additional checks can be implemented to handle specific statuses
        switch (machine.status)
        {
            case Machine.MachineStatus.Completed:
                workOrders.Remove(machine.currentOrder);
                break;
            case Machine.MachineStatus.Broken:
                // Logic for handling machine repair or notification
                //notifyMaintenance(machine);
                break;
                // Add other status handling as needed
        }
    }
    */
}
