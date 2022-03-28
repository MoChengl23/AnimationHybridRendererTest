using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using FSG.MeshAnimator.ShaderAnimated;
using Unity.Mathematics;
using FSG.MeshAnimator.ShaderAnimated;

[AlwaysUpdateSystem]
public partial class TestSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    private Vector4 _shaderTime { get { return Shader.GetGlobalVector("_Time"); } }
    public int ECSFrame;
    //  
    protected override void OnStartRunning()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        var ecb = endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
        Entities.ForEach((Entity entity, in NonUniformScale scale) =>
        {
            Debug.Log("ref");
            ecb.AddComponent<Scale>(entity, new Scale { });
        }).WithoutBurst().Run();
    }
    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var anim = Resources.Load<ShaderMeshAnimation>("KeepItemRun");
            Crossfade(anim);
        }
        UpdateAnimationFrame();

    }
    private void Play()
    {

    }
    private void OnCurrentAnimationChanged()
    {

    }
    private void UpdateAnimationFrame()
    {
        Entities.ForEach((ref AnimationFrameData animationFrameData) =>
        {

            float currentTime = UnityEngine.Time.time;
            animationFrameData.animationTimeSpend += currentTime - animationFrameData.lastFrameTime;
            if (animationFrameData.animationTimeSpend > animationFrameData.animationTimeLength)
            {
                animationFrameData.animationTimeSpend -= animationFrameData.animationTimeLength;
            }




            float normalizedTime = animationFrameData.animationTimeSpend / animationFrameData.animationTimeLength;


            var totalFrames = animationFrameData.totalFrames;
            animationFrameData.currentFrame = math.min((int)math.round(normalizedTime * totalFrames), totalFrames - 1);

            animationFrameData.lastFrameTime = currentTime;
            ECSFrame = animationFrameData.currentFrame;

            //exposedTransform
            //TODO
        }).WithoutBurst().Run();
    }


    private void Crossfade(ShaderMeshAnimation newAnimation)
    {
        Entities.ForEach((ref _AnimInfo animInfo,
                        ref _CrossfadeAnimTextureIndex crossfadeAnimTextureIndex,
                        // ref NonUniformScale scale,
                        ref _CrossfadeAnimInfo crossfadeAnimInfo,
                        ref _CrossfadeStartTime crossfadeStartTime,
                        ref _CrossfadeEndTime crossfadeEndTime,
                     ref AnimationFrameData animationFrameData,
                     ref _AnimTextureIndex animTextureIndex,
                       ref _AnimTimeInfo animTimeInfo










                     ) =>
        {
            var oldAnimationInfo = animInfo.Value;
            float4 crossfadeInfo = new float4(oldAnimationInfo);
            var currentFrame = animationFrameData.currentFrame;
            var textureStartIndex = animationFrameData.textureStartIndex;

            int framesPerTexture = (int)((oldAnimationInfo.z * oldAnimationInfo.w) / (oldAnimationInfo.y * 2));
            int localOffset = (int)(currentFrame / (float)framesPerTexture);
            int textureIndex = textureStartIndex + localOffset;
            int frameOffset = (int)(currentFrame % framesPerTexture);
            int pixelOffset = (int)oldAnimationInfo.y * 2 * frameOffset;

            crossfadeInfo.x = pixelOffset;
            var shaderTime = _shaderTime;



            crossfadeAnimTextureIndex.Value = textureIndex;
            //   scale.Value = 
            crossfadeAnimInfo.Value = crossfadeInfo;
            crossfadeStartTime.Value = shaderTime.y;
            crossfadeEndTime.Value = shaderTime.y + 0.1f;
            //exposedTransform
            //TODO


            // Play(newAnimation);

            animationFrameData.currentFrame = 0;
            animationFrameData.animationTimeSpend = 0;
            animationFrameData.currentAnimationIndex = 1;
            animationFrameData.animationTimeLength = newAnimation.length;
            animationFrameData.totalFrames = newAnimation.TotalFrames;
            //OnCurrentAnimationChange



            var temp = animInfo.Value;
            temp.x = 2;
            temp.z = 1024;
            temp.w = 1024;
            animInfo.Value = temp;
            animTextureIndex.Value = 2;

            var timeBlockData = new float4(
                0,
                animationFrameData.totalFrames,
                _shaderTime.y,
                _shaderTime.y + (animationFrameData.animationTimeLength));



            animTimeInfo.Value = timeBlockData;














        }).WithoutBurst().Run();
    }

    private void Play(ShaderMeshAnimation newAnimation)
    {

    }
}
