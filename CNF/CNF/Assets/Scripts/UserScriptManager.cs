using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserScriptManager : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_textFile;

	/// <summary>���͒��̕������Ă������߂̃��X�g</summary>
	private List<string> m_sentences = new List<string>();


	/// <summary>���݂̍s�̕����擾����</summary>
	/// <returns>string�^�B���݂̍s�̕�</returns>
	public string GetCurrentSentence()
    {
		return m_sentences[GameSystemManager.Instance.LineNumber];
    }

	/// <summary>�e�L�X�g�̍ő�s���擾����</summary>
	/// <returns>int�^�B�ő�s��</returns>
	public int GetMaxSentenceCount()
    {
		return m_sentences.Count;
    }

    private void Awake()
    {
		// �e�L�X�g�t�@�C���̒��g���A1�s�����X�g�ɓ���Ă���
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
