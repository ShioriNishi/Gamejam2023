using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemainingTimeManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_remainingTimeText;
	private float m_remainingTime;

	/// <summary>
	/// ŠÔØ‚ê‚É‚È‚Á‚Ä‚¢‚é‚©‚Ì”»’è
	/// </summary>
	/// <returns>bool ŠÔØ‚ê‚È‚çTrue</returns>
	public bool isTimeover()
	{
		return m_remainingTime <= 0;
	}

	// Start is called before the first frame update
	void Start()
	{
		m_remainingTime = 60.0f;
		m_remainingTimeText.text = m_remainingTime.ToString("00");
	}

	// Update is called once per frame
	void Update()
	{
		m_remainingTime -= Time.deltaTime;
		if (m_remainingTime > 0)
		{
			m_remainingTimeText.text = m_remainingTime.ToString("00");
		}
		else
		{
			m_remainingTimeText.text = "00";
		}

	}
}
