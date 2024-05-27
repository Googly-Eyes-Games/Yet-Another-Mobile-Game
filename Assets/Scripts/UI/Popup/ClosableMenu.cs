using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseablePopup : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    public Action OnClose;

    private void OnEnable() 
    {
        closeButton.onClick.AddListener(HandleCloseButton);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(HandleCloseButton);
    }

    private void HandleCloseButton()
    {
        OnClose?.Invoke();
        gameObject.SetActive(false);
    }
}
