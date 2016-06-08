using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
	private static float speed = 300f;
	private const short Accele = +1;
	private const short Decele = -1;
	private const float MinSpeed = 200f;
	private const float MaxSpeed = 690f;
	private const float Keep = 0;

	private static ParticleSystem Burner;
	private static ParticleSystem Glow;
	private static ParticleSystem.EmissionModule em;
	private static ParticleSystem.MinMaxCurve rate;

	private static Sprite[] SpeedLines;

	private static EngineSound EngineS;

	public static float Speed{
		get{
			return speed;
		}
	}

	void Awake(){
		SpeedLines = LoadSprite ("speedLineAll");
		Glow = GameObject.Find ("Glow").GetComponent<ParticleSystem> ();
		Burner = GameObject.Find ("Afterburner").GetComponent<ParticleSystem> ();
		em = Glow.emission;
		rate = Glow.emission.rate;
		EngineS = GameObject.Find ("engine").GetComponent<EngineSound> ();
	}

	void Start ()
	{
		StartCoroutine (Move ());
		StartCoroutine (ChangeSpeed ());
	}

	private static Sprite[] LoadSprite(string spriteName){
		return Resources.LoadAll<Sprite>("Images/" + spriteName);
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

	private void HighPower ()//ParticleSystem Burner, ParticleSystem Glow, ParticleSystem.EmissionModule em, ParticleSystem.MinMaxCurve rate)
	{
		Burner.startSpeed = 25;
		Glow.startSpeed = 25;
		rate.constantMax = 450f;
		em.rate = rate;
	}

	private void LowPower ()//ParticleSystem Burner, ParticleSystem Glow, ParticleSystem.EmissionModule em, ParticleSystem.MinMaxCurve rate)
	{
		Burner.startSpeed = 4;
		Glow.startSpeed = 4;
		rate.constantMax = 100f;
		em.rate = rate;
	}

	private void Rotation(Vector3 AddRot) {
		transform.Rotate (AddRot.x / 1.5f, 0f, AddRot.z * 2f);
	}
	private Vector3 InputController(){
		return new Vector3 (Input.GetAxis ("Vertical") * 3, 0, Input.GetAxis ("Horizontal") * 2);
	}
}
