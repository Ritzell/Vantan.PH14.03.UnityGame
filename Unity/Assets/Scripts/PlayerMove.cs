using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
	[SerializeField]
	private ReticleSystem Reticle;
	[SerializeField]
	private Material SpeedLineMaterial;
	[SerializeField]
	private float SpeedLineThickness = 0.75f;
	[SerializeField]
	private   List<ParticleSystem> Burner  = new List<ParticleSystem>();
	[SerializeField]
	private  List<ParticleSystem> Glow = new List<ParticleSystem>();

	private static float speed = 300f;
	private static Transform AirFrame;
	private static Quaternion DefaltRotation;
	private const short Accele = +1;
	private const short Decele = -1;
	private const float MinSpeed = 200f;
	private const float MaxSpeed = 690f;
	private const float Keep = 0;
	private static List<ParticleSystem.EmissionModule> em = new List<ParticleSystem.EmissionModule>();
	private static List<ParticleSystem.MinMaxCurve> rate = new List<ParticleSystem.MinMaxCurve>();
	private static EngineSound EngineS;

	public static float Speed{
		get{
			return speed;
		}
	}
		
	void Awake(){
		Glow.ForEach(glow => em.Add(glow.emission));
		Glow.ForEach (glow => rate.Add (glow.emission.rate));
		EngineS = GameObject.FindObjectOfType<EngineSound> ();
		AirFrame = GameObject.Find ("eurofighter").transform;
		DefaltRotation = AirFrame.localRotation;
	}

	void Start(){
		SpeedLineMaterial.SetColor ("_Color", new Color (1, 1, 1, 0));
		FindObjectOfType<GameManager> ().StartStage ();
		FindObjectOfType<CameraSetting> ().OnScene (Scenes.Stage);
	}

	public void Manual ()
	{
		
		gameObject.GetComponent<Animator>().Stop ();
		CameraSystem cameraSystem = FindObjectOfType<CameraSystem> ();
		cameraSystem.StartCoroutine(CameraSystem.CameraChangePosition());
		cameraSystem.StartCoroutine(cameraSystem.CameraModeChange());
		cameraSystem.SetUp ();
		FindObjectOfType<EnemyBase> ().StartCoroutine (EnemyBase.PlayerInArea ());
		gameObject.GetComponent<Attack> ().EnableAttack ();
		Reticle.EnableReticle ();
		StartCoroutine (Move ());
		StartCoroutine (ChangeSpeed ());
		StartCoroutine (NotificationSystem.UpdateNotification ("操縦権を搭乗者に委託します"));
	}

	public IEnumerator FadeInSpeedLine(){
		float fadeSpeed = 0.1f;
		while (SpeedLineMaterial.GetColor ("_Color").a < (300/MaxSpeed)*SpeedLineThickness) {
			SpeedLineMaterial.SetColor ("_Color", new Color (1, 1, 1, SpeedLineMaterial.GetColor ("_Color").a + (Time.deltaTime*SpeedLineThickness*fadeSpeed)));
			yield return null;
		}
	}

	private IEnumerator Move ()
	{
		while (!GameManager.GameOver) {
			Rotation(InputController());
			MoveForward ();
			yield return null;
		}
	}

	private IEnumerator AutoMove(){
		
		while(true){
			MoveForward ();
			yield return null;
		}
	}

	private void MoveForward ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * Speed);
	}

	/// <summary>
	/// 機体の速度を加減する。
	/// </summary>
	/// <returns>The speed.</returns>
	private IEnumerator ChangeSpeed ()
	{
		while (!GameManager.GameOver) {
			if (isKeyDown()) {
				if (isAccele()) {
					FuelInjector(Accele);
				} else {
					FuelInjector(Decele);
				}
			} else if (isKeyUp()) {
				AfterBurner (Keep);
				StartCoroutine (CameraSystem.CameraPosReset ());
			}
			yield return null;
		}
	}

	private bool isKeyUp(){
		return Input.GetKeyUp (KeyCode.JoystickButton13) || Input.GetKeyUp (KeyCode.JoystickButton14) || Input.GetKeyUp (KeyCode.Alpha1) || Input.GetKeyUp (KeyCode.Alpha2);
	}

	private bool isKeyDown(){
		return Input.GetKey (KeyCode.JoystickButton13) || Input.GetKey (KeyCode.JoystickButton14) || Input.GetKey (KeyCode.Alpha1) || Input.GetKey (KeyCode.Alpha2);
	}

	private bool isAccele(){
		return Input.GetKey (KeyCode.JoystickButton14) || Input.GetKey (KeyCode.Alpha2);
	}

	/// <summary>
	/// 機体の速度に制限。
	/// 巡航速度1000Km 最高速度2484Km (speed*60*60=時速とする)
	/// </summary>
	/// <value>The speed.</value>
	private void FuelInjector (float Power){
		speed = Mathf.Clamp(Speed + Power,MinSpeed,MaxSpeed);
		SpeedLineMaterial.SetColor("_Color",new Color (1,1,1,(speed/MaxSpeed)*SpeedLineThickness));
			if (Speed > MinSpeed && Speed < MaxSpeed) {
				AfterBurner (Power);
				CameraSystem.MoveCamera (Power);
			}
			EngineS.Pitch = Speed;
	}

	private void AfterBurner (float Fuel)
	{

		if (Fuel > Keep) {
			HighPower ();
		} else {
			LowPower ();
		}
	}

	private void HighPower ()
	{
		foreach (ParticleSystem burner in Burner) {
			burner.startSpeed = 25;
			burner.startSize = 1.4f;
		}
		Glow.ForEach(glow => glow.startSpeed = 25);
		rate.ForEach(rt => rt.constantMax = 450);
		em.ForEach(e => e.rate = rate[0]);
	}

	private void LowPower ()
	{
		foreach (ParticleSystem burner in Burner) {
			burner.startSpeed = 4;
			burner.startSize = 0.7f;
		}
		Glow.ForEach(glow => glow.startSpeed = 4);
		rate.ForEach(rt => rt.constantMax = 100);
		em.ForEach(e => e.rate = rate[0]);
	}

	private void Rotation(Vector3 AddRot) {
		transform.Rotate (AddRot.x / 1.5f, 0f, AddRot.z * 2f);
		AirFrame.localRotation = new Quaternion (DefaltRotation.x + AddRot.x/50,DefaltRotation.y,DefaltRotation.z,DefaltRotation.w);
	}
	private Vector3 InputController(){
		return new Vector3 (Input.GetAxis ("Vertical") * 3, 0, Input.GetAxis ("Horizontal") * 2);
	}
}
