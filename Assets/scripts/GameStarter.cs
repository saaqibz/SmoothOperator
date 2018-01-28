using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour {

    public bool singlePlayerBool = false;

    private float startTime;
    private GameController gameController;

    public Text countText;

	// Use this for initialization
	void Start () {
        startTime = Time.time + 3f;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
        countText.text = (Mathf.Ceil((startTime - Time.time))).ToString();
        if(startTime < Time.time)
        {
            StartGame();
        }
	}

    public void StartGame()
    {
        if (singlePlayerBool)
        {
            gameController.StartGameSinglePlayer();
        } else
        {
            gameController.StartGameMultiPlayer();
        }

        gameObject.SetActive(false);
    }
}
