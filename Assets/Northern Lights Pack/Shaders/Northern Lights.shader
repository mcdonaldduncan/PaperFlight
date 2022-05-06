// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Aurora Pack/Aurora" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_Noise ("Noise maps", 2D) = "white" {}
	_DistortSpeed ("Distort Speed", range(0, 0.3)) = 0.2
	_DistortScale ("Distort Scale", range(0, 0.009)) = 0.001
	_WidthScale ("Width Scale", range(0.1, 5)) = 2
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	//BlendOp Max
	Blend SrcAlpha One

	ColorMask RGB
	Cull Off Lighting Off ZWrite Off
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _Noise;
			fixed4 _TintColor;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 worldPos : TEXCOORD2;

			};

			float4 _MainTex_ST;
			float4 _Noise_ST;
			float _DistortSpeed;
			float _DistortScale;
			float _WidthScale;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.worldPos = v.vertex;

				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                float dotProduct = abs(dot(v.normal, viewDir));
                float rimPower = dotProduct;

				o.color = v.color * rimPower * 2;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}



			fixed4 frag (v2f i) : SV_Target
			{
				half2 detailCoords = half2((i.worldPos.x + i.worldPos.z) * _DistortScale*2 + _Time.x*_DistortSpeed, i.worldPos.y * _DistortScale + _Time.x*_DistortSpeed);
				float detail = tex2D (_Noise, detailCoords).r;

				float fade = saturate(2 - abs(i.texcoord.x - 0.5) * 4);//fade out edges of aurora  

				half2 texCoords = i.texcoord * float2(_WidthScale, 1) + float2(_Time.x*_DistortSpeed, 0);
				fixed4 col = _TintColor * tex2D(_MainTex, texCoords) * i.color * fade * detail.r;
				return col;
			}
			ENDCG 
		}
	}	
}
}
