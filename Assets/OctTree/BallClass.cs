using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallClass
{
    public GameObject go;
    public Vector3 velocity;
    public float rad;
    public int index;
    public GlobalClass global;
    public BallClass(Vector3 pos, float rad0, Vector3 velocity0, int index0, GlobalClass global0)
    {
        global = global0;
        rad = rad0;
        velocity = velocity0;
        index = index0;
        go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "ball " + index; 
        go.transform.parent = global.parentBalls.transform;
        go.name = "ball";
        go.transform.position = pos;
        go.transform.localScale = new Vector3(rad * 2, rad * 2, rad * 2);
    }
    public void Move()
    {
        go.transform.position += velocity;
        Vector3 normal = IsAtBorder();
        if (normal != Vector3.zero)
        {
            TurnAroundPlane(normal);
        }
        int c = IsColliding();
        if (c > -1)
        {
            TurnAroundBounce(c);
        }
    }
    public void TurnAroundBounce(int c)
    {
        float mag = velocity.magnitude;
        Vector3 vector = go.transform.position - global.balls[c].go.transform.position;
        velocity = Vector3.Normalize(vector) * mag;
    }
    public void TurnAroundPlane(Vector3 normal)
    {
        velocity = Vector3.Reflect(velocity, normal);
    }
    public int IsColliding()
    {
        int result = -1;
        if (global.ynUseOT == true)
        {
            float s = rad * 2;
            Vector3 sca = new Vector3(s, s, s);
            CubeType cubeRange = new CubeType(go.transform.position, sca);
            List<OctTreeDataType> results = new List<OctTreeDataType>();
            global.OT.FindRange(cubeRange, results);
//            global.cntSearch++;
            if (results.Count > 0)
            {
                result = results[0].index;
            }
        }
        else
        {
            for (int b = 0; b < global.balls.Length; b++)
            {
                if (index != b)
                {
                    global.cntSearch++;
                    float dist = Vector3.Distance(go.transform.position, global.balls[b].go.transform.position);
                    float distNear = (go.transform.localScale.x + global.balls[b].go.transform.localScale.x) / 2;
                    if (dist < distNear)
                    {
                        //                        Debug.Log("collide:" + dist + " < " + distNear + "\n");
                        result = b;
                        break;
                    }
                }
            }
        }
        return result;
    }
    public Vector3 IsAtBorder()
    {
        Vector3 normal = Vector3.zero;
        Vector3 pos = go.transform.position;
        if (pos.x < rad)
        {
            normal = Vector3.right;
        }
        if (pos.x > global.maxXYZ.x - rad)
        {
            normal = -1 * Vector3.right;
        }
        if (pos.y < rad)
        {
            normal = Vector3.up;
        }
        if (pos.y > global.maxXYZ.y - rad)
        {
            normal = -1 * Vector3.up;
        }
        if (pos.z < rad)
        {
            normal = Vector3.forward;
        }
        if (pos.z > global.maxXYZ.z - rad)
        {
            normal = -1 * Vector3.forward;
        }
        return normal;
    }
}
