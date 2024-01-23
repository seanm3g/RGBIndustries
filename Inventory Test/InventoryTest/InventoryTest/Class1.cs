using System;
using System.ComponentModel;



/*
 * THINGS TO DO:  when adding random pixel, check if it's already in the system yet and just add quantity not a new entry.
 * 
 */


namespace INVENTORYTEST
{
    public class LOGICCENTER
    {
        private Random random = new Random();


        static void Main()
        {
            LOGICCENTER logicCenter = new LOGICCENTER();
            Inventory inventory = new Inventory();

            for (int i = 0; i < 100; i++)   //generates 10 random entries for the inventory
            {
                int quantity = logicCenter.random.Next(100)+1;
                inventory.AddPixels(randomPixel(), quantity);
            }

            inventory.PrintInventory();

            inventory.SortByQuantity();
            inventory.PrintInventory();
            
            inventory.SortByLevel();
            inventory.PrintInventory();

            inventory.SortByLevelAndQuantity();
            inventory.PrintInventory();

            inventory.SortByRarity();
            inventory.PrintInventory();

            Pixel p2 = new Pixel(1,1,0,0);
            inventory.AddPixels(p2,10);

            int q = inventory.GetQuantity(p2);

            Console.Out.WriteLine(q);

        }

        public static Pixel randomPixel()
        {
            LOGICCENTER logicCenter = new LOGICCENTER();

            int l = logicCenter.random.Next(8)+1;
            int r = logicCenter.random.Next((int)Math.Pow(2, l));
            int g = logicCenter.random.Next((int)Math.Pow(2, l));
            int b = logicCenter.random.Next((int)Math.Pow(2, l));

            return new Pixel(l, r, g, b);
        }


    }

    public class Inventory
    {

        private Dictionary<Pixel, int> inventory = new Dictionary<Pixel, int>();

        public int GetQuantity(Pixel pixel)
        {
            if (inventory.ContainsKey(pixel))
                return inventory[pixel];
            else return -1;
        }

        public void AddPixels(Pixel pixel, int quantity)
        {
            if (inventory.ContainsKey(pixel))
                inventory[pixel] += quantity;
            else inventory[pixel] = quantity;
        }

        public void SortByQuantity()
        {
            // Sort the inventory items by quantity in descending order
            var sortedInventory = inventory.OrderByDescending(item => item.Value)
                                          .ToDictionary(item => item.Key, item => item.Value);

            Console.Out.WriteLine("\n\nRESORTING THE INVENTORY BY QUANTITY\n\n");
            inventory = sortedInventory;
        }

        public void SortByLevel()
        {
            // Sort the inventory items by the level of the Pixel keys in ascending order
            var sortedInventory = inventory.OrderByDescending(item => item.Key.Level)
                                          .ToDictionary(item => item.Key, item => item.Value);

            Console.Out.WriteLine("\n\nRESORTING THE INVENTORY BY LEVEL\n\n");
            inventory = sortedInventory;
        }

        public void SortByLevelAndQuantity()
        {
            // Sort the inventory items first by the level of the Pixel keys in descending order,
            // and then within each level, sort by quantity in descending order
            var sortedInventory = inventory.OrderByDescending(item => item.Key.Level)
                                          .ThenByDescending(item => item.Value)
                                          .ToDictionary(item => item.Key, item => item.Value);

            Console.Out.WriteLine("\n\nRESORTING THE INVENTORY BY LEVEL AND QUANTITY\n\n");
            inventory = sortedInventory;
        }

        public void SortByRarity()
        {
            // Sort the inventory items first by the level of the Pixel keys in descending order,
            // and then within each level, sort by quantity in ascending order
            var sortedInventory = inventory.OrderByDescending(item => item.Key.Level)
                                          .ThenBy(item => item.Value)
                                          .ToDictionary(item => item.Key, item => item.Value);

            Console.Out.WriteLine("\n\nRESORTING THE INVENTORY BY HIGHEST LEVEL AND LEAST QUANTITY\n\n");
            inventory = sortedInventory;
        }



        public void PrintInventory()
        {
            foreach (KeyValuePair<Pixel, int> entry in inventory)
            {
                string hexValue = new string(entry.Key.hex); // Get the hex value of the pixel as a string
                Console.Out.WriteLine("Level: "+entry.Key.Level+" | Hex Value: " + hexValue + " | Quantity: " + entry.Value);
            }
        }



    }

    public struct Pixel
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int Level { get; set; }

        public char[] hex { get; set; }
        public Pixel(int level,int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Level = level;
            hex = Hex();
        }

        public Pixel Mix(Pixel other)
        {
            // Calculate the mixed color components, truncating any overflow
            int mixedRed = (Red + other.Red) > ((1 << Level) - 1) ? ((1 << Level) - 1) : (Red + other.Red);
            int mixedGreen = (Green + other.Green) > ((1 << Level) - 1) ? ((1 << Level) - 1) : (Green + other.Green);
            int mixedBlue = (Blue + other.Blue) > ((1 << Level) - 1) ? ((1 << Level) - 1) : (Blue + other.Blue);

            // Return the mixed pixel with the same level
            return new Pixel(Level,mixedRed, mixedGreen, mixedBlue);
        }

        public Pixel Merge(Pixel other)
        {
            if (this.Equals(other))
            {
                // If the pixels are identical, return a pixel with one higher level
                return new Pixel(Red, Green, Blue, Level + 1);
            }
            else
            {
                throw new ArgumentException("Pixels must be identical to merge.");
            }
        }
        public char[] Hex()
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
            for(int i=0;i<results.Length;i++)
            {
                results[i] = hexString[i];
            }

            return results;
        }

        public string PrintValues()
        {
            return $"Level: {Level}, Red: {Red}, Green: {Green}, Blue: {Blue}";
        }

        public bool isEqual(Pixel other)
        {
            if (this.Equals(other))
                return true;
            else return false;


        }
    }

}