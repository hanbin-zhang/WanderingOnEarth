//#ifndef TESSELLATION_CGINC_INCLUDED
//#define TESSELLATION_CGINC_INCLUDED
#if defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE) || defined(SHADER_API_VULKAN) || defined(SHADER_API_METAL) || defined(SHADER_API_PSSL)
    #define UNITY_CAN_COMPILE_TESSELLATION 1
    #   define UNITY_domain                 domain
    #   define UNITY_partitioning           partitioning
    #   define UNITY_outputtopology         outputtopology
    #   define UNITY_patchconstantfunc      patchconstantfunc
    #   define UNITY_outputcontrolpoints    outputcontrolpoints
#endif



float _tess;
float _MaxtessDistance;
struct Attris
{
    float4 _vertex : POSITION;
    float3 _normal : NORMAL;
    float2 u_v : TEXCOORD0;    
};
struct TessFactors
{
    float edge[3] : SV_TessFactor;
    float inside : SV_InsideTessFactor;
};
struct CPoint
{
    float4 _vertex : INTERNALTESSPOS;
    float2 u_v : TEXCOORD0;
    float3 _normal : NORMAL;   
};

struct Varies
{       
    float3 worldPos : TEXCOORD1;
    float2 u_v : TEXCOORD0;
    float3 _normal : NORMAL;
    float3 viewDir : TEXCOORD3;
    float4 _vertex : SV_POSITION;
    float ffactor : TEXCOORD4;
};

[UNITY_domain("tri")]
[UNITY_outputcontrolpoints(3)]
[UNITY_outputtopology("triangle_cw")]
[UNITY_partitioning("fractional_odd")]
[UNITY_patchconstantfunc("patchConstantFunction")]
CPoint hull(InputPatch<CPoint, 3> patch, uint id : SV_OutputControlPointID)
{
    return patch[id];
}

TessFactors UnityCalcTriEdgeTessFactors (float3 tri_vertexFactors)
{
    TessFactors tess;
    tess.edge[0] = 0.5 * (tri_vertexFactors.y + tri_vertexFactors.z);
    tess.edge[1] = 0.5 * (tri_vertexFactors.x + tri_vertexFactors.z);
    tess.edge[2] = 0.5 * (tri_vertexFactors.x + tri_vertexFactors.y);
    tess.inside = (tri_vertexFactors.x + tri_vertexFactors.y + tri_vertexFactors.z) / 3.0f;
    return tess;
}

float CalcDistanceTessFactor(float4 _vertex, float minDist, float maxDist, float tess)
{
				float3 worldPosition = mul(unity_ObjectToWorld, _vertex).xyz;
				float dist = distance(worldPosition, _WorldSpaceCameraPos);
				float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0);
				return f * tess;
}

TessFactors DistanceBasedTess(float4 v0, float4 v1, float4 v2, float minDist, float maxDist, float tess)
{
				float3 f;
				f.x = CalcDistanceTessFactor(v0, minDist, maxDist, tess);
				f.y = CalcDistanceTessFactor(v1, minDist, maxDist, tess);
				f.z = CalcDistanceTessFactor(v2, minDist, maxDist, tess);

				return UnityCalcTriEdgeTessFactors(f);
}

uniform float3 _Position;
uniform sampler2D _GlobalEffectRT;
uniform float _OrthographicCamSize;

sampler2D  _Noise;
float _NoiseScale, _SnowHeight, _NoiseWeight, _SnowDepth;

TessFactors patchConstantFunction(InputPatch<CPoint, 3> patch)
{
    float minDist = 2.0;
    float maxDist = _MaxtessDistance;
    TessFactors f;
    return DistanceBasedTess(patch[0]._vertex, patch[1]._vertex, patch[2]._vertex, minDist, maxDist, _tess);
   
}

float4 GetShadowPositionHClip(Attris input)
{
    float3 positionWS = TransformObjectToWorld(input._vertex.xyz);
    float3 _normalWS = TransformObjectToWorldNormal(input._normal);
 
    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, _normalWS, 0));
 
#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
#endif
    return positionCS;
}

Varies vert(Attris input)
{
    Varies output;
    
    float3 worldPosition = mul(unity_ObjectToWorld, input._vertex).xyz;
    //create local u_v
    float2 u_v = worldPosition.xz - _Position.xz;
    u_v = u_v / (_OrthographicCamSize * 2);
    u_v += 0.5;
    
    // Effects RenderTexture Reading
    float4 RTEffect = tex2Dlod(_GlobalEffectRT, float4(u_v, 0, 0));
    // smoothstep to prevent bleeding
   	RTEffect *=  smoothstep(0.98, 0.9, u_v.x) * smoothstep(0.98, 0.9,1- u_v.x);
	RTEffect *=  smoothstep(0.98, 0.9, u_v.y) * smoothstep(0.98, 0.9,1- u_v.y);
    
    // worldspace noise texture
    float SnowNoise = tex2Dlod(_Noise, float4(worldPosition.xz * _NoiseScale, 0, 0)).r;
    output.viewDir = SafeNormalize(GetCameraPositionWS() - worldPosition);

	// move vertices up where snow is
	input._vertex.xyz += SafeNormalize(input._normal) * saturate(( _SnowHeight) + (SnowNoise * _NoiseWeight)) * saturate(1-(RTEffect.g * _SnowDepth));

    // transform to clip space
    #ifdef SHADERPASS_SHADOWCASTER
        output._vertex = GetShadowPositionHClip(input);
    #else
        output._vertex = TransformObjectToHClip(input._vertex.xyz);
    #endif

    //outputs
    output.worldPos =  mul(unity_ObjectToWorld, input._vertex).xyz;
    output._normal = input._normal;
    output.u_v = input.u_v;
    output.ffactor = ComputeFogFactor(output._vertex.z);
    return output;
}

[UNITY_domain("tri")]
Varies domain(TessFactors factors, OutputPatch<CPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
{
    Attris v;
    
    #define Interpolate(fieldName) v.fieldName = \
				patch[0].fieldName * barycentricCoordinates.x + \
				patch[1].fieldName * barycentricCoordinates.y + \
				patch[2].fieldName * barycentricCoordinates.z;

    Interpolate(__vertex)
    Interpolate(u_v)
    Interpolate(_normal)
    
    return vert(v);
}
