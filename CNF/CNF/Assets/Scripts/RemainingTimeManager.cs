using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemainingTimeManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_remainingTimeText;
    private float m_remainingTime;

    // Start is called before the first frame update
    void Start()
    {
        m_remainingTime = 60.0f;
        m_remainingTimeText.text = m_remainingTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        m_remainingTime -= Time.deltaTime;
        if (m_remainingTime > 0)
        {
            m_remainingTimeText.text = m_remainingTime.ToString();
        }
        else
        {
            m_remainingTimeText.text = "0";
        }

    }
}
