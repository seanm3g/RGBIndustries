using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
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

    public Text chosenColorText;

    public GameObject SelectMenu;
    public GameObject UIbackground;
    public GameObject CanvasGO;
    

    public Machine[] machines = new Machine[10];
    public Employee[] employees = new Employee[10];
    public int[] inventory = new int[8];
    public int[] history = new int[8];

    public GameObject[] events = new GameObject[8];
    public UnityEngine.UI.Text[] timestamps = new Text[8];
    public UnityEngine.UI.Image[] eventImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Text[] eventText = new UnityEngine.UI.Text[8];


    /// <summary>
    //public GameObject[] productionEntry = new GameObject[8];
    public List<GameObject> productionEntry = new List<GameObject>();
    public UnityEngine.UI.Image[] productionPixelImg = new UnityEngine.UI.Image[8];
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

    /// </summary>

    void Start()
    {
        r = new System.Random();

        SelectMenu.SetActive(true);
        UIbackground.SetActive(true);
        //eventGroup.SetActive(false);

        setupInventory();
        setupMenu();
        setupProcessingMenu();
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
        //updateQueue();
    }

    private void runMachines()
    {
        bool idle = false;

        for (int i = 0; i < machines.Length; i++)
        {
            //productionStatusText[i].text = ft.woStatuses[2];
            //productionMachineText[i].text = machines[i].name;

            //Debug.Log("Are we running machines each update?");
            if (machines[i].isRunning)
            {
                productionStatusText[i].text = ft.woStatuses[2];
                productionMachineText[i].text = machines[i].name;
                idle = true;
                //Debug.Log($"machine {i} is {machines[i].isRunning}");
                machines[i].elapsedTime += Time.deltaTime;
                //Debug.Log($"machine {i} time is {machines[i].elapsedTime}");

                if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
                {

                    // Reset the elapsed time and stop processing
                    machines[i].runMachine();
                    machines[i].elapsedTime = 0f;
                    machines[i].isRunning = false;

                    // Signal that the output is ready
                    machines[i].outputReady = true;
                }
            }

            if (machines[i].outputReady)  //unload the event
            {
                if (ProcessingQueue.Count > 0)
                {
                    inventory[ProcessingQueue[0].c3index] += machines[i].unloadMachine(); //unloads the production quantity to inventory.
                    updateEvents(4);  //says job is processed
                    machines[i].outputReady = false;

                    machines[i].completed = true;
                }
            }
            if (machines[i].completed)
            {
                machines[i].elapsedTime += Time.deltaTime;
                if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
                {
                    productionStatusText[i].text = ft.woStatuses[3];
                    machines[i].elapsedTime = 0f;
                    machines[i].completed = false;
                    ProcessingQueue.RemoveAt(0);
                }
            }
        }

        String activeList = "";
       for(int i = 0; i < ProcessingQueue.Count; i++)
        {
            activeList += i + " " + ProcessingQueue[i].isActive + "\n";
        }
        //Debug.Log("ProcessingQueue items: "+ProcessingQueue.Count);
        //Debug.Log(activeList);

        for (int i = 0; i < ProcessingQueue.Count; i++)  //set status based on the current machine status
        {
            if (machines[i].isRunning)
            {
                productionStatusText[i].text = ft.woStatuses[2];
                productionMachineText[i].text = machines[i].name;
            }
            else if (machines[i].completed)
            {
                productionStatusText[i].text = ft.woStatuses[3];
            }    
            else
            {
                
                productionStatusText[i].text = ft.woStatuses[1];
                productionMachineText[i].text = "Not Assigned";
            }
        }


        // if (idle) updateEvents(15);// this needs to be able to not repeat as an event before implementation.
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


        ProcessingQueue.Add(new workOrder(3, level1[1], level1[3], level1[5]));  //make magenta
        ProcessingQueue.Add(new workOrder(5, level1[1], level1[2], level1[4]));  //make yellow
        ProcessingQueue.Add(new workOrder(2, level1[1], level1[3], level1[5]));  //make magenta
        ProcessingQueue.Add(new workOrder(8, level1[1], level1[2], level1[4]));  //make yellow
        ProcessingQueue.Add(new workOrder(5, level1[1], level1[3], level1[5]));  //make magenta
        ProcessingQueue.Add(new workOrder(5, level1[1], level1[3], level1[5]));  //make magenta
        ProcessingQueue.Add(new workOrder(2, level1[1], level1[2], level1[4]));  //make yellow
        ProcessingQueue.Add(new workOrder(10,level1[3], level1[4], level1[7]));  //make white
    }
    
    public void updateQueue()  //if the queue is running, then try to pair a job with a machine.
    {
        int firstAvailable = -1; //first available machine
        int firstNeeded = -1; //The first process that needs assigning

        for (int i = 0; i < machines.Length; i++)
            if (machines[i].status == 7)
            {
                firstAvailable = i;

                break;  //leaves once first available machine is found.
            }

        if (ProcessingQueue.Count == 0 || firstAvailable ==-1)  //queue is empty or no machines available
            return;

        for (int i = 0; i < ProcessingQueue.Count; i++)
        {
            if (!ProcessingQueue[i].isActive)
            {
                firstNeeded = i;
                ProcessingQueue[firstNeeded].makeActive();
                break;
            }
        }
        //Debug.Log(firstAvailable + " " + ProcessingQueue[firstNeeded]);

        Debug.Log(productionStatusText[0].text);
        Debug.Log(productionMachineText[0].text);
        productionStatusText[0].text = ft.woStatuses[2];
        productionMachineText[0].text = machines[0].name;
        
        Debug.Log("after: "+productionStatusText[0].text);
        Debug.Log("after: " + productionMachineText[0].text);

        if (firstAvailable != -1 && firstNeeded != -1)
        {
            
            updateEvents(5);
            processOrder(firstAvailable, firstNeeded);  //runs the order
            
        }

    }
    
    private void processOrder(int index, int firstNeeded)  //index of the machine to be used
    {

        machines[index].assignOrder(ProcessingQueue[firstNeeded]);

        int c1q;  //determine the quantity to move to the machine.
        int c2q;

        if (inventory[ProcessingQueue[firstNeeded].c1index] >= ProcessingQueue[firstNeeded].quantity) //There's enough for a full order
        {
            c1q = ProcessingQueue[firstNeeded].quantity;
            inventory[ProcessingQueue[firstNeeded].c1index] -= c1q;
        }
        else  //if there's not enough for a full batch.
        {
            c1q = inventory[ProcessingQueue[firstNeeded].c1index];
            inventory[ProcessingQueue[firstNeeded].c1index] = 0; // Set to zero since we're using all available inventory
        }

        if (inventory[ProcessingQueue[firstNeeded].c2index] >= c1q)  //They need to match in batch size, only needs to be compared to c1q
        {
            c2q = c1q;
            inventory[ProcessingQueue[firstNeeded].c2index] -= c2q;
        }
        else  //if c2q is less
        {
            c2q = inventory[ProcessingQueue[firstNeeded].c2index];
            inventory[ProcessingQueue[firstNeeded].c2index] = 0; // Set to zero since we're using all available inventory

            // Reset c1 inventory to what it was before
            inventory[ProcessingQueue[firstNeeded].c1index] += c1q - c2q;
            c1q = c2q; // Update c1q to match c2q
        }

        
        machines[index].loadMachine(c1q, c2q, ProcessingQueue[firstNeeded].quantity);


        if (c1q == 0 || c2q == 0)  //if there's not enough set the status that the machine is starved
        {
            machines[index].status = 4;
            return;
        }

        machines[index].startMachine();
        //wo.isActive = true;
        
    }
    
    public void setupInventory()
    {
        for(int i = 0; i < inventory.Length;i++)
            inventory[i] = 0;

        inventory[1] = 10;
        inventory[2] = 10;
        inventory[3] = 10;

        //Console.WriteLine("Setup run");

        for (int i = 0; i < 10; i++)  //setup both things
        {
            employees[i] = new Employee(rollDice(1, 10), rollDice(1, 10), rollDice(1, 10), 16 + rollDice(1, 45), rollDice(1, 10), rollDice(1, 10));
            machines[i] = new Machine(ft.generateMachineName(), 1, rollDice(3, 6), 60 * rollDice(1, 6), 1 + rollDice(3, 3), 103 - rollDice(3, 6)); 
        }
        //Debug.Log(machines[0]);

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


        makeMachine();

        for (int i = 7; i >= 0; i--)  //INITALIZE THE OBJECTS TO INTERACT WITH FOR THE EVENTLIST
        {
            events[i] = Instantiate(eve, new Vector3(0f, 0f, 0f), Quaternion.identity);  //instanties each element of the panel array.
            events[i].transform.SetParent(eventslayout.transform, false);   //sets the parent to fit into the eventslayout place

        }

        for (int i = 0; i < 8; i++)
        {
           // Debug.Log(productionEntry[i]);
           // Debug.Log($"work order entry: {workOrderEntry}");
            
           

            timestamps[i] = events[i].transform.Find("time").GetComponent<Text>(); //set the timestamp from panels
            eventText[i] = events[i].transform.Find("bg/message").gameObject.GetComponent<Text>(); //set the text from panels
            eventImg[i] = events[i].transform.Find("bg").GetComponent<UnityEngine.UI.Image>(); //set the
        }
        

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
            productionQuantityText[i] = productionEntry[i].transform.Find("QUANTITYNUM").GetComponent<UnityEngine.UI.Text>();
            productionStatusText[i] = productionEntry[i].transform.Find("STATUS").GetComponent<UnityEngine.UI.Text>();
            productionMachineText[i] = productionEntry[i].transform.Find("MACHINE NAME").GetComponent<UnityEngine.UI.Text>();
        }
    }
    public void updateMenu()
    {

        for(int i = 0; i < history.Length; i++)
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

        Debug.Log("processing queue length: "+ProcessingQueue.Count);
        for (int i = 0; i < ProcessingQueue.Count; i++)
        {
            //Debug.Log($"production pixel image: {productionPixelImg[i]}");
            if (i < ProcessingQueue.Count)
            {
                int colorIndex = ProcessingQueue[i].c3index;

                Color tempColor = new Color(colorLib.colors[colorIndex].r,colorLib.colors[colorIndex].g,colorLib.colors[colorIndex].b);

                productionPixelImg[i].color = tempColor;
                productionQuantityText[i].text = ProcessingQueue[i].quantity.ToString();

            }
            else
            {
                productionMachineText[i].text = "";
                productionStatusText[i].text = "";
                productionPixelImg[i].color = Color.white;
                productionQuantityText[i].text = "";
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

    public int rollDice(int dice, int max)  //rolls x dice of 1 to max
    {
        int roll = 0;

        for(int i = 0; i < dice;i++)
        {
            roll += r.Next(1, max);
        }

        return roll;
    }
    

}