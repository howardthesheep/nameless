
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerAnimationController : MonoBehaviour
{
    
    public Animator animator;
    private NavMeshAgent _agent;

    private void Start()
    {
        // Grab agent
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Update running animation based on agent speed
        animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
}
