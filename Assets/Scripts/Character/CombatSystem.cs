using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public GameObject target;
    [SerializeField] private float basicAttackRange;

    private void Update()
    {
        
    }

    public bool OnRange()
    {
        return target != null && Vector3.Distance(target.transform.position, transform.position) <= basicAttackRange;
    }
}
