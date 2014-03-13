using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour 
{

	// Update is called once per frame
	void Update ()
	{
		if (tag == "PickUp")
			transform.Rotate(new Vector3(25, 50, 75) * Time.deltaTime);
		else
			transform.Rotate(new Vector3(-25, -50, -75) * Time.deltaTime);
	}
}
