Shader "Custom/DisableZWriteShader"
{
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
		}

		Pass
		{
			ZWrite Off
		}
	}
}