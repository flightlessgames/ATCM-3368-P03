using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineral_Controller : MonoBehaviour
{
    [SerializeField] Vector3 _closePoint = Vector3.zero;
    public Vector3 MiningPoint { get { return _closePoint; } }

    private GameObject _nearestHQ = null;

    private void Awake()
    {
        CheckForNearHQ();
    }
    

    private void CheckForNearHQ()
    {
        GameObject[] allHQ = GameObject.FindGameObjectsWithTag("Headquarters");
        float minDist = Mathf.Infinity;
        foreach (GameObject hq in allHQ)
        {
            float dist = (hq.transform.position - transform.position).magnitude;
            if (dist < minDist) { _nearestHQ = hq; }
        }

        transform.LookAt(_nearestHQ.transform);
        _closePoint = transform.position + transform.forward;
    }
}
