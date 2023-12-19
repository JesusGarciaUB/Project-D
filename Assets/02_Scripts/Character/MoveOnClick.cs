using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

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
    private bool attackedThisFrame = false;

    //CONTROLADORES DE DONDE ESTA EL RATON
    private bool onGround = false;
    private bool onEnemy = false;
    private bool onRange = false;
    private bool onSkilltree = false;
    private bool onPauseMenu = false;

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

    //PAUSE
    [SerializeField] private GameObject pause;

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
        attackedThisFrame = false;

        //JANDUMODE
        if(Input_Manager._INPUT_MANAGER.Get1Pressed())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(Input_Manager._INPUT_MANAGER.Get2Pressed())
        {
            combatSystem.SkillTreePoints += 50;
            Level_Manager._LEVELMANAGER.skillpointsText.text = combatSystem.SkillTreePoints.ToString();
        }

        //menu de pausa
        if (Input_Manager._INPUT_MANAGER.GetPPressed())
        {
            pause.SetActive(!pause.activeSelf);
            onPauseMenu = pause.activeSelf;
        }

        //si estamos muertos hacemos reset al movimiento del agente
        if (!combatSystem.isAlive)
        {
            myAgent.SetDestination(myAgent.transform.position);
            destination = myAgent.transform.position;
        }

        //para abrir/cerrar el arbol de habilidades
        if (Input_Manager._INPUT_MANAGER.GetNPressed())
        {
            skilltree.SetActive(!skilltree.activeSelf);
            onSkilltree = skilltree.activeSelf;
            if (!onSkilltree) Level_Manager._LEVELMANAGER.onHoverObject.SetActive(false);
        }

        //solo podremos enviar inputs si estamos fuera del arbol de habilidades y del menu de pausa
        if (!onSkilltree && !onPauseMenu)
        {
            //comprobamos que estemos vivos
            if (combatSystem.isAlive)
            {
                if (abilities.Count > 0)
                {
                    //Q
                    if (Input_Manager._INPUT_MANAGER.GetQPressed() && abilities[0] != null)
                    {
                        if (isActivatable[0] && !alwaysOn[0]) abilities[0].SetActive(!abilities[0].activeSelf);
                        else if (isActivatable[0] && alwaysOn[0]) abilities[0].GetComponent<Base_ability>().Performed();
                    }
                }

                if (abilities.Count > 1)
                {
                    //W
                    if (Input_Manager._INPUT_MANAGER.GetWPressed() && abilities[1] != null)
                    {
                        if (isActivatable[1] && !alwaysOn[1]) abilities[1].SetActive(!abilities[1].activeSelf);
                        else if (isActivatable[1] && alwaysOn[1]) abilities[1].GetComponent<Base_ability>().Performed();
                    }
                }

                if (abilities.Count > 2)
                {
                    //E
                    if (Input_Manager._INPUT_MANAGER.GetEPressed() && abilities[2] != null)
                    {
                        if (isActivatable[2] && !alwaysOn[2]) abilities[2].SetActive(!abilities[2].activeSelf);
                        else if (isActivatable[2] && alwaysOn[2]) abilities[2].GetComponent<Base_ability>().Performed();
                    }
                }

                if (abilities.Count > 3)
                {
                    //R
                    if (Input_Manager._INPUT_MANAGER.GetRPressed() && abilities[3] != null)
                    {
                        if (isActivatable[3] && !alwaysOn[3]) abilities[3].SetActive(!abilities[3].activeSelf);
                        else if (isActivatable[3] && alwaysOn[3]) abilities[3].GetComponent<Base_ability>().Performed();
                    }
                }
            }

            //Setup por frame del raycast | TODO posiblemente se deba mover al principio si algunas habilidades dependen de ser apuntadas
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
                        attacking = true;
                        attackedThisFrame = true;
                        combatSystem.attackTarget = combatSystem.target;

                        myAgent.SetDestination(myAgent.transform.position);
                        destination = myAgent.transform.position;
                        myAgent.transform.LookAt(combatSystem.target.transform);
                    }
                }
            }
        }
    }

    //nos setea las variables de posicion del raton, si estamos encima de un enemigo, suelo, etc..
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

    //funcion llamada por la animacion de atacar y por el controlador de inputs para determinar si estamos atacando
    public void Attacking()
    {
        attacking = false;
    }

    //Getters para el script de animaciones
    public bool isMoving()
    {
        return Vector3.Distance(myAgent.transform.position, destination) > myAgent.stoppingDistance;
    }

    public bool isAttacking()
    {
        return attacking;
    }

    public bool AttackedThisFrame()
    {
        return attackedThisFrame;
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

    //nos devuelve la siguiente posicion libre en la barra de acceso rapido
    public int GetNextFreeSlot() { return this.nextFreeSlot; }
    //funcion llamada por el arbol de habilidades para aplicar una habilidad a la barra de acceso rapido
    public void UseNextFreeSlot(int pos, GameObject ability, bool activatable, bool aOn, Sprite icon) 
    {
        abilities.Add(ability);
        isActivatable[pos] = activatable;
        alwaysOn[pos] = aOn;

        InstanceActivatables(pos);
        nextFreeSlot++;

        abilities[pos].GetComponent<Base_ability>().SetUI(icon, quickAccessBar[pos].transform.GetChild(3).GetComponent<TextMeshProUGUI>(), quickAccessBar[pos].transform.GetChild(0).gameObject, quickAccessBar[pos].transform.GetChild(2).gameObject);
    }

    //funcion llamada por el arbol de habilidades para subir de nivel una habilidad. Si la habilidad es una activa, reseteamos su estado para que aplique las nuevas stats
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
