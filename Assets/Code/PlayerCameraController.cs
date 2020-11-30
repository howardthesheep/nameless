
using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCameraController : MonoBehaviour
{
    public Vector3 cameraOffset;
    
    private Camera _cam;
    private NavMeshAgent _agent;
    private float _currentZoom = 4f;
    private void Start()
    {
        _cam = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        cameraOffset = new Vector3(0.0f, -4.45f, 4.5f);
    }

    private void Update()
    {
        Transform cameraTransform = _cam.transform;
        Transform agentTransform = _agent.transform;
        
        cameraTransform.position = agentTransform.position - cameraOffset * _currentZoom;
        cameraTransform.LookAt(agentTransform);
    }
    
    /********************************************** 
    * 
    * Helper Functions for Private Variables
    * 
    **********************************************/
    public Camera getCamera()
    {
        return _cam;
    }
    
    public float getCurrentZoom()
    {
        return _currentZoom;
    }

    public void setCurrentZoom(float zoom)
    {
        float _minZoom = 1.0f, _maxZoom = 8.0f;
        _currentZoom = Mathf.Clamp(zoom, _minZoom, _maxZoom);
    }
}
