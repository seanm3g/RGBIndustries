using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Contract
{
    #region variables
    public LogicCenter lc = new LogicCenter();
    public FlavorText ft = new FlavorText();

    public int[] requirements;
    public int totalRequirements;
    public int totalValue;

    public float deadline;
    #endregion


    public Contract()
    {

        requirements = new int[8];
        for(int i = 0;i<requirements.Length;i++)
        {
            requirements[i] = -1;
        }


    }
    public Contract(int[,] pixelValues,float deadline)
    {

        requirements = new int[8];
        setRequirements(pixelValues);
        this.deadline = deadline;
        

    }
    public void setRequirements(int[,] pixelValues)
    {
        for (int i = 0; i < requirements.Length; i++)  //init to zero
        {
            requirements[i] = 0;
        }

        for (int x = 0; x < pixelValues.GetLength(0); x++)
            for (int y = 0; y < pixelValues.GetLength(1); y++)
                switch (pixelValues[x,y])
                {
                    case 1: requirements[1]++; break;
                    case 2: requirements[2]++; break;
                    case 3: requirements[3]++; break;
                    case 4: requirements[4]++; break;
                    case 5: requirements[5]++; break;
                    case 6: requirements[6]++; break;
                    case 7: requirements[7]++; break;
                }

        updateTotal();
        updateTotalValue();
    }

    /*
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
    }*/
    public void convertToWorkOrder()
    {
        Debug.Log("CONVERT TO WORK ORDER");
        Debug.Log(requirements.Length);

        for (int i = 0; i < requirements.Length; i++)
        {
            Debug.Log("i" + i);
            if (i < 3) 
            {
                //if(i == lc.chosenColor)
                    //wait
                //else
                    //tradeOrder(i,requirements[i]);
            }
            else if (i > 3)
            {
                Debug.Log("requirements[i"+i+"]"+requirements[i]);

                switch (i)
                {
                    case 4:
                        lc.ProcessingQueue.Add(new workOrder(requirements[i], 1, 2, 4));
                        break;
                    case 5:
                        lc.ProcessingQueue.Add(new workOrder(requirements[i], 1, 3, 5));
                        break;
                    case 6:
                        lc.ProcessingQueue.Add(new workOrder(requirements[i], 2, 3, 6));
                        break;
                    case 7:
                        int randomCase = ft.rollDice(1, 3);
                        int param1 = randomCase == 1 ? 3 : randomCase == 2 ? 2 : 1;
                        int param2 = randomCase + 3;
                        lc.ProcessingQueue.Add(new workOrder(requirements[i], param1, param2, 7));
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

    /*
    public float currentlyAvailabile()
    {
        int requiredPixels = 0;

        for (int i = 0; i < requirements.Length; i++)  //if you have enough
        {
            int job = requirements[i];
            int available = lc.inventory[i];

            if (job > available)
                requiredPixels += job - available;
        }

        float difference = requiredPixels / (float)totalRequirements;

        if (difference == float.NaN) //this doesn't seem to work.
            return 0f;
        else return (1f - difference);
      
    }*/

}
