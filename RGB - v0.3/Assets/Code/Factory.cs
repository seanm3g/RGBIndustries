using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Factory
{
    public int tier;
    public int maxEmployeeCapacity;  //the max amount of employees
    public int minEmployeeCapacity; //does this factory require a certain number of employees? // like HR and stuff.
    public int maxMachineCapacity;

    public Factory(int tier)
    {
        this.tier = tier;
        maxEmployeeCapacity = 10;
        minEmployeeCapacity = 1;
        maxMachineCapacity = 10;
    }
}
