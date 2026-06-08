Shader "Custom/MissingTexture"
{
    Properties
    {
        _Scale ("Checker Scale", Float) = 2.0
        _Color1 ("Main Color (Pink)", Color) = (1.0, 0.0, 1.0, 1.0) 
        _Color2 ("Secondary Color (Black)", Color) = (0.0, 0.0, 0.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
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
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0; 
            };

            float _Scale;
            float4 _Color1;
            float4 _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                // 1. Get the vertex's position relative to the camera lens
                float3 vertexViewPos = UnityObjectToViewPos(v.vertex);
                
                // 2. Get the object's center (0,0,0 locally) relative to the camera lens
                float3 centerViewPos = UnityObjectToViewPos(float3(0, 0, 0));
                
                // 3. Subtract them. This gives us the exact vertex offset from the object's center,
                // but perfectly aligned to the camera's flat screen axes.
                float2 alignedUV = vertexViewPos.xy - centerViewPos.xy;
                
                o.uv = alignedUV * _Scale;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Procedural checkerboard math
                float gridX = floor(i.uv.x);
                float gridY = floor(i.uv.y);
                
                float checker = fmod(abs(gridX + gridY), 2.0);

                return lerp(_Color1, _Color2, checker);
            }
            ENDCG
        }
    }
}

// Shader "Custom/MissingTexture"
// {
//     Properties
//     {
//         _Scale ("Checker Scale", Float) = 15.0
//         _Color1 ("Main Color (Pink)", Color) = (1.0, 0.0, 1.0, 1.0) 
//         _Color2 ("Secondary Color (Black)", Color) = (0.0, 0.0, 0.0, 1.0)
//     }
//     SubShader
//     {
//         Tags { "RenderType"="Opaque" "Queue"="Geometry" }
//         LOD 100

//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             #include "UnityCG.cginc"

//             struct appdata
//             {
//                 float4 vertex : POSITION;
//             };

//             struct v2f
//             {
//                 float4 pos : SV_POSITION;
//                 // TEXCOORD0 will hold our screen space position
//                 float4 screenPos : TEXCOORD0; 
//             };

//             float _Scale;
//             float4 _Color1;
//             float4 _Color2;

//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 // Convert 3D object space to 2D screen clip space
//                 o.pos = UnityObjectToClipPos(v.vertex);
                
//                 // Calculate the screen position for this vertex
//                 o.screenPos = ComputeScreenPos(o.pos);
//                 return o;
//             }

//             fixed4 frag (v2f i) : SV_Target
//             {
//                 // Perspective divide to get normalized screen coordinates (0.0 to 1.0)
//                 float2 screenUV = i.screenPos.xy / i.screenPos.w;

//                 // Multiply the X axis by the screen's aspect ratio.
//                 // Without this, the checkerboard squares will stretch if your screen isn't perfectly square.
//                 float aspect = _ScreenParams.x / _ScreenParams.y;
//                 screenUV.x *= aspect;

//                 // Apply our custom scaling to determine how many squares fit on screen
//                 screenUV *= _Scale;

//                 // Procedural checkerboard math
//                 // Floor gives us whole numbers for the grid, absolute ensures no negative grid errors
//                 float gridX = floor(screenUV.x);
//                 float gridY = floor(screenUV.y);
                
//                 // fmod calculates the remainder. If the sum of X and Y is even, it returns 0. If odd, 1.
//                 float checker = fmod(abs(gridX + gridY), 2.0);

//                 // Lerp swaps between Color1 and Color2 based on whether checker is 0 or 1
//                 return lerp(_Color1, _Color2, checker);
//             }
//             ENDCG
//         }
//     }
// }