using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    private LoadBalancingClient client;

    // this is called when the client loaded and is ready to start
    void Start()
    {
        client = new LoadBalancingClient();
        client.AppId = "2926b240-406d-4491-aafb-6b260a169a8a";  // edit this!

        // "eu" is the European region's token
        bool connectInProcess = client.ConnectToRegionMaster("kr");  // can return false for errors
    }

    void Update()
    {
        client.Service();
    }

    void OnApplicationQuit()
    {
        client.Disconnect();
    }


    public GameLogic(string masterAddress, string appId, string gameVersion) : base(masterAddress, appId, gameVersion)
    {
        this.LocalPlayer.Name = "usr" + SupportClass.ThreadSafeRandom.Next() % 99;

        this.AutoJoinLobby = false;
        this.UseInterestGroups = true;
        this.JoinRandomGame = true;

        this.DispatchInterval = new TimeKeeper(10);
        this.SendInterval = new TimeKeeper(100);
        this.MoveInterval = new TimeKeeper(500);
        this.UpdateOthersInterval = new TimeKeeper(this.MoveInterval.Interval);
    }

}
