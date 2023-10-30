using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// 定数用
using Confression.Defines;

public class ConfressionManager : MonoBehaviour
{
	#region field
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
	[SerializeField, Tooltip("時間切れ終了演出用オブジェクト")]
	private GameObject m_finishObject;

	[SerializeField, Tooltip("正統派ポイント表示テキスト")]
	private TextMeshProUGUI m_meterOrthodoxText;
	[SerializeField, Tooltip("非正統派ポイント表示テキスト")]
	private TextMeshProUGUI m_meterUnorthodoxText;
	[SerializeField, Tooltip("混沌派ポイント表示テキスト")]
	private TextMeshProUGUI m_meterChaosText;

	[SerializeField, Tooltip("表示するシスター表情差分リスト")]
	private GameObject[] m_sisterExpressionList = new GameObject[10];
	[SerializeField, Tooltip("表示する漫符のリスト")]
	private GameObject[] m_mangaMarkList = new GameObject[6];

	[SerializeField, Tooltip("諫めるボタン")]
	private Button m_admonishButton;
	[SerializeField, Tooltip("同調ボタン")]
	private Button m_empathizeButton;

	[SerializeField, Tooltip("懺悔マスタ")]
	private Entity_confression_master m_confressionMasterDao;

	/// <summary>タイマーを司るクラス</summary>
	[SerializeField, Tooltip("タイマーを司るクラス")]
	private RemainingTimeManager m_remainingTimeManager;

	[SerializeField, Tooltip("エンディング画面に引き渡すリザルト情報")]
	private EndingResultSO m_endingResultSO;

	/// <summary>正統派ポイント：正しい回答をしたときに貯まるポイント</summary>
	private int m_orthodoxPoint = 0;
	/// <summary>非正統派ポイント：間違った回答をしたときに貯まるポイント</summary>
	private int m_unorthodoxPoint = 0;
	/// <summary>混沌ポイント：深淵を覗く回答をしたときに貯まるポイント</summary>
	private int m_chaosPoint = 0;

	/// <summary>ゲームが終了したかの判定フラグ</summary>
	private bool m_isFinishGame = false;

	/// <summary>現在出題中の懺悔マスタパラメーター</summary>
	private Entity_confression_master.Param m_confressionMasterParam;
	/// <summary>未出題中の懺悔マスタIDリスト</summary>
	private List<int> m_unansweredIdList;
	/// <summary>現在出題中の懺悔マスタID</summary>
	private int m_currentConfressionId;

	// ------------------------------
	#region fade // フェード関連の関数
	/// <summary>フェード開始時間</summary>
	[SerializeField, Tooltip("フェード用パネル")]
	private GameObject m_fadeObject;
	/// <summary>フェード開始時間</summary>
	private float m_fadeStartTime;
	/// <summary>フェード待ち時間、何秒かけてフェードするかの時間</summary>
	private int m_fadeDelayTime;
	/// <summary>フェード用アルファ値</summary>
	private Color m_fadeAlpha;
	/// <summary>フェードの種類</summary>
	private enum FadeMode
	{
		/// <summary>フェードイン</summary>
		FadeIn = 1,
		/// <summary>フェードアウト</summary>
		FadeOut,
		/// <summary>なにもしない</summary>
		DoNothing,
	}
	private FadeMode m_fadeMode;
	#endregion fade

	/// <summary>シーン切り替えまでの待ち時間</summary>
	private readonly float m_loadSceneDelayTime = 5.0f;
	/// <summary>シーン切り替えまでの経過時間</summary>
	private float m_loadSceneElaspedTime;

	#endregion field
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
		ViewSisterExpression((Expression)m_confressionMasterParam.sister_admonish_expression);
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
		ViewSisterExpression((Expression)m_confressionMasterParam.sister_empathize_expression);
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
		// フェード用変数初期化
		m_fadeDelayTime = 5;
		m_fadeStartTime = Time.time;
		m_fadeMode = FadeMode.DoNothing;

		// シーン遷移用変数初期化
		m_loadSceneElaspedTime = 0.0f;

		// 変数初期化処理
		m_isFinishGame = false;
		m_currentConfressionId = 0;
		m_orthodoxPoint = 0;
		m_unorthodoxPoint = 0;
		m_chaosPoint = 0;

		// EndingResultSO初期化
		m_endingResultSO.orthodoxPoint = 0;
		m_endingResultSO.unorthodoxPoint = 0;
		m_endingResultSO.chaosPoint = 0;

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
		if (m_remainingTimeManager.isTimeover() && m_isFinishGame == false)
		{
			// 時間切れ演出
			ChangeViewTextUI(ViewUIType.GameEnd);
			m_isFinishGame = true;

			// フェードモードをフェードアウトにする
			//m_fadeMode = FadeMode.FadeOut;	// なんかしっくりこないのでFO封印中
			m_fadeStartTime = Time.time;
		}

		// ゲーム終了フラグが立ったら、遷移処理まで指定時間待って、遷移する
		if (m_isFinishGame == true)
		{
			m_loadSceneElaspedTime += Time.deltaTime;

			// 指定時間経過したか？
			if (m_loadSceneElaspedTime > m_loadSceneDelayTime)
			{
				LoadEnding();	// 遷移処理
			}
		}

		// フェード処理
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
		//Debug.Log($"正統派ポイント = {m_orthodoxPoint} , ぐうたらポイント = {m_unorthodoxPoint} , 混沌ポイント = {m_chaosPoint}");

		m_endingResultSO.orthodoxPoint = m_orthodoxPoint;
		m_endingResultSO.unorthodoxPoint = m_unorthodoxPoint;
		m_endingResultSO.chaosPoint = m_chaosPoint;

		// ポイントUIの更新
		m_meterOrthodoxText.text = m_orthodoxPoint.ToString();
		m_meterUnorthodoxText.text = m_unorthodoxPoint.ToString();
		m_meterChaosText.text = m_chaosPoint.ToString();
	}

	/// <summary>定数で指定された種類に合わせて表示するTextUIを変える処理</summary>
	private void ChangeViewTextUI(ViewUIType viewUIType)
	{
		switch (viewUIType)
		{
			case ViewUIType.GameStart:                  // ゲーム開始（初期化時）
				m_fadeObject.SetActive(false);		// フェード用パネルは隠す
				m_finishObject.SetActive(false);		// 終了オブジェクトは隠す
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
				m_admonishButton.interactable = true;   // ボタンは押せるようにする
				m_empathizeButton.interactable = true;
				ViewSisterExpression(Expression.Normal);
				ViewVillagerMangaMark(MangaMark.None);
				break;

			case ViewUIType.SisterAdmonish:				// シスター諫める		// シスター回答自体はどのパターンでも演出一緒
			case ViewUIType.SisterEmpathize:			// シスター同調
				m_sisterThinkingObject.SetActive(false);
				m_sisterAnswerObject.SetActive(true);
				m_villagerOrthodoxReactionObject.SetActive(false);
				m_villagerUnorthodoxReactionObject.SetActive(false);
				m_villagerChaosReactionObject.SetActive(false);
				m_admonishButton.interactable = false;  // ボタンは押せなくする
				m_empathizeButton.interactable = false;
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

			case ViewUIType.GameEnd:                    // ゲーム終了時（時間切れ終了）
				m_fadeObject.SetActive(true);		// フェード用パネルは表示する
				m_finishObject.SetActive(true);         // 終了演出だけ表示する、他の状態は隠さない
				m_admonishButton.interactable = false;  // ボタンは押せなくする
				m_empathizeButton.interactable = false;
				break;

			default:
				break;
		}
	}

	/// <summary>シスター表情を表示する</summary>
	/// <param name="expression">表示対象表情</param>
	private void ViewSisterExpression(Expression expression)
	{
		for (int i = 0; i < m_sisterExpressionList.Length; i++)
		{
			// 表示対象表情なら表示する
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

	/// <summary>現在の懺悔マスタIDを決める処理</summary>
	private void LotteryCurrentConfressionId()
	{
		int index = Random.Range(0, m_unansweredIdList.Count);

		m_currentConfressionId = m_unansweredIdList[index];
		m_confressionMasterParam = m_confressionMasterDao.param.Find(cfnMaster => cfnMaster.id == m_currentConfressionId);

		// 該当のマスタをリストから削除する処理（ランダム重複を防ぎたい）
		m_unansweredIdList.RemoveAt(index);

		Debug.Log($"現在のマスタID = {m_currentConfressionId}");
		Debug.Log($"マスタの残りID : {string.Join(",", m_unansweredIdList)}");
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
		MangaMark villagerMangaMark = (MangaMark)m_confressionMasterParam.villager_admonish_manga_mark;
		ViewVillagerMangaMark(villagerMangaMark);
		Debug.Log($"現在の村人の反応漫符 = {villagerMangaMark}");

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
		MangaMark villagerMangaMark = (MangaMark)m_confressionMasterParam.villager_empathize_manga_mark;
		ViewVillagerMangaMark(villagerMangaMark);
		Debug.Log($"現在の村人の反応漫符 = {villagerMangaMark}");

		// 次の懺悔を出す
		Invoke("NextConfression", 3.0f);
	}

	/// <summary>村民の漫符を表示する</summary>
	/// <param name="mangaMark">表示対象漫符</param>
	private void ViewVillagerMangaMark(MangaMark mangaMark)
	{
		for (int i = 0; i < m_mangaMarkList.Length; i++)
		{
			// 表示対象漫符なら表示する
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

	/// <summary>次の懺悔を表示する処理</summary>
	private void NextConfression()
	{
		if (m_isFinishGame == true)
		{
			// ゲーム終了フラグが有効な場合は次の懺悔を出さない
			return;
		}

		// 最初のマスタを決める処理
		LotteryCurrentConfressionId();

		// 文言を抽出する処理
		m_villagerConfressionText.text = m_confressionMasterParam.villager_confression_text;

		// 懺悔開始状態にする
		ChangeViewTextUI(ViewUIType.ConfressionStart);
	}

	/// <summary>エンディング画面に遷移する</summary>
	private void LoadEnding()
	{
		// エンディング種類の分岐処理
		if (m_endingResultSO.chaosPoint > 0)
		{
			// 混沌ポイントが1点以上あったら狂化エンド
			m_endingResultSO.endingType = EndingType.ChaosEnding;
		}
		else if (m_endingResultSO.unorthodoxPoint >= 5)
		{
			// 非正統派（ぐうたら）ポイントが一定数以上ならバッドエンド
			m_endingResultSO.endingType = EndingType.BadEnding;
		}
		else if (m_endingResultSO.orthodoxPoint >= 5)
		{
			// 正統派ポイントが一定数以上なら正統派エンド
			m_endingResultSO.endingType = EndingType.BestEnding;
		}
		else
		{
			// どれにも当てはまらなかったらノーマルエンド
			m_endingResultSO.endingType = EndingType.NormalEnding;
		}

		SceneManager.LoadScene("Ending");
	}
	#endregion private
}
