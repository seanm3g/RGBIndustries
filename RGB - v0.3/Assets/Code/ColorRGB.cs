using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ColorRGB
{
    public int r;
    public int g;
    public int b;

    public ColorRGB(int r, int g, int b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public bool isMatch(ColorRGB c2)
    {
        if(this.r == c2.r && this.g == c2.g && this.b == c2.b)
            return true;
        else return false;
    }

    public ColorRGB colorProduct(ColorRGB c2)  //currently only functions with L1 color, but it might go higher.
    {
        ColorRGB c3 = new ColorRGB(r+c2.r,g+c2.g,b+c2.b);
        if(c3.r>1)
             c3.r = 1;
        if(c3.g>1)
            c3.g = 1;
        if(c3.b>1)
            c3.b = 1;
        
        return c3;
    }

    public Color toColor()
    {
        return new Color(r,g,b);

    }
}