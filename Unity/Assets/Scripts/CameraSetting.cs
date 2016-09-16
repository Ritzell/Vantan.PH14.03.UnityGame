using UnityEngine;
using System.Collections;

public class CameraSetting : MonoBehaviour {
    [SerializeField]
    private GameObject Mycamera, VRCamera, NormalCamera;

    public GameObject MyCamera {
        get {
            return Mycamera;
        }
    }

    public GameObject VRcamera
    {
        get
        {
            return VRCamera;
        }
    }

    public GameObject MyCameraParent
    {
        get
        {
            return FirstParent(MyCamera);
        }
    }

    GameObject FirstParent(GameObject child)
    {
        return child.transform.parent == null ? child : FirstParent(child.transform.parent.gameObject);
    }

	private static AudioClip TitleClip;


    public void ChangeCamera(bool isVR)
    {

        Mycamera = isVR ? VRCamera : NormalCamera;
    }

	public void DestroyCamera (){
		Destroy (Mycamera);
	}

	public void OnScene(Scenes scene){
		Camera camera;
		try{
        camera = Mycamera.GetComponent<Camera>();
		}catch{
			Mycamera = FindObjectOfType<Camera> ().gameObject;
			camera = Mycamera.GetComponent<Camera>();
		}

		if (scene == Scenes.Title) {
			TitleClip = (AudioClip)Resources.Load ("Sounds/The Nutcracker - Valse des fleurs");
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.fieldOfView = 60;
			camera.farClipPlane = 1000;
			camera.hdr = true;
			Mycamera.transform.parent = null;
			Mycamera.transform.position = new Vector3 (0, 0,  VRMode.isVRMode ? 0 : -39.5f);
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
            GameObject Parent = MyCameraParent.transform.gameObject;
			Parent.transform.parent = GameObject.Find ("AirPlain").transform;
			Mycamera.GetComponent<AudioSource> ().Stop ();
            Parent.transform.localPosition = VRMode.isVRMode ? new Vector3(-0.5f, 5.4f, -10) : new Vector3 (0, 15, -50);
            Parent.transform.localRotation = new Quaternion (0, 0, 0,0);
			if (MyCameraParent.GetComponent<ArsenalCamera> ()) {
				Destroy (Mycamera.GetComponent<ArsenalCamera> ());
			}

		}else if (scene == Scenes.Customize) {
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.fieldOfView = 60;
			camera.farClipPlane = 1000;
			camera.hdr = false;
            MyCameraParent.transform.parent = null;
            MyCameraParent.transform.localPosition = new Vector3 (2.4f, 4.4f, 23.7f);
            MyCameraParent.transform.localRotation = new Quaternion (0, -180, 0,0);
            MyCameraParent.AddComponent<ArsenalCamera> ().Fighter = GameObject.Find("F-14");
			Mycamera.GetComponent<AudioSource> ().Stop ();

		}else if (scene == Scenes.Result) {
			camera.clearFlags = CameraClearFlags.Skybox;
			camera.fieldOfView = 60;
			camera.farClipPlane = 1000;
			camera.hdr = true;
            MyCameraParent.transform.localRotation = new Quaternion (0, 0, 0,0);
            MyCameraParent.transform.parent = null;
            MyCameraParent.transform.localPosition = new Vector3 (293, 252, 0);
			if (MyCameraParent.GetComponent<ArsenalCamera> ()) {
				Destroy (Mycamera.GetComponent<ArsenalCamera> ());
			}
		}
	}
}
