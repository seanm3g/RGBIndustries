using System.Collections.Generic;

public class ContractController
{



    public List<Contract> contracts { get; set; }

    public ContractController(int i)
    {
        contracts = new List<Contract>(i);


    }
    public void setup()
    {

        //do stuff

    }

    public void update()
    {


    }
}
