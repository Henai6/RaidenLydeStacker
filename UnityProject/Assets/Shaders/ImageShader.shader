Shader "Unlit/ImageShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "Size" {}
        tex ("Texture", 2D) = "Texture" {}
        offsetx ("OffsetX", float) = 1
        offsety ("OffsetY", float) = 1
        ytiles ("yTiles", float) = 12
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
            sampler2D tex;
            float offsetx;
            float offsety;
            float ytiles;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Get image tile
                float texWidth = 1080; 
                float texHeight = 1080;

                float cellSize = texHeight / ytiles;
                float usedWidth = cellSize * 8;
                float cropWidth = (texWidth - usedWidth) / 2;

                float scaledCellSize = cellSize / texWidth;
                float2 tileUV = i.uv / ytiles;
                tileUV.x += (cropWidth / texWidth) + scaledCellSize * offsetx;
                tileUV.y += scaledCellSize * offsety;
                fixed4 col = tex2D(tex, tileUV);


                //Round corners
                float radius = 0.3;
                float2 d = abs(i.uv * 2 -1) - (1.0 - radius);
                float dist = length(max(d, 0.0)) - radius;
                float alpha = step(0.01, -dist);
                col.w = alpha;

                return col;
            }
            ENDCG
        }
    }
}
