using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

 
[MaterialProperty("_Color", MaterialPropertyFormat.Float4)]
[GenerateAuthoringComponent]
public struct ColorComponent : IComponentData
{
   public float4 Value;
}
