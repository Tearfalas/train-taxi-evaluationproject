using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class TextUpdate : MonoBehaviour
{   
    public GameObject passengers;
    public RectTransform fill;
    private int total;
    private int current = 0;
    // Start is called before the first frame update
    void Start()
    {
        total = passengers.transform.childCount;
        EventManager.StartListening("NEW_PASSENGER",newPassenger);
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = current + " / "+total;
    }

    // Update is called once per frame
    void newPassenger(){
        current++;
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = current + " / "+total;
        //fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,current/total);
        fill.localScale = new Vector3((current+0.0f)/total,1,1);
        if(current == total){
            EventManager.EmitEvent("WIN");
        }
    }
    void Update()
    {
        
    }
}
