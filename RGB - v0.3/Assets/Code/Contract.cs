using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Contract : MonoBehaviour
{
    #region variables
    public LogicCenter lc = new LogicCenter();
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
    public void convertToWorkOrder()  //this is maybe getting truncated?
    {
        for (int i = 4; i < requirements.Length; i++)
        {
            if (requirements[i] < 3) 
            {
                //if(i == lc.chosenColor)
                    //wait
                //else
                    //tradeOrder(i,requirements[i]);
            }
            else if (requirements[i] > 3)  //this shouldn't be calling production queue.  It should call a method in LC
            {
                switch (i)
                {
                    case 4:
                        lc.ProductionQueue.Add(new workOrder(requirements[i], 1, 2, 4,3));
                        break;
                    case 5:
                        lc.ProductionQueue.Add(new workOrder(requirements[i], 1, 3, 5,3));
                        break;
                    case 6:
                        lc.ProductionQueue.Add(new workOrder(requirements[i], 2, 3, 6,3));
                        break;
                    case 7:
                        int randomCase = ft.rollDice(1, 3);
                        int param1 = randomCase == 1 ? 3 : randomCase == 2 ? 2 : 1;
                        int param2 = randomCase + 3;
                        lc.ProductionQueue.Add(new workOrder(requirements[i], param1, param2, 7,3));
                        break;
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
