Shader "Custom/GameOverBackgroundShader"
{
    Properties
    {
        _ColorA("Color A", Color) = (0.2, 0.2, 0.6, 1)
        _ColorB("Color B", Color) = (0.8, 0.1, 0.3, 1)
        _Speed("Animation Speed", float) = 1.5
        _Scale("Pattern Scale", float) = 3
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            float4 _ColorA;
            float4 _ColorB;
            float _Speed;
            float _Scale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.screenPos.xy / i.screenPos.w;
                float pattern = sin((uv.x + _Time.y * _Speed) * _Scale) * cos((uv.y + _Time.y * _Speed) * _Scale);
                pattern = smoothstep(-0.5, 0.5, pattern);
                return lerp(_ColorA, _ColorB, pattern);
            }
            ENDCG
        }
    }
}
