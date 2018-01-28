using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.scripts;

public class Fuckup : MonoBehaviour {

    private PlugEnds tried;
    private PlugEnds meant;

    private char[] colCipher;
    private char[] rowCipher;

    public Text a_col1;
    public Text a_row1;
    public Text a_col2;
    public Text a_row2;
    public Text b_col1;
    public Text b_row1;
    public Text b_col2;
    public Text b_row2;


    public void DisplayFuckup(PlugEnds answer, PlugEnds solution, char[] cols, char[] rows)
    {
        colCipher = cols;
        rowCipher = rows;

        tried = answer;
        meant = solution;

        a_col1.text = cols[answer.first.x].ToString();
        a_row1.text = rows[answer.first.y].ToString();
        a_col2.text = cols[answer.second.x].ToString();
        a_row2.text = rows[answer.second.y].ToString();

        b_col1.text = cols[solution.first.x].ToString();
        b_row1.text = rows[solution.first.y].ToString();
        b_col2.text = cols[solution.second.x].ToString();
        b_row2.text = rows[solution.second.y].ToString();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
