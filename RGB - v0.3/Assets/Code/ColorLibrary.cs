using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLibrary
{
    // Start is called before the first frame update
    public ColorRGB[] colors;

    public ColorLibrary()
    {
        colors = new ColorRGB[8];

        colors[0] = new ColorRGB(0, 0, 0);  //ORE / black
        colors[1] = new ColorRGB(1, 0, 0);  //red
        colors[2] = new ColorRGB(0, 1, 0);  //green
        colors[3] = new ColorRGB(0, 0, 1);  //blue
        colors[4] = new ColorRGB(1, 1, 0);  //yellow
        colors[5] = new ColorRGB(0, 1, 1);  //cyan
        colors[6] = new ColorRGB(1, 0, 1);  //magenta
        colors[7] = new ColorRGB(1, 1, 1);  //white
    }
 
    public int getIndex(ColorRGB c)
    {
        int index = 0;
        for(int i=0; i < colors.Length; i++)
        {
            if (colors[i].isMatch(c))
            {
                index = i;
                //Debug.Log($"This is the index: {index}");
            }
        }

        return index;  //The +1 is to offset for the inventory storing ore at 0
    }
}
