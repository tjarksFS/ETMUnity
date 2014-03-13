using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed;
    public GUIText countText;
    public GUIText badCountText;
    public GUIText winText;
    public GameObject finishLine;
    public float finishLineSpeed;
    private int count;
    private int badCount;
    private bool gotToBox;
    private bool moveFinishUp;
	private bool hasLost;
	private bool hasWon;
	public GUITexture [] checks;
	public Texture filledCheck;


    void Start()
    {
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

	void FixedUpdate() 
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rigidbody.AddForce(movement * speed * Time.deltaTime);



		moveVertical = Input.acceleration.y;
		moveHorizontal = Input.acceleration.x;


		movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        if (movement.sqrMagnitude > 1)
            movement.Normalize();

        

		rigidbody.AddForce (movement * speed * 2 * Time.deltaTime);
       
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
			if (badCount < 3)
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
        badCountText.text = "Reds Picked Up: " + badCount.ToString();
        if (badCount >= 3)
        {
            winText.text = "YOU LOSE!";
			moveFinishUp = true;
			hasLost = true;
        }
    }
	
	private void OnGUI()
	{
		GUIStyle gs = new GUIStyle(GUI.skin.GetStyle("Button"));
		gs.fontSize = 50;
		if ((hasWon || hasLost) && GUI.Button(new Rect(Screen.width / 2 - 300, Screen.height / 2 + 100, 600, 200), "Reset", gs))
		{
			Application.LoadLevel("Game");
		}

//		for (int i = 0; i < (badCount < 3 ? badCount : 3); i++) {
//			checks[i].texture = filledCheck;
//		}
	}
}