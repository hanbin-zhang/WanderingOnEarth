#if defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE) || defined(SHADER_API_VULKAN) || defined(SHADER_API_METAL) || defined(SHADER_API_PSSL)
#define UNITY_CAN_COMPILE_TESSELLATION 1
#   define UNITY_domain                 domain
#   define UNITY_partitioning           partitioning
#   define UNITY_outputtopology         outputtopology
#   define UNITY_patchconstantfunc      patchconstantfunc
#   define UNITY_outputcontrolpoints    outputcontrolpoints
#endif


struct CPoint
{
	float4 _vertex : INTERNALTESSPOS;
	float2 u_v : TEXCOORD0;
	float4 _color : COLOR;
	float3 _normal : NORMAL;
};

// vertex to fragment struct
struct Varies
{
	float4 _color : COLOR;
	float3 _normal : NORMAL;
	float4 _vertex : SV_POSITION;
	float2 u_v : TEXCOORD0;
	float4 _noise : TEXCOORD1;
};

struct Attris
{
	float4 _vertex : POSITION;
	float3 _normal : NORMAL;
	float2 u_v : TEXCOORD0;
	float4 _color : COLOR;

};

struct TessFactors
{
	float edge[3] : SV_TessFactor;
	float Inside : SV_InsideTessFactor;
};


// tessellation variables, add these to your shader properties
float _Tess;
float _MaxTessDistance;

// info so the GPU knows what to do (triangles) and how to set it up , clockwise, fractional division
// hull takes the original vertices and outputs more
//[UNITY_partitioning("fractional_even")]
//[UNITY_partitioning("pow2")]
//[UNITY_partitioning("integer")]

[UNITY_outputtopology("triangle_cw")]
[UNITY_partitioning("fractional_odd")]
[UNITY_domain("tri")]
[UNITY_outputcontrolpoints(3)]
[UNITY_patchconstantfunc("patchConstantFunction")]
CPoint hull(InputPatch<CPoint, 3> patch, uint id : SV_OutputControlPointID)
{
	return patch[id];
}

TessFactors UnityCalcTriEdgeTessFactors (float3 triVertexFactors)
{
    TessellationFactors tesse;
    tess.edge[0] = 0.5 * (triVertexFactors.y + triVertexFactors.z);
    tess.edge[1] = 0.5 * (triVertexFactors.x + triVertexFactors.z);
    tess.edge[2] = 0.5 * (triVertexFactors.x + triVertexFactors.y);
    tess.inside = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
    return tesse;
}

// fade tessellation at a distance
float CalculateDistanceTessFactor(float4 _vertex, float minDistance, float maxDistance, float tesse)
{
				float3 worldPosition = mul(unity_ObjectToWorld, _vertex).xyz;
				float distance = distance(worldPosition, _WorldSpaceCameraPos);
				float fe = clamp(1.0 - (distance - minDistance) / (maxDistance - minDistance), 0.01, 1.0);

				return fe * tesse;
}

TessFactors DistanceBasedTess(float4 v0, float4 v1, float4 v2, float minDistance, float maxDistance, float tesse)
{
				float3 ff;
				ff.x = CalculateDistanceTessFactor(v0, minDistance, maxDistance, tesse);
				ff.y = CalculateDistanceTessFactor(v1, minDistance, maxDistance, tesse);
				ff.z = CalculateDistanceTessFactor(v2, minDistance, maxDistance, tesse);

				return UnityCalcTriEdgeTessFactors(ff);
}



float UnityCalculateEdgeTessFactor (float3 w_pos0, float3 w_pos1, float edgeLength)
{
    // distance to edge center
    float distan = distance (0.5 * (w_pos0+w_pos1), _WorldSpaceCameraPos);
    // length of the edge
    float length = distance(w_pos0, w_pos1);
    // edgeLen is approximate desired size in pixels
    float ff = max(length * _ScreenParams.y / (edgeLength * distan), 1.0);
    return ff;
}


float DistanceFromPlane (float3 _position, float4 plane)
{
    float de = dot (float4(_position,1.0f), plane);
    return de;
}


// Returns true if triangle with given 3 world _positions is outside of camera's view frustum.
// cullEps is distance outside of frustum that is still considered to be inside (i.e. max displacement)
bool UnityWorldViewFrustumCull (float3 w_pos0, float3 w_pos1, float3 w_pos2, float cull_eps)
{
    float4 plane_test;

    // left
    plane_test.x = (( DistanceFromPlane(w_pos0, unity_CameraWorldClipPlanes[0]) > -cull_eps) ? 1.0f : 0.0f ) +
                  (( DistanceFromPlane(w_pos1, unity_CameraWorldClipPlanes[0]) > -cull_eps) ? 1.0f : 0.0f ) +
                  (( DistanceFromPlane(w_pos2, unity_CameraWorldClipPlanes[0]) > -cull_eps) ? 1.0f : 0.0f );
    // right
    plane_test.y = (( DistanceFromPlane(w_pos0, unity_CameraWorldClipPlanes[1]) > -cull_eps) ? 1.0f : 0.0f ) +
                  (( DistanceFromPlane(w_pos1, unity_CameraWorldClipPlanes[1]) > -cull_eps) ? 1.0f : 0.0f ) +
                  (( DistanceFromPlane(w_pos2, unity_CameraWorldClipPlanes[1]) > -cull_eps) ? 1.0f : 0.0f );
    // top
    plane_test.z = (( DistanceFromPlane(w_pos0, unity_CameraWorldClipPlanes[2]) > -cull_eps) ? 1.0f : 0.0f ) +
                  (( DistanceFromPlane(w_pos1, unity_CameraWorldClipPlanes[2]) > -cull_eps) ? 1.0f : 0.0f ) +
                  (( DistanceFromPlane(w_pos2, unity_CameraWorldClipPlanes[2]) > -cull_eps) ? 1.0f : 0.0f );
    // bottom
    plane_test.w = (( DistanceFromPlane(w_pos0, unity_CameraWorldClipPlanes[3]) > -cull_eps) ? 1.0f : 0.0f ) +
                  (( DistanceFromPlane(w_pos1, unity_CameraWorldClipPlanes[3]) > -cull_eps) ? 1.0f : 0.0f ) +
                  (( DistanceFromPlane(w_pos2, unity_CameraWorldClipPlanes[3]) > -cull_eps) ? 1.0f : 0.0f );

    // has to pass all 4 plane tests to be visible
    return !all (plane_test);
}



TessFactors UnityEdgeLengthBasedTess (float4 v0, float4 v1, float4 v2, float edgeLength)
{
    float3 _pos0 = mul(unity_ObjectToWorld,v0).xyz;
    float3 _pos1 = mul(unity_ObjectToWorld,v1).xyz;
    float3 _pos2 = mul(unity_ObjectToWorld,v2).xyz;
    TessellationFactors tess;
    tess.edge[0] = UnityCalcEdgeTessFactor (_pos1, _pos2, edgeLength);
    tess.edge[1] = UnityCalcEdgeTessFactor (_pos2, _pos0, edgeLength);
    tess.edge[2] = UnityCalcEdgeTessFactor (_pos0, _pos1, edgeLength);
    tess.inside = (tess.edge[0] + tess.edge[1] + tess.edge[2]) / 3.0f;
    return tess;
}


// Same as UnityEdgeLengthBasedTess, but also does patch frustum culling:
// patches outside of camera's view are culled before GPU tessellation. Saves some wasted work.
TessFactors UnityEdgeLengthBasedTessCull (float4 v0, float4 v1, float4 v2, float edgeLen, float maxDisplacement)
{
    float3 _pos0 = mul(unity_ObjectToWorld,v0).xyz;
    float3 _pos1 = mul(unity_ObjectToWorld,v1).xyz;
    float3 _pos2 = mul(unity_ObjectToWorld,v2).xyz;
    TessFactors tess;

    if (UnityWorldViewFrustumCull(_pos0, _pos1, _pos2, maxDisplacement))
    {
        tess.edge[0] = 0.0f;
		tess.edge[1] = 0.0f;
		tess.edge[2] = 0.0f;
		tess.inside = 0.0f;
    }
    else
    {
        tess.edge[0] = UnityCalculateEdgeTessFactor (_pos1, _pos2, edgeLen);
        tess.edge[1] = UnityCalculateEdgeTessFactor (_pos2, _pos0, edgeLen);
        tess.edge[2] = UnityCalculateEdgeTessFactor (_pos0, _pos1, edgeLen);
        tess.inside = (tess.edge[0] + tess.edge[1] + tess.edge[2]) / 3.0f;
    }
    return tess;
}

TessFactors patchConstantFunction(InputPatch<CPoint, 3> patch)
{
    float minDistance = 2.0;
    float maxDistance = _MaxTessDistance + minDistance;
    TessFactors f;

    // distance based tesselation
    return DistanceBasedTess(patch[0].vertex, patch[1].vertex, patch[2].vertex, minDistance, maxDistance, _Tess);
    
}

#define Interpolate(fieldName) v.fieldName = \
				patch[0].fieldName * barycentricCoordinates.x + \
				patch[1].fieldName * barycentricCoordinates.y + \
				patch[2].fieldName * barycentricCoordinates.z;



