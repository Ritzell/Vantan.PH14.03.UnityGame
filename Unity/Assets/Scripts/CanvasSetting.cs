using UnityEngine;
using System.Collections;

public class CanvasSetting : MonoBehaviour {
    private static GameObject canvas;
    [SerializeField]
    private GameObject Reticle;
	// Use this for initialization
	void Start () {
        canvas = gameObject;
        ChangeRenderMode(VRMode.isVRMode);
	}

    void ChangeRenderMode(bool isVR)
    {
        canvas.transform.parent = isVR ? GameManager.FirstParent(Airframe.AirFrame).transform : null;
        canvas.GetComponent<Canvas>().renderMode = isVR ? RenderMode.WorldSpace : RenderMode.ScreenSpaceOverlay;
        //GameObject DamageEffect = canvas.transform.FindChild("DamageEffectImage").gameObject;
        if (isVR)
        {
            GameObject VRCamera = FindObjectOfType<InputVRController>().Controllers[(int)HandType.Right].transform.FindChild("HandCamera").gameObject;
            //canvas.GetComponent<Canvas>().worldCamera = FindObjectOfType<CameraSetting>().MyCamera.GetComponent<Camera>();
            //canvas.GetComponent<Canvas>().planeDistance = 10f;
            canvas.transform.localPosition = new Vector3(0, 16, 74);
            canvas.transform.localScale = new Vector3(0.08f, 0.08f, 1);
            Reticle.GetComponent<ReticleSystem>().MainCamera = VRCamera.GetComponent<Camera>();
            Reticle.transform.parent = VRCamera.transform.FindChild("Canvas").transform;
            Reticle.transform.localPosition = Vector3.zero;
            Reticle.transform.localRotation = new Quaternion(0, 0, 0, 0);
            Reticle.transform.localScale = new Vector3(0.0001f, 0.0001f, 1);
        } else
        {
            Reticle.GetComponent<ReticleSystem>().MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
}
