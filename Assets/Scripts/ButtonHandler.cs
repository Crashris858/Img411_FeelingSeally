using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ButtonHandler : MonoBehaviour
{

    //load easy scene 
     public void OnEasyClick()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    public void OnMediumClick()
    {
        SceneManager.LoadScene("_Scene_1");
    }

         public void OnHardClick()
    {
        SceneManager.LoadScene("_Scene_2");
    }
}
