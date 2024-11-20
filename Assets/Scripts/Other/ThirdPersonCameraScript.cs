using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraScript : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;

    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;
    public enum CameraStyle
    {
        Exploration,
        Combat,
        Topdown
    }
    public CameraStyle currentCamStyle;
    //public GameObject ExploreCam;
    //public GameObject CombatCam;
    public GameObject TopDownCam;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        //rotate player obj
        if(currentCamStyle == CameraStyle.Exploration)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float veritcalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * veritcalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }

        else if(currentCamStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObject.forward = dirToCombatLookAt.normalized;
        }

        //TEMPORY_____forTESTpurposes______________________________________________________________________________________________________________
        /*if(Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamType(CameraStyle.Combat);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchCamType(CameraStyle.Exploration);
        }*/
        //_________________________________________________________________________________________________________________________________________
    }

    /*private void SwitchCamType(CameraStyle newStyle)
    {
        ExploreCam.SetActive(false);
        CombatCam.SetActive(false);

        if (newStyle == CameraStyle.Exploration) ExploreCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) CombatCam.SetActive(true);

        currentCamStyle = newStyle;
    }*/
}
