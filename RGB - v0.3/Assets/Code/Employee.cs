using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Employee
{
    public int firstName,lastName;
    public int job;
    public int age;
    
    public int hobby; //values like this will all be index's in the master library of names, hobbies, flavortext, etc.
    FlavorText f;



    public int status;
    
    public const int INIT = 1;
    public const int REL = 2;
    public const int SPE = 3;

    public const int IDLE = 0;
    public const int ACTIVE = 1;
    public const int MISBEHAVE = 2;

    public int assignedMachineId;  //the id of the machine assigned (what is this?)


    public (int initalization, int reliability,int speed) stats{ get; private set; }
    public (int initalization, int reliability, int speed) statsMax { get; private set; }
    public int compensation;
    public int sunSign;
    public int hometown;
   
    public float elapsedTime;
    public DateTime birthdate;

    public Employee(int job)
    {
        f = new FlavorText();

        elapsedTime = 0;

        statsMax = (20,20,20);

        stats = (f.rollDice(1, statsMax.initalization), 
                 f.rollDice(1, statsMax.reliability), 
                 f.rollDice(1, statsMax.speed));

        firstName = f.rollDice(1,f.firstNames.Length)-1;
        lastName = f.rollDice(1, f.lastNames.Length)-1;
        hometown = f.rollDice(1,f.cities.Length)-1;
        
        this.job = job;
        this.age = 17 + f.rollDice(1,40);
        this.hobby = f.rollDice(1,f.hobbies.Length)-1;
        this.status = 0;
        this.compensation = f.rollDice(1,2);

        birthdate = f.GenerateRandomDate(f.currentYear - age);

        sunSign = setSunSign();

    }

    public override string ToString()
    {
        return $"Person [First Name: {f.firstNames[firstName]}\nLast Name: {f.lastNames[lastName]}\nAge: {age}\nHobby: {f.hobbies[this.hobby]}\nStatus: {f.employmentStatuses[this.status]}\n Initiative: {stats.initalization}\n Reliability: {stats.reliability}\n Speed: {stats.speed}";
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
    public void isBirthday(int month, int day)
    {
        if(month==birthdate.Month&& day ==birthdate.Day)
            age++;
            //event (happy birthday!);

    }


    public bool rollStat(int i)  //checks the employees specific stat
    {
        bool pass;

        int tempStat = 0;
        switch(i)
        {
            case 1:
                tempStat = stats.initalization;
                break;
            case 2:
                tempStat = stats.reliability;
                break;
            case 3:
                tempStat = stats.speed;
                break;
        }

        if (f.rollDice(1, 20) < tempStat)
            pass = true;
        else pass = false;

        return pass;
    }

    public int getStat(int i)
    {

        int tempStat = 0;

        switch (i)
        {
            case 1:
                tempStat = stats.initalization;
                break;
            case 2:
                tempStat = stats.reliability;
                break;
            case 3:
                tempStat = stats.speed;
                break;
        }

        return tempStat;
    }
}