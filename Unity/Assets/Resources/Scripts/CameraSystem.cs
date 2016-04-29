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

	private const float normalZ = -26f;
	private const float normalY = 7.5f;

	public static IEnumerator cameraPosReset(){
		float dis = myCamera.transform.localPosition.z - (normalZ);
		while ((myCamera.transform.localPosition.z >= normalZ+0.1f || myCamera.transform.localPosition.z <= normalZ-0.1f)&& !stopReset) 
		{
			myCamera.transform.Translate (0, 0, -0.05f * System.Math.Sign (dis));
			yield return null;
		}
		if (myCamera.transform.localPosition.z <= normalZ+0.1f && myCamera.transform.localPosition.z >= normalZ-0.1f) {
			myCamera.transform.localPosition = new Vector3 (0, normalY, normalZ);
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

			if (myCamera.transform.localPosition.z < normalZ-4)
			{
				myCamera.transform.localPosition = new Vector3(0,normalY,normalZ-4);
			}
			else if (myCamera.transform.localPosition.z > normalZ+4)
			{
				myCamera.transform.localPosition = new Vector3(0, normalY, normalZ+4);
			}
		}
	}

}
