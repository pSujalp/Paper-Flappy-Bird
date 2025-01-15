using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SpriteBillboarding : MonoBehaviour
{
    [SerializeField] Camera targetCamera; // The camera the sprite should face
    private CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        if(targetCamera==null)targetCamera = Camera.main;
    }

    private void OnEnable()
    {
        // Create a new CancellationTokenSource when the GameObject is enabled
        cancellationTokenSource = new CancellationTokenSource();

        // Start the billboarding task
        StartBillboarding(cancellationTokenSource.Token).Forget();
    }

    private void OnDisable()
    {
        // Cancel the billboarding task when the GameObject is disabled
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        cancellationTokenSource = null;
    }

    private async UniTaskVoid StartBillboarding(CancellationToken token)
    {
        if (targetCamera == null)
        {
            // Use the main camera if no target camera is specified
            targetCamera = Camera.main;
        }

        while (!token.IsCancellationRequested)
        {
            // Make the sprite face the camera
            if (targetCamera != null)
            {
                transform.LookAt(targetCamera.transform);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f); // Lock X and Z rotation
            }

            // Yield until the next frame
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }
}
