Shader "ForceField"
{
    Properties
    {
        // Controls the intensity of the fresnel effect.
        _FresnelPower("Fresnel Power", Range(0,5)) = 0.5
        // Controls the glow colour.
        _GlowColour("Glow Colour", Color) = (1,1,1,1)
        // Controls the edge colour.
        _EdgeColour("Edge Colour", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            // Render on top of everything else.
            "Queue" = "Overlay"
        }

        Pass
        {
            // Traditional transparency.
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            // Input vertex data.
            struct vertIn
            {
                float4 vertex : POSITION;
                float4 normal: NORMAL;
            };

            // Output vertex data.
            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float fresnelReflectionStrength : COLOR0;
            };

            // Uniform variables used in the implementation of the vertex shader.
            float _FresnelPower;

            // Implementation of the vertex shader.
            vertOut vert(vertIn v)
            {
                vertOut o;
                
                // Calculate the normalized view direction vector in object space. Represents the direction from the 
                // current vertex to the camera in object space. Used to calculate fresnelReflectionStrength.
                float3 normViewDir = normalize(ObjSpaceViewDir(v.vertex));
                // Calculates the strength of the fresnel reflection for a vertex.
                //
                // The dot product measures the cosine of the angle between the vertex normal vector and normalized view
                // direction vector in object space which represents how aligned or opposed the vertex normal vector and the
                // normalized view direction vector are. Subtracting the dot product from 1 effectively inverts the
                // value obtained from the dot product, meaning that when the vertex normal vector and normalised view
                // direction vector are perfectly aligned, the value will be 0, and when they are opposed, it will be 1.
                //
                // When the value is closer to 1, it means the vertex is viewed at an angle, and when it's closer to 0,
                // it means the vertex is viewed directly. This value is used to control the appearance of the force
                // field, creating a glow when the vertex is viewed at an angle. 
                float fresnelReflectionStrength = 1 - saturate(dot(v.normal, normViewDir));
                // Takes into account _FresnelPower in the calculated fresnelReflectionStrength. Subtracting _FresnelPower 
                // from 1 creates a threshold. smoothstep interpolates smoothly between 1 - _FresnelPower and 1 based on 
                // fresnelReflectionStrength.
                o.fresnelReflectionStrength = smoothstep(1 - _FresnelPower, 1, fresnelReflectionStrength);

                // Transforms vertex from object space to clip space.
                o.vertex = UnityObjectToClipPos(v.vertex);

                return o;
            }

            // Uniform variables used in the implementation of the fragment shader.
            uniform float4 _GlowColour;
            uniform float4 _EdgeColour;

            // Implementation of the fragment shader.
            fixed4 frag(vertOut v, fixed faceOrientation : VFACE) : SV_Target
            {
                // The alpha value of the fragment is determined by the fresnel reflection strength of the vertex.
                float alpha = v.fresnelReflectionStrength;
                // Only keep the alpha value of the fragment if it is facing the camera. If the fragment is facing away
                // from the camera, the alpha value of the is set to 0 making it transparent.
                alpha *= step(0, faceOrientation);
                
                // Calculates the RGB color for the fragment based on the linear interpolation between _GlowColour
                // and _EdgeColour, controlled by the alpha value. When alpha is 0, the RGB color for the fragment is
                // closer to _GlowColour, and when alpha is 1, the RGB color for the fragment is closer to _EdgeColour.
                float3 rgb = lerp(_GlowColour.rgb, _EdgeColour.rgb, alpha);
                // Construct the final color of the fragment.
                float4 rgba = float4(rgb, alpha);
                
                return rgba;
            }

            ENDCG
        }
    }
}
