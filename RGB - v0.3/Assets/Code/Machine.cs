using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Reflection;

public struct Machine
{

    public int type, durability, maxDurability, batchSize, cycleTime, Yield;
    public int status; //0 = working, 1 = needs maintenance, broken
    public String name;
    public int c1,c2,c3,c1q,c2q,c3q;  //color 1, color 2, color 1 quantity, color 2 quantity.
    public int result;
    public bool isRunning,outputReady,completed;
    public float elapsedTime;

    public Machine(String name,int type, int maxDurability, int batchSize, int cycleTime, int Yield)
    {

        result = 0;
        this.name = name;
        this.type = type;
        
        this.maxDurability = maxDurability;
        durability = maxDurability;
        
        this.batchSize = batchSize;
        this.cycleTime = cycleTime;
        this.Yield = Yield;
        this.status = 7;

        isRunning = false;
        outputReady = false;
        completed = false;
        elapsedTime = 0f;

        c1 = -1;
        c2 = -1;
        c3 = -1;
        c1q = 0;
        c2q = 0;
        c3q = 0;

    }
    
    public override string ToString()
    {
        String strng = $"Machine Name: {name}\n Type: {type}\n Durability: {durability}\n Batch Size: {batchSize}\n Cycle Time: {cycleTime}\n Yield: {Yield}";
        return strng;
    }

    public void assignOrder(workOrder wo)
    {
        c1=wo.c1index; //set the first ingredient index
        c2=wo.c2index; //set the second ingredient index
        c3=wo.c3index; //set the desired result index
        c3q=wo.quantity; //set the desired quantity
    }

    public void loadMachine(int c1q, int c2q,int quantity)
    {
        
        this.c1q= c1q;
        this.c2q = c2q;
        this.c3q = quantity;
    }

    public void startMachine()
    {
        Debug.Log($"c1q: {c1q} c2q:{c2q}");
        
        status = 1;
        if (c1q>0 && c2q>0)
        {
            isRunning = true;
        }
    }
    public void runMachine()
    {
        Debug.Log("Do we to the inside of Machine?");
        durability--;
        if (durability <= 0)
        {
            status = 1;
        }
        isRunning = false;


        result = c1q;  //add yield in, and recognize an order with still quanity
        
    }

    public int unloadMachine()
    {
        int sendOff = result;
        
        result = 0;
        status = 7;
        completed = true;
        Debug.Log($"result: {sendOff}");
        return sendOff;

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