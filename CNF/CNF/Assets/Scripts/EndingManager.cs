using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Confression.Defines;

/// <summary>
/// ��ɃG���f�B���O�̕\��������S���}�l�[�W���[�B
/// </summary>
public class EndingManager : MonoBehaviour
{
	#region field
	[SerializeField, Tooltip("���C���ŃZ�b�g���ꂽ���U���g���")]
	private EndingResultSO m_endingResultSO;

	[SerializeField, Tooltip("�x�X�g�G���h�\���p�摜�I�u�W�F�N�g")]
	private GameObject m_bestEndingImageObject;
	[SerializeField, Tooltip("�x�X�g�G���h�\���p�����I�u�W�F�N�g")]
	private GameObject m_bestEndingTextObject;
	[SerializeField, Tooltip("�x�X�g�G���h�\���pAudioSource")]
	private AudioSource m_bestEndingAudioSource;
	// [SerializeField, Tooltip("�x�X�g�G���h�\���pAudioClip")]
	// private AudioClip m_bestEndingAudioClip;

	[SerializeField, Tooltip("�o�b�h�G���h�\���p�摜�I�u�W�F�N�g")]
	private GameObject m_badEndingImageObject;
	[SerializeField, Tooltip("�o�b�h�G���h�\���p�����I�u�W�F�N�g")]
	private GameObject m_badEndingTextObject;
	[SerializeField, Tooltip("�o�b�g�G���h�\���pAudioSource")]
	private AudioSource m_badEndingAudioSource;
	// [SerializeField, Tooltip("�o�b�g�G���h�\���pAudioClip")]
	// private AudioClip m_badEndingAudioClip;

	[SerializeField, Tooltip("�m�[�}���G���h�\���p�摜�I�u�W�F�N�g")]
	private GameObject m_normalEndingImageObject;
	[SerializeField, Tooltip("�m�[�}���G���h�\���p�����I�u�W�F�N�g")]
	private GameObject m_normalEndingTextObject;
	[SerializeField, Tooltip("�m�[�}���G���h�\���pAudioSource")]
	private AudioSource m_normalEndingAudioSource;
	// [SerializeField, Tooltip("�m�[�}���G���h�\���pAudioClip")]
	// private AudioClip m_normalEndingAudioClip;

	[SerializeField, Tooltip("�����G���h�\���p�摜�I�u�W�F�N�g")]
	private GameObject m_chaosEndingImageObject;
	[SerializeField, Tooltip("�����G���h�\���p�����I�u�W�F�N�g")]
	private GameObject m_chaosEndingTextObject;
	[SerializeField, Tooltip("�����G���h�\���pAudioSource")]
	private AudioSource m_chaosEndingAudioSource;
	// [SerializeField ,Tooltip("�����G���h�\���pAudioClip")]
	// private AudioClip m_chaosEndingAudioClip;
	#endregion field

	#region public
	#endregion public

	#region private
	// Start is called before the first frame update
	private void Start()
	{
		Debug.Log($"EndingType = {m_endingResultSO.endingType}");

		// BGM�̐ݒ�
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
		// �SEnding����U��\���ɂ���
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
			case EndingType.BestEnding:     // �x�X�g�G���h�\��
				m_bestEndingImageObject.SetActive(true);
				m_bestEndingTextObject.SetActive(true);
				m_bestEndingAudioSource.Play();
				break;

			case EndingType.BadEnding:      // �o�b�h�G���h�\��
				m_badEndingImageObject.SetActive(true);
				m_badEndingTextObject.SetActive(true);
				m_badEndingAudioSource.Play();
				break;

			case EndingType.NormalEnding:   // �m�[�}���G���h�\��
				m_normalEndingImageObject.SetActive(true);
				m_normalEndingTextObject.SetActive(true);
				m_normalEndingAudioSource.Play();
				break;

			case EndingType.ChaosEnding:    // �����G���h�\��
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
