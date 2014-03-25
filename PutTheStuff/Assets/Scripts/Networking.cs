using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {

    public GameObject playerPrefab;
	// Use this for initialization
	void Start () {

        Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnGUI()
    {
        GUIStyle gs = new GUIStyle(GUI.skin.GetStyle("Button"));
        gs.fontSize = 50;
        if (!Network.isClient && !Network.isServer && GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2, 500, 150), "Start Server", gs))
        {
            Network.InitializeServer(1, 20000, true);//!Network.HavePublicAddress());
            MasterServer.RegisterHost("MyGame", "RoomName");
        }
        if (!Network.isServer && !Network.isClient && GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 200, 500, 150), "Refresh Hosts", gs))
        {
            RefreshHostList();
        }

        if (hostList != null)
        {
            for (int i = 0; i < hostList.Length; i++)
            {
                if (!Network.isServer && !Network.isClient && GUI.Button(new Rect(Screen.width / 2 - 250 + (200 * i), Screen.height / 2 + 200, 150, 150), "Join Game", gs))
                {
                    JoinServer(hostList[i]);
                }
            }
        }

    }

    void OnServerInitialized()
    {
        Network.Instantiate(playerPrefab, new Vector3(-1.0f, 0.5f, -7.0f), Quaternion.identity, 0);
        //Application.LoadLevel("Game");
    }

    //void OnConnectedToServer()
    //{
    //    clientConnected = true;
    //    ipText.text = "Connected";
    //}

    private HostData[] hostList;

    private void RefreshHostList()
    {
        MasterServer.RequestHostList("MyGame");
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }
    
    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
        Network.Instantiate(playerPrefab, new Vector3(1.0f, 0.5f, -7.0f), Quaternion.identity, 0);
        //Application.LoadLevel("Game");
    }
}
