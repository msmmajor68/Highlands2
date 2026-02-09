using UnityEngine;
using FarmingRPG.Inventory;

namespace FarmingRPG.Player
{
    /// <summary>
    /// Main player controller that integrates inventory and other systems
    /// </summary>
    [RequireComponent(typeof(InventoryManager))]
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        private InventoryManager inventoryManager;
        private PlayerStats playerStats;
        private Animator animator;
        
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Rigidbody2D rb;
        
        [Header("Interaction")]
        [SerializeField] private float interactionRange = 2f;
        [SerializeField] private LayerMask interactableLayer;
        
        private Vector2 moveInput;
        private Vector2 lastMoveDirection = Vector2.down;
        
        private void Awake()
        {
            inventoryManager = GetComponent<InventoryManager>();
            playerStats = GetComponent<PlayerStats>();
            animator = GetComponent<Animator>();
            
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();
            
            // Ensure player has correct tag
            if (!CompareTag("Player"))
                tag = "Player";
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        private void FixedUpdate()
        {
            HandleMovement();
        }
        
        /// <summary>
        /// Handle player input
        /// </summary>
        private void HandleInput()
        {
            // Movement input
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            
            // Lock to dominant axis when moving diagonally
            Vector2 dominantInput = Vector2.zero;
            if (moveInput.sqrMagnitude > 0f)
            {
                // Update last facing direction with raw input
                lastMoveDirection = moveInput.normalized;
                
                // For animation, use only the dominant axis
                if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                {
                    // Horizontal is stronger
                    dominantInput = new Vector2(moveInput.x, 0).normalized;
                }
                else
                {
                    // Vertical is stronger
                    dominantInput = new Vector2(0, moveInput.y).normalized;
                }
            }

            if (animator != null)
            {
                animator.SetFloat("MoveX", dominantInput.x);
                animator.SetFloat("MoveY", dominantInput.y);
                animator.SetFloat("Speed", dominantInput.sqrMagnitude);
                animator.SetFloat("LastMoveX", lastMoveDirection.x);
                animator.SetFloat("LastMoveY", lastMoveDirection.y);
            }
            
            // Interaction input
            if (Input.GetKeyDown(KeyCode.F))
            {
                TryInteract();
            }
            
            // Drop item
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropSelectedItem();
            }
        }
        
        /// <summary>
        /// Handle player movement
        /// </summary>
        private void HandleMovement()
        {
            if (rb == null)
                return;
            
            // Lock to dominant axis when moving diagonally
            Vector2 dominantInput = Vector2.zero;
            if (moveInput.sqrMagnitude > 0f)
            {
                if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                {
                    // Horizontal is stronger
                    dominantInput = new Vector2(moveInput.x, 0).normalized;
                }
                else
                {
                    // Vertical is stronger
                    dominantInput = new Vector2(0, moveInput.y).normalized;
                }
            }
            
            // Apply speed multiplier from buffs
            float currentSpeed = moveSpeed * playerStats.SpeedMultiplier;
            Vector2 movement = dominantInput * currentSpeed;
            
            rb.linearVelocity = movement;
        }
        
        /// <summary>
        /// Try to interact with nearby objects
        /// </summary>
        private void TryInteract()
        {
            Vector2 interactionPoint = (Vector2)transform.position + lastMoveDirection * interactionRange;
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(interactionPoint, 0.5f, interactableLayer);
            
            if (colliders.Length > 0)
            {
                // Get the closest interactable
                Collider2D closest = colliders[0];
                float closestDistance = Vector2.Distance(transform.position, closest.transform.position);
                
                foreach (Collider2D col in colliders)
                {
                    float distance = Vector2.Distance(transform.position, col.transform.position);
                    if (distance < closestDistance)
                    {
                        closest = col;
                        closestDistance = distance;
                    }
                }
                
                // Try to interact
                IInteractable interactable = closest.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(gameObject);
                }
            }
        }
        
        /// <summary>
        /// Drop the currently selected hotbar item
        /// </summary>
        private void DropSelectedItem()
        {
            if (inventoryManager == null)
                return;
            
            InventorySlot selectedSlot = inventoryManager.GetSelectedHotbarSlot();
            
            if (selectedSlot != null && !selectedSlot.IsEmpty)
            {
                Item itemToDrop = selectedSlot.item;
                
                // Spawn the item in the world
                if (itemToDrop.worldPrefab != null)
                {
                    Vector3 dropPosition = transform.position + (Vector3)lastMoveDirection;
                    GameObject droppedItem = Instantiate(itemToDrop.worldPrefab, dropPosition, Quaternion.identity);
                    
                    // Set up the dropped item (you can add ItemPickup component to handle this)
                    ItemPickup pickup = droppedItem.GetComponent<ItemPickup>();
                    if (pickup != null)
                    {
                        pickup.SetItem(itemToDrop, 1);
                    }
                }
                
                // Remove from inventory
                inventoryManager.RemoveItem(itemToDrop, 1);
                Debug.Log($"Dropped {itemToDrop.itemName}");
            }
        }
        
        /// <summary>
        /// Pick up an item
        /// </summary>
        public bool PickupItem(Item item, int quantity = 1)
        {
            if (inventoryManager == null)
                return false;
            
            bool success = inventoryManager.AddItem(item, quantity);
            
            if (success)
            {
                Debug.Log($"Picked up {quantity}x {item.itemName}");
            }
            
            return success;
        }
        
        private void OnDrawGizmosSelected()
        {
            // Draw interaction range
            Gizmos.color = Color.yellow;
            Vector2 interactionPoint = (Vector2)transform.position + lastMoveDirection * interactionRange;
            Gizmos.DrawWireSphere(interactionPoint, 0.5f);
        }
    }
    
    /// <summary>
    /// Interface for interactable objects
    /// </summary>
    public interface IInteractable
    {
        void Interact(GameObject player);
    }
}
