using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D player;
    private Animator animator;

    private Vector3 moveTarget = Vector3.zero;

    public float moveSpeed;

    void Start() {
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.SetFloat("MoveY", -1);
    }

    void Update() {
        moveTarget.x = Input.GetAxisRaw("Horizontal");
        moveTarget.y = Input.GetAxisRaw("Vertical");

        if (moveTarget.x > 0.5f || moveTarget.x < -0.5f || moveTarget.y > 0.5f || moveTarget.y < -0.5f) {
            animator.SetFloat("MoveX", moveTarget.x);
            animator.SetFloat("MoveY", moveTarget.y);
        }
    }

    private void FixedUpdate() {
        player.MovePosition(transform.position + moveTarget * moveSpeed * Time.deltaTime);
    }
}
