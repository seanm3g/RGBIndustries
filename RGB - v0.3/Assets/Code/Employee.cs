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
    public int[] stats;

    public float elapsedTime;
    public Employee(int job)
    {

        f = new FlavorText();
        stats = new int[3];
        
        elapsedTime = 0;
        firstName = f.rollDice(1,f.firstNames.Length)-1;
        lastName = f.rollDice(1, f.lastNames.Length)-1;
        //this.job = f.rollDice(1, f.factoryJobs.Length)-1;  //sets to random
        this.job = job;
        this.age = 17 + f.rollDice(1,40);
        this.hobby = f.rollDice(1,f.hobbies.Length)-1;
        this.status = 0;

        createStats();
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

    public void createStats()
    {
        stats[0] = f.rollDice(3, 20);  // SPEED
        stats[0] = 20 - stats[0];
        if (stats[0] < 1)
            stats[0] = 1;
        stats[1] = f.rollDice(1, 20);  // RELIABILITY
        stats[2] = f.rollDice(1, 20);  // INTELLIGENCE
    }

    public int getSpeed()
    {
        return stats[0];
    }

    public int getReliability()
    {
        return stats[1];
    }

    public int getIntelligence()
    {
        return stats[2]; 
    }


}