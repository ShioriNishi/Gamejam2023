using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 定数用
using Confression.Defines;

public class ConfressionManager : MonoBehaviour
{
	[SerializeField, Tooltip("村民懺悔テキストボックス")]
	private TextMeshProUGUI m_villagerConfressionText;
	[SerializeField, Tooltip("シスター考え中テキストオブジェクト")]
	private GameObject m_sisterThinkingObject;
	[SerializeField, Tooltip("シスター回答テキストオブジェクト")]
	private GameObject m_sisterAnswerObject;
	[SerializeField, Tooltip("村民返答テキストオブジェクト")]
	private GameObject m_villagerReactionObject;

	[SerializeField, Tooltip("諫めるボタン")]
	private Button m_admonishButton;
	[SerializeField, Tooltip("同調ボタン")]
	private Button m_empathizeButton;

	[SerializeField, Tooltip("懺悔マスタ")]
	private Entity_confression_master m_confressionMasterDao;

	/// <summary>正統派ポイント：正しい回答をしたときに貯まるポイント</summary>
	private int m_orthodoxPoint = 0;
	/// <summary>非正統派ポイント：間違った回答をしたときに貯まるポイント</summary>
	private int m_unorthodoxPoint = 0;
	/// <summary>混沌ポイント：深淵を覗く回答をしたときに貯まるポイント</summary>
	private int m_chaosPoint = 0;

	private Entity_confression_master.Param m_confressionMasterParam;
	private List<int> m_unselectedIdList;
	private int m_nowConfressionId;

	// ------------------------------
	// public
	#region public
	/// <summary>諫めるボタン押下時の処理</summary>
	public void OnClickAdmonishButton()
	{
		// ポイント加算処理
		int admonishPointType = m_confressionMasterParam.admonish_point_type;
		AddPoint((PointType)admonishPointType);

		// シスター回答文言表示
		m_sisterAnswerObject.transform.Find("SisterAnswerText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.sister_admonish_text;
	}
	/// <summary>同調ボタン押下時の処理</summary>
	public void OnClickEmpathizeButton()
	{
		// ポイント加算処理
		int empathizePointType = m_confressionMasterParam.empathize_point_type;
		AddPoint((PointType)empathizePointType);

		// シスター回答文言表示
		m_sisterAnswerObject.transform.Find("SisterAnswerText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.sister_empathize_text;
	}

	#endregion public
	// ------------------------------

	#region private
	// Start is called before the first frame update
	private void Start()
	{
		// 初期化処理
		m_nowConfressionId = 0;
		m_orthodoxPoint = 0;
		m_unorthodoxPoint = 0;
		m_chaosPoint = 0;


		// リストの初期化
		m_unselectedIdList = new List<int>();
		foreach (Entity_confression_master.Param confressionParam in m_confressionMasterDao.param)
		{
			m_unselectedIdList.Add(confressionParam.id);
		}

		// 最初のマスタを決める処理
		SetNowConfressionId();

		// 文言を抽出する処理

	}

	// Update is called once per frame
	private void Update()
	{
		
	}

	/// <summary>引数を元にポイントに加算する処理</summary>
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
				Debug.LogWarning("どこにも加算されませんでした！要確認 PointType = " + type);
				break;
		}
	}

	/// <summary>現在の懺悔マスタIDを決める処理</summary>
	private void SetNowConfressionId()
	{
		m_nowConfressionId = UnityEngine.Random.Range(1, m_unselectedIdList.Count);
		// 該当のマスタをリストから削除する処理を追加すること（ランダム重複を防ぎたい）
	}
	#endregion private
}
