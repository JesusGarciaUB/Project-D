using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MoveOnClick : MonoBehaviour
{
    public LayerMask clicableArea;
    public LayerMask enemyLayer;
    private NavMeshAgent myAgent;
    private Vector3 destination;

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        destination = myAgent.transform.position;
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input_Manager._INPUT_MANAGER.GetMousePosition());
        RaycastHit hit;

        if (Input_Manager._INPUT_MANAGER.GetClickPressed())
        {
            if (Physics.Raycast(ray, out hit, 100, clicableArea))
            {
                destination = hit.point;
                myAgent.SetDestination(hit.point);
            }
        }

        if (Physics.Raycast(ray, out hit, 100, enemyLayer))
        {
            hit.transform.gameObject.GetComponent<EnemyBehaviour>().MouseOverMob();
        }
    }

    //Getters
    public bool isMoving()
    {
        return Vector3.Distance(myAgent.transform.position, destination) > myAgent.stoppingDistance;
    }
}
