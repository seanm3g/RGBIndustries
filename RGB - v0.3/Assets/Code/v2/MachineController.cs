using System.Collections.Generic;


public class MachineController
{
    public List<Machine> Machines { get; private set; }

    public MachineController(int i)
    {
        Machines = new List<Machine>(i);
    }

    public void AddMachine(Machine machine)
    {
        Machines.Add(machine);
    }

    public void update(float deltaTime)
    {
        foreach (var machine in Machines)
        {
            if(machine.AssignedEmployee != null)
                UpdateMachine(machine, deltaTime);
        }
    }

    private void UpdateMachine(Machine machine, float deltaTime)  //this should not trigger events, only update them.
    {

        if (machine.AssignedEmployee != null && machine.AssignedEmployee.status == 1)
        {
            if (machine.STATUS != Machine.Status.IDLE)
            {
                machine.updateElapsedTime(deltaTime);

                if (machine.ElapsedTime >= machine.CycleTime)
                {
                    machine.resetElapsedTime();

                    switch (machine.STATUS)
                    {
                        case
                            Machine.Status.LOADING:
                            Loading(machine);
                            break;

                        case
                            Machine.Status.RUNNING:
                            Running(machine);
                            break;

                        case
                            Machine.Status.UNLOADING:
                            Unloading(machine);
                            break;
                    }
                }
            }
        }
    }

    private void Loading(Machine machine)
    {
            machine.setStatus(Machine.Status.RUNNING);
    }

    private void Running(Machine machine)
    {
            if (machine.ProductionCycles > 0)
            {

                if (machine.Durability.current > 0)
                {
                    machine.reduceDurability();
                    machine.reduceProductionCycle();  //reduces production cycles
                }
                else
                {
                    machine.setStatus(Machine.Status.BROKEN);
                }
            }
            else
            {
                machine.setStatus(Machine.Status.UNLOADING);
            }
    }

    private void Unloading(Machine machine)
    {
        machine.setStatus(Machine.Status.COMPLETED);
        machine.AssignedEmployee.status = Employee.IDLE;
    }

    public void CompleteOrder(Machine machine)
    {
        machine.ClearOrder();
    }

    public void breakMachine(int machineID)
    {
        GetMachineById(machineID).setStatus(Machine.Status.BROKEN);
    }

    public Machine GetMachineById(int id)
    {
        return Machines.Find(m => m.ID == id);
    }

    public void NotifyMaintenance(Machine machine)
    {
        //Debug.Log($"Machine {machine.Type} needs maintenance.");
    }

    public void assignWorkOrderToMachine(Machine assigned, workOrder wo)
    {
        //ASSIGN



    }
}
