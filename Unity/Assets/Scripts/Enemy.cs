using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

	[SerializeField]
	private AudioSource CryBox;
	[SerializeField]
	private Material ArmorMaterial;
	[SerializeField]
	private GameManager Manager;
	private CameraSystem CameraS;
	private bool isLife = true;
	private float HP = 600;
	private float MaxHP;
	private Material MyMaterial;
	private Color MaterialColor;
	private Coroutine Breth;
	private IEnumerator StateNotice;

	/// <summary>
	/// フレアによる回避はミサイルのスクリプトで行う
	/// </summary>

	private static ReticleSystem PlayerReticle;


//	void Start(){
//		Manager = FindObjectOfType<GameManager> ();
//		StartCoroutine (DebugMove ());
//	}

//	private IEnumerator DebugMove(){
//		GameObject DebugPlayer = FindObjectOfType<DebugPlayer> ().gameObject;
//
//		float Area = 1000;
//		float MoveSpeed = 10;
//		float RotateSpeed = 5f;
//		while (true) {
//			if(Manager.AbsDistance(DebugPlayer.transform.position,transform.position) < Area){
//				Quaternion StartRot = transform.rotation;
//				transform.LookAt (DebugPlayer.transform);
//				Quaternion EndRot = transform.rotation;
//				transform.rotation = StartRot;
//
//
//
//				for (float time = 0; time < 1; time+=Time.deltaTime) {
//					Quaternion.Slerp (StartRot, EndRot, Manager.Veje(time,0,1,0.5f,1));
//					yield return null;
//				}
//				//Quaternion.Slerp(
////			if(true){
////				Debug.Log(Manager.GetDegree (DebugPlayer.transform.position.x, transform.position.x, DebugPlayer.transform.position.y, transform.position.y));
////				float angle = Manager.GetDegree (DebugPlayer.transform.position.z, transform.position.z, DebugPlayer.transform.position.x, transform.position.x);
////				transform.LookAt (DebugPlayer.transform);
////				transform.Translate (0,0, -(MoveSpeed * Time.deltaTime));
////				transform.RotateAround (Vector3.zero, Vector3.up, RotateSpeed * Time.deltaTime);
//				if(false){
//				angle = Mathf.DeltaAngle (transform.eulerAngles.y, angle);
////				Debug.Log (angle);
//				//LookAt からのback方向に進行で良いプログラム
//				transform.Rotate (0, angle * Time.deltaTime, 0);
////				transform.rotation = Quaternion.Euler (0, angle, 0);
////				transform.eulerAngles = new Vector3(0f,Mathf.LerpAngle(transform.eulerAngles.y,angle < 0 ? angle - 100 : angle + 100,Time.deltaTime),0f);//				transform.Rotate (0, transform.rotation.y - (angle < 0 ? angle + 360 : angle)*RotateSpeed, 0);
//				//transform.eulerAngles = new Vector3(0,angle < 0 ? angle + 360 : angle,0);// = Quaternion.Euler(0,  transform.eulerAngles.y-Mathf.Abs(angle) , 0);
//				transform.Translate (Vector3.forward * (MoveSpeed * Time.deltaTime*-1));
////				transform.translate (new Vector3 (angle, 0, 0) * (MoveSpeed *Time.deltaTime),Space.Self);
//				}
//			}
//			yield return null;
//		}
//	}

	void Awake ()
	{
		PlayerReticle = GameObject.Find ("ReticleImage").GetComponent<ReticleSystem> ();

		MyMaterial = gameObject.GetComponent<Renderer> ().material;
		CameraS = GameObject.Find ("Main Camera").GetComponent<CameraSystem>();
		Manager = FindObjectOfType<GameManager> ();

	}

	void Start ()
	{
		MaxHP = HP;
		ArmorMaterial.color = new Color (0.3f, 0.3f, 0.3f, 1);
		MaterialColor = MyMaterial.GetColor ("_EmissionColor");
		EnemyBase.Rest = EnemyBase.Rest + 1;
		Breth = StartCoroutine (Respiration ());
		StateNotice = StateNotification();
		CryBox.pitch = 2.5f;
//		StartCoroutine (Move ());
	}
		

	void OnTriggerEnter (Collider Col)
	{
		if (!isLife) {
			return;
		}
		if (Col.gameObject.layer == (int)PlayerAttackPower.bulletLayer) {
			HP -= (int)PlayerAttackPower.bullet;
		} else if (Col.gameObject.layer == (int)PlayerAttackPower.missileLayer) {
			CryBox.Play ();
			HP -= (int)PlayerAttackPower.missile;
		}
		StateNotice.MoveNext ();
		DiedJudgment ();
	}
		
	private IEnumerator Move(){
		float Area = 4000;
		float MoveSpeed = 40;
		while (!GameManager.IsGameOver && Airframe.isLife) {
			if(Manager.AbsDistance(Airframe.AirFramePosition,gameObject.transform.position) < Area){
				float angle = Manager.GetDegree (Airframe.AirFramePosition.x, transform.position.x, Airframe.AirFramePosition.y, transform.position.y);
				transform.Translate (new Vector3 (angle, 0, 0) * (MoveSpeed *Time.deltaTime));
			}
			yield return null;
		}
	}
		
	private void DiedJudgment(){
		if (HP <= 0) {
			isLife = false;
			EnemyBase.Rest = EnemyBase.Rest - 1;
			PlayerReticle.DestoroyLockOnTgt (gameObject);
			StartCoroutine (Deth ());
		}
	}

	private IEnumerator StateNotification(){
		bool isPassing = false;
		while (!isPassing) {
			if (HP <= MaxHP/2) {
//				NotificationSystem.Announce = gameObject.name + "の体力が著しく消耗しています。";
				StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "の体力が著しく消耗しています。"));
				isPassing = true;
				yield return null;
			}
			yield return null;
		}
		isPassing = false;
		while (!isPassing) {
			if (HP <= MaxHP/4) {
				StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "が弱っています"));
				isPassing = true;
				yield return null;
			}
			yield return null;
		}
		isPassing = false;
		while (!isPassing) {
			if (HP <= MaxHP/8) {
				StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "がもう少しで撃破できます。"));
				isPassing = true;
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator Respiration ()
	{
		Material material;
		material = gameObject.GetComponent<Renderer> ().material;
		material.EnableKeyword ("_EMISSION");
		Color MaterialMaxColor = material.GetColor ("_EmissionColor");
		float Turning = (MaterialMaxColor.r + MaterialMaxColor.g + MaterialMaxColor.b) / 3;
		while (true) {

			while (0.05f < (material.GetColor ("_EmissionColor").r + material.GetColor ("_EmissionColor").g + material.GetColor ("_EmissionColor").b) / 3) {
				Brethes (material, MaterialMaxColor);
				yield return null;
			}
			while (Turning > (material.GetColor ("_EmissionColor").r + material.GetColor ("_EmissionColor").g + material.GetColor ("_EmissionColor").b) / 3) {
				Suck (material, MaterialMaxColor);
				yield return null;
			}
		}
	}

	private void Brethes(Material material,Color MaxColor){
		Color mColor = material.GetColor ("_EmissionColor");
		material.SetColor ("_EmissionColor", new Color (mColor.r - (MaxColor.r * (Time.deltaTime))
			, mColor.g - (MaxColor.g * (Time.deltaTime))
			, mColor.b - (MaxColor.b * (Time.deltaTime))));
	}

	private void Suck(Material material,Color MaxColor){
		Color mColor = material.GetColor ("_EmissionColor");
		material.SetColor ("_EmissionColor", new Color (mColor.r + (MaxColor.r * (Time.deltaTime))
			, mColor.g + (MaxColor.g * (Time.deltaTime))
			, mColor.b + (MaxColor.b * (Time.deltaTime))));
	}

	private IEnumerator Deth ()
	{
		Airframe Frame = Airframe.AirFrame.GetComponent<Airframe> ();

		CryBox.pitch = Random.Range (0.65f, 1.3f);
		CryBox.Play ();
		StopCoroutine (Breth);
		MyMaterial.EnableKeyword ("_EMISSION");
		MyMaterial.SetColor ("_EmissionColor",Color.red);
		StartCoroutine (StateChange (CryBox));
		StartCoroutine (ShakeBody());
		CameraS.StartCoroutine(CameraS.Flash(0.8f,true,0.35f,gameObject));

		while (true) {
			if (CryBox.isPlaying == false) {
//				NotificationSystem.Announce = gameObject.name + "を撃破しました！";
				Frame.StartCoroutine (NotificationSystem.UpdateNotification (gameObject.name + "を撃破しました！"));
				Destroy (gameObject);
			}
			yield return null;
		}
	}

	private IEnumerator StateChange(AudioSource AudioBox){
		var color = ArmorMaterial.color;
		while (AudioBox.isPlaying) {
			Expansion ();
			Hyperemia (color);
			yield return null;
		}
	}

	private IEnumerator ShakeBody(){
		Vector3 DefaultPos = transform.position;
		while (gameObject != null) {
			Vector3 RandomRange = new Vector3 (Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10));
			transform.position = new Vector3 (DefaultPos.x + RandomRange.x,DefaultPos.y + RandomRange.y,DefaultPos.z + RandomRange.z);
			yield return null;
		}
	}

	private void Expansion ()
	{
		transform.localScale = new Vector3 (transform.localScale.x + (800 * (Time.deltaTime / 3)), transform.localScale.y + (800 * (Time.deltaTime / 3)), transform.localScale.z + (800 * (Time.deltaTime / 3)));
	}

	private void Hyperemia (Color startColor)
	{
		ArmorMaterial.color = new Color (ArmorMaterial.color.r + ((0.5f - startColor.r) * Time.deltaTime / 2), ArmorMaterial.color.g + ((0 - startColor.g) * Time.deltaTime / 2), ArmorMaterial.color.b + ((0 - startColor.b) * Time.deltaTime / 2));
	}
}
