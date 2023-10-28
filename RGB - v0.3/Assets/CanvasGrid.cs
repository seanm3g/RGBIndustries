using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasGrid : MonoBehaviour, IPointerDownHandler
{
    public RawImage canvasImage;
    private Texture2D canvasTexture;
    private Color[] pixels;
    private int[,] pixelValues;

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

            pixelValues[clickedRow, clickedCol] = lc.chosenColor;
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
}
