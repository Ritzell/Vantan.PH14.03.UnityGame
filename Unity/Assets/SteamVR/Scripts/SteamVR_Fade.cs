using UnityEngine;
using Valve.VR;

public class SteamVR_Fade : MonoBehaviour
{
	private Color currentColor = new Color(0, 0, 0, 0);	// default starting color: black and fully transparent
	private Color targetColor = new Color(0, 0, 0, 0);	// default target color: black and fully transparent
	private Color deltaColor = new Color(0, 0, 0, 0);	// the delta-color is basically the "speed / second" at which the current color should change
	private bool fadeOverlay = false;

	static public void Start(Color newColor, float duration, bool fadeOverlay = false)
	{
		SteamVR_Utils.Event.Send("fade", newColor, duration, fadeOverlay);
	}

	static public void View(Color newColor, float duration)
	{
		var compositor = OpenVR.Compositor;
		if (compositor != null)
			compositor.FadeToColor(duration, newColor.r, newColor.g, newColor.b, newColor.a, false);
	}

#if TEST_FADE_VIEW
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SteamVR_Fade.View(Color.black, 0);
			SteamVR_Fade.View(Color.clear, 1);
		}
	}
#endif

	public void OnStartFade(params object[] args)
	{
		var newColor = (Color)args[0];
		var duration = (float)args[1];

		fadeOverlay = (args.Length > 2) && (bool)args[2];

		if (duration > 0.0f)
		{
			targetColor = newColor;
			deltaColor = (targetColor - currentColor) / duration;
		}
		else
		{
			currentColor = newColor;
		}
	}

	static Material fadeMaterial = null;
	static int fadeMaterialColorID = -1;

	void OnEnable()
	{
		if (fadeMaterial == null)
		{
			fadeMaterial = new Material(Shader.Find("Custom/SteamVR_Fade"));
			fadeMaterialColorID = Shader.PropertyToID("fadeColor");
		}

		SteamVR_Utils.Event.Listen("fade", OnStartFade);
		SteamVR_Utils.Event.Send("fade_ready");
	}

	void OnDisable()
	{
		SteamVR_Utils.Event.Remove("fade", OnStartFade);
	}

	void OnPostRender()
	{
		if (currentColor != targetColor)
		{
			// if the difference between the current alpha and the desired alpha is smaller than delta-alpha * deltaTime, then we're pretty much done fading:
			if (Mathf.Abs(currentColor.a - targetColor.a) < Mathf.Abs(deltaColor.a) * Time.deltaTime)
			{
				currentColor = targetColor;
				deltaColor = new Color(0, 0, 0, 0);
			}
			else
			{
				currentColor += deltaColor * Time.deltaTime;
			}

			if (fadeOverlay)
			{
				var overlay = SteamVR_Overlay.instance;
				if (overlay != null)
				{
					overlay.alpha = 1.0f - currentColor.a;
				}
			}
		}

		if (currentColor.a > 0 && fadeMaterial)
		{
			fadeMaterial.SetColor(fadeMaterialColorID, currentColor);
			fadeMaterial.SetPass(0);
			GL.Begin(GL.QUADS);

			GL.Vertex3(-1, -1, 0);
			GL.Vertex3( 1, -1, 0);
			GL.Vertex3(1, 1, 0);
			GL.Vertex3(-1, 1, 0);
			GL.End();
		}
	}
}