using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadOverhead : AbstractRoad
{
    // Start is called before the first frame update

    public int resolution = 6;
 


    /*
        For straight roads:
            If they're vertical, the path always goes from bottom to up
            If they're horizontal, the path always goes from left to right
     */
    void Awake()
    {
        level = 1;
        int rot = Mathf.RoundToInt(transform.eulerAngles.y);
        while(rot<0){
            rot += 360;
        }
        rot = rot%360;

        gameObject.transform.eulerAngles = new Vector3(0,rot,0);
        _vertical = true;
        if(rot == 0 || rot == 180){
            _vertical = false;
        }
        if(_vertical){
            _path = new Dictionary<string, Path>();
            Path newpath = new Path();
            _path.Add("vertical", newpath);

            Vector3 start_pos = new Vector3(gameObject.transform.position.x,0,gameObject.transform.position.z-3);
            float index_pos = 0;
            float func_pos = 0;
            float y_pos;
            for (int i = 0; i < resolution; i++)
            {
                index_pos = (i+0.0f)/(resolution-1);
                func_pos = index_pos;
                func_pos = func_pos + 1;
                if(func_pos>1.5){
                    func_pos = 3f-func_pos;
                }
                y_pos = (float) (AbstractRoad.tanh(2.6f*func_pos - 2.6f) +  0.964)*1.0375f;
                y_pos = y_pos*2.5f;
                Vector3 gen_pos = start_pos + new Vector3(0,y_pos,6*index_pos);
                PathNode node = new PathNode(gen_pos.x,gen_pos.z,gen_pos.y);
                newpath.AddNode(node);
            }
        }else{
            _path = new Dictionary<string, Path>();
            Path newpath = new Path();
            _path.Add("horizontal", newpath);

            Vector3 start_pos = new Vector3(gameObject.transform.position.x-3,0,gameObject.transform.position.z);
            float index_pos = 0;
            float func_pos = 0;
            float y_pos;
            for (int i = 0; i < resolution; i++)
            {
                index_pos = (i+0.0f)/(resolution-1);
                func_pos = index_pos;
                func_pos = func_pos + 1;
                if(func_pos>1.5){
                    func_pos = 3f-func_pos;
                }
                y_pos = (float) (AbstractRoad.tanh(2.6f*func_pos - 2.6f) +  0.964)*1.0375f;
                y_pos = y_pos*2.5f;
                Vector3 gen_pos = start_pos + new Vector3(6*index_pos,y_pos,0);
                PathNode node = new PathNode(gen_pos.x,gen_pos.z,gen_pos.y);
                newpath.AddNode(node);
            }
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
