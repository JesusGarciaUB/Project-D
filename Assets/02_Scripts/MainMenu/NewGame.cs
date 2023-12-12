using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : Base_Button
{
    public override void MouseClick()
    {
        SceneManager.LoadScene(1);
    }
}
