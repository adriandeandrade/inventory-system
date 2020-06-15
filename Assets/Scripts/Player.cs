using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement Configuration")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private ShopInventory shopInventory;

    // Private Variables
    private Vector2 movementVector;
    private float horizontalInput;
    private float verticalInput;

    // Components
    private Rigidbody2D rBody;
    private Interactable currentInteraction;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            shopInventory.InitializeShop(inventory);
        }
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            shopInventory.CloseShop();
        }

        GetInput();
    }

    private void FixedUpdate()
    {
        movementVector = new Vector2(horizontalInput, verticalInput) * moveSpeed;
        rBody.velocity = movementVector;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentInteraction = other.GetComponent<Interactable>();

        if(currentInteraction != null)
        {
            currentInteraction.OnInteractorInRange();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(currentInteraction != null)
        {
            currentInteraction.OnInteractorOutOfRange();
        }
    }
}
