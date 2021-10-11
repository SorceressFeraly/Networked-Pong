using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class PlayerMovement : NetworkBehaviour
{

    [SerializeField]
    [Tooltip("The speed the player moves.")]
    private float moveSpeed;

    private CharacterController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        transform.position = new Vector3(-7, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer) { return; }

        MovePlayer();
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4,4), transform.position.z);
    }

    void MovePlayer()
    {
        Vector3 dir = new Vector3(0, Input.GetAxis("Vertical"), 0);

        playerController.Move(dir * moveSpeed * Time.deltaTime);
    }
}
