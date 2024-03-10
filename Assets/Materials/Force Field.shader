Shader "Unlit/Force Field"
{
    Properties
    {
		_EdgeGlow("Edge Glow", Color) = (1,1,1,1) // Rim Glow
		_EdgeColour("Edge Colour", Color) = (1,1,1,1) // Rim Edge

		_FresnelPower("Fresnel Power", Range(0,5)) = 0.5
		_EdgeLength("Edge Length", Range(0,5)) = 0.5
		_EdgeFade("Edge Fade", Range(0,5)) = 0.5
    }

    SubShader
    {
		/* 1st Pass Start, Frasnel & Intersection */
        Pass
        {
			Lighting Off 
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
			#pragma target 3.0
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float4 normal: NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 fresnel : COLOR0;
				float4 screenPos : TEXCOORD1;
            };

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture; // Scene Depth

			float4 _MainTex_ST;
			float4 _EdgeGlow;
			float4 _EdgeColour;

			float _FresnelPower;
			float _EdgeLength; // Length of the edge detection
			float _EdgeFade;

			// VERTEX MAIN
            v2f vert (appdata v)
            {
                v2f o;

				// Fresnel
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				float fresnelDot = 1.0 - saturate(dot(v.normal, viewDir));
				o.fresnel = smoothstep(1.0 - _FresnelPower, 1.0, fresnelDot);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);

				COMPUTE_EYEDEPTH(o.screenPos.z);

                return o;
            }

			// FRAGMENT MAIN
            fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
            {
				// Intersection
				float depth = saturate(LinearEyeDepth(tex2Dproj(_CameraDepthTexture, i.screenPos)) - (i.screenPos.z + _EdgeLength));
				depth = 1.0 - pow(depth, _EdgeFade);

				// Fresnel
				i.fresnel.xyz *= step(0.5, facing);

				float alpha = saturate(depth + i.fresnel.r);

				return float4(lerp(_EdgeGlow.rgb, _EdgeColour.rgb, alpha), alpha);
            }
            ENDCG
        }
		/* 1st Pass End */
    }
}
