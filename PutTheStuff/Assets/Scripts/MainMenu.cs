using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GUIText directions;

	// Use this for initialization
	void Start () {
		directions.text = "Collect the Gold cubes\nCross the finish line to win\n\nAvoid the Red cubes\n3 strikes, you're out!";	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnGUI()
    {
        GUIStyle gs = new GUIStyle(GUI.skin.GetStyle("Button"));
        gs.fontSize = 50;
        if (GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2, 500, 150), "Start Game", gs))
        {
            Application.LoadLevel("Game");
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 + 200, 500, 150), "Lobby", gs))
        {
            Application.LoadLevel("Lobby");
        }


    }
}
