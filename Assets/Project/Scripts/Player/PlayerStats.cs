using UnityEngine;

namespace FarmingRPG.Player
{
    /// <summary>
    /// Player stats system (health, stamina, hunger, etc.)
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int currentHealth = 100;
        
        [Header("Stamina")]
        [SerializeField] private int maxStamina = 100;
        [SerializeField] private int currentStamina = 100;
        [SerializeField] private float staminaRegenRate = 5f; // Per second
        
        [Header("Hunger")]
        [SerializeField] private int maxHunger = 100;
        [SerializeField] private int currentHunger = 100;
        [SerializeField] private float hungerDecayRate = 1f; // Per minute
        
        [Header("Buffs")]
        private float activeSpeedMultiplier = 1f;
        private float activeStrengthMultiplier = 1f;
        private float buffTimeRemaining = 0f;
        
        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public int CurrentStamina => currentStamina;
        public int MaxStamina => maxStamina;
        public int CurrentHunger => currentHunger;
        public int MaxHunger => maxHunger;
        public float SpeedMultiplier => activeSpeedMultiplier;
        public float StrengthMultiplier => activeStrengthMultiplier;
        
        // Events
        public event System.Action<int, int> OnHealthChanged;
        public event System.Action<int, int> OnStaminaChanged;
        public event System.Action<int, int> OnHungerChanged;
        public event System.Action OnDeath;
        
        private void Update()
        {
            // Regenerate stamina
            if (currentStamina < maxStamina)
            {
                RestoreStamina(Mathf.RoundToInt(staminaRegenRate * Time.deltaTime));
            }
            
            // Decay hunger
            currentHunger -= Mathf.RoundToInt(hungerDecayRate * Time.deltaTime / 60f);
            if (currentHunger < 0)
            {
                currentHunger = 0;
                // Take damage when hungry
                TakeDamage(1);
            }
            OnHungerChanged?.Invoke(currentHunger, maxHunger);
            
            // Update buffs
            if (buffTimeRemaining > 0)
            {
                buffTimeRemaining -= Time.deltaTime;
                if (buffTimeRemaining <= 0)
                {
                    RemoveBuff();
                }
            }
        }
        
        /// <summary>
        /// Restore health
        /// </summary>
        public void RestoreHealth(int amount)
        {
            if (amount <= 0)
                return;
            
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            Debug.Log($"Restored {amount} health. Current: {currentHealth}/{maxHealth}");
        }
        
        /// <summary>
        /// Take damage
        /// </summary>
        public void TakeDamage(int amount)
        {
            if (amount <= 0)
                return;
            
            currentHealth -= amount;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            
            Debug.Log($"Took {amount} damage. Current: {currentHealth}/{maxHealth}");
        }
        
        /// <summary>
        /// Restore stamina
        /// </summary>
        public void RestoreStamina(int amount)
        {
            if (amount <= 0)
                return;
            
            currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }
        
        /// <summary>
        /// Use stamina
        /// </summary>
        public bool UseStamina(int amount)
        {
            if (currentStamina < amount)
                return false;
            
            currentStamina -= amount;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            return true;
        }
        
        /// <summary>
        /// Restore hunger
        /// </summary>
        public void RestoreHunger(int amount)
        {
            if (amount <= 0)
                return;
            
            currentHunger = Mathf.Min(currentHunger + amount, maxHunger);
            OnHungerChanged?.Invoke(currentHunger, maxHunger);
            Debug.Log($"Restored {amount} hunger. Current: {currentHunger}/{maxHunger}");
        }
        
        /// <summary>
        /// Apply temporary buff
        /// </summary>
        public void ApplyBuff(float speedMultiplier, float strengthMultiplier, float duration)
        {
            activeSpeedMultiplier = speedMultiplier;
            activeStrengthMultiplier = strengthMultiplier;
            buffTimeRemaining = duration;
            
            Debug.Log($"Applied buff: Speed x{speedMultiplier}, Strength x{strengthMultiplier} for {duration}s");
        }
        
        /// <summary>
        /// Remove active buff
        /// </summary>
        private void RemoveBuff()
        {
            activeSpeedMultiplier = 1f;
            activeStrengthMultiplier = 1f;
            buffTimeRemaining = 0f;
            Debug.Log("Buff expired");
        }
        
        /// <summary>
        /// Handle player death
        /// </summary>
        private void Die()
        {
            Debug.Log("Player died!");
            OnDeath?.Invoke();
            // Implement death logic (respawn, game over, etc.)
        }
        
        /// <summary>
        /// Reset all stats to max
        /// </summary>
        public void ResetStats()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentHunger = maxHunger;
            RemoveBuff();
            
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            OnHungerChanged?.Invoke(currentHunger, maxHunger);
        }
    }
}
