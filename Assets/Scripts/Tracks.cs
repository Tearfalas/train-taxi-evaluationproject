using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour
{

    //DEBUG
    public GameObject cube;
    //
    private List<Transform> tracks;

    public AbstractRoad startJunction;
    public Vector2 start_dir;
    public Traverser traverser;
    // Start is called before the first frame update
    void Start()
    {
        Manager.Instance.CurrentTracks = this;
        updateTrack();

        traverser.transform.position = startJunction.transform.position + new Vector3(start_dir.x*4,1,start_dir.y*4);

        traverser.transform.eulerAngles = new Vector3(0,Vector2.Angle(start_dir,new Vector2(0,-1)),0);

        startJunction.Activate();

        
        
        traverser.paused = false;

        /*
        foreach (var trform in tracks)
        {
            if (trform.gameObject.CompareTag("straight_road")){
                Road road = trform.gameObject.GetComponent<Road>();
                GameObject road_object = trform.gameObject;
                if(road.vertical){
                    PathNode node1 = new PathNode(Mathf.RoundToInt(trform.position.x),Mathf.RoundToInt(trform.position.y-3));
                    PathNode node2 = new PathNode(Mathf.RoundToInt(trform.position.x),Mathf.RoundToInt(trform.position.y+3));
                    node1.next = node2;
                    node2.prev = node1;
                }else{
                    PathNode node1 = new PathNode(Mathf.RoundToInt(trform.position.x-3),Mathf.RoundToInt(trform.position.y));
                    PathNode node2 = new PathNode(Mathf.RoundToInt(trform.position.x+3),Mathf.RoundToInt(trform.position.y));
                }
            }
        } */
    }

    public void updateTrack(){
        tracks = new List<Transform>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            tracks.Add(gameObject.transform.GetChild(i));
        }
    }


    public void AddPathAndCombine(AbstractRoad junc,Traverser traverser,string picked_dir){
        float y_diff = traverser.gameObject.transform.position.z - junc.gameObject.transform.position.z;
        float x_diff = traverser.gameObject.transform.position.x - junc.gameObject.transform.position.x;
        string relative_dir = "";
        if(y_diff>1){
            relative_dir = "above";
        }else if(y_diff<-1){
            relative_dir = "below";
        }
        if(x_diff>1){
            relative_dir = "right";
        }else if(x_diff<-1){
            relative_dir = "left";
        }
        Vector3 lookVector = new Vector3();
        if(y_diff>2){
            lookVector = new Vector3(0,0,-1);
        }else if(y_diff<-2){
            lookVector = new Vector3(0,0,1);
        }
        if(x_diff>2){
            lookVector = new Vector3(-1,0,0);
        }else if(x_diff<-2){
            lookVector = new Vector3(1,0,0);
        }
        Vector3 targetVector = new Vector3();
        if(picked_dir.Equals("top")){
            targetVector = new Vector3(0,0,1);
        }else if(picked_dir.Equals("down")){
            targetVector = new Vector3(0,0,-1);
        }else if(picked_dir.Equals("right")){
            targetVector = new Vector3(1,0,0);
        }else if(picked_dir.Equals("left")){
            targetVector = new Vector3(-1,0,0);
        }
        float sgn = Vector3.Cross(lookVector,targetVector).y;

        Vector2 trv_dir = new Vector2();
        if(junc.CompareTag("junction_4way")){
            string key = "";
            bool opposite = false;
            if((relative_dir.Equals("below") || relative_dir.Equals("above"))&&(picked_dir.Equals("top") || picked_dir.Equals("down"))){
                key = "vertical";
            }
            if((relative_dir.Equals("right") || relative_dir.Equals("left"))&&(picked_dir.Equals("right") || picked_dir.Equals("left"))){
                key = "horizontal";
            }
        
            //Debug.Log(lookVector+" "+targetVector);
            if(key==""){  // NOT VERTICAL OR HORIZONTAL
                string prefix = "";
                string suffix = "";
                if(relative_dir.Equals("below")){
                    prefix = "b";
                }
                else if(relative_dir.Equals("above")){
                    prefix = "t";
                }
                else if(relative_dir.Equals("right")){
                    suffix = "r";
                }
                else if(relative_dir.Equals("left")){
                    suffix = "l";
                }

                if(picked_dir.Equals("top")){
                    prefix = "t";
                }else if(picked_dir.Equals("down")){
                    prefix = "b";
                }else if(picked_dir.Equals("right")){
                    suffix = "r";
                }else if(picked_dir.Equals("left")){
                    suffix = "l";
                }

                key = prefix + suffix;
            }
            if(key.Equals("vertical")){
                if(picked_dir.Equals("down")){
                    opposite = true;
                }
            }
            if(key.Equals("horizontal")){
                if(picked_dir.Equals("left")){
                    opposite = true;
                }
            }
            if(sgn>0){
                opposite = true;
            }
            traverser.AddPath(junc.path[key], opposite);
        }else{
            int rot = Mathf.RoundToInt(junc.transform.eulerAngles.y);
            if(rot == 0){  //downwards
                string key = "";
                bool opposite = false;
                if( (relative_dir.Equals("left") || relative_dir.Equals("right")) && (picked_dir.Equals("left") || picked_dir.Equals("right"))){
                    key = "horizontal";
                }
                if (key.Equals("")){
                    if(relative_dir.Equals("left")){
                        key = "bl";
                    }
                    if(relative_dir.Equals("right")){
                        key = "br";
                    }
                    if(relative_dir.Equals("below")){
                        if(picked_dir.Equals("right")){
                            key = "br";
                        }else{
                            key = "bl";
                        }
                    }
                }
                if(key.Equals("vertical")){
                    if(picked_dir.Equals("down")){
                        opposite = true;
                    }
                }
                if(key.Equals("horizontal")){
                    if(picked_dir.Equals("left")){
                        opposite = true;
                    }
                }
                if(sgn>0){
                    opposite = true;
                }
                traverser.AddPath(junc.path[key], opposite);
            }else if(rot == 90){  //leftwards
                string key = "";
                bool opposite = false;
                if( (relative_dir.Equals("above") || relative_dir.Equals("below")) && (picked_dir.Equals("top") || picked_dir.Equals("down"))){
                    key = "vertical";
                }
                if (key.Equals("")){
                    if(relative_dir.Equals("below")){
                        key = "bl";
                    }
                    if(relative_dir.Equals("above")){
                        key = "tl";
                    }
                    if(relative_dir.Equals("left")){
                        if(picked_dir.Equals("top")){
                            key = "tl";
                        }else{
                            key = "bl";
                        }
                    }
                }
                if(key.Equals("vertical")){
                    if(picked_dir.Equals("down")){
                        opposite = true;
                    }
                }
                if(key.Equals("horizontal")){
                    if(picked_dir.Equals("left")){
                        opposite = true;
                    }
                }
                if(sgn>0){
                    opposite = true;
                }
                traverser.AddPath(junc.path[key], opposite);
            }else if(rot == 180){  //upwards
                string key = "";
                bool opposite = false;
                if( (relative_dir.Equals("left") || relative_dir.Equals("right")) && (picked_dir.Equals("left") || picked_dir.Equals("right"))){
                    key = "horizontal";
                }
                if (key.Equals("")){
                    if(relative_dir.Equals("left")){
                        key = "tl";
                    }
                    if(relative_dir.Equals("right")){
                        key = "tr";
                    }
                    if(relative_dir.Equals("above")){
                        if(picked_dir.Equals("right")){
                            key = "tr";
                        }else{
                            key = "tl";
                        }
                    }
                }
                if(key.Equals("vertical")){
                    if(picked_dir.Equals("down")){
                        opposite = true;
                    }
                }
                if(key.Equals("horizontal")){
                    if(picked_dir.Equals("left")){
                        opposite = true;
                    }
                }
                if(sgn>0){
                    opposite = true;
                }
                traverser.AddPath(junc.path[key], opposite);
            }else if(rot == 270){  //rightwards
                string key = "";
                bool opposite = false;
                if( (relative_dir.Equals("above") || relative_dir.Equals("below")) && (picked_dir.Equals("top") || picked_dir.Equals("down"))){
                    key = "vertical";
                }
                if (key.Equals("")){
                    if(relative_dir.Equals("below")){
                        key = "br";
                    }
                    if(relative_dir.Equals("above")){
                        key = "tr";
                    }
                    if(relative_dir.Equals("right")){
                        if(picked_dir.Equals("top")){
                            key = "tr";
                        }else{
                            key = "br";
                        }
                    }
                }
                if(key.Equals("vertical")){
                    if(picked_dir.Equals("down")){
                        opposite = true;
                    }
                }
                if(key.Equals("horizontal")){
                    if(picked_dir.Equals("left")){
                        opposite = true;
                    }
                }
                if(sgn>0){
                    opposite = true;
                }
                traverser.AddPath(junc.path[key], opposite);
            }
        }
    
        if(picked_dir.Equals("top")){
            trv_dir.y = 1;
        }else if(picked_dir.Equals("down")){
            trv_dir.y = -1;
        }else if(picked_dir.Equals("right")){
            trv_dir.x = 1;
        }else if(picked_dir.Equals("left")){
            trv_dir.x = -1;
        }

        //THE JUNCTION PATH HAS BEEN PICKED
        //DIRECTION HAS BEEN PICKED
        //NEED TO COMPUTE REST OF THE PATH
        Vector3 check_pos = junc.transform.position + new Vector3(trv_dir.x,0,trv_dir.y)*6;
        AbstractRoad next_road = getRoadAt(check_pos,0);
        
        var count = 0;
        while(!next_road.gameObject.CompareTag("junction_4way") && !next_road.gameObject.CompareTag("junction_3way"))
        {
            if(count>50){
                Debug.LogError("Infinite loop");
                break;
            }
            foreach(KeyValuePair<string,Path> temp in next_road.path)
            {   
                bool opposite = true;
                if(next_road.gameObject.CompareTag("straight_road") || next_road.gameObject.CompareTag("road_upper")
                || next_road.gameObject.CompareTag("road_rise_upper") || next_road.gameObject.CompareTag("road_rise_lower")){
                   // Debug.Log(trv_dir);
                    if(trv_dir.y>0){  //COMING FROM UP
                        opposite = false;
                    }
                    if(trv_dir.x>0){  //COMING FROM RIGHT
                        opposite = false;
                    }
                }else{
                    lookVector = new Vector3(trv_dir.x,0,trv_dir.y);
                    targetVector = new Vector3();
                    int rot = Mathf.RoundToInt(next_road.transform.eulerAngles.y);
                    if(rot == 0){ //bottom-left
                        if(lookVector.z == 1){
                            targetVector.z = 0;
                            targetVector.x = -1;
                        }else{
                            targetVector.x = 0;
                            targetVector.z = -1;
                        }
                    }else if(rot == 90){ //top-left
                        if(lookVector.x == 1){
                            targetVector.z = 1;
                            targetVector.x = 0;
                        }else{
                            targetVector.x = -1;
                            targetVector.z = 0;
                        }
                    }else if(rot == 180){ //top-right
                        if(lookVector.x == -1){
                            targetVector.z = 1;
                            targetVector.x = 0;
                        }else{
                            targetVector.x = 1;
                            targetVector.z = 0;
                        }
                    }else if(rot == 270){ //bottom-right
                        if(lookVector.x == -1){
                            targetVector.z = -1;
                            targetVector.x = 0;
                        }else{
                            targetVector.x = 1;
                            targetVector.z = 0;
                        }
                    }
                    sgn = Vector3.Cross(lookVector,targetVector).y;
                    if(sgn<0){
                        opposite = false;
                    }
                }
                
                traverser.AddPath(temp.Value,opposite);
                break;
            }

            //Instantiate(cube, next_road.transform.position + new Vector3(0,3,0), Quaternion.identity);
            if(next_road.gameObject.CompareTag("straight_road") || next_road.gameObject.CompareTag("road_upper")
                || next_road.gameObject.CompareTag("road_rise_upper") || next_road.gameObject.CompareTag("road_rise_lower")){
                if((next_road).vertical){
                    trv_dir.x = 0;
                }else{
                    trv_dir.y = 0;
                }
            }else if(next_road.gameObject.CompareTag("turn_road")){
                int rot = Mathf.RoundToInt(next_road.transform.eulerAngles.y);
                if(rot == 0){ //bottom-left
                    if(trv_dir.y == 1){
                        trv_dir.y = 0;
                        trv_dir.x = -1;
                    }else{
                        trv_dir.x = 0;
                        trv_dir.y = -1;
                    }
                }else if(rot == 90){ //top-left
                    if(trv_dir.x == 1){
                        trv_dir.y = 1;
                        trv_dir.x = 0;
                    }else{
                        trv_dir.x = -1;
                        trv_dir.y = 0;
                    }
                }else if(rot == 180){ //top-right
                    if(trv_dir.x == -1){
                        trv_dir.y = 1;
                        trv_dir.x = 0;
                    }else{
                        trv_dir.x = 1;
                        trv_dir.y = 0;
                    }
                }else if(rot == 270){ //bottom-right
                    if(trv_dir.x == -1){
                        trv_dir.y = -1;
                        trv_dir.x = 0;
                    }else{
                        trv_dir.x = 1;
                        trv_dir.y = 0;
                    }
                }
            }

            check_pos = next_road.transform.position + new Vector3(trv_dir.x,0,trv_dir.y)*6;

            int leveloffset = 0;
            if(next_road.gameObject.CompareTag("road_rise_lower")){
                leveloffset = 1;
            }
    
            next_road = getRoadAt(check_pos,next_road.level+leveloffset);
            count++;
        }

        Manager.Instance.nextJunction = next_road;
    }

    // Update is called once per frame
    public Transform getRoadUnder(Transform component){
        updateTrack();
        Vector3 pos = component.transform.position;
        Transform closestTrack = component; //so it doesnt error
        float distance = Mathf.Infinity;
        foreach(Transform track in tracks){
            if ((track.position-pos).magnitude < distance){
                closestTrack = track;
                distance = (track.position-pos).magnitude;
            }
        }
        return closestTrack;
    }

    public AbstractRoad getRoadAt(Vector3 check_pos, int level){
        updateTrack();
        ArrayList stacked_roads = new ArrayList();
        AbstractRoad closestRoad = tracks[0].gameObject.GetComponent<AbstractRoad>(); //so it doesnt error
        Vector3 track_pos;
        Vector3 check_posnew = new Vector3(check_pos.x,0,check_pos.z);
        foreach(Transform track in tracks){
            track_pos = new Vector3(track.position.x,0,track.position.z);
            if ((track_pos-check_posnew).magnitude < 6){
                stacked_roads.Add(track.gameObject.GetComponent<AbstractRoad>());
            }
        }
        if(stacked_roads.Count == 1){
            return stacked_roads[0] as AbstractRoad;
        } else if (stacked_roads.Count == 2){
            if( (stacked_roads[0] as AbstractRoad).level == level ){
                return (stacked_roads[0] as AbstractRoad);
            }
            if( (stacked_roads[1] as AbstractRoad).level == level ){
                return (stacked_roads[1] as AbstractRoad);
            }
        }
        return stacked_roads[0] as AbstractRoad;
    }

    public GameObject getClosestJunction(Traverser tr){
        Vector3 pos = tr.gameObject.transform.position;
        GameObject closestTrack = tr.gameObject; //so it doesnt error
        float distance = Mathf.Infinity;
        foreach(Transform track in tracks){
            if (track.gameObject.CompareTag("junction_3way")||track.gameObject.CompareTag("junction_4way")){
                if ((track.position-pos).magnitude < distance){
                    closestTrack = track.gameObject;
                    distance = (track.position-pos).magnitude;
                }
            }
        }
        return closestTrack;
    }
    void Update()
    {
        
    }
}
