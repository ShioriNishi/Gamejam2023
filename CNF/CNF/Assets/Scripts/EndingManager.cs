using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɃG���f�B���O�̕\��������S���}�l�[�W���[�B
/// </summary>
public class EndingManager : MonoBehaviour
{
    #region field
    [SerializeField, Tooltip("���C���ŃZ�b�g���ꂽ���U���g���")]
    private EndingResultSO m_endingResultSO;

    #endregion field

    #region public
    #endregion public

    #region private
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log($"EndingType = {m_endingResultSO.endingType}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion private
}
