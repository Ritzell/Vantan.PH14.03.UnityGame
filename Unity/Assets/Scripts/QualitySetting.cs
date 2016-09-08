using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
//using System.Collections.Generic;

//public static class Test{
//	static void Start(){
//		GameObject.Find("Main Camera").GetComponent<CameraSystem>().StartCoroutine (DropdownCallback.OutPut());
//	}
//}

public class QualitySetting : MonoBehaviour {
	//valueで選択肢を得る
	//QualitySettings.antiAliasing = 0;
	private static bool RunOutPut = false;

	void Start(){
		if (!RunOutPut) {
			GameObject.Find("Main Camera").GetComponent<CameraSystem>().StartCoroutine (SettingActive());
			GameObject.Find("Main Camera").GetComponent<CameraSystem>().StartCoroutine (SettingOutPut());
			RunOutPut = true;

		}
	}
	private static int[] Dates = new int[Enum.GetNames(typeof(DateNumber)).Length];
	public static int[] GameSetDates{
		get{
			return Dates;
		}
	}

	public enum DateNumber
	{
		AntiAliasing = 0,
		Shadow,
		CameraFar,
		DisplayObjects,
		Vsync,
		HDR,
		RenderingPath,
		DrowTree,
		DrowGlass
//		GlassDrow,
//		TreeDrow,
//		TerrainPixelError
	}

	private static IEnumerator SettingOutPut(){
		while (true) {
			if (Input.GetKeyDown (KeyCode.M)) {
				foreach (int i in Dates) {
					Debug.Log (i);
				}
			}
			yield return null;
		}
	}

	private static IEnumerator SettingActive(){
		Camera camera = FindObjectOfType<CameraSystem> ().GetComponent<Camera> ();
		while (true) {
			if (Input.GetKeyDown (KeyCode.Space)) {
//				QualitySettings.SetQualityLevel (Dates [(int)DateNumber.RenderingPath]);//RenderingPath
				yield return new WaitForEndOfFrame();
				QualitySettings.antiAliasing = Dates [(int)DateNumber.AntiAliasing];//AntiAliasing
				QualitySettings.vSyncCount = Dates [(int)DateNumber.Vsync];//vsync
				foreach (Light light in FindObjectsOfType<Light>()) {//shadow
					switch(Dates[(int)DateNumber.Shadow]){
						case 0:
						light.shadows = LightShadows.None;
						break;
						case 1:
						light.shadows = LightShadows.Hard;
						break;
						case 2:
						light.shadows = LightShadows.Soft;
						break;
					}
				}
//				FindObjectOfType<Terrain> ().drawTreesAndFoliage = Dates [(int)DateNumber.DrowTree] == 0 ? true : false;//Draw
				camera.hdr = Dates [(int)DateNumber.HDR] == 0 ? true : false;//Draw

			}
			yield return null;
		}
	}

	public void AntiAliasing(int result)
	{
		Debug.Log (result);
		Dates [(int)DateNumber.AntiAliasing] = result;
	}

	public void Shadow(int result)
	{
		Debug.Log (result);
		Dates [(int)DateNumber.Shadow] = result;
	}

	public void CameraFar(int result){
		Dates [(int)DateNumber.CameraFar] = result;
		Debug.Log (result);
	}

	public void DisplayObjects(int result){
		Dates [(int)DateNumber.DisplayObjects] = result;
		Debug.Log (result);
	}

	public void RenderingPath(bool result){
		Dates [(int)DateNumber.RenderingPath] = result ? 1 : 0;
		Debug.Log (result);
	}

	public void Vsync(int result){
		Dates [(int)DateNumber.Vsync] = result;
		Debug.Log (result);
	}

	public void HDR(int result){
		Dates [(int)DateNumber.HDR] = result;
		Debug.Log (result);
	}

	public void RenderingPath(int result){
		Dates [(int)DateNumber.RenderingPath] = result;
		Debug.Log (result);
	}


	public void DrowTree(bool isDrow){
		Debug.Log (isDrow);
		Dates [(int)DateNumber.DrowTree] = isDrow ? 0 : 1;
	}

	public void DrowGlass(bool isDrow){
		Debug.Log (isDrow);
		Dates [(int)DateNumber.DrowGlass] = isDrow ? 0 : 1;
	}
}
