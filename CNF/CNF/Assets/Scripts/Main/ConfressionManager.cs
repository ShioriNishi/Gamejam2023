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
	[SerializeField, Tooltip("���������h�ԓ��e�L�X�g�I�u�W�F�N�g")]
	private GameObject m_villagerOrthodoxReactionObject;
	[SerializeField, Tooltip("�����񐳓��h�ԓ��e�L�X�g�I�u�W�F�N�g")]
	private GameObject m_villagerUnorthodoxReactionObject;
	[SerializeField, Tooltip("�������הh�ԓ��e�L�X�g�I�u�W�F�N�g")]
	private GameObject m_villagerChaosReactionObject;

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
		ChangeViewTextUI(ViewUIType.SisterAdmonish);

		// ���l������\������i3�b��j
		Invoke("ViewVillagerAdmonishReaction", 3.0f);
    }
	/// <summary>�����{�^���������̏���</summary>
	public void OnClickEmpathizeButton()
	{
		// �|�C���g���Z����
		int empathizePointType = m_confressionMasterParam.empathize_point_type;
		AddPoint((PointType)empathizePointType);

		// �V�X�^�[�񓚕����\��
		m_sisterAnswerObject.transform.Find("SisterAnswerText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.sister_empathize_text;
		ChangeViewTextUI(ViewUIType.SisterEmpathize);

		// ���l������\������i3�b��j
		Invoke("ViewVillagerEmpathizeReaction", 3.0f);
	}

	#endregion public
	// ------------------------------

	#region private
	// Start is called before the first frame update
	private void Start()
	{
		// �ϐ�����������
		m_currentConfressionId = 0;
		m_orthodoxPoint = 0;
		m_unorthodoxPoint = 0;
		m_chaosPoint = 0;

		// �I�u�W�F�N�g�̃A�N�e�B�u��������Ԃɂ���
		ChangeViewTextUI(ViewUIType.GameStart);

		// ���X�g�̏�����
		m_unansweredIdList = new List<int>();
		foreach (Entity_confression_master.Param confressionParam in m_confressionMasterDao.param)
		{
			m_unansweredIdList.Add(confressionParam.id);
		}

		NextConfression();
	}

	// Update is called once per frame
	private void Update()
	{
		// Point�̕`��iUpdate�ł��Əd�����H�j
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

		Debug.Log($"�����h�|�C���g = {m_orthodoxPoint} , ��������|�C���g = {m_unorthodoxPoint} , ���׃|�C���g = {m_chaosPoint}");
		// �|�C���gUI�̍X�V
	}

	/// <summary>�萔�Ŏw�肳�ꂽ��ނɍ��킹�ĕ\������TextUI��ς��鏈��</summary>
	private void ChangeViewTextUI(ViewUIType viewUIType)
	{
		switch (viewUIType)
		{
			case ViewUIType.GameStart:					// �Q�[���J�n�i���������j
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(false);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				break;

			case ViewUIType.ConfressionStart:           // �����J�n��
				m_sisterThinkingObject.SetActive(true);	// �Ȃ�Ɠ����悤�E�E�E�͏o��
				m_sisterAnswerObject.SetActive(false);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				break;

			case ViewUIType.SisterAdmonish:				// �V�X�^�[�|�߂�		// �V�X�^�[�񓚎��̂͂ǂ̃p�^�[���ł����o�ꏏ
			case ViewUIType.SisterEmpathize:			// �V�X�^�[����
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				break;

			case ViewUIType.VillagerOrthodoxReaction:   // ���������h����
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(true);	// �����h�����̐����o����\������
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				break;
   
			case ViewUIType.VillagerUnorthodoxReaction:	// �����񐳓��h����
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(true);	// �񐳓��h�����̐����o����\������
				m_villagerChaosReactionObject.SetActive(false);
				break;

			case ViewUIType.VillagerChaosReaction:		// �������הh����
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(true);	// ���הh�����̐����o����\������
				break;

			default:
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

	/// <summary>�u�|�߂�v�ɑ΂��鑺���̔�����\������</summary>
	private void ViewVillagerAdmonishReaction()
	{
		// �񓚌��ʂ̔��f�|�C���g��ʂ𒊏o����
		PointType admonishPointType = (PointType)m_confressionMasterParam.admonish_point_type;

		// UI���|�C���g��ʂɍ��킹�ďo��������
        switch (admonishPointType)
        {
            case PointType.Orthodox:	// �|�߂邪�����h�񓚂������ꍇ
				m_villagerOrthodoxReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_admonish_text;
				ChangeViewTextUI(ViewUIType.VillagerOrthodoxReaction);
                break;

            case PointType.Unorthodox:  // �|�߂邪�񐳓��h�񓚂������ꍇ
				m_villagerUnorthodoxReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_admonish_text;
				ChangeViewTextUI(ViewUIType.VillagerUnorthodoxReaction);
                break;

            case PointType.Chaos:		// �|�߂邪���הh�񓚂������ꍇ
				m_villagerChaosReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_admonish_text;
				ChangeViewTextUI(ViewUIType.VillagerChaosReaction);
                break;

            default:
                break;
        }

		// ���������������o��


		// ���̜������o��
		Invoke("NextConfression", 3.0f);
    }

	/// <summary>�u�����v�ɑ΂��鑺�l�̔�����\������</summary>
	private void ViewVillagerEmpathizeReaction()
	{
		// �񓚌��ʂ̔��f�|�C���g��ʂ𒊏o����
		PointType empathizePointType = (PointType)m_confressionMasterParam.empathize_point_type;

        // UI���|�C���g��ʂɍ��킹�ďo��������
        switch (empathizePointType)
        {
            case PointType.Orthodox:    // �����������h�񓚂������ꍇ
				m_villagerOrthodoxReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_empathize_text;
				ChangeViewTextUI(ViewUIType.VillagerOrthodoxReaction);
                break;

            case PointType.Unorthodox:  // �������񐳓��h�񓚂������ꍇ
				m_villagerUnorthodoxReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_empathize_text;
				ChangeViewTextUI(ViewUIType.VillagerUnorthodoxReaction);
                break;

            case PointType.Chaos:		// ���������הh�񓚂������ꍇ
				m_villagerChaosReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_empathize_text;
				ChangeViewTextUI(ViewUIType.VillagerChaosReaction);
                break;

            default:
                break;
        }

        // ���������������o��

        // ���̜������o��
        Invoke("NextConfression", 3.0f);
    }

    private void NextConfression()
	{
        // �ŏ��̃}�X�^�����߂鏈��
        LotteryCurrentConfressionId();

        // �����𒊏o���鏈��
        m_villagerConfressionText.text = m_confressionMasterParam.villager_confression_text;

        // �����J�n��Ԃɂ���
        ChangeViewTextUI(ViewUIType.ConfressionStart);
    }
    #endregion private
}
