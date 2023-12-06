using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    //[SerializeField] private GameObject explosion;
    [SerializeField] private float delay;
    [SerializeField] private int damage;
    private List<GameObject> enemies = new List<GameObject>();
    private float delayTimer = 0f;
    private bool started = false;

    private void Update()
    {
        if (started)
        {
            delayTimer += Time.deltaTime;
        }

        if (delayTimer >= delay)
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyBehaviour>().ReceiveDamage(damage);
            }
            //GameObject ins = Instantiate(explosion);
            //ins.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject);
            if (!started) started = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
        }
    }
}
