using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    // Start is called before the first frame update
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;

    LogicCenter lc = new LogicCenter();

    int index;

    public void Start()
    {

        for(int i = 0;i<objectsToSwap.Count;i++)
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
        Debug.Log("Chosen Color:" + lc.chosenColor);

        button.bg.color = Color.green;

        switch (lc.chosenColor)
        {
            case 1: button.bg.color = Color.red; break;
            case 2: button.bg.color = Color.green; break;
            case 3: button.bg.color = Color.blue; break;
        }
        

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
    public void resetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if (selectedTab != null && selectedTab == button) continue;
            button.bg.color = Color.white;
        }
    }
}
