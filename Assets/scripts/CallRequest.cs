using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
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

    private GameController gameController;

    private PlugEnds solution;
    private char[] colCipher;
    private char[] rowCipher;

    public void StartRequest(PlugEnds answer, char[] colNames, char[] rowNames, float patience)
    {
        colCipher = colNames;
        rowCipher = rowNames;
        solution = answer;

        tCol1.text = colCipher[solution.first.x].ToString();
        tRow1.text = rowCipher[solution.first.y].ToString();
        tCol2.text = colCipher[solution.second.x].ToString();
        tRow2.text = rowCipher[solution.second.y].ToString();

        countdownStartTime = Time.time;
        waitTime = patience;

        waitSlider.value = 1f;
    }

    public PlugEnds getSolution()
    {
        return solution;
    }

    // Use this for initialization
    void Start() {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
        waitSlider.value = (waitTime + countdownStartTime - Time.time) / waitTime; // show percentage of time remaining
        if (waitSlider.value <= 0)
        {
            TimeOver();
        }
        // should implement wait timer freezing while call is connected?
	}

	/** Destroys instruction and returns wait time */
    public float CompleteCall()
    {
        // make it go Ding
        Destroy(gameObject);
		return waitTime;
    }

    private void TimeOver()
    {
        print("You took too long. Bad employee!");
        gameController.TimeOver(this);
        Destroy(gameObject);
    }
}
