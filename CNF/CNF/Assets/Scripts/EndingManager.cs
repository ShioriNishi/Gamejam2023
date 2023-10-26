using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Confression.Defines;

/// <summary>
/// 主にエンディングの表示分けを担うマネージャー。
/// </summary>
public class EndingManager : MonoBehaviour
{
	#region field
	[SerializeField, Tooltip("メインでセットされたリザルト情報")]
	private EndingResultSO m_endingResultSO;

	[SerializeField, Tooltip("ベストエンド表示用画像オブジェクト")]
	private GameObject m_bestEndingImageObject;
	[SerializeField, Tooltip("ベストエンド表示用文言オブジェクト")]
	private GameObject m_bestEndingTextObject;
	[SerializeField, Tooltip("ベストエンド表示用AudioSource")]
	private AudioSource m_bestEndingAudioSource;
	// [SerializeField, Tooltip("ベストエンド表示用AudioClip")]
	// private AudioClip m_bestEndingAudioClip;

	[SerializeField, Tooltip("バッドエンド表示用画像オブジェクト")]
	private GameObject m_badEndingImageObject;
	[SerializeField, Tooltip("バッドエンド表示用文言オブジェクト")]
	private GameObject m_badEndingTextObject;
	[SerializeField, Tooltip("バットエンド表示用AudioSource")]
	private AudioSource m_badEndingAudioSource;
	// [SerializeField, Tooltip("バットエンド表示用AudioClip")]
	// private AudioClip m_badEndingAudioClip;

	[SerializeField, Tooltip("ノーマルエンド表示用画像オブジェクト")]
	private GameObject m_normalEndingImageObject;
	[SerializeField, Tooltip("ノーマルエンド表示用文言オブジェクト")]
	private GameObject m_normalEndingTextObject;
	[SerializeField, Tooltip("ノーマルエンド表示用AudioSource")]
	private AudioSource m_normalEndingAudioSource;
	// [SerializeField, Tooltip("ノーマルエンド表示用AudioClip")]
	// private AudioClip m_normalEndingAudioClip;

	[SerializeField, Tooltip("狂化エンド表示用画像オブジェクト")]
	private GameObject m_chaosEndingImageObject;
	[SerializeField, Tooltip("狂化エンド表示用文言オブジェクト")]
	private GameObject m_chaosEndingTextObject;
	[SerializeField, Tooltip("狂化エンド表示用AudioSource")]
	private AudioSource m_chaosEndingAudioSource;
	// [SerializeField ,Tooltip("狂化エンド表示用AudioClip")]
	// private AudioClip m_chaosEndingAudioClip;
	#endregion field

	#region public
	#endregion public

	#region private
	// Start is called before the first frame update
	private void Start()
	{
		Debug.Log($"EndingType = {m_endingResultSO.endingType}");

		// BGMの設定
		// m_bestEndingAudioSource.clip = m_bestEndingAudioClip;
		// m_badEndingAudioSource.clip = m_badEndingAudioClip;
		// m_normalEndingAudioSource.clip = m_normalEndingAudioClip;
		// m_chaosEndingAudioSource.clip = m_chaosEndingAudioClip;

		ViewEnding(m_endingResultSO.endingType);
	}

	// Update is called once per frame
	private void Update()
	{
		
	}

	private void ViewEnding(EndingType endingType)
	{
		// 全Endingを一旦非表示にする
		m_bestEndingImageObject.SetActive(false);
		m_bestEndingTextObject.SetActive(false);
		m_badEndingImageObject.SetActive(false);
		m_badEndingTextObject.SetActive(false);
		m_normalEndingImageObject.SetActive(false);
		m_normalEndingTextObject.SetActive(false);
		m_chaosEndingImageObject.SetActive(false);
		m_chaosEndingTextObject.SetActive(false);

		switch (endingType)
		{
			case EndingType.BestEnding:     // ベストエンド表示
				m_bestEndingImageObject.SetActive(true);
				m_bestEndingTextObject.SetActive(true);
				m_bestEndingAudioSource.Play();
				break;

			case EndingType.BadEnding:      // バッドエンド表示
				m_badEndingImageObject.SetActive(true);
				m_badEndingTextObject.SetActive(true);
				m_badEndingAudioSource.Play();
				break;

			case EndingType.NormalEnding:   // ノーマルエンド表示
				m_normalEndingImageObject.SetActive(true);
				m_normalEndingTextObject.SetActive(true);
				m_normalEndingAudioSource.Play();
				break;

			case EndingType.ChaosEnding:    // 狂化エンド表示
				m_chaosEndingImageObject.SetActive(true);
				m_chaosEndingTextObject.SetActive(true);
				m_chaosEndingAudioSource.Play();
				break;

			default:
				break;
		}
	}
	#endregion private
}
