using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject startUI; // UI element to show "Tap to Start"
    [SerializeField] private GameObject countUI; // UI to show when the game starts
    [SerializeField] private GameObject GameOverUI; // UI for Game Over
    [SerializeField] private PlayerJump PlayerJump;
    [SerializeField] private Image GameOverImage; // Image to fade in on Game Over

    [SerializeField] private Transform Player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Rigidbody PlayerRigidbody;

    [SerializeField] Image restartbutton;

    [SerializeField] TextMeshProUGUI CurrentScore;

    [SerializeField] TextMeshProUGUI BestScore;

    [SerializeField] Counter counter;

    [SerializeField] GameObject[] array;

    [SerializeField] Image Scoreboard;


    private bool gameStarted = false;
    private bool isGameOver = false; // Prevent multiple GameOver calls
    private CancellationTokenSource cts;
    private CancellationTokenSource upDownCts; // For controlling up-and-down movement
    private bool isSceneLoading = false;

    void Start()
    {
        PlayerRigidbody.isKinematic = true;


        Application.targetFrameRate = 30;
        if (startUI != null)
        {
            startUI.SetActive(true);
        }

        cts = new CancellationTokenSource();
        WaitForStart(cts.Token).Forget();
    }

    private async UniTaskVoid WaitForStart(CancellationToken token)
    {
        try
        {
            StartUpDownMovement(0.25f, 0.45f);
            await UniTask.WaitUntil(() =>
                !gameStarted && (Input.GetMouseButtonDown(0) || Input.touchCount > 0),
                cancellationToken: token);
            StartGame();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Game start waiting was canceled.");
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        PlayerRigidbody.isKinematic = false;
        StopUpDownMovement();
        spawner.StartSpawning();

        

        if (startUI != null)
        {
            startUI.SetActive(false);
        }

        if (countUI != null)
        {
            countUI.SetActive(true);
        }

        // Start up-and-down movement
        // Amplitude 1, Speed 1.5

        cts?.Cancel();
        cts?.Dispose();
    }

    public void GameOver()
    {
        if (isGameOver) return; // Prevent multiple calls
        isGameOver = true;

        Lean.Pool.LeanPool.DespawnAll();

        if (counter.GetCount() > PlayerPrefs.GetInt("bestscore", 0))
       {
                PlayerPrefs.SetInt("bestscore", counter.GetCount());
       }
       
       CurrentScore.text = counter.GetCount().ToString();
       BestScore.text = PlayerPrefs.GetInt("bestscore", 0).ToString();

        

        GameObject[] rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();



        foreach (GameObject rootObject in rootGameObjects)
        {
            if (rootObject.name.Contains("LeanPool"))
            {
                // Make the LeanPool GameObject persistent
                DontDestroyOnLoad(rootObject);
                Debug.Log($"LeanPool '{rootObject.name}' and its children are now persistent.");
            }
        }

        GameOverUI.SetActive(true);
        countUI.SetActive(false);
        PlayerJump.GO();
        spawner.GO();
        FadeInImage(GameOverImage, 2.5f).Forget();
        FadeInImage(restartbutton, 2.5f).Forget();
        FadeInImage(Scoreboard, 2.5f).Forget();


        // Stop up-and-down movement

    }

    private async UniTask FadeInImage(Image image, float duration)
    {
        if (image.color.a >= 1f) return; // Prevent multiple lerps

        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            color.a = Mathf.Lerp(0f, 1f, t);
            image.color = color;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        color.a = 1f; // Ensure the alpha is exactly 1 at the end
        image.color = color;
        Time.timeScale = 0.00f;
    }

    public void ReloadCurrentScene()
    {
        if (isSceneLoading) return; // Prevent multiple reloads
        isSceneLoading = true; // Set the flag to indicate scene loading has started

       

        // Reload the scene while keeping specific objects intact
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name).completed += (operation) =>
        {
            isSceneLoading = false; // Reset the flag once loading completes
          
            Debug.Log("Current scene reloaded successfully.");
            Time.timeScale = 1f;
        };
    }


    public void OnPause()
    {
        float f = Time.timeScale;

        if(f>=1f)
        {
            Time.timeScale = 0f;
        }
        else if(f<=0f)
        {
            Time.timeScale = 1f;
        }
        
    }

    private void StartUpDownMovement(float amplitude, float speed)
    {
        // Cancel any existing movement
        upDownCts?.Cancel();
        upDownCts = new CancellationTokenSource();

        // Start the movement
        MoveUpAndDownAsync(amplitude, speed, upDownCts.Token).Forget();
    }

    private void StopUpDownMovement()
    {
        // Stop the up-and-down movement
        upDownCts?.Cancel();
        upDownCts?.Dispose();
    }

    private async UniTask MoveUpAndDownAsync(float amplitude, float speed, CancellationToken token)
    {
        float startY = Player.position.y;

        while (!token.IsCancellationRequested)
        {
            // Calculate the new Y position using a sine wave
            float newY = startY + Mathf.Sin(Time.time * speed) * amplitude;
            Vector3 newPosition = new Vector3(Player.position.x, newY, Player.position.z);

            Player.position = newPosition;

            // Yield until the next frame
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();

        upDownCts?.Cancel();
        upDownCts?.Dispose();
    }
}
