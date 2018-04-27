using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OctTree : MonoBehaviour
{
    public int capacityOT = 8;
    public int numBalls = 10;
    public float radMin = .1f;
    public float radMax = .5f;
    float maxX = 100;
    float maxY = 100;
    float maxZ = 100;
    int cntFrames;
    int cntFPS;
    public int fps;
    OctTreeClass OT;
    GlobalClass global;
    public int cntSearch;
    public int cntOcts;
    public int cntResults;
    public bool ynUseOT;
    public bool ynOnce;

    // Use this for initialization
    void Start()
    {
        //Test();
        //return;
        global = new GlobalClass();
        global.maxXYZ = new Vector3(maxX, maxY, maxZ);
        //global.parentOcts = new GameObject("parentOcts");
        global.parentBalls = new GameObject("parentBalls");
        InvokeRepeating("ShowFPS", 1, 1);
    }

    void Test() {
        CubeType cube = new CubeType(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        OctTreeDataType dat = new OctTreeDataType(new Vector3(.45f, .45f, .45f), 0);
        bool yn = cube.Contains(dat);
        Debug.Log("test: cube:" + cube.pos + " " + cube.sca + " point:" + dat.pos + " = " + yn + "\n");
    }

    // Update is called once per frame
    void Update()
    {
        if (cntFrames > 0) {
            if (ynOnce == true)
            {
                return;
            }
        }
        global.ynUseOT = ynUseOT;
        UpdateTargetBalls();
        UpdateBalls();
        cntOcts = global.cntOcts;
        cntResults = global.cntResults;
        cntFrames++;
        cntFPS++;
    }

    void UpdateTargetBalls() {
        Vector3 posCenter = new Vector3(maxX / 2, maxY / 2, maxZ / 2);
        global.targetBalls.x = posCenter.x + maxX * .35f * Mathf.Cos(cntFrames * Mathf.Deg2Rad);
        global.targetBalls.y = posCenter.y + maxY * .15f;
        global.targetBalls.z = posCenter.z + maxX * .35f * Mathf.Sin(cntFrames * Mathf.Deg2Rad);
    }

    void InitOT() {
        global.cntOcts = 0;
        if (global.parentOcts != null) DestroyImmediate(global.parentOcts);
        global.parentOcts = new GameObject("parentOcts");
        Vector3 pos = global.maxXYZ / 2;
        Vector3 sca = global.maxXYZ;
        global.capacityOT = capacityOT;
        OT = new OctTreeClass(pos, sca, global);
        global.OT = OT;
        //
        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //go.transform.position = pos;
    }

    void LoadOT() {
        for (int b = 0; b < numBalls; b++) {
            OctTreeDataType dat = new OctTreeDataType(global.balls[b].go.transform.position, b);
            OT.Insert(dat);
        }
    }

    void InitAndLoadOT() {
        InitOT();
        LoadOT();
    }

    void ShowFPS() {
        fps = cntFPS;
        cntFPS = 0;
    }

    void UpdateBalls()
    {
        if (global.balls == null)
        {
            global.balls = new BallClass[numBalls];
        }
        for (int b = 0; b < numBalls; b++) {
            if (global.balls[b] == null) {
                float s = 1;
                Vector3 pos = new Vector3(Random.Range(0, maxX / s), Random.Range(0, maxY / s), Random.Range(0, maxZ / s));
                float rad = Random.Range(radMin, radMax);
                Vector3 velocity = Random.insideUnitSphere * Random.Range(.1f, .5f);
                global.balls[b] = new BallClass(pos, rad, velocity, b, global);
            }
        }
        if (global.ynUseOT == true) {
            InitAndLoadOT();
        }
        global.cntSearch = 0; 
        for (int b = 0; b < numBalls; b++)
        {
            global.balls[b].Move();
        }
        cntSearch = global.cntSearch;
    }


}
public class GlobalClass
{
    public int cntSearch;
    public bool ynUseOT;
    public OctTreeClass OT;
    public int capacityOT;
    public Vector3 maxXYZ;
    public BallClass[] balls;
    public int cntOcts;
    public GameObject parentOcts;
    public GameObject parentBalls;
    public Vector3 targetBalls;
    public int cntResults;
}

