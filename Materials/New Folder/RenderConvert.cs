using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using FSG.MeshAnimator.ShaderAnimated;
using Unity.Transforms;

public class RenderConvert : MonoBehaviour, IConvertGameObjectToEntity
{
    public List<ShaderMeshAnimation> meshAnimation = new List<ShaderMeshAnimation>();
    private static Vector4 _shaderTime { get { return Shader.GetGlobalVector("_Time"); } }


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {

        // meshAnimation = Resources.Load<ShaderMeshAnimation>("IdleManBored");
        SetupTextureData();
        Debug.Log(meshAnimation[0]);

        dstManager.AddComponentData<NonUniformScale>(entity, new NonUniformScale { Value = new float3(meshAnimation[0].animScalar) });

        dstManager.AddComponentData<_AnimInfo>(entity, new _AnimInfo
        {
            Value = new float4(
                  0,
                  meshAnimation[0].vertexCount,
                  meshAnimation[0].textureSize.x,
                  meshAnimation[0].textureSize.y
              )
        });
        dstManager.AddComponentData<_AnimTimeInfo>(entity, new _AnimTimeInfo
        {
            Value = new Vector4(
                0,
                meshAnimation[0].TotalFrames,
                _shaderTime.y,
                 _shaderTime.y + (meshAnimation[0].length / (meshAnimation[0].playbackSpeed)))
        });
        dstManager.AddComponentData<_AnimScalar>(entity, new _AnimScalar
        {
            Value = new float3(meshAnimation[0].animScalar)
            // Value = new float3(1, 11, 1)
        });

        dstManager.AddComponentData<_AnimTextureIndex>(entity, new _AnimTextureIndex { Value = 0 });


        dstManager.AddComponent<_CrossfadeAnimInfo>(entity);
        dstManager.AddComponent<_CrossfadeAnimScalar>(entity);
        dstManager.AddComponent<_CrossfadeAnimTextureIndex>(entity);
        dstManager.AddComponent<_CrossfadeStartTime>(entity);
        dstManager.AddComponent<_CrossfadeEndTime>(entity);

        dstManager.AddComponentData<AnimationFrameData>(entity, new AnimationFrameData
        {
            animationTimeSpend = 0f,
            totalFrames = meshAnimation[0].TotalFrames,
            animationTimeLength = meshAnimation[0].length,
        });





    }
    private void SetupTextureData()
    {
        Debug.Log("Texture");



        // if (!_animTextures.ContainsKey(baseMesh))
        // {
        int totalTextures = 0;
        Vector2Int texSize = Vector2Int.zero;
        for (int i = 0; i < meshAnimation.Count; i++)
        {
            var anim = meshAnimation[i];
            totalTextures += anim.textures.Length;
            for (int t = 0; t < anim.textures.Length; t++)
            {
                if (anim.textures[t].width > texSize.x)
                    texSize.x = anim.textures[t].width;

                if (anim.textures[t].height > texSize.y)
                    texSize.y = anim.textures[t].height;
            }


        }



        var textureLimit = QualitySettings.masterTextureLimit;
        QualitySettings.masterTextureLimit = 0;
        var copyTextureSupport = SystemInfo.copyTextureSupport;
        Texture2DArray texture2DArray = new Texture2DArray(texSize.x, texSize.y, totalTextures, meshAnimation[0].textures[0].format, false, false);
        texture2DArray.filterMode = FilterMode.Point;
        DontDestroyOnLoad(texture2DArray);
        int index = 0;
        for (int i = 0; i < meshAnimation.Count; i++)
        {

            var anim = meshAnimation[i];
            for (int t = 0; t < anim.textures.Length; t++)
            {
                var tex = anim.textures[t];
                if (copyTextureSupport != UnityEngine.Rendering.CopyTextureSupport.None)
                {
                    Graphics.CopyTexture(tex, 0, 0, texture2DArray, index, 0);
                }
                else
                {
                    texture2DArray.SetPixels(tex.GetPixels(0), index);
                }
                index++;
            }
            totalTextures += anim.textures.Length;
        }

        if (copyTextureSupport == UnityEngine.Rendering.CopyTextureSupport.None)
        {
            texture2DArray.Apply(true, true);
        }
        // _animTextures.Add(baseMesh, texture2DArray);
        QualitySettings.masterTextureLimit = textureLimit;

        //     _materialCacheLookup.Clear();

        var meshRenderer = GetComponent<MeshRenderer>();
        List<Material> _materialCacheLookup = new List<Material>();
        meshRenderer.GetSharedMaterials(_materialCacheLookup);


        for (int m = 0; m < _materialCacheLookup.Count; m++)
        {
            Material material = _materialCacheLookup[m];
            // if (_setMaterials.Contains(material))
            //     continue;
            Debug.Log("ChangeMAter");
            material.SetTexture("_AnimTextures", texture2DArray);
            // _setMaterials.Add(material);
        }
        // Debug.Log(_animTextures[baseMesh]);

    }







}
