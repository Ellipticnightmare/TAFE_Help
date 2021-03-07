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
    public Camera mainCam; //Our camera
    public movementState curMove = movementState.walk; //A state controller to determine movement speeds and other variables
    public int playerLevel, playerEXP;
    string jump, crouch, shoot, run;
    private void Start()
    {
        Cursor.visible = false; //Hides cursor
        Cursor.lockState = CursorLockMode.Locked; //Confined cursor to center of screen
        mainCam = Camera.main; //Locate Camera
        if (mainCam == null)
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //Backup Locate Camera
        if (playerLevel <= 1)
            playerLevel = 1; //Set player level
        chara = GetComponent<CharacterController>(); //Find Character Controller Variable
    }
    private void Update()
    {
        if (UIManager.isPaused == false) //Determine if game is paused
        {
            //Register keybindings from playerPrefs
            KeyCode jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump"));
            KeyCode crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch"));
            KeyCode run = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Run"));
            KeyCode shoot = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Shoot"));
            if (Input.GetKey(run)) //run
                curMove = movementState.run;
            else if (Input.GetKey(crouch)) //crouch
                curMove = movementState.crouch;
            else //default to walk
                curMove = movementState.walk;
            if (Input.GetKeyDown(jump)) //Test Jump
                Debug.Log("Ki");
            switch (curMove) //Adjust stats based off of current movement type
            {
                case movementState.walk:
                    speed = 1.1f;
                    break;
                case movementState.crouch:
                    speed = .85f;
                    break;
                case movementState.run:
                    speed = 2.25f;
                    break;
            }
            horizontalDetect = Input.GetAxisRaw("Horizontal"); //Take in A + D input
            verticalDetect = Input.GetAxisRaw("Vertical"); //Take in S + W input
            chara.Move(new Vector3(horizontalDetect, -9.81f, verticalDetect) * speed * Time.deltaTime); //Move character controller

            this.gameObject.transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X"), 0)); //Rotate left to right
            mainCam.transform.Rotate(new Vector3(-(Input.GetAxisRaw("Mouse Y")), 0, 0)); //Look up and down
        }
    }
    public enum movementState //Categorize the different movement options
    {
        walk,
        crouch,
        run
    };
}