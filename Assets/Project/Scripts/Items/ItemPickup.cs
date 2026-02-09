using UnityEngine;
using FarmingRPG.Inventory;

namespace FarmingRPG.Player
{
    /// <summary>
    /// Component for items that can be picked up in the world
    /// </summary>
    public class ItemPickup : MonoBehaviour
    {
        [Header("Item Data")]
        public Item item;
        public int quantity = 1;
        
        [Header("Visuals")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float bobSpeed = 1f;
        [SerializeField] private float bobHeight = 0.2f;
        
        [Header("Pickup Settings")]
        [SerializeField] private float pickupRange = 1.5f;
        [SerializeField] private bool autoPickup = true;
        [SerializeField] private float pickupDelay = 0.5f; // Delay before item can be auto-picked up
        
        private Vector3 startPosition;
        private float bobTimer = 0f;
        private float pickupTimer = 0f; // Timer for pickup delay
        
        private void Start()
        {
            startPosition = transform.position;
            UpdateVisuals();
            pickupTimer = 0f; // Start timer at 0
        }
        
        /// <summary>
        /// Update sprite to match the item
        /// </summary>
        public void UpdateVisuals()
        {
            if (item != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = item.icon;
            }
        }
        
        private void Update()
        {
            // Bob animation
            bobTimer += Time.deltaTime * bobSpeed;
            Vector3 newPosition = startPosition;
            newPosition.y += Mathf.Sin(bobTimer) * bobHeight;
            transform.position = newPosition;
            
            // Increment pickup timer
            pickupTimer += Time.deltaTime;
            
            // Auto pickup check (only after delay has passed)
            if (autoPickup && pickupTimer >= pickupDelay)
            {
                CheckForPlayer();
            }
        }
        
        /// <summary>
        /// Set the item for this pickup
        /// </summary>
        public void SetItem(Item newItem, int newQuantity)
        {
            item = newItem;
            quantity = newQuantity;
            
            if (spriteRenderer != null && item != null)
            {
                spriteRenderer.sprite = item.icon;
            }
        }
        
        /// <summary>
        /// Reset the pickup delay timer (call when item is newly dropped)
        /// </summary>
        public void ResetPickupDelay()
        {
            pickupTimer = 0f;
        }
        
        /// <summary>
        /// Check for nearby player to pick up
        /// </summary>
        private void CheckForPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                
                if (distance <= pickupRange)
                {
                    TryPickup(player);
                }
            }
        }
        
        /// <summary>
        /// Try to pick up the item
        /// </summary>
        public bool TryPickup(GameObject player)
        {
            if (item == null || quantity <= 0)
                return false;
            
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                bool success = playerController.PickupItem(item, quantity);
                
                if (success)
                {
                    // Play pickup sound/effect here
                    Destroy(gameObject);
                    return true;
                }
            }
            
            return false;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (autoPickup && collision.CompareTag("Player"))
            {
                TryPickup(collision.gameObject);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, pickupRange);
        }
    }
}
