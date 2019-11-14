using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickableUnit : MonoBehaviour
{
    public event Action<Vector3> NewLocation = delegate { };

    protected void ChildInvokeNewLocation(Vector3 location)
    {
        NewLocation?.Invoke(location);
    }

    public void RollCall()
    {
        Debug.Log("Selected Unit[s]: " + gameObject.name);
    }

    public void IdentifyHit(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Ground"))
        {
            Debug.Log("New Location: " + hit.point);
            NewLocation?.Invoke(hit.point);
        }
        else
        {
            Debug.Log("I am " + gameObject.name + ", Indentify " + hit.transform.name);
            InteractWithObject(hit.transform.gameObject);
        }
    }

    protected virtual void InteractWithObject(GameObject target)
    {
        Debug.Log("Child Has Not Implemented Interaction");
    }
}
