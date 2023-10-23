using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTitle : MonoBehaviour
{
public void OnClickStartButton()
    {
        SceneManager.LoadScene("Prologue");
    }
 
}
   