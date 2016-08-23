using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ImageCamera : MonoBehaviour {
	[SerializeField]
	private List<Transform> CapturePositions = new List<Transform>();

	private static Camera _renderCamera;
    
    private static RenderTexture _target;
	private static Texture2D _outputTexture;
	public static Texture2D OutPutTexture2D{
		get{
			return _outputTexture;
		}
	}
    private static string _path;
	public static string ImagePath{
		set{
			_path = value;
		}get{
			return _path;
		}
	}

    void Awake()
    {
        _renderCamera = gameObject.GetComponent<Camera>();
        _target = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        _path = Path.Combine(Path.GetTempPath(), "Captured.png");
		Debug.Log (_path);
    }
		
	public static void SettingPosition(Vector3 originPos,Vector3 otherPos){
		Vector3[] Positions = CapturePos( MidPoints(originPos,otherPos));
		_renderCamera.transform.position = Positions[0];
		SettingRotation (MidPoints(originPos,Positions[1]));
	}

	private static void SettingRotation(Vector3 LookAtPosition){
		_renderCamera.transform.LookAt (LookAtPosition);
		_renderCamera.transform.rotation = new Quaternion (_renderCamera.transform.rotation.x, _renderCamera.transform.rotation.y, 0, _renderCamera.transform.rotation.w);
	}

	private static Vector3 MidPoints(Vector3 originPos,Vector3 otherPos){
		return new Vector3 ((originPos.x + otherPos.x) / 2, (originPos.y + otherPos.y) / 2, (originPos.z + otherPos.z) / 2);
	}

	private static Vector3[] CapturePos(Vector3 CenterPos){
		Vector3 CameraPos = Vector3.zero;
		float Min = 360;
		foreach(Transform CapturePosition in FindObjectOfType<ImageCamera>().CapturePositions){
			float[] angle = GetAim (CenterPos, CapturePosition.position);
			if(90f-Mathf.Abs(angle[0]+angle[1]) < Min){
				Min = 90f - Mathf.Abs(angle [0] + angle [1]);
				CameraPos = CapturePosition.position;
			}
		}
		return new Vector3[]{CameraPos,CenterPos};
	}

	private static float[] GetAim(Vector3 p1, Vector3 p2) {
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		float rad = Mathf.Atan2(dy, dx);
		float dxTop = p2.x - p1.x;
		float dz = p2.z - p1.z;
		float radTop = Mathf.Atan2(dz, dxTop);
		return new float[]{Mathf.Abs(rad * Mathf.Rad2Deg),Mathf.Abs(radTop * Mathf.Rad2Deg)};
	}

	//void?
	public static IEnumerator CaptureResultImage(Vector3 player,Vector3 missile){
		SettingPosition (player, missile);
		RenderTexture.active = _target;
		_renderCamera.enabled = true;
        _renderCamera.targetTexture = _target;
        _renderCamera.Render();
        _outputTexture = new Texture2D(_target.width, _target.height, TextureFormat.ARGB32, false);
        _outputTexture.ReadPixels(new Rect(0, 0, _target.width, _target.height), 0, 0);
        _outputTexture.Apply();
//		Debug.Log ("写真を撮った");
//		try{
//        File.WriteAllBytes(_path, _outputTexture.EncodeToPNG());
//			Debug.Log("写真の保存に成功");
//			Debug.Log(ImagePath);
//		}catch{
//			Debug.Log("写真の保存に失敗");
//		}
		_renderCamera.enabled = false;
		yield return null;
	}
}
