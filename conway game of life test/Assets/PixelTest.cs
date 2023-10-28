using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PixelTest : MonoBehaviour, IPointerDownHandler
{ 
    public int rows = 10;
    public int cols = 10;
    public Texture2D gridTexture;
    public RawImage gridRawImage;

    private Color[] pixels;
    private int cellWidth;
    private int cellHeight;

    void Start()
    {
        // Initialize the texture and RawImage
        gridRawImage = GetComponent<RawImage>();

        gridTexture = new Texture2D(500, 500);
        gridRawImage.texture = gridTexture;

        // Initialize pixel array
        pixels = new Color[gridTexture.width * gridTexture.height];

        // Calculate cell dimensions
        cellWidth = gridTexture.width / cols;
        cellHeight = gridTexture.height / rows;

        // Initialize grid to white
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }

        UpdateTexture();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Convert screen position to local position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridRawImage.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        // Calculate clicked cell
        int clickedRow = Mathf.FloorToInt((localPoint.y + gridTexture.height / 2) / cellHeight);
        int clickedCol = Mathf.FloorToInt((localPoint.x + gridTexture.width / 2) / cellWidth);

        Debug.Log("Clicked Row: " + clickedRow);
        Debug.Log("Clicked Col: " + clickedCol);

        if (clickedRow >= 0 && clickedRow < rows && clickedCol >= 0 && clickedCol < cols)
        {
            int index = clickedRow * cols + clickedCol;
            Debug.Log("Changing pixel at index: " + index);

            // Toggle the cell color between white and black
            pixels[index] = pixels[index] == Color.white ? Color.black : Color.white;

            // Update the texture
            UpdateTexture();
        }
    }

    void UpdateTexture()
    {
        // Loop through each cell in the grid
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Calculate the index in the 1D pixel array
                int index = row * cols + col;

                // Determine the color for this cell
                Color cellColor = pixels[index];

                // Loop through each pixel in this cell
                for (int y = 0; y < cellHeight; y++)
                {
                    for (int x = 0; x < cellWidth; x++)
                    {
                        // Calculate the index in the 1D pixel array
                        int pixelIndex = (row * cellHeight + y) * gridTexture.width + (col * cellWidth + x);

                        // Set this pixel's color
                        pixels[pixelIndex] = cellColor;
                    }
                }
            }
        }

        // Update the texture
        gridTexture.SetPixels(pixels);
        gridTexture.Apply();
    }

}
