using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraverseFollower : MonoBehaviour
{
    private Vector3 _direction;

    public int index;
    public Traverser head;
    public Vector3 direction{
        get{
            return _direction;
        }
        set{
            _direction = value;
        }
    }

    public float position = 0f;
    
    Vector3 prev_position;
    void Awake()
    {
        
    }

    void Start(){
        /*/
        if(head != null){
            position = head.get_pos - 5*index;
            transform.position = head._traverser_path.PositionAt(position,head) + new Vector3(0,3,0);
            _direction = (head._traverser_path.PositionAt(position+0.01f,head) - head._traverser_path.PositionAt(position,head)).normalized;
            transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(_direction,up_y));
            Debug.Log(_direction);
        }else{
            Debug.Log("was null");
        }*/
        //gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }
    private static Vector3 up_y = new Vector3(0,1,0);
    private float time = 0f;
    void Update()
    {   
        if(time+Time.deltaTime <0.15f){
            time+=Time.deltaTime;
        }
        if(head == null){
            return;
        }
        if(head._paused){
            return;
        }
        position = head.get_pos - 5.6f*index;

        Vector3 new_tr_pos =  head._traverser_path.PositionAt(position,head) + new Vector3(0,2,0);

        transform.position = new_tr_pos;

        if((transform.position - prev_position).magnitude > 0 && time>0.1f){
            _direction = (transform.position - prev_position).normalized;
        }else{
            _direction = head._traverser_path.directionAt(position,head);
        }

        if(_direction.magnitude>0){
            transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(_direction,up_y));
        }

        prev_position = transform.position;

    }
}
