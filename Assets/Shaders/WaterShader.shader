Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaterColor ("Water Color", Color) = (1,1,1,1)
        _WaterLevel ("Water Level", Range(0, 1)) = 0.5
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 1
        _WaveAmplitude ("Wave Amplitude", Range(0, 1)) = 0.1
        _WaveFrequency ("Wave Frequency", Range(0, 10)) = 1
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        
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
                float4 color : COLOR;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _WaterColor;
            float _WaterLevel;
            float _WaveSpeed;
            float _WaveAmplitude;
            float _WaveFrequency;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float time = _Time.y * _WaveSpeed;
                float wave = sin(i.uv.x * _WaveFrequency + time) * _WaveAmplitude;
                
                float waterMask = step(i.uv.y, _WaterLevel + wave);
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _WaterColor;
                col.a *= waterMask;
                
                return col;
            }
            ENDCG
        }
    }
} 