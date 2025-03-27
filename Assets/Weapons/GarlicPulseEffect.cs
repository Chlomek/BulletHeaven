using UnityEngine;

public class GarlicPulseEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rangeSprite;
    [SerializeField] private Color pulseColor = new Color(0.5f, 0.5f, 1f, 0.2f);
    [SerializeField] private float pulseSpeed = 1.5f;
    [SerializeField] private float minAlpha = 0.1f;
    [SerializeField] private float maxAlpha = 0.3f;

    private CircleCollider2D garlicCollider;
    private float pulseTimer = 0f;

    void Start()
    {
        garlicCollider = GetComponentInParent<CircleCollider2D>();

        if (rangeSprite == null)
        {
            rangeSprite = GetComponent<SpriteRenderer>();
        }

        rangeSprite.color = pulseColor;
    }

    void Update()
    {
        if (garlicCollider == null || rangeSprite == null) return;

        // Update the size based on collider radius
        transform.localScale = Vector3.one * garlicCollider.radius * 2;

        // Pulse effect
        pulseTimer += Time.deltaTime * pulseSpeed;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(pulseTimer) + 1) * 0.5f);
        Color newColor = pulseColor;
        newColor.a = alpha;
        rangeSprite.color = newColor;
    }
}