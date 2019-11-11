using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneNavigation : MonoBehaviour
{
    int index = 0;
    public void GotoScene(string sceneNameParam)
    {
        SceneManager.LoadScene(sceneNameParam);
    }
    public void GotoScene()
    {
        switch (index)
        {
            case 0:
                GotoScene("No Enemies");
                break;
            case 1:
                GotoScene("Homing Mine");
                break;
            case 2:
                GotoScene("Homing Missile");
                break;
            case 3:
                GotoScene("Fighter");
                break;
            case 4:
                GotoScene("Bomber");
                break;
        }

    }
    public void setIndex(int i)
    {
        index = i;
    }

}
