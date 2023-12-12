using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCredits : MonoBehaviour
{
    [SerializeField] private List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();
    private bool com = false;

    private void Update()
    {
        int dead = 0;

        foreach (EnemyBehaviour enemy in enemies)
        {
            if (!enemy.GetisAlive()) dead++;
        }

        if (dead == enemies.Count && !com)
        {
            StartCoroutine(completed());
            com = true;
        }
    }

    private IEnumerator completed()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(3);
    }
}
