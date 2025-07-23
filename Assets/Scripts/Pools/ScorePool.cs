using UnityEngine;


public class ScorePool : MonoBehaviour
{
    public static ScorePool Instance { get; private set; }
    [SerializeField] private UniversalText scoreTextPrefab;
    [SerializeField] private int initialPoolSize = 20;
    [SerializeField] public Canvas targetCanvas;

    private ObjectPool<UniversalText> _pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
            _pool = new ObjectPool<UniversalText>(scoreTextPrefab, initialPoolSize, transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public UniversalText GetText(Vector3 worldPosition)
    {        
        UniversalText scoreText = _pool.GetObject();
        scoreText.transform.SetParent(targetCanvas.transform, false);

        // ����������� ������� ������� � ��������
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

        // ����������� �������� ������� � ��������� ������� RectTransform �������
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetCanvas.GetComponent<RectTransform>(),
            screenPoint,
            null, // null ��� CanvasOverlay
            out Vector2 localPoint);

        // ������������� ������� ������
        scoreText.GetComponent<RectTransform>().anchoredPosition = localPoint;

        return scoreText;
    }

    public void ReturnText(UniversalText scoreText)
    {
        _pool.ReturnObject(scoreText);
    }    
}
