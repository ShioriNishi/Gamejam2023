using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
	// �ʂ̃N���X����GameSystemManager�̕ϐ��Ȃǂ��g����悤�ɂ��邽�߂ɃC���X�^���X���B
	public static GameSystemManager Instance { get; private set; }

	public UserScriptManager userScriptManager;
	public MainTextController mainTextController;

	/// <summary>UserScript�̍��̍s�̐��l�B�N���b�N�̓x��1��������B</summary>
	public int LineNumber { get => m_lineNumber; private set => m_lineNumber = value; }
	private int m_lineNumber;

	public void AddLineNumber(int add = 1)
    {
        if (LineNumber < userScriptManager.GetMaxSentenceCount() - 1)
        {
			LineNumber += add;
		}
	}

	private void Awake()
	{
		Instance = this;

		LineNumber = 0;
	}

	// Start is called before the first frame update
	private void Start()
	{
		
	}

	// Update is called once per frame
	private void Update()
	{
		
	}
}
