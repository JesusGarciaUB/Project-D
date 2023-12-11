using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial_Manager : MonoBehaviour
{
    //TEXTS
    [SerializeField] private TextMeshProUGUI tomove;
    [SerializeField] private TextMeshProUGUI toattack;
    [SerializeField] private TextMeshProUGUI tolevelup;
    [SerializeField] private TextMeshProUGUI tocomplete;

    //AUXILIARES
    private bool activated = false;
    private bool com = false;
    [SerializeField] private List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

    private void Update()
    {
        if (tomove.gameObject.activeSelf)
        {
            if (Level_Manager._LEVELMANAGER.player.GetComponent<MoveOnClick>().isMoving())
            {
                tomove.gameObject.SetActive(false);
                toattack.gameObject.SetActive(true);
            }
        }

        if (toattack.gameObject.activeSelf)
        {
            if (Level_Manager._LEVELMANAGER.player.GetComponent<MoveOnClick>().isAttacking())
            {
                toattack.gameObject.SetActive(false);
            }
        }

        if (Level_Manager._LEVELMANAGER.player.GetComponent<ExpSystem>().GetCurrentLevel() == 2 && !activated)
        {
            activated = true;
            tolevelup.gameObject.SetActive(true);
        }

        if (tolevelup.gameObject.activeSelf)
        {
            if (Input_Manager._INPUT_MANAGER.GetNPressed()) tolevelup.gameObject.SetActive(false);
        }

        int dead = 0;
        foreach (EnemyBehaviour enemy in enemies)
        {
            if (!enemy.GetisAlive()) dead++;
        }

        if (dead == 5 && !com)
        {
            StartCoroutine(completed());
            com = true;
        }
    }

    private IEnumerator completed()
    {
        tolevelup.gameObject.SetActive(false);
        tocomplete.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(2);
    }
}
