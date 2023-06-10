using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    // Velocidad de rotaci�n de la c�mara
    public float rotateSpeed = 5f;

    // Velocidad de zoom de la c�mara
    public float zoomSpeed = 5f;

    // Distancia m�nima de la c�mara al objetivo
    public float minDistance = 5f;

    // Distancia m�xima de la c�mara al objetivo
    public float maxDistance = 30f;

    // Distancia de la c�mara al objetivo
    public float distance = 20f;

    // Transform del objetivo al que apunta la c�mara
    public Transform target;

    // �ngulos de rotaci�n de la c�mara
    private float xAngle;
    private float yAngle;

    // M�todo que se llama al inicio del juego
    private void Start()
    {
        // Inicializar los �ngulos de rotaci�n seg�n la posici�n inicial de la c�mara
        Vector3 angles = transform.eulerAngles;
        xAngle = angles.y;
        yAngle = angles.x;
    }

    // M�todo que se llama cada frame del juego
    private void Update()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Obtener la posici�n del toque en la pantalla
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // Obtener la altura de la pantalla
            float screenHeight = Screen.height;

            // Si el toque est� en la mitad superior de la pantalla, rotar la c�mara
            if (touchPosition.y > screenHeight / 2f)
            {
                Rotate();
            }
        }
    }

    // M�todo que se llama para rotar la c�mara seg�n el desplazamiento del dedo en la pantalla
    private void Rotate()
    {
        // Obtener el vector2 del desplazamiento del dedo en la pantalla
        Vector2 inputVector = Touchscreen.current.primaryTouch.delta.ReadValue();
        
        // Actualizar los �ngulos de rotaci�n seg�n el vector2 del input y la velocidad de rotaci�n
        xAngle += inputVector.x * rotateSpeed * Time.deltaTime;
        yAngle -= inputVector.y * rotateSpeed * Time.deltaTime;

        // Limitar el �ngulo vertical entre 20 y 40 grados para evitar giros extra�os
        yAngle = Mathf.Clamp(yAngle, 20f, 40f);

        // Calcular la rotaci�n de la c�mara a partir de los �ngulos calculados
        Quaternion rotation = Quaternion.Euler(yAngle, xAngle, 0f);

        // Calcular la posici�n de la c�mara a partir de la distancia al objetivo y la rotaci�n calculada
        Vector3 position = rotation * new Vector3(0f, 0f, -distance) + target.position;

        // Aplicar la posici�n y la rotaci�n a la transformaci�n de la c�mara
        this.transform.position = position;
        this.transform.rotation = rotation;
    }
}
