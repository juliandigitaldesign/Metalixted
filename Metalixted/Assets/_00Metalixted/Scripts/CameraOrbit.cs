using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    // Velocidad de rotación de la cámara
    public float rotateSpeed = 5f;

    // Velocidad de zoom de la cámara
    public float zoomSpeed = 5f;

    // Distancia mínima de la cámara al objetivo
    public float minDistance = 5f;

    // Distancia máxima de la cámara al objetivo
    public float maxDistance = 30f;

    // Distancia de la cámara al objetivo
    public float distance = 20f;

    // Transform del objetivo al que apunta la cámara
    public Transform target;

    // Ángulos de rotación de la cámara
    private float xAngle;
    private float yAngle;

    // Método que se llama al inicio del juego
    private void Start()
    {
        // Inicializar los ángulos de rotación según la posición inicial de la cámara
        Vector3 angles = transform.eulerAngles;
        xAngle = angles.y;
        yAngle = angles.x;
    }

    // Método que se llama cada frame del juego
    private void Update()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Obtener la posición del toque en la pantalla
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // Obtener la altura de la pantalla
            float screenHeight = Screen.height;

            // Si el toque está en la mitad superior de la pantalla, rotar la cámara
            if (touchPosition.y > screenHeight / 2f)
            {
                Rotate();
            }
        }
    }

    // Método que se llama para rotar la cámara según el desplazamiento del dedo en la pantalla
    private void Rotate()
    {
        // Obtener el vector2 del desplazamiento del dedo en la pantalla
        Vector2 inputVector = Touchscreen.current.primaryTouch.delta.ReadValue();
        
        // Actualizar los ángulos de rotación según el vector2 del input y la velocidad de rotación
        xAngle += inputVector.x * rotateSpeed * Time.deltaTime;
        yAngle -= inputVector.y * rotateSpeed * Time.deltaTime;

        // Limitar el ángulo vertical entre 20 y 40 grados para evitar giros extraños
        yAngle = Mathf.Clamp(yAngle, 20f, 40f);

        // Calcular la rotación de la cámara a partir de los ángulos calculados
        Quaternion rotation = Quaternion.Euler(yAngle, xAngle, 0f);

        // Calcular la posición de la cámara a partir de la distancia al objetivo y la rotación calculada
        Vector3 position = rotation * new Vector3(0f, 0f, -distance) + target.position;

        // Aplicar la posición y la rotación a la transformación de la cámara
        this.transform.position = position;
        this.transform.rotation = rotation;
    }
}
