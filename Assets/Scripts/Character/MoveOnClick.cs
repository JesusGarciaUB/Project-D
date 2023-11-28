using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MoveOnClick : MonoBehaviour
{
    //LAYERS
    public LayerMask clicableArea;
    public LayerMask enemyLayer;
    //AGENT
    private NavMeshAgent myAgent;
    //AUXILIARES
    private Vector3 destination;
    private CombatSystem combatSystem;
    public bool attacking = false;

    //CONTROLADORES DE DONDE ESTA EL RATON
    private bool onGround = false;
    private bool onEnemy = false;
    private bool onRange = false;

    //HITS DEL RAYCAST DEL RATON
    private Vector3 hitGround;
    private Transform hitEnemy;

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        destination = myAgent.transform.position;
        combatSystem = GetComponent<CombatSystem>();
    }
    private void Update()
    {
        //si estamos muertos hacemos reset al movimiento del agente
        if (!combatSystem.isAlive)
        {
            myAgent.SetDestination(myAgent.transform.position);
            destination = myAgent.transform.position;
        }

        //Setup por frame del raycast
        MouseOver();

        //Si el raton esta encima de un enemigo, el enemigo se pone como target en el script de combatsystem
        if (onEnemy)
        {
            hitEnemy.gameObject.GetComponent<EnemyBehaviour>().MouseOverMob();
        }
        else GetComponent<CombatSystem>().target = null;

        //Esto es setup tambien, pero debe ir despues de settear el enemigo como target
        onRange = combatSystem.OnRange();

        //comprovamos que no estemos en medio de atacar ni que estemos muertos
        if (!attacking && combatSystem.isAlive)
        {
            //Click izquierdo
            if (Input_Manager._INPUT_MANAGER.GetClickPressed())
            {
                //Si pulsamos en el suelo o enemigo y no esta a rango nos movemos
                if (onGround && !onRange)
                {
                    destination = hitGround;
                    myAgent.SetDestination(hitGround);
                }
                //Si pulsamos en un enemigo y estamos a rango
                else if (onEnemy && onRange)
                {
                    Attacking();
                    combatSystem.attackTarget = combatSystem.target;

                    myAgent.SetDestination(myAgent.transform.position);
                    destination = myAgent.transform.position;
                    myAgent.transform.LookAt(combatSystem.target.transform);
                }
            }
        }
    }

    private void MouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input_Manager._INPUT_MANAGER.GetMousePosition());
        RaycastHit hit;

        //comprobamos el raton en layer ground
        onGround = Physics.Raycast(ray, out hit, 100, clicableArea);
        if (onGround) hitGround = hit.point;

        //comprobamos el raton en layer enemy
        onEnemy = Physics.Raycast(ray, out hit, 100, enemyLayer);
        if (onEnemy) hitEnemy = hit.transform;
    }

    public void Attacking()
    {
        attacking = !attacking;
    }

    //Getters
    public bool isMoving()
    {
        return Vector3.Distance(myAgent.transform.position, destination) > myAgent.stoppingDistance;
    }

    public bool isAttacking()
    {
        return attacking;
    }
}
