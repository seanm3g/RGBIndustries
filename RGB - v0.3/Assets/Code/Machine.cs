using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Reflection;
using UnityEngine.Analytics;
using System.Xml.Linq;
using System.ComponentModel;

public class Machine
{
    public enum MachineStatus { Idle, Loading, Running, Unloading, Completed, Broken, InMaintenance }
    public MachineStatus status { get; private set; }
    public int type { get; private set; }  //Type is the type of machine
    private (int current, int max) durability; //this is the durability stat
    public int batchSize { get; private set; }  //batch size stat
    public int cycleTime { get; private set; }  //cycletime stat
    public int yield { get; private set; }
    public float OEE { get; private set; }  // Operational efficiency

    public float elapsedTime { get; private set; }

    public int productionCycles { get; private set; }  //this is how many cycles are needed for the current work order to run.

    public workOrder? currentOrder { get; private set; }  // the ? makes it null

    // Constructor with initial setups

    public Machine(int type, int durabilityMax, int batchSize, int cycleTime, int Yield)
    {
        type = type;
        status = MachineStatus.Idle;
        durability = (durabilityMax, durabilityMax);
        batchSize = batchSize;
        cycleTime = cycleTime;
        productionCycles = -1;  //-1 means no thing is assigned
        OEE = calculateOEE();
    }




    public void Update()
    {
        // Update logic based on 
        if (status != MachineStatus.Idle)   //if it's idle it's not ticking
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= cycleTime)
            {

                elapsedTime = 0f; //reset elapsed time // update the status of the machine.

                switch (status)
                {
                    case MachineStatus.Loading: //this code only runs when
                        Loading();  //this runs at the end of the cycle when it is completed
                        break;

                    case MachineStatus.Running:
                        Running();
                        break;

                    case MachineStatus.Unloading:
                        Unloading();
                        break;
                        // Add other cases as needed
                }
            }
        }
    }


    public void AssignOrder(workOrder order)  //This seems to work
    {
        currentOrder = order;
        status = MachineStatus.Loading;

        productionCycles = (int)Math.Ceiling((double)order.product.quantity / batchSize);   //determines how many production cycles are needed to complete this order
    }

    private void Loading()
    {
        //load the 
        status = MachineStatus.Running;  //set for next
    }

    private void Running()
    {

        if (productionCycles > 0)
        {
            productionCycles--;
            status = MachineStatus.Running;

            if (durability.current > 0)
            {
                durability.current--;
            }
            else
            {
                status = MachineStatus.Broken;
                return;  //this might not be needed ( and might be breaking things. Revisit)
            }
        }
        else

        {
            status = MachineStatus.Unloading;
        }
         

    }



    private void Unloading()
    {
        // Simulate unloading process
        status = MachineStatus.Completed;
    }

    private void CompleteOrder(Machine machine)
    {
            machine.clearOrder();
    }

    public void clearOrder()
    {
        currentOrder = null;
        status= MachineStatus.Idle;
    }

    private void NotifyMaintenance(Machine machine)
    {
        // Logic to notify about maintenance or handle automatic repairs
        Console.WriteLine($"Machine {machine.type} needs maintenance.");
        // Potentially enqueue this machine for a maintenance schedule
    }



    public bool IsIdle()
    {
        return status == MachineStatus.Idle;
    }

    public float calculateOEE()
    {
        //Debug.Log("Are we calcuating OEE?");

        // Explicitly cast integers to float before division
        float availability = (float)durability.max / 2 * (1 + (float)durability.max);
        float performance = (float)batchSize / cycleTime;
        float tempOEE = availability * performance * ((float)yield / 100);

        //Debug.Log("availability: " + availability);
        //Debug.Log("performance: " + performance);
        //Debug.Log("OEE: " + tempOEE);

        return tempOEE;
    }


}








/*
public struct Machine
{
    readonly FlavorText ft;  //this refernces the class that generates all the flavor text

    public int type { get; set; }  //this sets the type of machine this is, and creates get/set method

    public const int TYPE_HARVESTER = 0;  // USED TO TURN ORE INTO PIXELS
    public const int TYPE_TRANSFIBULATOR = 1;  //converts a primary pixel into another primary pixel || SLOW
    public const int TYPE_ASSEMBLER = 2;   // combines to pixels
    public const int TYPE_SEPERATOR = 3;  //splits 1 pixel into 2
    public const int TYPE_REFINERY = 4;  //merges pixels to a higher level



    public (int current, int max) durability;  //a durability variable is created that contains a max and current attribute.
    public int batchSize { get; set; }  //the amount a machine can run at a time, creates a get/set methdo
    public int cycleTime { get; set; }  //the time it takes to run, creates a get/set method
    public int Yield { get; set; }  //the yield % when a job is completed, creates a get/set method

    public float OEE;   //how efficient is this machine?

    
    public String name;  //name of the machine

    public (Pixel p,int quantity) ingredientA, ingredientB, output;  //these are the things inside the machine.
    
    public int orderIndex;  //this references the index from the work order LIST that
   
    public int productionCycles;  //how many cycles are needed for a particular batch
    public int manufacturer;  //the index for the manufacturer

    public float elapsedTime;  //the time metric for the machine.

    public enum MachineStatus { Idle, Loading, Running, Unloading, Completed, Broken, InMaintenance }
    public MachineStatus status { get; private set; }

    public const int MACHINE_IDLE = 0;
    public const int MACHINE_LOADING = 1;
    public const int MACHINE_RUNNING = 2;
    public const int MACHINE_UNLOADING = 3;
    public const int MACHINE_COMPLETED = 4;
    public const int MACHINE_BROKEN = 5;
    public const int MACHINE_IN_MAINTENANCE = 6;
    public const int MACHINE_STARVED = 6;  //waiting on input
    public const int MACHINE_CHOKED = 7;   //waiting on output
    public const int MACHINE_RETIRED = 8;  //machine is recycled?  || This should give you something really good.




    public Machine(int type)  //these should be assigned not in the constructor
    {
        ft = new FlavorText();

        name = "";  //production number
        this.type = type;


        this.status = MachineStatus.Idle;  //every machine starts idle

        this.durability.max = ft.rollDice(3, 6);
        this.batchSize = ft.rollDice(3, 6);
        this.cycleTime = ft.rollDice(3, 3) + 1;
        this.Yield = 103 - ft.rollDice(3, 6);

        productionCycles = 0;

        elapsedTime = 0f;  //sets to zero
        orderIndex = -1;   // undefined

        ingredientA.Item1 = Pixel.assignDefault();  //p(0,0,0,0)
        ingredientA.Item2 = -1;

        ingredientB.Item1 = Pixel.assignDefault();
        ingredientB.Item2 = -1;

        output.Item1 = Pixel.assignDefault();
        output.Item2 = -1;

        OEE = 0;  //sets ths number default to 0.  

        manufacturer = UnityEngine.Random.Range(1, 4);

        durability.max = ft.rollDice(3,6);
        durability.current = durability.max;
        

        this.OEE = calculateOEE();  //measure OEE
        this.name = generateName();
        

    }

    public override string ToString()
    {
        String strng = $"Machine Name: {name}\n status: {status}\n Order Index: {orderIndex}\n Durability: {durability.current}\n Batch Size: {batchSize}\n Cycle Time: {cycleTime}\n Yield: {Yield}";
        return strng;
    }


    public static Machine operator +(Machine m, workOrder wo)
    {
        // Create a new Machine object based on m
        Machine newMachine = m;

        // Modify the properties of the new machine
        newMachine.output.p = wo.product.pixel;
        newMachine.output.quantity = wo.product.quantity;

        return newMachine;
    }

    public static bool operator ==(Machine m1, Machine m2)
    {

        if (m1.output.quantity == m2.output.quantity && m1.output.p == m2.output.p)
            return true;
        else return false;


    }

    public static bool operator !=(Machine m1, Machine m2)
    {
        if (m1.output.quantity != m2.output.quantity && m1.output.p != m2.output.p)
            return false;
        else return true;

    }

    public override bool Equals(object o)
    {  
       return true;  
    }  

    public override int GetHashCode()
    {
        return 0;
    }

    public void assignOrder(workOrder wo)
    {
        output.p = wo.product.pixel;
        output.quantity = wo.product.quantity;
    }
    public void loadMachine((Pixel pixel, int quantity) p1,(Pixel pixel,int quantity) p2)
    {
        
        //this.c1q = c1q;
        //this.c2q = c2q;
        //this.c3q = c2q;
        

        ingredientA = p1;
        ingredientB = p2;

        status = MachineStatus.Loading;
    }

    public void isLoading()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime>= cycleTime)
        {
            elapsedTime = 0f;
            status = MachineStatus.Running;
        }
    }

    public void isRunning()
    {
        elapsedTime += Time.deltaTime;


        if (elapsedTime>= cycleTime)  //when the trigger happens
        {
            if (productionCycles > 0)
            {
                productionCycles--;
                status = MachineStatus.Running;

                if (durability.current > 0)
                {
                    durability.current--;
                }
                else
                {
                    status = MachineStatus.Broken;
                    return;  //this might not be needed ( and might be breaking things. Revisit)
                }
            }
            else output.quantity = ingredientA.quantity;  //add yield in, and recognize an order with still quanity     

            elapsedTime = 0f;
            status = MachineStatus.Unloading;
            
        }

    }

    public void isUnloading()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= cycleTime)
        {
            
            status = MachineStatus.Completed;
            elapsedTime = 0f;
        }
    }

    public bool isCompleted()
    {
        elapsedTime += Time.deltaTime;
        bool result = false;

        if (elapsedTime >= cycleTime)
        {

            status = MachineStatus.Idle;  //set to the next state
            elapsedTime = 0f;  //reset timer
            result = true;  //
        }

        return result;
    }

    public void isBroken()  //this isn't implemented yet
    {



    }

    public void isRepairing() //this isn't implemented yet
    {




    }



    void repair()
    {
        status = MACHINE_IN_MAINTENANCE;

        durability.max -= 1;
        if (durability.max < 1)
        {
            status = MACHINE_BROKEN;
        }
        else durability.current = durability.max;
    }
    public void reset()
    {
        elapsedTime = 0;
        status = MACHINE_IDLE;
        orderIndex = -1;
    }
    public float calculateOEE()
    {
        //Debug.Log("Are we calcuating OEE?");

        // Explicitly cast integers to float before division
        float availability = (float)durability.max / 2 * (1 + (float)durability.max);
        float performance = (float)batchSize / cycleTime;
        float tempOEE = availability * performance * ((float)Yield / 100);

        //Debug.Log("availability: " + availability);
        //Debug.Log("performance: " + performance);
        //Debug.Log("OEE: " + tempOEE);

        return tempOEE;
    }
    private string generateName()  //machine name generation v2
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

        int maxcycles = durability.max / 2 * (1 + durability.max);

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
   */