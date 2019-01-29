Shader "Unlit/Differential Rendering"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MainTex2 ("Texture", 2D) = "white" {}
		_MainTex3 ("Texture", 2D) = "white" {}
		_MainTex4 ("Texture", 2D) = "white" {}
		Wireframe ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _MainTex2;
			sampler2D _MainTex3;
			sampler2D _MainTex4;
			sampler2D Wireframe;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 realScene = tex2D(_MainTex, i.uv);
				fixed4 virtualScene = tex2D(_MainTex2, float2(i.uv.x, 1 - i.uv.y));
				fixed4 virtualObj = tex2D(_MainTex3, float2(i.uv.x, 1 - i.uv.y));
				fixed4 localScene = tex2D(_MainTex4, float2(i.uv.x, 1 - i.uv.y)); //virtual object depth
				fixed4 wireframe = tex2D(Wireframe, float2(i.uv.x, 1 - i.uv.y));

				fixed4 occlusion = fixed4(1, 1, 1, 1);
				fixed4 mask;
				if (virtualObj.r != 0 || virtualObj.g != 0 || virtualObj.b != 0) mask = fixed4(1, 1, 1, 1);
				else mask = fixed4(0, 0, 0, 0);
				fixed4 shadow;
				if (virtualScene.r - localScene.r != 0   && virtualScene.g - localScene.g != 0  && virtualScene.b - localScene.b != 0 && mask.r != 1) shadow = fixed4(1, 1, 1, 1);
				else shadow = fixed4(0, 0, 0, 0);
				//compute differnece
				fixed4 dif = virtualScene - localScene;
				
				return (realScene + dif) * (1 - mask) + virtualScene * mask + wireframe;
				
			}
			ENDCG
		}
	}
}
