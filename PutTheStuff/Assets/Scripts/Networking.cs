using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {

    private bool serverStarted;
    private bool clientConnected;
    public GUIText ipText;

	// Use this for initialization
	void Start () {
        serverStarted = false;
        clientConnected = false;
        Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnGUI()
    {
        GUIStyle gs = new GUIStyle(GUI.skin.GetStyle("Button"));
        gs.fontSize = 50;
        if (!Network.isClient && !serverStarted && !clientConnected && GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2, 500, 150), "Start Server", gs))
        {
            Network.InitializeServer(3, 172, true);//!Network.HavePublicAddress());
            MasterServer.RegisterHost("GameName", "RoomName");
        }
        if (!serverStarted && !clientConnected && GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 200, 500, 150), "Refresh Hosts", gs))
        {
            RefreshHostList();
        }
                        
        if (hostList != null) 
        {
            for (int i = 0; i < hostList.Length; i++)
            {
                if (!serverStarted && !clientConnected && GUI.Button(new Rect(Screen.width / 2 - 250 + (200*i), Screen.height / 2 + 200, 150, 150), "Join Game", gs))
                {
                    JoinServer(hostList[i]);
                }
            }
        }
        if (serverStarted)
        {
            ipText.text = Network.player.ipAddress + " " + Network.player.externalIP;
        }

    }

    void OnServerInitialized()
    {
        serverStarted = true;
        Application.LoadLevel("Game");
    }

    //void OnConnectedToServer()
    //{
    //    clientConnected = true;
    //    ipText.text = "Connected";
    //}

    private HostData[] hostList;

    private void RefreshHostList()
    {
        MasterServer.RequestHostList("GameName");
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
        clientConnected = true;
        ipText.text = "Connected";
        Application.LoadLevel("Game");
    }
}
