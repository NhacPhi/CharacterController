using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ControlerPlayer : MonoBehaviour
{
    Animator m_animator;
    CharacterController m_character;

    public GameObject m_CharacterRoot;
    private Transform transformCharacterRoot;
    // Movement
    float Vertical = 0;
    float Horizontal = 0;
    bool isGrounded;

    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    Vector3 Move;
    public float jumpSpeed = 2f;
    public float jumpVelocity = 5f;
    float VelocityVetical = 0;

    // Handle Camera
    public float MouseSentitivity = 120;
    float m_HorizontalAngle;

    // Handle Camera
    [SerializeField] CinemachineVirtualCamera thirPersonCam;
    [SerializeField] CinemachineFreeLook freeVirtualCam;
    bool isCam = true;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_animator = m_CharacterRoot.gameObject.GetComponent<Animator>();
        transformCharacterRoot = m_CharacterRoot.gameObject.GetComponent<Transform>();

        m_character = GetComponent<CharacterController>();

        isGrounded = true;

        m_HorizontalAngle = transform.localEulerAngles.y;

        // Cam
        thirPersonCam.Priority = 11;
        freeVirtualCam.Priority = 10;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (isCam)
                OnEnable();
            else
                OnDisable();
        }
        Vertical = Input.GetAxis("Vertical");

        Horizontal = Input.GetAxis("Horizontal");

        m_animator.SetFloat("VelocityY", Vertical);

        m_animator.SetFloat("VelocityX", Horizontal);

        isGrounded = m_character.isGrounded;
        Move = Vector3.zero;
        Move = new Vector3(Horizontal, 0, Vertical);
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_animator.SetTrigger("Jump");
                StartCoroutine(StartJump());
            }

            if (Move.sqrMagnitude > 0)
            {
                Move.Normalize();

                if (Input.GetKey(KeyCode.LeftShift) /*&& Vertical != 0*/)
                {

                    Move = Move * Time.deltaTime * runSpeed;

                    if (Vertical != 0)
                        m_animator.SetFloat("VelocityY", Vertical * 2);
                    if (Horizontal != 0)
                        m_animator.SetFloat("VelocityX", Horizontal * 2);
                }
                else
                {
                    Move = Move * Time.deltaTime * walkSpeed;
                }

                Move = transform.TransformDirection(Move);

                m_character.Move(Move);
            }

        }
        else
        {
            // Jump movement
            if (Move.sqrMagnitude > 0)
            {
                Move.Normalize();

                Move = Move * Time.deltaTime * jumpSpeed;

                Move = transform.TransformDirection(Move);

                m_character.Move(Move);
            }
        }
        // handle gravity
        VelocityVetical = VelocityVetical - 9.8f * Time.deltaTime;
        if (VelocityVetical < -9.8f)
        {
            VelocityVetical = -9.8f;
        }
        Vector3 verticalSpeed = new Vector3(0, VelocityVetical * Time.deltaTime, 0);
        m_character.Move(verticalSpeed);

        // Player rotation by mouse

        // Axis Horizontal with CharactorRoot
        float turnPlayer = Input.GetAxis("Mouse X") * MouseSentitivity;
        Mathf.Round(turnPlayer);
        transform.Rotate(Vector3.up * turnPlayer);


        // Reset 
        m_CharacterRoot.gameObject.transform.localPosition = Vector3.zero;
        m_CharacterRoot.gameObject.transform.localEulerAngles = Vector3.zero;
    }
    IEnumerator StartJump()
    {
        yield return new WaitForSeconds(0.5f);
        VelocityVetical = jumpVelocity;
    }

    private void OnEnable()
    {
        thirPersonCam.Priority = 10;
        freeVirtualCam.Priority = 11;
        isCam = false;
    }
    private void OnDisable()
    {
        thirPersonCam.Priority = 11;
        freeVirtualCam.Priority = 10;
        isCam = true;
    }
}
