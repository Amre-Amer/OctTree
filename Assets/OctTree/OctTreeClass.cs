using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctTreeClass {
    public int numData;
    public OctTreeDataType[] data;
    public OctTreeClass[] octTrees;
    public int lastData;
    public bool isDivided;
    public Vector3 pos;
    public Vector3 sca;
    public CubeType cube;
    public int cntSearch;
    public GlobalClass global;
    public GameObject go;
    public OctTreeClass(Vector3 pos0, Vector3 sca0, GlobalClass global0) {
        global = global0;
        pos = pos0;
        sca = sca0;
        isDivided = false;
        numData = 8;
        data = new OctTreeDataType[numData];
        cube = new CubeType(pos, sca);
        Show();
        global.cntOcts++;
    }
    public void FindRange(CubeType cubeRange, List<OctTreeDataType>results) {
        global.cntSearch++; 
        //if (results.Count > 0) {
        //    return;
        //}
        //Debug.Log(cube.pos + " " + cube.sca + " = " + cubeRange.pos + " " + cubeRange.sca + " results:" + results.Count + " OT:" + go.name + "\n");
        if (CubesIntersect(cube, cubeRange) == false) {
            //Debug.Log("-\n");
            return;
        }
        //global.cntSearch++; 
        for (int d = 0; d < numData; d++) {
            if (cubeRange.Contains(data[d])) {
                results.Add(data[d]);
            }
        }
        if (isDivided == true) {
            for (int n = 0; n < numData; n++) {
                octTrees[n].FindRange(cubeRange, results);
            }
        }
//        Debug.Log("+ results:" + results.Count + " divided:" + isDivided + "\n");
    }
    public bool CubesIntersect(CubeType cube1, CubeType cube2) {
        bool yn = true; 
        if (cube1.pos.x + cube1.sca.x / 2 < cube2.pos.x - cube2.sca.x / 2) 
        {
            yn = false;
        }
        if (cube1.pos.x - cube1.sca.x / 2 > cube2.pos.x + cube2.sca.x / 2)
        {
            yn = false;
        }
        //
        if (cube1.pos.y + cube1.sca.y / 2 < cube2.pos.y - cube2.sca.y / 2)
        {
            yn = false;
        }
        if (cube1.pos.y - cube1.sca.y / 2 > cube2.pos.y + cube2.sca.y / 2)
        {
            yn = false;
        }
        //
        if (cube1.pos.z + cube1.sca.z / 2 < cube2.pos.z - cube2.sca.z / 2)
        {
            yn = false;
        }
        if (cube1.pos.z - cube1.sca.z / 2 > cube2.pos.z + cube2.sca.z / 2)
        {
            yn = false;
        }
        return yn;
    }
    public bool Insert(OctTreeDataType dat) {
        bool result = false;
        if (cube.Contains(dat) == false) {
            return result;
        }
        if (lastData < global.capacityOT) {
            data[lastData] = dat;
            lastData++;
            go.name += " |";
            result = true;
        } else {
            if (isDivided == false) {
                isDivided = true;
                octTrees = new OctTreeClass[numData];
                int n = 0;
                for (int x = -1; x <= 1; x += 2) 
                {
                    for (int y = -1; y <= 1; y += 2)
                    {
                        for (int z = -1; z <= 1; z += 2)
                        {
                            Vector3 posNew = new Vector3(pos.x + x * sca.x / 4, pos.y + y * sca.y / 4, pos.z + z * sca.z / 4);
                            Vector3 scaNew = new Vector3(sca.x / 2, sca.y / 2, sca.z / 2);
                            octTrees[n] = new OctTreeClass(posNew, scaNew, global);
                            n++;
                        }
                    }
                }
            }
            bool isInserted = false;
            for (int n = 0; n < numData; n++)
            {
                if (isInserted == false)
                {
                    isInserted = octTrees[n].Insert(dat);
                    if (isInserted == true) {
                        result = true;
                        break;
                    }
                }
            }
        }
        return result;
    }
    public void Show() {
        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.parent = global.parentOcts.transform;
        go.transform.position = pos;
        go.transform.localScale = sca;
        go.name = "OT " + pos + " " + sca;
        MakeMaterialTransparent(go.GetComponent<Renderer>().material);
        go.GetComponent<Renderer>().material.color = Color.clear;
    }
    void MakeMaterialTransparent(Material material)
    {
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }
}
public struct OctTreeDataType {
    public Vector3 pos;
    public int index;
    public OctTreeDataType(Vector3 pos0, int index0) {
        pos = pos0;
        index = index0;
    }
}
public struct CubeType {
    public Vector3 pos;
    public Vector3 sca;
    public CubeType(Vector3 pos0, Vector3 sca0) {
        pos = pos0;
        sca = sca0;
    }
    public bool Contains(OctTreeDataType dat) {
        bool yn = true;
        if (dat.pos.x < pos.x - sca.x / 2) {
            yn = false;
        }
        if (dat.pos.x > pos.x + sca.x / 2)
        {
            yn = false;
        }
        //
        if (dat.pos.y < pos.y - sca.y / 2)
        {
            yn = false;
        }
        if (dat.pos.y > pos.y + sca.y / 2)
        {
            yn = false;
        }
        //
        if (dat.pos.z < pos.z - sca.z / 2)
        {
            yn = false;
        }
        if (dat.pos.z > pos.z + sca.z / 2)
        {
            yn = false;
        }
        return yn;        
    }
}