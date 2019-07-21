using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRoad : MonoBehaviour
{
    protected Dictionary<string, Path> _path;
    public GameObject pointPrefab;
    public int level = 0;

    protected bool _vertical;
 
    public bool vertical {
        get {
            return _vertical;
        }    
    }

    public Dictionary<string, Path>  path{
        get{
            return _path;
        }
    }

    public void drawPath(){
        foreach(KeyValuePair<string,Path> temp in _path){
            PathNode curr = temp.Value.front;
            if(curr == null){
                return;
            }
            Point newpoint = Instantiate(pointPrefab, new Vector3(curr.x, curr.z + 1, curr.y), Quaternion.identity).GetComponent<Point>();
            int count = 1;
            newpoint.SetNum(0);
            while(curr.hasNext()){
                curr = curr.next;
                newpoint = Instantiate(pointPrefab, new Vector3(curr.x,  curr.z + 1, curr.y), Quaternion.identity).GetComponent<Point>();
                newpoint.SetNum(count);
                count++;
            }
        }
    }
    public abstract void Activate();
    public abstract void Deactivate();

    public abstract void Wobble();

    public abstract bool has(string direction,Traverser traverser);

    public static float tanh(float x){
        return (Mathf.Exp(2*x) - 1)/((Mathf.Exp(2*x) + 1));
    }
}
