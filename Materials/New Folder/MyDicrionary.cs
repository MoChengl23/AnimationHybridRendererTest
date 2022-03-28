using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSG.MeshAnimator.ShaderAnimated;
public class MyDicrionary : MonoBehaviour
{



    public static Dictionary<string, ShaderMeshAnimation> animationDic = new Dictionary<string, ShaderMeshAnimation>();

    void Start()
    {
        var anim = Resources.Load<ShaderMeshAnimation>("IdleManBored");
        animationDic.Add("IdleManBored", anim);
        anim = Resources.Load<ShaderMeshAnimation>("KeepItemRun");
        animationDic.Add("KeepItemRun", anim);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
