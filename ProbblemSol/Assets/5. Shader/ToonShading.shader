Shader "Unlit/ToonShading"
{
    Properties
    {
        _LightDirection("LightDirection", Vector) = (1,-1,-1,0)
        _DiffuseColor("DiffuseColor", Color) = (0,0,1,1)
        _AmbientStrength("Ambient Strength", Float) = 0.2
        _SpecularDegree("Specular Degree",Float) = 0.2
        _Threshold("Threshold", Float) = 0.4
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
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
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _LightDirection;
            float3 _DiffuseColor;
            float _AmbientStrength;
            float _SpecularDegree;
            float _Threshold;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 법선 벡터를 정규화합니다.
                float3 normal = normalize(i.normal);
                // 조명의 방향을 설정합니다. (여기서는 단순히 모든 방향으로 일정한 빛을 쏘는 것으로 가정합니다.)
                float3 lightDir = normalize(_LightDirection);
                // 시점 벡터를 정규화합니다.
                float3 viewDir = normalize(i.viewDir);

                // 앰비언트 광원을 계산합니다.
                float3 ambient = _AmbientStrength * float3(1.0, 1.0, 1.0);
                 
                // 디퓨즈(방향성) 조명을 계산합니다.
                float diff = max(dot(normal, lightDir), 0.0);
                float3 diffuse = diff * _DiffuseColor;

                // 반사 광택을 계산합니다.
                float3 reflectDir = reflect(-lightDir, normal);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0); 
                float3 specular = _SpecularDegree * spec;

                // 색상 밴딩을 구현합니다.
                float threshold = _Threshold;
                float3 banding = floor(diffuse / threshold) * threshold;
                float3 finalColor = banding + specular + ambient; // 디퓨즈, 반사 광택 및 앰비언트 광원을 모두 합칩니다.

                // 결과를 반환합니다.
                return float4(finalColor.rgb, 1.0);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
