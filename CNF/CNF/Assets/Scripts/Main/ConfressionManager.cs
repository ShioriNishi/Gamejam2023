using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// �萔�p
using Confression.Defines;

public class ConfressionManager : MonoBehaviour
{
	#region field
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
	[SerializeField, Tooltip("���Ԑ؂�I�����o�p�I�u�W�F�N�g")]
	private GameObject m_finishObject;

	[SerializeField, Tooltip("�����h�|�C���g�\���e�L�X�g")]
	private TextMeshProUGUI m_meterOrthodoxText;
	[SerializeField, Tooltip("�񐳓��h�|�C���g�\���e�L�X�g")]
	private TextMeshProUGUI m_meterUnorthodoxText;
	[SerializeField, Tooltip("���הh�|�C���g�\���e�L�X�g")]
	private TextMeshProUGUI m_meterChaosText;

	[SerializeField, Tooltip("�\������V�X�^�[�\������X�g")]
	private GameObject[] m_sisterExpressionList = new GameObject[10];
	[SerializeField, Tooltip("�\�����閟���̃��X�g")]
	private GameObject[] m_mangaMarkList = new GameObject[6];

	[SerializeField, Tooltip("�|�߂�{�^��")]
	private Button m_admonishButton;
	[SerializeField, Tooltip("�����{�^��")]
	private Button m_empathizeButton;

	[SerializeField, Tooltip("�����}�X�^")]
	private Entity_confression_master m_confressionMasterDao;

	/// <summary>�^�C�}�[���i��N���X</summary>
	[SerializeField, Tooltip("�^�C�}�[���i��N���X")]
	private RemainingTimeManager m_remainingTimeManager;

	[SerializeField, Tooltip("�G���f�B���O��ʂɈ����n�����U���g���")]
	private EndingResultSO m_endingResultSO;

	/// <summary>�����h�|�C���g�F�������񓚂������Ƃ��ɒ��܂�|�C���g</summary>
	private int m_orthodoxPoint = 0;
	/// <summary>�񐳓��h�|�C���g�F�Ԉ�����񓚂������Ƃ��ɒ��܂�|�C���g</summary>
	private int m_unorthodoxPoint = 0;
	/// <summary>���׃|�C���g�F�[����`���񓚂������Ƃ��ɒ��܂�|�C���g</summary>
	private int m_chaosPoint = 0;

	/// <summary>�Q�[�����I���������̔���t���O</summary>
	private bool m_isFinishGame = false;

	/// <summary>���ݏo�蒆�̜����}�X�^�p�����[�^�[</summary>
	private Entity_confression_master.Param m_confressionMasterParam;
	/// <summary>���o�蒆�̜����}�X�^ID���X�g</summary>
	private List<int> m_unansweredIdList;
	/// <summary>���ݏo�蒆�̜����}�X�^ID</summary>
	private int m_currentConfressionId;

	// ------------------------------
	#region fade // �t�F�[�h�֘A�̊֐�
	/// <summary>�t�F�[�h�J�n����</summary>
	[SerializeField, Tooltip("�t�F�[�h�p�p�l��")]
	private GameObject m_fadeObject;
	/// <summary>�t�F�[�h�J�n����</summary>
	private float m_fadeStartTime;
	/// <summary>�t�F�[�h�҂����ԁA���b�����ăt�F�[�h���邩�̎���</summary>
	private int m_fadeDelayTime;
	/// <summary>�t�F�[�h�p�A���t�@�l</summary>
	private Color m_fadeAlpha;
	/// <summary>�t�F�[�h�̎��</summary>
	private enum FadeMode
	{
		/// <summary>�t�F�[�h�C��</summary>
		FadeIn = 1,
		/// <summary>�t�F�[�h�A�E�g</summary>
		FadeOut,
		/// <summary>�Ȃɂ����Ȃ�</summary>
		DoNothing,
	}
	private FadeMode m_fadeMode;
	#endregion fade

	/// <summary>�V�[���؂�ւ��܂ł̑҂�����</summary>
	private readonly float m_loadSceneDelayTime = 5.0f;
	/// <summary>�V�[���؂�ւ��܂ł̌o�ߎ���</summary>
	private float m_loadSceneElaspedTime;

	#endregion field
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
		ViewSisterExpression((Expression)m_confressionMasterParam.sister_admonish_expression);
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
		ViewSisterExpression((Expression)m_confressionMasterParam.sister_empathize_expression);
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
		// �t�F�[�h�p�ϐ�������
		m_fadeDelayTime = 5;
		m_fadeStartTime = Time.time;
		m_fadeMode = FadeMode.DoNothing;

		// �V�[���J�ڗp�ϐ�������
		m_loadSceneElaspedTime = 0.0f;

		// �ϐ�����������
		m_isFinishGame = false;
		m_currentConfressionId = 0;
		m_orthodoxPoint = 0;
		m_unorthodoxPoint = 0;
		m_chaosPoint = 0;

		// EndingResultSO������
		m_endingResultSO.orthodoxPoint = 0;
		m_endingResultSO.unorthodoxPoint = 0;
		m_endingResultSO.chaosPoint = 0;

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
		if (m_remainingTimeManager.isTimeover() && m_isFinishGame == false)
		{
			// ���Ԑ؂ꉉ�o
			ChangeViewTextUI(ViewUIType.GameEnd);
			m_isFinishGame = true;

			// �t�F�[�h���[�h���t�F�[�h�A�E�g�ɂ���
			//m_fadeMode = FadeMode.FadeOut;	// �Ȃ񂩂������肱�Ȃ��̂�FO����
			m_fadeStartTime = Time.time;
		}

		// �Q�[���I���t���O����������A�J�ڏ����܂Ŏw�莞�ԑ҂��āA�J�ڂ���
		if (m_isFinishGame == true)
		{
			m_loadSceneElaspedTime += Time.deltaTime;

			// �w�莞�Ԍo�߂������H
			if (m_loadSceneElaspedTime > m_loadSceneDelayTime)
			{
				LoadEnding();	// �J�ڏ���
			}
		}

		// �t�F�[�h����
		switch (m_fadeMode)
		{
			case FadeMode.FadeIn:
				m_fadeAlpha.a = 1.0f - (Time.time - m_fadeStartTime) / m_fadeDelayTime;
				m_fadeObject.transform.GetComponent<Image>().color = new Color(0, 0, 0, m_fadeAlpha.a);
				break;
			case FadeMode.FadeOut:
				m_fadeAlpha.a = (Time.time - m_fadeStartTime) / m_fadeDelayTime;
				m_fadeObject.transform.GetComponent<Image>().color = new Color(0, 0, 0, m_fadeAlpha.a);

				break;

			case FadeMode.DoNothing:
				break;
			default:
				break;
		}
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
		//Debug.Log($"�����h�|�C���g = {m_orthodoxPoint} , ��������|�C���g = {m_unorthodoxPoint} , ���׃|�C���g = {m_chaosPoint}");

		m_endingResultSO.orthodoxPoint = m_orthodoxPoint;
		m_endingResultSO.unorthodoxPoint = m_unorthodoxPoint;
		m_endingResultSO.chaosPoint = m_chaosPoint;

		// �|�C���gUI�̍X�V
		m_meterOrthodoxText.text = m_orthodoxPoint.ToString();
		m_meterUnorthodoxText.text = m_unorthodoxPoint.ToString();
		m_meterChaosText.text = m_chaosPoint.ToString();
	}

	/// <summary>�萔�Ŏw�肳�ꂽ��ނɍ��킹�ĕ\������TextUI��ς��鏈��</summary>
	private void ChangeViewTextUI(ViewUIType viewUIType)
	{
		switch (viewUIType)
		{
			case ViewUIType.GameStart:                  // �Q�[���J�n�i���������j
				m_fadeObject.SetActive(false);		// �t�F�[�h�p�p�l���͉B��
				m_finishObject.SetActive(false);		// �I���I�u�W�F�N�g�͉B��
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
				m_admonishButton.interactable = true;   // �{�^���͉�����悤�ɂ���
				m_empathizeButton.interactable = true;
				ViewSisterExpression(Expression.Normal);
				ViewVillagerMangaMark(MangaMark.None);
				break;

			case ViewUIType.SisterAdmonish:				// �V�X�^�[�|�߂�		// �V�X�^�[�񓚎��̂͂ǂ̃p�^�[���ł����o�ꏏ
			case ViewUIType.SisterEmpathize:			// �V�X�^�[����
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				m_admonishButton.interactable = false;  // �{�^���͉����Ȃ�����
				m_empathizeButton.interactable = false;
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

			case ViewUIType.GameEnd:                    // �Q�[���I�����i���Ԑ؂�I���j
				m_fadeObject.SetActive(true);		// �t�F�[�h�p�p�l���͕\������
				m_finishObject.SetActive(true);         // �I�����o�����\������A���̏�Ԃ͉B���Ȃ�
				m_admonishButton.interactable = false;  // �{�^���͉����Ȃ�����
				m_empathizeButton.interactable = false;
				break;

			default:
				break;
		}
	}

	/// <summary>�V�X�^�[�\���\������</summary>
	/// <param name="expression">�\���Ώە\��</param>
	private void ViewSisterExpression(Expression expression)
	{
		for (int i = 0; i < m_sisterExpressionList.Length; i++)
		{
			// �\���Ώە\��Ȃ�\������
			if (i + 1 == (int)expression)
			{
				m_sisterExpressionList[i].SetActive(true);
			}
			else
			{
				m_sisterExpressionList[i].SetActive(false);
			}
		}
	}

	/// <summary>���݂̜����}�X�^ID�����߂鏈��</summary>
	private void LotteryCurrentConfressionId()
	{
		int index = Random.Range(0, m_unansweredIdList.Count);

		m_currentConfressionId = m_unansweredIdList[index];
		m_confressionMasterParam = m_confressionMasterDao.param.Find(cfnMaster => cfnMaster.id == m_currentConfressionId);

		// �Y���̃}�X�^�����X�g����폜���鏈���i�����_���d����h�������j
		m_unansweredIdList.RemoveAt(index);

		Debug.Log($"���݂̃}�X�^ID = {m_currentConfressionId}");
		Debug.Log($"�}�X�^�̎c��ID : {string.Join(",", m_unansweredIdList)}");
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
		MangaMark villagerMangaMark = (MangaMark)m_confressionMasterParam.villager_admonish_manga_mark;
		ViewVillagerMangaMark(villagerMangaMark);
		Debug.Log($"���݂̑��l�̔������� = {villagerMangaMark}");

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
		MangaMark villagerMangaMark = (MangaMark)m_confressionMasterParam.villager_empathize_manga_mark;
		ViewVillagerMangaMark(villagerMangaMark);
		Debug.Log($"���݂̑��l�̔������� = {villagerMangaMark}");

		// ���̜������o��
		Invoke("NextConfression", 3.0f);
	}

	/// <summary>�����̖�����\������</summary>
	/// <param name="mangaMark">�\���Ώۖ���</param>
	private void ViewVillagerMangaMark(MangaMark mangaMark)
	{
		for (int i = 0; i < m_mangaMarkList.Length; i++)
		{
			// �\���Ώۖ����Ȃ�\������
			if (i + 1 == (int)mangaMark)
			{
				m_mangaMarkList[i].SetActive(true);
			}
			else
			{
				m_mangaMarkList[i].SetActive(false);
			}
		}
	}

	/// <summary>���̜�����\�����鏈��</summary>
	private void NextConfression()
	{
		if (m_isFinishGame == true)
		{
			// �Q�[���I���t���O���L���ȏꍇ�͎��̜������o���Ȃ�
			return;
		}

		// �ŏ��̃}�X�^�����߂鏈��
		LotteryCurrentConfressionId();

		// �����𒊏o���鏈��
		m_villagerConfressionText.text = m_confressionMasterParam.villager_confression_text;

		// �����J�n��Ԃɂ���
		ChangeViewTextUI(ViewUIType.ConfressionStart);
	}

	/// <summary>�G���f�B���O��ʂɑJ�ڂ���</summary>
	private void LoadEnding()
	{
		// �G���f�B���O��ނ̕��򏈗�
		if (m_endingResultSO.chaosPoint > 0)
		{
			// ���׃|�C���g��1�_�ȏ゠�����狶���G���h
			m_endingResultSO.endingType = EndingType.ChaosEnding;
		}
		else if (m_endingResultSO.unorthodoxPoint >= 5)
		{
			// �񐳓��h�i��������j�|�C���g����萔�ȏ�Ȃ�o�b�h�G���h
			m_endingResultSO.endingType = EndingType.BadEnding;
		}
		else if (m_endingResultSO.orthodoxPoint >= 5)
		{
			// �����h�|�C���g����萔�ȏ�Ȃ琳���h�G���h
			m_endingResultSO.endingType = EndingType.BestEnding;
		}
		else
		{
			// �ǂ�ɂ����Ă͂܂�Ȃ�������m�[�}���G���h
			m_endingResultSO.endingType = EndingType.NormalEnding;
		}

		SceneManager.LoadScene("Ending");
	}
	#endregion private
}
