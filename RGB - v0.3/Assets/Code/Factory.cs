using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Factory
{
    public int tier;
    public int maxEmployeeCapacity;  //the max amount of employees
    public int minEmployeeCapacity; //does this factory require a certain number of employees? // like HR and stuff.
    public int maxMachineCapacity;
    public int upkeep;
    public float elapsedTime;

    public Factory(int tier)
    {
        this.tier = tier;

        elapsedTime = 0;

        maxEmployeeCapacity = 0;
        minEmployeeCapacity = 0;
        maxMachineCapacity = 0;
        upkeep = 0;
        

        switch (tier)
        {
            case 1:
                maxEmployeeCapacity = 10;
                minEmployeeCapacity = 1;
                maxMachineCapacity = 1;
                upkeep = 0;
                break;
            case 2:
                maxEmployeeCapacity = 25;
                minEmployeeCapacity = 10;
                maxMachineCapacity = 10;
                upkeep = 2;
                break;
            case 3:
                maxEmployeeCapacity = 50;
                minEmployeeCapacity = 25;
                maxMachineCapacity = 20;
                upkeep = 5;
                break;
            case 4:
                maxEmployeeCapacity = 75;
                minEmployeeCapacity = 50;
                maxMachineCapacity = 30;
                upkeep = 10;
                break;
            case 5:
                maxEmployeeCapacity = 100;
                minEmployeeCapacity = 75;
                maxMachineCapacity = 40;
                upkeep = 20;
                break;
        }
    }

    public void upgradeFactory()
    {
        tier += 1;
    }

    public void downgradeFactory()
    {
        tier -= 1;
    }
}
