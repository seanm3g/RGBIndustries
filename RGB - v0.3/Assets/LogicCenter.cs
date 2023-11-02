﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LogicCenter : MonoBehaviour
{
    #region variables

    #region libraries
    public FlavorText ft = new();
    public ColorLibrary colorLib = new();
    public System.Random r;
    public CanvasGrid cg;
    #endregion

    #region basic functions

    /// </summary>
    public int oreTokens = 10;

    public int selectedColor = 0;  //color being selected in the menu
    public int chosenColor = 0;  //color currently set as primary

    public int harvestCapacity = 10;

    public int runningIndex = 0;

    public float timer = 0;
    public float spawnRate = 1;
    #endregion     //right panel

    #region basicUI
    public Text oreValueText;
    public Text redPixelText;
    public Text greenPixelText;
    public Text bluePixelText;
    public Text yellowPixelText;
    public Text magentaPixelText;
    public Text cyanPixelText;
    public Text whitePixelText;
    public Text chosenColorText;
    #endregion

    #region UI Game Objects
    public GameObject SelectMenu;           //top bar
    public GameObject UIbackground;         //
    public GameObject CanvasGO;             //
    public GameObject ProductionPage;       //
    public GameObject newWorkOrderWindow;   //
    public GameObject machineObject;        // machine object?
    public GameObject machineryPage;        //machine page

    public TabGroup tabgroup;               //used to control the top part
    #endregion

    #region machines

    public List<Machine> machines = new List<Machine>();

    public GameObject[] machineEntry = new GameObject[12];
    public Text[] machineMenuNameText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuStatusText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuAssignementText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuDMDText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuBText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuCText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuYText = new UnityEngine.UI.Text[12];
    public Text[] machineOEEText = new UnityEngine.UI.Text[12];
    public int selectedMachineIndex = -1;

    private const int MACHINE_IDLE = 0;
    private const int MACHINE_LOADING = 1;
    private const int MACHINE_RUNNING = 2;
    private const int MACHINE_UNLOADING = 3;
    private const int MACHINE_COMPLETED = 4;
    private const int MACHINE_BROKEN = 5;
    private const int MACHINE_IN_MAINTENANCE = 6;
    private const int MACHINE_CHOKED = 7;
    
    #endregion

    #region employees

    public List<Employee> employees = new List<Employee>();

    public GameObject[] employeeEntry = new GameObject[12];
    public Text[] employeeEntryNameText = new UnityEngine.UI.Text[12];
    public Text[] employeeJobText = new UnityEngine.UI.Text[12];
    public Text[] employeeStatusText = new UnityEngine.UI.Text[12];
    public Text[] employeeHobbyText = new Text[12];
    public Text[] employeeAgeText = new Text[12];
    #endregion

    #region selected Employee Box
    public int selectedEmployeeIndex = -1;
    public int employeeOfTheMonthIndex = 0;
    public List<int> newHires = new List<int>();
    public Text selectedEmployeeName;
    public Text selectedEmployeeDetails;
    public int newEmployeeJob=1;
    #endregion

    #region Trade
    public Text[] tradeCadenceText = new Text[8];
    public Text[] tradeLengthText = new Text[8];
    public Text[] tradeSendText = new Text[8];
    public UnityEngine.UI.Image[] tradeSendIMG = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Image[] tradeRecieveIMG = new UnityEngine.UI.Image[8];
    public Text[] tradeRecieveText = new Text[8];

    public List<Trade> availableTrades = new List<Trade>();
    public GameObject[] tradeEntry = new GameObject[8];

    public int selectedTrade;
    public float MarketElapsedTime;
    #endregion

    #region active trade
    public List<Trade> activeTrades = new List<Trade>();
    public Text[] activeTradeSendText = new Text[8];
    public UnityEngine.UI.Image[] activeTradeSendIMG = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Image[] activeTradeRecieveIMG = new UnityEngine.UI.Image[8];
    public Text[] activeTradeRecieveText = new Text[8];
    public GameObject[] activeTradeEntry = new GameObject[8];
    #endregion

    #region inventory
    //public int[] inventory = new int[8];
    public InventoryManager inv = new InventoryManager();
    #endregion

    #region eventlist

    public int[] history = new int[8]; //the recent 8 events.  // this would get larger if the game scales.
    public GameObject[] events = new GameObject[8];
    public UnityEngine.UI.Text[] timestamps = new Text[8];
    public UnityEngine.UI.Image[] eventImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Text[] eventText = new UnityEngine.UI.Text[8];
    public DateTime currentTime;
    public String timeString;
    #endregion

    #region production Queue
    public List<workOrder> ProcessingQueue = new List<workOrder>();  //this is the production queue
    public List<GameObject> productionEntry = new List<GameObject>();
    public UnityEngine.UI.Image[] productionBarImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Image[] productionPixelImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Image[] productionIngredientAImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Image[] productionIngredientBImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Text[] productionNameText = new UnityEngine.UI.Text[8];
    public UnityEngine.UI.Text[] productionQuantityText = new UnityEngine.UI.Text[8];
    public UnityEngine.UI.Text[] productionStatusText = new UnityEngine.UI.Text[8];
    public UnityEngine.UI.Text[] productionMachineText = new UnityEngine.UI.Text[8];
    public int selectedWorkOrderIndex = -1;
    #endregion

    #region new work order
    public TextMeshProUGUI NewWorkOrderoutput;
    public TMP_InputField NewWorkOrderQuantityText;
    public UnityEngine.UI.Image newWorkOrderPixelImg;

    public GameObject workOrderEntry;
    public Text newWorkOrderButton;
    public Text newWorkOrderQuantityBigText;
    public int newWorkOrderColor;
    public int newWorkOrderQuantity = 1;
    #endregion

    #region random events
    public float randomEventElapsedTime;
    public float randomEventTriggerTime;
    #endregion

    #region factory
    public Factory factory = new(1);
    #endregion

    #region harvesting
    public int distribution = 1; //not sure what this is doing
    public int lastChosenColor = 0; //not sure what this is doing either
    #endregion

    #region paint
    public int chosenPaintColor;

    public Text pictureStats;
    public Text[] hueQuantitiesText = new UnityEngine.UI.Text[8];

    public List<Contract> availableJobs = new List<Contract>();
    public List<Contract> ProductionContracts = new List<Contract>();
    #endregion

    #region costs
    float expenseElapsedTime = 0;
    public Text burnRateText;
    #endregion

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Start
    void Start()
    {
        setupGame();
    }
    public void setupGame()
    {

        setupInventory();
        setupMenu();

        setupEmployees(0);
        setupEmployeeMenu();

        setupMachines(5);
        setupMachineMenu();

        setupTrades(5);
        setupTradeMenu();

        setupQueue(0);
        setupProcessingMenu();

        setupRandomEvents();

        setupPainting();

        startingPage();
    }

    #endregion

    #region Setup Functions
    public void startingPage()  //sets the window that shows up when the game is run.
    {
        SelectMenu.SetActive(true);
        UIbackground.SetActive(true);
        ProductionPage.SetActive(false);
        newWorkOrderWindow.SetActive(false);
    }
    public void setupSystemFunctions()
    {
        r = new System.Random();
    }  //just sets up random for now.
    public void setupQueue(int quantity) //production queue
    {
        ColorRGB[] level1 = new ColorRGB[8];

        level1[1] = new ColorRGB(1, 0, 0);
        level1[2] = new ColorRGB(0, 1, 0);
        level1[3] = new ColorRGB(0, 0, 1);
        level1[4] = new ColorRGB(1, 1, 0);
        level1[5] = new ColorRGB(1, 0, 1);
        level1[6] = new ColorRGB(0, 1, 1);
        level1[7] = new ColorRGB(1, 1, 1);

        for (int i = 0; i < quantity; i++)
        {
            int r = ft.rollDice(1, 4);

            if (i < 6)
                r = ft.rollDice(1, 3);

            int q = ft.rollDice(2, 4);

            switch (r)
            {
                case 1:
                    ProcessingQueue.Add(new workOrder(q, 1, 2, 4));  //make yellow
                    break;
                case 2:
                    ProcessingQueue.Add(new workOrder(q, 1, 3, 5));  //makes magenta
                    break;
                case 3:
                    ProcessingQueue.Add(new workOrder(q, 2, 3, 6)); //makes cyan
                    break;
                case 4:
                    switch (ft.rollDice(1, 3))
                    {
                        case 1:
                            ProcessingQueue.Add(new workOrder(q, 3, 4, 7)); //white
                            break;
                        case 2:
                            ProcessingQueue.Add(new workOrder(q, 2, 5, 7)); //white
                            break;
                        case 3:
                            ProcessingQueue.Add(new workOrder(q, 1, 6, 7)); //white
                            break;
                    }
                    break;
            }
        }
    }
    public void setupInventory()  //init the inventory
    {
        inv.setupInventory(1);
        /*
        for (int i = 0; i < inventory.Length; i++)
            inventory[i] = 0; //set everything to start as zero
        */
    }
    public void setupEmployees(int quantity)
    {
        for (int i = 0; i < quantity; i++)  //setup employees
        {
            employees.Add(new Employee(i));
            //Debug.Log("Random birthday for a " + employees[i].age + "-year-old: " + employees[i].birthdate.ToShortDateString());
        }
    }
    public void setupMachines(int quantity)
    {
        for (int i = 0; i < quantity; i++) //setup machines
            machines.Add(new Machine(ft.generateMachineName(), 1, ft.rollDice(3, 6), ft.rollDice(3, 6), ft.rollDice(3, 3) + 1, 103 - ft.rollDice(3, 6)));

    }
    public void setupMenu()
    {
        tabgroup = GameObject.FindObjectOfType<TabGroup>();

        GameObject eventslayout = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/events page/eventslayout");

        if (eventslayout == null)
        {
            Debug.LogError("Could not find GameObject named 'eventslayout'");
            return;
        }

        GameObject productionlayout = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/Production layout");

        if (productionlayout == null)
        {
            Debug.LogError("Could not find GameObject named 'productionlayout'");
            return;
        }

        if (machineryPage == null)
        {
            Debug.LogError("Could not find GameObject named 'machineryPage'");
            return;
        }

        GameObject eve = Resources.Load<GameObject>("Prefabs/event");  //creates an object of a prefab to be instantiated for each panel.


        //makeMachine();

        for (int i = 7; i >= 0; i--)  //INITALIZE THE OBJECTS TO INTERACT WITH FOR THE EVENTLIST
        {
            events[i] = Instantiate(eve, new Vector3(0f, 0f, 0f), Quaternion.identity);  //instanties each element of the panel array.
            events[i].transform.SetParent(eventslayout.transform, false);   //sets the parent to fit into the eventslayout place

        }

        for (int i = 0; i < 8; i++)  //SETUP EVENTLIST
        {



            timestamps[i] = events[i].transform.Find("time").GetComponent<Text>(); //set the timestamp from panels
            eventText[i] = events[i].transform.Find("bg/message").gameObject.GetComponent<Text>(); //set the text from panels
            eventImg[i] = events[i].transform.Find("bg").GetComponent<UnityEngine.UI.Image>(); //set the
        }

        newWorkOrderWindow = GameObject.Find("Canvas/");

    }
    public void setupProcessingMenu()
    {
        setupNewWorkOrderMenu();

        for (int i = 0; i < productionEntry.Count; i++)
        {
            productionPixelImg[i] = productionEntry[i].transform.Find("image").GetComponent<UnityEngine.UI.Image>();
            productionIngredientAImg[i] = productionEntry[i].transform.Find("image/ingredient A").GetComponent<UnityEngine.UI.Image>();
            productionIngredientBImg[i] = productionEntry[i].transform.Find("image/ingredient B").GetComponent<UnityEngine.UI.Image>();
            productionNameText[i] = productionEntry[i].transform.Find("NAME").GetComponent<UnityEngine.UI.Text>();
            productionQuantityText[i] = productionEntry[i].transform.Find("QUANTITYNUM").GetComponent<UnityEngine.UI.Text>();
            productionStatusText[i] = productionEntry[i].transform.Find("STATUS").GetComponent<UnityEngine.UI.Text>();
            productionMachineText[i] = productionEntry[i].transform.Find("MACHINE NAME").GetComponent<UnityEngine.UI.Text>();
            productionBarImg[i] = productionEntry[i].transform.GetComponent<UnityEngine.UI.Image>();
        }
    }
    public void setupNewWorkOrderMenu()
    {
        newWorkOrderWindow = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/new order");
        newWorkOrderQuantityBigText = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/new order/25X").GetComponent<Text>();
        NewWorkOrderoutput = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/new order/COLOR/Dropdown").GetComponent<TextMeshProUGUI>();
        NewWorkOrderQuantityText = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/new order/Quantity/QuantityText").GetComponent<TMP_InputField>();
        newWorkOrderPixelImg = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/new order/25X/CHOSEN COLOR").GetComponent<UnityEngine.UI.Image>();

    }
    public void setupMachineMenu()
    {

        for (int i = 0; i < machineEntry.Length; i++)  //SETS UP MACHINE ENTRY
        {
            if (i == 0) machineEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Machinery Page/MACHINE LIST/MACHINE ENTRY");
            else machineEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Machinery Page/MACHINE LIST/MACHINE ENTRY (" + i + ")");

            if (machineEntry[i] == null)
                Debug.Log("This crashes!" + i);
            else
            {
                machineMenuNameText[i] = machineEntry[i].transform.Find("MACHINE NAME").GetComponent<Text>();
                machineMenuStatusText[i] = machineEntry[i].transform.Find("STATUS").GetComponent<Text>();
                machineMenuAssignementText[i] = machineEntry[i].transform.Find("ASSIGNMENT").GetComponent<Text>();
                machineMenuDMDText[i] = machineEntry[i].transform.Find("DURABILITY").GetComponent<Text>();
                machineMenuBText[i] = machineEntry[i].transform.Find("BATCH SIZE").GetComponent<Text>();
                machineMenuCText[i] = machineEntry[i].transform.Find("CYCLE TIME").GetComponent<Text>();
                machineMenuYText[i] = machineEntry[i].transform.Find("YIELD").GetComponent<Text>();
                machineOEEText[i] = machineEntry[i].transform.Find("OEE").GetComponent<Text>();
            }
        }

    }
    public void setupEmployeeMenu()
    {
        for (int i = 0; i < employeeEntry.Length; i++)  //SETS UP MACHINE ENTRY
        {
            if (i == 0) employeeEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Mgmt Page/EMPLOYEE FULL MENU/EMPLOYEE LIST/Employee Entry");
            else employeeEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Mgmt Page/EMPLOYEE FULL MENU/EMPLOYEE LIST/Employee Entry (" + i + ")");



            if (employeeEntry[i] == null)
                Debug.Log("This crashes!" + i);
            else
            {
                employeeEntryNameText[i] = employeeEntry[i].transform.Find("NAME").GetComponent<Text>();
                employeeJobText[i] = employeeEntry[i].transform.Find("JOB").GetComponent<Text>();
                employeeStatusText[i] = employeeEntry[i].transform.Find("STATUS").GetComponent<Text>();
                employeeHobbyText[i] = employeeEntry[i].transform.Find("HOBBY").GetComponent<Text>();
                employeeAgeText[i] = employeeEntry[i].transform.Find("AGE").GetComponent<Text>();
            }
        }

        selectedEmployeeName = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Mgmt Page/EMPLOYEE FULL MENU/Employees Feature/Employees Body/EMPLOYEE HEADER/Employee Name Banner").GetComponent<Text>();
        selectedEmployeeDetails = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Mgmt Page/EMPLOYEE FULL MENU/Employees Feature/Employees Body/Employee Details").GetComponent<Text>();
    }
    public void setupTradeMenu()
    {

        for (int i = 0; i < tradeEntry.Length; i++)  //SETS UP TRADE ENTRY
        {
            if (i == 0) tradeEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Trading Page/TRADE OFFERS/TRADE ENTRY");
            else tradeEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Trading Page/TRADE OFFERS/TRADE ENTRY (" + i + ")");

            if (tradeEntry[i] == null)
            {
                Debug.Log("This crashes!" + i);
            }
            else
            {

                tradeCadenceText[i] = tradeEntry[i].transform.Find("CADENCE").GetComponent<Text>();
                tradeLengthText[i] = tradeEntry[i].transform.Find("LENGTH NUM").GetComponent<Text>();
                tradeSendText[i] = tradeEntry[i].transform.Find("PIXEL 1/QUANTITY A").GetComponent<Text>();
                tradeSendIMG[i] = tradeEntry[i].transform.Find("PIXEL 1/Image").GetComponent<UnityEngine.UI.Image>();
                tradeRecieveIMG[i] = tradeEntry[i].transform.Find("PIXEL 2/Image").GetComponent<UnityEngine.UI.Image>();
                tradeRecieveText[i] = tradeEntry[i].transform.Find("PIXEL 2/QUANTITY A").GetComponent<Text>();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////active trade
        
        for (int i = 0; i < activeTradeEntry.Length; i++)  //SETS UP TRADE ENTRY
        {
            if (i == 0) activeTradeEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Trading Page/ACTIVE/Active Trade Entry");
            else activeTradeEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Trading Page/ACTIVE/Active Trade Entry (" + i + ")");

            if (activeTradeEntry[i] == null)
            {
                Debug.Log("This crashes!" + i);
            }
            else
            {
                activeTradeSendText[i] = activeTradeEntry[i].transform.Find("PIXEL 1/QUANTITY A").GetComponent<Text>();
                activeTradeSendIMG[i] = activeTradeEntry[i].transform.Find("PIXEL 1/Image").GetComponent<UnityEngine.UI.Image>();
                activeTradeRecieveIMG[i] = activeTradeEntry[i].transform.Find("PIXEL 2/Image").GetComponent<UnityEngine.UI.Image>();
                activeTradeRecieveText[i] = activeTradeEntry[i].transform.Find("PIXEL 2/QUANTITY A").GetComponent<Text>();
            }
        }
    }
    public void setupTrades(int numberOfTrades)
    {
        for (int i = 0; i < numberOfTrades; i++)
        {
            Trade newTrade = new Trade(5);
            availableTrades.Add(newTrade);
        }
    }
    public void setupRandomEvents()
    {
        randomEventElapsedTime = 0;
        randomEventTriggerTime = ft.rollDice(10, 10);
    }

    public void setupPainting()
    {
        for (int i = 0; i < hueQuantitiesText.Length; i++)
        {
            int index = hueQuantitiesText.Length - i - 1;
            hueQuantitiesText[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/OEE Page/paint canvas/quantity/Text (Legacy) ("+index+")").GetComponent<Text>();
            hueQuantitiesText[i].text = "0x";
        }

        pictureStats = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/OEE Page/paint canvas/TOTAL").GetComponent<Text>();
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            distributeTokens();
            timer = 0f;
            
        }
        runMachines();   //these should live inside the above else statement and increment by 1 instead of by timedelta.
        
        runEmployees();
        runExpenses();
        runTrades();
        runMarket();

        runRandomEvents();

        updateUI();

    }  
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region costs
    public void runExpenses()  //break expenses up
    {
        employeeCosts();
        factoryCosts();

    }

    public void employeeCosts()
    {
        expenseElapsedTime += Time.deltaTime;

        if (expenseElapsedTime > 60)  //pay employees every 10 seconds
        {
            expenseElapsedTime = 0f;

            int employeeExpenses = 0;

            for (int i = 0; i < employees.Count; i++)
                employeeExpenses += employees[i].compensation;
            
            updateEvents(16);

            inv.payBill(employeeExpenses);
        
        }
    }

    public void factoryCosts()
    {
        factory.elapsedTime += Time.deltaTime;

        if (factory.elapsedTime > 60)  //pay rent every minute
        {
            //int highestValue = Math.Max(Math.Max(inventory[1], inventory[2]), inventory[3]);

            factory.elapsedTime = 0f;

            inv.payBill(factory.upkeep);

        }
    }

    public int calculateExpenses()
    {
        int totalExpenses = 0;
        int employeeExpenses = calculateEmployeeExpenses();

        updateEvents(16);

        expenseElapsedTime = 0f;
        totalExpenses += employeeExpenses;
        totalExpenses += factory.upkeep;

        return totalExpenses;

    }

    public int calculateEmployeeExpenses()
    {
        int employeeExpenses = 0;

        for (int i = 0; i < employees.Count; i++)
            employeeExpenses += employees[i].compensation;

        return employeeExpenses;
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Events
    public void updateEvents(int status)
    {

        for (int i = history.Length - 1; i > 0; i--)  //moves everything down one
        {
            history[i] = history[i - 1];
            timestamps[i].text = timestamps[i - 1].text;
        }

        history[0] = status;

        currentTime = DateTime.Now;
        timeString = currentTime.ToString("hh:MM:sstt");
        timestamps[0].text = timeString;

    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Random Events
    public void runRandomEvents()
    {
        randomEventElapsedTime += Time.deltaTime;

        if (randomEventElapsedTime >= randomEventTriggerTime)
        {


            randomEventElapsedTime = 0f;
            updateEvents(6);

        }
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region employee agency
    private void runEmployees()
    {
        for (int i = 0; i < employees.Count; i++)
        {
            Employee e = employees[i];

            e.elapsedTime += Time.deltaTime;

            //Debug.Log("i:" +i+"\nElapsed Time: " + e.elapsedTime);
            if (e.elapsedTime >= 5 - e.getSpeed())
            {
                e.elapsedTime = 0f;

                if (competance(e))
                switch (e.job)  //job index
                {
                    case 0: break;
                    case 1: employeeHarvester(e); break;
                    case 2:employeeRunMachine(e); break;
                }
            }
            else 
            {
                e.status = 0;
            }
            employees[i] = e;
        }
    }
    private bool initiative(Employee e)
    {
        //Debug.Log("Does init function call?");
        if (e.elapsedTime >= 5-e.getSpeed())  //has initative every 5-speed seconds
        {
            e.elapsedTime = 0f;
            return true;
        }

        return false;
    }
    private bool competance(Employee e)
    {
        int reliability = e.getReliability();

        if (ft.skillCheck(reliability))
        {
            //Debug.Log(e.firstName+"did the thing!");
            return true;
        }
        return false;
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region employees
    private void employeeHarvester(Employee e)  // this is all of the things an employee can do, if any action is taken, the consume their action.
    {


        /*
        if (min > harvestCapacity)  //if the minimum value is higher than the capacity. (You have some of each)
            if (inventory[selectedColor] >= harvestCapacity && harvestCapacity < 10)  //update this until you hit 10.
            {
                if (competance(e))
                {
                    e.status = 1;
                    harvestUpgrade();
                    return; 
                }
            }
        */

        if (inv.getOre() >= 0)
            if(competance(e))
            {
                employeeSelectColor(inv.MinInventorySlot());
                harvest();    //if they have initiatve
                return;
            }


    }
    private void employeeSelectColor(String min)
    {
        if (min == inv.ColorToHex("Red"))
        {
            updateRed();
        }
        else if (min == inv.ColorToHex("Green"))
        {
            updateGreen();
        }
        else if (min == inv.ColorToHex("Blue"))
        {
            updateBlue();
        }


    }
    private void employeeRunMachine(Employee e)
    {
        if(competance(e))
            updateQueue();
    }

    public void fireEmployee() 
    {
        employees.RemoveAt(selectedEmployeeIndex); //there's probably a better way to do selectedEmployeeIndex with just passing the right value when a thing is selected.

    }

    public void performanceReview()  //doesn't do anything yet.
    {


    }
    
    public void pickEmployeeJob(int i)
    {
        //Debug.Log("JOB:"+i);
        newEmployeeJob = 1+i;
        //Debug.Log("new Employee Job:" + newEmployeeJob);
    }

    public void hireEmployee()
    {
        employees.Add(new(newEmployeeJob));
    }

    public void selectEmployee(int index)
    {
        selectedEmployeeIndex = index;

        for (int i = 0; i < employeeEntry.Length; i++)
        {
            if (i == index && index < employees.Count)
            {
                employeeEntry[i].GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
            }
            else
            {
                employeeEntry[i].GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Machine Functions
    private void runMachines()  //could add an idle condition
    {

        updateProductionUI();

        for (int i = 0; i < machines.Count; i++)
        {
            switch (machines[i].status)
            {
                case 1:  
                    machineIsLoading(i);break;
                case 2:
                    machineIsRunning(i); break;
                case 3:
                    machineIsUnloading(i); break;
                case 4:
                    machineIsComplete(i); break;
                case 5:
                    machineIsBroken(i); break;
                case 6:
                    machineIsRepairing(i); break;

            }
        }
        

    }

    public void machineIsLoading(int i)
    {
        Machine m = machines[i];

        m.elapsedTime += Time.deltaTime;

        if (m.elapsedTime >= m.cycleTime)  //finishes processing
        {
            // Reset the elapsed time and stop processing
            m.elapsedTime = 0f;

           // Debug.Log("but are we getting here?");
            // Signal that the output is ready
            m.status = MACHINE_RUNNING;
        }
        machines[i] = m;
    }

    public void machineIsRunning(int i)
    {
        Machine m = machines[i];
        m.elapsedTime += Time.deltaTime;

        if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
        {
            m.status = MACHINE_UNLOADING;
            // Reset the elapsed time and stop processing
            m.runMachine();
            m.elapsedTime = 0f;

            // Signal that the output is ready
            updateEvents(5);
            
        }
        machines[i] = m;
    }

    public void machineIsUnloading(int i)
    {
        Machine m = machines[i];
        m.elapsedTime += Time.deltaTime;

        if (m.elapsedTime >= m.cycleTime)
        {
            int inventoryIndex = ProcessingQueue[m.orderIndex].c3index;
            
            inv.AddToInventory(1,inv.intToColorKey(inventoryIndex),m.unloadMachine());

            m.status = MACHINE_COMPLETED;
            m.elapsedTime = 0f;
            //m.orderIndex = -1;  // Unlink the machine from the work order
        }
        machines[i] = m;
    }

    public void machineIsComplete(int i)
    {
        if (i < 0 || i >= machines.Count)
        {
            return;
        }

        Machine m = machines[i];
        m.elapsedTime += Time.deltaTime;

        if (m.elapsedTime >= m.cycleTime)
        {
            workOrder wo = ProcessingQueue[m.orderIndex];
            wo.quantity -= m.c3q;
            wo.isActive = false;

            if (wo.quantity > 0)
            {
                ProcessingQueue.Add(new workOrder(wo));
            }

            m.elapsedTime = 0;
            m.status = 0;  // Free the machine
            

            ProcessingQueue[m.orderIndex] = wo;

            ProcessingQueue.RemoveAt(m.orderIndex);

            m.orderIndex = -1;  // Unlink the machine from the work order
            machines[i] = m;

            // Adjust the orderIndex for all other machines
            for (int j = 0; j < machines.Count; j++)
            {
                Machine tempMachine = machines[j];
                if (tempMachine.orderIndex > m.orderIndex)
                {
                    tempMachine.orderIndex -= 1;
                    machines[j] = tempMachine;
                }
            }
        }
        machines[i] = m;
    }



    public void machineIsBroken(int i) //not setup yet
    {



    }

    public void machineIsRepairing(int i)  //not setup yet.
    {



    }


    public void updateProductionUI()
    {
        for (int i = 0; i < productionEntry.Count; i++)
        {
            Color barColor = Color.white;
            Color textColor = Color.black;
            string statusText = ft.woStatuses[7];
            string machineName = "";

            if (i < ProcessingQueue.Count)
            {
                int index = ProcessingQueue[i].machineIndex;

                if (index >= 0 && index < machines.Count)
                {
                    Machine machine = machines[index];
                    machineName = machine.name;

                    switch (machine.status)
                    {
                        case MACHINE_LOADING:
                            barColor = Color.yellow;
                            textColor = Color.green;
                            statusText = ft.woStatuses[2];
                            break;
                        case MACHINE_RUNNING:
                            barColor = Color.green;
                            textColor = Color.white;
                            statusText = ft.woStatuses[3];
                            break;
                        case MACHINE_UNLOADING:
                            barColor = Color.white;
                            textColor = Color.red;
                            statusText = ft.woStatuses[4];
                            break;
                        case MACHINE_COMPLETED:
                            barColor = Color.white;
                            textColor = Color.green;
                            statusText = ft.woStatuses[5];
                            machineName = "";
                            break;
                        default:
                            statusText = ft.woStatuses[1];
                            break;
                    }
                }
                else
                {
                    statusText = ft.woStatuses[1];
                }
            }

            UpdateUIElements(i, barColor, textColor, statusText, machineName);
        }
    }

    private void UpdateUIElements(int i, Color barColor, Color textColor, string statusText, string machineName)
    {
        productionBarImg[i].color = barColor;
        productionStatusText[i].color = textColor;
        productionStatusText[i].text = statusText;
        productionMachineText[i].text = machineName;
    }

    public void addNewMachine() //add new random machine
    {
        if (inv.valueAtSlot(inv.MaxInventorySlot()) > 25)  //Checks
        {
            machines.Add(new Machine(ft.generateMachineName(), 1, ft.rollDice(3, 6), ft.rollDice(3, 6), ft.rollDice(3, 3) + 1, 103 - ft.rollDice(3, 6)));
            inv.payBill(25); //right now machines cost 25 pixels each
        }
    }

    public void selectMachine(int index)
    {
        // Reset all tradeEntry colors to white
        for (int i = 0; i < machineEntry.Length; i++)
        {
            machineEntry[i].GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }

        // If the selected index is within the bounds of availableTrades, proceed
        if (index >= 0 && index < machines.Count)
        {
            // Highlight the selected tradeEntry
            machineEntry[index].GetComponent<UnityEngine.UI.Image>().color = Color.yellow;

            // Update the selectedTrade index
            selectedMachineIndex = index;
        }
        else
        {
            // Reset the selectedTrade index if the selected index is out of bounds
            selectedMachineIndex = -1;
        }
    }

    public void sellMachine()
    {
        // Check if a trade has been selected
        if (selectedMachineIndex >= 0 && selectedMachineIndex < machines.Count && machines[selectedMachineIndex].status == 0)
        {


            // Remove it from the list of available trades
            machines.RemoveAt(selectedMachineIndex);

            // Reset the selectedTrade index
            selectedMachineIndex = -1;
            inv.ShowInventoryStatus();
            inv.payBill(-5);

            // Reset all machine colors to white
            for (int i = 0; i < machineEntry.Length; i++)
            {
                machineEntry[i].GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }
        else
        {
            Debug.LogWarning("No trade selected or selected index is out of bounds.");
        }

        
    }

    public float machineAverage()  //This gives you the average rate of production per pixel based on the machines you have.
    {

        int runningCycleTotal = 0;
        int runningBatchTotal = 0;
        for (int i = 0; i < machines.Count; i++)
        {
            runningCycleTotal = machines[i].cycleTime * 3;
            runningBatchTotal = machines[i].batchSize;
        }

        float avgCycles = runningCycleTotal/machines.Count;
        float avgBatch = runningBatchTotal/machines.Count;

        return avgBatch/avgCycles;
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region New Work Order

    /// <summary>
    /// This section is for adding a new work order and displaying it.  Organized 10/30/2023
    /// </summary>
    /// <param name="val"></param>
    public void setNewWorkOrderColor(int val)  //sets the new color
    {
        val += 4; //This accounts for the list only having some entries.
        newWorkOrderColor = val;

        UpdateUIColor(newWorkOrderPixelImg, val);
    }

    public void UpdateUIColor(UnityEngine.UI.Image img, int val)
    {
        switch (val)
        {
            case 0: img.color = Color.black; break;
            case 1: img.color = Color.red; break;
            case 2: img.color = Color.green; break;
            case 3: img.color = Color.blue; break;
            case 4: img.color = Color.yellow; break;
            case 5: img.color = Color.magenta; break;
            case 6: img.color = Color.cyan; break;
            case 7: img.color = Color.white; break;
        }
    }
    public void SetNewWorkOrderQuantity(string val)
    {
        if (int.TryParse(val, out newWorkOrderQuantity))
        {
            newWorkOrderQuantityBigText.text = val + "x";
        }
    }

    public void ToggleNewWorkOrderWindow()  //updates the button to be used in two ways.
    {
        if (newWorkOrderWindow.activeSelf)
            CloseNewWorkOrderWindow();
        else
            OpenNewWorkOrderWindow();
    }

    public void OpenNewWorkOrderWindow()
    {
        newWorkOrderButton.transform.Rotate(0, 0, 45);
        newWorkOrderButton.color = Color.red;
        newWorkOrderWindow.SetActive(true);

    }
    public void CloseNewWorkOrderWindow()
    {
        newWorkOrderWindow.SetActive(false);

        newWorkOrderButton.text = "+";
        newWorkOrderButton.color = Color.black;
        newWorkOrderButton.transform.Rotate(0, 0, -45);

    }

    public void AddNewWorkOrderToQueue()
    {
        // Offset the queue if necessary

        if (ProcessingQueue.Count > 0 && HasActiveWorkOrders())
        {
            OffsetActiveWorkOrders();
        }

        // Create a new work order based on the color  ? means it might be null
        workOrder? newOrder = CreateWorkOrder(newWorkOrderColor, newWorkOrderQuantity);


        // Add the new work order to the front of the queue
        if (newOrder.HasValue)  // Check if newOrder is not null
        {
            ProcessingQueue.Insert(0, newOrder.Value);  // Use .Value to get the underlying workOrder
        }


        CloseNewWorkOrderWindow();
    }

    public workOrder? CreateWorkOrder(int color, int quantity)
    {
        switch (color)
        {
            case 4:
                return new workOrder(quantity, 1, 2, color);
            case 5:
                return new workOrder(quantity, 2, 3, color);
            case 6:
                return new workOrder(quantity, 1, 3, color);
            case 7:
                int randomCase = ft.rollDice(1, 3);
                return new workOrder(5, randomCase == 1 ? 3 : randomCase == 2 ? 2 : 1, randomCase + 3, color);
            default:
                return null;
        }
    }

    private void OffsetActiveWorkOrders()
    {
        for (int i = 0; i < machines.Count; i++)
        {
            Machine tempMachine = machines[i];

            // If the machine is actively working on a work order, increment its orderIndex
            if (tempMachine.status > 0 && tempMachine.orderIndex >= 0)
            {
                tempMachine.orderIndex++;
                machines[i] = tempMachine; // Update the original struct in the collection
            }
        }

        for (int i = 0; i < ProcessingQueue.Count; i++)
        {
            workOrder wo = ProcessingQueue[i];

            // If the machine is actively working on a work order, increment its orderIndex
            if (wo.isActive && wo.machineIndex >= 2)
            {
                wo.machineIndex--;
                ProcessingQueue[i] = wo; // Update the original struct in the collection
            }
        }
    }


    private bool HasActiveWorkOrders()
    {
        return machines.Any(machine => machine.status > 0);
    }



    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region trade

    public void runTrades()
    {
        for (int i = 0; i < activeTrades.Count; i++)
        {
            Trade t = activeTrades[i];  //check out 

            if (t.isActive)
            {
               
                t.elapsedTime += Time.deltaTime;
                //Debug.Log(t.elapsedTime);
                if (t.elapsedTime > t.cadence)
                {
                    t.elapsedTime = 0;

                    activeTrades[i] = t;
                    executeTrade(i);
                }
                else
                    activeTrades[i] = t;
            }
        }
    }

    public void runMarket()
    {
        MarketElapsedTime += Time.deltaTime;

        if (MarketElapsedTime > 2)
        {
            MarketElapsedTime = 0;
            int maxSize = availableTrades.Count;

            if (ft.rollDice(1, 2) == 2)
            {
                if (maxSize < 8)
                {
                    availableTrades.Add(new Trade(5));
                }
            }
            else
            {
                if (maxSize > 1)
                { 
                    int removeIndex = ft.rollDice(1, maxSize);
                    availableTrades.RemoveAt(removeIndex - 1);
                }
            }
        }
    }

    public void selectTrade(int index)  ///used for the button
    {
        // Reset all tradeEntry colors to white
        for (int i = 0; i < tradeEntry.Length; i++)
        {
            tradeEntry[i].GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }

        // If the selected index is within the bounds of availableTrades, proceed
        if (index >= 0 && index < availableTrades.Count)
        {
            // Highlight the selected tradeEntry
            tradeEntry[index].GetComponent<UnityEngine.UI.Image>().color = Color.yellow;

            // Update the selectedTrade index
            selectedTrade = index;
        }
        else
        {
            // Reset the selectedTrade index if the selected index is out of bounds
            selectedTrade = -1;
        }
    }

    public void acceptTrade()
    {
        // Check if a trade has been selected
        if (selectedTrade >= 0 && selectedTrade < availableTrades.Count)
        {
            // Get the selected trade
            Trade t = availableTrades[selectedTrade];

            // Mark it as active
            t.isActive = true;

            // Add it to the list of active trades
            activeTrades.Add(t);

            // Remove it from the list of available trades
            availableTrades.RemoveAt(selectedTrade);

            // Reset the selectedTrade index
            selectedTrade = -1;

            // Reset all tradeEntry colors to white
            for (int i = 0; i < tradeEntry.Length; i++)
            {
                tradeEntry[i].GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }
        else
        {
            Debug.LogWarning("No trade selected or selected index is out of bounds.");
        }
    }


    public void executeTrade(int i)  //i is the index of the trade being executed
    {
        Trade t = activeTrades[i];  //check trade out

        if(inv.executeTrade(t))
        {
            t.length -= 1;  //reduce the iterations by 1.

            activeTrades[i] = t;    //check back in

            if (activeTrades[i].length < 1)  //if it's run the length of it's trade, remove it from the queue
                activeTrades.RemoveAt(i);
        }        
    }
    public void setActiveTrade(int i)
    {
        activeTrades.Add(availableTrades[i]);

    }



    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void makeMachine()
    {
        Text[] machineAttributes = new Text[10];
        //This pulls the specific parent and sets it as the parent without having to create an object for it.
        GameObject machineObjectGO = Resources.Load<GameObject>("Resources/Prefabs/Machine info");
        //GameObject workOrderEntry = Resources.Load<GameObject>("Resources/Prefabs/Work Order Entry");


        machineObject = Instantiate(Resources.Load<GameObject>("Prefabs/Machine info"), new Vector3(0f, 0f, 0f), Quaternion.identity);
        machineObject.transform.SetParent(machineryPage.transform, false);

        machineAttributes[0] = machineObject.transform.Find("Layout/MACHINE").GetComponent<Text>();
        machineAttributes[1] = machineObject.transform.Find("Layout/Top Body/STATUS/STATUS info").GetComponent<Text>();  // IN PRODUCTION
        machineAttributes[5] = machineObject.transform.Find("Layout/Bottom Body/DURABILITY/durabilityValue").GetComponent<Text>();
        //machineAttributes[6] = machineObject.transform.Find("Layout/Bottom Body/DURABILITY/maxDurabilityValue").GetComponent<Text>();
        machineAttributes[7] = machineObject.transform.Find("Layout/Bottom Body/DURABILITY/cycleTimeValue").GetComponent<Text>();
        machineAttributes[8] = machineObject.transform.Find("Layout/Bottom Body/DURABILITY/batchSizeValue").GetComponent<Text>();
        machineAttributes[9] = machineObject.transform.Find("Layout/Bottom Body/DURABILITY/yieldValue").GetComponent<Text>();

        machineAttributes[0].text = machines[0].name.ToUpper();
        machineAttributes[5].text = machines[0].durability.ToString() + " / " + machines[0].maxDurability.ToString() + " CYCLES";
        //machineAttributes[6].text = machines[0].maxDurability.ToString()+ " CYCLES";
        machineAttributes[7].text = machines[0].cycleTime.ToString() + " MINUTES";
        machineAttributes[8].text = machines[0].batchSize.ToString() + " PIXELS".ToUpper();
        machineAttributes[9].text = machines[0].Yield.ToString() + "%";

    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region update UI

    public void updateUI()
    {//this could only re-write and update only the one it's looking at to speed things up.  But currently not needed

        displayInventory();

        UpdateEventUI();
        UpdateProductionUI();
        UpdateMachineUI();
        UpdateEmployeeUI();
        UpdateTradeUI();
    }
    public void displayInventory()
    {
        //oreValueText.text = inventory[0].ToString();
        //oreValueText.text = inventory[0].ToString() + "/" + harvestCapacity;   //OLD WAY OF DOING IT
        oreValueText.text = inv.valueAtSlot(inv.ColorToHex("Black")).ToString();
        redPixelText.text = inv.valueAtSlot(inv.ColorToHex("Red")).ToString();
        greenPixelText.text = inv.valueAtSlot(inv.ColorToHex("Green")).ToString();
        bluePixelText.text = inv.valueAtSlot(inv.ColorToHex("Blue")).ToString();
        yellowPixelText.text = inv.valueAtSlot(inv.ColorToHex("Yellow")).ToString();
        magentaPixelText.text = inv.valueAtSlot(inv.ColorToHex("Magenta")).ToString();
        cyanPixelText.text = inv.valueAtSlot(inv.ColorToHex("Cyan")).ToString();
        whitePixelText.text = inv.valueAtSlot(inv.ColorToHex("White")).ToString();

        burnRateText.text = calculateExpenses().ToString();


    }
    public void UpdateEventUI()
    {
        for (int i = 0; i < history.Length; i++)
        {
            eventText[i] = events[i].transform.Find("bg/message").gameObject.GetComponent<Text>();
            eventImg[i] = events[i].transform.Find("bg").GetComponent<UnityEngine.UI.Image>();

            //update the sprite for each event type.

            eventText[i].text = " " + ft.eventStatuses[history[i]].ToUpper() + " ";  // Assuming ft is an array or list

            switch (history[i])
            {
                case 1:
                    eventImg[i].color = Color.red;
                    eventText[i].color = Color.cyan;
                    break;
                case 2:  //chose green
                    eventImg[i].color = Color.green;
                    eventText[i].color = Color.magenta;
                    break;
                case 3:  //blue
                    eventImg[i].color = Color.blue;
                    eventText[i].color = Color.yellow;
                    break;
                case 4:  //halted machine
                    eventImg[i].color = Color.red;
                    eventText[i].color = Color.yellow;
                    break;
                case 5:  //running machine
                    eventImg[i].color = Color.green;
                    eventText[i].color = Color.white;
                    break;
                case 6:  //random event
                    eventImg[i].color = Color.blue;
                    eventText[i].color = Color.white;
                    break;
                case 7:  //machine is starved
                    eventImg[i].color = Color.red;
                    eventText[i].color = Color.yellow;
                    break;
                case 8: //harvesting
                    eventImg[i].color = Color.green;
                    eventText[i].color = Color.black;
                    break;
                case 9: //upgrade harvester
                    eventImg[i].color = Color.yellow;
                    eventText[i].color = Color.black;
                    break;
                case 10: //harvester at capacity
                    eventImg[i].color = Color.black;
                    eventText[i].color = Color.red;
                    break;
                case 11: // seasonal
                    eventImg[i].color = Color.white;
                    eventText[i].color = Color.black;
                    break;
                case 12:  //System
                    eventImg[i].color = Color.white;
                    eventText[i].color = Color.black;
                    break;
                case 15:  //System
                    eventImg[i].color = Color.white;
                    eventText[i].color = Color.white;
                    break;

            }
        }
    }
    public void UpdateProductionUI()
    {
        for (int i = 0; i < productionEntry.Count; i++)
        {
            if (i < ProcessingQueue.Count)
            {
                int outputIndex = ProcessingQueue[i].c3index;

                int ingredientAindex = ProcessingQueue[i].c1index;
                int ingredientBindex = ProcessingQueue[i].c2index;
                //Debug.Log("ingredient A: "+ ProcessingQueue[i].c1index);           RESUME HERE
                //Debug.Log("ingredient B: " + ProcessingQueue[i].c2index);

                Color tempColor = new Color(colorLib.colors[outputIndex].r, colorLib.colors[outputIndex].g, colorLib.colors[outputIndex].b);
                Color ingredientA = new Color(colorLib.colors[ingredientAindex].r, colorLib.colors[ingredientAindex].g, colorLib.colors[ingredientAindex].b);
                Color ingredientB = new Color(colorLib.colors[ingredientBindex].r, colorLib.colors[ingredientBindex].g, colorLib.colors[ingredientBindex].b);

                productionPixelImg[i].color = tempColor;
                productionIngredientAImg[i].color = ingredientA;
                productionIngredientBImg[i].color = ingredientB;
                productionNameText[i].text = ProcessingQueue[i].name;

                //if (!ProcessingQueue[i].isActive)
                productionQuantityText[i].text = ProcessingQueue[i].quantity.ToString();
                ///else productionQuantityText[i].text = ProcessingQueue[i].quantity.ToString() + "(-" + machines[ProcessingQueue[i].machineIndex].c2q +")";

            }
            else
            {
                productionMachineText[i].text = "";
                productionStatusText[i].text = "";
                productionPixelImg[i].color = Color.white;
                productionQuantityText[i].text = "";
                productionIngredientAImg[i].color = Color.white;
                productionIngredientBImg[i].color = Color.white;
            }

        }



    }
    public void UpdateMachineUI()
    {
        for (int i = 0; i < machineEntry.Length; i++)  //UPDATE MACHINES
        {
            if (i < machines.Count)
            {
                Machine m = machines[i];
                //Debug.Log(m);

                machineMenuNameText[i].text = m.name;



                machineMenuStatusText[i].text = ft.machineStatuses[m.status];    /////////////


                if (m.orderIndex == -1)
                    machineMenuAssignementText[i].text = "not assigned";
                else
                    machineMenuAssignementText[i].text = ProcessingQueue[m.orderIndex].name;

                machineMenuDMDText[i].text = m.durability + "/" + m.maxDurability;
                machineMenuBText[i].text = m.batchSize.ToString();
                machineMenuCText[i].text = m.cycleTime.ToString();
                machineMenuYText[i].text = m.Yield.ToString() + "%";
                machineOEEText[i].text = m.OEE.ToString("F0");

            }
            else //unassigned
            {
                machineMenuNameText[i].text = "";
                machineMenuStatusText[i].text = "";
                machineMenuAssignementText[i].text = "";
                machineMenuDMDText[i].text = "";
                machineMenuBText[i].text = "";
                machineMenuCText[i].text = "";
                machineMenuYText[i].text = "";
                machineOEEText[i].text = "";

            }

        }

    }
    public void UpdateEmployeeUI()
    {
        for (int i = 0; i < employeeEntry.Length; i++)   //update employees
        {
            if (i < employees.Count)
            {
                String fullname = ft.lastNames[employees[i].lastName] + ", " + ft.firstNames[employees[i].firstName];
                employeeEntryNameText[i].text = fullname;
                employeeJobText[i].text = ft.factoryJobs[employees[i].job];
                employeeStatusText[i].text = ft.employeeStatus[employees[i].status];
                employeeHobbyText[i].text = ft.hobbies[employees[i].hobby];
                employeeAgeText[i].text = " " + employees[i].age.ToString();
            }
            else //unassigned
            {
                employeeEntryNameText[i].text = "";
                employeeJobText[i].text = "";
                employeeStatusText[i].text = "";
                employeeHobbyText[i].text = "";
                employeeAgeText[i].text = "";

            }
        }


        if (selectedEmployeeIndex < employees.Count)     //selected employee info
        {
            Employee e = employees[selectedEmployeeIndex];
            selectedEmployeeName.text = ft.firstNames[e.firstName] + " " + ft.lastNames[e.lastName];
            String details = "ROLE: " + ft.factoryJobs[e.job] + "\nSTART DATE: 04/21/2011 \nCOMPENSATION: " + e.compensation.ToString() + "■\n\nAGE: " + e.age.ToString() + "\nBIRTHDATE: " + e.birthdate.ToString("M/dd") +
                "\nSUN SIGN: " + ft.zodiacSigns[e.sunSign] + "\nHOMETOWN: " + ft.cities[e.hometown] + "\nHOBBY: " + ft.hobbies[e.hobby] + "\n\nSKILLS: \nSPEED: " + e.getSpeed().ToString() + "\nRELIABILITY: " + e.getReliability().ToString() + "\nINTELLIGENCE: " + e.getIntelligence().ToString();

            selectedEmployeeDetails.text = details;
        }
        else
        {
            selectedEmployeeName.text = "";
            selectedEmployeeDetails.text = "";
        }


    }
    public void UpdateTradeUI()
    {
        for (int i = 0; i < tradeEntry.Length; i++)
        {
            if (i < availableTrades.Count)
            {
                tradeCadenceText[i].text = availableTrades[i].cadence.ToString();
                tradeLengthText[i].text = availableTrades[i].length.ToString();
                tradeSendText[i].text = availableTrades[i].sendQuantity.ToString();
                UpdateUIColor(tradeSendIMG[i], availableTrades[i].sendColor);
                UpdateUIColor(tradeRecieveIMG[i], availableTrades[i].recieveColor);
                tradeRecieveText[i].text = availableTrades[i].recieveQuantity.ToString();
            }
            else
            {
                tradeCadenceText[i].text = "";
                tradeLengthText[i].text = "";
                tradeSendText[i].text = "";
                UpdateUIColor(tradeSendIMG[i], 7);
                UpdateUIColor(tradeRecieveIMG[i], 7);
                tradeRecieveText[i].text = "";
            }
        }

        for (int i = 0; i < activeTradeEntry.Length; i++)
        {
            if (i < activeTrades.Count)
            {

                activeTradeSendText[i].text = activeTrades[i].sendQuantity.ToString();
                UpdateUIColor(activeTradeSendIMG[i], activeTrades[i].sendColor);
                UpdateUIColor(activeTradeRecieveIMG[i], activeTrades[i].recieveColor);
                activeTradeRecieveText[i].text = activeTrades[i].recieveQuantity.ToString();
            }
            else
            {
                activeTradeSendText[i].text = "";
                UpdateUIColor(activeTradeSendIMG[i], 7);
                UpdateUIColor(activeTradeRecieveIMG[i], 7);
                activeTradeRecieveText[i].text = "";
            }
        }


    }

    //
    public void oreValueTextColor()
    {
        switch (chosenColor)
        {
            case 1:
                oreValueText.color = new Color(1, 0, 0);
                break;
            case 2:
                oreValueText.color = new Color(0, 1, 0);
                break;
            case 3:
                oreValueText.color = new Color(0, 0, 1);
                break;
        }
        
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Production
    public void updateQueue()  // Pairs orders to machines // Could use an update if an order is too large.
    {
        int firstAvailableMachineIndex = FindFirstAvailableMachine();
        int firstUnassignedOrderIndex = FindFirstUnassignedOrder();

        if (firstAvailableMachineIndex != -1 && firstUnassignedOrderIndex != -1)  //if there's a match
        {
            AssignOrderToMachine(firstAvailableMachineIndex, firstUnassignedOrderIndex);
            
        }
    }
    private int FindFirstAvailableMachine()
    {
        for (int i = 0; i < machines.Count; i++)
        {
            if (machines[i].status == MACHINE_IDLE)
            {
                return i;
            }
        }
        return -1;
    }
    private int FindFirstUnassignedOrder()
    {
        for (int i = 0; i < ProcessingQueue.Count; i++)
        {
            if (!ProcessingQueue[i].isActive)
            {
                return i;
            }
        }
        return -1;
    }
    private void AssignOrderToMachine(int machineIndex, int orderIndex)
    {
        workOrder wo = ProcessingQueue[orderIndex];
        Machine m = machines[machineIndex];

        int c1Quantity = DetermineQuantity(wo.c1index, wo.quantity); //Determine if there's enough in the inventory to run the order.
        int c2Quantity = DetermineQuantity(wo.c2index, c1Quantity);
        c1Quantity = c2Quantity;

        if(m.batchSize < wo.quantity) //This sets the number of cycles needed to produce the whole amount.
            while(m.cycles*m.batchSize<wo.quantity)
            {
                m.cycles++;
            }

        if (c1Quantity == wo.quantity)  //it's a full order
        {
            wo.machineIndex = machineIndex; //sets the machine being used for the order.
            m.orderIndex = orderIndex;  //sets the order being run for the machine

            ProcessingQueue[orderIndex] = wo;

            wo.isActive = true;

            m.assignOrder(wo);
            //inventory[m.c1] -= c1Quantity;
            //inventory[m.c2] -= c2Quantity;

            inv.removeFromInventory(1,inv.ColorToHex(inv.intToColorKey(m.c1)), c1Quantity);
            inv.removeFromInventory(1,inv.ColorToHex(inv.intToColorKey(m.c2)), c2Quantity);
            m.loadMachine(c1Quantity, c2Quantity);

        }

        machines[machineIndex] = m;
        ProcessingQueue[orderIndex] = wo;
    }
   
    private int DetermineQuantity(int componentIndex, int requiredQuantity)  //checks if there is enough to fill the full order or not.
    {
        int availableQuantity = inv.valueAtSlot(inv.intToColorKey(componentIndex));

        //int availableQuantity = inventory[componentIndex];
        int quantityToUse;

        if (availableQuantity >= requiredQuantity)
        {
            quantityToUse = requiredQuantity;
        }
        else
        {
            quantityToUse = availableQuantity;
        }

        return quantityToUse;
    }

    public void selectWorkOrder(int index)
    {
        // Reset all tradeEntry colors to white
        for (int i = 0; i < productionEntry.Count; i++)
        {
            productionBarImg[i].color = Color.white;
        }

        // If the selected index is within the bounds of availableTrades, proceed
        if (index >= 0 && index < ProcessingQueue.Count)
        {
            // Highlight the selected tradeEntry
            productionBarImg[index].color = Color.yellow;

            // Update the selectedTrade index
            selectedWorkOrderIndex = index;
        }
        else
        {
            // Reset the selectedTrade index if the selected index is out of bounds
            selectedWorkOrderIndex = -1;
        }
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Token Selection
    public void selectRed()
    {
        selectedColor = 1;
    }
    public void selectGreen()
    {
        selectedColor = 2;
    }
    public void selectBlue()
    {
        selectedColor = 3;   
    }
    public void selectToken()
    {
        updateToken();

        SelectMenu.SetActive(false);
        UIbackground.SetActive(true);


        if (chosenColor == 1)
        {
            updateEvents(1);
        }
        else if (chosenColor == 2)
        {
            updateEvents(2);
        }

        else if (chosenColor == 3)
        {
            updateEvents(3);
        }

    }
    public void updateToken()
    {
        chosenColor = selectedColor;
        switch (chosenColor)
        {
            case 1:
                chosenColorText.text = "RED";
                chosenColorText.color = Color.red;
                break;
            case 2:
                chosenColorText.text = "GREEN";
                chosenColorText.color = Color.green;
                break;
            case 3:
                chosenColorText.text = "BLUE";
                chosenColorText.color = Color.blue;
                break; 
        }

    }
    public void updateRed()
    {
        selectedColor = 1;
        updateToken();
        tabgroup.updateColor();
       // updateEvents(1);
        
    }
    public void updateGreen()
    {
        selectedColor = 2;
        updateToken();
        tabgroup.updateColor();

        // updateEvents(2);

    }
    public void updateBlue()
    {
        selectedColor = 3;
        updateToken();
        tabgroup.updateColor();

        // updateEvents(3);    
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Harvesting
    public void distributeTokens()
    {
        if(lastChosenColor != chosenColor)//checks to see if it's changed since last time and if it has then reset the momentum;
        {
            distribution = 1;  //reset the distribution
            lastChosenColor=chosenColor;  //reset the color
            inv.resetOre();  //no carryover.

            oreValueText.text = inv.getOre().ToString();
        }


        Debug.Log("chosen color"+chosenColor);
        if (chosenColor != 0)
        {
            Debug.Log("do we ever get inside of this?" + chosenColor);
            inv.AddToInventory(1,"Black",distribution);

            distribution++;

            if (inv.getOre() > 10)
            {
                inv.maxOre();
                oreValueText.color = Color.red;
                //updateEvents(10);
            }
        }

    }
    public void harvest()
    {
        Debug.Log("Harvesting: "+inv.getOre());
        if(inv.getOre() > 0) 
        {

            updateEvents(8);
            //cg.available();
            cg.UpdateTexture();
            oreValueText.color = Color.white;


            switch (chosenColor)
            {
                case 1:  //red
                    inv.AddToInventory(1,"Red",inv.getOre());
                    inv.resetOre();
                    redPixelText.text = inv.key("Red").ToString();
                    break;
                case 2:  //green
                    inv.AddToInventory(1, "Green", inv.getOre());
                    inv.resetOre();
                    greenPixelText.text = inv.key("Green").ToString();
                    break;
                case 3:  //blue
                    inv.AddToInventory(1, "Blue", inv.getOre());
                    inv.resetOre();
                    bluePixelText.text = inv.key("Blue").ToString();
                    break;
                default:
                    Debug.Log("not selected"); 
                    break;
            }   
        }
    }

    /*
    public void harvestUpgrade()
    {

        int chosenQuant = inventory[chosenColor];
        
        if (chosenQuant >= harvestCapacity)
        {
            updateEvents(9);

            oreValueText.color = Color.white;

            inventory[chosenColor] -= harvestCapacity;
            harvestCapacity++;

            // Updates the text on screen
            if (chosenColor == 1) redPixelText.text = inventory[1].ToString();
            else if (chosenColor == 2) greenPixelText.text = inventory[2].ToString();
            else if (chosenColor == 3) bluePixelText.text = inventory[3].ToString();
         
        }
        
    }*/
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region painting

    public void setPaintColor(int i)
    {
        chosenPaintColor = i;

    }

    public void updateHueQuantities()
    {


    }


    #endregion
}