using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pixel
{

    public int quantity;
    int level;
    int r;
    int g;
    int b;

    public Pixel(int q, int l, int r, int g, int b)
    {
        this.level = l;
        this.r = r;
        this.g = g;
        this.b = b;
        this.quantity = q;
    }

    public bool isMatch(Pixel p)
    {
        bool equals = false;
        if (this.level == p.level && this.r == p.r && this.g == p.g && this.b == p.b)
            equals = true;

        return equals;
    }

}