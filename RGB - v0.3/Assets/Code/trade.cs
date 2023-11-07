using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Trade
{
    // Start is called before the first frame update

    FlavorText f;

    public float cadence;
    public float length;
    public int sendQuantity;
    public int sendColor;
    public int recieveQuantity;
    public int recieveColor;
    
    public float elapsedTime;
    public bool isActive;

    public int quantity;

    public Trade(int t)
    {
        f = new();
        quantity = 0;
        //cadence = 15*f.rollDice(1,4);
        //length = f.rollDice(1, 10);
        cadence = 1;
        length = 1;
        sendQuantity = 5 * f.rollDice(1, 10);  //sets the initial quantity
        sendColor = f.rollDice(1,7);
        recieveColor = f.rollDice(1,7);
        while (recieveColor == sendColor) //if it's the same, then redo it.
            recieveColor = f.rollDice(1,7);

        recieveQuantity = 0;

       
        
        elapsedTime = 0;
        isActive = false;

        setRecieveQuantity();  //sets the quantity so that there somewhat balanced
    }

    public void makeActive()
    {
        isActive = true;
    }

    private void setRecieveQuantity()  //sets up the trade to have arbitrage and be random
    {
        float sendModifier = 1;
        float recieveModifier = 1;

        // Assuming sendColor and recieveColor are integers declared elsewhere
        if (sendColor < 4)
            sendModifier = 1;
        else if (sendColor < 7)
            sendModifier = 2;
        else
            sendModifier = 3;

        if (recieveColor < 4)
            recieveModifier = 1;
        else if (recieveColor < 7)
            recieveModifier = 2;
        else
            recieveModifier = 3;

        // Cast sendQuantity to float for the calculation, then cast the result back to int
        recieveQuantity = (int)((float)sendQuantity * sendModifier / recieveModifier);  //compares the amount of each color to adjust

        if(recieveQuantity<0)
        {
            recieveQuantity = recieveQuantity * -1;
            sendQuantity += recieveQuantity;
        }
        // Assuming f is an object with a rollDice method
        if (f.rollDice(1, 2) > 1)  //randomly do one or the other.
            recieveQuantity += f.rollDice(1, 5);
        else
            recieveQuantity -= f.rollDice(1, 5);
    }

    public bool isFulfilled()
    {

        if (quantity >= sendQuantity) //if the order is full  
            return true;
        else
            return false;
    }
}
