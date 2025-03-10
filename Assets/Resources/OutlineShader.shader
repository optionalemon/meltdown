Shader "Custom/Outlined" {
    SubShader {
        Tags { "RenderType"="Opaque" }
        
        Pass {
            // Outline pass
            Name "OUTLINE"
            Tags { "LightMode"="Always" }
            
            Cull Front
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha
            Offset 10, 10
            
            SetTexture[_OutlineTex] {
                combine primary
            }
        }
    }
    Fallback "Diffuse"
}
