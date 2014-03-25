using UnityEngine;
using System.Collections;

public class TargetMulti : MonoBehaviour {

    private bool left;
    public float speed;
    public GUIText winText;
    public ParticleSystem targetParticles;
    // Use this for initialization
    void Start()
    {
        left = true;
        winText.text = "";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (left)
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            if (transform.position.x < -10)
                left = false;
        }
        else
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            if (transform.position.x > 10)
                left = true;
        }
    }
    
    void OnCollisionEnter(Collision other)
    {
        NetworkView nv = other.gameObject.GetComponent("NetworkView") as NetworkView;
        targetParticles.Emit(500);
        if (nv.networkView.isMine)
        {
            winText.text = "You Win";
        }
        else
        {
            winText.text = "You Lose";
        }

    }
}

