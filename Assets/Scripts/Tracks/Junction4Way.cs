using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junction4Way : AbstractRoad
{
    // Start is called before the first frame update
    public int resolution = 6;
    public Animator cylinder;

    /*
        path direction always counter-clockwise
            downwards           -> 0 deg
            leftwards           -> 90 deg
            upwards             -> 180 deg
            rightwards          -> 270 deg

     */
    void Start()
    {
        int rot = Mathf.RoundToInt(transform.eulerAngles.y);
        while(rot<0){
            rot += 360;
        }
        rot = rot%360;

        gameObject.transform.eulerAngles = new Vector3(0,rot,0);




        _path = new Dictionary<string, Path>();
        Path straight_vertical = new Path();
        Path straight_horizontal = new Path();
        Path turn_topright = new Path();
        Path turn_topleft = new Path(); 
        Path turn_bottomright = new Path(); 
        Path turn_bottomleft = new Path(); 

        _path.Add("vertical", straight_vertical);
        _path.Add("horizontal", straight_horizontal);
        _path.Add("tr",turn_topright);
        _path.Add("tl", turn_topleft);
        _path.Add("br", turn_bottomright);
        _path.Add("bl", turn_bottomleft);

        /*                     STRAIGHT PATH                 */

       
            PathNode node1 = new PathNode(gameObject.transform.position.x,gameObject.transform.position.z-3);
            PathNode node2 = new PathNode(gameObject.transform.position.x,gameObject.transform.position.z+3);
            node1.next = node2;
            node2.prev = node1;
            straight_vertical.AddNode(node1);
            straight_vertical.AddNode(node2);
        
            PathNode node3 = new PathNode(gameObject.transform.position.x-3,gameObject.transform.position.z);
            PathNode node4= new PathNode(gameObject.transform.position.x+3,gameObject.transform.position.z);
            node3.next = node4;
            node4.prev = node3;
            straight_horizontal.AddNode(node3);
            straight_horizontal.AddNode(node4);
        

        /*             SEPERATOR             */

        Vector3 center_topl = gameObject.transform.position + new Vector3(-3,0,3);
        Vector3 center_bottoml = gameObject.transform.position + new Vector3(-3,0,-3);
        Vector3 center_bottomr = gameObject.transform.position + new Vector3(3,0,-3);
        Vector3 center_topr = gameObject.transform.position + new Vector3(3,0,3);

        /*             ALL CORNERS        */

        PathNode[] nodes_topleft = new PathNode[resolution];
        PathNode[] nodes_topright = new PathNode[resolution];
        PathNode[] nodes_bottomleft = new PathNode[resolution];
        PathNode[] nodes_bottomright = new PathNode[resolution];
        float max_x = 3.0f;
        float max_y = 3.0f;
        
        //TOPLEFT
        for (int i = 0; i < resolution; i++)
        {
            float degree = Mathf.Deg2Rad*((270)+(90/(resolution-1)*i));
            float x = Mathf.Cos(degree)*max_x;
            float y = Mathf.Sin(degree)*max_y;

            Vector3 pos = center_topl + new Vector3(x,0,y);
                //nodes[i] = new PathNode(pos.x,pos.z);
            nodes_topleft[i] = new PathNode(pos.x,pos.z);
            turn_topleft.AddNode(nodes_topleft[i]);
        }

        
        //TOPRIGHT
        for (int i = 0; i < resolution; i++)
        {
            float degree = Mathf.Deg2Rad*((180)+(90/(resolution-1)*i));
            float x = Mathf.Cos(degree)*max_x;
            float y = Mathf.Sin(degree)*max_y;

            Vector3 pos = center_topr + new Vector3(x,0,y);
                //nodes[i] = new PathNode(pos.x,pos.z);
            nodes_topright[i] = new PathNode(pos.x,pos.z);
            turn_topright.AddNode(nodes_topright[i]);
        }

        //BOTTOMRIGHT
        for (int i = 0; i < resolution; i++)
        {
            float degree = Mathf.Deg2Rad*((90)+(90/(resolution-1)*i));
            float x = Mathf.Cos(degree)*max_x;
            float y = Mathf.Sin(degree)*max_y;

            Vector3 pos = center_bottomr + new Vector3(x,0,y);
                //nodes[i] = new PathNode(pos.x,pos.z);
            nodes_bottomright[i] = new PathNode(pos.x,pos.z);
            turn_bottomright.AddNode(nodes_bottomright[i]);
        }

        //BOTTOMLEFT
        for (int i = 0; i < resolution; i++)
        {
            float degree = Mathf.Deg2Rad*((0)+(90/(resolution-1)*i));
            float x = Mathf.Cos(degree)*max_x;
            float y = Mathf.Sin(degree)*max_y;

            Vector3 pos = center_bottoml + new Vector3(x,0,y);
                //nodes[i] = new PathNode(pos.x,pos.z);
            nodes_bottomleft[i] = new PathNode(pos.x,pos.z);
            turn_bottomleft.AddNode(nodes_bottomleft[i]);
        }


    
        //drawPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void Activate(){
        cylinder.SetBool("active",true);
        Manager.Instance.ActiveJunction = this;
        Manager.Instance.requestInput = true;

        if(has("top",Manager.Instance.CurrentTraverser)){
            cylinder.transform.Find("arrow_up").Find("default").GetComponent<MeshRenderer>().enabled = true;
        }else{
            cylinder.transform.Find("arrow_up").Find("default").GetComponent<MeshRenderer>().enabled = false;
        }
        if(has("left",Manager.Instance.CurrentTraverser)){
            cylinder.transform.Find("arrow_left").Find("default").GetComponent<MeshRenderer>().enabled = true;
        }else{
            cylinder.transform.Find("arrow_left").Find("default").GetComponent<MeshRenderer>().enabled = false;
        }
        if(has("down",Manager.Instance.CurrentTraverser)){
            cylinder.transform.Find("arrow_down").Find("default").GetComponent<MeshRenderer>().enabled = true;
        }else{
            cylinder.transform.Find("arrow_down").Find("default").GetComponent<MeshRenderer>().enabled = false;
        }
        if(has("right",Manager.Instance.CurrentTraverser)){
            cylinder.transform.Find("arrow_right").Find("default").GetComponent<MeshRenderer>().enabled = true;
        }else{
            cylinder.transform.Find("arrow_right").Find("default").GetComponent<MeshRenderer>().enabled = false;
        }
    }

    override public void Wobble(){
        cylinder.SetTrigger("wobble");
        cylinder.SetTrigger("wobble");
    }

    override public void Deactivate(){
        cylinder.SetBool("active",false);
        cylinder.transform.Find("arrow_up").Find("default").GetComponent<MeshRenderer>().enabled = false;
        cylinder.transform.Find("arrow_left").Find("default").GetComponent<MeshRenderer>().enabled = false;
        cylinder.transform.Find("arrow_down").Find("default").GetComponent<MeshRenderer>().enabled = false;
        cylinder.transform.Find("arrow_right").Find("default").GetComponent<MeshRenderer>().enabled = false;
    }


    override public bool has(string direction,Traverser traverser){
        int rot = Mathf.RoundToInt(transform.eulerAngles.y);
        float x_diff = traverser.transform.position.x - transform.position.x;
        float y_diff = traverser.transform.position.z - transform.position.z;
        string relative_dir = "";
        if(y_diff>2){
            relative_dir = "top";
        }else if(y_diff<-2){
            relative_dir = "down";
        }
        if(x_diff>2){
            relative_dir = "right";
        }else if(x_diff<-2){
            relative_dir = "left";
        }
        while(rot<0){
            rot += 360;
        }
        rot = rot%360;
        if(direction.Equals(relative_dir)){
                return false;
        }
       
        return true;
    }
}
