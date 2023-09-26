using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainTextController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_mainTextObject;

	private int m_displayedSentenceLength;
	private int m_sentenceLength;
	/// <summary>�o�ߎ���</summary>
	private float m_elapsedTime;
	/// <summary>�A�b�v�f�[�g����</summary>
	private float m_feedTime;


	public bool CanGoToTheNextLine()
	{
		string sentence = GameSystemManager.Instance.userScriptManager.GetCurrentSentence();
		m_sentenceLength = sentence.Length;
		return (m_displayedSentenceLength > sentence.Length);
	}

	/// <summary>���̍s�ֈړ�</summary>
	public void GoToTheNextLine()
	{
		m_displayedSentenceLength = 0;
		m_elapsedTime = 0.0f;
		m_mainTextObject.maxVisibleCharacters = 0;

		GameSystemManager.Instance.AddLineNumber();
	}

	/// <summary>
	/// �e�L�X�g��\��
	/// </summary>
	public void DisplayText()
	{
		string sentence = GameSystemManager.Instance.userScriptManager.GetCurrentSentence();    // ������DB�����ɕς���
		m_mainTextObject.text = sentence;
	}    



	// Start is called before the first frame update
	private void Start()
	{
		m_elapsedTime = 0.0f;
		//m_feedTime = 0.05f;
		m_feedTime = 0.1f;
		
		DisplayText();
	}

	// Update is called once per frame
	private void Update()
	{
		// ���͂�1�������\������
		m_elapsedTime += Time.deltaTime;
		//Debug.Log($"m_elapsedTime = {m_elapsedTime}");
		if (m_elapsedTime >= m_feedTime)
		{
			m_elapsedTime -= m_feedTime;
			if (!CanGoToTheNextLine())
			{
				m_displayedSentenceLength++;
				m_mainTextObject.maxVisibleCharacters = m_displayedSentenceLength;
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			if (CanGoToTheNextLine())
			{
				GoToTheNextLine();
				DisplayText();
			}
			else
			{
				m_displayedSentenceLength = m_sentenceLength;
			}
		}

	}

}
