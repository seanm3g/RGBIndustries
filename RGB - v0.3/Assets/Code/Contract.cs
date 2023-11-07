using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Contract : MonoBehaviour
{
    #region variables
    public LogicCenter lc;
    public FlavorText ft = new FlavorText();

    public int[] requirements;
    public int totalRequirements;
    public int totalValue;
    #endregion

    public Contract()
    {
        requirements = new int[8];

        for (int i = 0; i < requirements.Length; i++)  //init to zero
        {
            requirements[i] = -1;
        }

        
    }
    public void setRequirements(int[,] pixelValues)
    {
        for (int i = 0; i < requirements.Length; i++)
            requirements[i] = 0;
        if (pixelValues.GetLength(0) > 0)
            for (int i = 0; i < pixelValues.GetLength(0); i++)
                for (int j = 0; j < pixelValues.GetLength(1); j++)
                    requirements[pixelValues[i, j]]++;  //adds quantity per pixel in the image.



        updateTotal();
        updateTotalValue();
    }
    public void fillRequirements()
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
    public void convertToWorkOrder() //this creates a local copy of LC for some reason
    {
        for (int i = 1; i < requirements.Length; i++)
        {
            // Correcting the logic to subtract the requirements from inventory when enough items are available
            if (i <= 3)
            {
                if (lc.inventory[i] >= requirements[i])  // Check if you have enough inventory to meet the requirements
                {
                    lc.inventory[i] -= requirements[i];  // Subtract requirements from inventory
                    requirements[i] = 0;  // Set the requirements for this item to 0 since it's been fulfilled
                }
            }
            else // This can be just 'else' since the condition is the direct opposite of 'if (i <= 3)'
            {
                switch (i)
                {
                    // Adding work orders to the production queue
                    case 4:
                        Debug.Log("Before PQ update: " + lc.ProductionQueue.Count);
                        lc.ProductionQueue.Add(new workOrder(requirements[i], 1, 2, 4, 3));
                        Debug.Log("After PQ update: " + lc.ProductionQueue.Count);
                        break;
                    case 5:
                        lc.ProductionQueue.Add(new workOrder(requirements[i], 1, 3, 5, 3));
                        break;
                    case 6:
                        lc.ProductionQueue.Add(new workOrder(requirements[i], 2, 3, 6, 3));
                        break;
                    case 7:
                        int randomCase = ft.rollDice(1, 3);
                        int param1 = randomCase == 1 ? 3 : randomCase == 2 ? 2 : 1;
                        int param2 = randomCase + 3;
                        lc.ProductionQueue.Add(new workOrder(requirements[i], param1, param2, 7, 3));
                        break;
                        // No default case is necessary if all cases are covered
                }
            }
        }
    }

    public void updateTotal()
    {
        totalRequirements = 0;

        for (int i = 0; i < requirements.Length; i++)
            totalRequirements += requirements[i];
    }
    public void updateTotalValue()
    {
        totalValue = 0;
        int valueMultiplier = 1;

        for (int i = 0; i < requirements.Length; i++)
        {
            if (i < 4)
                valueMultiplier = 1;
            else if (i < 7)
                valueMultiplier = 2;
            else valueMultiplier = 3;

            totalValue += requirements[i] * valueMultiplier;
        }         
    }
    public bool isComplete()
    {
        for(int i = 0; i < requirements.Length; i++)
        {
            if (requirements[i] != 0)  //if any entry has some left
            {
                return false;
            }
        }
        return true;
    }

    public bool isFulfilled()
    {
        if (totalRequirements == 0)
            return true;
        else 
            return false;
    }

}
