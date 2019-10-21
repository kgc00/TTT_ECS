// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TileShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(.002,1)) = .005
    }
    SubShader
    {        
        CGPROGRAM
        #pragma surface surf Lambert
        
        struct Input {
            float4 color : COLOR;
        };
        
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o){
            o.Albedo = _Color.rgb;
        }
        ENDCG

        Pass{
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert(appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float3 norm = normalize(
                mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal)
                );
                float2 offset = TransformViewToProjection(norm.xy);

                o.pos.xy += offset * o.pos.z * _OutlineWidth;
                o.color = _OutlineColor;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                return i.color;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
