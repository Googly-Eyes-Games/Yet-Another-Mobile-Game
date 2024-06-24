using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(OilCleaner))]
public class CleanerVFX : MonoBehaviour
{
    [SerializeField]
    private float cleanFactorToPerfectClean = 0.4f;

    [Foldout("Sprites")]
    [SerializeField]
    private Texture perfectSprite;
    
    [Foldout("Sprites")]
    [SerializeField]
    private Texture tooBigSprite;
    
    [Foldout("Sprites")]
    [SerializeField]
    private Texture tooSmallSprite;
    
    private ParticleSystem feedbackVFX;
    private ParticleSystemRenderer feedbackVFXRenderer;
    private OilCleaner oilCleaner;

    private void Awake()
    {
        feedbackVFX = GetComponent<ParticleSystem>();
        feedbackVFXRenderer = GetComponent<ParticleSystemRenderer>();
        oilCleaner = GetComponent<OilCleaner>();
        oilCleaner.OnCleanSpill += OnCleanSpill;
        oilCleaner.OnPartlyClean += OnPartlyClean;
    }

    private void OnPartlyClean()
    {
#if !UNITY_WEBGL
        Handheld.Vibrate();
#endif
    }

    private void OnCleanSpill(float cleanFactor)
    {
        Texture particleTexture;
        
        if (cleanFactor > cleanFactorToPerfectClean)
        {
            particleTexture = perfectSprite;
        }
        else if (cleanFactor > 0f)
        {
            particleTexture = tooBigSprite;
        }
        else
        {
            particleTexture = tooSmallSprite;
        }
        
        Material particleMaterial = feedbackVFXRenderer.material;
        particleMaterial.SetTexture(ShaderLookup.BaseMap, particleTexture);
        feedbackVFX.Play();
    }

    private static class ShaderLookup
    {
        public static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    }
}
