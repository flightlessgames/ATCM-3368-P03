using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Utility;   //using a Utility Script to save typing


public class ClickableUnit : MonoBehaviour
{
    public event Action<Vector3> NewLocation = delegate { };

    [Header("Settings")]
    [SerializeField] CanvasGroup _unitCanvas = null;
    [SerializeField] GameObject _buttonPanel = null;

    protected void ChildInvokeNewLocation(Vector3 location) //special method to call from Child/Inherited Class, uses this/Parent event call
    {
        NewLocation?.Invoke(location);
    }

    public void RollCall()
    {
        if(_buttonPanel != null)
            ToggleCanvasGroup(1, _unitCanvas, _buttonPanel);
        else
            ToggleCanvasGroup(1, _unitCanvas);
    }

    public void Deselect()
    {
        if (_buttonPanel != null)
            ToggleCanvasGroup(0, _unitCanvas, _buttonPanel);
        else
            ToggleCanvasGroup(0, _unitCanvas);
    }

    public void Identify(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Ground"))
        {
            NewLocation?.Invoke(hit.point);
        }
        else
        {
            InteractWithObject(hit.transform.gameObject);
        }
    }

    protected virtual void InteractWithObject(GameObject target)
    {
        Debug.Log("Child Has Not Implemented Interaction");
    }
}
