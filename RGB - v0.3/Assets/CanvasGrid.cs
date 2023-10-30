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

    public void OnPointerDown(PointerEventData eventData)
    {
        paint(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        paint(eventData);
    }

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
    public void UpdateTexture()
    {
        canvasTexture.SetPixels(pixels);
        canvasTexture.Apply();

        updateHueQuantities();
        updateHueUI();
        updateJobStats();
        //available();
    }

    public void updateJobStats() //updates the UI
    {
        int total = currentTotal();
        int totalValue = currentTotalValue();
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

    public int currentTotal()
    {
        int total = 0;

        for (int i = 0; i < quantity.Length; i++)
            total += quantity[i];

        return total;
    }

    public int currentTotalValue()
    {
        int total = 0;
        int valueMultiplier = 1;
        for (int i = 0; i < quantity.Length; i++)
        { 
            if(i<4)
                valueMultiplier = 1;
            else if(i<7)
                valueMultiplier = 2;
            else valueMultiplier = 3;
            
            total += quantity[i]*valueMultiplier;
        }
        return total;
    }

    public void updateHueQuantities()
    {

        for (int i = 0; i < quantity.Length; i++)  //init to zero
        {
            quantity[i] = 0;
        }

        for (int x = 0; x < pixelValues.GetLength(0); x++)
            for (int y = 0; y < pixelValues.GetLength(1); y++)
                switch (pixelValues[x, y])
                {
                    case 1:
                        quantity[1]++;
                        break;
                    case 2:
                        quantity[2]++;
                        break;
                    case 3:
                        quantity[3]++;
                        break;
                    case 4:
                        quantity[4]++;
                        break;
                    case 5:
                        quantity[5]++;
                        break;
                    case 6:
                        quantity[6]++;
                        break;
                    case 7:
                        quantity[7]++;
                        break;
                }
    }

    public void updateHueUI()
    {
        int length = lc.hueQuantitiesText.Length;
        for (int i = 1; i < length;i++)  //we're not using 0 for anything because it's ore and not a value in this system.
            if (lc.hueQuantitiesText[i] != null)
                lc.hueQuantitiesText[i].text = quantity[i].ToString()+"x";


    }
    public void convertToWorkOrder()
    {
        int[] vals = new int[8];
        
        for(int i = 0;i<vals.Length;i++)  //init to zero
        {
            vals[i] = 0;
        }

        for(int x=0; x < pixelValues.GetLength(0);x++)
            for(int y = 0; y < pixelValues.GetLength(1);y++)
                switch (pixelValues[x,y])
                {
                    case 1:
                        vals[1]++;
                        break;
                    case 2:
                        vals[2]++;
                        break;
                    case 3:
                        vals[3]++;
                        break;
                    case 4:
                        vals[4]++;
                        break;
                    case 5:
                        vals[5]++;
                        break;
                    case 6:
                        vals[6]++;
                        break;
                    case 7:
                        vals[7]++;
                        break;
                }
        
        for (int x = 0; x < vals.Length; x++)
        {
            
            if (vals[x] > 3)
                switch (x)
                {
                    
                    case 4:
                        lc.ProcessingQueue.Add(new workOrder(vals[x],1,2,4));
                        break;
                    case 5:
                        lc.ProcessingQueue.Add(new workOrder(vals[x],1,3,5));
                        break;
                    case 6:
                        lc.ProcessingQueue.Add(new workOrder(vals[x],2,3,6));
                        break;
                    case 7:
                        switch (ft.rollDice(1, 3))
                        {
                            case 1:
                                lc.ProcessingQueue.Add(new workOrder(vals[x], 3, 4, 7)); //white
                                break;
                            case 2:
                                lc.ProcessingQueue.Add(new workOrder(vals[x], 2, 5, 7)); //white
                                break;
                            case 3:
                                lc.ProcessingQueue.Add(new workOrder(vals[x], 1, 6, 7)); //white
                                break;
                        }
                        break;
                }
            
        }

    }

    public void saveAsJPEG()
    {

        byte[] bytes = canvasTexture.EncodeToJPG();
        File.WriteAllBytes(Application.dataPath + "/Work of Art.jpg", bytes);
    }

    public void clearCanvas()
    {

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.black;
            
        }

        for(int i = 0;i< quantity.Length;i++)  
        {
            quantity[i] = 0;   //this gets overwritten when updateTexture() is called.
        }

        for (int y = 0; y < pixelValues.GetLength(0);y++)
            for(int x= 0;x<pixelValues.GetLength(1);x++)
            pixelValues[y,x] = 0;




        UpdateTexture();  
    }

    public float available()  //what % of the image do you have the resources to make?
    {
        requiredPixels = 0;
        float total = (float)currentTotal();

        for (int i = 0; i < quantity.Length; i++)  //if you have enough
        {
            int job = quantity[i];
            int available = lc.inventory[i];

            if (job > available)
                requiredPixels += job - available;
        }
        


        float difference = requiredPixels / total;

        if (difference == float.NaN)
            return 0f;
        else return (1f - difference);
      
    }
    
    public int estimate()
    {
        int t = 0;

        for(int i = 0; i < quantity.Length; i++)
        {
            if (i < 4)
                t += quantity[i];
            else if (i < 7)
                t += quantity[i] * (int)lc.machineAverage();
            else t += quantity[i] * (int)lc.machineAverage() * 2;
        }

        return t;
    }
    public void newJobLogic()
    {

        int required = 0;
        int total = currentTotal();

        for (int i = 0; i < quantity.Length; i++)  //if you have enough
        {
            int req = quantity[i];
            int available = lc.inventory[i];

            if (req <= available)  //just subtract the amount
                lc.inventory[i] -= req;
            else if(req > available)
                { 
                    lc.inventory[i] = available;
                 lc.inventory[i] = available;
            } 
        }


    }
}
