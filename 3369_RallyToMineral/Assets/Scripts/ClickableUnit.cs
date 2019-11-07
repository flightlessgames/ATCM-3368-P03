using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickableUnit : MonoBehaviour
{
    public event Action<Vector3> NewLocation = delegate { };
    public event Action<GameObject> TargetObject = delegate { };

    private PlayerInput _input;

    private void Awake()
    {
        _input = GameObject.Find("GameManager").GetComponent<PlayerInput>();
    }

    private void PickNewLocation(Vector3 location)
    {
        NewLocation?.Invoke(location);
        Debug.Log("New Location: " + location);
    }

    public void RollCall()
    {
        Debug.Log("Selected Unit[s]: " + gameObject.name);
    }

    public void IdentifyHit(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Ground"))
        {
            PickNewLocation(hit.point);
        }
        else
        {
            PickNewLocation(hit.transform.position);
            TargetObject?.Invoke(hit.transform.gameObject);
        }
    }
}
