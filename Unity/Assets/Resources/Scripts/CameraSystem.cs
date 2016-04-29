using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour {
	private static GameObject myCamera;
	public static bool stopReset = false;

	void Start () {
		myCamera = GameObject.Find ("Main Camera");
		StartCoroutine (cameraWork());
	}

	private static IEnumerator cameraWork()
	{
		while(!GameManager.GameOver){
			if(Input.GetKeyDown(KeyCode.JoystickButton11)||Input.GetKeyUp(KeyCode.JoystickButton11)){
				myCamera.transform.Rotate (0,180,0);
			}
			yield return null;
		}
	}

	public static IEnumerator cameraPosReset(){
		float dis = myCamera.transform.localPosition.z - (-14.5f);
		while ((myCamera.transform.localPosition.z >= -14.45f || myCamera.transform.localPosition.z <= -14.55f)&& !stopReset) 
		{
			myCamera.transform.Translate (0, 0, -0.05f * System.Math.Sign (dis));
			yield return null;
		}
		if (myCamera.transform.localPosition.z <= -14.45f && myCamera.transform.localPosition.z >= -14.55f) {
			myCamera.transform.localPosition = new Vector3 (-0.85f, 12.7f, -14.5f);
		}
		yield return null;
	}

	public static float moveCamera
	{
		set
		{
			if (PlayerMove.speed < speedConfig.maxSpeed && PlayerMove.speed > speedConfig.cruisingSpeed)
			{
				myCamera.transform.Translate(0, 0, -0.025f * (value / (value / value)));
			}

			if (myCamera.transform.localPosition.z < -16.5f)
			{
				myCamera.transform.localPosition = new Vector3(-0.85f, 12.7f, -16.5f);
			}
			else if (myCamera.transform.localPosition.z > -12)
			{
				myCamera.transform.localPosition = new Vector3(-0.85f, 12.7f, -12);
			}
		}
	}

}
