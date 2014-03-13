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
    private Vector3 baselineAcceleration;


    void Start()
    {
        count = 0;
        badCount = 0;
        SetCountText();
        SetBadCountText();
        winText.text = "";
        gotToBox = false;
        moveFinishUp = false;
        baselineAcceleration = Input.acceleration;
        baselineAcceleration.z = baselineAcceleration.y;
        baselineAcceleration.y = 0.0f;
        if (baselineAcceleration.sqrMagnitude > 1)
            baselineAcceleration.Normalize();
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

        movement -= baselineAcceleration;

		rigidbody.AddForce (movement * speed * 2 * Time.deltaTime);
       
        if (moveFinishUp)
        {
            finishLine.transform.Translate(new Vector3(0.0f, finishLineSpeed * Time.deltaTime, 0.0f));
            if (finishLine.transform.position.y > 0)
                moveFinishUp = false;
        }
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
            badCount++;
            SetBadCountText();
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if (gotToBox && other.collider.gameObject.tag == "SouthWall")
        {
            winText.text = "YOU WIN!\nScore: " + count;
            moveFinishUp = true;
        }
        else if (other.collider.gameObject.tag == "Finish")
        {
            gotToBox = true;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 32)
        {
            //winText.text = "YOU WIN!";
        }
    }

    void SetBadCountText()
    {
        badCountText.text = "Reds Picked Up: " + badCount.ToString();
        if (badCount >= 3)
        {
            winText.text = "YOU LOSE!";
        }
    }
}