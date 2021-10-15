using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;


public class BallMovement : NetworkBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float speedIncrease = 0.5f;

    private NetworkVariable<float> randomDirX = new NetworkVariable<float>();
    private NetworkVariable<float> randomDirY = new NetworkVariable<float>();

    [SerializeField]
    private Vector3 randomDir;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomDirectionServerRpc();
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(randomDir * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colliding!");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Colliding with player!");
            randomDir = new Vector3(randomDir.x * -1, randomDir.y, 0);
            speed = speed + speedIncrease;
        }

        else if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Colliding with wall!");
            randomDir = new Vector3(randomDir.x, randomDir.y * -1, 0);
            speed = speed + speedIncrease;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GoalTriggerP1") gm.P2Score.Value++;
        if (other.gameObject.name == "GoalTriggerP2") gm.P1Score.Value++;

        gm.DeSpawnBallServerRpc(NetworkObjectId);
    }

    [ServerRpc]
    void SetRandomDirectionServerRpc()
    {
        randomDirX.Value = Random.Range(-1.0f, 1.0f);
        randomDirY.Value = Random.Range(-1.0f, 1.0f);
        randomDir = new Vector3(randomDirX.Value, randomDirY.Value, 0).normalized;
    }
}
