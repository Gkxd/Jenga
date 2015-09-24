Shader "Custom Shaders/Stencil" {

SubShader {
    Tags {"Queue"="Geometry-1" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane"}
    LOD 0
    
	ColorMask 0
    ZWrite Off
 
    Pass {
	    Stencil {
			Ref 1
			Comp Always
			Pass Replace
		}
    }
}

}
