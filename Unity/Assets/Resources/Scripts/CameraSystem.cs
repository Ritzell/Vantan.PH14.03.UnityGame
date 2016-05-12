using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour {
	private static GameObject myCamera;
	public static bool StopReset = false;

	struct CameraLimitter{
		public const float NormalZ = -26f;
		public const float NormalY = 7.5f;
		public const float MaxNormalZ = NormalZ+4;
		public const float MinNormalZ = NormalZ-4;
		public const float MaxNormalZ_Error = NormalZ+0.1f;
		public const float MinNormalZ_Error = NormalZ-0.1f;
	}

	void Start () {
		myCamera = GameObject.Find ("Main Camera");
		StartCoroutine (CameraWork());
	}

	private static IEnumerator CameraWork()
	{
		while(!GameManager.GameOver){
			//myCamera.transform.rotation = new Quaternion(myCamera.transform.rotation.x,myCamera.transform.rotation.y,0,myCamera.transform.rotation.w);
			if(Input.GetKeyDown(KeyCode.JoystickButton11)||Input.GetKeyUp(KeyCode.JoystickButton11)){
				myCamera.transform.Rotate (0,180,0);
			}
			yield return null;
		}
	}

	public static IEnumerator CameraPosReset(){
		float dis = myCamera.transform.localPosition.z - (CameraLimitter.NormalZ);
		while ((myCamera.transform.localPosition.z >= CameraLimitter.MaxNormalZ_Error || myCamera.transform.localPosition.z <= CameraLimitter.MinNormalZ_Error)&& !StopReset) 
		{
			myCamera.transform.Translate (0, 0, -0.05f * System.Math.Sign (dis));
			yield return null;
		}
		if (myCamera.transform.localPosition.z <= CameraLimitter.MaxNormalZ_Error && myCamera.transform.localPosition.z >= CameraLimitter.MinNormalZ_Error) {
			myCamera.transform.localPosition = new Vector3 (0, CameraLimitter.NormalY, CameraLimitter.NormalZ);
		}
		yield return null;
	}

	public static float MoveCamera
	{
		set
		{
			if (PlayerMove.SpeedConfig.Speed < PlayerMove.SpeedConfig.MaxSpeed && PlayerMove.SpeedConfig.Speed > PlayerMove.SpeedConfig.MinSpeed)
			{
				myCamera.transform.Translate(0, 0, -0.025f * (value / (value / value)));
			}

			if (myCamera.transform.localPosition.z < CameraLimitter.MinNormalZ)
			{
				myCamera.transform.localPosition = new Vector3(0,CameraLimitter.NormalY,CameraLimitter.MinNormalZ);
			}
			else if (myCamera.transform.localPosition.z > CameraLimitter.MaxNormalZ)
			{
				myCamera.transform.localPosition = new Vector3(0, CameraLimitter.NormalY, CameraLimitter.MaxNormalZ);
			}
		}
	}

}
