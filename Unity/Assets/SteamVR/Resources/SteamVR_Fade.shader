<<<<<<< HEAD
﻿Shader "Custom/SteamVR_Fade"
{
	SubShader
	{
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Always
			Cull Off
			ZWrite Off

			CGPROGRAM
				#pragma vertex MainVS
				#pragma fragment MainPS

				float4 fadeColor;

				float4 MainVS( float4 vertex : POSITION ) : SV_POSITION
				{
					return vertex.xyzw;
				}

				float4 MainPS() : SV_Target
				{
					return fadeColor.rgba;
				}
			ENDCG
		}
	}
=======
﻿Shader "Custom/SteamVR_Fade"
{
	SubShader
	{
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Always
			Cull Off
			ZWrite Off

			CGPROGRAM
				#pragma vertex MainVS
				#pragma fragment MainPS

				float4 fadeColor;

				float4 MainVS( float4 vertex : POSITION ) : SV_POSITION
				{
					return vertex.xyzw;
				}

				float4 MainPS() : SV_Target
				{
					return fadeColor.rgba;
				}
			ENDCG
		}
	}
>>>>>>> 7a2dac852c2189b0ba9d13ef8a6daa196fae8c91
}