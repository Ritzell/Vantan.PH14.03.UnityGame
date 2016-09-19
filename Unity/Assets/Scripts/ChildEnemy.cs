using UnityEngine;
using System.Collections;

public class ChildEnemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EnemyBase.Rest = EnemyBase.Rest + 1;
	}

    public IEnumerator Death(Enemy EnemyScript)
    {
        Airframe Frame = Airframe.AirFrame.GetComponent<Airframe>();
        EnemyBase.Rest = EnemyBase.Rest - 1;
        EnemyScript.CryBox.pitch = Random.Range(0.65f, 1.3f);
        EnemyScript.CryBox.Play();
        StopCoroutine(EnemyScript.Breth);
        EnemyScript.MyMaterial.EnableKeyword("_EMISSION");
        EnemyScript.MyMaterial.SetColor("_EmissionColor", Color.red);
        StartCoroutine(EnemyScript.StateChange(EnemyScript.CryBox));
        StartCoroutine(EnemyScript.ShakeBody());
        EnemyScript.CameraS.StartCoroutine(EnemyScript.CameraS.Flash(0.8f, true, 0.35f, gameObject));

        while (true)
        {
            if (EnemyScript.CryBox.isPlaying == false)
            {
                Frame.StartCoroutine(NotificationSystem.UpdateNotification(gameObject.name + "を撃破しました！"));
                Destroy(gameObject);
            }
            yield return null;
        }
    }
}
