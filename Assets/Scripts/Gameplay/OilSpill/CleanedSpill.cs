using UnityEngine;

public class CleanedSpill : MonoBehaviour
{
    [SerializeField]
    private float timeToDissolve;

    private Material material;
    private float timeEnabled;
    
    private void OnEnable()
    {
        Renderer meshRenderer = GetComponent<Renderer>();
        material = meshRenderer.material;
        
        timeEnabled = Time.time;
    }

    void Update()
    {
        float dissolveFactor = (Time.time - timeEnabled) / timeToDissolve;
        dissolveFactor = Mathf.Clamp01(dissolveFactor);
        
        if (Time.time - timeEnabled > timeToDissolve)
        {
            Destroy(gameObject);
        }
        else
        {
            material.SetFloat(ShaderLookUp.DissolveFactor, dissolveFactor);
        }
    }

    private static class ShaderLookUp
    {
        public static readonly int DissolveFactor = Shader.PropertyToID("_DissolveFactor");
    }
}
