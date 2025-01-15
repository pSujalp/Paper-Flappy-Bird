using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private int counter = 0;

    [SerializeField] GameObject PointSound;

    private void Awake()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                Debug.LogError("TextMeshProUGUI component is not assigned or found.");
            }
        }
    }
    private void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                Debug.LogError("TextMeshProUGUI component is not assigned or found.");
            }
        }
    }
    public int GetCount()
    {
        return counter;
    }

    public void IncreaseCount()
    {
        counter++;
        if (textMeshPro != null)
        {
            textMeshPro.text = counter.ToString();
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI is not assigned. Cannot update the text.");
        }

        if (PointSound != null)
        {
            GameObject obj = Lean.Pool.LeanPool.Spawn(PointSound,transform.position,Quaternion.identity);
            Lean.Pool.LeanPool.Despawn(obj, 1.05f);

        }
    }
}
