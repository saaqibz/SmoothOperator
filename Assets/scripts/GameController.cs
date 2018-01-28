using Assets.scripts;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	private const int NUM_COLS = 4;
	private const int NUM_ROWS = 4;

	public GameObject plugPrefab;
    public GameObject callPanelPrefab;

    // symbols for the rows and columns
    public char[] rowSyms;
    public char[] colSyms;

    // x- and y-spacing for the plugs
    public float colSpace;
	public float rowSpace;

	private GameObject startPlug;
	private GameObject endPlug;

    public Cord cord;

    private List<PlugEnds> requestEnds;

    private List<GameObject> plugs = new List<GameObject>();

    // Use this for initialization
    void Start () {
        requestEnds = new List<PlugEnds>();
		InstantiatePlugs ();

        rowSyms = new char[] { '1', '2', '3', '4' };
        colSyms = new char[] { 'A', 'B', 'C', 'D' };
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
        requestEnds.Add(goalPlug);
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
                if (isValidTransmission(plugs))
                {
                    print("Well done.");
                    requestEnds.Clear();
                    CreatePlugGoal();
                }
                else
                {
                    var startString = (startPlug != null) ? startPlug.GetComponent<Plug>().ToString() : "null";
                    var endString = (endPlug != null) ? endPlug.GetComponent<Plug>().ToString() : "null";
                    print("You silly goose, look what you've done! You've connected " + startString + " and " + endString);
                }
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
    

    /*
     * Validate a plugging attempt. If this is valid, we need to let the world know that someone succeeded.
     *      If it is not valid, do nothing.
     */
    private bool isValidTransmission(PlugEnds attemptedPlugCoordinates)
    {
        foreach (PlugEnds curPlugCoordinate in requestEnds)
        {
            if (curPlugCoordinate.Equals(attemptedPlugCoordinates))
            {
                return true;
            }
        }
        return false;
    }
}
