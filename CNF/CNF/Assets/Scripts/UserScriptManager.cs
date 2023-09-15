using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserScriptManager : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_textFile;

	[SerializeField]
	private Entity_confression_master m_confressionMasterDao;

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
		string testText = m_confressionMasterDao.param.Find(cnfMaster => cnfMaster.id == 1).villager_confression_text;
		Debug.Log(testText);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
