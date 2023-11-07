using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class CanvasGrid : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    public LogicCenter lc = new LogicCenter();
    ColorLibrary ColorLibrary = new ColorLibrary();

    public RawImage canvasImage;
    private Texture2D canvasTexture;
    private Color[] pixels;  //This is the actual canvas texture

    private int[,] pixelValues;  //This is the grid of pixel colors.
    private int[] quantity = new int[8];
    private Contract c;
    
    public float requiredPixels;  //This is the amount of additional pixels required to make this picture.

    FlavorText ft = new();



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

        //c.setRequirements(pixelValues);
        updateHueQuantities();
        updateHueUI();
        updateJobStats();
        //available();
    }

    public void updateHueQuantities()
    {
        for (int i = 0; i < quantity.Length; i++)
            quantity[i] = 0;
        if(pixelValues.GetLength(0) > 0)
            for (int i = 0; i < pixelValues.GetLength(0); i++)
                for (int j = 0; j < pixelValues.GetLength(1); j++)
                    quantity[pixelValues[i, j]]++;  //adds quantity per pixel in the image.


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
            lc.pictureStats.text += "AVAILABLE: " + available().ToString("P0")+"\n";
            //lc.pictureStats.text += "Est. completion: "+estimate().ToString()+" seconds \n";
        }
    }    
    public void updateHueUI()  //updates UI
    {
        
        int length = lc.hueQuantitiesText.Length;
        for (int i = 1; i < length;i++)  //we're not using 0 for anything because it's ore and not a value in this system.
        {

           // Debug.Log("i:"+i+"Quantity:"+quantity[i]);

            if (lc.hueQuantitiesText[i] != null)
                lc.hueQuantitiesText[i].text = quantity[i].ToString() + "x";
        }
            
    }

    public void convertToWorkOrder()
    {
        Contract c = new Contract();
        c.setRequirements(pixelValues);
        lc.activeContracts.Add(c);
        c.convertToWorkOrder();

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
        }*/
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public float available()  //what % of the image do you have the resources to make?
    {
        requiredPixels = 0;
        float total = c.totalValue;

        for (int i = 0; i < quantity.Length; i++)  //if you have enough
        {
            int job = quantity[i];
            int available = lc.inventory[i];

            if (job > available)
                requiredPixels += job - available;
        }

        float difference = requiredPixels / total;

        if (difference == float.NaN) //this doesn't seem to work.
            return 0f;
        else return (1f - difference);
      
    }
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
