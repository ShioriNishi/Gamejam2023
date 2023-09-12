using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainTextController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_mainTextObject;


    /// <summary>
    /// 次の行へ移動
    /// </summary>
    public void GoToTheNextLine()
    {
        GameSystemManager.Instance.AddLineNumber();
    }

    /// <summary>
    /// テキストを表示
    /// </summary>
    public void DisplayText()
    {
        string sentence = GameSystemManager.Instance.userScriptManager.GetCurrentSentence();
        m_mainTextObject.text = sentence;
    }    

    // Start is called before the first frame update
    private void Start()
    {
        DisplayText();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GoToTheNextLine();
            DisplayText();
        }
    }

}
