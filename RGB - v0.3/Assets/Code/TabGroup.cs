using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    // Start is called before the first frame update
    public TabButton selectedTab;
    Text childText;

    public List<GameObject> objectsToSwap;

    public LogicCenter lc;

    int index;

    public void Start()
    {
        lc = FindObjectOfType<LogicCenter>();

        

        for (int i = 0;i<objectsToSwap.Count;i++)
            objectsToSwap[i].SetActive(false);

        index = 2;
    }

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);

    }

    public void onTabEnter(TabButton button)
    {
        resetTabs();

        if(selectedTab == null || button != selectedTab)
            button.bg.color = Color.gray;

    }
    public void onTabExit(TabButton button)
    {
        resetTabs();

    }
    public void onTabSelected(TabButton button)
    {
        selectedTab = button;
        resetTabs();


        button.bg.color = Color.blue;

        updateColor();
        

        index = button.transform.GetSiblingIndex();
        for(int i = 0; i<objectsToSwap.Count; i++)
        {
            if(i==index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }

    }

    public void updateColor()
    {

        if(selectedTab != null)
        {

        
            childText = selectedTab.GetComponentInChildren<Text>();

            switch (lc.chosenColor)
            {
                case 1:
                    selectedTab.bg.color = new Color(1, 0, 0);
                        childText.color = new Color(1, 1, 1);
                    break;
                case 2:
                    selectedTab.bg.color = new Color(0, 1, 0);
                        childText.color = new Color(0, 0, 0);
                    break;
                case 3:
                    selectedTab.bg.color = new Color(0, 0, 1);
                
                        childText.color = new Color(1, 1, 1);
                    break;
            }
        }
    }
    public void resetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            childText = button.GetComponentInChildren<Text>();

            if (selectedTab != null && selectedTab == button) continue;
            button.bg.color = Color.white;
            if(childText != null)
            {
                childText.color = Color.black;
            }
         
        }
    }
}
