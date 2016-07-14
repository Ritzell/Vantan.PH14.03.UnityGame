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

public class GameSetting : MonoBehaviour {
	//valueで選択肢を得る
	//QualitySettings.antiAliasing = 0;
	private static bool RunOutPut = false;

	void Start(){
		if (!RunOutPut) {
			GameObject.Find("Main Camera").GetComponent<CameraSystem>().StartCoroutine (OutPut());
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
		HDR,
		RenderingPath,
		GlassDrow,
		TreeDrow,
		TerrainPixelError
	}

	private static IEnumerator OutPut(){
		while (true) {
			if (Input.GetKeyDown (KeyCode.M)) {
				foreach (int i in Dates) {
					Debug.Log (i);
				}
			}
			yield return null;
		}
	}

	static int displayObjectsTest = 0;

	private static IEnumerator SettingActive(){
		Camera camera = FindObjectOfType<CameraSystem> ().GetComponent<Camera> ();
		while (true) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				QualitySettings.antiAliasing = Dates [(int)DateNumber.AntiAliasing];
				camera.farClipPlane = Dates [(int)DateNumber.CameraFar];
				camera.hdr = Dates [(int)DateNumber.CameraFar] == 1 ? true : false;

				switch (Dates [(int)DateNumber.RenderingPath]) {
				case 0:
					camera.renderingPath = UnityEngine.RenderingPath.VertexLit;
					break;
				case 1:
					camera.renderingPath = UnityEngine.RenderingPath.Forward;
					break;
				case 2:
					camera.renderingPath = UnityEngine.RenderingPath.DeferredLighting;
					break;
				}
					
				displayObjectsTest = Dates [(int)DateNumber.DisplayObjects];
//				QualityLevel.
//				QualitySettings.
				yield return null;
			}
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

	public void HDR(int result){
		Dates [(int)DateNumber.HDR] = result;
		Debug.Log (result);
	}
}
