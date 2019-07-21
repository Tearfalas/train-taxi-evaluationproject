using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    
    private Traverser train_head;


    private bool active = false;

    public bool is_active{
        get{
            return active;
        }
    }

    public float speed = 4f;
    private Vector3 base_scale;
    private Vector3 base_pos;
    void Start()
    {
        train_head = Manager.Instance.CurrentTraverser;
        transform.localScale = transform.localScale*(0.85f + Random.Range(0,transform.position.x+transform.position.y)/(transform.position.x+transform.position.y)/3);
        base_scale = transform.localScale;
        gameObject.transform.Rotate(new Vector3(0,Random.Range(-70,70),0),Space.World);
        time = Mathf.Abs(transform.position.y/80) + Mathf.Abs(transform.position.x/30);
    }

    Vector3 dir;
    Vector3 diff;
    float height = 7;
    float t = 0;
    float x;
    float time = 0;
    void Update()
    {
        if(active){
            diff= (train_head.gameObject.transform.position - base_pos);
            diff.y = 0;
            dir = (diff).normalized;
            float dist = diff.magnitude*0.9f;
            t = Mathf.Clamp(t + speed*Time.deltaTime,0,1);
            x = t*dist;
            
            //y = -x*(x-dist)*4*height/dist^2 

            Vector3 target_posxz =  base_pos + dir*dist*t;
            //Vector3 target_y = new Vector3(target_posxz.x,Mathf.Clamp((dist-2)*(dist-7)*(dist-9)*dist/13 + 1.5f,0,25),target_posxz.z);
            Vector3 target_y = new Vector3(target_posxz.x,-Mathf.Sqrt(dist-x)*(Mathf.Sqrt(dist-x)-Mathf.Sqrt(dist))*4*height/(dist), target_posxz.z);

            transform.position = target_y;
            transform.localScale = base_scale*Mathf.Clamp(1.3f-t/2,0.4f,1);
            if(t>0.99){
                train_head.newPassenger();
                Destroy(gameObject);
            }
        }
        time += Time.deltaTime;
        if(time>3){
            time = time - 3;
            if(Random.Range(0,transform.position.x+transform.position.y)/(transform.position.x+transform.position.y)<0.1f){
                gameObject.GetComponent<Animator>().Play("Jump");
            }
        }
    }
    
    public void Collect(){
        if(train_head == null){
            train_head = Manager.Instance.CurrentTraverser;
        }
        base_pos = transform.position;
        active = true;
    }
}
