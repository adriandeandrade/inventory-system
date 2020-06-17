using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Button Animation Configuration")]
    [SerializeField] private bool doScaleAnimation = true;
    [SerializeField] private bool switchSpritesManually = false;
    [SerializeField] private Vector3 hoverSize = new Vector3(1.1f, 1.1f, 1f);
    [Space]
    [SerializeField] private Sprite originalSprite;
    [SerializeField] private Sprite hoveredSprite;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite disabledSprite;

    // Private Variables
    private bool isDisabled = false;

    // Events
    public UnityEvent OnClicked;
    public UnityEvent OnMouseEnter;
    public UnityEvent OnMouseExit;

    // Components
    protected Image buttonImage;

    private void OnEnable()
    {
        buttonImage = GetComponent<Image>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnClicked?.Invoke();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(doScaleAnimation)
            buttonImage.transform.DOScale(hoverSize, 0.1f).SetUpdate(true);

        if (switchSpritesManually)
            SetHoveredSprite();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (doScaleAnimation)
            buttonImage.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f).SetUpdate(true);

        if (switchSpritesManually)
            SetOriginalSprite();
    }

    public void Enable()
    {
        isDisabled = false;
    }

    public void Disable()
    {
        isDisabled = true;
    }

    public void SetSelectedSprite()
    {
        if (isDisabled || selectedSprite == null) return;

        buttonImage.sprite = selectedSprite;
    }

    public void SetHoveredSprite()
    {
        if (isDisabled) return;
        buttonImage.sprite = hoveredSprite;
    }

    public void SetOriginalSprite()
    {
        if (isDisabled) return;
        buttonImage.sprite = originalSprite;
    }

    public void SetDisabledSprite()
    {
        if (isDisabled || disabledSprite == null) return;
        buttonImage.sprite = disabledSprite;
    }
}
