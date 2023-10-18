using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Confression.Defines;

[System.Serializable, CreateAssetMenu(menuName = "MyScriptable/Create EndingResultSO")]
public class EndingResultSO : ScriptableObject
{
	public EndingType endingType;
	public int orthodoxPoint = 0;
	public int unorthodoxPoint = 0;
	public int chaosPoint = 0;
}
