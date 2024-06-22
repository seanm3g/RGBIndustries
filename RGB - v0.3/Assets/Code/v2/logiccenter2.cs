using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logiccenter2 : MonoBehaviour
{
    #region variables
    
    public System.Random r;  //should get moved?
    public CanvasGrid cg;  //maybe gets moved?


    public InventoryController inventory = new();

    public MachineController machines = new ();
    public WorkorderController workorders = new ();
    public ContractController contracts = new();

    public TradeController trades = new ();
    
    public EmployeeController employees = new();
    public FactoryController factory = new ();
    public ExpensesController expenses = new();

    public UIController ui = new();
    public EventsController events = new();
    public FlavorText ft = new();

    #endregion

    #region starting functions
    void Start()
    {
        //setupGame(1,3,5,0,100);  //factory, machines, employees, Production Queue, starting quantity
        setupGame(3);
    }

    public void setupGame(int startMode)
    {
        int startingEmployees = 0;
        int startingMachines = 0;
        int startingQueue = 0;
        int startingContracts = 0;

        switch (startMode)
        {
            case 0:      //default no help
                startingEmployees = 0;
                startingMachines = 0;
                startingQueue = 0;
                startingContracts = 0;
                break;
            case 1:   //starting game
                startingEmployees = 0;
                startingMachines = 1;
                startingQueue = 10;
                startingContracts = 0;
                break;
            case 2:   //tower defense game
                startingEmployees = 2;
                startingMachines = 3;
                startingQueue = 25;
                startingContracts = 0;
                break;
            case 3:   //cheats game
                startingEmployees = 10;
                startingMachines = 10;
                startingQueue = 0;
                startingContracts = 0;
                break;
        }





        inventory.setup(10);
        machines.setup();
        workorders.setup();
        contracts.setup();

        trades.setup();
        employees.setup();
        factory.setup();
        expenses.setup();

        ui.setup();
        events.setup();
        ft.setup();

}


    #endregion

    void Update()
    {
        
    }
}
