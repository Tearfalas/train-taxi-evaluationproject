using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadUpper : AbstractRoad
{
    // Start is called before the first frame update
  

    /*
        For straight roads:
            If they're vertical, the path always goes from bottom to up
            If they're horizontal, the path always goes from left to right
     */
    void Awake()
    {
        level = 1;
        if((Mathf.RoundToInt(transform.eulerAngles.y))%180 == 0){
            gameObject.transform.eulerAngles = new Vector3(0,0,0);
            _vertical = true;
        }else{
            gameObject.transform.eulerAngles = new Vector3(0,90,0);
            _vertical = false;
        }
        float y_pos = (float) (AbstractRoad.tanh(2.6f*2 - 2.6f) +  0.964)*1.0375f;
        y_pos = y_pos*2.5f;
        if(_vertical){
            PathNode node1 = new PathNode(gameObject.transform.position.x,gameObject.transform.position.z-3,y_pos);
            PathNode node2 = new PathNode(gameObject.transform.position.x,gameObject.transform.position.z+3,y_pos);
            node1.next = node2;
            node2.prev = node1;
            _path = new Dictionary<string, Path>();
            Path newpath = new Path();
            newpath.AddNode(node1);
            newpath.AddNode(node2);
            _path.Add("vertical", newpath);
        }else{
            PathNode node1 = new PathNode(gameObject.transform.position.x-3,gameObject.transform.position.z,y_pos);
            PathNode node2 = new PathNode(gameObject.transform.position.x+3,gameObject.transform.position.z,y_pos);
            node1.next = node2;
            node2.prev = node1;
            _path = new Dictionary<string, Path>();
            Path newpath = new Path();
            newpath.AddNode(node1);
            newpath.AddNode(node2);
            _path.Add("horizontal", newpath);
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
