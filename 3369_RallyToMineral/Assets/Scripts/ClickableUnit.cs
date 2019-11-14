using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClickableUnit : MonoBehaviour
{
    public event Action<Vector3> NewLocation = delegate { };

    [SerializeField] CanvasGroup _buttonCanvas = null;

    protected void ChildInvokeNewLocation(Vector3 location)
    {
        NewLocation?.Invoke(location);
    }

    public void RollCall()
    {
        _buttonCanvas.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        _buttonCanvas.gameObject.SetActive(false);
    }

    public void Identify(GameObject target)
    {
        if (target.CompareTag("Ground"))
        {
            NewLocation?.Invoke(target.transform.position);
        }
        else
        {
            InteractWithObject(target);
        }
    }

    protected virtual void InteractWithObject(GameObject target)
    {
        Debug.Log("Child Has Not Implemented Interaction");
    }
}
