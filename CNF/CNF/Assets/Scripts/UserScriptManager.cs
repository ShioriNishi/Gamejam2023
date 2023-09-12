using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserScriptManager : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_textFile;

	/// <summary>文章中の文を入れておくためのリスト</summary>
	private List<string> m_sentences = new List<string>();


	/// <summary>現在の行の文を取得する</summary>
	/// <returns>string型。現在の行の文</returns>
	public string GetCurrentSentence()
    {
		return m_sentences[GameSystemManager.Instance.LineNumber];
    }

	/// <summary>テキストの最大行を取得する</summary>
	/// <returns>int型。最大行数</returns>
	public int GetMaxSentenceCount()
    {
		return m_sentences.Count;
    }

    private void Awake()
    {
		// テキストファイルの中身を、1行ずつリストに入れておく
		StringReader reader = new StringReader(m_textFile.text);
        while (reader.Peek() != -1)
        {
			string line = reader.ReadLine();
			m_sentences.Add(line);
        }
    }

    // Start is called before the first frame update
    void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
