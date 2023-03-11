// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CriMana/Yuv2Rgb" {
Properties {
 _MainTex ("Texture", 2D) = "white" {}
 Texture_y ("Texture Y", 2D) = "white" {}
 Texture_u ("Texture U", 2D) = "white" {}
 Texture_v ("Texture V", 2D) = "white" {}
}
SubShader {
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    sampler2D Texture_y;
    sampler2D Texture_u;
    sampler2D Texture_v;
    float4 _MainTex_ST;

    struct appdata_t
    {
      float4 vertex : POSITION;
      half2 texcoord : TEXCOORD0;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      half2 texcoord : TEXCOORD0;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = UnityObjectToClipPos(v.vertex);
      o.texcoord = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);

      return o;
    }

    fixed4 frag(v2f f) : COLOR
    {
      fixed3 tmpvar;
      fixed4 color;

      tmpvar.x = (tex2D(Texture_y, f.texcoord).w - 0.06275);
      tmpvar.y = (tex2D(Texture_u, f.texcoord).w - 0.50196);
      tmpvar.z = (tex2D(Texture_v, f.texcoord).w - 0.50196);
      color.xyz = mul(float3x3(1.16438, 0.0, 1.59603, 1.16438, -0.39176, -0.81297, 1.16438, 2.01723, 0.0), tmpvar);
      color.w = 1.0;

      return color;
    }
  ENDCG
 }
}
}
