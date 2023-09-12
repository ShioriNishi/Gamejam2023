using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
	// 別のクラスからGameSystemManagerの変数などを使えるようにするためにインスタンス化。
	public static GameSystemManager Instance { get; private set; }

	public UserScriptManager userScriptManager;
	public MainTextController mainTextController;

	/// <summary>UserScriptの今の行の数値。クリックの度に1ずつ増える。</summary>
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
