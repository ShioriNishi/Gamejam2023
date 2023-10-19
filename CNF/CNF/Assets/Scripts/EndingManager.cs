using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主にエンディングの表示分けを担うマネージャー。
/// </summary>
public class EndingManager : MonoBehaviour
{
    #region field
    [SerializeField, Tooltip("メインでセットされたリザルト情報")]
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
