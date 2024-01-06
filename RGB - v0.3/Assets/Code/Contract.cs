using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Contract
{
    #region variables
    public LogicCenter lc;
    public FlavorText ft = new FlavorText();

    public int[] requirements;
    public int currentRequirements;  //what is the remaining pixels required
    public int currentCost;  //what is remaining cost to be filled?

    public int totalRequirements;  //what is the total number of pixels required
    public int totalCost;  //what is the total cost of those pixels?

    

    public string clientName;
    public string contractName;
    public int status = 0;
    public float progress;
    #endregion

    public Contract()
    {
        clientName = ft.companyNames[ft.rollDice(1,ft.companyNames.Length)-1];
        contractName = ft.contractName[ft.rollDice(1, ft.contractName.Length) - 1];
        requirements = new int[8];

        for (int i = 0; i < requirements.Length; i++)  //init to zero
        {
            requirements[i] = -1;
        }
    }

    public void setRequirements(int[,] pixelValues)
    {
        for (int i = 0; i < requirements.Length; i++)  //sets to zero
            requirements[i] = 0;


        for (int i = 0; i < pixelValues.GetLength(0); i++)
            for (int j = 0; j < pixelValues.GetLength(1); j++)
                requirements[pixelValues[i, j]]++;  //adds quantity per pixel in the image.



        //Debug.Log("CYAN REQUIREMENTS:" + requirements[6]);
        //Debug.Log("MAGENTA REQUIREMENTS:" + requirements[5]);

        updateCurrentRequirements();
        setTotal();
        setTotalCost();

        /*
         * updateTotal();
         updateCurrentCost();
         */
    }

    public void setTotal()
    {
        totalRequirements = 0;

        for (int i = 1; i < requirements.Length; i++) //start at one to remove canvas color
            totalRequirements += requirements[i];
    }
    public void fillRequirements()  //this isn't getting called anywhere.
    {
        for (int i = 0;i < requirements.Length;i++)
        {
            int req = requirements[i];
            int inv = lc.inventory[i];

            if (req > 0 && inv > 0)
            {
                if (req > inv)
                {
                    req -= inv;
                    inv = 0;

                }
                else //inv >= req
                {
                    inv -= req;
                    req = 0;
                }
            }
            requirements[i] = req;
            lc.inventory[i] = inv;
        }
    }
    public void updateCurrentRequirements()  //this isn't getting updated.
    {
        currentRequirements = 0;

        for (int i = 1; i < requirements.Length; i++) //start at one to ignore canvas color
        {
            currentRequirements += requirements[i];
        }

        if (totalRequirements > 0)
            progress = 1-((float)currentRequirements / totalRequirements);
    }

    public void setTotalCost()  //cost of the pixels (based on expense of making each pixel)
    {
        totalCost = 0;
        int valueMultiplier = 1;

        for (int i = 0; i < requirements.Length; i++)
        {
            if (i < 4)
                valueMultiplier = 1;
            else if (i < 7)
                valueMultiplier = 2;
            else valueMultiplier = 3;

            totalCost += requirements[i] * valueMultiplier;
        }
    }
    public void updateCurrentCost()
    {
        currentCost = 0;
        int valueMultiplier = 1;

        for (int i = 0; i < requirements.Length; i++)
        {
            if (i < 4)
                valueMultiplier = 1;
            else if (i < 7)
                valueMultiplier = 2;
            else valueMultiplier = 3;

            currentCost += requirements[i] * valueMultiplier;
        }         
    }
    public bool isComplete()
    {
        for(int i = 1; i < requirements.Length; i++)
        {
            if (requirements[i] != 0)  //if any entry has some left
            {
                return false;
            }
        }
        return true;
    }

}
