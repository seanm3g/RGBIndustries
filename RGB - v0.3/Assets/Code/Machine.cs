using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Reflection;

public struct Machine
{

    public int type, durability, maxDurability, batchSize, cycleTime, Yield;
    public int status; //0 = idle, 1 = loading, 2 = running, 3 = unloading, 4 = completed, 5 = broken, 6 = in maintenance, 7 = choked 8= TOTALED;
    public String name;
    public int c1, c2, c3, c1q, c2q, c3q;  //color 1, color 2, color 1 quantity, color 2 quantity.
    public int result;
    public float elapsedTime;
    public int orderIndex;

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
        if (durability > 0)
        {
            durability--;
            result = c1q;  //add yield in, and recognize an order with still quanity        
        }
        else status = 5;  //broken

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
}