public class harvestController
{
    private int harvestCapacity;
    private int distribution;
    private int lastChosenColor;
    private int chosenColor;

    private Pixel chosen;
    private Inventory inventory;

    public harvestController(int capacity, Inventory inv)  
    {
        inventory = inventory;
        harvestCapacity = capacity;
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

    

    public void distribute()
    {
        inventory.addPixels(chosen, distribution);
        reset();
    }

    public void setToMin()
    {
        updateSelection(inventory.getMinPrimitive());


    }

    public void updateSelection(Pixel p)  //there's no safeguard on this now
    {
        chosen = p;
    }


    void reset()
    {
        distribution = 0;
        lastChosenColor = chosenColor;  //what does this do?
    }

    public void upgradeCapacity()
    {
        harvestCapacity++;
    }
}
