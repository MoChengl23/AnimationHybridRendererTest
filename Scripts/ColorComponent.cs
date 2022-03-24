using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

using FSG.MeshAnimator;
using FSG.MeshAnimator.ShaderAnimated;


// [MaterialProperty("_Color", MaterialPropertyFormat.Float4)]
// [GenerateAuthoringComponent]
// public struct ColorComponent : IComponentData
// {
//    public float4 Value;
// }

[MaterialProperty("_AnimTimeInfo", MaterialPropertyFormat.Float4)]
[GenerateAuthoringComponent]
public struct _AnimTimeInfo : IComponentData
{
    public float4 Value;
}
[MaterialProperty("_AnimInfo", MaterialPropertyFormat.Float4)]
[GenerateAuthoringComponent]
public struct _AnimInfo : IComponentData
{
    public float4 Value;
}
[MaterialProperty("_AnimScalar", MaterialPropertyFormat.Float3)]
[GenerateAuthoringComponent]
public struct _AnimScalar : IComponentData
{
    public float3 Value;
}
[MaterialProperty("_AnimTextureIndex", MaterialPropertyFormat.Float)]
[GenerateAuthoringComponent]
public struct _AnimTextureIndex : IComponentData
{
    public float Value;
}
// [MaterialProperty("_AnimTextures",MaterialPropertyFormat.Float)]
// [GenerateAuthoringComponent]
// public struct _AnimTextures : IComponentData
// {
//    public Texture2DArray Value;
// }

[AlwaysUpdateSystem]
partial class AnimateMyOwnColorSystem : SystemBase
{
    ShaderMeshAnimation meshAnimation;
    protected override void OnCreate()
    {
        meshAnimation = Resources.Load<ShaderMeshAnimation>("Mine");
    }
    protected override void OnUpdate()
    {
         
        var t = Time.ElapsedTime;
        Entities.ForEach((ref _AnimScalar animScalar, ref _AnimInfo animInfo, ref _AnimTextureIndex animTextureIndex, in RenderMesh renderMesh) =>
          {
            //   animScalar.Value = new float3(meshAnimation.animScalar);
            //   animInfo.Value = new float4(
            //       0,
            //       meshAnimation.vertexCount,
            //       meshAnimation.textureSize.x,
            //       meshAnimation.textureSize.y
            //   );
            //   animTextureIndex.Value = 0;
              



          })
          .WithoutBurst().Run();
    }
}