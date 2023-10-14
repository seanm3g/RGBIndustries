using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public struct Pixel
{

    public int level;
    public ColorRGB c;

    public Pixel(int l, ColorRGB c)
    {
        this.level = l;
        this.c = c;
        
    }

    public int[] leveled()  //returns the value of the color by level translated to 8bit.
    {
        int[] vals = new int[3];
        for(int i=0; i<vals.Length;i++)
        {
            switch (level)
            {
                case 1:
                    vals[i] = level * 255;
                    break;
                case 2:
                    vals[i] = level * 85;
                    break;
                case 3:
                    vals[i] = level * 32;
                    break;
            }
        }
        return vals;
    }

    public bool isMatch(Pixel p)
    {
        if (c.isMatch(p.c))
            return true;
         return false;
    }

    public int[] getColor()
    {
        int[] a = {c.r, c.g, c.b};

        return a; 
    }

    
}