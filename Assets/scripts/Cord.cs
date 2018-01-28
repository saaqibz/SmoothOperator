using UnityEngine;

public class Cord : MonoBehaviour {
    private const float CORD_Z_AXIS = 2.363f;

    private Transform startPos;
    private Transform endPos;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
    }
	
    private Vector3 getMousePosition()
    {
        var boardCenterPos = new Vector3(-0.127f, 4.512f, CORD_Z_AXIS);

        // FIXME: Need to fix plane or something about this script to properly get the mouse position
        Plane plane = new Plane(boardCenterPos, 0);

        float dist;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out dist))
        {
            return ray.GetPoint(dist);
        }
        else
        {
            return boardCenterPos;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        var mousePos = getMousePosition();
        Debug.Log("Mouse: " + mousePos);

        if (startPos == null && endPos == null)
        {
            // Do Nothing
        }
        else if (startPos == null)
        {
            drawCord(endPos.position, mousePos);
        }
        else if (endPos == null)
        {
            drawCord(startPos.position, mousePos);
        }
        else
        {
            drawCord(startPos.position, endPos.position);
        }

    }

    public void SetStart(Transform start)
    {
        startPos = start;
        gameObject.SetActive(true);
        /*
        Vector3 pos = startPos.position;
        Debug.Log("startPos is (" + pos.x + "," + pos.y + "," + pos.z + ")");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("mousePos is (" + mousePos.x + "," + mousePos.y + "," + mousePos.z + ")");
        */
    }

    public void SetEnd(Transform end)
    {
        endPos = end;
        gameObject.SetActive(true);
    }

    public void RemoveStart()
    {
        startPos = null;
        if (endPos == null)
        {
            gameObject.SetActive(false);
        }
    }

    public void RemoveEnd()
    {
        endPos = null;
        if (startPos == null)
        {
            gameObject.SetActive(false);
        }
    }

    private void drawCord(Vector3 start, Vector3 end)
    {
        start.z = CORD_Z_AXIS;
        end.z = CORD_Z_AXIS;

        transform.position = Vector3.Lerp(start, end, .5f);
        transform.LookAt(end);
        transform.localScale = new Vector3(transform.localScale.x, 0.05f,
            Vector3.Distance(start, end) * 0.95f);
    }

}
