using UnityEngine;
using TMPro;

namespace FarmingRPG.Inventory.UI
{
    /// <summary>
    /// Tooltip display for items when hovering
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        public static ItemTooltip Instance { get; private set; }
        
        [Header("UI References")]
        [SerializeField] private RectTransform tooltipRect;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemTypeText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private CanvasGroup canvasGroup;
        
        [Header("Settings")]
        [SerializeField] private Vector2 offset = new Vector2(10, -10);
        [SerializeField] private float fadeSpeed = 10f;
        
        private bool isVisible = false;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            Hide();
        }
        
        private void Update()
        {
            if (isVisible && canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            }
            else if (!isVisible && canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                if (canvasGroup.alpha <= 0f)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        
        /// <summary>
        /// Show tooltip for an item
        /// </summary>
        public void Show(Item item, Vector3 position)
        {
            if (item == null)
                return;
            
            gameObject.SetActive(true);
            isVisible = true;
            
            // Set item info
            if (itemNameText != null)
                itemNameText.text = item.itemName;
            
            if (itemTypeText != null)
                itemTypeText.text = item.itemType.ToString();
            
            if (descriptionText != null)
                descriptionText.text = item.description;
            
            if (priceText != null)
            {
                if (item.sellPrice > 0)
                    priceText.text = $"Sell: {item.sellPrice}g";
                else
                    priceText.text = "";
            }
            
            // Position tooltip
            tooltipRect.position = position + (Vector3)offset;
            
            // Keep tooltip on screen
            Canvas.ForceUpdateCanvases();
            ClampToScreen();
        }
        
        /// <summary>
        /// Hide the tooltip
        /// </summary>
        public void Hide()
        {
            isVisible = false;
        }
        
        private void ClampToScreen()
        {
            Vector3[] corners = new Vector3[4];
            tooltipRect.GetWorldCorners(corners);
            
            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            
            float xMin = canvasRect.rect.xMin;
            float xMax = canvasRect.rect.xMax;
            float yMin = canvasRect.rect.yMin;
            float yMax = canvasRect.rect.yMax;
            
            Vector3 pos = tooltipRect.position;
            
            // Check right edge
            if (corners[2].x > xMax)
                pos.x -= (corners[2].x - xMax);
            
            // Check left edge
            if (corners[0].x < xMin)
                pos.x += (xMin - corners[0].x);
            
            // Check top edge
            if (corners[1].y > yMax)
                pos.y -= (corners[1].y - yMax);
            
            // Check bottom edge
            if (corners[0].y < yMin)
                pos.y += (yMin - corners[0].y);
            
            tooltipRect.position = pos;
        }
    }
}
