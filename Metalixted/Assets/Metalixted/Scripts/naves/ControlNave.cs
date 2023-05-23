using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class ControlNave : MonoBehaviourPun
{

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _joystick;


    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity;
    private Vector3 move = Vector3.zero;

    public Transform _meshPlayer;

    public float xSpeed = 5.0f;
    public float ySpeed = 5.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    public float bottomScreenIgnoreThreshold = 0.35f; // La parte inferior de la pantalla a ignorar (en porcentaje)


    private void FixedUpdate() 
    {
        move = new Vector3(_joystick.GetComponent<JoystickReader>().touchDirection.x * _moveSpeed, 0.0f, _joystick.GetComponent<JoystickReader>().touchDirection.y * _moveSpeed);
        move = transform.TransformDirection(move) * _moveSpeed * 10 *Time.deltaTime;

        if (_joystick.GetComponent<JoystickReader>().touchDirection.x != 0 && _joystick.GetComponent<JoystickReader>().touchDirection.y != 0)
        {
            _meshPlayer.transform.rotation = Quaternion.LookRotation(move);
        }

        move.y -= _gravity * 100 * Time.deltaTime;
        _rigidbody.velocity = move;


        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Obtener la posición del toque en la pantalla
            Vector2 touchPosition = Input.GetTouch(0).position;

            // Verificar si el toque está fuera de la parte inferior de la pantalla a ignorar
            if (touchPosition.y > Screen.height * bottomScreenIgnoreThreshold)
            {
                // Obtener el desplazamiento del dedo en la pantalla
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                // Rotar alrededor del objeto
                x += touchDeltaPosition.x * xSpeed * Time.deltaTime;
                y -= touchDeltaPosition.y * ySpeed * Time.deltaTime;

                // Restringir el ángulo de rotación en el eje Y
                y = ClampAngle(y, -90, 90);

                // Actualizar la rotación del jugador con la camara.
                Quaternion rotation = Quaternion.Euler(y, x, 0);

                transform.rotation = rotation;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //_camera.parent = null;
    }
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }    
}
