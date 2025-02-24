Shader "UI/WhiteBandWithColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // UI纹理
        _BandPosition ("Band Position", Range(0, 10)) = 0 // 光带位置
        _BandWidth ("Band Width", Range(0, 1)) = 0.1 // 光带宽度
        _BandColor ("Band Color", Color) = (1,1,1,1) // 光带颜色
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR; // 接收Image的颜色
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR; // 传递Image的颜色到片段着色器
            };

            sampler2D _MainTex;
            float _BandPosition;
            float _BandWidth;
            fixed4 _BandColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color; // 传递Image的颜色
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); // 原始纹理颜色

                 // 计算斜向的光带区域，基于 i.uv.x 和 i.uv.y
                float angle = 45.0; // 斜向角度，例如 45 度
                float slope = tan(radians(angle)); // 根据角度计算斜率
                float uv_value = i.uv.x + slope * i.uv.y; // 斜向的 UV 坐标值
                // float band = smoothstep(_BandPosition - _BandWidth, _BandPosition, i.uv.x) - 
                //              smoothstep(_BandPosition, _BandPosition + _BandWidth, i.uv.x); // 光带区域
                float band = smoothstep(_BandPosition - _BandWidth, _BandPosition, uv_value) - 
                             smoothstep(_BandPosition, _BandPosition + _BandWidth, uv_value); // 光带区域
                col.rgb = lerp(i.color, _BandColor.rgb, band); // 混合光带颜色
                return col;
            }
            ENDCG
        }
    }
}