using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallRequest : MonoBehaviour {

    public float waitTime = 20f;

    private float countdownStartTime;

    public Text tCol1;
    public Text tRow1;
    public Text tCol2;
    public Text tRow2;
    public Slider waitSlider;

    private GameObject gameController;

    public CallRequest(char col1, char row1, char col2, char row2, float patience)
    {
        tCol1.text = col1.ToString();
        tRow1.text = row1.ToString();
        tCol2.text = col2.ToString();
        tRow2.text = row2.ToString();

        countdownStartTime = Time.time;
        waitTime = patience;

        waitSlider.value = 1f;
    } // possibly useless for Object.Instantiate

    public void StartRequest(char col1, char row1, char col2, char row2, float patience)
    {
        tCol1.text = col1.ToString();
        tRow1.text = row1.ToString();
        tCol2.text = col2.ToString();
        tRow2.text = row2.ToString();

        countdownStartTime = Time.time;
        waitTime = patience;

        waitSlider.value = 1f;
    }

    // Use this for initialization
    void Start() {
        gameController = GameObject.Find("GameController");
    }
	
	// Update is called once per frame
	void Update () {
        waitSlider.value = (waitTime + countdownStartTime - Time.time) / waitTime; // show percentage of time remaining

        // should implement wait timer freezing while call is connected?
	}

    private void CompleteCall()
    {
        // make it go Ding
        Destroy(gameObject);
    }

    private void TimeOver()
    {
        print("You took too long. Bad employee!");
        Destroy(gameObject);
    }
}
