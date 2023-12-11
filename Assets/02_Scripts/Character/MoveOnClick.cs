using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using TMPro;

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
    private bool onSkilltree = false;

    //HITS DEL RAYCAST DEL RATON
    private Vector3 hitGround;
    private Transform hitEnemy;

    //HABILIDADES
    [SerializeField] private List<GameObject> abilities = new List<GameObject>();
    [SerializeField] private bool[] isActivatable = { false, false, false, false };
    [SerializeField] private bool[] alwaysOn = { false, false, false, false };
    private int nextFreeSlot = 0;

    //SKILLTREE
    [SerializeField] private GameObject skilltree;

    //ICONS
    [SerializeField] private GameObject[] quickAccessBar;
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

        if (Input_Manager._INPUT_MANAGER.GetNPressed())
        {
            skilltree.SetActive(!skilltree.activeSelf);
            onSkilltree = skilltree.activeSelf;
            if (!onSkilltree) Level_Manager._LEVELMANAGER.onHoverObject.SetActive(false);
        }

        if (!onSkilltree)
        {
            //botones habilidades
            if (combatSystem.isAlive)
            {
                if (abilities.Count > 0)
                {
                    if (Input_Manager._INPUT_MANAGER.GetQPressed() && abilities[0] != null)
                    {
                        if (isActivatable[0] && !alwaysOn[0]) abilities[0].SetActive(!abilities[0].activeSelf);
                        else if (isActivatable[0] && alwaysOn[0]) abilities[0].GetComponent<Base_ability>().Performed();
                    }
                }

                if (abilities.Count > 1)
                {
                    if (Input_Manager._INPUT_MANAGER.GetWPressed() && abilities[1] != null)
                    {
                        if (isActivatable[1] && !alwaysOn[1]) abilities[1].SetActive(!abilities[1].activeSelf);
                        else if (isActivatable[1] && alwaysOn[1]) abilities[1].GetComponent<Base_ability>().Performed();
                    }
                }

                if (abilities.Count > 2)
                {
                    if (Input_Manager._INPUT_MANAGER.GetEPressed() && abilities[2] != null)
                    {
                        if (isActivatable[2] && !alwaysOn[2]) abilities[2].SetActive(!abilities[2].activeSelf);
                        else if (isActivatable[2] && alwaysOn[2]) abilities[2].GetComponent<Base_ability>().Performed();
                    }
                }

                if (abilities.Count > 3)
                {
                    if (Input_Manager._INPUT_MANAGER.GetRPressed() && abilities[3] != null)
                    {
                        if (isActivatable[3] && !alwaysOn[3]) abilities[3].SetActive(!abilities[3].activeSelf);
                        else if (isActivatable[3] && alwaysOn[3]) abilities[3].GetComponent<Base_ability>().Performed();
                    }
                }
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

    //si las habilidades son activas, las instanciamos para que solo haya 1 en escena
    public void InstanceActivatables(int pos)
    {
        if (isActivatable[pos])
        {
            abilities[pos] = Instantiate(abilities[pos]);
            if (!alwaysOn[pos]) abilities[pos].SetActive(false);
            if (pos == 0) Level_Manager._LEVELMANAGER.main_ability = abilities[pos];
        }
    }

    public int GetNextFreeSlot() { return this.nextFreeSlot; }
    public void UseNextFreeSlot(int pos, GameObject ability, bool activatable, bool aOn, Sprite icon) 
    {
        abilities.Add(ability);
        isActivatable[pos] = activatable;
        alwaysOn[pos] = aOn;

        InstanceActivatables(pos);
        nextFreeSlot++;

        abilities[pos].GetComponent<Base_ability>().SetUI(icon, quickAccessBar[pos].transform.GetChild(3).GetComponent<TextMeshProUGUI>(), quickAccessBar[pos].transform.GetChild(0).gameObject, quickAccessBar[pos].transform.GetChild(2).gameObject);
    }

    public void LevelUpAbility(int pos)
    {
        bool wasActive = false;
        if (abilities[pos].activeSelf)
        {
            abilities[pos].SetActive(false);
            wasActive = true;
        }
        abilities[pos].GetComponent<Base_ability>().LevelUpAbility();
        if (wasActive) abilities[pos].SetActive(true);
        
    }
}
