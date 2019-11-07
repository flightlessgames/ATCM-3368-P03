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
    
    private void Awake()
    {
        _controller = GetComponent<ClickableUnit>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _controller.NewLocation += DriveToLocation;
    }

    public void DriveToLocation(Vector3 position)
    {
        Debug.Log("Drive to: " + position);
        _agent.destination = position;
        Debug.DrawRay(position, Vector3.up * 5f, Color.red, 1f);
    }
}
