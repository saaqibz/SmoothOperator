using UnityEngine;

public class Cord : MonoBehaviour {
    private const float CORD_Z_AXIS = 1.6f;

    private Transform startPos;
    private Transform endPos;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
    }
	
    private Vector3 GetMousePosition()
    {
        var boardCenterPos = new Vector3(0f, 0f, CORD_Z_AXIS);

        Plane plane = new Plane(new Vector3(0,0,-1), boardCenterPos);

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
    void Update () {
        var mousePos = GetMousePosition();
        // Debug.Log("Mouse: " + mousePos);

        if (startPos == null && endPos == null)
        {
            // Do Nothing
        }
        else if (startPos == null)
        {
            DrawCord(endPos.position, mousePos);
        }
        else if (endPos == null)
        {
            DrawCord(startPos.position, mousePos);
        }
        else
        {
            DrawCord(startPos.position, endPos.position);
        }

    }

    public void SetStart(Transform start)
    {
        startPos = start;
        gameObject.SetActive(true);
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

    private void DrawCord(Vector3 start, Vector3 end)
    {
        start.z = CORD_Z_AXIS;
        end.z = CORD_Z_AXIS;

        transform.position = Vector3.Lerp(start, end, .5f);
        transform.LookAt(end);
        transform.localScale = new Vector3(0.1f, 0.1f,
            Vector3.Distance(start, end));
    }

}
