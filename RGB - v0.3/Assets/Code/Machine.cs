using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public struct Machine
{

    public int type, durability, batchSize, cycleTime, Yield, status;

    public Machine(int type, int durability, int batchSize, int cycleTime, int Yield)
    {
        this.type = type;
        this.durability = durability;
        this.batchSize = batchSize;
        this.cycleTime = cycleTime;
        this.Yield = Yield;
        this.status = 0;

    }

    public override string ToString()
    {
        String strng = $"Type: {type}\n Durability: {durability}\n Batch Size: {batchSize}\n Cycle Time: {cycleTime}\n Yield: {Yield}";
        return strng;
    }
}