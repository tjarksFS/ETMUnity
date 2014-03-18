using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

    public GameObject playerPrefab;
   // public GameObject followCamera;

	// Use this for initialization
	void Start () {
        Network.Instantiate(playerPrefab, new Vector3(-9.0f, 0.5f, -9.0f), Quaternion.identity, 0);
         
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
