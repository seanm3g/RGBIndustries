using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaletteColor : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    PaletteSelect ps;
    public Image bg;

    int index;
    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<Image>();
        ps.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ps.onClick(this);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        ps.onEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ps.onExit(this);
    }


}
