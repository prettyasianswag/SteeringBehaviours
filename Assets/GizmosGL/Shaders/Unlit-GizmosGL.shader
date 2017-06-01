Shader "GizmosGL/Unlit"
{
	Properties
	{
		_Color("Main Color (A=Opacity)", Color) = (1,1,1,1)
	}

	Category
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True"}
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha

		SubShader
		{
			Pass
			{
				GLSLPROGRAM
				varying mediump vec2 uv;

				#ifdef VERTEX
				uniform mediump vec4 _MainTex_ST;
				void main()
				{
					gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
					uv = gl_MultiTexCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				}
				#endif

				#ifdef FRAGMENT
				uniform lowp sampler2D _MainTex;
				uniform lowp vec4 _Color;
				void main()
				{
					gl_FragColor = _Color;
				}
				#endif     
				ENDGLSL
			}
		}

		SubShader
		{ 
			Color[_Color]
			Pass {}
		}
	}

}