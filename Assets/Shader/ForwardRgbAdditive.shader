// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CriMana/ForwardRgbAdditive" {
Properties {
 _MainTex ("Texture", 2D) = "white" {}
}
SubShader {
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  Blend One One

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    sampler2D _MainTex;
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
      return tex2D(_MainTex, f.texcoord);
    }
  ENDCG
 }
}
}
