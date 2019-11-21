using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static void ToggleCanvasGroup(float value, CanvasGroup group)
    {
        group.alpha = value;

        Canvas cnv = group.transform.GetComponent<Canvas>();
        if (cnv != null)
            cnv.sortingOrder = (int)value;
    }

    public static void ToggleCanvasGroup(float value, CanvasGroup group, GameObject other)
    {
        if(value > 0)
        {
            other.SetActive(true);
        }
        else
        {
            other.SetActive(false);
        }

        group.alpha = value;

        Canvas cnv = group.transform.GetComponent<Canvas>();
        if (cnv != null)
            cnv.sortingOrder = (int)value;
    }
}
