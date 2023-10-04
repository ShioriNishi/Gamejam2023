using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// �萔�p
using Confression.Defines;

public class ConfressionManager : MonoBehaviour
{
	[SerializeField, Tooltip("���������e�L�X�g�{�b�N�X")]
	private TextMeshProUGUI m_villagerConfressionText;
	[SerializeField, Tooltip("�V�X�^�[�l�����e�L�X�g�I�u�W�F�N�g")]
	private GameObject m_sisterThinkingObject;
	[SerializeField, Tooltip("�V�X�^�[�񓚃e�L�X�g�I�u�W�F�N�g")]
	private GameObject m_sisterAnswerObject;
	[SerializeField, Tooltip("�����ԓ��e�L�X�g�I�u�W�F�N�g")]
	private GameObject m_villagerReactionObject;

	[SerializeField, Tooltip("�|�߂�{�^��")]
	private Button m_admonishButton;
	[SerializeField, Tooltip("�����{�^��")]
	private Button m_empathizeButton;

	[SerializeField, Tooltip("�����}�X�^")]
	private Entity_confression_master m_confressionMasterDao;

	/// <summary>�����h�|�C���g�F�������񓚂������Ƃ��ɒ��܂�|�C���g</summary>
	private int m_orthodoxPoint = 0;
	/// <summary>�񐳓��h�|�C���g�F�Ԉ�����񓚂������Ƃ��ɒ��܂�|�C���g</summary>
	private int m_unorthodoxPoint = 0;
	/// <summary>���׃|�C���g�F�[����`���񓚂������Ƃ��ɒ��܂�|�C���g</summary>
	private int m_chaosPoint = 0;

	/// <summary>���ݏo�蒆�̜����}�X�^�p�����[�^�[</summary>
	private Entity_confression_master.Param m_confressionMasterParam;
	/// <summary>���o�蒆�̜����}�X�^ID���X�g</summary>
	private List<int> m_unansweredIdList;
	/// <summary>���ݏo�蒆�̜����}�X�^ID</summary>
	private int m_currentConfressionId;

	// ------------------------------
	// public
	#region public
	/// <summary>�|�߂�{�^���������̏���</summary>
	public void OnClickAdmonishButton()
	{
		// �|�C���g���Z����
		int admonishPointType = m_confressionMasterParam.admonish_point_type;
		AddPoint((PointType)admonishPointType);

		// �V�X�^�[�񓚕����\��
		m_sisterAnswerObject.transform.Find("SisterAnswerText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.sister_admonish_text;
		m_sisterAnswerObject.SetActive(true);
		m_sisterThinkingObject.SetActive(false);

		//Invoke()
	}
	/// <summary>�����{�^���������̏���</summary>
	public void OnClickEmpathizeButton()
	{
		// �|�C���g���Z����
		int empathizePointType = m_confressionMasterParam.empathize_point_type;
		AddPoint((PointType)empathizePointType);

		// �V�X�^�[�񓚕����\��
		m_sisterAnswerObject.transform.Find("SisterAnswerText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.sister_empathize_text;
		m_sisterAnswerObject.SetActive(true);
		m_sisterThinkingObject.SetActive(false);
	}

	#endregion public
	// ------------------------------

	#region private
	// Start is called before the first frame update
	private void Start()
	{
		// ����������
		m_currentConfressionId = 0;
		m_orthodoxPoint = 0;
		m_unorthodoxPoint = 0;
		m_chaosPoint = 0;
		m_sisterAnswerObject.SetActive(false);


		// ���X�g�̏�����
		m_unansweredIdList = new List<int>();
		foreach (Entity_confression_master.Param confressionParam in m_confressionMasterDao.param)
		{
			m_unansweredIdList.Add(confressionParam.id);
		}

		// �ŏ��̃}�X�^�����߂鏈��
		LotteryCurrentConfressionId();

		// �����𒊏o���鏈��
		m_villagerConfressionText.text = m_confressionMasterParam.villager_confression_text;
	}

	// Update is called once per frame
	private void Update()
	{
		
	}

	/// <summary>���������Ƀ|�C���g�ɉ��Z���鏈��</summary>
	private void AddPoint(PointType type)
	{
		switch (type)
		{
			case PointType.Orthodox:
				m_orthodoxPoint++;
				break;
			case PointType.Unorthodox:
				m_unorthodoxPoint++;
				break;
			case PointType.Chaos:
				m_chaosPoint++;
				break;
			default:
				Debug.LogWarning("�ǂ��ɂ����Z����܂���ł����I�v�m�F PointType = " + type);
				break;
		}
	}

	/// <summary>���݂̜����}�X�^ID�����߂鏈��</summary>
	private void LotteryCurrentConfressionId()
	{
		m_currentConfressionId = UnityEngine.Random.Range(1, m_unansweredIdList.Count);
		m_confressionMasterParam = m_confressionMasterDao.param.Find(cfnMaster => cfnMaster.id == m_currentConfressionId);

		// �Y���̃}�X�^�����X�g����폜���鏈����ǉ����邱�Ɓi�����_���d����h�������j
		m_unansweredIdList.Remove(m_currentConfressionId);

	}

	private void ViewVillagerAdmonishReaction()
    {

    }

	private void ViewVillagerEmpathizeReaction()
    {

    }
	#endregion private
}
