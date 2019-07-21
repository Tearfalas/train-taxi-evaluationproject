using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.SceneManagement;

public class PlayScreen : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform playbutton;
    public string next_level;
    private Transform fade;


    private Transform crashpanel;
    private Transform restartbutton;
    void Start()
    {
        EventManager.StartListening("MOVED",PlayPressed);
        EventManager.StartListening("CRASHED",Crashed);
        EventManager.StartListening("WIN", Win);
        playbutton = gameObject.transform.Find("PlayButton");
        fade = gameObject.transform.Find("Fade");
        crashpanel = gameObject.transform.Find("CrashPanelBack");
        restartbutton = gameObject.transform.Find("RestartButton");
        GetComponent<Animator>().SetBool("black_active",false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Crashed(){
        fade.gameObject.SetActive(true);
        crashpanel.gameObject.SetActive(true);
        restartbutton.gameObject.SetActive(true);

    }
    public void PlayPressed(){
        playbutton.gameObject.SetActive(false);
        fade.gameObject.SetActive(false);
    }

    public void NextPressed(){
        GetComponent<Animator>().SetBool("black_active",true);
        Invoke("LoadNextScene",2);
    }

    public void Restart(){
        GetComponent<Animator>().SetBool("black_active",true);
        Invoke("RestartScene",2);
    }

    private void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadNextScene(){
        SceneManager.LoadScene(next_level);
    }

    public void Win(){
        Manager.Instance.CurrentTraverser.paused = true;
        transform.Find("Grey").gameObject.SetActive(true);
        transform.Find("Win").gameObject.SetActive(true);
        transform.Find("NextButton").gameObject.SetActive(true);
    }
}
