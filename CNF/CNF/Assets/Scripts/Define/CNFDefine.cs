// CNF�p��Define

namespace Confression.Defines
{
	public enum PointType
	{
		/// <summary>�����h�|�C���g</summary>
		Orthodox = 1,
		/// <summary>�񐳓��h�|�C���g</summary>
		Unorthodox,
		/// <summary>���׃|�C���g</summary>
		Chaos,
	}

	public enum MangaMark
	{

	}

	public enum ViewUIType
	{
		/// <summary>�Q�[���J�n</summary>
		GameStart = 1,

		/// <summary>�����J�n</summary>
		ConfressionStart,

		/// <summary>�V�X�^�[�|�߂��</summary>
		SisterAdmonish,
		/// <summary>�V�X�^�[������</summary>
		SisterEmpathize,

		/// <summary>���l�����h����</summary>
		VillagerOrthodoxReaction,
		/// <summary>���l�񐳓��h����</summary>
		VillagerUnorthodoxReaction,
		/// <summary>���l���הh����</summary>
		VillagerChaosReaction,

		/// <summary>�Q�[���I��</summary>
		GameEnd,
	}

	/// <summary>�G���f�B���O���</summary>
	public enum EndingType
	{
		/// <summary>�x�X�g�G���h</summary>
		BestEnding = 1,
		/// <summary>�o�b�h�G���h</summary>
		BadEnding,
		/// <summary>�m�[�}���G���h</summary>
		NormalEnding,
		/// <summary>�����G���h</summary>
		ChaosEnding,
	}
}