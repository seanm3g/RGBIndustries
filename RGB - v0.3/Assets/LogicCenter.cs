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

    public Text chosenColorText;

    public GameObject SelectMenu;
    public GameObject UIbackground;
    public GameObject CanvasGO;
    public GameObject ProductionPage;
    

    public Machine[] machines = new Machine[10];
    public Employee[] employees = new Employee[10];
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
        ProductionPage.SetActive(false);

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

    private void runMachines()  //could add an idle condition
    {

        for (int i = 0; i < machines.Length; i++)
        {
            if (machines[i].isLoading)
                isLoading(i);
            if (machines[i].isRunning)
                isRunning(i);
            if (machines[i].unloading)  //unload the pixels to inventory
                isUnloading(i);
            if (machines[i].completed)
                isComplete(i);
        }
        updateProductionUI();

    }

    public void isLoading(int i)
    {
        //productionStatusText[i].text = ft.woStatuses[2];
        productionMachineText[i].text = machines[i].name;
        machines[i].elapsedTime += Time.deltaTime;

        if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
        {
            // Reset the elapsed time and stop processing
            machines[i].startMachine();
            machines[i].elapsedTime = 0f;

           // Debug.Log("but are we getting here?");
            // Signal that the output is ready
            machines[i].isLoading = false;
            machines[i].isRunning = true;
        }
    }

    public void isRunning(int i)
    {

       // productionStatusText[i].text = ft.woStatuses[3];  //sets to in production
      //  productionMachineText[i].text = machines[i].name;
        //Debug.Log($"machine {i} is {machines[i].isRunning}");
        machines[i].elapsedTime += Time.deltaTime;
        //Debug.Log($"machine {i} time is {machines[i].elapsedTime}");

        if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
        {

            // Reset the elapsed time and stop processing
            machines[i].runMachine();
            machines[i].elapsedTime = 0f;

            // Signal that the output is ready
            machines[i].unloading = true;
        }

    }

    public void isUnloading(int i)
    {
        //productionStatusText[i].text = ft.woStatuses[4];  //sets to in production

        Debug.Log("i: " + i);
        Debug.Log("ProcessingQueue.Count: " + ProcessingQueue.Count);
        Debug.Log("machines.Length: " + machines.Length);

        

        machines[i].elapsedTime += Time.deltaTime;

        if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
        {
            workOrder wo = ProcessingQueue[machines[i].orderIndex];
            int product = machines[i].unloadMachine();
            
            inventory[ProcessingQueue[machines[i].orderIndex].c3index] += product; //unloads the production quantity to inventory. Could be added to have randomness.
            wo.quantity -= product;

            machines[i].unloading = false;
            machines[i].completed = true;
            machines[i].elapsedTime = 0f;
            updateEvents(4);  //says job is processed
            ProcessingQueue[machines[i].orderIndex] = wo;  //check it back into the data

        }
        
    }

    public void isComplete(int i)
    {
        machines[i].elapsedTime += Time.deltaTime;

        if (machines[i].elapsedTime >= machines[i].cycleTime)  //finishes processing
        {
            //productionStatusText[i].text = ft.woStatuses[5];
            machines[i].elapsedTime = 0f;

            machines[i].isRunning = false;
            machines[i].isLoading = false;
            machines[i].unloading = false;
            machines[i].completed = false;

            //if (ProcessingQueue[i].quantity > 0)

            if(ProcessingQueue[machines[i].orderIndex].quantity == 0)  //removes the order if it's been fully completed.
                ProcessingQueue.RemoveAt(machines[i].orderIndex);

               
        }
    }

    public void updateProductionUI()
    {
        for (int i = 0; i < ProcessingQueue.Count; i++)  //set status based on the current machine status
        {
            int index = ProcessingQueue[i].machineIndex;

            //Debug.Log("INDEX: " + index);
            //Debug.Log("machine "+i+": "+ machines[i].isLoading);
            //Debug.Log("are we getting to the UI update?");
            if (index > -1)
            {
                if (machines[index].isLoading)
                {
                    productionBarImg[i].color = Color.yellow;
                    productionStatusText[i].color = Color.blue;
                    productionStatusText[i].text = ft.woStatuses[2];
                    productionMachineText[i].text = machines[i].name;
                }
                else if (machines[index].isRunning)
                {
                    productionBarImg[i].color = Color.green;
                    productionStatusText[i].color = Color.white;
                    productionStatusText[i].text = ft.woStatuses[3];
                    productionMachineText[i].text = machines[i].name;
                }
                else if (machines[index].unloading)
                {
                    productionBarImg[i].color = Color.gray;
                    productionStatusText[i].color = Color.yellow;
                    productionStatusText[i].text = ft.woStatuses[4];

                }
                else if (machines[index].completed)
                {

                    if (ProcessingQueue[machines[index].orderIndex].quantity < 1)
                    {
                        productionStatusText[i].text = ft.woStatuses[5];
                        productionBarImg[i].color = Color.yellow;
                        productionStatusText[i].color = Color.green;
                    }
                    else
                    {
                        productionStatusText[i].text = ft.woStatuses[1];
                        productionBarImg[i].color = Color.gray;
                        productionStatusText[i].color = Color.red;
                    }

                }
            }
            else  //unassigned
            {
                productionBarImg[i].color = Color.white;
                productionStatusText[i].color = Color.black;
                productionStatusText[i].text = ft.woStatuses[1];
                productionMachineText[i].text = "Not Assigned";
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

        for(int i = 0; i < 8; i++)
        {      
            int r = rollDice(1, 4);

            if (i < 5)
                r = rollDice(1, 3);

            int q = rollDice(4, 2);

            switch (r)
            {
                case 1:
                    ProcessingQueue.Add(new workOrder(q, level1[1], level1[2], level1[4]));  //make yellow
                    break;
                case 2: 
                    ProcessingQueue.Add(new workOrder(q, level1[1], level1[3], level1[5]));  //makes magenta
                    break;
                case 3:
                    ProcessingQueue.Add(new workOrder(q, level1[2], level1[3], level1[6])); //cyan
                    break;
                case 4:
                    switch (rollDice(1, 3))
                    {
                        case 1: ProcessingQueue.Add(new workOrder(q, level1[3], level1[4], level1[7])); //white
                            break;
                        case 2: ProcessingQueue.Add(new workOrder(q, level1[2], level1[5], level1[7])); //white
                            break;
                        case 3: ProcessingQueue.Add(new workOrder(q, level1[1], level1[6], level1[7])); //white
                            break;
                    }
                    break;
            }


        }

        /*
        ProcessingQueue.Add(new workOrder(3, level1[1], level1[3], level1[5]));  //make magenta
        ProcessingQueue.Add(new workOrder(5, level1[1], level1[2], level1[4]));  //make yellow
        ProcessingQueue.Add(new workOrder(2, level1[1], level1[3], level1[5]));  //make magenta
        ProcessingQueue.Add(new workOrder(8, level1[1], level1[2], level1[4]));  //make yellow
        ProcessingQueue.Add(new workOrder(5, level1[1], level1[3], level1[5]));  //make magenta
        ProcessingQueue.Add(new workOrder(5, level1[1], level1[3], level1[5]));  //make magenta
        ProcessingQueue.Add(new workOrder(2, level1[1], level1[2], level1[4]));  //make yellow
        ProcessingQueue.Add(new workOrder(10,level1[3], level1[4], level1[7]));  //make white
        */
    }

    public void updateQueue()  //pairs orders to Machines
    {
        // Initialize variables to keep track of the first available machine and the first job that needs assigning.
        int firstAvailable = -1;
        int firstNeeded = -1;

        // Search for the first available machine.
        for (int i = 0; i < machines.Length; i++)
        {
            if (machines[i].status == 7)  // 7 indicates an available machine
            {
                firstAvailable = i;
                break;  // Exit the loop once the first available machine is found.
            }
        }

        // Search for the first job that needs assigning.
        for (int i = 0; i < ProcessingQueue.Count; i++)
        {
            if (!ProcessingQueue[i].isActive)
            {
                firstNeeded = i;
                
                break;  // Exit the loop once the first job that needs assigning is found.
            }
        }

        // If the queue is empty or no machines are available, exit the method.
        if (ProcessingQueue.Count == 0 || firstAvailable == -1)
        {
            updateEvents(15);
            return;
        }

        workOrder wo = ProcessingQueue[firstNeeded];


        updateEvents(5);  // Update events (assuming this method logs or updates some state)
        
        wo.isActive = true;  // Mark the job as active.
        wo.machineIndex = firstAvailable;  //testing this, sets which machine it's assigned to
        
        machines[firstAvailable].orderIndex = firstNeeded;        //sets the index the machine is working with in the queue.

        
        
        ProcessingQueue[firstNeeded] = wo;

        processOrder(firstAvailable, firstNeeded);  // Process the order.

        
    }

    private void processOrder(int selectedMachine, int selectedWO)  //selectedMachine of the machine to be used
    {
        workOrder wo = ProcessingQueue[selectedWO];

        // Assign the order to the machine and update the selectedMachine

        machines[selectedMachine].assignOrder(wo); // sets the machine up for what it wants to produce.
        wo.machineIndex = selectedMachine;  // sets up the work order to

        // Determine the quantity to move to the machine
        int c1Quantity = DetermineQuantity(wo.c1index, wo.quantity);
        int c2Quantity = DetermineQuantity(wo.c2index, c1Quantity);
        c1Quantity = c2Quantity;

        // If either quantity is zero, set the machine status to 'starved' and return
        if (c1Quantity <= 0 || c2Quantity <= 0)
        {
            machines[selectedMachine].status = 4;
            return;
        }

        // Load the machine and start it
        machines[selectedMachine].loadMachine(c1Quantity, c2Quantity);

        // Update the order to reflect what has been produced already.
        

        ProcessingQueue[selectedWO] = wo; //adds the wo back to the collection.
        // If there's a remaining quantity, update the ProcessingQueue

        // machines[selectedMachine].startMachine();
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

        // Update the inventory
        inventory[componentIndex] -= quantityToUse;
        return quantityToUse;
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
            productionIngredientAImg[i] = productionEntry[i].transform.Find("ingredient A").GetComponent<UnityEngine.UI.Image>();
            productionIngredientBImg[i] = productionEntry[i].transform.Find("ingredient B").GetComponent<UnityEngine.UI.Image>();
            productionQuantityText[i] = productionEntry[i].transform.Find("QUANTITYNUM").GetComponent<UnityEngine.UI.Text>();
            productionStatusText[i] = productionEntry[i].transform.Find("STATUS").GetComponent<UnityEngine.UI.Text>();
            productionMachineText[i] = productionEntry[i].transform.Find("MACHINE NAME").GetComponent<UnityEngine.UI.Text>();
            productionBarImg[i]= productionEntry[i].transform.GetComponent<UnityEngine.UI.Image>();
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
        for (int i = 0; i < productionEntry.Count; i++)
        {
            //Debug.Log($"production pixel image: {productionPixelImg[i]}");
            if (i < ProcessingQueue.Count)
            {
                int outputIndex = ProcessingQueue[i].c3index;
                int ingredientBindex = ProcessingQueue[i].c2index;
                int ingredientAindex = ProcessingQueue[i].c1index;

                Color tempColor = new Color(colorLib.colors[outputIndex].r,colorLib.colors[outputIndex].g,colorLib.colors[outputIndex].b);
                Color ingredientA = new Color(colorLib.colors[ingredientAindex].r, colorLib.colors[ingredientAindex].g, colorLib.colors[ingredientAindex].b);
                Color ingredientB = new Color(colorLib.colors[ingredientBindex].r, colorLib.colors[ingredientBindex].g, colorLib.colors[ingredientBindex].b);
                productionPixelImg[i].color = tempColor;
                productionIngredientAImg[i].color = ingredientA;
                productionIngredientBImg[i].color = ingredientB;

                if (!ProcessingQueue[i].isActive)
                    productionQuantityText[i].text = ProcessingQueue[i].quantity.ToString();
                else productionQuantityText[i].text = ProcessingQueue[i].quantity.ToString() + "(-" + machines[ProcessingQueue[i].machineIndex].c2q +")";

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
            roll += r.Next(1, max+1);
        }

        return roll;
    }
    

}