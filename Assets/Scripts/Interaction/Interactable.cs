using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour, IInteractable
{
    [Header("Interactable Configuration")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Interactable UI Configuration")]
    [SerializeField] private GameObject interactKeyPrompt;

    // Events
    public Action OnInteracted;
    public Action OnInteractionInRange;
    public Action OnInteractionOutOfRange;

    // Private Variables
    private bool hasInteraction = false; // When the valid interactor has interacted with the interaction.
    private bool hasInteractor = false; // When a valid interactor is within the interaction radius.

    private void Update()
    {
        if (Input.GetKeyDown(interactKey) && !hasInteraction)
        {
            Interact();
        }
    }

    public void EnableInteractionPrompt()
    {
        interactKeyPrompt.SetActive(true);
        hasInteractor = true;
    }

    public void DisableInteractionPrompt()
    {
        //interactKeyPrompt.SetActive(false);
        hasInteractor = false;
        hasInteraction = false;
    }

    public void Interact()
    {
        hasInteraction = true;
        interactKeyPrompt.SetActive(false);
        OnInteracted?.Invoke();
    }

    public void OnInteractorInRange()
    {
        EnableInteractionPrompt();
        OnInteractionInRange?.Invoke();
    }

    public void OnInteractorOutOfRange()
    {
        DisableInteractionPrompt();
        OnInteractionOutOfRange?.Invoke();
    }
}
