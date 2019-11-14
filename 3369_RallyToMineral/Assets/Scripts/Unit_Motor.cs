using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(ClickableUnit))]
[RequireComponent(typeof(NavMeshAgent))]
public class Unit_Motor : MonoBehaviour
{
    public event Action OnMove = delegate { };

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
        Debug.Log("Move to: " + position);
        _agent.destination = position;
        OnMove?.Invoke();
        Debug.DrawRay(position, Vector3.up * 5f, Color.red, 1f);

    }
}