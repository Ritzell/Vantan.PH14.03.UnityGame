using UnityEngine;
using System.Collections;
using System.IO;

public class ImageCamera : MonoBehaviour {

	private Camera _renderCamera;
    private Texture2D _outputTexture;
    private RenderTexture _target;
    private string _path;

    void Awake()
    {
        _renderCamera = gameObject.GetComponent<Camera>();
        _target = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        _path = Path.Combine(Path.GetTempPath(), "Captured.png");
    }

	void Start()
    {
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
		
	public IEnumerator CaptureResultImage(){
		RenderTexture.active = _target;
		_renderCamera.enabled = true;
        _renderCamera.targetTexture = _target;
        
        _renderCamera.Render();
        _outputTexture = new Texture2D(_target.width, _target.height, TextureFormat.ARGB32, false);
        _outputTexture.ReadPixels(new Rect(0, 0, _target.width, _target.height), 0, 0);
        _outputTexture.Apply();
        File.WriteAllBytes(_path, _outputTexture.EncodeToPNG());

        Debug.Log("Captured " + _path);

		_renderCamera.enabled = false;
		yield return null;
	}
}
