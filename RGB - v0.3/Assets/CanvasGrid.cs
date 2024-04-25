using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class CanvasGrid : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    public LogicCenter lc;
    //ColorLibrary ColorLibrary = new ColorLibrary();

    public RawImage canvasImage;
    private Texture2D canvasTexture;
    private Color[] pixels;  //This is the actual canvas texture

    public int[,] pixelValues;  //This is the grid of pixel colors with the value being which color is being used. // deprecated
    public Inventory CanvasPixels = new Inventory();  //this should be used to store the quantity and the pixel //deprecated


    public Pixel[,] pixelValues2;

    private Contract contract;

    //private Inventory canvas;


    public float requiredPixels;  //This is the amount of total addition pixels required to make this picture.

    FlavorText ft = new();

    private int canvasWidth = 500;  // 50 pixels * 10 width each
    private int canvasHeight = 500; // 50 pixels * 10 height each

    [SerializeField] private int pixelSize;

    void Start()
    {
        // Create a new contract instance
        contract = new();

        // Get the RectTransform component of the game object this script is attached to
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        // Set canvasWidth and canvasHeight based on the RectTransform's dimensions
        canvasWidth = (int)rectTransform.rect.width;
        canvasHeight = (int)rectTransform.rect.height;

        // Initialize a Texture2D with the specified dimensions
        canvasTexture = new Texture2D(canvasWidth, canvasHeight);

        // Initialize an array of Color to represent the pixels in the canvas
        pixels = new Color[canvasWidth * canvasHeight];

        // Initialize a 2D integer array to represent pixelValues
        // The dimensions are determined by dividing canvasWidth and canvasHeight by pixelSize
        //pixelValues = new int[canvasWidth / pixelSize, canvasHeight / pixelSize];

        // Set the initial color of the canvas to black
        clearCanvas();

        // Apply the changes made to the texture and assign it to the RawImage component
        UpdateTexture();
        canvasImage.texture = canvasTexture;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region IO
    public void OnPointerDown(PointerEventData eventData)
    {
        paint(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        paint(eventData);
    }
    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region paint/canvas
    public void paint(PointerEventData eventData)
    {
        // Convert screen position to local position within the RawImage
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasImage.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        // Calculate which "pixel" is clicked
        int clickedRow = Mathf.FloorToInt((localPoint.y + canvasHeight / 2) / pixelSize);
        int clickedCol = Mathf.FloorToInt((localPoint.x + canvasWidth / 2) / pixelSize);

        // Check if the click is within bounds
        if (clickedRow >= 0 && clickedRow < (canvasHeight / pixelSize) && clickedCol >= 0 && clickedCol < (canvasWidth / pixelSize))
        {

            pixelValues2[clickedRow, clickedCol] = lc.chosenPaintColor2;
            
            Pixel coloredPixel = lc.chosenPaintColor2;


                // Loop to color the 10x10 area representing eachj "pixel"
                for (int y = 0; y < pixelSize; y++)
                {
                    for (int x = 0; x < pixelSize; x++)
                    {
                        int pixelIndex = (clickedRow * pixelSize + y) * canvasWidth + (clickedCol * pixelSize + x);
                        pixels[pixelIndex] = coloredPixel.toColor();  // Set color to black (or any other color)  //THIS LINE IS NOW BROKEN
                    }
                }
                // Update the texture
                UpdateTexture();
        }

    }

    public void clearCanvas()
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.black;
        }

        int numRows = pixelValues2.GetLength(0);
        int numCols = pixelValues2.GetLength(1);

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                pixelValues2[row, col] = new Pixel(); // You can customize this based on your Pixel struct/class
            }
        }
    }

    public void UpdateTexture()  //for painting
    {
        canvasTexture.SetPixels(pixels);
        canvasTexture.Apply();

        //c.setRequirements(pixelValues);
        updateHueQuantities();  //what
        updateHueUI();
        updateJobStats();
        //available();
    }
    #endregion 
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////   
    #region saveImage
    public void SaveAsJPEG()
{
    byte[] bytes = canvasTexture.EncodeToJPG();
    string filePath = GetUniqueFilePathOnDesktop("Work of Art.jpg");
    File.WriteAllBytes(filePath, bytes);
    Debug.Log("Does this run?");
}

    private string GetUniqueFilePathOnDesktop(string fileName)
{
    string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory)+"/art";
    string filePath = Path.Combine(desktopPath, fileName);

        // Check if the file already exists
        if (File.Exists(filePath))
        {
            int counter = 1;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string fileExtension = Path.GetExtension(fileName);

            // Keep incrementing the counter until a unique filename is found
            while (File.Exists(filePath))
            {
                fileName = $"{fileNameWithoutExtension}_{counter}{fileExtension}";
                filePath = Path.Combine(desktopPath, fileName);
                counter++;
            }
        }

    return filePath;
}
    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region work order

    public void updateHueQuantities()  //updates the running total  DOES THIS WORK?
    {

        CanvasPixels.Clear();   //not sure if this needed
        for (int i = 0; i < pixelValues2.GetLength(0); i++)
            for (int j = 0; j < pixelValues2.GetLength(1); j++)
                CanvasPixels.addPixels(pixelValues2[i, j], 1);  //adds quantity per pixel in the image.


    }

    public void updateJobStats() //updates the UI
    {
        int total = 0;  //not sure what this is trying to do.
        int totalValue = 15;
        float valueDensity = 0f;


        if (lc.activeContracts.Count>0)
        {
            total = CanvasPixels.totalPixels(); //this doesn't seem right.  It's only the first contract.
            totalValue = lc.activeContracts[0].inventory.remaining.totalValue();  //amount of value in the first contract.
        }

        
        if (total > 0) { 
            valueDensity = (float)totalValue/total;   //how dense is the value of the pixels?
        }

        if (lc.pictureStats != null)  //the detail stats
        {
            lc.pictureStats.text = "TOTAL PIXELS: " + total.ToString() + "\nTOTAL VALUE: " + totalValue.ToString() + "\nVALUE DENSITY: " + valueDensity.ToString("0.00") + "x\n\n";
            lc.pictureStats.text += "AVAILABLE: " + available().ToString("P0")+"\n";  //available() has been gutted. 
            //lc.pictureStats.text += "Est. completion: "+estimate().ToString()+" seconds \n";
        }
    }    
    public void updateHueUI()  //updates UI
    {
        
        int length = lc.hueQuantitiesText.Length;

        for (int i = 1; i < length;i++)  //we're not using 0 for anything because it's ore and not a value in this system.
        {

            /*
            if (lc.hueQuantitiesText[i] != null)
                lc.hueQuantitiesText[i].text = quantity[i].ToString() + "x";
            */

            if (lc.hueQuantitiesText[i] != null)
                lc.hueQuantitiesText[i].text = CanvasPixels.totalPixels().ToString() + "x"; //should this be CanvasPixels or Pixels[,]

        }
            
    }

    public void convertToWorkOrder()   //some logic could be changed here to put trades in for R,G,B tokens that don't require production
    {
        Contract contract = new Contract(pixelValues2);
        lc.activeContracts.Add(contract);



        foreach (var kvp in contract.remainingPixels()) //should pull every entry in the contract and add it to the work order.
        {
            Pixel pixel = new Pixel(kvp.Key.Level, kvp.Key.Red, kvp.Key.Green, kvp.Key.Blue);
            int quantity = kvp.Value;

            lc.addToQueue(new workOrder(pixel, quantity)); ///how should workOrder work now?

        }
        /*

        
        for (int i = 4; i < quantity.Length; i++)
        {
            if (quantity[i] < 3)
            {
                //if(i == lc.chosenColor)
                //wait
                //else
                //tradeOrder(i,quantity[i]);
            }
            else if (quantity[i] > 3)  //this shouldn't be calling production queue.  It should call a method in LC
            {
                switch (i)
                {
                    case 4:
                        lc.ProductionQueue.Add(new workOrder(quantity[i], 1, 2, 4, 3));
                        break;
                    case 5:
                        lc.ProductionQueue.Add(new workOrder(quantity[i], 1, 3, 5, 3));
                        break;
                    case 6:
                        lc.ProductionQueue.Add(new workOrder(quantity[i], 2, 3, 6, 3));
                        break;
                    case 7:
                        int randomCase = ft.rollDice(1, 3);
                        int param1 = randomCase == 1 ? 3 : randomCase == 2 ? 2 : 1;
                        int param2 = randomCase + 3;
                        lc.ProductionQueue.Add(new workOrder(quantity[i], param1, param2, 7, 3));
                        break;
                }
            }
        }

        */




    }

    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region stats
    public float available()  //what % of the image do you have the resources to make?
    {
        /*
        requiredPixels = 0;
        
        float total = contract.currentCost;

        for (int i = 0; i < quantity.Length; i++)  //if you have enough
        {
            int job = quantity[i];
            int available = lc.inventory.GetQuantity(); ///how does this work?

            if (job > available)
                requiredPixels += job - available;
        }

        float difference = requiredPixels / total;
        
        if (difference == float.NaN) //this doesn't seem to work.
            return 0f;
        else return (1f - difference);
        */
        return 0f;

    }
    public int estimate()  //used for other things not implemented yet // how long it would take to make this.  It's broken currently, I think?
    {

        
        int t = 0;  //time?
        /*
        for(int i = 0; i < quantity.Length; i++)
        {
            if (i < 4)
                t += quantity[i];
            else if (i < 7)
                t += quantity[i] / (int)lc.machineAverage();
            else t += quantity[i] / (int)lc.machineAverage() * 2;
        }
        */
        return t;

    }

    #endregion
}
