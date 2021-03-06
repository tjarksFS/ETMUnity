﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed;
    public GUIText countText;
    public GUIText winText;
    public GameObject finishLine;
    public GameObject virtualJoystick;
    public float finishLineSpeed;
    private int count;
    private int badCount;
    private bool gotToBox;
    private bool moveFinishUp;
	private bool hasLost;
	private bool hasWon;
	public GUITexture [] checks;
	public Texture2D filledCheck;
    public Joystick joystick;

    private Vector2 scrollViewVector = Vector2.zero;
    private string[] controlType = { "Accelerometer", "Virtual Joystick", "Screen Edge" };//add the rest

    private bool dropDownOpen;

    int n, whichControl;

    void Start()
    {
        dropDownOpen = false;
        whichControl = 0;

        count = 0;
        badCount = 0;
        SetCountText();
        SetBadCountText();
        winText.text = "";
        gotToBox = false;
        moveFinishUp = false;
		hasLost = false;
		hasWon = false;

    }
	private void OnGUI()
	{
        GUIStyle gs = new GUIStyle(GUI.skin.GetStyle("Button"));
        gs.fontSize = 25;
        //Code adapted heavily from http://forum.unity3d.com/threads/simple-drop-down-menu-script-for-gui.189139/
        if (dropDownOpen == false && GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 100, 250, 75), controlType[whichControl],gs))
        {
            dropDownOpen = !dropDownOpen;
        }

        if (dropDownOpen == true)
        {
            scrollViewVector = GUI.BeginScrollView(new Rect(Screen.width / 2 - 150, Screen.height - 250, 250, 225), scrollViewVector, new Rect(0, 0, 250, 225));
            GUI.Box(new Rect(0, 0, 300, 500), "");
            for (int i = 0; i < controlType.Length; i++)
            {
                if (GUI.Button(new Rect(0, i * 75, 250, 75), controlType[i], gs))
                {
                    dropDownOpen = false;
                    whichControl = i;
                    if (whichControl == 1)
                    {
                        virtualJoystick.SetActive(true);
                    }
                    else
                    {
                        virtualJoystick.SetActive(false);
                    }
                }
            }
            GUI.EndScrollView();
        }


		gs = new GUIStyle(GUI.skin.GetStyle("Button"));
		gs.fontSize = 50;

        bool resolutionWide = Screen.currentResolution.width > Screen.currentResolution.height ? true : false;

        if ((hasWon || hasLost))
        {
            if (GUI.Button(new Rect(Screen.width / 2 - ((!resolutionWide || hasLost) ? 250 : 525), Screen.height / 2 + 100, 500, 150), "Reset", gs))
            {
                Application.LoadLevel("MainScene");
            }
        }

        if (hasWon)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - (!resolutionWide ? 250 : -25), Screen.height / 2 + (!resolutionWide ? 300 : 100), 500, 150), "Next Level", gs))
            {
                Application.LoadLevel("ProjectileGame");
            }
        }
//		for (int i = 0; i < (badCount < 3 ? badCount : 3); i++) {
//			checks[i].texture = filledCheck;
//		}
	}

	void FixedUpdate() 
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rigidbody.AddForce(movement * speed * Time.deltaTime);


        switch (whichControl)
        {
            case 0:
                moveVertical = Input.acceleration.y;
                moveHorizontal = Input.acceleration.x;


                movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

                if (movement.sqrMagnitude > 1)
                    movement.Normalize();
       
                rigidbody.AddForce (movement * speed * 2 * Time.deltaTime);
                break;
            case 1:
                moveVertical = joystick.position.y;
                moveHorizontal = joystick.position.x;


                movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

                if (movement.sqrMagnitude > 1)
                    movement.Normalize();

                rigidbody.AddForce(movement * speed * Time.deltaTime);
                break;
            case 2:
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                    {
                        if (touch.position.x < Screen.width * .1)
                        {

                            moveHorizontal = -1;
                        }
                        else if (touch.position.x > Screen.width * .9)
                        {
                            moveHorizontal = 1;
                        }
                        else if (touch.position.y < Screen.height * .1)
                        {
                            moveVertical = -1;
                        }
                        else if (touch.position.y > Screen.height * .9)
                        {
                            moveVertical = 1;
                        }
                    }

                }
                movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

                rigidbody.AddForce(movement * speed * Time.deltaTime);
                break;
        }

        if (moveFinishUp)
        {
            finishLine.transform.Translate(new Vector3(0.0f, finishLineSpeed * Time.deltaTime, 0.0f));
            if (finishLine.transform.position.y > 0)
                moveFinishUp = false;
        }

		//if (Input.acceleration.sqrMagnitude > 2.25)
		//	rigidbody.AddForce (new Vector3 (0.0f, 1000, 0.0f));
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "PickUp")
		{
			other.gameObject.SetActive(false);
            count++;
            SetCountText();
		}
        else if (other.gameObject.tag == "BadPickUp")
        {
            other.gameObject.SetActive(false);
            if (badCount < checks.Length)
				checks[badCount].texture = filledCheck;
            badCount++;
            SetBadCountText();
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (!hasLost && gotToBox && other.collider.gameObject.tag == "SouthWall")
        {
            winText.text = "YOU WIN!\nScore: " + count;
            moveFinishUp = true;
			hasWon = true;
        }
        else if (other.collider.gameObject.tag == "Finish")
        {
            gotToBox = true;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    void SetBadCountText()
    {
        if (badCount >= checks.Length)
        {
            winText.text = "YOU LOSE!";
			moveFinishUp = true;
			hasLost = true;
        }
    }
	
}