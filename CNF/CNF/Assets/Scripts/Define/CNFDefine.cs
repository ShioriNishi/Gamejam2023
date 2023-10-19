// CNF用のDefine

namespace Confression.Defines
{
	/// <summary>加算ポイント種別</summary>
	public enum PointType
	{
		/// <summary>正統派ポイント</summary>
		Orthodox = 1,
		/// <summary>非正統派ポイント</summary>
		Unorthodox,
		/// <summary>混沌ポイント</summary>
		Chaos,
	}

	/// <summary>村民反応漫符</summary>
	public enum MangaMark
	{
		/// <summary>表示なし</summary>
		None = 0,
		/// <summary>ハート</summary>
		Love = 1,
		/// <summary>イライラ</summary>
		Angry,
		/// <summary>ぐるぐる</summary>
		Confused,
		/// <summary>汗</summary>
		Sweat,
		/// <summary>びっくり</summary>
		Surprised,
		/// <summary>わいわい</summary>
		Enlighted,
	}

	/// <summary>UI表示タイプ（どのタイミングで出すUIか）</summary>
	public enum ViewUIType
	{
		/// <summary>ゲーム開始</summary>
		GameStart = 1,

		/// <summary>懺悔開始</summary>
		ConfressionStart,

		/// <summary>シスター諫める回答</summary>
		SisterAdmonish,
		/// <summary>シスター同調回答</summary>
		SisterEmpathize,

		/// <summary>村人正統派反応</summary>
		VillagerOrthodoxReaction,
		/// <summary>村人非正統派反応</summary>
		VillagerUnorthodoxReaction,
		/// <summary>村人混沌派反応</summary>
		VillagerChaosReaction,

		/// <summary>ゲーム終了</summary>
		GameEnd,
	}

	/// <summary>エンディング種類</summary>
	public enum EndingType
	{
		/// <summary>ベストエンド</summary>
		BestEnding = 1,
		/// <summary>バッドエンド</summary>
		BadEnding,
		/// <summary>ノーマルエンド</summary>
		NormalEnding,
		/// <summary>狂化エンド</summary>
		ChaosEnding,
	}
}