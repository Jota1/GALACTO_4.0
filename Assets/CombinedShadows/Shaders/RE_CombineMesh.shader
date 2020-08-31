// Upgrade NOTE: replaced 'defined SHADER_API_D3D9' with 'defined (SHADER_API_D3D9)'

// Upgrade NOTE: replaced 'defined SHADER_API_D3D11' with 'defined (SHADER_API_D3D11)'
// Upgrade NOTE: replaced 'defined SHADER_API_D3D11_9X' with 'defined (SHADER_API_D3D11_9X)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'


Shader "RE/CombineMesh" {
	Properties {
		_MainTex ("MainTex (RGB)", 2D) = "white" {}
		_Cutoff( "Alpha Cutoff", Range( 0,1 ) ) = 0.333
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry" "LightMode" = "ShadowCaster" }
		//Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200
		//Blend [_EnableBlend]
		//Blend SrcAlpha OneMinusSrcAlpha
		//Blend [_SrcBlend] [_DstBlend]
		Blend Off//One One
		//ZTest Off
		//ZWrite Off
		
		Pass 
		{
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
			#pragma multi_compile _ ENABLE_ALPHA_TEST

            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"

			struct VS_IN
			{
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float4 texcoord0 : TEXCOORD0;
				float  objectIndex : TEXCOORD1;
            };
			struct VS_OUT
			{
                float4 pos:SV_POSITION;
                float2 Texcoord : TEXCOORD0;
            };
            			
			uniform sampler2D _MainTex;
			uniform float _Cutoff;
			
			#if defined (SHADER_API_D3D11) || defined (SHADER_API_D3D11_9X)
				float4x4 _meshMatrices[ 1022 ];
			#elif defined (SHADER_API_VULKAN)
				float4x4 _meshMatrices[ 980 ];
			#elif defined (SHADER_API_D3D9)
				float4x4 _meshMatrices[ 60 ];
			#else//assuming GL
				float4x4 _meshMatrices[ 200 ];//limited by GL_MAX_VERTEX_UNIFORM_COMPONENTS
			#endif

			float3 ApplyShadowBias( float3 wPos, float3 normal )
			{
				if ( unity_LightShadowBias.z != 0.0 )
				{
					float3 wNormal = UnityObjectToWorldNormal( normal );
					float3 wLight = normalize( UnityWorldSpaceLightDir( wPos.xyz ) );

					// apply normal offset bias (inset position along the normal)
					// bias needs to be scaled by sine between normal and light direction
					// (http://the-witness.net/news/2013/09/shadow-mapping-summary-part-1/)
					//
					// unity_LightShadowBias.z contains user-specified normal offset amount
					// scaled by world space texel size.

					float shadowCos = dot( wNormal, wLight );
					float shadowSine = sqrt( 1 - shadowCos * shadowCos );
					float normalBias = unity_LightShadowBias.z * shadowSine;

					wPos.xyz -= wNormal * normalBias;
				}

				return wPos;
			}
            VS_OUT vert(VS_IN In)
            {
            	VS_OUT Out;
            	float3 LocalPos = In.vertex.xyz;
            	float2 Texcoord = float2( In.texcoord0.x, In.texcoord0.y );

            	float3 WorldPosition = mul( unity_ObjectToWorld, float4(LocalPos,1) );
				int index = (int)In.objectIndex;
				float3 ActualWorldPosition = mul( _meshMatrices[ index ], float4( WorldPosition, 1 ) );
            	
				ActualWorldPosition = ApplyShadowBias( ActualWorldPosition, In.normal );

            	Out.pos = mul (UNITY_MATRIX_VP, float4( ActualWorldPosition, 1) );

				Out.pos = UnityApplyLinearShadowBias( Out.pos );

            	Out.Texcoord = In.texcoord0;         	        	
                return Out;
            }
			float4 frag(VS_OUT In) : SV_Target
            {
				#ifdef ENABLE_ALPHA_TEST
					//TODO - texture could be sampled in VS for SM4+ for reduced texture samples
					float4 texel = tex2D( _MainTex, In.Texcoord );
					if ( texel.a < _Cutoff )
						discard;
				//_Cutoff
				#endif

				return float4(1,1,1,1);
            }

            ENDCG
        }
	} 
	FallBack "Diffuse"
}
