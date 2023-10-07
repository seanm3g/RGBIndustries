using System;
using System.Collections;
using System.Collections.Generic;
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
    public int oreTokens = 0;

    public int selectedColor = 0;  //color being selected in the menu
    public int chosenColor = 0;  //color currently set as primary

    public int harvestCapacity = 1;

    public float timer = 0;
    public float spawnRate = 1;

    public Text oreValueText;
    public Text redPixelText;
    public Text greenPixelText;
    public Text bluePixelText;

    public GameObject SelectMenu;
    public GameObject StakingMenu;
    public GameObject UIbackground;
    public GameObject CanvasGO;

    //v0.2   && v0.3
    public GameObject[] panels = new GameObject[8];
    public GameObject[] group = new GameObject[8];
    public UnityEngine.UI.Text[] timestamps = new Text[8];
    //.v0.2

    public Machine[] machines = new Machine[10];
    public Employee[] employees = new Employee[10];
    public Pixel[] inventory = new Pixel[20];
    public TimelineEvent[] timelineEvents = new TimelineEvent[8];

    public int timelineIndex = 0;
    public UnityEngine.UI.Image panelImage;

    public GameObject[] box = new GameObject[8];
    public GameObject[] eventTextGO = new GameObject[8];

    public UnityEngine.UI.Image[] eventImg = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Text[] eventText = new UnityEngine.UI.Text[8];
    public RectTransform[] rectTransform = new RectTransform[8];
    public RectTransform[] rectTransformText = new RectTransform[8];

    int invIndex = 4;
    public FlavorText ft = new FlavorText();

    public DateTime currentTime;
    String timeString;


    /// <summary>
    /// </summary>

    void Start()
    {
        r = new System.Random();

       // SelectMenu.SetActive(true);
        StakingMenu.SetActive(false);
        UIbackground.SetActive(true);
        // eventGroup.SetActive(false);

        GameObject eventslayout = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/events page/eventslayout");
        if (eventslayout == null)
        {
            Debug.LogError("Could not find GameObject named 'eventslayout'");
            return;
        }

        GameObject bg = GameObject.Find("Canvas/UI LAYOUT/MAIN AREA/PAGE AREA/events page/eventslayout");
        if (eventslayout == null)
        {
            Debug.LogError("Could not find GameObject named 'eventslayout'");
            return;
        }


        for (int i = 0; i < 8; i++)
        {
            GameObject e = Resources.Load<GameObject>("Prefabs/event");
            panels[i] = Instantiate(e,new Vector3(0f,0f,0f), Quaternion.identity);

            panels[i].transform.SetParent(eventslayout.transform, false);
            Debug.Log("Is this assigning?");
            timestamps[i] = panels[i].transform.Find("time").GetComponent<Text>();
            group[i] = panels[i].transform.Find("bg/message").gameObject;
            //timelineEvents[i].eventType = panels[i].transform.Find("bg/message").GetComponent<Text>().text;

        }
       

    }

    public void updateMenu()
    {

        for (int i = 0; i < timelineEvents.Length; i++)
        {

            
            eventText[i] = group[i].GetComponent<Text>();
            eventImg[i] = panels[i].transform.Find("bg").GetComponent<UnityEngine.UI.Image>();

            eventText[i].text = ft.eventStatuses[timelineEvents[i].eventType].ToUpper();  // Assuming ft is an array or list

            switch (timelineEvents[i].eventType)
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
                case 4:  ///halted machine
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

    public void updateEvents(int status)
    {
        //Debug.Log(DateTime.Now.ToString("hh:MM:ss tt"));

        
        for (int i = timelineEvents.Length-1; i > 0; i--)
        {
            timelineEvents[i] = timelineEvents[i-1];
        }
        for (int i = timestamps.Length - 1; i > 0; i--)
        {
            timestamps[i].text = timestamps[i - 1].text;
        }
        
        timelineEvents[0].eventType = status;

        currentTime = DateTime.Now;
        timeString = currentTime.ToString("hh:MM:sstt");
        timestamps[0].text = timeString;
        
        updateMenu();
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
    }
    public void distributeTokens() 
    {
        if (inventory[0].quantity < harvestCapacity && chosenColor != 0)
        {
            inventory[0].quantity++;

            if (inventory[0].quantity == harvestCapacity)
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
        StakingMenu.SetActive(true);
        UIbackground.SetActive(true);
      //  eventGroup.SetActive(true);


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
        oreValueText.text = inventory[0].quantity.ToString() + "/" + harvestCapacity;
    }

    public void harvest()
    {
        if(inventory[0].quantity > 0) 
        {

            updateEvents(8);

            oreValueText.color = Color.white;


            switch (chosenColor)
            {
                case 1:  //red
                    inventory[1].quantity += inventory[0].quantity;
                    inventory[0].quantity = 0;
                    redPixelText.text = inventory[1].quantity.ToString();
                    break;
                case 2:  //green
                    inventory[2].quantity += inventory[0].quantity;
                    inventory[0].quantity = 0;
                    greenPixelText.text = inventory[2].quantity.ToString();
                    break;
                case 3:  //blue
                    inventory[3].quantity += inventory[0].quantity;
                    inventory[0].quantity = 0;
                    bluePixelText.text = inventory[3].quantity.ToString();
                    break;
                default:
                    Debug.Log("not selected"); 
                    break;
            }   
        }
    }
   
    public void harvestUpgrade()
    {

        int chosenQuant = inventory[chosenColor].quantity;
        
        if (chosenQuant >= harvestCapacity)
        {
            updateEvents(9);

            oreValueText.color = Color.white;

            inventory[chosenColor].quantity -= harvestCapacity;
            harvestCapacity++;

            // Updates the text on screen
            if (chosenColor == 1) redPixelText.text = inventory[1].quantity.ToString();
            else if (chosenColor == 2) greenPixelText.text = inventory[2].quantity.ToString();
            else if (chosenColor == 3) bluePixelText.text = inventory[3].quantity.ToString();
         
        }
        
    }
public void setupInventory()
    {
        inventory[0] = new Pixel(0,0,0,0,0);  // ore value
        inventory[1] = new Pixel(0,1,1,0,0);  // red value
        inventory[2] = new Pixel(0,1,0,1,0);  // green value
        inventory[3] = new Pixel(0,1,0,0,1);  // blue value

        Console.WriteLine("Setup run");

        for (int i = 0; i < 10; i++)  //setup both things
        {
            employees[i] = new Employee(rollDice(1, 10), rollDice(1, 10), rollDice(1, 10), 16 + rollDice(1, 45), rollDice(1, 10), rollDice(1, 10));
            machines[i] = new Machine(1, rollDice(3, 6), 60 * rollDice(1, 6), 1 + rollDice(3, 3), 103 - rollDice(3, 6));
        }
    }

    public void addToInventory(Pixel p)
    {
        bool inserted = false;
        for(int i = 0;i < inventory.Length - 1; i++)
        {
            if (inventory[i].isMatch(p))
            {
                inventory[i].quantity += p.quantity;
                inserted = true;
            }
        }

        if (!inserted)  //if it's a new pixel, add it to the inventory.
        {
            inventoryIndex();
            inventory[invIndex] = p;
        }
    }

    public void inventoryIndex()
    {
        invIndex = 0;
        while (!inventory[invIndex].isMatch(new Pixel(0,0,0,0,0))) 
        {
            invIndex++;
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