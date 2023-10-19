using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct Employee
{
    public int firstName,lastName;
    public int job;
    public int age;
    public int status;
    public int hobby; //values like this will all be index's in the master library of names, hobbies, flavortext, etc.
    FlavorText f;
    public Employee(int job)
    {

        f = new FlavorText();
        firstName = f.rollDice(1,f.firstNames.Length)-1;
        lastName = f.rollDice(1, f.lastNames.Length)-1;
        this.job = f.rollDice(1, f.factoryJobs.Length)-1;
        this.age = 14+f.rollDice(2,25);
        this.hobby = f.rollDice(1,f.hobbies.Length)-1;
        this.status = 0; 
        //hobby[0] = "Doesn't find anything particularly interesting";
        //hobby[1] = "loves dogs";
        //hobby[2] = "can't stop talking about the weather;
        //hobby[3] = "loves cats";
        //hobby[4] = "enjoys going to the movies";
        //hobby[5] = "loves cats";
        //Debug.Log($"First Name:{f.firstNames[fn]}/n Last Name:{f.lastNames[ln]}/n Job:{job}/n age:{age}/n Status:{status}");

    }

    public override string ToString()
    {
        return $"Person [First Name: {f.firstNames[firstName]}\nLast Name: {f.lastNames[lastName]}\nAge: {age}\nHobby: {f.hobbies[this.hobby]}\nStatus: {f.employmentStatuses[this.status]}";
    }


}