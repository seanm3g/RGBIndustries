using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavorText
{
    public string[] firstNames = { "Alice", "Bob", "Charlie", "David", "Emily", "Fiona", "George", "Hannah", "Ivy", "Jack" };
    public string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
    public string[] hobbies = { "Reading", "Swimming", "Cycling", "Hiking", "Gaming", "Cooking", "Painting", "Fishing", "Photography", "Music" };
<<<<<<< Updated upstream
    public string[] factoryJobs = { "Assembler", "Machine Operator", "Quality Inspector", "Forklift Driver", "Maintenance Technician", "Welder", "Packager", "Material Handler", "Production Supervisor", "Safety Manager" };
    public string[] employeeStatuses = { "Full-Time", "Part-Time", "Temporary", "Contract", "Intern", "Freelance", "Remote", "On Leave", "Unemployed", "Retired" };
    public string[] machineStatuses = { "Idle", "Running", "Faulted", "Blocked", "Starved", "Changeover", "Maintenace", "Available", "Ready" };
=======
    public string[] factoryJobs = { "N/A","Harvester","Assembler", "Machine Operator", "Quality Inspector", "Forklift Driver", "Maintenance Technician", "Welder", "Packager", "Material Handler", "Production Supervisor", "Safety Manager" };
    public string[] employeeStatuses = {"Unemployeed","Full-Time", "Part-Time", "Temporary", "Contract", "Intern", "Freelance", "Remote", "On Leave", "Unemployed", "Retired", "Dead"};
    public string[] machineStatuses = { "IDLE", "RUNNING", "Faulted", "Blocked", "Starved", "Changeover", "Maintenace", "Available", "Ready" };
>>>>>>> Stashed changes
    public string[] factoryEvents = { "Machine Malfunction", "Safety Inspection", "Inventory Restock", "Employee Training", "Quality Control Audit", "Shift Change", "Power Outage", "Scheduled Maintenance", "Product Launch", "Emergency Drill" };
    public string[] eventStatuses = { "", "Red Chosen!", "Green Chosen!", "Blue Chosen!", "Job is processed", "Machine is Running", "Random Event/Flavor Text", "Machine is Starved", "harvesting...", "Upgrade Harvest Capacity", "Harvester at capacity", "Maintenance", "Refinery Started", "Seasonal", "System","Production Idle"};
    public string[] woStatuses = {"N/A","In Queue","Loading","In Production","Unloading","Completed","Incomplete",""};
    //public string[][] int = { [3,3,] };
    // A way of storing a bunch of arrays.

    public string generateMachineName()
    {
        // Define the characters and numbers to be used in each segment
        string segment1Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string segment2Numbers = "0123456789";
        string segment3Chars = "abcdefghijklmnopqrstuvwxyz";

        // Initialize the machine machineName as an empty string
        String machineName = "";

        // Generate the first segment (e.g., "TJZ")
        for (int i = 0; i < 3; i++)
        {
            char randomChar = segment1Chars[UnityEngine.Random.Range(0, segment1Chars.Length)];
            machineName += randomChar;
        }

        // Add a dash
        machineName += "-";

        // Generate the second segment (e.g., "523")
        for (int i = 0; i < 3; i++)
        {
            char randomNum = segment2Numbers[UnityEngine.Random.Range(0, segment2Numbers.Length)];
            machineName += randomNum;
        }

        // Add another dash
        machineName += "-";

        // Generate the third segment (e.g., "j42")
        char randomChar3 = segment3Chars[UnityEngine.Random.Range(0, segment3Chars.Length)];
        machineName += randomChar3;
        for (int i = 0; i < 2; i++)
        {
            char randomNum = segment2Numbers[UnityEngine.Random.Range(0, segment2Numbers.Length)];
            machineName += randomNum;
        }
        return machineName;
    }

    public string orderName()
    {
        string segment1Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        String machineName = "";

        // Generate the first segment (e.g., "TJZ")
        for (int i = 0; i < 3; i++)
        {
            char randomChar = segment1Chars[UnityEngine.Random.Range(0, segment1Chars.Length)];
            machineName += randomChar;
        }

        return machineName;
    }

    public int rollDice(int dice, int sides)  //rolls x dice of 1 to max
    {
        System.Random r = new System.Random();
        int roll = 0;

        for (int i = 0; i < dice; i++)
        {
            roll += r.Next(1, sides + 1);
        }

        return roll;
    }

    public bool skillCheck(int val)
    {
        if (rollDice(1, 20) <= val)
            return true;
        return false;

    }
}
