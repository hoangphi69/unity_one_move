using UnityEngine;

public class OutlineHighlight : MonoBehaviour
{
    [Header("Highlight Settings")]
    [SerializeField] private Color highlightColor = Color.red;
    [SerializeField] private float emissionIntensity = 2f;
    
    private Renderer meshRenderer;
    private Material material;
    private bool isHighlighted = false;
    
    void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        if (meshRenderer != null)
        {
            material = meshRenderer.material;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", Color.black);
        }
    }
    
    public void Show()
    {
        if (!isHighlighted && material != null)
        {
            material.SetColor("_EmissionColor", highlightColor * emissionIntensity);
            isHighlighted = true;
        }
    }
    
    public void Hide()
    {
        if (isHighlighted && material != null)
        {
            material.SetColor("_EmissionColor", Color.black);
            isHighlighted = false;
        }
    }
    
    void OnDestroy()
    {
        if (material != null)
        {
            Destroy(material);
        }
    }
}