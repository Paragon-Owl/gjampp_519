// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effect/TexTransitions"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DoubleTex("Other Texture", 2D) = "white" {}
		_TransitionTex("Transition Texture", 2D) = "white" {}
		_Cutoff("Cutoff", Range(0, 1)) = 0
	}

		SubShader
		{
			// No culling or depth
			Cull On
			ZWrite On
			ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				float4 _MainTex_TexelSize;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;

					#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv1.y = 1 - o.uv1.y;
					#endif

					return o;
				}

				sampler2D _TransitionTex;
				sampler2D _MainTex;
				sampler2D _DoubleTex;
				float _Cutoff;

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 transit = tex2D(_TransitionTex, i.uv1);

					if (transit.r < _Cutoff)
						return tex2D(_DoubleTex, i.uv);

					return tex2D(_MainTex, i.uv);
				}
				ENDCG
			}
		}
}
