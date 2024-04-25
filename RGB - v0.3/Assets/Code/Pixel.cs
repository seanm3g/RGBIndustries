using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public struct Pixel
{


    #region variables

    FlavorText ft;
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
    public int Level { get; set; }
    public char[] hex { get; set; }

    public (int l, int r, int g, int b) p;  //should I switch to this here?  Maybe not
    #endregion

    #region CONSTANTS
    public static Pixel ORE()
    {

        return new Pixel(1, 0, 0, 0);

    }

    public static Pixel RED()
    {

        return new Pixel(1, 1, 0, 0);

    }

    public static Pixel GREEN()
    {

        return new Pixel(1, 0, 1, 0);

    }

    public static Pixel BLUE()
    {

        return new Pixel(1, 0, 0, 1);

    }

    #endregion

    #region constructors
    public Pixel(int level, int red, int green, int blue)
    {

        ft = new FlavorText();
        Red = red;
        Green = green;
        Blue = blue;
        Level = level;
        hex = new char[] { '5' }; //dummy assign

        p.l = 1;
        p.r = 1;
        p.g = 1;
        p.b = 1;

        hex = valueToHex();
    }

    #endregion

    #region static operators

    public static Pixel operator +(Pixel pixel1, Pixel pixel2)
    {
        if (pixel1.Level == pixel2.Level)
        {
            // Calculate the mixed color components, truncating any overflow
            int mixedRed = (pixel1.Red + pixel2.Red) > ((1 << pixel1.Level) - 1) ? ((1 << pixel1.Level) - 1) : (pixel1.Red + pixel2.Red);
            int mixedGreen = (pixel1.Green + pixel2.Green) > ((1 << pixel1.Level) - 1) ? ((1 << pixel1.Level) - 1) : (pixel1.Green + pixel2.Green);
            int mixedBlue = (pixel1.Blue + pixel2.Blue) > ((1 << pixel1.Level) - 1) ? ((1 << pixel1.Level) - 1) : (pixel1.Blue + pixel2.Blue);

            // Return the mixed pixel with the same level as pixel1
            return new Pixel(pixel1.Level, mixedRed, mixedGreen, mixedBlue);
        }
        else
        {
            // Throw an exception if levels are mismatched
            throw new InvalidOperationException("Pixels must be of equal level");
        }
    }

    public static Pixel operator -(Pixel pixel1, Pixel pixel2)
    {
        if (pixel1.Level == pixel2.Level)
        {
            // Calculate the subtracted color components
            int subtractedRed = pixel1.Red - pixel2.Red;
            int subtractedGreen = pixel1.Green - pixel2.Green;
            int subtractedBlue = pixel1.Blue - pixel2.Blue;

            // Check if any color component is negative
            if (subtractedRed < 0 || subtractedGreen < 0 || subtractedBlue < 0)
            {
                throw new InvalidOperationException("Subtraction result contains negative color component(s).");
            }

            // Return the result pixel with the same level as pixel1
            return new Pixel(pixel1.Level, subtractedRed, subtractedGreen, subtractedBlue);
        }
        else
        {
            // Throw an exception if levels are mismatched
            throw new InvalidOperationException("Pixels must be of equal level");
        }
    }

    public static bool operator ==(Pixel pixel1, Pixel pixel2)
    {
        return pixel1.Red == pixel2.Red && pixel1.Green == pixel2.Green && pixel1.Blue == pixel2.Blue && pixel1.Level == pixel2.Level;
    }

    public static bool operator !=(Pixel pixel1, Pixel pixel2)
    {
        return !(pixel1 == pixel2);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Red, Green, Blue, Level);
    }
    public bool Equals(Pixel other)
    {
        return Level == other.Level && Red == other.Red && Green == other.Green && Blue == other.Blue;
    }

    public static Pixel assignDefault()
    {
        return new Pixel(0, 0, 0, 0);
    }

    public static bool isPrimary(Pixel p)
    {
        // Check if the pixel is red, green, or blue, and the level is 1
        bool isRed = p.Red == 1 && p.Green == 0 && p.Blue == 0 && p.Level == 1;
        bool isGreen = p.Red == 0 && p.Green == 1 && p.Blue == 0 && p.Level == 1;
        bool isBlue = p.Red == 0 && p.Green == 0 && p.Blue == 1 && p.Level == 1;

        return isRed || isGreen || isBlue;
    }

    public static Pixel randomPixel(int l)
    {
        System.Random random = new System.Random();

        int r = random.Next((int)Math.Pow(2, l));
        int g = random.Next((int)Math.Pow(2, l));
        int b = random.Next((int)Math.Pow(2, l));


        return new Pixel(l, r, g, b);

    }


    #endregion

    #region old code
    public bool mixedEquals(Pixel otherPixel, Pixel resultPixel)
    {
        // Calculate the mixed color components for this Pixel
        int mixedRed = (Red + otherPixel.Red) > ((1 << Level) - 1) ? ((1 << Level) - 1) : (Red + otherPixel.Red);
        int mixedGreen = (Green + otherPixel.Green) > ((1 << Level) - 1) ? ((1 << Level) - 1) : (Green + otherPixel.Green);
        int mixedBlue = (Blue + otherPixel.Blue) > ((1 << Level) - 1) ? ((1 << Level) - 1) : (Blue + otherPixel.Blue);

        // Create a new Pixel with the mixed color components
        Pixel mixedPixel = new Pixel(Level, mixedRed, mixedGreen, mixedBlue);

        // Check if the resulting mixedPixel equals the given resultPixel
        return mixedPixel == resultPixel;
    }



    public Pixel Mix(Pixel other)
    {
        // Calculate the mixed color components, truncating any overflow
        int mixedRed = (Red + other.Red) > ((1 << Level) - 1) ? ((1 << Level) - 1) : (Red + other.Red);
        int mixedGreen = (Green + other.Green) > ((1 << Level) - 1) ? ((1 << Level) - 1) : (Green + other.Green);
        int mixedBlue = (Blue + other.Blue) > ((1 << Level) - 1) ? ((1 << Level) - 1) : (Blue + other.Blue);

        // Return the mixed pixel with the same level
        return new Pixel(Level, mixedRed, mixedGreen, mixedBlue);
    }

    public Pixel Merge(Pixel other)
    {
        if (this.Equals(other))
        {
            // If the pixels are identical, return a pixel with one higher level
            return new Pixel(Level++, Red, Green, Blue);
        }
        else
        {
            throw new ArgumentException("Pixels must be identical to merge.");
        }
    }

    #endregion

    #region hex functions
    public char[] valueToHex()
    {
        // Calculate the maximum value for the specified bit depth
        int maxValue = (1 << Level) - 1;

        // Ensure color components are within the specified bit depth
        int validRed = Red > maxValue ? maxValue : (Red < 0 ? 0 : Red);
        int validGreen = Green > maxValue ? maxValue : (Green < 0 ? 0 : Green);
        int validBlue = Blue > maxValue ? maxValue : (Blue < 0 ? 0 : Blue);

        // Calculate scaling factor to convert from specified bit depth to 8-bit
        double scale = 255.0 / maxValue;

        // Convert color components to 8-bit hexadecimal strings with padding
        string hexRed = ((int)(validRed * scale)).ToString("X2");
        string hexGreen = ((int)(validGreen * scale)).ToString("X2");
        string hexBlue = ((int)(validBlue * scale)).ToString("X2");

        // Construct the final hex color code with #
        String hexString = $"#{hexRed}{hexGreen}{hexBlue}";

        char[] results = new char[7];
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = hexString[i];
        }

        return results;
    }

    public Color toColor()
    {

        string hexString = hex.ToString();

        // Remove the "#" character if it's included in the hex string
        hexString = hexString.TrimStart('#');

        // Parse the hex values into byte values
        byte r = byte.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        // Check if the hex string includes an alpha channel (8 characters)
        if (hexString.Length == 8)
        {
            byte a = byte.Parse(hexString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, a);
        }
        else
        {
            return new Color32(r, g, b, 255); // Default to full opacity if alpha is not provided
        }
    }

    #endregion

    #region utility

    public string PrintValues()
    {
        return $"Level: {Level}, Red: {Red}, Green: {Green}, Blue: {Blue}";
    }

    public int pixelvalue()  //total resources in the pixel
    {
        return Red + Green + Blue;

    }

    public void Clear()  //Black
    {
        Level = 1;
        Red = 0;
        Green = 0;
        Blue = 0;

    }

    #endregion
}






















/*
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

*/