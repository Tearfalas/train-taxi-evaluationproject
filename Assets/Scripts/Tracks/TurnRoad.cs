using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRoad : AbstractRoad
{
    // Start is called before the first frame update
    public int resolution = 6;
    
    private static Dictionary<float,string> dir_map = new Dictionary<float,string>(){
        {0,"bl"},
        {90,"tl"},
        {180,"tr"},
        {270,"br"}
    };
    /*
        path direction always counter-clockwise
            bottom<->left           -> 0 deg
            top<->left              -> 90 deg
            top<->right             -> 180 deg
            bottom<->right          -> 270 deg

     */
    void Awake()
    {
        int rot = Mathf.RoundToInt(transform.eulerAngles.y);
        while(rot<0){
            rot += 360;
        }
        rot = rot%360;

        gameObject.transform.eulerAngles = new Vector3(0,rot,0);

        /*
        if(rot == 0){
            PathNode[] nodes = new PathNode[resolution];
            float max_x = 3.0f;
            float max_y = 3.0f;
            Vector3 center = gameObject.transform.position + new Vector3(-3,0,-3);
            for (int i = 0; i < resolution; i++)
            {
                float degree = Mathf.Deg2Rad*(90/(resolution-1)*i);
                float x = Mathf.Cos(degree)*max_x;
                float y = Mathf.Sin(degree)*max_y;

                Vector3 pos = center + new Vector3(x,0,y);
                nodes[i] = new PathNode(Mathf.RoundToInt(pos.x),Mathf.RoundToInt(pos.y));
            }
        }else if(rot == 90){
            PathNode[] nodes = new PathNode[resolution];
            float max_x = 3.0f;
            float max_y = 3.0f;
            Vector3 center = gameObject.transform.position + new Vector3(-3,0,3);
            for (int i = 0; i < resolution; i++)
            {
                float degree = Mathf.Deg2Rad*(270+(90/(resolution-1)*i));
                float x = Mathf.Cos(degree)*max_x;
                float y = Mathf.Sin(degree)*max_y;

                Vector3 pos = center + new Vector3(x,0,y);
                nodes[i] = new PathNode(Mathf.RoundToInt(pos.x),Mathf.RoundToInt(pos.y));
            }
        }else if(rot == 180){
            PathNode[] nodes = new PathNode[resolution];
            float max_x = 3.0f;
            float max_y = 3.0f;
            Vector3 center = gameObject.transform.position + new Vector3(-3,0,3);
            for (int i = 0; i < resolution; i++)
            {
                float degree = Mathf.Deg2Rad*(180+(90/(resolution-1)*i));
                float x = Mathf.Cos(degree)*max_x;
                float y = Mathf.Sin(degree)*max_y;

                Vector3 pos = center + new Vector3(x,0,y);
                nodes[i] = new PathNode(Mathf.RoundToInt(pos.x),Mathf.RoundToInt(pos.y));
            }
        }else if(rot == 270){
            PathNode[] nodes = new PathNode[resolution];
            float max_x = 3.0f;
            float max_y = 3.0f;
            Vector3 center = gameObject.transform.position + new Vector3(-3,0,3);
            for (int i = 0; i < resolution; i++)
            {
                float degree = Mathf.Deg2Rad*(90+(90/(resolution-1)*i));
                float x = Mathf.Cos(degree)*max_x;
                float y = Mathf.Sin(degree)*max_y;

                Vector3 pos = center + new Vector3(x,0,y);
                nodes[i] = new PathNode(Mathf.RoundToInt(pos.x),Mathf.RoundToInt(pos.y));
            }
        } */

        _path = new Dictionary<string, Path>();
        Path newpath = new Path();
        _path.Add(dir_map[rot],newpath);
        PathNode[] nodes = new PathNode[resolution];
        float max_x = 3.0f;
        float max_y = 3.0f;
        Vector3 center;
        if(rot == 0){
            center = gameObject.transform.position + new Vector3(-3,0,-3);
        }else if(rot == 90){
            center = gameObject.transform.position + new Vector3(-3,0,3);
        }else if(rot == 180){
            center = gameObject.transform.position + new Vector3(3,0,3);
        }else{
            center = gameObject.transform.position + new Vector3(3,0,-3);
        }


        for (int i = 0; i < resolution; i++)
        {
            float degree = Mathf.Deg2Rad*((360-rot)+(90/(resolution-1)*i));
            float x = Mathf.Cos(degree)*max_x;
            float y = Mathf.Sin(degree)*max_y;

            Vector3 pos = center + new Vector3(x,0,y);
            //nodes[i] = new PathNode(pos.x,pos.z);
            nodes[i] = new PathNode(pos.x,pos.z);
            newpath.AddNode(nodes[i]);
        }
        //drawPath();
    }

    override public void Activate(){

    }
    override public void Deactivate(){

    }
    override public void Wobble(){
    }

     override public bool has(string direction,Traverser traverser){
        
        return true;
    }
}
