//Adding the tessellation
Shader "Example/URPUnlitShaderTessallated"
{


	Properties
	{
		_Tess("Tessellation", Range(1, 32)) = 20
		_MaxTessDistance("Max Tess Distance", Range(1, 32)) = 20
		_Noise("Noise", 2D) = "gray" {}

	_Weight("Displacement Amount", Range(0, 1)) = 0
	}


		SubShader
	{
		
		Tags{ "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

		Pass
	{
		Tags{ "LightMode" = "UniversalForward" }


		HLSLPROGRAM

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"    
#include "CustomTessellation.hlsl"


#pragma require tessellation
		
#pragma _vertex TessVertexProgram
	
#pragma fragment frag

#pragma hull hull

#pragma domain domain

		sampler2D _Noise;
	float _Weight;

	// pre tesselation vertex program
	CPoint TessVertexProgram(Attris v)
	{
		CPoint pe;

		pe._vertex = v._vertex;
		pe.u_v = v.u_v;
		pe._normal = v._normal;
		pe._color = v._color;

		return pe;
	}

	// after tesselation
	Varies vert(Attris input)
	{
		Varies output;
		float Noise = tex2Dlod(_Noise, float4(input.u_v, 0, 0)).r;

		input._vertex.xyz += (input._normal) *  Noise * _Weight;
		output._vertex = TransformObjectToHClip(input._vertex.xyz);
		output._color = input._color;
		output._normal = input._normal;
		output.u_v = input.u_v;
		return output;
	}

	[UNITY_domain("tri")]
	Varies domain(TessFactors factors, OutputPatch<CPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
	{
		Attris v;

#define DomainPos(fieldName) v.fieldName = \
				patch[0].fieldName * barycentricCoordinates.x + \
				patch[1].fieldName * barycentricCoordinates.y + \
				patch[2].fieldName * barycentricCoordinates.z;

			DomainPos(__vertex)
			DomainPos(u_v)
			DomainPos(__color)
			DomainPos(__normal)

			return vert(v);
	}

	// The fragment shader definition.            
	half4 frag(Varies IN) : SV_Target
	{
		half4 tex = tex2D(_Noise, IN.u_v);

		return tex;
	}
		ENDHLSL
	}
	}
}