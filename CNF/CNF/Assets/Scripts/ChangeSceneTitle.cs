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

//�Q�[���I��
public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
  }
}

