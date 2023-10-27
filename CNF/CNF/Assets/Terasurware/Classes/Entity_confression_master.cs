using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_confression_master : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string villager_confression_text;
		public string sister_empathize_text;
		public int sister_empathize_expression;
		public int empathize_point_type;
		public string sister_admonish_text;
		public int sister_admonish_expression;
		public int admonish_point_type;
		public string villager_empathize_text;
		public int villager_empathize_manga_mark;
		public string villager_admonish_text;
		public int villager_admonish_manga_mark;
	}
}