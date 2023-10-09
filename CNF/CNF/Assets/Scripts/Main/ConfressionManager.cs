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
	[SerializeField, Tooltip("村民正統派返答テキストオブジェクト")]
	private GameObject m_villagerOrthodoxReactionObject;
	[SerializeField, Tooltip("村民非正統派返答テキストオブジェクト")]
	private GameObject m_villagerUnorthodoxReactionObject;
	[SerializeField, Tooltip("村民混沌派返答テキストオブジェクト")]
	private GameObject m_villagerChaosReactionObject;

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

	/// <summary>現在出題中の懺悔マスタパラメーター</summary>
	private Entity_confression_master.Param m_confressionMasterParam;
	/// <summary>未出題中の懺悔マスタIDリスト</summary>
	private List<int> m_unansweredIdList;
	/// <summary>現在出題中の懺悔マスタID</summary>
	private int m_currentConfressionId;

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
		ChangeViewTextUI(ViewUIType.SisterAdmonish);

		// 村人反応を表示する（3秒後）
		Invoke("ViewVillagerAdmonishReaction", 3.0f);
    }
	/// <summary>同調ボタン押下時の処理</summary>
	public void OnClickEmpathizeButton()
	{
		// ポイント加算処理
		int empathizePointType = m_confressionMasterParam.empathize_point_type;
		AddPoint((PointType)empathizePointType);

		// シスター回答文言表示
		m_sisterAnswerObject.transform.Find("SisterAnswerText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.sister_empathize_text;
		ChangeViewTextUI(ViewUIType.SisterEmpathize);

		// 村人反応を表示する（3秒後）
		Invoke("ViewVillagerEmpathizeReaction", 3.0f);
	}

	#endregion public
	// ------------------------------

	#region private
	// Start is called before the first frame update
	private void Start()
	{
		// 変数初期化処理
		m_currentConfressionId = 0;
		m_orthodoxPoint = 0;
		m_unorthodoxPoint = 0;
		m_chaosPoint = 0;

		// オブジェクトのアクティブを初期状態にする
		ChangeViewTextUI(ViewUIType.GameStart);

		// リストの初期化
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
		// Pointの描画（Updateでやると重いか？）
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

		Debug.Log($"正統派ポイント = {m_orthodoxPoint} , ぐうたらポイント = {m_unorthodoxPoint} , 混沌ポイント = {m_chaosPoint}");
		// ポイントUIの更新
	}

	/// <summary>定数で指定された種類に合わせて表示するTextUIを変える処理</summary>
	private void ChangeViewTextUI(ViewUIType viewUIType)
	{
		switch (viewUIType)
		{
			case ViewUIType.GameStart:					// ゲーム開始（初期化時）
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(false);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				break;

			case ViewUIType.ConfressionStart:           // 懺悔開始時
				m_sisterThinkingObject.SetActive(true);	// なんと答えよう・・・は出す
				m_sisterAnswerObject.SetActive(false);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				break;

			case ViewUIType.SisterAdmonish:				// シスター諫める		// シスター回答自体はどのパターンでも演出一緒
			case ViewUIType.SisterEmpathize:			// シスター同調
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				break;

			case ViewUIType.VillagerOrthodoxReaction:   // 村民正統派反応
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(true);	// 正統派反応の吹き出しを表示する
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				break;
   
			case ViewUIType.VillagerUnorthodoxReaction:	// 村民非正統派反応
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(true);	// 非正統派反応の吹き出しを表示する
				m_villagerChaosReactionObject.SetActive(false);
				break;

			case ViewUIType.VillagerChaosReaction:		// 村民混沌派反応
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(true);	// 混沌派反応の吹き出しを表示する
				break;

			default:
				break;
		}
	}

	/// <summary>現在の懺悔マスタIDを決める処理</summary>
	private void LotteryCurrentConfressionId()
	{
		m_currentConfressionId = UnityEngine.Random.Range(1, m_unansweredIdList.Count);
		m_confressionMasterParam = m_confressionMasterDao.param.Find(cfnMaster => cfnMaster.id == m_currentConfressionId);

		// 該当のマスタをリストから削除する処理を追加すること（ランダム重複を防ぎたい）
		m_unansweredIdList.Remove(m_currentConfressionId);

	}

	/// <summary>「諫める」に対する村民の反応を表示する</summary>
	private void ViewVillagerAdmonishReaction()
	{
		// 回答結果の反映ポイント種別を抽出する
		PointType admonishPointType = (PointType)m_confressionMasterParam.admonish_point_type;

		// UIをポイント種別に合わせて出し分ける
        switch (admonishPointType)
        {
            case PointType.Orthodox:	// 諫めるが正統派回答だった場合
				m_villagerOrthodoxReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_admonish_text;
				ChangeViewTextUI(ViewUIType.VillagerOrthodoxReaction);
                break;

            case PointType.Unorthodox:  // 諫めるが非正統派回答だった場合
				m_villagerUnorthodoxReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_admonish_text;
				ChangeViewTextUI(ViewUIType.VillagerUnorthodoxReaction);
                break;

            case PointType.Chaos:		// 諫めるが混沌派回答だった場合
				m_villagerChaosReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_admonish_text;
				ChangeViewTextUI(ViewUIType.VillagerChaosReaction);
                break;

            default:
                break;
        }

		// 村民反応漫符を出す


		// 次の懺悔を出す
		Invoke("NextConfression", 3.0f);
    }

	/// <summary>「同調」に対する村人の反応を表示する</summary>
	private void ViewVillagerEmpathizeReaction()
	{
		// 回答結果の反映ポイント種別を抽出する
		PointType empathizePointType = (PointType)m_confressionMasterParam.empathize_point_type;

        // UIをポイント種別に合わせて出し分ける
        switch (empathizePointType)
        {
            case PointType.Orthodox:    // 同調が正統派回答だった場合
				m_villagerOrthodoxReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_empathize_text;
				ChangeViewTextUI(ViewUIType.VillagerOrthodoxReaction);
                break;

            case PointType.Unorthodox:  // 同調が非正統派回答だった場合
				m_villagerUnorthodoxReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_empathize_text;
				ChangeViewTextUI(ViewUIType.VillagerUnorthodoxReaction);
                break;

            case PointType.Chaos:		// 同調が混沌派回答だった場合
				m_villagerChaosReactionObject.transform.Find("VillagerReactionText").GetComponent<TextMeshProUGUI>().text = m_confressionMasterParam.villager_empathize_text;
				ChangeViewTextUI(ViewUIType.VillagerChaosReaction);
                break;

            default:
                break;
        }

        // 村民反応漫符を出す

        // 次の懺悔を出す
        Invoke("NextConfression", 3.0f);
    }

    private void NextConfression()
	{
        // 最初のマスタを決める処理
        LotteryCurrentConfressionId();

        // 文言を抽出する処理
        m_villagerConfressionText.text = m_confressionMasterParam.villager_confression_text;

        // 懺悔開始状態にする
        ChangeViewTextUI(ViewUIType.ConfressionStart);
    }
    #endregion private
}
