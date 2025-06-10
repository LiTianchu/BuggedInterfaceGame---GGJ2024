Shader "UI/StartMenuFile"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BGColor ("BGColor", Color) = (1,1,1,1)
        _Progress ("Progress", Range(0, 1)) = 0.0
        [HDR] _LineColor ("Line Color", Color) = (1, 0, 0, 1)
        _LineWidth ("Line Width", Range(0,1)) = 0.02
        
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]  // This is crucial for Screen Space - Camera
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _BGColor;
            fixed4 _LineColor;
            float _Progress;
            float _LineWidth;
            float4 _MainTex_ST;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                float y = IN.texcoord.y;

                
                // Apply background color if y is above progress
                if (y > _Progress)
                {
                    color *= _BGColor;
                }
                if(y+ _LineWidth >=_Progress && y <= _Progress)
                {
                    // If the y coordinate is within the line width of the progress, apply line color
                    color = _LineColor;
                }
                
                return color;
            }
            ENDCG
        }
    }
}