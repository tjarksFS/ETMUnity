using UnityEngine;
using System.Collections;

public class TargetController : MonoBehaviour {

    private bool left;
    public float speed;
   

	// Use this for initialization
	void Start () {
        left = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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


}
