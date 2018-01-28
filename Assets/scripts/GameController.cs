using Assets.scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	private const int NUM_COLS = 4;
	private const int NUM_ROWS = 4;

	public GameObject plugPrefab;
    public GameObject callPanelPrefab;
    public GameObject commandPanel;
    public GameObject gameOverPanel;
    public Text scoreText;
    public Text gameOverText;
	private int score = 0;

    // Text Shit
    public GameObject topLabel;
    public GameObject leftLabel;
    public GameObject rightLabel;

    // symbols for the rows and columns
    public char[] rowSyms;
    public char[] colSyms;

    // x- and y-spacing for the plugs
    public float colSpace;
	public float rowSpace;

	private GameObject startPlug;
	private GameObject endPlug;

    public Cord cord;

    private List<CallRequest> requestEnds;

    private List<GameObject> plugs = new List<GameObject>();

    static System.Random rand = new System.Random();

    // Use this for initialization
    void Start () {
        requestEnds = new List<CallRequest>();

        rowSyms = GenerateCipher() ;
        colSyms = GenerateCipher();

        topLabel.GetComponent<TextMesh>().text = colSyms[0] + " " + colSyms[1] + " " + colSyms[2] + " " + colSyms[3];
        leftLabel.GetComponent<TextMesh>().text = rowSyms[0] + "\n" + rowSyms[1] + "\n" + rowSyms[2] + "\n" + rowSyms[3];
        rightLabel.GetComponent<TextMesh>().text = rowSyms[0] + "\n" + rowSyms[1] + "\n" + rowSyms[2] + "\n" + rowSyms[3];

    }

    private void ShouldShowPlug(GameObject plug, bool show)
    {
        plug.GetComponent<MeshRenderer>().enabled = show;
    }

	private void InstantiatePlugs() {
		for (var col = 0 ; col < NUM_COLS; ++col) {
			for (var row = 0; row < NUM_ROWS; ++row) {
				var position = new Vector3(-4.254f + colSpace * col, 3.223f + -1 * rowSpace * row, 2.571f);		
				var plug = Instantiate(plugPrefab, position, Quaternion.Euler(90, 0, 0));
                ShouldShowPlug(plug, false);
                plug.GetComponent<Plug>().setPosition(col, row);
                plugs.Add(plug);
			}
		}
    }

    private void CreatePlugGoal()
    {
        var goalPlug = new PlugEnds();
        print("Okay, now connect " + goalPlug.first.CoordString() + " to " + goalPlug.second.CoordString());
        GameObject newCallPanel = Instantiate(callPanelPrefab, commandPanel.transform);
        CallRequest call = newCallPanel.GetComponent<CallRequest>();
        call.StartRequest(goalPlug, colSyms, rowSyms, 20f);

        requestEnds.Add(call);
    }

    public void TriggerPlug(GameObject plug)
    { // was GameObject instead of Plug
        if (plug == startPlug)
        {
            startPlug = null;
            ShouldShowPlug(plug, false);
            cord.RemoveStart();
        }
        else if (plug == endPlug)
        {
            endPlug = null;
            ShouldShowPlug(plug, false);
            cord.RemoveEnd();
        }
        else if (!(startPlug != null && endPlug != null)) // i.e. only one plug is null and we're about to finish a connection
        {
            if (startPlug == null)
            {
                startPlug = plug;
                ShouldShowPlug(plug, true);
                cord.SetStart(plug.transform);
            }
            else // endPlug == null; there's no other option
            {
                endPlug = plug;
                ShouldShowPlug(plug, true);
                cord.SetEnd(plug.transform);
            }

            // connection is completed, let's see if the player got it right...
            if (startPlug != null && endPlug != null) // !(plugs == null) will always return true
            {
                var plugs = ToPlugEnds();
				tryTransmission (plugs);
            }
        }


        var _startPlug = (startPlug != null) ? startPlug.GetComponent<Plug>().ToString() : "null";
        var _endPlug = (endPlug != null) ? endPlug.GetComponent<Plug>().ToString() : "null";
        Debug.Log("startPlug: " + _startPlug + "| endPlug: " + _endPlug);
    }

    // Update is called once per frame
    void Update () {}

    private PlugEnds ToPlugEnds()
    {
        return new PlugEnds(startPlug.GetComponent<Plug>().getCol(),
            startPlug.GetComponent<Plug>().getRow(),
            endPlug.GetComponent<Plug>().getCol(),
            endPlug.GetComponent<Plug>().getRow());
    }

	private IEnumerator<WaitForSeconds> removeBothPlugs() {
		yield return new WaitForSeconds(0.5f);
		TriggerPlug (startPlug);
		TriggerPlug (endPlug);
	}
    
	private bool tryTransmission(PlugEnds attemptedPlugCoordinates) {
		foreach (CallRequest curRequest in requestEnds)
		{
			if (curRequest.getSolution().Equals(attemptedPlugCoordinates))
			{
                requestEnds.Remove(curRequest);
				var waitTime = curRequest.CompleteCall();
				print("Well done.");
				score += Mathf.FloorToInt(10f + 50 * waitTime);
                scoreText.text = "Score: " + score;
				CreatePlugGoal();
				StartCoroutine(removeBothPlugs ());
				Debug.Log (score);
			}
			else
			{
				var startString = (startPlug != null) ? startPlug.GetComponent<Plug>().ToString() : "null";
				var endString = (endPlug != null) ? endPlug.GetComponent<Plug>().ToString() : "null";
                print("You silly goose, look what you've done! You've connected " + startString + " and " + endString);
                GameOver("You made a bad connection. Jimmy ended up calling his ex and now things are awkward.");
			}
				
			// to do: remove curPlugCoordinate, kill the CallRequest UI element.
			// and play a ding ding sound
		}
		return false;
	}

    public void TimeOver(CallRequest unhappyCustomer)
    {
        print("Time over, you lazy fool");
        requestEnds.Remove(unhappyCustomer);
        if (rand.Next(0,1000) == 69)
        {
            GameOver("You have died of dysentery");
        }
        GameOver("You ran out of time. Martha couldn't order her pizza and starved to death.");
    }

    public void StartGameSinglePlayer()
    {
        InstantiatePlugs();
        commandPanel.SetActive(true);
        CreatePlugGoal();
    }

    public void StartGameMultiPlayer()
    {
        CreatePlugGoal();
    }

    public char[] GenerateCipher()
    {
        HashSet<char> cipher = new HashSet<char>();
        var i = 0;
        while(i < 4)
        {
            char thing = (char)(rand.Next(65, 91));
            if (!cipher.Contains(thing))
            {
                cipher.Add(thing);
                i++;
            }
        }
        char[] returnThis = new char[4];
        cipher.CopyTo(returnThis);
        return returnThis;
    }

    public void GameOver(string message)
    {
        gameOverText.text = message;
        gameOverPanel.SetActive(true);
        commandPanel.SetActive(false);
        foreach(GameObject plug in plugs)
        {
            Destroy(plug);
        }
        foreach(CallRequest request in requestEnds)
        {
            Destroy(request.gameObject);
        }
    }
}
