using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : GameManager
{
    [HideInInspector] //Our Character Controller, which moves and manipulates the player
    public CharacterController chara;
    [HideInInspector] //Our floats which detect and influence how the Character Controller behaves
    public float horizontalDetect, verticalDetect, speed;
    public Camera mainCam;
    public movementState curMove = movementState.walk;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainCam = Camera.main;
        if (mainCam == null)
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (base.playerLevel <= 1)
            base.playerLevel = 1;
        chara = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (UIManager.isPaused == false)
        {
            switch (curMove)
            {
                case movementState.walk:
                    speed = 1.1f;
                    break;
                case movementState.crouch:
                    speed = .85f;
                    break;
                case movementState.run:
                    speed = 1.65f;
                    break;
            }
            horizontalDetect = Input.GetAxisRaw("Horizontal");
            verticalDetect = Input.GetAxisRaw("Vertical");
            chara.Move(new Vector3(horizontalDetect, -9.81f, verticalDetect) * speed * Time.deltaTime);

            this.gameObject.transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X"), 0));
            mainCam.transform.Rotate(new Vector3(-(Input.GetAxisRaw("Mouse Y")), 0, 0));
        }
    }
    public enum movementState
    {
        walk,
        crouch,
        run
    };
}