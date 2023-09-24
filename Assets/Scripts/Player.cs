using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float pauseDuration = 2f; // Duración de la pausa en segundos
    private bool isJumping = false;
    private bool isPaused = false;
    private Rigidbody2D rb;
    private float xMin, xMax, yMin, yMax;
    private float objectWidth;

    private GameObject pointsObject;
    private TextMeshProUGUI pointsText;
    private int level = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Obtener el ancho del objeto
        objectWidth = GetComponent<Renderer>().bounds.size.x;

        // Definir los límites de la vista del juego considerando el ancho del objeto
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        xMin = -cameraWidth + objectWidth / 2f;
        xMax = cameraWidth - objectWidth / 2f;
        yMin = -cameraHeight;
        yMax = cameraHeight;

        // Encontrar el objeto con el componente de texto TextMeshProUGUI utilizando el tag "Points"
        pointsObject = GameObject.FindGameObjectWithTag("Points");
        pointsText = pointsObject.GetComponent<TextMeshProUGUI>();
        pointsText.text = "Level " + level.ToString();
    }

    void Update()
    {
        if (isPaused)
        {
            // The game is paused
            return;
        }

        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Adjust the positions within the boundaries
        float clampedX = Mathf.Clamp(rb.position.x, (xMin), (xMax));
        float clampedY = Mathf.Clamp(rb.position.y, yMin, yMax);
        rb.position = new Vector2(clampedX, clampedY);

        // Jump
        bool leftButtonPressed = GameObject.FindGameObjectWithTag("BtnIzquierdo")?.GetComponent<Button>()?.isPressed ?? false;
        bool rightButtonPressed = GameObject.FindGameObjectWithTag("BtnDerecho")?.GetComponent<Button>()?.isPressed ?? false;

        if (leftButtonPressed && rightButtonPressed && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            Debug.Log("Jump");
        }

        // Check if the top of the player has touched the top of the screen
        if (rb.position.y + (GetComponent<Renderer>().bounds.size.y / 2f) > yMax)
        {
            Debug.Log("Level passed");
            level++;
            pointsText.text = "Level " + level.ToString();
            RestartGame();
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            Debug.Log("Colisión con el suelo");
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ContactPoint2D contact = collision.contacts[0];
            if (contact.normal.y < -0.7f)
            {
                // Colisión desde la parte superior del jugador
                isJumping = false;
                Debug.Log("Perdiste");
                isPaused = true; // Pausar el juego
                Time.timeScale = 0f;
                StartCoroutine(ResumeGame());
            }
            else
            {
                // Continuar el juego si no se colisiona desde la parte superior del jugador
                Debug.Log("Continuar el juego");
                isJumping = false;
            }
        }
    }

    public void BtnLeftClick()
    {
        Debug.Log("Button left click");
    }

    public void BtnRigthClick()
    {
        Debug.Log("Button Rigth click");
    }

    private IEnumerator ResumeGame()
    {
        yield return new WaitForSecondsRealtime(pauseDuration);
        isPaused = false;
        Time.timeScale = 1f;

        // Desactivar o destruir el objeto con la etiqueta "Ground"
        GameObject groundObject = GameObject.FindGameObjectWithTag("Ground");
        GameObject instanceObject = GameObject.FindGameObjectWithTag("Instance");
        if (groundObject != null)
        {
            // Desactivar el objeto
            // groundObject.SetActive(false);

            // Destruir el objeto
            Destroy(groundObject);
            Destroy(instanceObject);
            Destroy(gameObject);
        }
    }

    private void RestartGame()
    {
        // Reiniciar el juego
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
