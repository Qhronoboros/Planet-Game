// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

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

        Pass 
        {
            ZWrite On
            ColorMask 0
        }


        Blend OneMinusDstColor OneMinusSrcAlpha
        BlendOp Add

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;

            struct vertexInput
            {
                float4 vertex: POSITION;
                float4 color : COLOR;  
            };
 
            struct fragmentInput
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR0;
            };
 
            fragmentInput vert( vertexInput i )
            {
                fragmentInput o;
                o.pos = UnityObjectToClipPos(i.vertex);
                o.color = _Color;
                return o;
            }
 
            half4 frag( fragmentInput i ) : COLOR
            {
                return i.color;
            }
            ENDCG
        }
    }
}
