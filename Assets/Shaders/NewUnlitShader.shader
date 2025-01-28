Shader "Custom/LightMask"
{
    Properties
    {
        _Radius ("Radius", Float) = 0.3
        _EdgeSmoothness ("Edge Smoothness", Float) = 0.1
        _DarknessColor ("Darkness Color", Color) = (0, 0, 0, 1)  
        _LightColor ("Light Color", Color) = (1, 1, 1, 0)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }

        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off

            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Radius;
            float _EdgeSmoothness;
            float4 _DarknessColor;
            float4 _LightColor;

            float2 AdjustForAspect(float2 uv, float aspect)
            {
                uv.x *= aspect;
                return uv;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float smoothstep(float edge0, float edge1, float x)
            {
                return saturate((x - edge0) / (edge1 - edge0));
            }

            half4 frag(v2f i) : SV_Target
            {

                float aspect = 1920.0 / 1080.0;
                float2 adjustedUV = AdjustForAspect(i.uv, aspect);
                
                float distanceFromCenter = length(adjustedUV - float2(0.89, 0.5));

                float alpha = smoothstep(_Radius, _Radius - _EdgeSmoothness, distanceFromCenter);

                half4 color = _LightColor - 1;
                color.a = 0.1 - alpha;

                color = lerp(_DarknessColor, color, alpha);

                return color;
            }
            ENDCG
        }
    }

    FallBack "Unlit/Transparent"
}
