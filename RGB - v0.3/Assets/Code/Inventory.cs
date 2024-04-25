using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Inventory
{
    // Start is called before the first frame update

    private Dictionary<Pixel, int> inventory = new Dictionary<Pixel, int>();
    private System.Random random = new System.Random();


    Pixel ore = new Pixel(1, 0, 0, 0);

    #region static methods


    #region core
    public int ORE()
    {
        return inventory[ore];

    }

    public void setORE(int quantity)
    {
        inventory[ore] = quantity;

    }
    public void clearORE()
    {
        inventory[ore] = 0;

    }
    #endregion

    public static Inventory operator -(Inventory inventory, (Pixel p, int q) pixel)
    {
        // Check if the inventory contains the pixel and subtract the quantity
        if (inventory.inventory.ContainsKey(pixel.p))
        {
            inventory.inventory[pixel.p] -= pixel.q;
            // Optionally, remove the pixel from inventory if its quantity becomes 0 or less
            if (inventory.inventory[pixel.p] <= 0)
            {
                inventory.inventory.Remove(pixel.p);
            }
        }

        return inventory;
    }

    public static Inventory operator +(Inventory inventory, (Pixel p, int q) pixel)
    {
        // Check if the inventory contains the pixel and subtract the quantity
        if (inventory.inventory.ContainsKey(pixel.p))
        {
            inventory.inventory[pixel.p] += pixel.q;
            // Optionally, remove the pixel from inventory if its quantity becomes 0 or less
            if (inventory.inventory[pixel.p] <= 0)
            {
                inventory.inventory.Remove(pixel.p);
            }
        }

        return inventory;
    }

    #endregion

    #region operators
    public void addPixels(Pixel pixel, int quantity)
    {
        if (inventory.ContainsKey(pixel))
            inventory[pixel] += quantity;
        else inventory[pixel] = quantity;
    }

    public void subtractPixels(Pixel pixel, int quantity)
    {
        if (inventory.ContainsKey(pixel))
            inventory[pixel] -= quantity;
    }

    public void setQuantity(Pixel pixel,int q)
    {
        if (inventory.ContainsKey(pixel))
            inventory[pixel] = q;

    }
    #endregion

    #region sort

    public void SortByQuantity()
    {
        // Sort the inventory items by quantity in descending order
        var sortedInventory = inventory.OrderByDescending(item => item.Value)
                                        .ToDictionary(item => item.Key, item => item.Value);

        Console.Out.WriteLine("\n\nRESORTING THE INVENTORY BY QUANTITY\n\n");
        inventory = sortedInventory;
    }

    public void SortByLevel()
    {
        // Sort the inventory items by the level of the Pixel keys in ascending order
        var sortedInventory = inventory.OrderByDescending(item => item.Key.Level)
                                        .ToDictionary(item => item.Key, item => item.Value);

        Console.Out.WriteLine("\n\nRESORTING THE INVENTORY BY LEVEL\n\n");
        inventory = sortedInventory;
    }

    public void SortByLevelAndQuantity()
    {
        // Sort the inventory items first by the level of the Pixel keys in descending order,
        // and then within each level, sort by quantity in descending order
        var sortedInventory = inventory.OrderByDescending(item => item.Key.Level)
                                        .ThenByDescending(item => item.Value)
                                        .ToDictionary(item => item.Key, item => item.Value);

        Console.Out.WriteLine("\n\nRESORTING THE INVENTORY BY LEVEL AND QUANTITY\n\n");
        inventory = sortedInventory;
    }

    public void SortByRarity()
    {
        // Sort the inventory items first by the level of the Pixel keys in descending order,
        // and then within each level, sort by quantity in ascending order
        var sortedInventory = inventory.OrderByDescending(item => item.Key.Level)
                                        .ThenBy(item => item.Value)
                                        .ToDictionary(item => item.Key, item => item.Value);

        Console.Out.WriteLine("\n\nRESORTING THE INVENTORY BY HIGHEST LEVEL AND LEAST QUANTITY\n\n");
        inventory = sortedInventory;
    }

    #endregion

    #region get

    public void PrintInventory()
    {
        foreach (KeyValuePair<Pixel, int> entry in inventory)
        {
            string hexValue = new string(entry.Key.hex); // Get the hex value of the pixel as a string
            Console.Out.WriteLine("Level: " + entry.Key.Level + " | Hex Value: " + hexValue + " | Quantity: " + entry.Value);
        }
    }
    

    public Dictionary<Pixel,int> getInventory()
    {
        return inventory;
    }

    public int getEntry(Pixel p)
    {
        return inventory[p];
    }

    public KeyValuePair<Pixel, int> GetRandomEntry()
    {
        if (inventory.Count == 0)
        {
            throw new InvalidOperationException("Inventory is empty.");
        }

        int randomIndex = random.Next(inventory.Count);

        return inventory.ElementAt(randomIndex);
    }

    public int GetQuantity(Pixel pixel)
    {
        if (inventory.ContainsKey(pixel))
            return inventory[pixel];
        else return -1;
    }

    public int totalPixels()  //how many pixels are in the Inventory?
    {
        int quantity = 0;

        foreach (var kvp in inventory)
        {
            quantity += kvp.Value;
        }

        return quantity;

    }

    public int totalValue() //how much 
    {
        int value = 0;

        foreach (var kvp in inventory)
        {
            value += kvp.Key.pixelvalue();  //counts how much r,g,b is used to make this pixel
        }

        return value;
    }
    #endregion

    #region empty
    public void Clear()
    {
        inventory.Clear();

    }

    public bool isEmpty()
    {
        if(inventory.Count == 0) 
            return true;
        else return false;

    }

    #endregion

}