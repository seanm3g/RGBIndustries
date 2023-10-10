using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public struct Machine
{

    public int type, durability, maxDurability, batchSize, cycleTime, Yield;
    public int status; //0 = working, 1 = needs maintenance, broken
    public String name;

    public Machine(String name,int type, int maxDurability, int batchSize, int cycleTime, int Yield)
    {

        this.name = name;
        this.type = type;
        
        this.maxDurability = maxDurability;
        durability = maxDurability;
        
        this.batchSize = batchSize;
        this.cycleTime = cycleTime;
        this.Yield = Yield;
        this.status = 0;
        
    }
    
    public override string ToString()
    {
        String strng = $"Machine Name: {name}\n Type: {type}\n Durability: {durability}\n Batch Size: {batchSize}\n Cycle Time: {cycleTime}\n Yield: {Yield}";
        return strng;
    }

    void runMachine()
    {
        durability--;
        if (durability <= 0) 
        {
            status = 1;
        }
    }
    
    void repair()
    {
        maxDurability -= 1;
        if (maxDurability <= 0)
        {
            status = 3;
        }
        else durability = maxDurability;
    }
}