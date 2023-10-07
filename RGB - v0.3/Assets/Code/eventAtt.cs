using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EventAtt : MonoBehaviour
{
    // Start is called before the first frame update
    public Color bgColor = Color.blue;
    public Text eventText;

    void Update()
    {

       // eventText.text = "Test";
    }

    void setText(string t)
    {
        eventText.text = t;
    }

    
}
