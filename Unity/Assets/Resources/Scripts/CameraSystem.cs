using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour {
	private static GameObject myCamera;
	public static bool stopReset = false;

	struct cameraLimiter{
		public const float normalZ = -26f;
		public const float normalY = 7.5f;
		public const float maxNormalZ = normalZ+4;
		public const float miniNormalZ = normalZ-4;
		public const float maxNormalZError = normalZ+0.1f;
		public const float miniNormalZError = normalZ-0.1f;
	}

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
		float dis = myCamera.transform.localPosition.z - (cameraLimiter.normalZ);
		while ((myCamera.transform.localPosition.z >= cameraLimiter.maxNormalZError || myCamera.transform.localPosition.z <= cameraLimiter.miniNormalZError)&& !stopReset) 
		{
			myCamera.transform.Translate (0, 0, -0.05f * System.Math.Sign (dis));
			yield return null;
		}
		if (myCamera.transform.localPosition.z <= cameraLimiter.maxNormalZError && myCamera.transform.localPosition.z >= cameraLimiter.miniNormalZError) {
			myCamera.transform.localPosition = new Vector3 (0, cameraLimiter.normalY, cameraLimiter.normalZ);
		}
		yield return null;
	}

	public static float moveCamera
	{
		set
		{
			if (PlayerMove.speedConfig.speed < PlayerMove.speedConfig.maxSpeed && PlayerMove.speedConfig.speed > PlayerMove.speedConfig.cruisingSpeed)
			{
				myCamera.transform.Translate(0, 0, -0.025f * (value / (value / value)));
			}

			if (myCamera.transform.localPosition.z < cameraLimiter.miniNormalZ)
			{
				myCamera.transform.localPosition = new Vector3(0,cameraLimiter.normalY,cameraLimiter.miniNormalZ);
			}
			else if (myCamera.transform.localPosition.z > cameraLimiter.maxNormalZ)
			{
				myCamera.transform.localPosition = new Vector3(0, cameraLimiter.normalY, cameraLimiter.maxNormalZ);
			}
		}
	}

}
