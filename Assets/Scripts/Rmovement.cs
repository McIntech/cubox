using UnityEngine;

public class Rmovement : MonoBehaviour
{
    private float objectWidth;
    private float xMin, xMax, yMin, yMax;
    public float changeInterval = 2f;

    private void Start()
    {
        objectWidth = GetComponent<Renderer>().bounds.size.x;
        // Definir los límites de la vista del juego
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        xMin = -cameraWidth + objectWidth / 2f;
        xMax = cameraWidth - objectWidth / 2f;
        yMin = -cameraHeight;
        yMax = cameraHeight;

        InvokeRepeating("GenerateRandomPosition", 0f, changeInterval);
    }

    private void GenerateRandomPosition()
    {
        float randomX = Random.Range(xMin, xMax);
        float randomY = transform.position.y;

        Vector3 newPosition = new Vector3(randomX, randomY, transform.position.z);

        transform.position = newPosition;
    }
}
