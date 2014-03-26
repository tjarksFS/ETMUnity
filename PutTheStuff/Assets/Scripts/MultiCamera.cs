using UnityEngine;
using System.Collections;

public class MultiCamera : MonoBehaviour
{

    private GameObject player;
    private Vector3 offset;
    private float lastPercent;
    // Use this for initialization
    void Start()
    {
        player = null;
        offset = transform.position;


    }

    void Update()
    {
        GameObject[] playerArr = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in playerArr)
        {
            if (p.networkView.isMine)
            {
                player = p;
                break;
            }
        }


    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
            if ((player.transform.position.x > -3.5 && player.transform.position.x < 3.5 && player.transform.position.z > -6) || player.transform.position.z > 10)
            {
                float percent = (player.transform.position.z + 6) / 10.0f;
                if (percent > 1.0f) percent = 1.0f;
                transform.position = new Vector3(transform.position.x, transform.position.y - percent * 6, transform.position.z - 6 * percent);
                transform.Rotate((percent - lastPercent) * -50, 0, 0);
                lastPercent = percent;
                //transform.rotation = new Quaternion(50 * percent + 20, 0, 0, 0);
            }

            if (player.transform.position.y < -4)
            {
                transform.Rotate(lastPercent * 50, 0, 0);
                lastPercent = 0;
            }
        }
    }
}
