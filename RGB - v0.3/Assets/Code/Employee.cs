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
    public int sunSign;
    public int hometown;
   
    public float elapsedTime;
    public DateTime birthdate;

    public Employee(int job)
    {
        f = new FlavorText();
        stats = new int[3];

       

        elapsedTime = 0;
        firstName = f.rollDice(1,f.firstNames.Length)-1;
        lastName = f.rollDice(1, f.lastNames.Length)-1;
        hometown = f.rollDice(1,f.cities.Length)-1;
        
        this.job = job;
        this.age = 17 + f.rollDice(1,40);
        this.hobby = f.rollDice(1,f.hobbies.Length)-1;
        this.status = 0;
        this.compensation = f.rollDice(1,2);

        birthdate = f.GenerateRandomDate(f.currentYear - age);

        sunSign = 0;
        sunSign = setSunSign();
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

    public int setSunSign()
    {
        int day = birthdate.Day;
        int month = birthdate.Month;

        if ((month == 3 && day >= 21) || (month == 4 && day <= 19))
            return 1;
        else if ((month == 4 && day >= 20) || (month == 5 && day <= 20))
            return 2;
        else if ((month == 5 && day >= 21) || (month == 6 && day <= 20))
            return 3;
        else if ((month == 6 && day >= 21) || (month == 7 && day <= 22))
            return 4;
        else if ((month == 7 && day >= 23) || (month == 8 && day <= 22))
            return 5;
        else if ((month == 8 && day >= 23) || (month == 9 && day <= 22))
            return 6;
        else if ((month == 9 && day >= 23) || (month == 10 && day <= 22))
            return 7;
        else if ((month == 10 && day >= 23) || (month == 11 && day <= 21))
            return 8;
        else if ((month == 11 && day >= 22) || (month == 12 && day <= 21))
            return 9;
        else if ((month == 12 && day >= 22) || (month == 1 && day <= 19))
            return 10;
        else if ((month == 1 && day >= 20) || (month == 2 && day <= 18))
            return 11;
        else if ((month == 2 && day >= 19) || (month == 3 && day <= 20))
            return 12;
        else
            return -1;
    }
}