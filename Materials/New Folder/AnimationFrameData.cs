using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct AnimationFrameData : IComponentData
{
    public int currentFrame;
    [HideInInspector]public float lastFrameTime;
    [HideInInspector]public float animationTimeSpend;
    public float animationTimeLength;
    public int totalFrames;
    public int textureStartIndex;
    public int currentAnimationIndex;
}
