using UnityEngine;
using System.Collections;

public class CanvasSetting : MonoBehaviour {
    private static GameObject canvas;
	// Use this for initialization
	void Start () {
        canvas = gameObject;
        ChangeRenderMode(VRMode.isVRMode);
	}

    void ChangeRenderMode(bool isVR)
    {
        canvas.transform.parent = isVR ? GameManager.FirstParent(Airframe.AirFrame).transform : null;
        canvas.GetComponent<Canvas>().renderMode = isVR ? RenderMode.WorldSpace : RenderMode.ScreenSpaceOverlay;
        if (isVR)
        {
            canvas.transform.localPosition = new Vector3(0, 16, 74);
            canvas.transform.localScale = new Vector3(0.08f, 0.08f, 1);
        }
    }
}
