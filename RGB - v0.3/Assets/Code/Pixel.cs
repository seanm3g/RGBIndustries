using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
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

    public int[] Leveled(int level, int x)
    {
        int[] vals = new int[3];
        for (int i = 0; i < vals.Length; i++)
        {
            int maxValue = 255; // 8-bit maximum value

            // Calculate the scaled value based on bit depth
            vals[i] = (int)((level / 8.0) * maxValue * x);
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