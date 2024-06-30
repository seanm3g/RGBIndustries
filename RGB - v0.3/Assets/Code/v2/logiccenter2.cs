
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Jobs.LowLevel.Unsafe;

public class logiccenter2 : MonoBehaviour
{
    #region variables

    public float gameTimer = 0;
    public float spawnRate = 1;  //how frequently the game ticks
    int ticks;

    public System.Random r;  //should get moved?
    public CanvasGrid cg;  //maybe gets moved?
                           //


    #region CONTROLLERS
    public Inventory inventory;

    public harvestController harvest;

    public MachineController machines;
    public workOrderController workorders;
    public TradeController trades;
    public ContractController contracts;
    public EmployeeController employees;
    
    public FactoryController factory;
    public ExpensesController expenses;

    public UIController ui;
    public EventsController events;
    public FlavorText flavor;
    #endregion

    private Dictionary<string, ScheduledTask> systems = new Dictionary<string, ScheduledTask>();
    #endregion

    #region SETUP
    void Start()
    {
        //setupGame(1,3,5,0,100);  //factory, machines, employees, Production Queue, starting quantity
        setupGame(3);

    }

    public void setupGame(int startMode)
    {
        int startingEmployees = 0;  //people employeed
        int startingMachines = 0;   //machines owned
        int startingQueue = 0;      //work orders in queue
        int startingContracts = 0;  //starting contracts
        int startingCapacity = 0;   //harvest capacity

        switch (startMode)
        {
            case 0:   //default no help
                startingEmployees = 0;
                startingMachines = 0;
                startingQueue = 0;
                startingContracts = 0;
                startingCapacity = 1;
                break;
            case 1:   //starting game
                startingEmployees = 0;
                startingMachines = 1;
                startingQueue = 10;
                startingContracts = 0;
                startingCapacity = 1;
                break;
            case 2:   //tower defense game
                startingEmployees = 2;
                startingMachines = 3;
                startingQueue = 25;
                startingContracts = 0;
                startingCapacity = 5;
                break;
            case 3:   //cheats game
                startingEmployees = 10;
                startingMachines = 10;
                startingQueue = 0;
                startingContracts = 0;
                startingCapacity = 25;
                break;
        }

            ticks = 0;  //used to determine every certain number of seconds.
        

            inventory = new(10);
            harvest = new(startingCapacity);
            machines = new(startingMachines);
            workorders = new(startingQueue);
            trades = new();
            contracts = new(startingContracts);
            employees = new(startingEmployees);
            factory = new();
            expenses = new();
            ui = new();
            events = new();
            flavor = new();

            InitializeScheduledTasks();
    }

    #endregion

    #region UPDATE

    void Update()
    {

        if(gameTimer < spawnRate)  //this builds the core tick mechanic of the game.
        {
            gameTimer += Time.deltaTime;
        }
        else
        {
            
            runSystems();
            gameTimer = 0f;
        }

    }
    #endregion

    void runSystems()  //does ticks ever reset?  it should reset after the longest Task
    {
        ticks++;

        foreach (var task in systems.Values)
        {
            if (ticks - task.LastExecution >= task.Interval)
            {
                task.Task();
                task.LastExecution = ticks;
            }
        }
    }


    private void InitializeScheduledTasks()  //creates a task and how often that function happens.
    {

        systems.Add("employeesUpdate", new ScheduledTask(() => employees.update(), 1));

        systems.Add("harvestIncrement", new ScheduledTask(() => harvest.increment(), 1));
        systems.Add("machinesUpdate", new ScheduledTask(() => machines.update(Time.deltaTime), 1));
        systems.Add("workordersUpdate", new ScheduledTask(() => workorders.update(), 1));
        systems.Add("contractsUpdate", new ScheduledTask(() => contracts.update(), 1));
        systems.Add("tradesUpdate", new ScheduledTask(() => trades.update(), 1));



        systems.Add("factoryUpdate", new ScheduledTask(() => factory.update(), 1));
        systems.Add("expensesUpdate", new ScheduledTask(() => expenses.update(), 60));   //once a minute
        systems.Add("uiUpdate", new ScheduledTask(() => ui.update(), 1));
        systems.Add("eventsUpdate", new ScheduledTask(() => events.update(), 1));
    }





    /*
    public void harvestUpgrade() //used for reference
    {
        if (inventory.getEntry(chosenPaintColor2) >= harvestCapacity)  //if theres enough to pay for it.
        {
            updateEvents(9);
            oreValueText.color = Color.white; // Reset text color

            inventory.subtractPixels(chosenPaintColor2, harvestCapacity);  //pay the cost (your harvest capacity)
            harvestCapacity++;
            UpdatePixelText(chosenColor); // Update text display based on color
        }
    }*/


    


    

}

public class ScheduledTask  //used to organize what systems need to be run // This can be used in other classes too.
{
    public Action Task { get; private set; }
    public int Interval { get; private set; }
    public int LastExecution { get; set; }


    public ScheduledTask(Action task, int interval)
    {
        Task = task;
        Interval = interval;
        LastExecution = 0;
    }
}

public static class IDGenerator  //used to simplify identifying matching objects for pairing, etc.
{
    private static int currentID = 0;

    public static int getNextID()
    {
        return currentID++;
    }
}
