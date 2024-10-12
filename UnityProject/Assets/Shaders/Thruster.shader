Shader "Unlit/Thruster"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 hsb2rgb(float3 c)
            {
                float3 rgb = clamp(
                    abs(
                        fmod(c.x*30.0 + float3(0.0, 4.0, 2.0), 6.0)
                    - 3.0) - 1.0, 
                    0.0, 1.0);
                rgb = rgb * rgb * (3.0 - 2.0 * rgb);
                return c.z * lerp(float3(1.0, 1.0, 1.0), rgb, c.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed2 st = i.uv;

                fixed2 toCenter = fixed2(0.5,1)-st;
                float twoPi = 6.28318530718f;
                float onePi = twoPi / 2.0f;
                float angle = atan2(toCenter.y ,toCenter.x ) + 10.0f + (_Time.y);
                float radius = length(toCenter)*4.0f;

                col.xyz *= hsb2rgb(fixed3((angle/twoPi),radius,1.0));

                return col;
            }
            ENDCG
        }
    }
}
