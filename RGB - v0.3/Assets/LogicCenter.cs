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
    // Start is called before the first frame update
    public System.Random r;
    public int oreTokens = 10;

    public int selectedColor = 0;  //color being selected in the menu
    public int chosenColor = 0;  //color currently set as primary

    public int harvestCapacity = 10;

    public int runningIndex = 0;

    public float timer = 0;
    public float spawnRate = 1;

    public Text oreValueText;
    public Text redPixelText;
    public Text greenPixelText;
    public Text bluePixelText;
    public Text yellowPixelText;
    public Text magentaPixelText;
    public Text cyanPixelText;
    public Text whitePixelText;

    public Text newWorkOrderButton;
    public Text newWorkOrderQuantityBigText;
    
    public Text availableMachinesText;
    public Text chosenColorText;

    //machines page
    public GameObject[] machineEntry = new GameObject[12];
    public Text[] machineMenuNameText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuStatusText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuAssignementText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuDMDText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuBText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuCText = new UnityEngine.UI.Text[12];
    public Text[] machineMenuYText = new UnityEngine.UI.Text[12];
    //machines page


    public GameObject SelectMenu;
    public GameObject UIbackground;
    public GameObject CanvasGO;
    public GameObject ProductionPage;
    public GameObject newWorkOrderWindow;


    public Machine[] machines = new Machine[12];
    public Employee[] employees = new Employee[12];
    public int[] inventory = new int[8];
    public int[] history = new int[8];

    public GameObject[] events = new GameObject[8];
    public UnityEngine.UI.Text[] timestamps = new Text[8];
    public UnityEngine.UI.Image[] eventImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Text[] eventText = new UnityEngine.UI.Text[8];


    /// <summary>
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
    ///

    public FlavorText ft = new FlavorText();

    public DateTime currentTime;
    String timeString;

    public GameObject machineObject;
    public GameObject machineryPage;

    public ColorLibrary colorLib = new ColorLibrary();

    public List<workOrder> ProcessingQueue = new List<workOrder>();  //this is the production queue


    private const int MACHINE_IDLE = 0;
    private const int MACHINE_LOADING = 1;
    private const int MACHINE_RUNNING = 2;
    private const int MACHINE_UNLOADING = 3;
    private const int MACHINE_COMPLETED = 4;
    private const int MACHINE_BROKEN = 5;
    private const int MACHINE_IN_MAINTENANCE = 6;
    private const int MACHINE_CHOKED = 7;


    public int newWorkOrderColor;
    public int newWorkOrderQuantity=1;

    public TextMeshProUGUI NewWorkOrderoutput;
    public TMP_InputField NewWorkOrderQuantityText;
    public UnityEngine.UI.Image newWorkOrderPixelImg;

    void Start()
    {
        r = new System.Random();

        SelectMenu.SetActive(true);
        UIbackground.SetActive(true);
        ProductionPage.SetActive(false);
        newWorkOrderWindow.SetActive(false);

        setupInventory();
        setupMenu();
        setupProcessingMenu();
        setupMachineMenu();
        setupQueue();

        
    }

    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            distributeTokens();
            timer = 0;
            
        }
        setText();
        
        updateMenu();
        runMachines();

    }
    /// <summary>
    /// machine function begin
    /// 
    /// </summary>

    private void runMachines()  //could add an idle condition
    {

        for (int i = 0; i < machines.Length; i++)
        {
            if (machines[i].status == 1)
                machineIsLoading(i);
            if (machines[i].status == 2)
                machineIsRunning(i);
            if (machines[i].status == 3)  //unload the pixels to inventory
                machineIsUnloading(i);
            if (machines[i].status == 4)
                machineIsComplete(i);
        }
        updateProductionUI();

    }

    public void machineIsLoading(int i)
    {

        machines[i].elapsedTime += Time.deltaTime;

        if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
        {
            // Reset the elapsed time and stop processing
            machines[i].elapsedTime = 0f;

           // Debug.Log("but are we getting here?");
            // Signal that the output is ready
            machines[i].status = MACHINE_RUNNING;
        }
    }

    public void machineIsRunning(int i)
    {
        machines[i].elapsedTime += Time.deltaTime;

        if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
        {

            // Reset the elapsed time and stop processing
            machines[i].runMachine();
            machines[i].elapsedTime = 0f;

            // Signal that the output is ready
            updateEvents(5);
            machines[i].status = MACHINE_UNLOADING;
        }

    }

    public void machineIsUnloading(int i)
    {
        machines[i].elapsedTime += Time.deltaTime;

        if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
        {
            workOrder wo = ProcessingQueue[machines[i].orderIndex];
            int inventoryIndex = ProcessingQueue[machines[i].orderIndex].c3index;
            int product = machines[i].unloadMachine();

            inventory[inventoryIndex] += product; //unloads the production quantity to inventory. Could be added to have randomness.
            machines[i].status = MACHINE_COMPLETED;
            machines[i].elapsedTime = 0f;

            updateEvents(4);  //says job is processed
            ProcessingQueue[machines[i].orderIndex] = wo;  //check it back into the data
        }
    }

    public void machineIsComplete(int i)
    {
        machines[i].elapsedTime += Time.deltaTime;



        if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
        {
            //check out
            Machine m = machines[i];
            int orderIndex = machines[i].orderIndex;
            workOrder wo = ProcessingQueue[orderIndex];

            Debug.Log("The order:" + orderIndex + " is being completed");

            //the work
            wo.quantity -= m.c3q;
            wo.isActive = false;

            if (wo.quantity > 0)
                ProcessingQueue.Add(new workOrder(wo));

            m.elapsedTime = 0;
            m.status = 0;
            m.orderIndex = -1;
            //m.reset();  //resets the machine

            //check in
            ProcessingQueue[orderIndex] = wo;
            machines[i] = m;
            Debug.Log(machines[i].status);
            Debug.Log(ProcessingQueue[orderIndex].name);

            //readyToRemove[i] = orderIndex;
            // Update orderIndex for all machines that are affected by the removal
            

            // Remove the work order from the queue
            ProcessingQueue.RemoveAt(orderIndex);
            for (int j = 0; j < machines.Length; j++)
            {
                if (machines[j].orderIndex > orderIndex)
                {
                    machines[j].orderIndex -= 1;
                }
            }
        }
    }
    
    public void updateProductionUI()
    {
        int index =-1;

        for (int i = 0; i < productionEntry.Count; i++)  //set status based on the current machine status
        {
            if(i < ProcessingQueue.Count)  //if the orders count is less
            { 
                index = ProcessingQueue[i].machineIndex;

                if (index >= 0 && index < machines.Length)
                {
                    if (machines[index].status == MACHINE_LOADING) 
                    {
                        productionBarImg[i].color = Color.yellow;
                        productionStatusText[i].color = Color.green;
                        productionStatusText[i].text = ft.woStatuses[2];
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

    /// <summary>
    /// / machine functions end
    /// </summary>
    /// 

    public void assignNewWorkOrderColor(int val)  //sets the new color
    {
        if (val == 0)
        {
            newWorkOrderPixelImg.color = Color.yellow;
            newWorkOrderColor = 4;
        }
        else if (val == 1)
        {
            newWorkOrderPixelImg.color = Color.magenta;
            newWorkOrderColor = 5;
        }
        else if (val == 2)
        {
            newWorkOrderPixelImg.color = Color.cyan;
            newWorkOrderColor = 6;
        }
        else if (val == 3) 
        {
            newWorkOrderPixelImg.color = Color.white;
            newWorkOrderColor = 7; 
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
    public void addNewWorkOrder()
    {
        if(newWorkOrderColor == 4) //yellow
            ProcessingQueue.Add(new workOrder(newWorkOrderQuantity, 1,2,newWorkOrderColor));
        if (newWorkOrderColor == 5) //magenta
            ProcessingQueue.Add(new workOrder(newWorkOrderQuantity, 2, 3, newWorkOrderColor));
        if (newWorkOrderColor == 6) //cyan
            ProcessingQueue.Add(new workOrder(newWorkOrderQuantity, 1, 3, newWorkOrderColor));
        if (newWorkOrderColor == 7) //white
        {

            switch(rollDice(1,3))  //randomly chooses which one you have to make
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
    
    public void setupQueue()
    {
        ColorRGB[] level1 = new ColorRGB[8];

        level1[1] = new ColorRGB(1,0,0);
        level1[2] = new ColorRGB(0,1,0);
        level1[3] = new ColorRGB(0,0,1);
        level1[4] = new ColorRGB(1,1,0);
        level1[5] = new ColorRGB(1,0,1);
        level1[6] = new ColorRGB(0,1,1);
        level1[7] = new ColorRGB(1,1,1);

        for(int i = 0; i < 15; i++)
        {      
            int r = rollDice(1, 4);

            if (i < 6)
                r = rollDice(1, 3);

            int q = rollDice(2, 4);

            switch (r)
            {
                case 1:
                    ProcessingQueue.Add(new workOrder(q,1,2,4));  //make yellow
                    break;
                case 2: 
                    ProcessingQueue.Add(new workOrder(q,1, 3, 5));  //makes magenta
                    break;
                case 3:
                    ProcessingQueue.Add(new workOrder(q,2, 3, 6)); //makes cyan
                    break;
                case 4:
                    switch (rollDice(1, 3))
                    {
                        case 1: ProcessingQueue.Add(new workOrder(q, 3, 4, 7)); //white
                            break;
                        case 2: ProcessingQueue.Add(new workOrder(q, 2, 5, 7)); //white
                            break;
                        case 3: ProcessingQueue.Add(new workOrder(q, 1, 6, 7)); //white
                            break;
                    }
                    break;
            }
        }
    }   //this initliaties the random beginning of the game

    public void updateQueue()  // Pairs orders to machines // Could use an update if an order is too large.
    {
        int firstAvailableMachineIndex = FindFirstAvailableMachine();
        int firstUnassignedOrderIndex = FindFirstUnassignedOrder();

        if (firstAvailableMachineIndex != -1 && firstUnassignedOrderIndex != -1)
        {
            AssignOrderToMachine(firstAvailableMachineIndex, firstUnassignedOrderIndex);
            updateEvents(4);
        }
        
    }

    private int totalAvailableMachines()
    {
        int total = 0;

        for(int i=0; i<machines.Length;i++)
            if (machines[i].status == 0)
                total++;

        return total;
    }

    private int FindFirstAvailableMachine()
    {
        for (int i = 0; i < machines.Length; i++)
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

        if(c1Quantity==wo.quantity)  //it's a full order
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

    public void setupInventory()
    {
        for(int i = 0; i < inventory.Length;i++)
            inventory[i] = 0;

        inventory[1] = 100;
        inventory[2] = 100;
        inventory[3] = 100;

        //Console.WriteLine("Setup run");

        for (int i = 0; i < employees.Length; i++)  //setup both things
        {
            employees[i] = new Employee(rollDice(1, 10), rollDice(1, 10), rollDice(1, 10), 16 + rollDice(1, 45), rollDice(1, 10), rollDice(1, 10));
            
        }
        for(int i = 0; i < machines.Length;i++)
            machines[i] = new Machine(ft.generateMachineName(), 1, rollDice(3, 6), 60 * rollDice(1, 6),rollDice(1,6), 103 - rollDice(3, 6));

    }
    
    public void setupMenu()
    {
        GameObject eventslayout = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/events page/eventslayout");
        
        if (eventslayout == null)
        {
            Debug.LogError("Could not find GameObject named 'eventslayout'");
            return;
        }

        GameObject productionlayout = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Production Page/Production layout");

        if(productionlayout == null)
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
           // Debug.Log(productionEntry[i]);
           // Debug.Log($"work order entry: {workOrderEntry}");
            
           

            timestamps[i] = events[i].transform.Find("time").GetComponent<Text>(); //set the timestamp from panels
            eventText[i] = events[i].transform.Find("bg/message").gameObject.GetComponent<Text>(); //set the text from panels
            eventImg[i] = events[i].transform.Find("bg").GetComponent<UnityEngine.UI.Image>(); //set the
        }
        /*
        for(int i = 0;i<machineEntry.Length; i++)  //SETS UP MACHINE ENTRY
        {
            Debug.Log("Do we get here?");
            if(i==0) machineEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Machinery Page/MACHINE LIST/MACHINE ENTRY");
            else machineEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Machinery Page/MACHINE LIST/MACHINE ENTRY (" + i+")");
            Debug.Log("But do we get here?");

            if (machineEntry[i] == null) { Debug.Log("This crashes!" + i); }
            else Debug.Log("This one worked!" + i);
            /*
            else
            {
                machineMenuNameText[i] = machineEntry[i].transform.Find("MACHINE ENTRY/MACHINE NAME").GetComponent<Text>();
                machineMenuStatusText[i] = machineEntry[i].transform.Find("STATUS").GetComponent<Text>();
                machineMenuAssignementText[i] = machineEntry[i].transform.Find("ASSIGNMENT").GetComponent<Text>();
                machineMenuDMDText[i] = machineEntry[i].transform.Find("DURABILITY").GetComponent<Text>();
                machineMenuBText[i] = machineEntry[i].transform.Find("BATCH SIZE").GetComponent<Text>();
                machineMenuCText[i] = machineEntry[i].transform.Find("CYCLE TIME").GetComponent<Text>();
                machineMenuYText[i] = machineEntry[i].transform.Find("YIELD").GetComponent<Text>();
            }
        }*/

    }

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

    public void setupProcessingMenu()
    {
        for(int i = 0;i < ProcessingQueue.Count; i++)
        {
            productionPixelImg[i] = productionEntry[i].transform.Find("image").GetComponent<UnityEngine.UI.Image>();
            productionIngredientAImg[i] = productionEntry[i].transform.Find("ingredient A").GetComponent<UnityEngine.UI.Image>();
            productionIngredientBImg[i] = productionEntry[i].transform.Find("ingredient B").GetComponent<UnityEngine.UI.Image>();
            productionNameText[i] = productionEntry[i].transform.Find("NAME").GetComponent<UnityEngine.UI.Text>();
            productionQuantityText[i] = productionEntry[i].transform.Find("QUANTITYNUM").GetComponent<UnityEngine.UI.Text>();
            productionStatusText[i] = productionEntry[i].transform.Find("STATUS").GetComponent<UnityEngine.UI.Text>();
            productionMachineText[i] = productionEntry[i].transform.Find("MACHINE NAME").GetComponent<UnityEngine.UI.Text>();
            productionBarImg[i]= productionEntry[i].transform.GetComponent<UnityEngine.UI.Image>();
        }
    }

    public void setupMachineMenu()
    {

        for (int i = 0; i < machineEntry.Length; i++)  //SETS UP MACHINE ENTRY
        {
            if (i == 0) machineEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Machinery Page/MACHINE LIST/MACHINE ENTRY");
            else machineEntry[i] = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/Machinery Page/MACHINE LIST/MACHINE ENTRY (" + i + ")");

            if (machineEntry[i] == null) { Debug.Log("This crashes!" + i); }
            else Debug.Log("This one worked!" + i);



        
            machineMenuNameText[i] = machineEntry[i].transform.Find("MACHINE NAME").GetComponent<Text>();
            machineMenuStatusText[i] = machineEntry[i].transform.Find("STATUS").GetComponent<Text>();
            machineMenuAssignementText[i] = machineEntry[i].transform.Find("ASSIGNMENT").GetComponent<Text>();
            machineMenuDMDText[i] = machineEntry[i].transform.Find("DURABILITY").GetComponent<Text>();
            machineMenuBText[i] = machineEntry[i].transform.Find("BATCH SIZE").GetComponent<Text>();
            machineMenuCText[i] = machineEntry[i].transform.Find("CYCLE TIME").GetComponent<Text>();
            machineMenuYText[i] = machineEntry[i].transform.Find("YIELD").GetComponent<Text>();
        }

    }
    public void updateMenu()
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

        // Debug.Log("processing queue length: "+ProcessingQueue.Count);
        for (int i = 0; i < productionEntry.Count; i++)
        {
            //Debug.Log($"production pixel image: {productionPixelImg[i]}");
            if (i < ProcessingQueue.Count)
            {
                int outputIndex = ProcessingQueue[i].c3index;
                int ingredientBindex = ProcessingQueue[i].c2index;
                int ingredientAindex = ProcessingQueue[i].c1index;

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
            Debug.Log(i);
            if (i < machines.Length)
            {
                Machine m = machines[i];
                //Debug.Log(m);

                machineMenuNameText[i].text = m.name;
                machineMenuStatusText[i].text = ft.machineStatuses[m.status];

                if (m.orderIndex == -1)
                    machineMenuAssignementText[i].text = "not assigned";
                else 
                    machineMenuAssignementText[i].text = ProcessingQueue[m.orderIndex].name;
                
                machineMenuDMDText[i].text = m.durability + "/" + m.maxDurability;
                machineMenuBText[i].text = m.batchSize.ToString();
                machineMenuCText[i].text = m.cycleTime.ToString();
                machineMenuYText[i].text = m.Yield.ToString();

            }
            else //unassigned
            {
                machineMenuNameText[i].text = "test";
                machineMenuStatusText[i].text = "";
                machineMenuAssignementText[i].text = "";
                machineMenuDMDText[i].text = "";
                machineMenuBText[i].text = "";
                machineMenuCText[i].text = "";
                machineMenuYText[i].text = "";

            }

        }
    }
    public void updateEvents(int status)
    {
        
        for (int i = history.Length-1; i > 0; i--)
        {
            history[i] = history[i-1];
        }
        for (int i = timestamps.Length - 1; i > 0; i--)
        {
            timestamps[i].text = timestamps[i - 1].text;
        }
        
        history[0] = status;

        currentTime = DateTime.Now;
        timeString = currentTime.ToString("hh:MM:sstt");
        timestamps[0].text = timeString;
        
       // updateMenu();
    }

    public void distributeTokens() 
    {
        if (inventory[0] < harvestCapacity && chosenColor != 0)
        {
            inventory[0]++;

            if (inventory[0] == harvestCapacity)  //warns when it is full.
            {
                oreValueText.color = Color.red;
                updateEvents(10);
            }
        }

    }
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

        updateEvents(1);
        
    }
    public void updateGreen()
    {
        selectedColor = 2;
        updateToken();

        updateEvents(2);
        
    }
    public void updateBlue()
    {
        selectedColor = 3;
        updateToken();

        updateEvents(3);    
    }
    public void setText()
    {
        oreValueText.text = inventory[0].ToString() + "/" + harvestCapacity;

        
        redPixelText.text = inventory[1].ToString();
        greenPixelText.text = inventory[2].ToString();
        bluePixelText.text = inventory[3].ToString();
        yellowPixelText.text = inventory[4].ToString();
        magentaPixelText.text = inventory[5].ToString();
        cyanPixelText.text = inventory[6].ToString();
        whitePixelText.text = inventory[7].ToString();

        availableMachinesText.text = "Available Machines:\n" + totalAvailableMachines().ToString() + "/" + machines.Length;
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

    /// <summary>
    /// //
    /// </summary>
    /// <param name="dice"> number of dice</param>
    /// <param name="max"> sides of dice</param>
    /// <returns>a random value  </returns>
    /// 
    public int rollDice(int dice, int sides)  //rolls x dice of 1 to max
    {
        int roll = 0;

        for(int i = 0; i < dice;i++)
        {
            roll += r.Next(1, sides+1);
        }

        return roll;
    }
    

}