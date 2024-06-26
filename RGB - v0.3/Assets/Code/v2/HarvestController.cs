using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class harvestController
{
    private int harvestCapacity;
    private int distribution;
    private int lastChosenColor;
    private int chosenColor;

    public harvestController(int i)
    {
        harvestCapacity = i;
    }


    public void increment()
    {


        if(distribution < harvestCapacity && chosenColor != 0 )  // we don't know chosenColor here
        {
            distribution++;
        }

        else if (distribution >= harvestCapacity)  //is this needed?
        {
            distribution = harvestCapacity;
            //SET TEXT TO RED
            //UPDATE EVENTS
        }    

    }

    public (Pixel, int) distribute()
    {


        int tempDistr = distribution;
        reset();

        return (new Pixel(1, 1, 0, 0), tempDistr);

    }


    void reset()
    {
        distribution = 0;
        lastChosenColor = chosenColor;
    }

    public void upgradeCapacity()
    {
        harvestCapacity++;
    }
}
