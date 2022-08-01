using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.5f;
    public int interactableLayerIndex;
    private Vector3 raycastPos;
    public GameObject lookObject;
    private FPSGrab physicsObject;
    private Camera mainCamera;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;

    [Header("ObjectFollow")]
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float maxDistance = 10f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    //A simple visualization of the point we're following in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pickupParent.position, 0.5f);
    }

    //Interactable Object detections and distance check
    void Update()
    {
        //Here we check if we're currently looking at an interactable object
        raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.SphereCast(raycastPos, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, 1 << interactableLayerIndex))
        {

            lookObject = hit.collider.transform.root.gameObject;

        }
        else
        {
            lookObject = null;

        }



        //if we press the button of choice
        if (Input.GetButtonDown("Fire2"))
        {
            //and we're not holding anything
            if (currentlyPickedUpObject == null)
            {
                //and we are looking an interactable object
                if (lookObject != null)
                {

                    PickUpObject();
                }

            }
            //if we press the pickup button and have something, we drop it
            else
            {
                BreakConnection();
            }
        }


    }

    //Velocity movement toward pickup parent and rotation
    private void FixedUpdate()
    {
        if (currentlyPickedUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed *= Time.fixedDeltaTime;
            Vector3 direction = pickupParent.position - pickupRB.position;
            pickupRB.velocity = direction.normalized * currentSpeed;
            //Rotation
            lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
            lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
            pickupRB.MoveRotation(lookRot);
        }

    }

    //Release the object
    public void BreakConnection()
    {
        pickupRB.constraints = RigidbodyConstraints.None;
        currentlyPickedUpObject = null;
        physicsObject.pickedUp = false;
        currentDist = 0;
    }

    public void PickUpObject()
    {
        physicsObject = lookObject.GetComponentInChildren<FPSGrab>();
        currentlyPickedUpObject = lookObject;
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;
        physicsObject.playerInteractions = this;
        StartCoroutine(physicsObject.PickUp());

        if (physicsObject.tag == "Laptop")
        {
            info1 = true;
            Debug.Log("success");
        }

        if (physicsObject.tag == "Briefcase")
        {
            info2 = true;
            Debug.Log("success");
        }

        if (physicsObject.tag == "Phone")
        {
            info3 = true;
            Debug.Log("success");
        }
    }

    bool awakeBool = true;
    bool awakeBool2 = true;
    bool cubicalStory = true;
    bool info1 = false;
    bool info2 = false;
    bool info3 = false;
    bool cubicalStory2 = true;
    bool manager = true;
    bool toHallWayBool = true;
    bool CEODoor = true;
    bool CEOOfficeBool = true;
    bool staircaseStory = true;
    bool staircaseEntered = false;
    bool win = false;
    bool lose = false;



    [Header("Story")]
    public GameObject blockade;
    public GameObject awake;
    public GameObject awake2;
    public GameObject cubical;
    public GameObject cubical2;
    public GameObject puzzle1found;
    public GameObject managers;
    public GameObject puzzle2found;
    public GameObject toHallWay;
    public GameObject toCEO;
    public GameObject PuzzleDone;
    public GameObject PuzzleNotDone;
    public GameObject CEOOffice;
    public GameObject puzzle3found;
    public GameObject ToExit;
    public GameObject Staircase;
    public GameObject ManagerExit;
    public GameObject JanitorsCloset;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Awake" && awakeBool == true)
        {
            awake.SetActive(true);
        }

        if (other.tag == "Awake2" && awakeBool2 == true)
        {
            awake2.SetActive(true);
        }

        if (other.tag == "Blockade")
        {
            blockade.SetActive(true);
        }
        
        
        if (other.tag == "Cubical" && cubicalStory == true && info1 == false)
        {
            cubical.SetActive(true);
        }
        
        else if(other.tag == "Cubical" && cubicalStory == true && info1 == true)
        {
            cubical.SetActive(false);
            puzzle1found.SetActive(true);
        }

        if (other.tag == "Managers" && manager == true && info2 == false)
        {
            managers.SetActive(true);
        }

        else if (other.tag == "Managers" && manager == true && info2 == true)
        {
            managers.SetActive(false);
            puzzle2found.SetActive(true);
        }

        if (other.tag == "Cubical2" && cubicalStory2 == true)
        {
            cubical2.SetActive(true);
        }

        if (other.tag == "ToHallWay" && toHallWayBool == true)
        {
            if (info1 == true && info2 == true)
            {
                toCEO.SetActive(true);
            }
            
            else if (info1 == false && info2 == true)
            {
                toHallWay.SetActive(true);
            }

            else if (info1 == true && info2 == false)
            {
                toHallWay.SetActive(true);
            }

            else if (info1 == false && info2 == false)
            {
                toHallWay.SetActive(true);
            }
            
        }

        if (other.tag == "CEODoor" && CEODoor == true)
        {
            if(info1 == true && info2 == true)
            {
                PuzzleDone.SetActive(true);
            }

            else if (info1 == true && info2 == false)
            {
                PuzzleNotDone.SetActive(true);
            }

            else if (info1 == false && info2 == true)
            {
                PuzzleNotDone.SetActive(true);
            }

            else if (info1 == false && info2 == false)
            {
                PuzzleNotDone.SetActive(true);
            }

        }

        if (other.tag == "CEOOffice" && CEOOfficeBool == true && info3 == false)
        {
            CEOOffice.SetActive(true);
        }

        else if (other.tag == "CEOOffice" && CEOOfficeBool == true && info3 == true)
        {
            CEOOffice.SetActive(false);
            puzzle3found.SetActive(true);
        }

        if (other.tag == "ToExit" && info3 == true)
        {
            ToExit.SetActive(true);
        }

        if (other.tag == "Staircase" && staircaseStory == true)
        {
            Staircase.SetActive(true);
        }

        if (other.tag == "ManagerExit" && info3 == true)
        {
            win = true;
            StartCoroutine(EndGame());
        }

        if (other.tag == "JanitorCloset" && staircaseEntered == true)
        {
            lose = true;
            StartCoroutine(EndGame());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Awake")
        {
            awake.SetActive(false);
            awakeBool = false;

        }

        if (other.tag == "Awake2")
        {
            awake2.SetActive(false);
            awakeBool2 = false;
        }

        if (other.tag == "Blockade")
        {
            blockade.SetActive(false);
        }

        if (other.tag == "Cubical")
        {
            cubical.SetActive(false);
            puzzle1found.SetActive(false);
            
            if(info1 == true)
            {
                if (info1 == true && info2 == true)
                {
                    this.transform.tag = "PlayerCont2";
                    Debug.Log(this.tag);
                }
                cubicalStory = false;
            }
            
        }

        if (other.tag == "Cubical2")
        {
            cubical2.SetActive(false);
            cubicalStory2 = false;
        }

        if (other.tag == "Managers")
        {
            managers.SetActive(false);
            puzzle2found.SetActive(false);

            if (info2 == true)
            {
                if(info1 == true && info2 == true)
                {
                    this.transform.tag = "PlayerCont2";
                    Debug.Log(this.tag);
                }

                manager = false;
            }

        }

        if (other.tag == "ToHallWay")
        {
            toCEO.SetActive(false);
            toHallWay.SetActive(false);
            toHallWayBool = false;
        }

        if (other.tag == "CEODoor")
        {
            PuzzleDone.SetActive(false);
            PuzzleNotDone.SetActive(false);
            CEODoor = false;
        }

        if (other.tag == "CEOOffice")
        {
            CEOOffice.SetActive(false);
            puzzle3found.SetActive(false);

            if (info3 == true)
            {
                CEOOfficeBool = false;
            }

        }

        if (other.tag == "ToExit" && info3 == true)
        {
            ToExit.SetActive(false);
        }

        if (other.tag == "Staircase")
        {
            Staircase.SetActive(false);
            staircaseEntered = true;
            staircaseStory = false;
        }

    }
    
    IEnumerator EndGame()
    {
        if (win == true)
        {
            ManagerExit.SetActive(true);
        }

        else if (lose == true)
        {
            JanitorsCloset.SetActive(true);
        }

        yield return new WaitForSeconds(5);

        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


}