using UnityEngine;
using UnityEngine.UI;
using FarmingRPG.Player;

namespace FarmingRPG.UI
{
    /// <summary>
    /// Character Sheet Window - Shows detailed player stats, equipment, and attributes
    /// Opened with C key, togglable overlay
    /// This is Phase 2 of character UI - detailed information display
    /// </summary>
    public class CharacterSheetUI : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private CanvasGroup characterSheetPanel;
        
        [Header("Button References")]
        [SerializeField] private Button closeButton;
        
        [Header("Character Info")]
        [SerializeField] private Text characterNameText;
        [SerializeField] private Text levelText;
        [SerializeField] private Text experienceText;
        
        [Header("Stats Display")]
        [SerializeField] private Text healthValueText;
        [SerializeField] private Text staminaValueText;
        [SerializeField] private Text hungerValueText;
        [SerializeField] private Text defenseValueText;
        [SerializeField] private Text attackValueText;
        
        [Header("Buff Display")]
        [SerializeField] private Text activeBuffsText;
        
        private bool isOpen = false;
        
        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindObjectOfType<PlayerStats>();
            }
            
            if (characterSheetPanel == null)
            {
                Debug.LogError("CharacterSheetUI: Character sheet panel not assigned!");
                enabled = false;
                return;
            }
            
            // Close button
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Toggle);
            }
            
            // Subscribe to stat changes
            if (playerStats != null)
            {
                playerStats.OnHealthChanged += UpdateStatsDisplay;
                playerStats.OnStaminaChanged += UpdateStatsDisplay;
                playerStats.OnHungerChanged += UpdateStatsDisplay;
            }
            
            // Start closed
            Close();
            UpdateStatsDisplay(0, 0); // Trigger initial display
        }
        
        private void OnDestroy()
        {
            if (playerStats != null)
            {
                playerStats.OnHealthChanged -= UpdateStatsDisplay;
                playerStats.OnStaminaChanged -= UpdateStatsDisplay;
                playerStats.OnHungerChanged -= UpdateStatsDisplay;
            }
            
            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Toggle);
            }
        }
        
        private void Update()
        {
            // Toggle on C key
            if (Input.GetKeyDown(KeyCode.C))
            {
                Toggle();
            }
        }
        
        /// <summary>
        /// Toggle character sheet open/closed
        /// </summary>
        public void Toggle()
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
        
        /// <summary>
        /// Open character sheet
        /// </summary>
        public void Open()
        {
            isOpen = true;
            characterSheetPanel.alpha = 1f;
            characterSheetPanel.interactable = true;
            characterSheetPanel.blocksRaycasts = true;
            UpdateStatsDisplay(0, 0);
        }
        
        /// <summary>
        /// Close character sheet
        /// </summary>
        public void Close()
        {
            isOpen = false;
            characterSheetPanel.alpha = 0f;
            characterSheetPanel.interactable = false;
            characterSheetPanel.blocksRaycasts = false;
        }
        
        /// <summary>
        /// Update all stat displays
        /// </summary>
        private void UpdateStatsDisplay(int unused1, int unused2)
        {
            if (playerStats == null)
                return;
            
            // Character info
            if (characterNameText != null)
                characterNameText.text = "Player";
            
            if (levelText != null)
                levelText.text = "Level: 1";
            
            if (experienceText != null)
                experienceText.text = "Experience: 0/1000";
            
            // Core stats
            if (healthValueText != null)
                healthValueText.text = $"{playerStats.CurrentHealth} / {playerStats.MaxHealth}";
            
            if (staminaValueText != null)
                staminaValueText.text = $"{playerStats.CurrentStamina} / {playerStats.MaxStamina}";
            
            if (hungerValueText != null)
                hungerValueText.text = $"{playerStats.CurrentHunger} / {playerStats.MaxHunger}";
            
            // Secondary stats (placeholder)
            if (defenseValueText != null)
                defenseValueText.text = "0";
            
            if (attackValueText != null)
                attackValueText.text = "0";
            
            // Buffs
            if (activeBuffsText != null)
            {
                if (playerStats.SpeedMultiplier > 1f || playerStats.StrengthMultiplier > 1f)
                {
                    activeBuffsText.text = "Active buffs detected";
                }
                else
                {
                    activeBuffsText.text = "No active buffs";
                }
            }
        }
        
        /// <summary>
        /// Check if character sheet is currently open
        /// </summary>
        public bool IsOpen => isOpen;
    }
}
