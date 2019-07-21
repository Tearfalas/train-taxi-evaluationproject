using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TigerForge;

public class Traverser : MonoBehaviour
{
    public GameObject traverse_follower_prefab;
    private List<TraverseFollower> followers;
    private Vector3 _direction;

    public Vector3 direction{
        get{
            return _direction;
        }
        set{
            _direction = value;
        }
    }

    public Tracks tracks;
    //private float totalLength = 0f;

    //private Dictionary<float,Path> pos_map;
    private Path traverser_path;

    public Path _traverser_path{
        get{
            return traverser_path;
        }
    }

    private float position = 0f;

    public float get_pos{
        get{
            return position;
        }
        set{
            position = value;
        }
    }
    public float speed = 7.0f;
    void Start()
    {
        Manager.Instance.CurrentTraverser = this;
        //path_queue = new List<Path>();
        //pos_map = new Dictionary<float,Path>();
        //AbstractRoad junction_object = tracks.getClosestJunction(this).GetComponent<AbstractRoad>();
        traverser_path = new Path();
        //junction_object.Activate();

        AbstractRoad road = tracks.getRoadUnder(transform).gameObject.GetComponent<AbstractRoad>();
        //Debug.Log(road.path);
        //AddPath(road.path["bl"]);
                       //DEBUG
        followers = new List<TraverseFollower>();
        /*
        for (int i = 0; i < 7; i++)
        {
            TraverseFollower newfollower = Instantiate(traverse_follower_prefab).GetComponent<TraverseFollower>();
            newfollower.head = this;
            followers.Add(newfollower);
        }*/
        //liner = linerendererobj.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private static Vector3 up_z = new Vector3(0,0,1);
    private static Vector3 up_y = new Vector3(0,1,0);
    Vector3 prev_position;
    public bool paused = true;

    private void newFollower(){
        float newpos = position - 5*(followers.Count + 1);
        TraverseFollower newfollower = Instantiate(traverse_follower_prefab,this._traverser_path.PositionAt(newpos,this) + new Vector3(0,3,0),Quaternion.LookRotation(_traverser_path.directionAt(newpos,this),up_y)).GetComponent<TraverseFollower>();
        newfollower.index = followers.Count + 1;
        newfollower.head = this;

        newfollower.position = newpos;
        newfollower.direction = _traverser_path.directionAt(newpos,this);


        followers.Add(newfollower);
    }

    private int passenger_count = 0;
    public void newPassenger(){
        passenger_count++;
        int followers_to_be = Mathf.FloorToInt(passenger_count/3);
        int current_followers = followers.Count;
        int diff = followers_to_be-current_followers;
        EventManager.EmitEvent("NEW_PASSENGER");
        if(diff>0){
            for (int i = 0; i < diff; i++)
            {
                newFollower();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other == null){
            return;
        }
        if(other.gameObject == null){
            return;
        }
        if(other.gameObject.CompareTag("passenger")){
            if(other.gameObject.GetComponent<Passenger>().is_active){
                return;
            }
            other.gameObject.GetComponent<Passenger>().Collect();
        } else if (other.gameObject.CompareTag("traverse_follower")) {
            paused = true;
            //SceneManager.LoadScene("SampleScene");
            EventManager.EmitEvent("CRASHED");
            transform.Find("smoke").gameObject.SetActive(true);
        }
        
    }

    public bool _paused{
        get{
            return paused;
        }
    }
    void Update()
    {
        
        if(paused){
            //transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(traverser_path.directionAt(position,this),up_y));
            return;
        }
        if(traverser_path.front == null){
            return;
        }

        if(traverser_path.totalLength - position<2){    //REACHED JUNCTION
            if(Manager.Instance.nextJunction != null){
                Manager.Instance.nextJunction.Activate();
            }
            return;
        }


        position += speed*Time.deltaTime;

       
        Vector3 new_tr_pos = traverser_path.PositionAt(position,this) + new Vector3(0,2,0);

        transform.position = new_tr_pos;

        _direction = (transform.position - prev_position).normalized;

        if(_direction.magnitude!=0){
            transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(_direction,up_y));
        }else{
            transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(traverser_path.directionAt(position,this),traverser_path.directionAt(position,this)));
        }

        prev_position = transform.position;
    }

    private List<Vector3> vertices = new List<Vector3>();
    public void AddPath(Path path,bool opposite){
        //paused = true;
        //path_queue.Add(path);
        //pos_map.Add(totalLength,path);
        LinkedList<KeyValuePair<float,PathNode>> list = new LinkedList<KeyValuePair<float,PathNode>>();
        
        foreach (KeyValuePair<float,PathNode> temp in path.node_map)
        {  
            if(!opposite){
                list.AddLast(temp);
            }else{
                list.AddFirst(temp);
            }
        }



        foreach (KeyValuePair<float,PathNode> temp in list)
        {   
            if(traverser_path.back != null){
                if(Mathf.Abs(temp.Value.x - traverser_path.back.x)<0.05 && Mathf.Abs(temp.Value.y - traverser_path.back.y)<0.05){
                    continue;
                }
            }
            //liner.positionCount = (liner.positionCount + 1);
            PathNode newnode = new PathNode(temp.Value.x,temp.Value.y,temp.Value.z);
            //vertices.Add(newnode.getVector() + new Vector3(0,5,0));
            //Point newpoint = Instantiate(linerendererobj, new Vector3(newnode.x, 2, newnode.y), Quaternion.identity).GetComponent<Point>();
            //newpoint.SetNum(count);
            //count++;
            //liner.SetPositions(vertices.ToArray());
            traverser_path.AddNode(newnode);
        }
        //paused = false;
    }


    /*
    private KeyValuePair<float,Path> getPathAt(float pos){
        KeyValuePair<float,Path> prev = new KeyValuePair<float,Path>(0f, pos_map[0f]);
        foreach (KeyValuePair<float,Path> temp in pos_map)
        {
            if(pos<=temp.Key){
                break;
            }

            prev = temp;
        }
        return prev;
    }*/
}
