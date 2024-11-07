Shader "Custom/EdgeOutlineShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1) // Màu đường viền (mặc định là đen)
        _OutlineThickness ("Outline Thickness", Float) = 0.03 // Độ dày của đường viền (mặc định là 0.03)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // Pass cho phần Outline
        Pass
        {
            Name "Outline"
            Cull Front
            ZWrite On
            ColorMask 0

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            fixed4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal); // Lấy normal trong không gian thế giới
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Đẩy các đỉnh ra ngoài dựa trên normal và độ dày
                worldPos += worldNormal * _OutlineThickness;
                o.pos = UnityWorldToClipPos(float4(worldPos, 1.0));
                o.color = _OutlineColor;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }

        // Pass cho phần gốc của đối tượng
        Pass
        {
            Name "Base"
            Cull Back
            ZWrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
