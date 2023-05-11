struct VS_IN
{
	float4 pos : POSITION;
	float4 col : COLOR;
	float4 locPos : LOCALPOS;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float4 col : COLOR;
	float4 locPos : LP;
};

float4x4 WorldViewProj;

PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN)0;

	output.pos = mul(input.pos, WorldViewProj);
	output.col = input.col;
	output.locPos = input.locPos;
	return output;
}

float4 PS(PS_IN input) : SV_Target
{
	float epsilon = 0.1f;
	if ( (abs(input.locPos.x)>1-epsilon && abs(input.locPos.y)> 1 - epsilon) || (abs(input.locPos.x) > 1 - epsilon && abs(input.locPos.z) > 1 - epsilon) || (abs(input.locPos.z) > 1 - epsilon && abs(input.locPos.y) > 1 - epsilon))
	{
		float4 c = float4(0,0,0, 1.0f);
		return c;
	}
	else
	{
		return input.col;
	}

}