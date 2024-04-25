using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Trade
{
    // Start is called before the first frame update

    FlavorText f;

    public float cadence;           //frequency
    public float length;            //

    public (Pixel pixel, int quantity) send, recieve;
    
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
        send.quantity = 5 * f.rollDice(1, 10);  //sets the initial quantity
        send.pixel = Pixel.randomPixel(1);  //this shouldn't be 1 by default
        recieve.quantity = 5 * f.rollDice(1, 10);  //sets the initial quantity
        recieve.pixel = Pixel.randomPixel(1);  //this shouldn't be 1 by default

        while (recieve.pixel == send.pixel) //if it's the same, then redo it.
            recieve.pixel = Pixel.randomPixel(1);

        recieve.quantity = 0;

       
        
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


        if (Pixel.isPrimary(send.pixel))
            sendModifier = 1;
        else if (send.pixel != new Pixel(1, 1, 1, 1))
            sendModifier = 2;
        else sendModifier = 3;

        if (Pixel.isPrimary(recieve.pixel))
            recieveModifier = 1;
        else if (recieve.pixel != new Pixel(1, 1, 1, 1))
            recieveModifier = 2;
        else recieveModifier = 3;

        recieve.quantity = (int)((float)send.quantity * sendModifier / recieveModifier);

        if (recieve.quantity < 0)  //what does this code do?
        {
            recieve.quantity = recieve.quantity * -1;
            send.quantity += recieve.quantity;
        }

        if (f.rollDice(1, 2) > 1)
            recieve.quantity += f.rollDice(1, 5);
        else
            recieve.quantity -= f.rollDice(1, 5);
    }

    public bool isFulfilled()
    {

        if (quantity >= send.quantity) //if the order is full  
            return true;
        else
            return false;
    }
}
