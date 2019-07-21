using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junction : AbstractRoad
{
    // Start is called before the first frame update
    public int resolution = 6;
    public Animator cylinder;

    private static Dictionary<float,string> dir_map = new Dictionary<float,string>(){
        {0,"bl"},
        {90,"tl"},
        {180,"tr"},
        {270,"br"}
    };
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
        Path straight = new Path();
        Path turn1 = new Path(); //LEFT or TOP
        Path turn2 = new Path(); //RIGHT or BOTTOM

        
        string straightkey;
        string turn1key;
        string turn2key;
        if(rot==0){
            turn1key = "bl";
            turn2key = "br";
            straightkey = "horizontal";
        }else if(rot==90){
            turn1key = "tl";
            turn2key = "bl";
            straightkey = "vertical";
        }else if(rot==180){
            turn1key = "tl";
            turn2key = "tr";
            straightkey = "horizontal";
        }else{
            turn1key = "tr";
            turn2key = "br";
            straightkey = "vertical";
        }
        _path.Add(turn1key,turn1);
        _path.Add(turn2key,turn2);
        _path.Add(straightkey, straight);

        /*                     STRAIGHT PATH                 */
        bool vertical = false;
        if(rot==90 || rot==270){
            vertical = true;
        }

        if(vertical){
            PathNode node1 = new PathNode(gameObject.transform.position.x,gameObject.transform.position.z-3);
            PathNode node2 = new PathNode(gameObject.transform.position.x,gameObject.transform.position.z+3);
            node1.next = node2;
            node2.prev = node1;
            straight.AddNode(node1);
            straight.AddNode(node2);
        }else{
            PathNode node1 = new PathNode(gameObject.transform.position.x-3,gameObject.transform.position.z);
            PathNode node2 = new PathNode(gameObject.transform.position.x+3,gameObject.transform.position.z);
            node1.next = node2;
            node2.prev = node1;
            straight.AddNode(node1);
            straight.AddNode(node2);
        }

        /*             SEPERATOR             */

        Vector3 center_topl = gameObject.transform.position + new Vector3(-3,0,3);
        Vector3 center_bottoml = gameObject.transform.position + new Vector3(-3,0,-3);
        Vector3 center_bottomr = gameObject.transform.position + new Vector3(3,0,-3);
        Vector3 center_topr = gameObject.transform.position + new Vector3(3,0,3);

        /*             TURN1 AND TURN2           */
        Vector3 center_turn1;
        float turn1_startangle;
        Vector3 center_turn2;
        float turn2_startangle;
        if(rot == 0){ //FACING DOWN
            center_turn1 = center_bottoml;
            turn1_startangle = 0;
            center_turn2 = center_bottomr;
            turn2_startangle = 90;
        }else if(rot == 90){   //FACING LEFT
            center_turn1 = center_topl;
            turn1_startangle = -90;
            center_turn2 = center_bottoml;
            turn2_startangle = 0;
        }else if(rot == 180){  //FACING TOP
            center_turn1 = center_topl;
            turn1_startangle = -90;
            center_turn2 = center_topr;
            turn2_startangle = -180;
        }else{  //FACING RIGHT
            center_turn1 = center_topr;
            turn1_startangle = -180;
            center_turn2 = center_bottomr;
            turn2_startangle = 90;
        }


        PathNode[] nodes_t1 = new PathNode[resolution];
        PathNode[] nodes_t2 = new PathNode[resolution];
        float max_x = 3.0f;
        float max_y = 3.0f;
        
        //TURN 1
        for (int i = 0; i < resolution; i++)
        {
            float degree = Mathf.Deg2Rad*((turn1_startangle)+(90/(resolution-1)*i));
            float x = Mathf.Cos(degree)*max_x;
            float y = Mathf.Sin(degree)*max_y;

            Vector3 pos = center_turn1 + new Vector3(x,0,y);
                //nodes[i] = new PathNode(pos.x,pos.z);
            nodes_t1[i] = new PathNode(pos.x,pos.z);
            turn1.AddNode(nodes_t1[i]);
        }

        
        //TURN 2
        for (int i = 0; i < resolution; i++)
        {
            float degree = Mathf.Deg2Rad*((turn2_startangle)+(90/(resolution-1)*i));
            float x = Mathf.Cos(degree)*max_x;
            float y = Mathf.Sin(degree)*max_y;

            Vector3 pos = center_turn2 + new Vector3(x,0,y);
                //nodes[i] = new PathNode(pos.x,pos.z);
            nodes_t2[i] = new PathNode(pos.x,pos.z);
            turn2.AddNode(nodes_t2[i]);
        } 
    
        cylinder.gameObject.transform.SetPositionAndRotation(cylinder.gameObject.transform.position,Quaternion.identity);


    
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


     /*
        path direction always counter-clockwise
            downwards           -> 0 deg
            leftwards           -> 90 deg
            upwards             -> 180 deg
            rightwards          -> 270 deg

     */
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
        if(rot == 0){ //downwards
            if(direction.Equals("top")){
                return false;
            }
        }else if(rot == 90){ //leftwards
            if(direction.Equals("right")){
                return false;
            }
        }else if(rot == 180){ //upwards
            if(direction.Equals("down")){
                return false;
            }
        }else if(rot == 270){ //rightwards
            if(direction.Equals("left")){
                return false;
            }
        }
        return true;
    }
}

