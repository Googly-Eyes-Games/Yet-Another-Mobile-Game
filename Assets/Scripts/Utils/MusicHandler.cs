using DG.Tweening;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    private static MusicHandler instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.DOFade(1f, 2.0f);
        
        DontDestroyOnLoad(gameObject);
    }
}
