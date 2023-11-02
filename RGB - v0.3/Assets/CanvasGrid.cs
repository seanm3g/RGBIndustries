using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class CanvasGrid : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RawImage canvasImage;
    private Texture2D canvasTexture;
    private Color[] pixels;  //This is the actual canvas texture

    private int[,] pixelValues;  //This is the grid of pixel colors.
    private int[] quantity = new int[8];
    private Contract c;
    
    public float requiredPixels;  //This is the amount of additional pixels required to make this picture.

    FlavorText ft = new();

    public LogicCenter lc = new LogicCenter();
    ColorLibrary ColorLibrary = new ColorLibrary();

    private int canvasWidth = 500;  // 50 pixels * 10 width each
    private int canvasHeight = 500; // 50 pixels * 10 height each
    [SerializeField] private int pixelSize;

    void Start()
    {
        //init the quantity of each pixel
        quantity = new int[8];

        c = new();

        for (int i = 0; i < quantity.Length; i++)  //init to zero
        {
            quantity[i] = 0;
        }
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        canvasWidth = (int)rectTransform.rect.width;
        canvasHeight = (int)rectTransform.rect.height;

        // Initialize Texture2D
        canvasTexture = new Texture2D(canvasWidth, canvasHeight);

        // Initialize pixel array
        pixels = new Color[canvasWidth * canvasHeight];

        pixelValues = new int[canvasWidth / pixelSize, canvasHeight / pixelSize];



        // Set initial color to black
        clearCanvas();

        // Apply changes to texture and assign to RawImage
        UpdateTexture();
        canvasImage.texture = canvasTexture;
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnPointerDown(PointerEventData eventData)
    {
        paint(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        paint(eventData);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

            pixelValues[clickedRow, clickedCol] = lc.chosenPaintColor;
            //Debug.Log("pixel Value:" + pixelValues[clickedRow, clickedCol]);

            // Loop to color the 10x10 area representing the "pixel"
            for (int y = 0; y < pixelSize; y++)
            {
                for (int x = 0; x < pixelSize; x++)
                {
                    int pixelIndex = (clickedRow * pixelSize + y) * canvasWidth + (clickedCol * pixelSize + x);
                    pixels[pixelIndex] = ColorLibrary.colors[lc.chosenPaintColor].toColor();  // Set color to black (or any other color)
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

        for (int i = 0; i < quantity.Length; i++)
        {
            quantity[i] = 0;   //this gets overwritten when updateTexture() is called.
        }

        for (int y = 0; y < pixelValues.GetLength(0); y++)
            for (int x = 0; x < pixelValues.GetLength(1); x++)
                pixelValues[y, x] = 0;




        UpdateTexture();
    }
    public void saveAsJPEG()
    {

        byte[] bytes = canvasTexture.EncodeToJPG();
        File.WriteAllBytes(Application.dataPath + "/Work of Art.jpg", bytes);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void UpdateTexture()
    {
        canvasTexture.SetPixels(pixels);
        canvasTexture.Apply();

        c.setRequirements(pixelValues);
        
        //convertToWorkOrder();
        //updateHueQuantities();
        updateHueUI();
        updateJobStats();
        //available();
    }

    public void convertToWorkOrder()
    {


        for (int i = 0; i < c.requirements.Length; i++)
        {
            if (i < 3)
            {
                //if(i == lc.chosenColor)
                //wait
                //else
                //tradeOrder(i,requirements[i]);
            }
            else if (i > 3)
            {
                if(c.requirements[i]>0)
                {   
                    
                Debug.Log("requirements[i" + i + "]" + c.requirements[i]);

                switch (i)
                    {
                        case 4:
                            lc.ProcessingQueue.Add(new workOrder(c.requirements[i], 1, 2, 4));
                            break;
                        case 5:
                            lc.ProcessingQueue.Add(new workOrder(c.requirements[i], 1, 3, 5));
                            break;
                        case 6:
                            lc.ProcessingQueue.Add(new workOrder(c.requirements[i], 2, 3, 6));
                            break;
                        case 7:
                            int randomCase = ft.rollDice(1, 3);
                            int param1 = randomCase == 1 ? 3 : randomCase == 2 ? 2 : 1;
                            int param2 = randomCase + 3;
                            lc.ProcessingQueue.Add(new workOrder(c.requirements[i], param1, param2, 7));
                        break;
                    }
                }
            }
        }
    }
    public void updateJobStats() //updates the UI
    {
        int total = c.totalRequirements;
        int totalValue = c.totalValue;
        float valueDensity = 0f;

        
        if (total > 0) { 
            valueDensity = (float)totalValue/total;
        }

        if (lc.pictureStats != null)  //the detail stats
        {
            lc.pictureStats.text = "TOTAL PIXELS: " + total.ToString() + "\nTOTAL VALUE: " + totalValue.ToString() + "\nVALUE DENSITY: " + valueDensity.ToString("0.00") + "x\n\n";
            lc.pictureStats.text += "AVAILABLE: " + currentlyAvailable().ToString("P0")+"\n";
            //lc.pictureStats.text += "Est. completion: "+estimate().ToString()+" seconds \n";
        }
    }    
    public void updateHueUI()  //updates UI
    {
        int length = lc.hueQuantitiesText.Length;
        for (int i = 1; i < length;i++)  //we're not using 0 for anything because it's ore and not a value in this system.
            if (lc.hueQuantitiesText[i] != null)
                lc.hueQuantitiesText[i].text = c.requirements[i].ToString()+"x";
    }

    public float currentlyAvailable()
    {
        int requiredPixels = 0;

        for (int i = 0; i < c.requirements.Length; i++)  //if you have enough
        {
            int job = c.requirements[i];
            //int available = lc.inventory[i];
            
            string color = lc.inv.intToColorKey(i);
            string hex = lc.inv.ColorToHex(color);

            int available = lc.inv.valueAtSlot(hex);

            if (job > available)
                requiredPixels += job - available;
        }

        float difference = requiredPixels / (float)c.totalRequirements;

        if (difference == float.NaN) //this doesn't seem to work.
            return 0f;
        else return (1f - difference);

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public int estimate()  //used for other things not implemented yet // how long it would take to make this.  It's broken currently, I think?
    {
        int t = 0;

        for(int i = 0; i < quantity.Length; i++)
        {
            if (i < 4)
                t += quantity[i];
            else if (i < 7)
                t += quantity[i] / (int)lc.machineAverage();
            else t += quantity[i] / (int)lc.machineAverage() * 2;
        }

        return t;
    }
}
