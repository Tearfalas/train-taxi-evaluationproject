using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class MyCamera : MonoBehaviour
{
    public GameObject trainhead;
    public float initial_difference = 60f;
    void Start()
    {
        EventManager.StartListening("CRASHED",Crashed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y,trainhead.transform.position.z - initial_difference);
    }


    void Crashed(){
        GetComponent<Animator>().SetBool("blurred",true);
    }
}
