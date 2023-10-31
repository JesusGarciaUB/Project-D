using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MoveOnClick : MonoBehaviour
{
    public LayerMask clicableArea;
    private NavMeshAgent myAgent;

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (Input_Manager._INPUT_MANAGER.GetClickPressed())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input_Manager._INPUT_MANAGER.GetMousePosition());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, clicableArea))
            {
                myAgent.SetDestination(hit.point);
            }
        }
    }
}
