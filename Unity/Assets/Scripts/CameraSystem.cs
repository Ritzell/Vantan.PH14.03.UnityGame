using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour
{
	private static GameObject MyCamera;

	private static float NormalZ;//-26
	private  static float NormalY;//7.5
	private static float MaxNormalZ_Error;
	private static float MinNormalZ_Error;
	private static Vector3 LookBehindPos;
	private static Vector3 LookFrontPos;
	private static GameObject AirPlain;
	private static bool LookBehind = false;


	private static bool freemove = false;
	public static bool FreeMove{
		set{
			if (value == false) {
				MyCamera.transform.localPosition = LookFrontPos;
				MyCamera.transform.localRotation = new Quaternion(0,0,0,MyCamera.transform.localRotation.w);
			}
			Color color = ReticleSystem.UIImage.color;
			ReticleSystem.UIImage.color = new Vector4(color.r,color.g,color.b,value ? 0 : 1);
			//GameObject.Find ("ReticleImage").SetActive (!value);
			freemove = value;
		}get{
			return freemove;
		}
	}

	private static bool stopReset = false;
	public static bool StopReset {
		set {
			stopReset = value;
		}
		get {
			return stopReset;
		}
	}

	void Awake(){
		MyCamera = GameObject.Find ("Main Camera");
		AirPlain = GameObject.Find ("eurofighter");
		NormalZ = GameObject.Find ("Main Camera").transform.localPosition.z;
		NormalY = GameObject.Find ("Main Camera").transform.localPosition.y;
		MaxNormalZ_Error = NormalZ + 0.1f;
		MinNormalZ_Error = NormalZ - 0.1f;
	}

	void Start ()
	{
		StartCoroutine (CameraSwitching ());
		StartCoroutine (CameraModeChange ());
		LookBehindPos = new Vector3 (0, 5.8f, 30.5f);//GameObject.Find ("CameraPos1").transform.localPosition;
		LookFrontPos = MyCamera.transform.localPosition;
	}

	public IEnumerator LookTgt(GameObject Tgt){
		Vector3 TgtPos = Tgt.transform.position;
		while(!GameManager.GameOver){
			transform.LookAt (TgtPos);
			yield return null;
		}
		yield return null;
	}

	private IEnumerator CameraModeChange ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.JoystickButton6)) {
				FreeMove = !FreeMove;
				StartCoroutine (CameraFreeMove());
			}
			yield return null;
		}
	}

	private IEnumerator CameraFreeMove(){
		while (!GameManager.GameOver && freemove) {
			CameraMove (Input.GetAxis("3thAxis"),Input.GetAxis("4thAxis"));
			yield return null;
		}
	}

	private void CameraMove(float MoveX,float MoveY){
		transform.RotateAround (AirPlain.transform.position, Vector3.up, MoveX*2);
		transform.RotateAround (AirPlain.transform.position, Vector3.left, MoveY*2);
	}

	private static IEnumerator CameraSwitching ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.JoystickButton11) || Input.GetKeyDown(KeyCode.M)) {
				if(FreeMove){
					FreeMove = false;
					yield return null;
					continue;
				}
				MyCamera.transform.localPosition = ChangePos (MyCamera.transform.localPosition);
				MyCamera.transform.Rotate (MyCamera.transform.rotation.x - 5, 180, 0);
			}
			yield return null;
		}
	}

	private static Vector3 ChangePos (Vector3 NowPos)
	{
		if (NowPos.z <= NormalZ+7 && NowPos.x >= NormalZ-7) {
			LookBehind = true;
			return LookBehindPos;
		} else {
			LookBehind = false;
			return LookFrontPos;
		}
	}

	public static IEnumerator CameraPosReset ()
	{
		if(LookBehind){
			yield break;
		}
		float dis = MyCamera.transform.localPosition.z - (NormalZ);
		while ((MyCamera.transform.localPosition.z >= MaxNormalZ_Error || MyCamera.transform.localPosition.z <= MinNormalZ_Error) && !stopReset) {
			MyCamera.transform.Translate (0, 0, -0.05f * System.Math.Sign (dis));
			yield return null;
		}
		if (MyCamera.transform.localPosition.z <= MaxNormalZ_Error && MyCamera.transform.localPosition.z >= MinNormalZ_Error) {
			MyCamera.transform.localPosition = new Vector3 (0, NormalY, NormalZ);
		}
		yield return null;
	}

	public static IEnumerator SwayCamera(){
		float SwayTime = 0;
		float AmplitudeTime = 0f;
		Vector3 NormalPos = MyCamera.transform.localPosition;
		while (SwayTime < 0.4f) {
			SwayTime += Time.deltaTime;
			AmplitudeTime += Time.deltaTime;
			if (AmplitudeTime > 0.01f) {
				Vector3 Amplitude = new Vector3 (Random.Range(-5,5),Random.Range(-5,5),0);
				MyCamera.transform.localPosition = new Vector3 (NormalPos.x+Amplitude.x,NormalPos.y+Amplitude.y,NormalPos.z);
				AmplitudeTime = 0f;
				yield return null;
			}

			yield return null;
		}
		MyCamera.transform.localPosition = NormalPos;
		yield return null;
	}

	public static void MoveCamera (float value)
	{
		if (freemove || LookBehind) {
			return;
		}
		Vector3 CameraPos = MyCamera.transform.localPosition;
		Vector3 BehindPos = GameObject.Find ("CameraPos1").transform.localPosition;
		MyCamera.transform.localPosition = new Vector3(CameraPos.x,CameraPos.y,Mathf.Clamp(CameraPos.z + -0.05f * (value / (value / value)),NormalZ-7,NormalZ+7));
	}

}
