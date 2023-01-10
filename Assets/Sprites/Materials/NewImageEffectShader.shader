// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/NewImageEffectShader"
{
   Properties
{
[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
_Color ("Tint", Color) = (1,1,1,1)
[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
_WavesBumpMap ("MainNormalmap", 2D) = "bump" {}

_WavesScale("Waves Scale", Range(0.01, 100.0)) = 1.0
_WavesSpeed("Waves Speed", Range(0.01, 0.5)) = 0.05
_RefractionEffect("Refraction Effect", Range(0.001, 0.05)) = 0.01
_DirX("Dir X", Range(1.0, 1.0)) = 1.0
_DirY("Dir Y", Range(1.0, 1.0)) = 1.0
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
// ��������� ����� ����� �������� ����� �������� ��� ������ � ����� ���
Cull Off
Lighting Off
ZWrite Off
Blend One OneMinusSrcAlpha

Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ PIXELSNAP_ON
#include "UnityCG.cginc"

// ���������� � �������� �������� ������, ��� ��� ������ �� properties �������� ������� ������������� � ���������
sampler2D _MainTex;
fixed4 _Color;
sampler2D _WavesBumpMap;

half _WavesSpeed;
half _WavesScale;
half _RefractionEffect;
half _DirX;
half _DirY;

struct appdata_t
{
float4 vertex : POSITION;
float4 color : COLOR;
float2 texcoord : TEXCOORD0;
};

struct v2f
{
float4 vertex : SV_POSITION;
fixed4 color : COLOR;
half2 texcoord : TEXCOORD0;
};

// ����������� ��������� ������
v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
OUT.vertex = UnityPixelSnap (OUT.vertex);
#endif

return OUT;
}

// ����������� ������ ����������� �� ������ ��������� ����� ��������
fixed4 frag(v2f IN) : SV_Target
{
fixed2 waveBump = IN.texcoord;
waveBump.x += _DirX * _Time.y * _WavesSpeed;
waveBump.y += _DirY * _Time.y * _WavesSpeed;

// ������� �������� ��������� �� ���� �����
waveBump = UnpackNormal(tex2D(_WavesBumpMap, waveBump * _WavesScale)).xy;
// ������� ������ �� �������� ������ ��������� ���������� ���������� ������� ��������� �� waveBump
fixed4 c = tex2D(_MainTex, IN.texcoord + waveBump * _RefractionEffect) * IN.color;
// ������� ���������� ���� � ����� � ���������
c.rgb *= c.a;
return c;
}
ENDCG
}
}
}
