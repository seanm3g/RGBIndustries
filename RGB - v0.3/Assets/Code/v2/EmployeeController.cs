using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeController
{
    private List<Employee> employees { get; set; }  //where all of the employees information lives.

    FlavorText ft = new();
    private MachineController mc;
    private harvestController harvest;
    //private WorkOrderController workOrderController;  //not sure if this needed yet.

    public EmployeeController(int initialCapacity, harvestController harvest, MachineController mc)
    {
        employees = new List<Employee>(initialCapacity);
        this.harvest = harvest;
        this.mc = mc;
    }

    public void HireEmployee(Employee employee)
    {
        employees.Add(employee);
    }

    public void FireEmployee(Employee employee)
    {
        employees.Remove(employee);
    }

    public void update()
    {
        // Update logic for employees, such as aging or status changes

        bool init = false;
        bool rel = false;

        List<Employee> IDLEEmployees = getIdleEmployees();

        foreach (var employee in employees)
        {
            init = employee.rollStat(Employee.INIT);
            rel = employee.rollStat(Employee.REL);
            if (init && rel)                            //productive!
                employee.status = Employee.ACTIVE;
            else if (init && !rel)                      //unproductive
                employee.status = Employee.MISBEHAVE;
            else employee.status = Employee.IDLE;       //lazy

        }

        ExecuteEmployeeJob();
    }

    public void ExecuteEmployeeJob()
    {
        List<Employee> activeEmployees = getActiveEmployees();

        foreach (var e in activeEmployees)
        {
            if (e.status != Employee.IDLE)
            {
                if (e.status == Employee.ACTIVE)
                    switch (e.job)
                    {
                        case 1:
                            employeeHarvest();
                            break;
                        case 2:
                            assignWorkOrder();
                            break;
                        case 3:
                            harvestSelect(); //updates the color being harvested
                            break;
                        case 4:
                            employeeManage();  //adds bonus to employee rolls
                            break;
                        case 5:
                            employeeRunMachine(e);
                            break;
                            // Add other job types as needed
                    }
                else
                    employeeMisbehave(e);

                e.status = Employee.IDLE;  //this needs to happen at the end of the task. several cycles?
            }
        }
    }

    #region JOBS

    private void employeeHarvest()
    {
        harvest.distribute();  //shares Logiccenter2
    }

    private void harvestSelect()
    {
        harvest.setToMin();
    }

    void assignWorkOrder()
    {
    }

    private void employeeManage()
    {
    }

    private void employeeRunMachine(Employee e)
    {

        //does this happen here, or just trigger the flag that happens when the employee is active?
        //is it weird for this to be only true for this specific job?

        /*
        if (e.assignedMachineId > 0)
        { machines.update(Time.deltaTime); }
        */

    }

    #endregion


    #region MISBEHAVIOR
    private void employeeMisbehave(Employee e)
    {
        int roll = ft.rollDice(1, 100);

        const int severeThreshold = 99;
        const int moderateThreshold = 98;
        const int mildThreshold = 70;


        if (roll > severeThreshold)  //did a bad thing
            severeBehavior(e);
        else if (roll > moderateThreshold)  //did a meh thing
            moderateBehavior(e);
        else if (roll > mildThreshold)  //did a mild thing
            mildBehavior(e);
        else
            doNothing();  //zoned out

        // roll for nothing, minor, major, catastrophic
        // pull event from those lists
        // find people for the event
        // adjust data vales, break things
    }

    public void mildBehavior(Employee e)  //this should be a table
    {
        int roll = ft.rollDice(1, 5);

        switch (roll)
        {
            case 1: gossip(e); break;
            case 2: fallAsleep(e); break;
            case 3: break;
            case 4: break;
            case 5: break;

        }
    }

    private void doNothing(){ }

    public void gossip(Employee e)
    {
        int roll = ft.rollDice(1,100);
        Employee other = null;
        if (roll > 80)
            other = e.getRandomRelationship().other;

        else other = getRandomEmployee();

        //pull from existing relatoinships 80% of the time. 20% other people.
    }

    private void fallAsleep(Employee e)
    {
    }

    public void moderateBehavior(Employee e)
    {
        int roll = ft.rollDice(1, 5);

        switch (roll)
        {
            case 1: fight(e); break;
            case 2: breakMachine(e);  break;
            case 3: break;
            case 4: break;
            case 5: break;

        }

    }

    private void breakMachine(Employee e)
    {
        //e.breakMachine(e);
        mc.breakMachine(e.assignedMachineId);

    }

    private void fight(Employee e) { }
    public void severeBehavior(Employee e)
    {
        int roll = ft.rollDice(1, 5);

        switch (roll)
        {
            case 1: die(e); break;
            case 2: break;
            case 3: break;
            case 4: break;
            case 5: break;

        }

    }

    private void die(Employee e)
    {
        

    }

    #endregion


    #region getMethods

    public Employee GetEmployeeById(int id)
    {
        return employees.Find(e => e.ID == id);
    }


    public List<Employee> getEmployees()
    {
        return employees;
    }

    public List<Employee> getActiveEmployees()
    {
        return employees.FindAll(e => e.status == Employee.ACTIVE);

    }

    public List<Employee> getIdleEmployees()
    {
        return employees.FindAll(e => e.status == Employee.IDLE);

    }

    public Employee getRandomEmployee()
    {
        int upper = employees.Count-1;  //does this need -1?
        return employees[ft.rollDice(1, upper)];

    }
    #endregion
}

