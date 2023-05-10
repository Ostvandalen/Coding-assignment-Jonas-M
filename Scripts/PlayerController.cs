using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true; // determine whether or not a player is in control or not
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey) && currentInput.x > 0; //Checks whether the player can sprint or not
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded; // checks if the player can jump
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded; //checks if the player can crouch
    
    // public Slider slider;
    

    //Before changing values in the code check if you can do it in the inspector since it is easier


    [Header("Options")] // Just check the abilities on/off in the inspector
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool WillSlideOnSlopes = true;
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool useStamina = true;


    [Header("Controls")] // Assigned keys can be altered in the inspector
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.C;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode zoomkey = KeyCode.Mouse1;
    
    

    [Header("Movement")] //To keep it clean in the inspector
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float slopeSpeed = 8f;


    [Header("Look")]

    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f; //lookspeed x
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f; //lookspeed y
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f; //amount of degrees the player can look up
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f; //amount of degrees the player can look down


    [Header("Health")]
    [SerializeField] private float maxHealth = 100; // Player max health
    [SerializeField] private float timeBeforeRegenStarts = 3; // time amount before the player regenerates health
    [SerializeField] private float healthValueIncrement = 1; // How much health the player regenerates
    [SerializeField] private float healthTimeIncrement = 0.1f;
    private float currentHealth;
    private Coroutine regeneratingHealth;
    public static Action<float> OnTakeDamage;
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;


    [Header("Splatter Image")]
    //[SerializeField] private Image redSplatterImage = null;
    [SerializeField] private Image splatterImage = null;
    [SerializeField] private float splatterTimer = 0.1f;

    [Header("Hurt Image Flash")]
    [SerializeField] private Image hurtImage = null;
    [SerializeField] private float hurtTimer = 0.1f;


    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaUseMultiplier = 5;
    [SerializeField] private float timeBeforeStaminaRegenStarts = 5;
    [SerializeField] private float staminaValueIncrement = 2;
    [SerializeField] private float staminaTimeIncrement = 0.1f;
    private float currentStamina;
    private Coroutine regeneratingStamina;
    public static Action<float> OnStaminaChange;


    [Header("Jumping")]
    [SerializeField] private float jumpForce = 8.0f; //amount of height the player can jump. Can be adjusted in the inspector
    [SerializeField] private float gravity = 30.0f; //amount of gravity can be adjusted in the inspector

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.5f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;


    [Header("Headbob")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;


    [Header("Zoom")]
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 30f;
    private float defaultFOV;
    private Coroutine zoomRoutine;


    [Header("Footsteps")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodSounds = default;
    [SerializeField] private AudioClip[] metalSounds = default;
    [SerializeField] private AudioClip[] grassSounds = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

    // Sliding doesnt need header

    private Vector3 hitPointNormal;

    private bool IsSliding
    {
        get
        {

            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;


    private Camera playerCamera; // reference to the camera
    private CharacterController characterController; // reference to the character controller

    private Vector3 moveDirection; //move amount applied to the character
    private Vector2 currentInput; // value given to the controller

    private float rotationX = 0;

    public static PlayerController instance;

    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
    }

    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }


    void Awake()
    {
        instance = this;

        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        Cursor.lockState = CursorLockMode.Locked; // Locking the cursor so the cursor doesnt interfere with the gameplay
        Cursor.visible = false;
        
    }


    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if (canJump)
                HandleJump();
            if (canCrouch)
                HandleCrouch();
            if (canUseHeadbob)
                HandleHeadBob();

            if (canZoom)
                HandleZoom();

            if (useFootsteps)
                Handle_Footsteps();

            if (canInteract)
                HandleInteractionCheck();
                HandleInteractionInput();
            if (useStamina)
                HandleStamina();

            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2((IsSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed) * Input.GetAxis("Vertical"), (IsSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed) * Input.GetAxis("Horizontal"));


       


        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump)
            moveDirection.y = jumpForce;
    }

    private void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
    }

    private void HandleHeadBob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
               playerCamera.transform.localPosition.x,
               defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount),
               playerCamera.transform.localPosition.z);
        }
    }

    private void HandleStamina()
    {
        if(IsSprinting && currentInput != Vector2.zero) //makes sure that stamina isnt used when standing still
        {
            if(regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }

            currentStamina -= staminaUseMultiplier * Time.deltaTime;

            if (currentStamina < 0)
                currentStamina = 0;

            OnStaminaChange?.Invoke(currentStamina);


            if (currentStamina <= 0)
                canSprint = false; // disables ability to sprint when stamina reach zero
        }

        if(IsSprinting && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
        }
    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(zoomkey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }
        if (Input.GetKeyUp(zoomkey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }

    private void HandleInteractionCheck() //looks for interactable objects
    {
        if(Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
           if(hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                    currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLooseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput() //player input on interactable objects
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }

    

    private void Handle_Footsteps()
    {
        if (!characterController.isGrounded) return; //if the player is jumping no footsteps will play
        if (currentInput == Vector2.zero) return; //if player stands still no footsteps will play

        footstepTimer -= Time.deltaTime;

        if(footstepTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3)) //checks which material player is walking on with raycast
            {
                switch ( hit.collider.tag) //This is the list with the different sfx depending of ground material. Add more by adding cases and create a Tag in unity with the same name remember to add a higher number depending of how many cases you add
                {
                    case "Footsteps/WOOD":
                        footstepAudioSource.PlayOneShot(woodSounds[UnityEngine.Random.Range(0, woodSounds.Length - 1)]);
                        break;
                    case "Footsteps/METAL":
                        footstepAudioSource.PlayOneShot(metalSounds[UnityEngine.Random.Range(0, metalSounds.Length - 1)]);
                        break;
                    case "Footsteps/GRASS":
                        footstepAudioSource.PlayOneShot(grassSounds[UnityEngine.Random.Range(0, grassSounds.Length - 1)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(grassSounds[UnityEngine.Random.Range(0, grassSounds.Length - 1)]);
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }

   // void UpdateHealth()
    //{
      //  Color splatterAlpha = redSplatterImage.color;
       // splatterAlpha.a = 1 - (currentHealth / maxHealth);
        //redSplatterImage.color = splatterAlpha;
    //}

    IEnumerator HurtFlash()
    {
        hurtImage.enabled = true;
        
        FindObjectOfType<AudioManager>().Play("Hurt");
        yield return new WaitForSeconds(hurtTimer);
        hurtImage.enabled = false;
        
    }

    IEnumerator SplatterFlash()
    {
        splatterImage.enabled = true;
        yield return new WaitForSeconds(splatterTimer);
        splatterImage.enabled = false;
    }

    private void ApplyDamage(float dmg)
    {
        

        currentHealth -= dmg;
        OnDamage?.Invoke(currentHealth);

        if(currentHealth >= 0)
        {
            StartCoroutine(HurtFlash());
            StartCoroutine(SplatterFlash());
        }


        if (currentHealth <= 0)
            KillPlayer(); // if the players health reach zero the player dies
        else if (regeneratingHealth != null)
            StopCoroutine(regeneratingHealth);

        regeneratingHealth = StartCoroutine(RegenerateHealth());
    }

    private void KillPlayer()
    {
        currentHealth = 0;
        

        if (regeneratingHealth != null)
            StopCoroutine(regeneratingHealth);

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reloads scene when player dies
        print("Dead"); // Put in the function for what happens when the player dies here
    }


    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        if (WillSlideOnSlopes && IsSliding)
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;

        characterController.Move(moveDirection * Time.deltaTime);
    }


    private IEnumerator CrouchStand()
    {

        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break; // checks if theres anything above the player while crouching if there is the player cant stand up

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while (timeElapsed > timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(timeBeforeRegenStarts); //time before regen starts
        WaitForSeconds timeToWait = new WaitForSeconds(healthTimeIncrement);

        while(currentHealth < maxHealth)
        {
            currentHealth += healthValueIncrement;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            OnHeal?.Invoke(currentHealth);
            yield return timeToWait;
        }

        regeneratingHealth = null;

    }

    private IEnumerator RegenerateStamina() //isnt used in the example game
    {
        yield return new WaitForSeconds(timeBeforeStaminaRegenStarts);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaTimeIncrement);

        while(currentStamina < maxStamina)
        {
            if (currentStamina > 0)
                canSprint = true;

            currentStamina += staminaValueIncrement;

            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            OnStaminaChange?.Invoke(currentStamina);

            yield return timeToWait;
        }

        regeneratingStamina = null;
    }
}