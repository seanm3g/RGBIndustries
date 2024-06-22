using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController
{
    // Start is called before the first frame update
{

    

    void setup()
    {

        //do stuff

    }

// Update is called once per frame

    public void startingPage()  //sets the window that shows up when the game is run.
    {
        SelectMenu.SetActive(true);
        ProductionPage.SetActive(false);
        newWorkOrderWindow.SetActive(false);
    }
}
