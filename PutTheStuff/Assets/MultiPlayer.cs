using UnityEngine;
using System.Collections;

public class MultiPlayer : MonoBehaviour
{
    public float speed;
    private Vector3 startPos;
    private GUIText winText;

    void Start()
    {
        //DontDestroyOnLoad(this);
        startPos = rigidbody.position;
        winText = Object.FindObjectOfType(typeof(GUIText)) as GUIText;
    }

    void FixedUpdate()
    {
        if (networkView.isMine)
        {
            if (winText.text != "")
                gameObject.SetActive(false);
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rigidbody.AddForce(movement * speed * Time.deltaTime);



            moveVertical = Input.acceleration.y;
            moveHorizontal = Input.acceleration.x;


            movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            if (movement.sqrMagnitude > 1)
                movement.Normalize();



            rigidbody.AddForce(movement * speed * 2 * Time.deltaTime);

            if (transform.position.y < -5)
            {
                rigidbody.position = startPos;
                rigidbody.velocity = Vector3.zero;
            }

        //    if (moveFinishUp)
        //    {
        //        finishLine.transform.Translate(new Vector3(0.0f, finishLineSpeed * Time.deltaTime, 0.0f));
        //        if (finishLine.transform.position.y > 0)
        //            moveFinishUp = false;
        //    }
        }
        //if (Input.acceleration.sqrMagnitude > 2.25)
        //    rigidbody.AddForce (new Vector3 (0.0f, 1000, 0.0f));
    }

    void OnTriggerEnter(Collider other)
    {

        //if (other.gameObject.tag == "PickUp")
        //{
        //    other.gameObject.SetActive(false);
        //    count++;
        //    SetCountText();
        //}
        //else if (other.gameObject.tag == "BadPickUp")
        //{
        //    other.gameObject.SetActive(false);
        //    if (badCount < checks.Length)
        //        checks[badCount].texture = filledCheck;
        //    badCount++;
        //    SetBadCountText();
        //}
    }

    void OnCollisionStay(Collision other)
    {

        if (other.gameObject.tag == "Accelerator")
        {
            rigidbody.AddForce(0.0f, 0.0f, 1000.0f*Time.deltaTime);
        }
        
        //if (!hasLost && gotToBox && other.collider.gameObject.tag == "SouthWall")
        //{
        //    winText.text = "YOU WIN!\nScore: " + count;
        //    moveFinishUp = true;
        //    hasWon = true;
        //}
        //else if (other.collider.gameObject.tag == "Finish")
        //{
        //    gotToBox = true;
        //}
    }

    void SetCountText()
    {
        //countText.text = "Count: " + count.ToString();
    }

    void SetBadCountText()
    {
        //if (badCount >= checks.Length)
        //{
        //    winText.text = "YOU LOSE!";
        //    moveFinishUp = true;
        //    hasLost = true;
        //}
    }

    private void OnGUI()
    {
        GUIStyle gs = new GUIStyle(GUI.skin.GetStyle("Button"));
        gs.fontSize = 50;

        //bool resolutionWide = Screen.currentResolution.width > Screen.currentResolution.height ? true : false;

        //if ((hasWon || hasLost))
        //{
        //    if (GUI.Button(new Rect(Screen.width / 2 - ((!resolutionWide || hasLost) ? 250 : 525), Screen.height / 2 + 100, 500, 150), "Reset", gs))
        //    {
        //        Application.LoadLevel("MainScene");
        //    }
        //}

        if (winText.text != "")
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 + 100, 500, 150), "Reset", gs))
            {
                Application.LoadLevel("MainScene");
            }
        }

    }
}