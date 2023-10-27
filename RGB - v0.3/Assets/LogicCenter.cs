using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LogicCenter : MonoBehaviour
{
    #region variables
    // Start is called before the first frame update
    public System.Random r;
    public int oreTokens = 10;

    public int selectedColor = 0;  //color being selected in the menu
    public int chosenColor = 0;  //color currently set as primary

    public int harvestCapacity = 10;

    public int runningIndex = 0;

    public float timer = 0;
    public float spawnRate = 1;

    //right panel
    public Text oreValueText;
    public Text redPixelText;
    public Text greenPixelText;
    public Text bluePixelText;
    public Text yellowPixelText;
    public Text magentaPixelText;
    public Text cyanPixelText;
    public Text whitePixelText;
    public Text chosenColorText;

    public GameObject SelectMenu;           //top bar
    public GameObject UIbackground;         //
    public GameObject CanvasGO;             //
    public GameObject ProductionPage;       //
    public GameObject newWorkOrderWindow;   //
    public GameObject machineObject;        // machine object?
    public GameObject machineryPage;        //machine page

    //machines page
    public GameObject[] machineEntry = new GameObject[12];
    public Text[] machineMenuNameText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuStatusText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuAssignementText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuDMDText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuBText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuCText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuYText = new UnityEngine.UI.Text[12];
    public Text[] machineOEEText = new UnityEngine.UI.Text[12];
    //machines page

    //Employee Page
    public GameObject[] employeeEntry = new GameObject[12];
    public Text[] employeeEntryNameText = new UnityEngine.UI.Text[12];
    public Text[] employeeJobText = new UnityEngine.UI.Text[12];
    public Text[] employeeStatusText = new UnityEngine.UI.Text[12];
    public Text[] employeeHobbyText = new Text[12];
    public Text[] employeeAgeText = new Text[12];
    
    //selected Employee
    public int selectedEmployeeIndex = -1;
    public int employeeOfTheMonthIndex = 0;
    public List<int> newHires = new List<int>();
    public Text selectedEmployeeName;
    public Text selectedEmployeeDetails;
    public int newEmployeeJob=1;
    //trade
    public Text[] tradeCadenceText = new Text[8];
    public Text[] tradeLengthText = new Text[8];
    public Text[] tradeSendText = new Text[8];
    public UnityEngine.UI.Image[] tradeSendIMG = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Image[] tradeRecieveIMG = new UnityEngine.UI.Image[8];
    public Text[] tradeRecieveText = new Text[8];

    public List<Trade> availableTrades = new List<Trade>();
    public GameObject[] tradeEntry = new GameObject[8];
    public List<Trade> activeTrades = new List<Trade>();
    //end trade

    //active trade
    public Text[] activeTradeSendText = new Text[8];
    public UnityEngine.UI.Image[] activeTradeSendIMG = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Image[] activeTradeRecieveIMG = new UnityEngine.UI.Image[8];
    public Text[] activeTradeRecieveText = new Text[8];
    public GameObject[] activeTradeEntry = new GameObject[8];
    //active trade end

    public List<Machine> machines = new List<Machine>();
    public List<Employee> employees = new List<Employee>();
    public int[] inventory = new int[8];
    public int[] history = new int[8];
    
    public GameObject[] events = new GameObject[8];
    public UnityEngine.UI.Text[] timestamps = new Text[8];
    public UnityEngine.UI.Image[] eventImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Text[] eventText = new UnityEngine.UI.Text[8];
    public DateTime currentTime;
    public String timeString;

    /// <summary>
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
    
    public GameObject workOrderEntry;
    public Text newWorkOrderButton;
    public Text newWorkOrderQuantityBigText;
    public int newWorkOrderColor;
    public int newWorkOrderQuantity = 1;

    float expenseElapsedTime = 0;

    public FlavorText ft = new();
    public ColorLibrary colorLib = new();


    private const int MACHINE_IDLE = 0;
    private const int MACHINE_LOADING = 1;
    private const int MACHINE_RUNNING = 2;
    private const int MACHINE_UNLOADING = 3;
    private const int MACHINE_COMPLETED = 4;
    private const int MACHINE_BROKEN = 5;
    private const int MACHINE_IN_MAINTENANCE = 6;
    private const int MACHINE_CHOKED = 7;


    public TextMeshProUGUI NewWorkOrderoutput;
    public TMP_InputField NewWorkOrderQuantityText;
    public UnityEngine.UI.Image newWorkOrderPixelImg;


    public float randomEventElapsedTime;
    public float randomEventTriggerTime;

    public Factory factory = new(1);

    public TabGroup tabgroup;

    public int distribution = 1; //not sure what this is doing
    public int lastChosenColor = 0; //not sure what this is doing either
    #endregion

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

        setupTrades(15);
        setupTradeMenu();

        setupQueue(25);
        setupProcessingMenu();

        setupRandomEvents();

        startingPage();
    }

    #endregion
    ///////////////////////////////////
    ///////////////////////////////////
    #region setup Functions
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
        for (int i = 0; i < inventory.Length; i++)
            inventory[i] = 0; //set everything to start as zero

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
    void setupTrades(int numberOfTrades)
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
        
        
        runMachines();   //these should live inside the above else statement and increment by 1 isntead of by timedelta.
        
        runEmployees();
        runExpenses();
        runTrades();

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
        int highestValue = Math.Max(Math.Max(inventory[1], inventory[2]), inventory[3]);

        int index=1;
        for (int i = 1; i < 4; i++)  //check the primaries
        {
            index = i;
            if (inventory[i] == highestValue)
            {
                inventory[i] -= c;
                break;
            }   
        }
        if (inventory[index] < 0)  //this is wrong but works for now.
            inventory[index] = 0;
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
            updateRed();
        }
        else if (min == inventory[2])
        {
            updateGreen();
        }
        else if (min == inventory[3])
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
        Debug.Log("JOB:"+i);
        newEmployeeJob = 1+i;
        Debug.Log("new Employee Job:" + newEmployeeJob);
    }

    public void hireEmployee()
    {
        employees.Add(new(newEmployeeJob));
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Machine Functions
    private void runMachines()  //could add an idle condition
    {

        for (int i = 0; i < machines.Count; i++)
        {
            if (machines[i].status == 1)
                machineIsLoading(i);
            if (machines[i].status == 2)
                machineIsRunning(i);
            if (machines[i].status == 3)  //unload the pixels to inventory
                machineIsUnloading(i);
            if (machines[i].status == 4)  //ideal result
                machineIsComplete(i);
            if (machines[i].status == 5)
                machineIsBroken(i);
            if (machines[i].status == 6)
                machineIsRepairing(i);
        }
        updateProductionUI();

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

            // Reset the elapsed time and stop processing
            m.runMachine();
            m.elapsedTime = 0f;

            // Signal that the output is ready
            updateEvents(5);
            m.status = MACHINE_UNLOADING;
        }
        machines[i] = m;
    }

    public void machineIsUnloading(int i)
    {
        Machine m = machines[i];

        m.elapsedTime += Time.deltaTime;

        if (m.elapsedTime >= m.cycleTime)  //finishes processing
        {
            workOrder wo = ProcessingQueue[m.orderIndex];
            int inventoryIndex = ProcessingQueue[m.orderIndex].c3index;

            inventory[inventoryIndex] += m.unloadMachine(); //unloads the production quantity to inventory. Could be added to have randomness.
            
            m.status = MACHINE_COMPLETED;
            m.elapsedTime = 0f;

            updateEvents(4);  //says job is processed
            ProcessingQueue[m.orderIndex] = wo;  //check it back into the data
        }
        machines[i] = m;
    }

    public void machineIsComplete(int i)
    {
        if (i < 0 || i >= machines.Count)
        {
            // Log an error or throw an exception
            return;
        }

        Machine m = machines[i];
        m.elapsedTime += Time.deltaTime;

        if (m.elapsedTime >= m.cycleTime)
        {
            int orderIndex = m.orderIndex;

            if (orderIndex < 0 || orderIndex >= ProcessingQueue.Count)
            {
                // Log an error or throw an exception
                return;
            }

            workOrder wo = ProcessingQueue[orderIndex];
            wo.quantity -= m.c3q;
            wo.isActive = false;

            if (wo.quantity > 0)
            {
                ProcessingQueue.Add(new workOrder(wo));
            }

            m.elapsedTime = 0;
            m.status = 0;
            m.orderIndex = -1;

            ProcessingQueue[orderIndex] = wo;
            machines[i] = m;

            ProcessingQueue.RemoveAt(orderIndex);

            for (int j = 0; j < machines.Count; j++)
            {
                Machine tempMachine = machines[j];
                if (tempMachine.orderIndex > orderIndex)
                {
                    tempMachine.orderIndex -= 1;
                    machines[j] = tempMachine; // Note the index change here
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
        int index =-1;

        for (int i = 0; i < productionEntry.Count; i++)  //set status based on the current machine status
        {
            if(i > -1 && i < ProcessingQueue.Count)  //if the orders count is less
            { 
                index = ProcessingQueue[i].machineIndex;

                if (index >= 0 && index < machines.Count)
                {
                    if (machines[index].status == MACHINE_LOADING) 
                    {
                        productionBarImg[i].color = Color.yellow;
                        productionStatusText[i].color = Color.green;
                        productionStatusText[i].text = ft.woStatuses[2];

                        //Debug.Log("productiont text: "+productionMachineText[i].text);
                        //Debug.Log("machine name: "+machines[i].name);

                        productionMachineText[i].text = machines[i].name;
                    }
                    else if (machines[index].status == MACHINE_RUNNING)
                    {
                        productionBarImg[i].color = Color.green;
                        productionStatusText[i].color = Color.white;
                        productionStatusText[i].text = ft.woStatuses[3];
                    }
                    else if (machines[index].status == MACHINE_UNLOADING)
                    {
                        productionBarImg[i].color = Color.white;
                        productionStatusText[i].color = Color.red;
                        productionStatusText[i].text = ft.woStatuses[4];

                    }
                    else if (machines[index].status == MACHINE_COMPLETED)
                    {
                            productionStatusText[i].text = ft.woStatuses[5];
                            productionBarImg[i].color = Color.white;
                            productionStatusText[i].color = Color.green;
                            productionMachineText[i].text = "";
                    }
                }
                else  //unassigned
                {
                    productionBarImg[i].color = Color.white;
                    productionStatusText[i].color = Color.black;
                    productionStatusText[i].text = ft.woStatuses[1];
                    //productionMachineText[i].text = "Not Assigned";
                    productionMachineText[i].text = "";
                }
            }
            else
            {
                productionBarImg[i].color = Color.white;
                productionStatusText[i].color = Color.black;
                productionStatusText[i].text = ft.woStatuses[7];
                //productionMachineText[i].text = "Not Assigned";
                productionMachineText[i].text = "";
            }
        }
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region New Work Order
    public void assignNewWorkOrderColor(int val)  //sets the new color
    {
        val += 4; //This accounts for the list only having some entries.
        newWorkOrderColor = val;

        updatePixelColor(newWorkOrderPixelImg, val);
    }

    public void updatePixelColor(UnityEngine.UI.Image i, int val)
    {
        if (val == 0)
        {
            i.color = Color.black;

        }
        else if (val == 1)
        {
            i.color = Color.red;

        }
        else if (val == 2)
        {
            i.color = Color.green;

        }
        else if (val == 3)
        {
            i.color = Color.blue;

        }
        else if (val == 4)
        {
            i.color = Color.yellow;

        }
        else if (val == 5)
        {
            i.color = Color.magenta;

        }
        else if (val == 6)
        {
            i.color = Color.cyan;

        }
        else if (val == 7)
        {
            i.color = Color.white;

        }
    }
    public void assignNewWorkOrderQuantity(String val)
    {
        bool isNum = int.TryParse(val,out newWorkOrderQuantity);
        if(isNum)
            newWorkOrderQuantity = int.Parse(val);

        if(val !=null)
            newWorkOrderQuantityBigText.text = val + "x";
       // NewWorkOrderQuantity.text = int.Parse(val);
    }

    public void makeNewWorkOrder()  //updates the button to be used in two ways.
    {

        if (newWorkOrderWindow.activeSelf)
        {
            newWorkOrderWindow.SetActive(false);

            newWorkOrderButton.text = "+";
            newWorkOrderButton.color = Color.black;
            newWorkOrderButton.transform.Rotate(0, 0, -45);
        }

        else
        {
            newWorkOrderButton.transform.Rotate(0,0,45);
            newWorkOrderButton.color = Color.red;
            newWorkOrderWindow.SetActive(true);
        }
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
                Debug.Log(t.elapsedTime);
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

    public void selectTrade(int i)  ///used for the button
    {
        
        if (availableTrades.Count > 0)
        {
            Trade t = availableTrades[i];

            t.isActive = true;
            activeTrades.Add(t);
            
            tradeEntry[i].GetComponent<UnityEngine.UI.Image>().color = Color.yellow;

            availableTrades[i]= t;
            availableTrades.RemoveAt(i);
        }
    }

    public void executeTrade(int i)  //i is the index of the trade being executed
    {
        Trade t = activeTrades[i];  //check trade out

        inventory[t.sendColor] -= t.sendQuantity;  //swap send
        inventory[t.recieveColor] += t.recieveQuantity; //swap recieve

        t.length -= 1;  //reduce the iterations by 1.
        activeTrades[i] = t;    //check back in


        if (activeTrades[i].length < 1)  //if it's run the length of it's trade, remove it from the queue
            activeTrades.RemoveAt(i);
        
        
    
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
    public void updateUI()  //this could be setup into 1 loop, maybe?
    {
        displayInventory();

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


        for (int i = 0; i < productionEntry.Count; i++)
        {
            if (i < ProcessingQueue.Count)
            {
                int outputIndex = ProcessingQueue[i].c3index;

                int ingredientAindex = ProcessingQueue[i].c1index;
                int ingredientBindex = ProcessingQueue[i].c2index;
                Debug.Log("ingredient A: "+ ProcessingQueue[i].c1index);
                Debug.Log("ingredient B: " + ProcessingQueue[i].c2index);

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
            String details = "ROLE: " + ft.factoryJobs[e.job] + "\nSTART DATE: 04/21/2011 \nCOMPENSATION: " + e.compensation.ToString() + "■\n\nAGE: " + e.age.ToString() +"\nBIRTHDATE: "+e.birthdate.ToString("MM/dd/yyyy")+
                "\nSUN SIGN: "+ft.zodiacSigns[e.sunSign]+"\nHOMETOWN: "+ ft.cities[e.hometown] + "\nHOBBY: " + ft.hobbies[e.hobby] + "\n\nSKILLS: \nSPEED: " + e.getSpeed().ToString() + "\nRELIABILITY: " + e.getReliability().ToString() + "\nINTELLIGENCE: " + e.getIntelligence().ToString();
        
            selectedEmployeeDetails.text = details;
        }
        else
        {
            selectedEmployeeName.text = "";
            selectedEmployeeDetails.text = "";
        }

        for(int i = 0;i < tradeEntry.Length; i++)
        {
            if(i < availableTrades.Count)
            {
                tradeCadenceText[i].text = availableTrades[i].cadence.ToString();
                tradeLengthText[i].text = availableTrades[i].length.ToString();
                tradeSendText[i].text = availableTrades[i].sendQuantity.ToString();
                updatePixelColor(tradeSendIMG[i], availableTrades[i].sendColor);
                updatePixelColor(tradeRecieveIMG[i], availableTrades[i].recieveColor);
                tradeRecieveText[i].text = availableTrades[i].recieveQuantity.ToString();
            }
            else
            {
                tradeCadenceText[i].text = "";
                tradeLengthText[i].text = "";
                tradeSendText[i].text = "";
                updatePixelColor(tradeSendIMG[i], 7);
                updatePixelColor(tradeRecieveIMG[i], 7);
                tradeRecieveText[i].text = "";
            }
        }

        for (int i = 0; i < activeTradeEntry.Length; i++)
        {
            if (i < activeTrades.Count)
            {

                activeTradeSendText[i].text = activeTrades[i].sendQuantity.ToString();
                updatePixelColor(activeTradeSendIMG[i], activeTrades[i].sendColor);
                updatePixelColor(activeTradeRecieveIMG[i], activeTrades[i].recieveColor);
                activeTradeRecieveText[i].text = activeTrades[i].recieveQuantity.ToString();
            }
            else
            {
                activeTradeSendText[i].text = "";
                updatePixelColor(activeTradeSendIMG[i], 7);
                updatePixelColor(activeTradeRecieveIMG[i], 7);
                activeTradeRecieveText[i].text = "";
            }
        }

    }
    public void updateEvents(int status)
    {
        
        for (int i = history.Length-1; i > 0; i--)  //moves everything down one
        {
            history[i] = history[i-1];
            timestamps[i].text = timestamps[i - 1].text;
        }
        
        history[0] = status;

        currentTime = DateTime.Now;
        timeString = currentTime.ToString("hh:MM:sstt");
        timestamps[0].text = timeString;
        
    }
    
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

    public void selectEmployee(int index)
    {
        selectedEmployeeIndex = index;

        for(int i = 0;i<employeeEntry.Length;i++)
        {
            if (i == index  && index < employees.Count)
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
    #region Production
    public void updateQueue()  // Pairs orders to machines // Could use an update if an order is too large.
    {
        int firstAvailableMachineIndex = FindFirstAvailableMachine();
        int firstUnassignedOrderIndex = FindFirstUnassignedOrder();

        if (firstAvailableMachineIndex != -1 && firstUnassignedOrderIndex != -1)
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

        if (c1Quantity == wo.quantity)  //it's a full order
        {
            wo.machineIndex = machineIndex; //sets the machine being used for the order.
            m.orderIndex = orderIndex;  //sets the order being run for the machine
            ProcessingQueue[orderIndex] = wo;

            wo.isActive = true;

            m.assignOrder(wo);
            inventory[m.c1] -= c1Quantity;
            inventory[m.c2] -= c2Quantity;
            m.loadMachine(c1Quantity, c2Quantity);

        }

        machines[machineIndex] = m;
        ProcessingQueue[orderIndex] = wo;
    }
    public void addNewWorkOrder()  //adds a new order to the queue
    {
        if (newWorkOrderColor == 4) //yellow
            ProcessingQueue.Add(new workOrder(newWorkOrderQuantity, 1, 2, newWorkOrderColor));
        if (newWorkOrderColor == 5) //magenta
            ProcessingQueue.Add(new workOrder(newWorkOrderQuantity, 2, 3, newWorkOrderColor));
        if (newWorkOrderColor == 6) //cyan
            ProcessingQueue.Add(new workOrder(newWorkOrderQuantity, 1, 3, newWorkOrderColor));
        if (newWorkOrderColor == 7) //white
        {

            switch (ft.rollDice(1, 3))  //randomly chooses which one you have to make
            {
                case 1:
                    ProcessingQueue.Add(new workOrder(5, 3, 4, newWorkOrderColor));
                    break;
                case 2:
                    ProcessingQueue.Add(new workOrder(5, 2, 5, newWorkOrderColor));
                    break;
                case 3:
                    ProcessingQueue.Add(new workOrder(5, 1, 6, newWorkOrderColor));
                    break;
            }

        }
    }
    private int DetermineQuantity(int componentIndex, int requiredQuantity)  //checks if there is enough to fill the full order or not.
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
    public void displayInventory()
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
            inventory[0] = 0;  //no carryover.
            oreValueText.text = inventory[0].ToString();
        }

        if (inventory[0] < harvestCapacity && chosenColor != 0)
        {
            inventory[0]+=distribution;

            distribution++;

            if (inventory[0]>10)
                inventory[0]=10;

            if (inventory[0] == harvestCapacity)  //warns when it is full.
            {
                oreValueText.color = Color.red;
                updateEvents(10);
            }
        }

    }
    public void harvest()
    {
        if(inventory[0] > 0) 
        {

            updateEvents(8);

            oreValueText.color = Color.white;


            switch (chosenColor)
            {
                case 1:  //red
                    inventory[1]+= inventory[0];
                    inventory[0]= 0;
                    redPixelText.text = inventory[1].ToString();
                    break;
                case 2:  //green
                    inventory[2] += inventory[0];
                    inventory[0] = 0;
                    greenPixelText.text = inventory[2].ToString();
                    break;
                case 3:  //blue
                    inventory[3] += inventory[0];
                    inventory[0] = 0;
                    bluePixelText.text = inventory[3].ToString();
                    break;
                default:
                    Debug.Log("not selected"); 
                    break;
            }   
        }
    }
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
        
    }
    #endregion
}