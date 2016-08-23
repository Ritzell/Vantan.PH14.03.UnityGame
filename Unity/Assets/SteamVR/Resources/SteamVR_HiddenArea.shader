<<<<<<< HEAD
﻿Shader "Custom/SteamVR_HiddenArea" {

	CGINCLUDE

	#include "UnityCG.cginc"

	float4 vert(appdata_base v) : SV_POSITION { return v.vertex; }
	float4 frag(float4 v : SV_POSITION) : COLOR { return float4(0,0,0,0); }

	ENDCG

	SubShader {
		Tags { "Queue" = "Background" }
		Pass {
			ZTest Always Cull Off ZWrite On
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
=======
﻿Shader "Custom/SteamVR_HiddenArea" {

	CGINCLUDE

	#include "UnityCG.cginc"

	float4 vert(appdata_base v) : SV_POSITION { return v.vertex; }
	float4 frag(float4 v : SV_POSITION) : COLOR { return float4(0,0,0,0); }

	ENDCG

	SubShader {
		Tags { "Queue" = "Background" }
		Pass {
			ZTest Always Cull Off ZWrite On
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
>>>>>>> 7a2dac852c2189b0ba9d13ef8a6daa196fae8c91