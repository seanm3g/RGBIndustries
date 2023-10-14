using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteSelect : MonoBehaviour
{
    // Start is called before the first frame update

    List<PaletteColor> paletteColors;
    PaletteColor selected;
    void Start()
    {
        
    }

    public void Subscribe(PaletteColor s)
    {
        if(paletteColors == null)
        { 
            paletteColors = new List<PaletteColor>();
        }
        paletteColors.Add(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onEnter(PaletteColor s)
    {
        selected = s;
        
    }
    public void onClick(PaletteColor s)
    {
        selected = s;
    }
    public void onExit(PaletteColor s)
    {
        selected = s;
    }
}
