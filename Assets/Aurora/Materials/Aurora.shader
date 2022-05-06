Shader "Unlit/Aurora"
{
    Properties
    {
		_MainColor("Main Color", Color) = (1, 1, 1, 0.5)
		_MainTex("Main Texture", 2D) = "white" {}
		_Palette("Palette", 2D) = "white" {}

		_Twirl("Twirl", 2D) = "white" {}
		_TwirlScale("Twirl Scale", Range(0, 2)) = 1

		_Speed("Changes Speed", range(0, 1)) = 2
		_Flow("Flow Speed", range(0, 10)) = 3

		_Intensity("Intensity", Range(0,150)) = 30

		_Iterations("Iterations", Range(0,100)) = 30
		_IterationStep("Iteration Step", Range(0,0.02)) = 0.01
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			sampler2D _Twirl;
			float4 _Twirl_ST;
			half _TwirlScale;
			half _Intensity;
			half _Speed;
			float _Flow;
			sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _MainColor;
			sampler2D _Palette;
			float _Iterations;
			float _IterationStep;

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
				half3 viewDir: POSITION1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float Randomize(float x, float y)
			{
				return tex2D(_Twirl, float2(x, y)).x;
			}

			float GetNoise(float2 UV)
			{
				UV -= float2(0.5, 0.5);
				float time = _Speed * _Time.x;
				float2 uv = UV * _Twirl_ST;
				float2 uv2 = UV * _MainTex_ST;
				float x = UV + _TwirlScale * Randomize(uv.y + time * _Flow, time + uv.x / 2);
				uv2.x += x;

				float noise =  tex2D(_MainTex, (uv2 * 0.22 - fixed2(-time * 0.2, time * 0.1)) * 1 ).r;
				noise *= tex2D(_MainTex, (uv2 * 0.25 - fixed2(time * 0.12, -time * 0.3)) * 1.2).r;
				noise *= tex2D(_MainTex, (uv2 * 0.2 - fixed2(time * 0.2, time * 0.23)) * 1.1).r;

				noise = noise * _Intensity;

				const float _Size = 0.3;
				float len = length(UV);
				if (len > _Size) 
					noise *= 1 - (len - _Size) / (0.5 - _Size);

				noise = saturate(noise);

				return noise;
			}

			fixed4 frag (v2f i) : SV_Target
            {
				float bright = GetNoise(i.uv);

				float2 uv = i.uv;
				float amp = 0.3;
				float step = saturate(amp/ _Iterations);
				float2 d = - _IterationStep * float2(1, 1) * i.viewDir;

				for(float k = 0; k < _Iterations; k++)
				{
					uv += d;
					float n = GetNoise(uv);
					bright += amp * n;
					//
					amp -= step;
				}

				fixed4 color = tex2D(_Palette, _Time.x);

#ifdef UNITY_COLORSPACE_GAMMA
				if (bright > 1) bright = sqrt(bright);
				color *= _MainColor * bright;
#else
				color *= _MainColor * bright * 100;
#endif
                return color;
            }
            ENDCG
        }
    }
}
