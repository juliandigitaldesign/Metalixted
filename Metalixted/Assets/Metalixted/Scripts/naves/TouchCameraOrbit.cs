using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCameraOrbitIgnoreBottom : MonoBehaviour
{
    public Transform target; // El objeto alrededor del cual orbitar� la c�mara
    public float distance = 10.0f; // La distancia entre la c�mara y el objeto
    public float xSpeed = 250.0f; // La velocidad de rotaci�n en el eje X
    public float ySpeed = 120.0f; // La velocidad de rotaci�n en el eje Y
    public float bottomScreenIgnoreThreshold = 0.2f; // La parte inferior de la pantalla a ignorar (en porcentaje)

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // Obtener la posici�n del toque en la pantalla
                Vector2 touchPosition = Input.GetTouch(0).position;

                // Verificar si el toque est� fuera de la parte inferior de la pantalla a ignorar
                if (touchPosition.y > Screen.height * bottomScreenIgnoreThreshold)
                {
                    // Obtener el desplazamiento del dedo en la pantalla
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                    // Rotar alrededor del objeto
                    x += touchDeltaPosition.x * xSpeed * distance * Time.deltaTime;
                    y -= touchDeltaPosition.y * ySpeed * Time.deltaTime;

                    // Restringir el �ngulo de rotaci�n en el eje Y
                    y = ClampAngle(y, -90, 90);

                    // Actualizar la posici�n y rotaci�n de la c�mara
                    Quaternion rotation = Quaternion.Euler(y, x, 0);
                    Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

                    transform.rotation = rotation;
                    transform.position = position;
                }
            }
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}