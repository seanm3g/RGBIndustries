using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;

public class EmployeeController
{
    private List<Employee> employees { get; set;}
    
    private MachineController machineController;
    //private WorkOrderController workOrderController;  //not sure if this needed yet.

    public EmployeeController(int initialCapacity)
    {
        employees = new List<Employee>(initialCapacity);
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
        foreach (var employee in employees)
        {
            runEmployee(employee);

        }
    }

    private void runEmployee(Employee e)
    {
        e.elapsedTime += Time.deltaTime;

        if (e.rollStat(Employee.INIT))  //checks intelligence
        {
            takesInitiative(e);
        }
        else
        {
            //reset timer
            e.status = Employee.IDLE;   //employee didn't do anything.
        }
    }

    private void takesInitiative(Employee e)  //action is taken
    {
        if (e.rollStat(Employee.REL) || e.rollStat(Employee.REL))  //reliablity // DO they do their job or something else?
        {
            e.status = Employee.ACTIVE;

            switch (e.job)  //this might not be necessary.
            {
                case 0: break;
                case 1: employeeHarveter(e); break;  //should jobs be a class?
                case 2: employeeRunMachine(e); break;
                case 3: employeeManage(e); break;   //adds a roll to their teams's reliability
                case 4: employeeAssign(e); break;   // assign work order tasks.
            }

        }
        else
            employeeMisbehave(e);

    }


    public List<Employee> GetActiveEmployees()
    {
        return employees.FindAll(e => e.status == 1); // Assuming status 1 means active or working


    }

    private void employeeHarveter(Employee e)    //job
    {

    }

    private void employeeRunMachine(Employee e)  //job
    {

    }

    private void employeeManage(Employee e)  //manage
    {

    }

    private void employeeAssign(Employee e)   //assign
    {


    }

    private void employeeMisbehave(Employee e)
    {
        e.status = Employee.MISBEHAVE;
    }

    public List<Employee> getEmployees()
    {
        return employees;
    }
}

