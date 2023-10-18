// CNF用のDefine

namespace Confression.Defines
{
	public enum PointType
	{
		/// <summary>正統派ポイント</summary>
		Orthodox = 1,
		/// <summary>非正統派ポイント</summary>
		Unorthodox,
		/// <summary>混沌ポイント</summary>
		Chaos,
	}

	public enum MangaMark
	{

	}

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
}