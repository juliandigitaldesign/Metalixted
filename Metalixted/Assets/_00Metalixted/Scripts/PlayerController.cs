using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{


    #region ControllPlayer

    private NetworkVariable<MyCustomData> ramdomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 56,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable 
    { 
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        ramdomNumber.OnValueChanged += (MyCustomData previusValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; ramdomnumber: " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };
    }

    // Variables Control Movimiento
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject meshPlayer;
    [SerializeField] private Vector2 JoystickSize = new Vector2(200,200);
    [SerializeField] private JoystickTouch Joystick;
    [SerializeField] private float playerSpeed = 3f;

    [SerializeField] public bool IsGrounded = false;
    [SerializeField] private float gravity;
    private Vector3 move = Vector3.zero;
    public float xSpeed = 5.0f;
    public float ySpeed = 5.0f;

    //Movimiento del dedo sobre la pantalla
    private Finger MovementFinger;
    public Vector3 MovementAmount;

    // Ángulos de rotación
    private float xAngle;
    private float yAngle;
    public float rotateSpeed = 5f;

    //Variables a ignorar de otros jugadores
    public MonoBehaviour[] IgnoreCodes;

    //Variables Control Disparo
    [SerializeField] private Vector2 JoystickSizeShooter = new Vector2(200, 200);
    [SerializeField] private JoystickShooter JoystickShooter;
    private Finger MovementFinger2;
    public Vector3 MovementAmount2;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (!IsOwner)
        {
            foreach (var codes in IgnoreCodes)
            {
                codes.enabled = false;
                this.GetComponentInChildren<Canvas>().enabled = false;
            }
        }
    }  

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;

        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += get_touch_details; //Show in console position of touch
    }
    /*
     void get_touch_details(Finger fin)
    {
        Debug.Log(fin.screenPosition);
    }
     */


    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (MovementFinger == null
            && touchedFinger.screenPosition.x <= Screen.width / 2f
            && touchedFinger.screenPosition.y <= Screen.height / 4f)
        {
            MovementFinger = touchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }

        if (MovementFinger == null
            && touchedFinger.screenPosition.x >= Screen.width / 2f
            && touchedFinger.screenPosition.y <= Screen.height / 4f)
        {
            MovementFinger2 = touchedFinger;
            MovementAmount2 = Vector2.zero;
            JoystickShooter.gameObject.SetActive(true);
            JoystickShooter.RectTransform.sizeDelta = JoystickSizeShooter;
            JoystickShooter.RectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition) - new Vector2(Screen.width, 0);
        }
    }

    private void HandleFingerMove(Finger movedFinger)
    {

        ETouch.Touch currentTouch = movedFinger.currentTouch;

        //inner Joystick Movement///////////////////////////////////////////////////
        if (movedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float MaxMovement = JoystickSize.x / 2f;

            if (Vector2.Distance(
                currentTouch.screenPosition,
                Joystick.RectTransform.anchoredPosition
                ) > MaxMovement)
            {
                knobPosition = (
                                currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition
                    ).normalized
                    * MaxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / MaxMovement;
        }

        //inner Joystick Shooter////////////////////////////////////////////////
        if (movedFinger == MovementFinger2)
        {
            Vector2 knobPosition2;
            float MaxMovement2 = JoystickSizeShooter.x / 2f;
            knobPosition2 = currentTouch.screenPosition - new Vector2(Screen.width, 0) - JoystickShooter.RectTransform.anchoredPosition;

            JoystickShooter.knob.anchoredPosition = knobPosition2;
            MovementAmount2 = knobPosition2 / MaxMovement2;
        }

    }
    
    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.gameObject.transform.position = new Vector2(250, 250);
            Joystick.knob.anchoredPosition = Vector2.zero;
            MovementAmount = Vector2.zero;
        }

        if (lostFinger == MovementFinger2)
        { 
            MovementFinger2 = null;
            JoystickShooter.gameObject.transform.position = new Vector2(Screen.width -250, 250);
            JoystickShooter.knob.anchoredPosition = Vector2.zero;
            MovementAmount2 = Vector2.zero;
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        //CLAMP MOVEMENT
        if (startPosition.x < JoystickSize.x / 2)
        {
            startPosition.x = JoystickSize.x / 2;
        }

        if (startPosition.y < JoystickSize.y / 2)
        {
            startPosition.y = JoystickSize.y / 2;
        }
        else if (startPosition.y > Screen.height - JoystickSize.y / 2) 
        {
            startPosition.y = Screen.height - JoystickSize.y / 2;
        }
        return startPosition;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;        

        if (MovementAmount.x != 0 && MovementAmount.y != 0)
        {
            gravity -= gravity * Time.deltaTime;
            Vector3 m_Input = new Vector3(MovementAmount.x, 0.0f, MovementAmount.y);
            move = transform.TransformDirection(m_Input);
            rb.MovePosition(transform.position + move * Time.deltaTime * playerSpeed);

            if (MovementAmount2.magnitude <= 0.6f)
            {
                meshPlayer.transform.rotation = Quaternion.LookRotation(move);
            }


        }
        if (MovementAmount2.magnitude >= 0.6f)
        {
            Vector3 m_Input = new Vector3(MovementAmount2.x, 0.0f, MovementAmount2.y);
            Vector3 move2 = transform.TransformDirection(m_Input);
            meshPlayer.transform.rotation = Quaternion.LookRotation(move2);
        }

        try
        {
            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                // Obtener la posición del toque en la pantalla
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

                // Obtener la altura de la pantalla
                float screenHeight = Screen.height;

                // Si el toque está en la mitad superior de la pantalla, rotar 
                if (touchPosition.y > screenHeight / 2.5f)
                {
                    Rotate();
                }
            }
        }
        catch { }        
    }
    public void Rotate()
    {
        Vector2 inputVector = Touchscreen.current.primaryTouch.delta.ReadValue();

        xAngle += inputVector.x * rotateSpeed * Time.deltaTime;
        yAngle -= inputVector.y * rotateSpeed * Time.deltaTime;

        yAngle = Mathf.Clamp(yAngle, 0f, 0f);

        Quaternion rotation = Quaternion.Euler(yAngle, xAngle, 0f);
        transform.rotation = rotation;
    }

    
    #endregion

    #region Update
    private void Update()
    {
        if (!IsOwner) return;

        //Eject Bullets System part1...
        if (Time.time > fireRate + lastShot && shooterPress == true)
        {
            SpawnBulletServerRpc();
            lastShot = Time.time;
        }

    }
    #endregion

    #region Collisions
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        { // using Tags instead of name checking is recommended.
            IsGrounded = true;
            gravity = 1;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            IsGrounded = false;
            gravity = 999;
        }
    }
    #endregion

    #region RPC Bullets System

    //Variables Bullets System
    [SerializeField] private Transform bulletSpawnPrefab;
    public GameObject startBullet;
    bool shooterPress = false;
    public float fireRate = 0.5f;
    private float lastShot = 0.0f;

    //Eject Bullets System part2...
    public void Onclick_ShootON()
    {
        shooterPress = true;
    }

    public void Onclick_ShootOFF()
    {
        shooterPress = false;
    }
    
    [ServerRpc]
    private void SpawnBulletServerRpc()
    {
        Instantiate(bulletSpawnPrefab, startBullet.transform.position, startBullet.transform.rotation);
        ShowBulletClientRpc();
    }

    [ClientRpc]
    void ShowBulletClientRpc()
    {
        Instantiate(bulletSpawnPrefab, startBullet.transform.position, startBullet.transform.rotation);
    }
    #endregion

    #region RPC Heal System

    //HEAL SYSTEM
    public Image healthBar;
    private float maxHealth = 1f;
    public float curretHealth = 1f;


    [ServerRpc(RequireOwnership = false)]
    public void DamageServerRpc()
    {
        curretHealth -= 0.001f;
        healthBar.fillAmount = curretHealth;

        if(curretHealth <= 0.8f)
            healthBar.color = Color.red;

        if (curretHealth <= 0.3f)
            healthBar.color = Color.yellow;

        if (curretHealth <= 0.0f)
        {
            curretHealth = maxHealth;
            healthBar.fillAmount = curretHealth;
            healthBar.color = Color.green;
        }
        DamageClientRpc();
    }

    [ClientRpc]
    public void DamageClientRpc()
    {
        curretHealth -= 0.001f;
        healthBar.fillAmount = curretHealth;

        if (curretHealth <= 0.8f)
            healthBar.color = Color.yellow;

        if (curretHealth <= 0.3f)
            healthBar.color = Color.red;

        if (curretHealth <= 0.0f)
        {
            healthBar.color = Color.green;
            curretHealth = maxHealth;
            healthBar.fillAmount = curretHealth;
        }
    }



    #endregion
}
