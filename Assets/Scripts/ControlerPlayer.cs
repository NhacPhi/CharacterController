using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlerPlayer : MonoBehaviour
{
    Animator m_animator;
    CharacterController m_character;

    public GameObject m_CharacterRoot;
    private Transform transformCharacterRoot;

    float Vertical = 0;
    float Horizontal = 0;
    bool isGrounded;

    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    Vector3 Move;

    float VelocityVetical = 0;

    // Handle Camera
    public float MouseSentitivity = 120;
    float m_HorizontalAngle;


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
    }

    // Update is called once per frame
    void Update()
    {
        //transformCharacterRoot.position = Vector3.zero;
        Vertical = Input.GetAxis("Vertical");
        if (Vertical < 0)
        {
            Vertical = 0;
        }
        Horizontal = Input.GetAxis("Horizontal");

        m_animator.SetFloat("VelocityY", Vertical);

        m_animator.SetFloat("VelocityX", Horizontal);

        isGrounded = m_character.isGrounded;
        //Debug.Log("Shift :" + isGrounded);
        if (isGrounded)
        {
           Move = Vector3.zero;
            Move = new Vector3(Horizontal, 0, Vertical);

            if (Move.sqrMagnitude > 0)
            {
                Move.Normalize();

                if (Input.GetKey(KeyCode.LeftShift) && Vertical != 0)
                {
                    //Debug.Log("Shift");
                    Move = Move * Time.deltaTime * runSpeed;
                    //m_animator.SetBool("IsRun", true);
                    m_animator.SetFloat("VelocityY", Vertical * 2);
                }
                else
                {
                    Move = Move * Time.deltaTime * walkSpeed;
                    //m_animator.SetBool("IsRun", false);
                }

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
}
