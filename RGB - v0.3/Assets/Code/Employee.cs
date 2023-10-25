using System;
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
    public int compensation;

    public float elapsedTime;
    public DateTime birthdate;

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
        this.compensation = f.rollDice(1,2);

        birthdate = f.GenerateRandomDate(f.currentYear - age);

        createStats();

    }

    public override string ToString()
    {
        return $"Person [First Name: {f.firstNames[firstName]}\nLast Name: {f.lastNames[lastName]}\nAge: {age}\nHobby: {f.hobbies[this.hobby]}\nStatus: {f.employmentStatuses[this.status]}\n Initiative: {stats[0]}\n Reliability: {stats[0]}\n Speed: {stats[0]}";
    }

    public void createStats()
    {
        stats[0] = f.rollDice(1, 3);  // SPEED
        //stats[0] = 1;  // SPEED
        stats[1] = f.rollDice(1, 2);  // RELIABILITY
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