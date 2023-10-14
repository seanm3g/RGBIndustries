using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class machineProcessing : MonoBehaviour
{
    // Start is called before the first frame update

    public Image pixelImage;
    public float cooldown=5;
    bool isCooldown = false;
    public KeyCode key;

    public bool triggered = false;

    void Start()
    {
        pixelImage.fillAmount = 1;   
    }

    // Update is called once per frame
    void Update()
    {
        runMachine();
    }


    public void isPressed()
    {
        triggered = true;
    }
    public void runMachine()
    {

        if (triggered && !isCooldown)
        {
            pixelImage.fillAmount = 1;
            isCooldown = true;
        }

        if(isCooldown)
        {
            pixelImage.fillAmount -= 1 / cooldown * Time.deltaTime;

            if(pixelImage.fillAmount <= 0)
            {
                pixelImage.fillAmount = 1;
                isCooldown = false;
                triggered = false;
            }
        }

    }
}
