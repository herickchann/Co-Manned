#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'

// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:True,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32645,y:32966|diff-39-OUT,spec-21-OUT,gloss-32-OUT,normal-8-RGB,emission-71-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33442,y:32506,ptlb:Main Tex,ptin:_MainTex,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3,x:33495,y:32698,ptlb:Custom Specular,ptin:_CustomSpecular,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4,x:33474,y:32890,ptlb:Custom Gloss,ptin:_CustomGloss,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:5,x:33464,y:32333,ptlb:Color,ptin:_Color,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:6,x:33656,y:32618,ptlb:Spec Color,ptin:_SpecColor,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:7,x:33464,y:33082,ptlb:Illum,ptin:_Illum,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:8,x:33807,y:32811,ptlb:Bump Map,ptin:_BumpMap,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Color,id:13,x:33464,y:33354,ptlb:Illum Color,ptin:_IllumColor,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:14,x:33464,y:33276,ptlb:Illum Power,ptin:_IllumPower,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:15,x:33113,y:33130|A-7-A,B-14-OUT,C-13-RGB;n:type:ShaderForge.SFN_SwitchProperty,id:20,x:33328,y:32682,ptlb:Custom Spec Map,ptin:_CustomSpecMap,on:False|A-2-A,B-3-A;n:type:ShaderForge.SFN_Multiply,id:21,x:33182,y:32717|A-6-RGB,B-20-OUT,C-23-OUT,D-44-OUT;n:type:ShaderForge.SFN_ValueProperty,id:23,x:33667,y:32810,ptlb:Spec Power,ptin:_SpecPower,glob:False,v1:1;n:type:ShaderForge.SFN_SwitchProperty,id:30,x:33231,y:32883,ptlb:Use Gloss Map,ptin:_UseGlossMap,on:False|A-31-OUT,B-4-A;n:type:ShaderForge.SFN_Vector1,id:31,x:33636,y:32941,v1:1;n:type:ShaderForge.SFN_Multiply,id:32,x:33051,y:32838|A-30-OUT,B-33-OUT;n:type:ShaderForge.SFN_Slider,id:33,x:33178,y:33024,ptlb:Shininess,ptin:_Shininess,min:0,cur:1,max:2;n:type:ShaderForge.SFN_Multiply,id:39,x:33212,y:32492|A-5-RGB,B-2-RGB,C-44-OUT;n:type:ShaderForge.SFN_Lerp,id:40,x:32439,y:32822|A-41-A,B-42-OUT,T-43-OUT;n:type:ShaderForge.SFN_Tex2d,id:41,x:32439,y:32971,ptlb:Ambient Oclusion,ptin:_AmbientOclusion,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:42,x:32439,y:32753,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:43,x:32439,y:33144,ptlb:AO Power,ptin:_AOPower,glob:False,v1:1;n:type:ShaderForge.SFN_SwitchProperty,id:44,x:32256,y:32789,ptlb:AO Enabled,ptin:_AOEnabled,on:False|A-42-OUT,B-40-OUT;n:type:ShaderForge.SFN_Cubemap,id:57,x:33132,y:33377,ptlb:Reflection,ptin:_Reflection;n:type:ShaderForge.SFN_Multiply,id:58,x:32956,y:33436|A-57-RGB,B-59-A,C-60-OUT,D-77-RGB;n:type:ShaderForge.SFN_Tex2d,id:59,x:33132,y:33552,ptlb:Reflection Mask,ptin:_ReflectionMask,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:60,x:33132,y:33740,ptlb:Reflection Power,ptin:_ReflectionPower,glob:False,v1:1;n:type:ShaderForge.SFN_Add,id:71,x:32917,y:33184|A-15-OUT,B-58-OUT;n:type:ShaderForge.SFN_Color,id:77,x:33132,y:33821,ptlb:Reflection Color,ptin:_ReflectionColor,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:5-2-8-6-23-20-3-33-30-4-13-7-14-57-59-60-77-44-41-43;pass:END;sub:END;*/

Shader "DLNK/Reflective/BumpReflectionLight" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Main Tex", 2D) = "white" {}
        _BumpMap ("Bump Map", 2D) = "bump" {}
        _SpecColor ("Spec Color", Color) = (0.5,0.5,0.5,1)
        _SpecPower ("Spec Power", Float ) = 1
        [MaterialToggle] _CustomSpecMap ("Custom Spec Map", Float ) = 1
        _CustomSpecular ("Custom Specular", 2D) = "white" {}
        _Shininess ("Shininess", Range(0, 2)) = 1
        [MaterialToggle] _UseGlossMap ("Use Gloss Map", Float ) = 1
        _CustomGloss ("Custom Gloss", 2D) = "white" {}
        _IllumColor ("Illum Color", Color) = (0.5,0.5,0.5,1)
        _Illum ("Illum", 2D) = "white" {}
        _IllumPower ("Illum Power", Float ) = 1
        _Reflection ("Reflection", Cube) = "_Skybox" {}
        _ReflectionMask ("Reflection Mask", 2D) = "white" {}
        _ReflectionPower ("Reflection Power", Float ) = 1
        _ReflectionColor ("Reflection Color", Color) = (0.5,0.5,0.5,1)
        [MaterialToggle] _AOEnabled ("AO Enabled", Float ) = 1
        _AmbientOclusion ("Ambient Oclusion", 2D) = "white" {}
        _AOPower ("AO Power", Float ) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _CustomSpecular; uniform float4 _CustomSpecular_ST;
            uniform sampler2D _CustomGloss; uniform float4 _CustomGloss_ST;
            uniform float4 _Color;
            uniform float4 _SpecColor;
            uniform sampler2D _Illum; uniform float4 _Illum_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float4 _IllumColor;
            uniform float _IllumPower;
            uniform fixed _CustomSpecMap;
            uniform float _SpecPower;
            uniform fixed _UseGlossMap;
            uniform float _Shininess;
            uniform sampler2D _AmbientOclusion; uniform float4 _AmbientOclusion_ST;
            uniform float _AOPower;
            uniform fixed _AOEnabled;
            uniform samplerCUBE _Reflection;
            uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
            uniform float _ReflectionPower;
            uniform float4 _ReflectionColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                float3 shLight : TEXCOORD7;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.shLight = ShadeSH9(float4(mul(_Object2World, float4(v.normal,0)).xyz * 1.0,1)) * 0.5;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_91 = i.uv0;
                float3 node_8 = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(node_91.rg, _BumpMap)));
                float3 normalLocal = node_8.rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
////// Emissive:
                float4 node_57 = texCUBE(_Reflection,viewReflectDirection);
                float3 emissive = ((tex2D(_Illum,TRANSFORM_TEX(node_91.rg, _Illum)).a*_IllumPower*_IllumColor.rgb)+(node_57.rgb*tex2D(_ReflectionMask,TRANSFORM_TEX(node_91.rg, _ReflectionMask)).a*_ReflectionPower*_ReflectionColor.rgb));
///////// Gloss:
                float gloss = (lerp( 1.0, tex2D(_CustomGloss,TRANSFORM_TEX(node_91.rg, _CustomGloss)).a, _UseGlossMap )*_Shininess);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float4 node_2 = tex2D(_MainTex,TRANSFORM_TEX(node_91.rg, _MainTex));
                float node_42 = 1.0;
                float node_44 = lerp( node_42, lerp(tex2D(_AmbientOclusion,TRANSFORM_TEX(node_91.rg, _AmbientOclusion)).a,node_42,_AOPower), _AOEnabled );
                float3 specularColor = (_SpecColor.rgb*lerp( node_2.a, tex2D(_CustomSpecular,TRANSFORM_TEX(node_91.rg, _CustomSpecular)).a, _CustomSpecMap )*_SpecPower*node_44);
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                diffuseLight += i.shLight; // Per-Vertex Light Probes / Spherical harmonics
                finalColor += diffuseLight * (_Color.rgb*node_2.rgb*node_44);
                finalColor += specular;
                finalColor += emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _CustomSpecular; uniform float4 _CustomSpecular_ST;
            uniform sampler2D _CustomGloss; uniform float4 _CustomGloss_ST;
            uniform float4 _Color;
            uniform float4 _SpecColor;
            uniform sampler2D _Illum; uniform float4 _Illum_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float4 _IllumColor;
            uniform float _IllumPower;
            uniform fixed _CustomSpecMap;
            uniform float _SpecPower;
            uniform fixed _UseGlossMap;
            uniform float _Shininess;
            uniform sampler2D _AmbientOclusion; uniform float4 _AmbientOclusion_ST;
            uniform float _AOPower;
            uniform fixed _AOEnabled;
            uniform samplerCUBE _Reflection;
            uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
            uniform float _ReflectionPower;
            uniform float4 _ReflectionColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_92 = i.uv0;
                float3 node_8 = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(node_92.rg, _BumpMap)));
                float3 normalLocal = node_8.rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
///////// Gloss:
                float gloss = (lerp( 1.0, tex2D(_CustomGloss,TRANSFORM_TEX(node_92.rg, _CustomGloss)).a, _UseGlossMap )*_Shininess);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float4 node_2 = tex2D(_MainTex,TRANSFORM_TEX(node_92.rg, _MainTex));
                float node_42 = 1.0;
                float node_44 = lerp( node_42, lerp(tex2D(_AmbientOclusion,TRANSFORM_TEX(node_92.rg, _AmbientOclusion)).a,node_42,_AOPower), _AOEnabled );
                float3 specularColor = (_SpecColor.rgb*lerp( node_2.a, tex2D(_CustomSpecular,TRANSFORM_TEX(node_92.rg, _CustomSpecular)).a, _CustomSpecMap )*_SpecPower*node_44);
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * (_Color.rgb*node_2.rgb*node_44);
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
