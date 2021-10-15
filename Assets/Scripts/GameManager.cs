using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class GameManager : NetworkBehaviour
{

    [SerializeField]
    [Tooltip("Ball.")]
    private NetworkObject ballPrefab;

    [SerializeField]
    public NetworkVariable<int> P1Score = new NetworkVariable<int>(0);
    [SerializeField]
    public NetworkVariable<int> P2Score = new NetworkVariable<int>(0);


    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
            StartGameButton();
        }

        GUILayout.EndArea();
    }

    void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    void StartGameButton()
    {
        if (GUILayout.Button("Start Game")) { StartGame(); }
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    void StartGame()
    {
        SpawnBallServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    void SpawnBallServerRpc()
    {
        NetworkObject ballObject = Instantiate(ballPrefab);
        ballObject.Spawn();
   //     SpawnBallClientRpc();
    }

    [ClientRpc]
    void SpawnBallClientRpc()
    {
        NetworkObject ballObject = Instantiate(ballPrefab);
        ballObject.Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeSpawnBallServerRpc(ulong id)
    {
        NetworkObject ballObject = GetNetworkObject(id);
        ballObject.Despawn();
    }

}
