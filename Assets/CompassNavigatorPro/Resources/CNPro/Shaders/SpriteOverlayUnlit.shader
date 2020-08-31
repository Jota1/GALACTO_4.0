Shader "CompassNavigatorPro/Sprite Overlay Unlit"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Overlay"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"DisableBatching"="LODFading"
		}
		Cull Off
		ZWrite Off
		ZTest Always
		
		CGPROGRAM
			#pragma surface surf Unlit vertex:vert nolightmap alpha

			struct SpriteData {
				float4 vertex		: POSITION;
				float3 normal		: NORMAL;
				float4 texcoord		: TEXCOORD0;
				half4 color			: COLOR;
			};
	
			sampler2D _MainTex;
			fixed4 _Color;

			struct Input {
				fixed4 color;
				half2 mainTexUV;
			};

        	half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) {
            	half4 c;
                c.rgb = s.Albedo * atten;
            	c.a = s.Alpha;
            	return c;
        	}
        
			void vert(inout SpriteData IN, out Input OUT) {
				UNITY_INITIALIZE_OUTPUT(Input, OUT);
				OUT.mainTexUV = IN.texcoord.xy;
				OUT.color = IN.color;
			}

			void surf(Input IN, inout SurfaceOutput OUT)
			{
				half4 diffuseColor = tex2D(_MainTex, IN.mainTexUV);
				half alpha = diffuseColor.a * IN.color.a;
				OUT.Alpha = alpha;
				OUT.Gloss = diffuseColor.a;
				OUT.Albedo = diffuseColor.rgb * alpha;
			}
		ENDCG
		
	}

	FallBack "Transparent/Cutout/VertexLit"
}
