using UnityEngine;
using System.Collections;

public class CameraSetting : MonoBehaviour {
	private static GameObject Mycamera;
	private static AudioClip TitleClip;


	public void OnScene(Scenes scene){
		Mycamera = GameObject.Find ("Main Camera");

		Camera camera  = FindObjectOfType<Camera> ();

		if (scene == Scenes.Title) {
			TitleClip = (AudioClip)Resources.Load ("Sounds/The Nutcracker - Valse des fleurs");
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.fieldOfView = 60;
			camera.farClipPlane = 1000;
			camera.hdr = true;
			Mycamera.transform.parent = null;
			Mycamera.transform.position = new Vector3 (0, 0, -39.5f);
			Mycamera.transform.localRotation = new Quaternion (0, 0, 0,0);
			Mycamera.GetComponent<AudioSource> ().clip = TitleClip;
			Mycamera.GetComponent<AudioSource> ().Play();
			if (Mycamera.GetComponent<ArsenalCamera> ()) {
				Destroy (Mycamera.GetComponent<ArsenalCamera> ());
			}

		}else if (scene == Scenes.Stage) {
			camera.clearFlags = CameraClearFlags.Skybox;
			camera.fieldOfView = 75;
			camera.farClipPlane = 100000;
			camera.hdr = true;
			Mycamera.transform.parent = GameObject.Find ("AirPlain").transform;
			Mycamera.GetComponent<AudioSource> ().Stop ();
			Mycamera.transform.localPosition = new Vector3 (0, 15, -50);
			Mycamera.transform.localRotation = new Quaternion (0, 0, 0,0);
			if (Mycamera.GetComponent<ArsenalCamera> ()) {
				Destroy (Mycamera.GetComponent<ArsenalCamera> ());
			}

		}else if (scene == Scenes.Customize) {
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.fieldOfView = 60;
			camera.farClipPlane = 1000;
			camera.hdr = false;
			Mycamera.transform.parent = null;
			Mycamera.transform.localPosition = new Vector3 (2.4f, 4.4f, 23.7f);
			Mycamera.transform.localRotation = new Quaternion (0, -180, 0,0);
			Mycamera.AddComponent<ArsenalCamera> ().Fighter = GameObject.Find("F-14");
			Mycamera.GetComponent<AudioSource> ().Stop ();

		}else if (scene == Scenes.Result) {
			camera.clearFlags = CameraClearFlags.Skybox;
			camera.fieldOfView = 60;
			camera.farClipPlane = 1000;
			camera.hdr = true;
			Mycamera.transform.localRotation = new Quaternion (0, 0, 0,0);
			Mycamera.transform.parent = null;
			Mycamera.transform.localPosition = new Vector3 (293, 252, 0);
			if (Mycamera.GetComponent<ArsenalCamera> ()) {
				Destroy (Mycamera.GetComponent<ArsenalCamera> ());
			}
		}
	}
}
