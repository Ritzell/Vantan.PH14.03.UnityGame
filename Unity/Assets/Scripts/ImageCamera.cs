using UnityEngine;
using System.Collections;

public class ImageCamera : MonoBehaviour {

	private static Texture2D _resultTexture;
	public static Texture2D ResultTexture{
		set{
			_resultTexture = value;
		}get{
			return _resultTexture;
		}
	}
		
	public IEnumerator CaptureResultImage(RenderTexture target,Camera RenderCamera){
		RenderTexture.active = target;
		RenderCamera.Render ();
		yield return new WaitForEndOfFrame ();
		var renderTex = new Texture2D (target.width, target.height, TextureFormat.ARGB32, false);
		renderTex.ReadPixels (new Rect(0, 0, target.width, target.height), 0, 0);
		renderTex.Apply();
		ResultTexture = renderTex;
		yield return null;
	}
}
