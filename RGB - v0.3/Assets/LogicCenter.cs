using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
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
    public int oreTokens = 0;
    public int selectedColor = 0;  //color being selected in the menu
    public int chosenColor = 0;  //color currently set as primary
    public int harvestCapacity = 10;
    public float gameTimer = 0;
    public float spawnRate = 1;  //how frequently the game ticks
    #endregion

    #region INVENTORY UI
    public Text oreValueText;
    public Text redPixelText;
    public Text greenPixelText;
    public Text bluePixelText;
    public Text yellowPixelText;
    public Text magentaPixelText;
    public Text cyanPixelText;
    public Text whitePixelText;
    #endregion

    #region UI Game Objects
    public GameObject SelectMenu;           //top bar
    public GameObject ProductionPage;       //Production Page GO
    public GameObject newWorkOrderWindow;   //new Work order WIndow
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
    public Text[] machineMenuOeeText = new UnityEngine.UI.Text[12];
    public int selectedMachineIndex = -1;



    //machine CONSTANTS
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
    public int[] inventory = new int[8];
    #endregion

    #region eventlist

    public int[] history = new int[8]; //the recent 8 events.  // this would get larger if the game scales.
    public GameObject[] events = new GameObject[8];
    public UnityEngine.UI.Text[] timestamps = new Text[8];
    public UnityEngine.UI.Image[] eventEntryImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Text[] eventEntryText = new UnityEngine.UI.Text[8];
    public DateTime currentTime;
    public String timeString;
    #endregion

    #region production Queue
    public List<workOrder> ProductionQueue = new List<workOrder>();  //this is the production queue
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
    public TextMeshProUGUI NewWorkOrderOutput;
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

    public int distribution = 1; //not sure what this is doing
    public int lastChosenColor = 0; //not sure what this is doing either

    #region paint
    public int chosenPaintColor;

    public Text pictureStats;
    public Text[] hueQuantitiesText = new UnityEngine.UI.Text[8];

    public List<Contract> availableContracts = new List<Contract>();
    public List<Contract> activeContracts = new List<Contract>();
    public CanvasGrid canvasGrid;

    public GameObject paintCanvasGO;  //assigned in Unity
    public GameObject contractGO;  //assigned in Unity

    public GameObject[] contractEntry = new GameObject[8];
    public Text[] contractClientNameText = new UnityEngine.UI.Text[8];
    public Text[] contractNameText = new UnityEngine.UI.Text[8];
    public Text[] contractStatusText = new UnityEngine.UI.Text[8];
    public Text[] contractProgressText = new UnityEngine.UI.Text[8];

    #endregion

    #region costs
    float expenseElapsedTime = 0;
    
    #endregion

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Start
    
    void Start()
    {
        //setupGame(1,3,5,0,100);  //factory, machines, employees, Production Queue, starting quantity
        setupGame(3);
    }
    
    //public void setupGame(int factory,int machines, int employees, int queue, int inventory)
    public void setupGame(int startMode)
    {
        int inventory=0;
        int employees=0;
        int machines = 0;
        int queue = 0;
        int contracts = 0;
      
        switch(startMode)
        {
            case 0:      //default no help
                inventory = 0;
                employees = 0;
                machines = 0;
                queue = 0;
                contracts = 0;
                break;
            case 1:   //starting game
                inventory = 10;
                employees = 0;
                machines = 1;
                queue = 10;
                contracts = 0;
                break;
            case 2:   //queue game
                inventory = 50;
                employees = 2;
                machines = 3;
                queue = 25;
                contracts = 0;
                break;
            case 3:   //cheating game
                inventory = 100;
                employees = 10;
                machines = 10;
                queue = 0;
                contracts = 0;
                break;
        }
        
        setupMenu();
        setupInventory(inventory);
        startingPage();

        setupEmployees(employees);
        setupEmployeeMenu();

        setupMachines(machines);
        setupMachineMenu();

        setupTrades(5);
        setupTradeMenu();

        setupQueue(queue);
        setupProcessingMenu();

        setupRandomEvents();

        setupPainting();

        setupContracts(contracts);
        setupContractsMenu();
   
    }
    #endregion

    #region Setup Functions
    public void startingPage()  //sets the window that shows up when the game is run.
    {
        SelectMenu.SetActive(true);
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
                    ProductionQueue.Add(new workOrder(q, 1, 2, 4,4));  //make yellow
                    break;
                case 2:
                    ProductionQueue.Add(new workOrder(q, 1, 3, 5,4));  //makes magenta
                    break;
                case 3:
                    ProductionQueue.Add(new workOrder(q, 2, 3, 6,4)); //makes cyan
                    break;
                case 4:
                    switch (ft.rollDice(1, 3))
                    {
                        case 1:
                            ProductionQueue.Add(new workOrder(q, 3, 4, 7,4)); //white
                            break;
                        case 2:
                            ProductionQueue.Add(new workOrder(q, 2, 5, 7,4)); //white
                            break;
                        case 3:
                            ProductionQueue.Add(new workOrder(q, 1, 6, 7,4)); //white
                            break;
                    }
                    break;
            }
        }
    }
    public void setupInventory(int init)  //init the inventory
    {
        for (int i = 0; i < inventory.Length; i++)
        {
           // if (i < 4)
                inventory[i] = init; //set everything to start as zero
            //else inventory[i] = 0;
        }    
            
    }
    public void setupEmployees(int quantity)
    {
        for (int i = 0; i < quantity; i++)  //setup employees
        {
            employees.Add(new Employee(i));
        }
    }
    public void setupMachines(int quantity)
    {
        for (int i = 0; i < quantity; i++) //setup machines
            machines.Add(new Machine(ft.generateMachineName(), 1, ft.rollDice(3, 6), ft.rollDice(3, 6), ft.rollDice(2, 6) + 1, 103 - ft.rollDice(3, 6)));

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

        GameObject eve = Resources.Load<GameObject>("Prefabs/event");  //creates an object of a prefab to be instantiated for each panel.

        for (int i = 7; i >= 0; i--)  //INITALIZE THE OBJECTS TO INTERACT WITH FOR THE EVENTLIST
        {
            events[i] = Instantiate(eve, new Vector3(0f, 0f, 0f), Quaternion.identity);  //instanties each element of the panel array.
            events[i].transform.SetParent(eventslayout.transform, false);   //sets the parent to fit into the eventslayout place

        }

        for (int i = 0; i < 8; i++)  //SETUP EVENTLIST
        {



            timestamps[i] = events[i].transform.Find("time").GetComponent<Text>(); //set the timestamp from panels
            eventEntryText[i] = events[i].transform.Find("bg/message").gameObject.GetComponent<Text>(); //set the text from panels
            eventEntryImg[i] = events[i].transform.Find("bg").GetComponent<UnityEngine.UI.Image>(); //set the
        }

        newWorkOrderWindow = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/new order"); 

    }
    public void setupProcessingMenu()
    {

        GameObject productionlayout = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/Production layout");

        if (productionlayout == null)
        {
            Debug.LogError("Could not find GameObject named 'productionlayout'");
            return;
        }

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
        NewWorkOrderOutput = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/new order/COLOR/Dropdown").GetComponent<TextMeshProUGUI>();
        NewWorkOrderQuantityText = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/new order/Quantity/QuantityText").GetComponent<TMP_InputField>();
        newWorkOrderPixelImg = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/new order/25X/CHOSEN COLOR").GetComponent<UnityEngine.UI.Image>();

    }
    public void setupMachineMenu()
    {

        if (machineryPage == null)
        {
            Debug.LogError("Could not find GameObject named 'machineryPage'");
            return;
        }
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
                machineMenuOeeText[i] = machineEntry[i].transform.Find("OEE").GetComponent<Text>();
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
            hueQuantitiesText[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/OEE Page/PAINT CANVAS/paint canvas/quantity/Text (Legacy) ("+index+")").GetComponent<Text>();
            hueQuantitiesText[i].text = "0x";
        }

        pictureStats = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/OEE Page/PAINT CANVAS/paint canvas/TOTAL").GetComponent<Text>();
        paintCanvasGO = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/OEE Page/PAINT CANVAS");
        contractGO = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/OEE Page/CONTRACT");
    }

    public void setupContractsMenu()
    {


        for (int i = 0;i<contractEntry.Length;i++)
        {
            
            contractEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/OEE Page/CONTRACT/CONTRACT LIST/CONTRACT ENTRY ("+i+")");

            if (contractEntry[i] == null)
            {
                Debug.Log("This crashes!" + i);
            }
            else
            {

                contractClientNameText[i] = contractEntry[i].transform.Find("CLIENT NAME").GetComponent<Text>();
                contractNameText[i] = contractEntry[i].transform.Find("CONTRACT NAME").GetComponent<Text>();
                contractStatusText[i] = contractEntry[i].transform.Find("CONTRACT STATUS").GetComponent<Text>();
                contractProgressText[i] = contractEntry[i].transform.Find("CONTRACT PROGRESS").GetComponent<Text>();
            }

        }

    }

    public void setupContracts(int startingContracts)
    {
        for (int i = 0; i < startingContracts; i++)
        {
            Contract c = new Contract();
            activeContracts.Add(c);
        }

    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        if (gameTimer < spawnRate)
        {
            gameTimer += Time.deltaTime;
        }
        else
        {
            distributeTokens();
            gameTimer = 0f;
            
        }

        //these should live inside the above else statement and increment by 1 isntead of by timedelta.
        runMachines();   
        runEmployees();
        runExpenses();

        runTrades();
        runMarket();
        runContracts();

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

            payBill(employeeExpenses);
        }
    }

    public void factoryCosts()
    {
        factory.elapsedTime += Time.deltaTime;

        if (factory.elapsedTime > 60)  //pay rent every minute
        {
            int highestValue = Math.Max(Math.Max(inventory[1], inventory[2]), inventory[3]);

            factory.elapsedTime = 0f;

            payBill(factory.upkeep);
        }
    }

    public void payBill(int c)
    {
        int highestValueIndex = highestQuantityColorIndex();

        inventory[highestValueIndex] -= c;
        
        if (inventory[highestValueIndex] < 0)  //this is wrong but works for now.
            inventory[highestValueIndex] = 0;
    }

    public int highestQuantityColorIndex()
    {

        int highestValueQuantity = Math.Max(Math.Max(inventory[1], inventory[2]), inventory[3]);
        int highestValueIndex = 0;

        for (int i = 1; i < 4; i++)  //check the primaries
        {
            if (inventory[i] == highestValueQuantity)
                highestValueIndex = i;
        }

        return highestValueIndex;

    }

    public int calculateExpenses()
    {
        int totalExpenses = 0;
        int employeeExpenses = calculateEmployeeExpenses();
        updateEvents(16);

        expenseElapsedTime = 0f;
        int index = Math.Max(Math.Max(inventory[1], inventory[2]), inventory[3]);
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

    public int maxInventorySlot()
    {
        int max = Math.Max(Math.Max(inventory[1], inventory[2]), inventory[3]);
        return max;
    }

    public int minInventorySlot()
    {
        int min = Math.Min(Math.Min(inventory[1], inventory[2]), inventory[3]);
        return min;
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
        int min = Math.Min(Math.Min(inventory[1], inventory[2]), inventory[3]);

        if (inventory[0] >= harvestCapacity)
            if(competance(e))
            {
                employeeSelectColor(min);
                harvest();    //if they have initiatve
                return;
            }
    }
    private void employeeSelectColor(int min)
    {
        if (min == inventory[1])
        {
            selectColor(1);
        }
        else if (min == inventory[2])
        {
            selectColor(2);
        }
        else if (min == inventory[3])
        {
            selectColor(3);
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
    private void runMachines()
    {
        updateProductionUI();

        for (int i = 0; i < machines.Count; i++)
        {
            Machine machine = machines[i];  //check out

            switch (machine.status)
            {
                
                case 1:
                    machineIsLoading(ref machine);
                    break;
                case 2:
                    machineIsRunning(ref machine);
                    break;
                case 3:
                    machineIsUnloading(ref machine);
                    break;
                case 4:
                    machineIsComplete(ref machine);
                    break;
                case 5:
                    machineIsBroken(ref machine); // Assuming you have a method for this
                    break;
                case 6:
                    machineIsRepairing(ref machine); // Assuming you have a method for this
                    break;
            }

            machines[i] = machine;   //check in
        }
    }
    public void machineIsLoading(ref Machine machine)
    {
        machine.elapsedTime += Time.deltaTime;

        if (machine.elapsedTime >= machine.cycleTime)

            Debug.Log("LOADING - ACTIVE CONTRACTS:" + activeContracts.Count);
        {
            machine.elapsedTime = 0f;
            machine.status = MACHINE_RUNNING; // Assuming MACHINE_RUNNING is a constant or static readonly field
        }
    }
    public void machineIsRunning(ref Machine machine)
    {
        machine.elapsedTime += Time.deltaTime;

        if (machine.elapsedTime >= machine.cycleTime)
        {
            machine.status = MACHINE_UNLOADING; // Assuming MACHINE_UNLOADING is a constant or static readonly field
            machine.runMachine(); // Assuming runMachine is a method that exists within the Machine struct
            machine.elapsedTime = 0f;
            updateEvents(5);
        }
    }
    public void machineIsUnloading(ref Machine machine)
    {
        machine.elapsedTime += Time.deltaTime;

        if (machine.elapsedTime >= machine.cycleTime)
        {
            workOrder order = ProductionQueue[machine.orderIndex];

            Debug.Log("UNLOADING - ACTIVE CONTRACTS:" + activeContracts.Count);

            workOrderDestination(ref order);
            

            //this might get cut and moved into workOrderOutput (likely)
           // int inventoryIndex = ProductionQueue[machine.orderIndex].c3index;
           // inventory[inventoryIndex] += machine.unloadMachine(); // Assuming unloadMachine is a method that exists within the Machine struct
            
            machine.status = MACHINE_COMPLETED; // Assuming MACHINE_COMPLETED is a constant or static readonly field
            machine.elapsedTime = 0f;

            ProductionQueue[machine.orderIndex] = order; //I'm not sure where this line of code goes yet.
        }
    }
    public void workOrderDestination(ref workOrder order)  //called whenever a workorder is unloading. It determines where it goes.
    {

        //what is the right sequence here of priority?  Should this be a setting the user gets?
        switch(order.destination)
        {
            case 1: checkWorkOrders(ref order); break;
            case 2: checkTrades(ref order); break;
            case 3: checkJobs(ref order); break;
        }
        if(order.quantity>0)  //if there is any leftovers put them in the inventory
            inventory[order.c3index] += machines[order.machineIndex].unloadMachine(); // Assuming unloadMachine is a method that exists within the Machine struct
    }
    public void checkWorkOrders(ref workOrder order) //THIS IS USED FOR SEQUENCING TOGETHER
    {
        // Iterate through the production queue to find work orders waiting for this color
        for (int i = 0; i < ProductionQueue.Count; i++)
        {
            workOrder queuedOrder = ProductionQueue[i];

            // Only proceed if the work order status indicates it's waiting for input
            if (queuedOrder.status == 5)
            {
                Debug.Log("There is a work order to be filled");
                // Determine the quantity that can be transferred based on the matching color index
                int transferQuantity = DetermineTransferQuantity(ref order, ref queuedOrder);

                // Add to queuedOrder and remove from order
                queuedOrder.quantity += transferQuantity;
                order.quantity -= transferQuantity;

                // Update the work order in the queue
                ProductionQueue[i] = queuedOrder;

                // If the current order is depleted, break out of the loop
                if (order.quantity <= 0) break;
            }
        }

        // If there's any remaining quantity, send it to inventory or next destination
        if (order.quantity > 0)
        {
            sendToInventory(ref order);
        }
    }
    private int DetermineTransferQuantity(ref workOrder order, ref workOrder queuedOrder) //support method for checkWorkOrders
    {
        // Check if the queued order requires the color from the current order
        if (order.c3index == queuedOrder.c1index)
        {
            return Math.Min(order.quantity, queuedOrder.requiredAQuantity - queuedOrder.currentAQuantity);
        }
        else if (order.c3index == queuedOrder.c2index)
        {
            return Math.Min(order.quantity, queuedOrder.requiredBQuantity - queuedOrder.currentBQuantity);
        }
        return 0; // No transfer needed if color indices don't match
    }
    public void checkTrades(ref workOrder order)  //need to re-write trades
    {
        // Iterate through active trades to find trades waiting for this color
        for (int i = 0; i < activeTrades.Count; i++)
        {
            Trade trade = activeTrades[i];
            // Assuming we match trades by color index and there's a quantity to send
            if (trade.sendColor == order.c3index && trade.sendQuantity > 0)
            {

                Debug.Log("There is a trade order to be filled");
                // Determine the quantity that can be transferred
                int transferQuantity = Math.Min(order.quantity, trade.sendQuantity);  //pull as much of the order as possible.

                trade.sendQuantity -= transferQuantity;
                order.quantity -= transferQuantity;

                // Update the trade
                activeTrades[i] = trade;

                // If the current order is depleted, break out of the loop
                if (order.quantity <= 0) break;
            }
        }

        // If there's any remaining quantity, send it to inventory or next destination
        if (order.quantity > 0)
        {
            sendToInventory(ref order);
        }
    }
    public void checkJobs(ref workOrder order)
    {

        if (activeContracts.Count >= 0)// Iterate through active contracts to find jobs waiting for this color
        for (int i = 0; i < activeContracts.Count; i++)
        {
            Contract contract = activeContracts[i];
            int index = order.c3index;
                // Check if the contract requires this color and has an outstanding quantity

                
                
                
                if (contract.requirements[index] > 0)
                {

                    // Determine the quantity that can be fulfilled
                    int fulfillQuantity = Math.Min(order.quantity, contract.requirements[index]);


                    contract.requirements[index] -= fulfillQuantity;
                    order.quantity -= fulfillQuantity;



                    activeContracts[i] = contract;  // Update the contract


                    if (contract.isComplete())
                    {
                        Debug.Log("Contract Finished");        /////////////////This needs work
                                                               // Contract fulfillment logic here
                        canvasGrid.SaveAsJPEG();
                        canvasGrid.clearCanvas();
                        activeContracts.RemoveAt(i);
                    }

                // If the current order is depleted, break out of the loop
                if (order.quantity <= 0) break;
            }
        }

        // If there's any remaining quantity, send it to inventory
        if (order.quantity > 0)
        {
            sendToInventory(ref order);
        }
    }
    private void sendToInventory(ref workOrder order)
    {
        // Add remaining quantity to inventory
        int index = order.c3index;
        inventory[index] += order.quantity;
        order.quantity = 0; // Clear the order quantity as it's now in inventory
    }
    public void machineIsComplete(ref Machine machine)
    {
        machine.elapsedTime += Time.deltaTime;

        if (machine.elapsedTime >= machine.cycleTime)
        {
            if (machine.orderIndex >= 0 && machine.orderIndex < ProductionQueue.Count)
            {
                workOrder wo = ProductionQueue[machine.orderIndex];
                // ... rest of the completion logic ...

                // Remove the completed order from the queue
                ProductionQueue.RemoveAt(machine.orderIndex);

                // Adjust the orderIndex for all machines since the queue has changed
                AdjustMachineOrderIndices(machine.orderIndex);

                // Reset the machine's orderIndex to indicate it's no longer linked to an order
                machine.orderIndex = -1;
            }
            else
            {
                // Handle invalid orderIndex case
            }

            // Reset machine status to idle
            machine.status = MACHINE_IDLE;
            machine.elapsedTime = 0f;
        }
    }
    private void AdjustMachineOrderIndices(int removedOrderIndex)
    {
        // Iterate over all machines to update their orderIndex if it's affected by the removal
        for (int i = 0; i < machines.Count; i++)
        {
            Machine currentMachine = machines[i];
            if (currentMachine.orderIndex > removedOrderIndex)
            {
                // Decrement the orderIndex to account for the removed order
                currentMachine.orderIndex--;
                machines[i] = currentMachine; // Save the updated machine back to the list
            }
        }
    }
    public void machineIsBroken(ref Machine machine) //not setup yet
    {



    }
    public void machineIsRepairing(ref Machine machine)  //not setup yet.
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

            if (i < ProductionQueue.Count)
            {
                int index = ProductionQueue[i].machineIndex;

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
        if (inventory[highestQuantityColorIndex()] > 25)
        {
            machines.Add(new Machine(ft.generateMachineName(), 1, ft.rollDice(3, 6), ft.rollDice(3, 6), ft.rollDice(3, 3) + 1, 103 - ft.rollDice(3, 6)));
            payBill(25); //right now machines cost 25 pixels each
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

            //payBill(-5);

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

        if (ProductionQueue.Count > 0 && HasActiveWorkOrders())
        {
            OffsetActiveWorkOrders();
        }

        // Create a new work order based on the color  ? means it might be null
        workOrder? newOrder = CreateWorkOrder(newWorkOrderColor, newWorkOrderQuantity);


        // Add the new work order to the front of the queue
        if (newOrder.HasValue)  // Check if newOrder is not null
        {
            ProductionQueue.Insert(0, newOrder.Value);  // Use .Value to get the underlying workOrder
        }


        CloseNewWorkOrderWindow();
    }

    public workOrder? CreateWorkOrder(int color, int quantity)
    {
        switch (color)
        {
            case 4:
                return new workOrder(quantity, 1, 2, color,4);
            case 5:
                return new workOrder(quantity, 1, 3, color,4);
            case 6:
                return new workOrder(quantity, 2, 3, color,4);
            case 7:
                int randomCase = ft.rollDice(1, 3);
                return new workOrder(5, randomCase == 1 ? 3 : randomCase == 2 ? 2 : 1, randomCase + 3, color,4);
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
        /*
        for (int i = 0; i < ProductionQueue.Count; i++)
        {
            workOrder wo = ProductionQueue[i];

            // If the wo is active, increment its orderIndex
            if (wo.isActive && wo.machineIndex >= 2)
            {
                wo.machineIndex--;
                ProductionQueue[i] = wo; // check in
            }
        }*/
    }


    private bool HasActiveWorkOrders()
    {
        return machines.Any(machine => machine.status > 0);
    }



    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Trade
    public void runTrades()
    {
        // Iterate in reverse to safely remove items without affecting next indices
        for (int i = activeTrades.Count - 1; i >= 0; i--)
        {
            Trade t = activeTrades[i]; // check out
            
            if (t.isActive)
            {

                t.elapsedTime += Time.deltaTime;
                
                activeTrades[i] = t;

                if (t.elapsedTime > t.cadence)
                {
                    t.elapsedTime = 0;
                    executeTrade(i);
                }
                
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
                    /*
                    // Ensure the range for rollDice starts after the selectedTrade and ends at the last index
                    int rangeStart = selectedTrade + 1;
                    int rangeEnd = maxSize; // No need to subtract selectedTrade here
                                            // Roll the dice for the range after the selected trade
                    int removeIndex = rangeStart + ft.rollDice(0, rangeEnd - rangeStart) - 1; // Adjusted range for dice roll
                    */
                    // Remove the trade at the calculated index
                    availableTrades.RemoveAt(ft.rollDice(0,maxSize)); // No need to subtract one, as removeIndex is already in the correct range
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
    public void acceptTrade()     // this is good clean code
    {
        // Check if a trade has been selected

        if (selectedTrade >= 0 && selectedTrade < availableTrades.Count)
        {
            
            Trade selected = availableTrades[selectedTrade];
            int tradeColorIndex = selected.sendColor;

            if (inventory[tradeColorIndex] > selected.sendQuantity)  //if there's enough for the trade it does it, otherwise it doesn't.
            {
                // Get the selected trade

                // Mark it as active
                selected.isActive = true;

                // Add it to the list of active trades
                activeTrades.Add(selected);

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
        }
        else
        {
            Debug.LogWarning("No trade selected or selected index is out of bounds.");
        }
    }
    public void executeTrade(int i) // i is the index of the trade being executed
    {
        Trade t = activeTrades[i]; // check trade out

        inventory[t.sendColor] -= t.sendQuantity; // swap send
        inventory[t.recieveColor] += t.recieveQuantity; // swap receive

        t.length -= 1; // reduce the iterations by 1.
        if (t.length < 1) // if it's run the length of its trade, remove it from the queue
        {
            activeTrades.RemoveAt(i);
        }
        else
        {
            activeTrades[i] = t; // check back in if not removed
        }
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region update UI
    public void updateUI()  //this could be re-written to update the one it's looking versus the entire list to speed things up.  But currently not needed
    {
        UpdateInventoryUI();
        UpdateEventUI();
        UpdateProductionUI();
        UpdateMachineUI();
        UpdateEmployeeUI();
        UpdateTradeUI();
        UpdateContractUI();
    }
    public void UpdateInventoryUI()
    {
        oreValueText.text = inventory[0].ToString() + "/" + harvestCapacity;


        redPixelText.text = inventory[1].ToString();
        greenPixelText.text = inventory[2].ToString();
        bluePixelText.text = inventory[3].ToString();
        yellowPixelText.text = inventory[4].ToString();
        magentaPixelText.text = inventory[5].ToString();
        cyanPixelText.text = inventory[6].ToString();
        whitePixelText.text = inventory[7].ToString();


    }
    public void UpdateEventUI()
    {
        for (int i = 0; i < history.Length; i++)
        {
            eventEntryText[i] = events[i].transform.Find("bg/message").gameObject.GetComponent<Text>();
            eventEntryImg[i] = events[i].transform.Find("bg").GetComponent<UnityEngine.UI.Image>();

            //update the sprite for each event type.

            eventEntryText[i].text = " " + ft.eventStatuses[history[i]].ToUpper() + " ";  // Assuming ft is an array or list

            switch (history[i])
            {
                case 1:
                    eventEntryImg[i].color = Color.red;
                    eventEntryText[i].color = Color.cyan;
                    break;
                case 2:  //chose green
                    eventEntryImg[i].color = Color.green;
                    eventEntryText[i].color = Color.magenta;
                    break;
                case 3:  //blue
                    eventEntryImg[i].color = Color.blue;
                    eventEntryText[i].color = Color.yellow;
                    break;
                case 4:  //halted machine
                    eventEntryImg[i].color = Color.red;
                    eventEntryText[i].color = Color.yellow;
                    break;
                case 5:  //running machine
                    eventEntryImg[i].color = Color.green;
                    eventEntryText[i].color = Color.white;
                    break;
                case 6:  //random event
                    eventEntryImg[i].color = Color.blue;
                    eventEntryText[i].color = Color.white;
                    break;
                case 7:  //machine is starved
                    eventEntryImg[i].color = Color.red;
                    eventEntryText[i].color = Color.yellow;
                    break;
                case 8: //harvesting
                    eventEntryImg[i].color = Color.green;
                    eventEntryText[i].color = Color.black;
                    break;
                case 9: //upgrade harvester
                    eventEntryImg[i].color = Color.yellow;
                    eventEntryText[i].color = Color.black;
                    break;
                case 10: //harvester at capacity
                    eventEntryImg[i].color = Color.black;
                    eventEntryText[i].color = Color.red;
                    break;
                case 11: // seasonal
                    eventEntryImg[i].color = Color.white;
                    eventEntryText[i].color = Color.black;
                    break;
                case 12:  //System
                    eventEntryImg[i].color = Color.white;
                    eventEntryText[i].color = Color.black;
                    break;
                case 15:  //System
                    eventEntryImg[i].color = Color.white;
                    eventEntryText[i].color = Color.white;
                    break;

            }
        }
    }
    public void UpdateProductionUI()
    {
        for (int i = 0; i < productionEntry.Count; i++)
        {
            if (i < ProductionQueue.Count)
            {
                productionEntry[i].SetActive(true);
                int outputIndex = ProductionQueue[i].c3index;

                int ingredientAindex = ProductionQueue[i].c1index;
                int ingredientBindex = ProductionQueue[i].c2index;
                //Debug.Log("ingredient A: "+ ProductionQueue[i].c1index);           RESUME HERE
                //Debug.Log("ingredient B: " + ProductionQueue[i].c2index);

                Color tempColor = new Color(colorLib.colors[outputIndex].r, colorLib.colors[outputIndex].g, colorLib.colors[outputIndex].b);
                Color ingredientA = new Color(colorLib.colors[ingredientAindex].r, colorLib.colors[ingredientAindex].g, colorLib.colors[ingredientAindex].b);
                Color ingredientB = new Color(colorLib.colors[ingredientBindex].r, colorLib.colors[ingredientBindex].g, colorLib.colors[ingredientBindex].b);

                productionPixelImg[i].color = tempColor;
                productionIngredientAImg[i].color = ingredientA;
                productionIngredientBImg[i].color = ingredientB;
                productionNameText[i].text = ProductionQueue[i].name;

                //if (!ProductionQueue[i].isActive)
                productionQuantityText[i].text = ProductionQueue[i].quantity.ToString();
                ///else productionQuantityText[i].text = ProductionQueue[i].quantity.ToString() + "(-" + machines[ProductionQueue[i].machineIndex].c2q +")";

            }
            else
            {
                productionEntry[i].SetActive(false);
            }

        }



    }
    public void UpdateMachineUI()
    {
        for (int i = 0; i < machineEntry.Length; i++)  //UPDATE MACHINES
        {
            if (i < machines.Count)
            {

                machineEntry[i].SetActive(true);
                Machine m = machines[i];
                //Debug.Log(m);

                machineMenuNameText[i].text = m.name;
                machineMenuStatusText[i].text = ft.machineStatuses[m.status];    /////////////

                if (m.orderIndex == -1)
                    machineMenuAssignementText[i].text = "not assigned";
                else
                    machineMenuAssignementText[i].text = ProductionQueue[m.orderIndex].name;

                machineMenuDMDText[i].text = m.durability + "/" + m.maxDurability;
                machineMenuBText[i].text = m.batchSize.ToString();
                machineMenuCText[i].text = m.cycleTime.ToString();
                machineMenuYText[i].text = m.Yield.ToString() + "%";
                machineMenuOeeText[i].text = m.OEE.ToString("F0");

            }
            else //unassigned
            {
                machineEntry[i].SetActive(false);
                machineMenuNameText[i].text = "";
                machineMenuStatusText[i].text = "";
                machineMenuAssignementText[i].text = "";
                machineMenuDMDText[i].text = "";
                machineMenuBText[i].text = "";
                machineMenuCText[i].text = "";
                machineMenuYText[i].text = "";
                machineMenuOeeText[i].text = "";

            }

        }
    }
    public void UpdateEmployeeUI()
    {
        for (int i = 0; i < employeeEntry.Length; i++)   //update employees
        {
            if (i < employees.Count)
            {
                employeeEntry[i].SetActive(true);
                String fullname = ft.lastNames[employees[i].lastName] + ", " + ft.firstNames[employees[i].firstName];
                employeeEntryNameText[i].text = fullname;
                employeeJobText[i].text = ft.factoryJobs[employees[i].job];
                employeeStatusText[i].text = ft.employeeStatus[employees[i].status];
                employeeHobbyText[i].text = ft.hobbies[employees[i].hobby];
                employeeAgeText[i].text = " " + employees[i].age.ToString();
            }
            else //unassigned
            {
                employeeEntry[i].SetActive(false);


            }
        }


        if (selectedEmployeeIndex < employees.Count)     //selected employee info
        {
            Employee e = employees[selectedEmployeeIndex];
            selectedEmployeeName.text = ft.firstNames[e.firstName] + " " + ft.lastNames[e.lastName];
            String details = "ROLE: " + ft.factoryJobs[e.job] + "\nSTART DATE: 04/21/2011 \nCOMPENSATION: " + e.compensation.ToString() + "■\n\nAGE: " + e.age.ToString() + "\nBIRTHDATE: " + e.birthdate.ToString("MM/dd") +
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
                tradeEntry[i].SetActive(true);
                tradeCadenceText[i].text = availableTrades[i].cadence.ToString();
                tradeLengthText[i].text = availableTrades[i].length.ToString();
                tradeSendText[i].text = availableTrades[i].sendQuantity.ToString();
                UpdateUIColor(tradeSendIMG[i], availableTrades[i].sendColor);
                UpdateUIColor(tradeRecieveIMG[i], availableTrades[i].recieveColor);
                tradeRecieveText[i].text = availableTrades[i].recieveQuantity.ToString();
            }
            else
            {
                tradeEntry[i].SetActive(false);

            }
        }

        for (int i = 0; i < activeTradeEntry.Length; i++)
        {
            if (i < activeTrades.Count)
            {
                activeTradeEntry[i].SetActive(true);
                activeTradeSendText[i].text = activeTrades[i].sendQuantity.ToString();
                UpdateUIColor(activeTradeSendIMG[i], activeTrades[i].sendColor);
                UpdateUIColor(activeTradeRecieveIMG[i], activeTrades[i].recieveColor);
                activeTradeRecieveText[i].text = activeTrades[i].recieveQuantity.ToString();
            }
            else
            {
                activeTradeEntry[i].SetActive(false); 
            }
        }
    }
    public void UpdateContractUI()
    {
        for(int i = 0;i<contractEntry.Length;i++)
        {
            if (i < activeContracts.Count)
            {
                contractEntry[i].SetActive(true);
                contractClientNameText[i].text = activeContracts[i].clientName.ToString();
                contractNameText[i].text = activeContracts[i].contractName.ToString();
                contractStatusText[i].text = activeContracts[i].status.ToString();
                //Debug.Log(activeContracts[i].progress);
                contractProgressText[i].text = (activeContracts[i].progress * 100).ToString("00.0") + "%";

            }
            else
            {
                contractEntry[i].SetActive(false);
            }
         
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
        for (int i = 0; i < ProductionQueue.Count; i++)
        {
            if (!ProductionQueue[i].isActive)
            {
                return i;
            }
        }
        return -1;
    }
    private void AssignOrderToMachine(int machineIndex, int orderIndex)
    {
        workOrder wo = ProductionQueue[orderIndex];
        Machine machine = machines[machineIndex];

        int c1Quantity = calculateAvailableQuantity(wo.c1index, wo.quantity); //Determine if there's enough in the inventory to run the order.
        int c2Quantity = calculateAvailableQuantity(wo.c2index, c1Quantity);
        c1Quantity = c2Quantity;

        if(machine.batchSize < wo.quantity) //This sets the number of cycles needed to produce the whole amount.
            while(machine.productionCycles*machine.batchSize<wo.quantity)
            {
                machine.productionCycles++;
            }

        if (c1Quantity == wo.quantity)  //it's a full order
        {
            wo.machineIndex = machineIndex; //sets the machine being used for the order.
            machine.orderIndex = orderIndex;  //sets the order being run for the machine

            ProductionQueue[orderIndex] = wo;

            wo.isActive = true;

            machine.assignOrder(wo);
            inventory[machine.c1] -= c1Quantity;
            inventory[machine.c2] -= c2Quantity;
            machine.loadMachine(c1Quantity, c2Quantity);

        }

        machines[machineIndex] = machine;
        ProductionQueue[orderIndex] = wo;
    }
    private int calculateAvailableQuantity(int componentIndex, int requiredQuantity)  //checks if there is enough to fill the full order or not.
    {
        int availableQuantity = inventory[componentIndex];
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
    public void selectWorkOrder(int index)  //used for the UI interacting with the elements.
    {
        // Reset all tradeEntry colors to white
        for (int i = 0; i < productionEntry.Count; i++)
        {
            productionBarImg[i].color = Color.white;
        }

        // If the selected index is within the bounds of availableTrades, proceed
        if (index >= 0 && index < ProductionQueue.Count)
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
    public void selectColor(int i)
    {
        selectedColor = i;

        tabgroup.updateColor();
    }
    public void chooseToken()
    {
        chosenColor = selectedColor;

        if(SelectMenu.activeSelf)
        {     
            SelectMenu.SetActive(false);
            ProductionPage.SetActive(true);
        }
        updateEvents(chosenColor);
    }
    public void updateToken()
    {
        chosenColor = selectedColor;

        tabgroup.updateColor();

    }
    public void updateSelectedColor(int i)
    {
        selectColor(i);
        updateToken();
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Harvesting
    public void distributeTokens()
    {
        ResetDistributionIfNeeded();
        IncrementAndCapInventory();
        CheckHarvestCapacity();
    }
    private void ResetDistributionIfNeeded()
    {
        if (lastChosenColor != chosenColor) // Reset if color has changed
        {
            distribution = 1; // Reset distribution
            lastChosenColor = chosenColor; // Update last chosen color
            inventory[0] = 0; // Reset ore inventory
            oreValueText.text = "0"; // Update text display
        }
    }
    private void IncrementAndCapInventory()
    {
        if (inventory[0] < harvestCapacity && chosenColor != 0)
        {
            inventory[0] = Math.Min(inventory[0] + distribution++, harvestCapacity);
            oreValueText.text = inventory[0].ToString(); // Update text display
        }
    }
    private void CheckHarvestCapacity()
    {
        if (inventory[0] == harvestCapacity) // Check if inventory is full
        {
            oreValueText.color = Color.red; // Change text color to red
            updateEvents(10); // Trigger full capacity event
        }
    }
    public void harvest()
    {
        if (inventory[0] > 0)
        {
            updateEvents(8);
            cg.available();
            cg.UpdateTexture();
            oreValueText.color = Color.white; // Reset text color

            TransferOreToColorInventory();
        }
    }
    private void TransferOreToColorInventory()  //what does this do?
    {
        if (chosenColor >= 1 && chosenColor <= 3)
        {
            inventory[chosenColor] += inventory[0];
            inventory[0] = 0;
            UpdatePixelText(chosenColor);
        }
        else
        {
            Debug.Log("Color not selected for harvesting");
        }
    }
    private void UpdatePixelText(int colorIndex)
    {
        switch (colorIndex)
        {
            case 1: redPixelText.text = inventory[1].ToString(); break;
            case 2: greenPixelText.text = inventory[2].ToString(); break;
            case 3: bluePixelText.text = inventory[3].ToString(); break;
        }
    }
    public void harvestUpgrade()
    {
        if (inventory[chosenColor] >= harvestCapacity)
        {
            updateEvents(9);
            oreValueText.color = Color.white; // Reset text color

            inventory[chosenColor] -= harvestCapacity;
            harvestCapacity++;
            UpdatePixelText(chosenColor); // Update text display based on color
        }
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region painting
    public void setPaintColor(int i)
    {
        chosenPaintColor = i;
    }
    public void convertToContract()
    {
        Contract c = new Contract();
        c.setRequirements(canvasGrid.pixelValues); // Assuming pixelValues is directly assignable to requirements


        processInventory(ref c); //does this assign it correctly?
        
        c.updateCurrentRequirements();  //this one isn't working, but the one inside is.

        c.updateCurrentCost();  //this doesn't.


        // Remove fully satisfied contracts

        if (c.totalRequirements > 0)
        {
            //Debug.Log("requirements are above 0");
            c.clientName = "SEANYE";
            c.contractName = "water bottle";
            c.status = 1;
            
            activeContracts.Add(c);
        }
        else canvasGrid.SaveAsJPEG();
    }

    // Helper method to process inventory against requirements
    private void processInventory(ref Contract c)  //should this get moved to the Contract class?
    {

        for (int i = 1; i < c.requirements.Length; i++)
        {
            if (c.requirements[i] <= inventory[i])   //if there is enough
            {
                // If inventory can cover the requirements, subtract and reset requirement
                inventory[i] -= c.requirements[i];
                c.requirements[i] = 0;

            }
            else  //if there is not enough
            {
                // If inventory can't cover, add a workOrder for the shortfall and reset inventory
                int shortfall = c.requirements[i] - inventory[i]; // Calculate the shortfall
                c.requirements[i] -= inventory[i]; // Requirement is set to zero as it will now be processed by the work order
                inventory[i] = 0; // Inventory is depleted for this item
                if(i>3)
                    ProductionQueue.Add(generateWorkOrder(shortfall, i)); // Add only the shortfall amount to the production queue
            }
        }
    }

    // Helper method to generate a work order based on shortfall
    private workOrder generateWorkOrder(int shortfall, int index)
    {
        // This method now only takes the shortfall, which is the amount needed to be produced
        switch (index)
        {
            case 4:
                return new workOrder(shortfall, 1, 2, 4, 3);
            case 5:
                return new workOrder(shortfall, 1, 3, 5, 3);
            case 6:
                return new workOrder(shortfall, 2, 3, 6, 3);
            case 7:
                int randomCase = ft.rollDice(1, 3);
                int param1 = randomCase == 1 ? 3 : randomCase == 2 ? 2 : 1;
                int param2 = randomCase + 3;
                return new workOrder(shortfall, param1, param2, 7, 3);
            default:
                // Handle default case or throw an exception if index is out of expected range
                throw new ArgumentException("Invalid index for work order generation");
        }
    }

    public void openCanvas()
    {
        paintCanvasGO.SetActive(true);
        contractGO.SetActive(false);
    }

    public void openContracts()
    {
        paintCanvasGO.SetActive(false);
        contractGO.SetActive(true);
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Contracts

    public void runContracts()  //check if there are contracts to update
    { 
        for(int i = 0;i<activeContracts.Count;i++)
        {
            Contract c = activeContracts[i];

            //c.requirements[i];
        }
    }

    public void addContract(Contract c)  //it doesn't seem like this gets called.
    {

        c.clientName = "SEANYEf";
        c.contractName = "water bottle";
        c.status = 1;
        c.progress = c.currentRequirements / c.totalRequirements;
        activeContracts.Add(c);
    }
    #endregion
}