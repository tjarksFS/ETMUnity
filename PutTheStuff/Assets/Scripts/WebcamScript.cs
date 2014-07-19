using UnityEngine;
using System.Collections;

public class WebcamScript : MonoBehaviour {
    
    public string cameraName;
    WebCamTexture texture;
    static public Texture2D picture = null;
    static public Quaternion pictureRotation;
    private Quaternion baseRotation;
    // Use this for initialization
	void Start () {
        WebCamDevice[] cameras = WebCamTexture.devices;
        cameraName = cameras[0].name;
        texture = new WebCamTexture(cameraName, 720, 720);
        renderer.material.mainTexture = texture;
        baseRotation = transform.rotation;
        pictureRotation = baseRotation;
        texture.Play();
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = baseRotation * Quaternion.AngleAxis(texture.videoRotationAngle, Vector3.up);
	}

    private void OnGUI()
    {
        GUIStyle gs = new GUIStyle(GUI.skin.GetStyle("Button"));
        gs.fontSize = 50;
        if (GUI.Button(new Rect(Screen.width / 2 - 300, Screen.height - 250, 600, 200), "Take and Use Picture", gs))
        {
            picture = new Texture2D(texture.width, texture.height);
            picture.SetPixels(texture.GetPixels());
            picture.Apply();

            pictureRotation = Quaternion.AngleAxis(texture.videoRotationAngle, Vector3.up);

            texture.Stop();

            Application.LoadLevel("Game");
        }



    }
}
