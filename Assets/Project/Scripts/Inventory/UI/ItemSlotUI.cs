using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace FarmingRPG.Inventory.UI
{
    /// <summary>
    /// UI component for a single inventory slot
    /// </summary>
    public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("UI References")]
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private Image highlightImage;
        [SerializeField] private Image backgroundImage;
        
        [Header("Colors")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private Color selectedColor = Color.green;
        
        private InventorySlot slot;
        private int slotIndex;
        private InventoryUI inventoryUI;
        private Canvas canvas;
        private GameObject draggedIcon;
        
        public InventorySlot Slot => slot;
        public int SlotIndex => slotIndex;
        
        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            
            if (highlightImage != null)
                highlightImage.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Initialize the slot UI
        /// </summary>
        public void Initialize(int index, InventoryUI invUI)
        {
            slotIndex = index;
            inventoryUI = invUI;
        }
        
        /// <summary>
        /// Update the slot display
        /// </summary>
        public void UpdateSlot(InventorySlot inventorySlot)
        {
            slot = inventorySlot;
            
            if (slot == null || slot.IsEmpty)
            {
                ClearSlot();
                return;
            }
            
            // Update icon
            if (itemIcon != null)
            {
                itemIcon.sprite = slot.item.icon;
                itemIcon.enabled = true;
            }
            
            // Update quantity text
            if (quantityText != null)
            {
                if (slot.quantity > 1)
                {
                    quantityText.text = slot.quantity.ToString();
                    quantityText.enabled = true;
                }
                else
                {
                    quantityText.enabled = false;
                }
            }
        }
        
        /// <summary>
        /// Clear the slot display
        /// </summary>
        public void ClearSlot()
        {
            if (itemIcon != null)
                itemIcon.enabled = false;
            
            if (quantityText != null)
                quantityText.enabled = false;
            
            slot = null;
        }
        
        /// <summary>
        /// Set highlight state
        /// </summary>
        public void SetHighlight(bool enabled)
        {
            if (highlightImage != null)
                highlightImage.gameObject.SetActive(enabled);
        }
        
        /// <summary>
        /// Set selected state
        /// </summary>
        public void SetSelected(bool selected)
        {
            if (backgroundImage != null)
            {
                backgroundImage.color = selected ? selectedColor : normalColor;
            }
        }
        
        // Pointer Click Handler
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                inventoryUI?.OnSlotClicked(slotIndex);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                inventoryUI?.OnSlotRightClicked(slotIndex);
            }
        }
        
        // Pointer Enter Handler
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slot != null && !slot.IsEmpty)
            {
                ItemTooltip.Instance?.Show(slot.item, transform.position);
            }
        }
        
        // Pointer Exit Handler
        public void OnPointerExit(PointerEventData eventData)
        {
            ItemTooltip.Instance?.Hide();
        }
        
        // Begin Drag Handler
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (slot == null || slot.IsEmpty)
                return;
            
            // Create dragged icon
            draggedIcon = new GameObject("Dragged Icon");
            draggedIcon.transform.SetParent(canvas.transform);
            draggedIcon.transform.SetAsLastSibling();
            
            Image draggedImage = draggedIcon.AddComponent<Image>();
            draggedImage.sprite = slot.item.icon;
            draggedImage.raycastTarget = false;
            
            RectTransform rectTransform = draggedIcon.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 50);
            
            CanvasGroup canvasGroup = draggedIcon.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
        
        // Drag Handler
        public void OnDrag(PointerEventData eventData)
        {
            if (draggedIcon != null)
            {
                draggedIcon.transform.position = eventData.position;
            }
        }
        
        // End Drag Handler
        public void OnEndDrag(PointerEventData eventData)
        {
            if (draggedIcon != null)
            {
                Destroy(draggedIcon);
            }
            
            // Check if dropped on another slot
            if (eventData.pointerEnter != null)
            {
                ItemSlotUI targetSlot = eventData.pointerEnter.GetComponent<ItemSlotUI>();
                if (targetSlot != null && targetSlot != this)
                {
                    inventoryUI?.SwapSlots(slotIndex, targetSlot.SlotIndex);
                    return;
                }
            }
            
            // If shift is held and not dropped on a slot, drop the item
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                DropItemToWorld();
            }
        }
        
        /// <summary>
        /// Drop the item from this slot into the world
        /// </summary>
        private void DropItemToWorld()
        {
            if (slot == null || slot.IsEmpty || inventoryUI == null)
                return;
            
            // Access InventoryManager through a public method
            inventoryUI.DropItemFromInventory(slotIndex, slot.quantity);
        }
    }
}
