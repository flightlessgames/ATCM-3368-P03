using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SCV_Motor))]
[RequireComponent(typeof(ClickableUnit))]
public class SCV_Controller : ClickableUnit
{
    public event Action<Mineral_Controller> isMining = delegate { };

    private SCV_Motor _motor = null;
    private ClickableUnit _brain = null;

    private Mineral_Controller _minerals;
    private Coroutine _miningRoutine = null;
    

    private void Awake()
    {
        _motor = GetComponent<SCV_Motor>();
        _brain = GetComponent<ClickableUnit>();
    }

    private void OnEnable()
    {
        _brain.TargetObject += InteractWithObject;
    }
    protected override void InteractWithObject(GameObject target)
    {
        Debug.Log("Interacting with " + target.transform.name);
        //switch cases: minerals, structure, default
        //strucures switch cases: command center, default

        switch (target.tag)
        {
            case "Mineral":
                Debug.Log("Mining Minerals");

                _minerals = target.transform.GetComponent<Mineral_Controller>();
                _motor.DriveToLocation(_minerals.MiningPoint);

                isMining?.Invoke(_minerals);

                //todo Contact Minerals to create Queue
                //in minerals script, if queue is longer than smallest+1 in Mineral Group, send new Target (other minerals)
                break;
            default:
                break;
        }
    }

    private IEnumerator MiningCycle()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("End Mining");
    }
}
