Shader "Tri-Planar World"
{
	Properties
	{
		_Side("Side", 2D) = "white" {}
		_Top("Top", 2D) = "white" {}
		_Bottom("Bottom", 2D) = "white" {}
		_WallOffset("Top Offset", Float) = 0
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
			"IgnoreProjector" = "False"
			"RenderType" = "Opaque"
		}

		Cull Back
		ZWrite On

		CGPROGRAM

		#pragma surface surf Lambert
		#pragma exclude_renderers flash

		sampler2D _Side, _Top, _Bottom;
		float _SideScale, _TopScale, _BottomScale, _WallOffset;

		struct Input {
			float3 worldPos;
			float3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			float3 projNormal = saturate(pow(IN.worldNormal * 1.4, 4));
			IN.worldPos.y += _WallOffset;
			// SIDE X
			float3 x = tex2D(_Side, frac(IN.worldPos.zy)) * abs(IN.worldNormal.x);

			// TOP / BOTTOM
			float3 y = 0;
			if (IN.worldNormal.y > 0) {
				y = tex2D(_Top, frac(IN.worldPos.zx)) * abs(IN.worldNormal.y);
			}
			else {
				y = tex2D(_Bottom, frac(IN.worldPos.zx)) * abs(IN.worldNormal.y);
			}

			// SIDE Z	
			float3 z = tex2D(_Side, frac(IN.worldPos.xy)) * abs(IN.worldNormal.z);

			o.Albedo = z;
			o.Albedo = lerp(o.Albedo, x, projNormal.x);
			o.Albedo = lerp(o.Albedo, y, projNormal.y);
		}
		ENDCG
	}
	Fallback "Diffuse"
}