Shader "Hidden/Transition"
{
	Properties
	{
	    _MainTex ("", 2D) = "" {} 
		_TransitionTime ("Time", Float) = 0
		_PositionX("PositionX", Float) = 0
		_PositionY("PositionY", Float) = 0
	}
	SubShader
	{
		Tags {"Queue" = "Transparent+10000"}
		GrabPass{}
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _TransitionTime;
			float _PositionX;
			float _PositionY;
			fixed4 frag (v2f i) : SV_Target
			{
			    float Distance = distance(i.uv ,float2(_PositionX,_PositionY));
			    float Angle = asin((i.uv.y - _PositionY) / Distance);
			    if(i.uv.x < _PositionX){
			        Angle = acos((i.uv.x - _PositionX) / Distance);
			        if(i.uv.y < _PositionY){
			        Angle *= -1;
			        }
			    }
			    Angle += ((Distance * 5) * (_TransitionTime * 5) / 5);
				fixed4 col = tex2D(_MainTex, float2(_PositionX,_PositionY) + float2(cos(Angle) * Distance,sin(Angle) * Distance));
				return col;
			}
			ENDCG
		}
	}
}
