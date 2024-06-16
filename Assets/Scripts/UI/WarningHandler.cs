using System;
using erulathra;
using UnityEngine;

public class WarningHandler : MonoBehaviour
{
    [SerializeField]
    private OilValidator oilValidator;
    
    [SerializeField]
    private float distanceToStartFade = 15f;
    
    [SerializeField]
    private float distanceToEndFade = 10f;

    [SerializeField]
    private Renderer warningRenderer;

    private Material material;

    private void Awake()
    {
        material = warningRenderer.material;
    }

    void Update()
    {
        OilSpillsSubsystem oilSpillsSubsystem = SceneSubsystemManager.GetSubsystem<OilSpillsSubsystem>();
        float minDistanceSqrt = float.MaxValue;

        foreach (OilSpill oilSpill in oilSpillsSubsystem.ActiveOilSpills)
        {
            Vector2 nearestPointOnValidator = MathUtils.ClosestPointOnLineSegment2D(
                oilValidator.Start,
                oilValidator.End,
                oilSpill.transform.position);

            float distanceToValidatorSqrt = (oilSpill.transform.position.ToVector2() - nearestPointOnValidator).SqrMagnitude();
            minDistanceSqrt = Mathf.Min(minDistanceSqrt, distanceToValidatorSqrt);
        }

        float newAlpha = Mathf.InverseLerp(distanceToStartFade, distanceToEndFade, Mathf.Sqrt(minDistanceSqrt));
        material.SetFloat(ShaderLookUp.WarningAlpha, newAlpha);
    }

    private static class ShaderLookUp
    {
        public static int WarningAlpha = Shader.PropertyToID("_Alpha");
    }
}
