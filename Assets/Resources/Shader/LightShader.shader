Shader "UI/DarknessHolePerfectCircle"
{
    Properties
    {
        _PlayerPos ("Player Position", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", float) = 0.15
        _Softness ("Softness", float) = 0.05
        _AspectRatio ("Aspect Ratio", float) = 1.7778 // 1920 / 1080
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _PlayerPos;
            float _Radius;
            float _Softness;
            float _AspectRatio;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Correction de l'UV selon le ratio écran
                float2 uv = i.uv;
                uv.x = (uv.x - 0.5) * _AspectRatio + 0.5;
                float2 playerUV = _PlayerPos.xy;
                playerUV.x = (playerUV.x - 0.5) * _AspectRatio + 0.5;

                float dist = distance(uv, playerUV);

                float alpha = smoothstep(_Radius, _Radius + _Softness, dist);

                return fixed4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}
