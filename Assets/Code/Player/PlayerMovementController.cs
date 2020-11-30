using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Vector3 _destination = Vector3.zero;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_destination != Vector3.zero)
        {
            _agent.SetDestination(_destination);   
        }
    }

    public void setDestination(Vector3 dest)
    {
        _destination = dest;
    }

    // Getter Setter for _agent
    public NavMeshAgent agent
    {
        get => _agent;
        set => _agent = value;
    }
}