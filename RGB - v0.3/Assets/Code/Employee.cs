using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public struct Employee
{
    public int firstName, lastName;
    public int job;   //
    public int employmentStatus;
    public int age;
    public int status;  //1 = fulltime.
    public int hobby; //values like this will all be index's in the master library of names, hobbies, flavortext, etc.
    FlavorText ft;

    public System.Random r;

    public int[] skills;   // 0 = SPEED, RELIABILITY, INTELLIGENCE

    public Employee(int job)
    {
        r = new System.Random();
        ft = new FlavorText();


        firstName = ft.rollDice(1,ft.firstNames.Length); //random name
        lastName = ft.rollDice(1, ft.firstNames.Length);
        this.hobby = r.Next(0, ft.hobbies.Length); //random hobby

        employmentStatus = 0;
        this.job = job;
        
        this.status = 0;

        skills = new int[]{0,0,0};
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i] = ft.rollDice(1,20); 
        }
        age = 0;
        age = 14 + ft.rollDice(4, 8);
        
    }


    public override string ToString()
    {
        return $"Person [First Name: {ft.firstNames[firstName]}\nLast Name: {ft.lastNames[lastName]}\nAge: {age}\nHobby: {ft.hobbies[this.hobby]}\nStatus: {ft.employeeStatuses[this.status]}";
    }

    public void happybirthday()
    {
        age++;
    }

    public void retire()
    {
        status = 9;
    }
}