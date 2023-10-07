using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public struct TimelineEvent
{
    // Start is called before the first frame update
    public int eventType;
    public TimelineEvent(int type)
    {
        eventType = type;
    }

    public override string ToString()
    {
        return $"eventType: {eventType}";
    }
}
