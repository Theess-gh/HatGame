using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Camera cam;

    private float maxWidth;

    public GameObject ball;

    public float timeLeft;

    public Text timerText;
    public GameObject[] endGameGameObjects;
    public HatController hatController;

    private bool IsGameEnded = false;

    void UpdateText()
    {
        timerText.text = "Time Left:" + Mathf.RoundToInt(timeLeft);
    }

    void Start()
    {
        UpdateText();
        if (cam == null)
        {
            cam = Camera.main;
        }

        Vector3 upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
        Vector3 targetWidth = cam.ScreenToWorldPoint(upperCorner);

        float ballWidth = ball.GetComponent<Renderer>().bounds.extents.x;
        maxWidth = targetWidth.x - ballWidth;

        // Запускаем создание шаров
        StartCoroutine(Spawn());
    }

    /// <summary>
    /// Уменьшаем время игры
    /// </summary>
    private void FixedUpdate()
    {
        if (!IsGameEnded)
        {
            UpdateText();
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                timeLeft = 0;

                IsGameEnded = true;
            }

            if (IsGameEnded)
            {
                timerText.enabled = false;
                foreach (GameObject endGameGameObject in endGameGameObjects)
                {
                    endGameGameObject.SetActive(true);
                }
                hatController.IsActive = false;
                IsGameEnded = false;
            }
        }
    }

    /// <summary>
    /// Создание шаров
    /// </summary>
    IEnumerator Spawn()
    {
        // Отложим созданеие шаров на 32 секунды
        yield return new WaitForSeconds(2.0f);

        while (timeLeft > 0)
        {
            Vector3 spawnPosition =
                new Vector3(Random.Range(-maxWidth, maxWidth),
                    transform.position.y,
                    0.0f);

            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(ball, spawnPosition, spawnRotation);

            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Second");
    }
}
