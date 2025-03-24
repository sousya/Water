Shader "Custom/ShineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _ShineSpeed ("Shine Speed", Range(0, 10)) = 1
        _ShineWidth ("Shine Width", Range(0, 1)) = 0.2
        _ShineIntensity ("Shine Intensity", Range(0, 1)) = 0.5
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
            float4 _ShineColor;
            float _ShineSpeed;
            float _ShineWidth;
            float _ShineIntensity;
            
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
                float time = _Time.y * _ShineSpeed;
                float shine = sin((i.uv.x - time) * 10) * 0.5 + 0.5;
                shine = smoothstep(0, _ShineWidth, shine) * smoothstep(_ShineWidth, 0, shine);
                
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.color;
                col += _ShineColor * shine * _ShineIntensity;
                
                return col;
            }
            ENDCG
        }
    }
} 