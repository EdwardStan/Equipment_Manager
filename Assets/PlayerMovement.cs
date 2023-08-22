using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkingSpeed;

    Rigidbody2D rb;
    Vector3 playerMovement;
    Animator anim;

    [SerializeField] GameObject EquippedClothes;
    [SerializeField] GameObject EquippedHair;


    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        playerMovement = Vector3.zero;
        playerMovement.x = Input.GetAxisRaw("Horizontal");
        playerMovement.y = Input.GetAxisRaw("Vertical");

        UpdateAnimationOnMove();
    }

    void UpdateAnimationOnMove()
    {
        rb.MovePosition(transform.position + playerMovement * walkingSpeed * Time.deltaTime);

        anim.SetFloat("Horizontal", playerMovement.x);
        anim.SetFloat("Vertical", playerMovement.y);
        anim.SetFloat("Speed", playerMovement.sqrMagnitude);

        EquippedClothes.GetComponent<Animator>().SetFloat("Horizontal", playerMovement.x);
        EquippedClothes.GetComponent<Animator>().SetFloat("Vertical", playerMovement.y);
        EquippedClothes.GetComponent<Animator>().SetFloat("Speed", playerMovement.sqrMagnitude);
        EquippedHair.GetComponent<Animator>().SetFloat("Horizontal", playerMovement.x);
        EquippedHair.GetComponent<Animator>().SetFloat("Vertical", playerMovement.y);
        EquippedHair.GetComponent<Animator>().SetFloat("Speed", playerMovement.sqrMagnitude);

    }
}

