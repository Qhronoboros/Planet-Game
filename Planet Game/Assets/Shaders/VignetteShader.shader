Shader "Custom/VignetteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VColor ("Vignette Color", COLOR) = (1, 1, 1, 1)
        _VRadius ("Vignette Radius", Range(0.0, 1.0)) = 1.0
        _VSoft ("Vignette Softness", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _VColor;
            float _VRadius;
            float _VSoft;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mainCol = tex2D(_MainTex, i.uv);

                // Find how far pixel is from center
                float distanceCenter = distance(i.uv.xy, float2(0.5, 0.5));

                // Apply the radius and softness of the vignette
                float vignette = smoothstep(_VRadius, _VRadius - _VSoft, distanceCenter).r;

                // Clamp between 0 and 1 and apply color
                fixed4 result = saturate(mainCol * (vignette * _VColor));
                
                return result;
            }
            ENDCG
        }
    }
}
