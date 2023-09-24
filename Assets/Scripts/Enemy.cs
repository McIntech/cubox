using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    private float objectWidth;

    private float xMin, xMax, yMin, yMax;


    public float changeInterval = 2f;

    private float currentRandomNumber;

    void Start()
    {
        objectWidth = GetComponent<Renderer>().bounds.size.x;
        // Definir los límites de la vista del juego
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        xMin = -cameraWidth + objectWidth / 2f;
        xMax = cameraWidth - objectWidth / 2f;
        yMin = -cameraHeight;
        yMax = cameraHeight;

        InvokeRepeating("GenerateRandomNumber", 0f, changeInterval);
    }

    void Update()
    {

    }

    void GenerateRandomNumber()
    {
        float randomNumber = Random.Range(xMin, xMax + 1);
        Debug.Log("Número aleatorio: " + randomNumber);

        float clampedX = Mathf.Clamp(randomNumber, xMin, xMax);
        Vector3 newPosition = new Vector3(clampedX, transform.position.y, transform.position.z);

        transform.position = newPosition;
    }
}
