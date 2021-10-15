using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class PlayerMovement : NetworkBehaviour
{

    [SerializeField]
    [Tooltip("The speed the player moves.")]
    private float moveSpeed;

    private float xAxis;

    private CharacterController playerController;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();

        if (IsHost)
        {
            xAxis = -7;
        }
        else if (IsClient)
        {
            xAxis = 7;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer) { return; }

        MovePlayer();
        transform.position = new Vector3(xAxis, Mathf.Clamp(transform.position.y, -4,4), transform.position.z);
    }

    void MovePlayer()
    {
        Vector3 dir = new Vector3(0, Input.GetAxis("Vertical"), 0);

        playerController.Move(dir * moveSpeed * Time.deltaTime);
    }
}
