using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OctTree : MonoBehaviour
{
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
    public bool ynUseOT;

    // Use this for initialization
    void Start()
    {
        global = new GlobalClass();
        global.maxXYZ = new Vector3(maxX, maxY, maxZ);
        InvokeRepeating("ShowFPS", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (cntFrames > 0) {
            return;
        }
        global.ynUseOT = ynUseOT;
        UpdateBalls();
        cntOcts = global.cntOcts;
        cntFrames++;
        cntFPS++;
    }

    void InitOT() {
        Vector3 pos = global.maxXYZ / 2;
        Vector3 sca = global.maxXYZ;
        OT = new OctTreeClass(pos, sca, global);
        global.OT = OT;
        //
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = pos;
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
                float s = 4;
                Vector3 pos = new Vector3(Random.Range(0, maxX/s), Random.Range(0, maxY/s), Random.Range(0, maxZ/s));
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
    public Vector3 maxXYZ;
    public BallClass[] balls;
    public int cntOcts;
}

