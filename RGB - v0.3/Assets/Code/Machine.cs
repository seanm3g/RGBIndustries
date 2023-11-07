using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Reflection;
using UnityEngine.Analytics;

public struct Machine
{

    public int type, durability, maxDurability, batchSize, cycleTime, Yield;
    public float OEE;

    public int status; //0 = idle, 1 = loading, 2 = running, 3 = unloading, 4 = completed, 5 = broken, 6 = in maintenance, 7 = choked 8= TOTALED;
    public String name;
    public int c1, c2, c3, c1q, c2q, c3q;  //color 1, color 2, color 1 quantity, color 2 quantity.
    public int result;
    
    public int orderIndex;
   
    public int productionCycles;
    public int manufacturer;

    public float elapsedTime;


    FlavorText ft;
    private const int MACHINE_IDLE = 0;
    private const int MACHINE_LOADING = 1;
    private const int MACHINE_RUNNING = 2;
    private const int MACHINE_UNLOADING = 3;
    private const int MACHINE_COMPLETED = 4;
    private const int MACHINE_BROKEN = 5;
    private const int MACHINE_IN_MAINTENANCE = 6;
    private const int MACHINE_CHOKED = 7;
    

    public Machine(String name, int type, int maxDurability, int batchSize, int cycleTime, int Yield)
    {
        productionCycles = 0;
        result = 0;
        this.name = name;
        
        this.type = type;

        this.maxDurability = maxDurability;
        durability = maxDurability;

        this.batchSize = batchSize;
        this.cycleTime = cycleTime;
        this.Yield = Yield;
        this.status = 0;

        elapsedTime = 0f;
        orderIndex = -1;


        c1 = -1;
        c2 = -1;
        c3 = -1;
        c1q = 0;
        c2q = 0;
        c3q = 0;

        OEE = 0;

        manufacturer = UnityEngine.Random.Range(1, 4);
        ft = new FlavorText();

        this.name = generateName();
        OEE = calculateOEE();

    }

    public override string ToString()
    {
        String strng = $"Machine Name: {name}\n status: {status}\n Order Index: {orderIndex}\n Durability: {durability}\n Batch Size: {batchSize}\n Cycle Time: {cycleTime}\n Yield: {Yield}";
        return strng;
    }

    public void assignOrder(workOrder wo)
    {
        c1 = wo.c1index; //set the first ingredient index
        c2 = wo.c2index; //set the second ingredient index
        c3 = wo.c3index; //set the desired result index
        c3q = wo.quantity; //set the desired quantity
    }

    public void loadMachine(int c1q, int c2q)
    {

        this.c1q = c1q;
        this.c2q = c2q;
        this.c3q = c2q;
        status = MACHINE_LOADING;
    }
    public void runMachine()
    {
        if (productionCycles > 0)
        {
            productionCycles--;
            status = MACHINE_RUNNING;

            if (durability > 0)
            {
                durability--;
                 
            }
            else status = 5;  //broken
        } 
        else result = c1q;  //add yield in, and recognize an order with still quanity       

    }

    public int unloadMachine()
    {
        int sendOff = result;
        result = 0;

        return sendOff;

    }

    void repair()
    {
        status = 6;
        maxDurability -= 1;
        if (maxDurability < 1)
        {
            status = 8; //totaled machine
        }
        
        else durability = maxDurability;
    }
    public void reset()
    {
        elapsedTime = 0;
        status = 0;
        orderIndex = -1;
    }

    public float calculateOEE()
    {
        //Debug.Log("Are we calcuating OEE?");

        // Explicitly cast integers to float before division
        float availability = (float)maxDurability / 2 * (1 + (float)maxDurability);
        float performance = (float)batchSize / cycleTime;
        float tempOEE = availability * performance * ((float)Yield / 100);

        //Debug.Log("availability: " + availability);
        //Debug.Log("performance: " + performance);
        //Debug.Log("OEE: " + tempOEE);

        return tempOEE;
    }


    private string generateName()
    {
        string name = "";

        switch(manufacturer)
        {
            case 1:
                name += "N";
                break;
            case 2:
                name += "E";
                break;
            case 3:
                name += "S";
                break;
            case 4:
                name += "W";
                break;
        }
        name += "-";

        int productionRate = (int)Math.Round((double)batchSize / cycleTime * Yield);  //this is garbage

        int maxcycles = maxDurability / 2 * (1 + maxDurability);

        int serialNum = productionRate * maxcycles;

        name += serialNum.ToString();

        switch(ft.rarity())
        {
            case 1: name += "c"; break;
            case 2: name += "u"; break;
            case 3: name += "r"; break;
        }
        
        return name;
    }


}