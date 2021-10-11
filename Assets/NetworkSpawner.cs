using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class NetworkSpawner : NetworkBehaviour
{

    [SerializeField]
    private GameObject[] spawnPoints;

    [SerializeField]
    private GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    private Transform GetSpawnPoint()
    {
        if (spawnPoints.Length == 0) { Debug.Log("Error, no spawnpoints in game object!"); return null; }

        if (NetworkManager.Singleton.IsHost)
        {
            return spawnPoints[0].transform;
        }

        else
        {
            ulong id = NetworkManager.Singleton.LocalClientId;
            int count = (int)(id - 1);

            return (spawnPoints)[count].transform;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientId)
    {
        // Get Spawn.  Stop if there are no spawn points in the seen
        Transform spawn = GetSpawnPoint();
        if (spawn == null) { Debug.Log("No Spawn Points in Scene!"); return; }

        // Spawn on Client
        GameObject playerToInstantiate = Instantiate(playerPrefab, spawn.position, spawn.rotation);


        Debug.Log("clientId = " + clientId);
        playerToInstantiate.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        ulong objectId = playerToInstantiate.GetComponent<NetworkObject>().NetworkObjectId;

        SpawnClientRpc(objectId);

    }

    // A ClientRpc can be invoked by the server to be executed on a client
    [ClientRpc]
    private void SpawnClientRpc(ulong objectId)
    {
       // NetworkObject player = NetworkSpawnManager.SpawnedObjects[objectId];
    }
}
