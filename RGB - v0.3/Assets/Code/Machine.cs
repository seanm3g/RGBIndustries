using System;

public class Machine
{
    public enum Status{ IDLE, LOADING, RUNNING, UNLOADING, COMPLETED, BROKEN, MAINTENANCE, UNASSIGNED}

    public Status STATUS { get; private set; }
    public int Type { get; private set; }
    public (int current, int max) Durability { get; private set; }
    public int BatchSize { get; private set; }
    public int CycleTime { get; private set; }
    public int Yield { get; private set; }
    public float OEE { get; private set; }
    public float ElapsedTime { get; private set; }
    public int ProductionCycles { get; private set; }
    public workOrder? CurrentOrder { get; private set; }

    public int Id { get; private set; }

    public Employee AssignedEmployee { get; private set; }

    public Machine(int type, int durabilityMax, int batchSize, int cycleTime, int yield)
    {
        Type = type;
        STATUS = Status.IDLE;
        Durability = (durabilityMax, durabilityMax);
        BatchSize = batchSize;
        CycleTime = cycleTime;
        Yield = yield;
        ProductionCycles = -1;
        CurrentOrder = null;
        OEE = CalculateOEE();
    }

    public void AssignOrder(workOrder order)
    {
        CurrentOrder = order;
        STATUS = Status.LOADING;
        ProductionCycles = (int)Math.Ceiling((double)order.product.quantity / BatchSize);
    }

    public void AssignEmployee(Employee employee)
    {
        AssignedEmployee = employee;
    }

    public void reduceProductionCycle()
    {
        ProductionCycles--;
    }

    public void reduceDurability()
    {
        Durability = (Durability.current - 1, Durability.max);
    }

    public void setStatus(Status stat)
    {
        STATUS = stat;
    }

    public void updateElapsedTime(float deltaTime)
    {
        ElapsedTime += deltaTime;
    }

    public void resetElapsedTime()
    {
        ElapsedTime = 0;
    }
    public void ClearOrder()
    {
        CurrentOrder = null;
        STATUS = Status.IDLE;
    }

    public bool IsIdle()
    {
        return STATUS == Status.IDLE;
    }

    public float CalculateOEE()
    {
        float availability = (float)Durability.max / 2 * (1 + (float)Durability.max);
        float performance = (float)BatchSize / CycleTime;
        float OEE = availability * performance * ((float)Yield / 100);
        
        return OEE;
    }
}
