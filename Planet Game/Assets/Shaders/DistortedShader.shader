// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Distorted"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", COLOR) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent"}

        GrabPass {"_GrabTexture"}

        ZWrite Off
        Blend Off

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;
            sampler2D _GrabTexture;

            struct appdata
            {
                float4 pos : POSITION;
                float4 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 grabPos : TEXCOORD0;
            };
 
            v2f vert( appdata v )
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }
 
             float random (float2 uv)
            {
                return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
            }


            half4 frag( v2f i ) : SV_Target
            {
                float3 baseWorldPos = mul( unity_ObjectToWorld, float4(0,0,0,0) ).xyz;
                float distanceCenter = distance(i.grabPos.xy, baseWorldPos.xy);
                float step = smoothstep(1.0, 0.0, distanceCenter);
                float4 col = tex2Dproj(_GrabTexture, i.grabPos + float4(1.1f*2, 1.1f/2, 1.1f/2, 1.1f/2));

                return col;
            }
            ENDCG
        }
    }
}
