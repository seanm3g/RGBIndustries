using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EarmarkCategory
{
    None,
    OtherWorkOrder,
    Trade,
    Contract,
    Inventory,
}
public class program : MonoBehaviour
{
    void Start()
    {
        InventoryManager manager = new InventoryManager();
        List<Contract> ProductionContracts = new List<Contract>();
        z
        manager.setupInventory(1);


        manager.AddToInventory(1,"Red",10);
        manager.AddToInventory(1,"Green", 10);
        manager.AddToInventory(1,"Blue", 10);
        /*
    // Add items to inventory
        manager.AddToInventory("#FF0000", 10);
        manager.AddToInventory("#00FF00", 20);
        manager.AddToInventory("#0000FF", 20);

        // Show initial status
        manager.ShowInventoryStatus();

        // Create work orders
        WorkOrder workOrder1 = new WorkOrder(5, "Red", EarmarkCategory.Trade);
        WorkOrder workOrder2 = new WorkOrder(15, "Blue", EarmarkCategory.OtherWorkOrder);
        WorkOrder workOrder3 = new WorkOrder(6, "Red", EarmarkCategory.Contract);

        // Process work orders
        manager.ProcessWorkOrder(workOrder1);
        manager.ProcessWorkOrder(workOrder2);
        manager.ProcessWorkOrder(workOrder3);
        */
        // Show final status
        manager.ShowInventoryStatus();
    }
}
public class InventoryManager
{

    static Dictionary<string, string> colorToHex = new Dictionary<string, string>
    {
        {"Red", "1#FF0000"},
        {"Green", "1#00FF00"},
        {"Blue", "1#0000FF"},
        {"White", "1#FFFFFF"},
        {"Black", "1#000000"},
        {"Yellow", "1#FFFF00"},
        {"Magenta", "1#FF00FF"},
        {"Cyan", "1#00FFFF"},
        // Add more colors here
    };


    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    private Dictionary<string, Dictionary<EarmarkCategory, int>> earmarks = new Dictionary<string, Dictionary<EarmarkCategory, int>>();

    public void setupInventory(int level) //inventory
    {
       //can handle an inventory up to 100 pixels.
     

        for (int L = 1; L <= level; L++)
        {
            int highestValue = (int)Mathf.Pow(2, L) - 1; // Max L-bit value

            for (int r = 0; r <= highestValue; r++)
            {
                for (int g = 0; g <= highestValue; g++)
                {
                    for (int b = 0; b <= highestValue; b++)
                    {
                        // Scale L-bit to 8-bit
                        int r8 = (int)((r / (float)highestValue) * 255);
                        int g8 = (int)((g / (float)highestValue) * 255);
                        int b8 = (int)((b / (float)highestValue) * 255);

                        // Convert to hex
                        string hex = $"#{r8:X2}{g8:X2}{b8:X2}";
                        bool keyExists = false;

                        for (int i = 0; i < L; i++)
                            if (inventory.ContainsKey(i + hex))
                                keyExists = true;

                        if (!keyExists)
                        {
                            inventory.Add(L+hex, 0);
                        }
                    }
                }
            }
        }

        Debug.Log("Size: "+inventory.Count);
    }
    public void AddToInventory(int L,string color, int quantity)
    {
        color = ColorToHex(color);

        if (!inventory.ContainsKey(L+color))
        {
            inventory[L+color] = 0;
        }
        inventory[L+color] += quantity;
    }

    public void removeFromInventory(int L,string color,int quantity)
    {
        if (inventory.ContainsKey(L + color))
        {
            inventory[L + color] -= quantity;
        }
    }
    private string intToHex(int c)
    {
        switch(c)
        {
            case 0: return "Black";
            case 1: return "Red";
            case 2: return "Green";
            case 3: return "Blue";
            case 4: return "Yellow";
            case 5: return "Magenta";
            case 6: return "Cyan";
            case 7: return "White";
            default: return "N/A";
        }
    } //not used currently

    public bool EarmarkInventory(string color, int quantity, EarmarkCategory category)
    {
        if (!inventory.ContainsKey(color) || inventory[color] < quantity)
        {
            return false;
        }

        if (!earmarks.ContainsKey(color))
        {
            earmarks[color] = new Dictionary<EarmarkCategory, int>();
        }

        if (!earmarks[color].ContainsKey(category))
        {
            earmarks[color][category] = 0;
        }

        earmarks[color][category] += quantity;
        inventory[color] -= quantity;
        return true;
    }

    public void ProcessWorkOrder(WorkOrder workOrder)
    {
        if (EarmarkInventory(workOrder.Color, workOrder.Quantity, workOrder.Category))
        {
            Debug.Log($"Successfully processed work order for {workOrder.Quantity} units of {workOrder.Color} for category {workOrder.Category}.");
        }
        else
        {
            Debug.Log($"Failed to process work order for {workOrder.Quantity} units of {workOrder.Color} for category {workOrder.Category}.");
        }
    }

    public void ShowInventoryStatus()
    {
        Debug.Log("Current Inventory:");
        foreach (var item in inventory)
        {
            Debug.Log($"Color: {item.Key}, Quantity: {item.Value}");
        }

        Debug.Log("Current Earmarks:");
        foreach (var color in earmarks)
        {
            foreach (var category in color.Value)
            {
                Debug.Log($"Color: {color.Key}, Category: {category.Key}, Quantity: {category.Value}");
            }
        }
    }

    public int key(string t)
    {
        return inventory[ColorToHex(t)];

    }

    public string MaxInventorySlot()
    {
        string key = "";

        if (inventory.Count > 0)
        {
            var filteredInventory = inventory.Where(kvp => kvp.Key == "Red" || kvp.Key == "Green" || kvp.Key == "Blue");

            if (filteredInventory.Any())
            {
                key = filteredInventory.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            }
        }
        Debug.Log("key:"+key);
        return key;
    }

    public string MinInventorySlot()
    {
        string key = "";

        if (inventory.Count > 0)
        {
            var filteredInventory = inventory.Where(kvp => kvp.Key == ColorToHex("Red") || kvp.Key == ColorToHex("Green") || kvp.Key == ColorToHex("Blue"));

            if (filteredInventory.Any())
            {
                key = filteredInventory.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            }
        }

        return key;
    }

    public int valueAtSlot(string t)
    {
        return inventory[t];
    }
    public void payBill(int c)
    {
        inventory[MaxInventorySlot()]-= c;

    }
    //////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////
    public void resetOre()
    {
        inventory["1#000000"] = 0;

    }
    public int getOre()
    {
        return inventory["1#000000"];

    }

    public void maxOre() //currently 10
    {
        inventory["1#000000"] = 10;

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////
    public bool executeTrade(Trade t)  //i is the index of the trade being executed
    {
        int availableQuantity = inventory[intToColorKey(t.sendColor)];
        int sendQ = t.sendQuantity;

        if (availableQuantity > sendQ)
        {
            inventory[intToColorKey(t.sendColor)] -= t.sendQuantity;  //swap send
            inventory[intToColorKey(t.recieveColor)] += t.recieveQuantity; //swap recieve
            return true;
        }
        else return false;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////
    public string intToColorKey(int colorInt)
    {
        switch (colorInt)
        {
            case 1: return "Red";
            case 2: return "Green";
            case 3: return "Blue";
            case 4: return "Yellow";
            case 5: return "Magenta";
            case 6: return "Cyan";
            case 7: return "White";
            default: return null;
        }
    }
    public string ColorToHex(string colorName)
    {
        if (colorToHex.ContainsKey(colorName))
        {
            return colorToHex[colorName];
        }
        else
        {
            return "Color not found";
        }
    }

}

public class WorkOrder
{
    public int Quantity { get; set; }
    public string Color { get; set; }
    public EarmarkCategory Category { get; set; }
    public bool isActive {  get; set; }
    
    public int machineIndex;

    public string name;

    public WorkOrder(int quantity, string color, EarmarkCategory category)
    {
        Quantity = quantity;
        Color = color;
        Category = category;
    }
}
