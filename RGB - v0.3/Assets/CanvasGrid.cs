using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasGrid : MonoBehaviour, IPointerDownHandler
{
    public RawImage canvasImage;
    private Texture2D canvasTexture;
    private Color[] pixels;
    private int[,] pixelValues;

    FlavorText ft = new();

    public LogicCenter lc = new LogicCenter();
    ColorLibrary ColorLibrary = new ColorLibrary();

    private int canvasWidth = 500;  // 50 pixels * 10 width each
    private int canvasHeight = 500; // 50 pixels * 10 height each
    [SerializeField] private int pixelSize;

    void Start()
    {
        
        // Initialize Texture2D
        canvasTexture = new Texture2D(canvasWidth, canvasHeight);

        // Initialize pixel array
        pixels = new Color[canvasWidth * canvasHeight];

        pixelValues = new int[canvasWidth / pixelSize, canvasHeight / pixelSize];

        // Set initial color to white
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }

        // Apply changes to texture and assign to RawImage
        UpdateTexture();
        canvasImage.texture = canvasTexture;
    }

    public void OnPointerDown(PointerEventData eventData)
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
            Debug.Log("pixel Value:"+ pixelValues[clickedRow,clickedCol]);

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

    void UpdateTexture()
    {
        canvasTexture.SetPixels(pixels);
        canvasTexture.Apply();
    }


    public void convertToWorkOrder()
    {
        Debug.Log("Do we enter this function?");
        int[] vals = new int[8];
        
        for(int i = 0;i<vals.Length;i++)
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
            Debug.Log("Do we get in this switch"+x);
            Debug.Log("Quantity of: "+x+"="+vals[x]);
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
}
