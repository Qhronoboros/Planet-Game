Shader "Custom/WhiteFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [PerRendererData]_Color ("Color", COLOR) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

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
            float4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) + float4(_Color.r, _Color.g, _Color.b, 0.0f);
                col.a = saturate(col.a - _Color.a);
                return col;
            }
            ENDCG
        }
    }
}
