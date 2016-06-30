using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;

public class ImageCamera : MonoBehaviour {
	[SerializeField]
	private RenderTexture target;
	[SerializeField]
	private Image newSprite;

	private Camera RenderCamera;

	private static Texture2D _resultTexture;
	public static Texture2D ResultTexture{
		set{
			_resultTexture = value;
		}get{
			return _resultTexture;
		}
	}

	void Awake(){
		RenderCamera = gameObject.GetComponent<Camera> ();
		//DontDestroyOnLoad (target);
		//DontDestroyOnLoad (ResultTexture);
		//DontDestroyOnLoad (gameObject);
	}

	void Start(){
		StartCoroutine (Capture ());
	}

	private IEnumerator Capture(){
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				yield return StartCoroutine(CaptureResultImage ());
			}
			yield return null;
		}
	}

	private static IntPtr ptr;
	public static IntPtr Ptr{
		set{
			ptr = value;
		}get{
			return ptr;
		}
	}
		
	public IEnumerator CaptureResultImage(){
		RenderTexture.active = target;
		RenderCamera.enabled = true;
		yield return new WaitForEndOfFrame ();
		ResultTexture = new Texture2D (1024, 1024, TextureFormat.ARGB32, false);
		//tex2d.ReadPixels (new Rect(0, 0, target.width, target.height), 0, 0);
		//tex2d.Apply ();
		//ResultTexture = tex2d;
		ResultTexture.Apply();
//		Ptr = target.GetNativeTexturePtr();
		ResultTexture.UpdateExternalTexture (target.GetNativeTexturePtr());
		ResultTexture.Apply ();
		newSprite.sprite = Sprite.Create(ResultTexture,new Rect(0, 0, 1024, 1024),Vector2.zero);
		GameManager.backGroundSprite = Sprite.Create(ResultTexture,new Rect(0, 0, 1024, 1024),Vector2.zero);
//		Texture2D externalTexture = Texture2D.CreateExternalTexture (1024, 1024, TextureFormat.ARGB32, false, false, ptr);
//		Ptr = externalTexture.GetNativeTexturePtr ();
//		tex2d.UpdateExternalTexture (externalTexture.GetNativeTexturePtr ());
//		tex2d.Apply();
		RenderTexture.active = null;
//		target.Release ();
		RenderCamera.enabled = false;
//		byte[] pngData = ResultTexture.EncodeToPNG ();
//		File.WriteAllBytes (Application.dataPath + "/../tmp.png", pngData);
		yield return null;
	}
}
