using UnityEngine;
using System.Collections;

public class ThrowableController : MonoBehaviour {

    private bool thrown;
    public float initialVelocity;
    public ParticleSystem targetParticles;
    public float shakeMagnitude;
    private Vector3 projectileStart;
    private int hitCount;
    private int missedCount;
    public Texture filledCheck;
    public GUITexture[] checks;
    public GUIText winText;
    public GUIText hitCountText;
    public bool gameOver;
	// Use this for initialization
	void Start () {
        hitCount = 0;
        missedCount = 0;
        projectileStart = rigidbody.position;
        thrown = false;
        gameOver = false;
        SetCountText();
        winText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        bool fire = Input.GetButtonUp("Jump") || Input.acceleration.sqrMagnitude > shakeMagnitude*shakeMagnitude;
        if (fire && !thrown)
        {
            rigidbody.AddForce(new Vector3(0, 0, initialVelocity));
            thrown = true;
        }
    }

    void SetCountText()
    {
        hitCountText.text = "Hits: " + hitCount.ToString();
        if (hitCount >= 10)
        {
            winText.text = "YOU WIN!";
            gameOver = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            //targetParticles.transform.position = collision.contacts[0].point;
            targetParticles.Emit(500);

            rigidbody.velocity = Vector3.zero;
            rigidbody.position = projectileStart;
            thrown = false;
            hitCount++;
            SetCountText();
        }
        if (other.gameObject.tag == "MissedTarget")
        {
            rigidbody.position = projectileStart;
            rigidbody.velocity = Vector3.zero;
            thrown = false;
            if (missedCount < checks.Length)
                checks[missedCount].texture = filledCheck;
            missedCount++;
            if (missedCount >= checks.Length)
            {
                winText.text = "YOU LOSE!";
                gameOver = true;
            }
        }
    }

    private void OnGUI()
    {
        GUIStyle gs = new GUIStyle(GUI.skin.GetStyle("Button"));
        gs.fontSize = 50;

        if (gameOver)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 + 100, 500, 150), "New Game", gs))
            {
                Application.LoadLevel("MainScene");
            }
        }
    }
}
