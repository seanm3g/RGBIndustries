using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Contract
{
    #region variables
    public LogicCenter lc;
    public FlavorText ft = new FlavorText();

    public (Inventory remaining, Inventory total) inventory;

    public string clientName;
    public string contractName;
    public int status = 0;
    public float progress;
    #endregion


    public Contract()
    {
        clientName = ft.companyNames[ft.rollDice(1, ft.companyNames.Length) - 1];
        contractName = ft.contractName[ft.rollDice(1, ft.contractName.Length) - 1];


        inventory.total = new Inventory();
        inventory.remaining = new Inventory();

    }

    public Contract(Pixel[,] reqs)
    {
        clientName = ft.companyNames[ft.rollDice(1,ft.companyNames.Length)-1];
        contractName = ft.contractName[ft.rollDice(1, ft.contractName.Length) - 1];
        setRequirements(reqs);



        inventory.total = new Inventory();
        inventory.remaining = new Inventory();

    }

    public void setRequirements(Pixel[,] pixelValues)
    {

        inventory.total.Clear();


        for (int i = 0; i < pixelValues.GetLength(0); i++)
            for (int j = 0; j < pixelValues.GetLength(1); j++)
                inventory.total.addPixels(pixelValues[i, j],1);  //adds quantity per pixel in the image.

        //updateRequirements();


    }


    public Dictionary<Pixel, int> getInventory()  //this has to possible returns
    {
            return inventory.total.getInventory();
    }

    public Dictionary<Pixel, int> remainingPixels()  //this has to possible returns
    {
        return inventory.remaining.getInventory();
    }

    public int uniquePixelsLeft()
    {
        return inventory.remaining.getInventory().Count;
    }

    public int getEntry(Pixel p)
    {
        return inventory.remaining.getInventory()[p];
    }

    public void subtractPixels(Pixel p, int q)
    {
        inventory.remaining.subtractPixels(p,q);

    }

    public void setQuantity(Pixel p, int q)
    {
        inventory.remaining.setQuantity(p, q);

    }
    public bool isComplete()
    {
        if(inventory.remaining.isEmpty())
            return true;
        else return false;
    }

    public int totalRequirements()
    {

        return inventory.total.totalPixels();

    }

}
