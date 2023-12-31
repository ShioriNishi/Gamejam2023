using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class confression_master_importer : AssetPostprocessor
{
	private static readonly string filePath = "Assets/Terasurware/Excel/confression_master.xlsx";
	private static readonly string[] sheetNames = { "confression_master", };
	
	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets)
		{
			if (!filePath.Equals(asset))
				continue;

			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}

				foreach (string sheetName in sheetNames)
				{
					var exportPath = "Assets/Databases/" + sheetName + ".asset";
					
					// check scriptable object
					var data = (Entity_confression_master)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_confression_master));
					if (data == null)
					{
						data = ScriptableObject.CreateInstance<Entity_confression_master>();
						AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
						data.hideFlags = HideFlags.NotEditable;
					}
					data.param.Clear();

					// check sheet
					var sheet = book.GetSheet(sheetName);
					if (sheet == null)
					{
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					// add infomation
					for (int i=1; i<= sheet.LastRowNum; i++)
					{
						IRow row = sheet.GetRow(i);
						ICell cell = null;
						
						var p = new Entity_confression_master.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.villager_confression_text = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.sister_empathize_text = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.sister_empathize_expression = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.empathize_point_type = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.sister_admonish_text = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.sister_admonish_expression = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.admonish_point_type = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.villager_empathize_text = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.villager_empathize_manga_mark = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.villager_admonish_text = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.villager_admonish_manga_mark = (int)(cell == null ? 0 : cell.NumericCellValue);

						data.param.Add(p);
					}
					
					// save scriptable object
					ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
					EditorUtility.SetDirty(obj);
				}
			}

		}
	}
}
