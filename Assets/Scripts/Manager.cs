using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using Lean.Touch;
public class Manager: Singleton<Manager> 
{
    public AbstractRoad nextJunction;
    public AbstractRoad ActiveJunction;
    public Traverser CurrentTraverser;
    public Tracks CurrentTracks;
    public bool requestInput = false;
    protected Manager() { }


    private void Start() {
        EventManager.StartListening("WIN",Win);
        EventManager.StartListening("SwipeUp",OnSwipeUp);
        EventManager.StartListening("SwipeDown",OnSwipeDown);
        EventManager.StartListening("SwipeLeft",OnSwipeLeft);
        EventManager.StartListening("SwipeRight",OnSwipeRight);
    }

    private string picked_dir = "";

    public void Win(){
        requestInput = false;
        if(ActiveJunction != null){
            ActiveJunction.Deactivate();
        }
    }

    void OnSwipeUp()
    {
        if(!requestInput){
            return;
        }
        if(ActiveJunction.has("top",CurrentTraverser)){
                picked_dir = "top";
            }else{
                ActiveJunction.Wobble();
            }
    }

    void OnSwipeDown()
    {
        if(!requestInput){
            return;
        }
        if(ActiveJunction.has("down",CurrentTraverser)){
                picked_dir = "down";
            }else{
                ActiveJunction.Wobble();
            }
    }

    void OnSwipeLeft()
    {
        if(!requestInput){
            return;
        }
        
            if(ActiveJunction.has("left",CurrentTraverser)){
                picked_dir = "left";
            }else{
                ActiveJunction.Wobble();
            }
    }

    void OnSwipeRight()
    {
        if(!requestInput){
            return;
        }
        if(ActiveJunction.has("right",CurrentTraverser)){
                picked_dir = "right";
            }else{
                ActiveJunction.Wobble();
            }
    }

    Touch touch;
    Vector2 startPos;
    private void Update() {
        if(!requestInput){
            return;
        }else{
            CurrentTraverser.paused = true;
        }
        if(Input.GetKeyDown(KeyCode.A)){
            if(ActiveJunction.has("left",CurrentTraverser)){
                picked_dir = "left";
            }else{
                ActiveJunction.Wobble();
            }
        }
        else if(Input.GetKeyDown(KeyCode.W)){
            if(ActiveJunction.has("top",CurrentTraverser)){
                picked_dir = "top";
            }else{
                ActiveJunction.Wobble();
            }
        }
        else if(Input.GetKeyDown(KeyCode.D)){
            if(ActiveJunction.has("right",CurrentTraverser)){
                picked_dir = "right";
            }else{
                ActiveJunction.Wobble();
            }
        }
        else if(Input.GetKeyDown(KeyCode.S)){
            if(ActiveJunction.has("down",CurrentTraverser)){
                picked_dir = "down";
            }else{
                ActiveJunction.Wobble();
            }
        }
        if(!picked_dir.Equals("")){  //SOMETHING WAS PICKED
            CurrentTracks.AddPathAndCombine(ActiveJunction,CurrentTraverser,picked_dir);
            picked_dir = "";
            ActiveJunction.Deactivate();
            EventManager.EmitEvent("MOVED");
            CurrentTraverser.paused = false;
            requestInput = false;
        }
    }


}
