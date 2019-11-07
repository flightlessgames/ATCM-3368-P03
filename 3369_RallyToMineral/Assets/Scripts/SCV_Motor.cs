using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(ClickableUnit))]
[RequireComponent(typeof(NavMeshAgent))]
public class SCV_Motor : MonoBehaviour
{
    public event Action<Mineral_Controller> isMining = delegate { };

    private ClickableUnit _controller;
    private NavMeshAgent _agent;
    private Mineral_Controller _minerals;

    private Coroutine _miningRoutine = null;

    private void Awake()
    {
        _controller = GetComponent<ClickableUnit>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _controller.NewLocation += DriveToLocation;
        _controller.TargetObject += InteractWithObject;
    }

    private void DriveToLocation(Vector3 position)
    {
        Debug.Log("Drive to: " + position);
        _agent.destination = position;
        Debug.DrawRay(position, Vector3.up * 5f, Color.red, 1f);
    }

    private void InteractWithObject(GameObject target)
    {
        //switch cases: minerals, structure, default
        //strucures switch cases: command center, default

        switch (target.tag)
        {
            case "Mineral":
                Debug.Log("Mining Minerals");

                _minerals = target.transform.GetComponent<Mineral_Controller>();
                DriveToLocation(_minerals.MiningPoint);

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
