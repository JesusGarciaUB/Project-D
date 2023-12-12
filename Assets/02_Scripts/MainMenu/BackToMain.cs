using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : Base_Button
{
    public override void MouseClick()
    {
        SceneManager.LoadScene(0);
    }
}
