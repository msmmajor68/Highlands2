# 🎮 2D Farming/RPG Game - Complete Inventory System

## 📦 What's Included

A complete, production-ready inventory system for a 2D Farming/RPG game similar to Mad Island. This system provides everything you need to start developing your game!

### ✨ Features

#### Core Systems
- ✅ **Item System** - Flexible base class supporting multiple item types
- ✅ **Inventory Management** - Slot-based system with stacking
- ✅ **Hotbar System** - Quick access to frequently used items
- ✅ **UI Components** - Drag-and-drop inventory interface
- ✅ **Player Stats** - Health, stamina, hunger, and buff system
- ✅ **Save/Load** - Automatic saving with JSON serialization

#### Item Types
- ✅ **Consumables** - Food, potions with stat restoration
- ✅ **Tools** - Axes, pickaxes, weapons with durability
- ✅ **Seeds** - Plantable items with growth settings
- ✅ **Resources** - Materials for crafting and building
- ✅ **Equipment** - Base support for armor and weapons

#### Advanced Features
- ✅ **Crafting System** - Recipe-based item creation
- ✅ **Resource Nodes** - Harvestable trees, rocks, plants
- ✅ **Storage Containers** - Chests with separate inventories
- ✅ **Item Pickup** - World items that can be collected
- ✅ **Item Database** - Centralized item management
- ✅ **Tooltips** - Rich item information on hover

---

## 📂 File Structure

```
Assets/Scripts/
│
├── Core/
│   └── GameManager.cs ...................... Game initialization
│
├── Inventory/
│   ├── Item.cs ............................. Base item class
│   ├── ConsumableItem.cs ................... Food/potions
│   ├── ToolItem.cs ......................... Tools/weapons
│   ├── SeedItem.cs ......................... Seeds
│   ├── ItemType.cs ......................... Item categories enum
│   ├── InventorySlot.cs .................... Single inventory slot
│   ├── InventoryManager.cs ................. Main inventory logic
│   ├── ItemDatabase.cs ..................... Item registry
│   └── InventorySaveLoad.cs ................ Save/load functionality
│
├── Inventory/UI/
│   ├── ItemSlotUI.cs ....................... Individual slot UI
│   ├── InventoryUI.cs ...................... Main inventory panel
│   ├── HotbarUI.cs ......................... Hotbar display
│   ├── ItemTooltip.cs ...................... Item hover tooltip
│   └── StorageUI.cs ........................ Chest/storage UI
│
├── Player/
│   ├── PlayerController.cs ................. Player movement & controls
│   └── PlayerStats.cs ...................... Health/stamina/hunger
│
├── Items/
│   └── ItemPickup.cs ....................... World item pickup
│
├── Crafting/
│   └── CraftingRecipe.cs ................... Crafting recipes
│
├── Interactables/
│   ├── ResourceNode.cs ..................... Harvestable resources
│   └── StorageContainer.cs ................. Chests/containers
│
└── Examples/
    └── InventoryExamples.cs ................ Usage examples

Documentation/
├── INVENTORY_SETUP_GUIDE.md ................ Detailed setup instructions
├── QUICK_START.md .......................... Quick reference checklist
└── README.md ............................... This file
```

---

## 🚀 Quick Start

### Minimal Setup (5 minutes)

1. **Create GameManager**
   - Empty GameObject named "GameManager"
   - Add `GameManager` script
   - Create ItemDatabase asset and assign it

2. **Setup Player**
   
   **Create Player GameObject:**
   - In Hierarchy, right-click → 2D Object → Sprite → Square (or use your character sprite)
   - Rename it to "Player"
   - Position at (0, 0, 0)
   
   **Add Rigidbody2D:**
   - Select Player GameObject
   - Inspector → Add Component → Rigidbody2D
   - Set **Body Type** = Dynamic
   - Set **Gravity Scale** = 0 (important for top-down movement!)
   - Set **Constraints** → Freeze Rotation Z (prevents spinning)
   
   **Add Collider:**
   - Add Component → Box Collider 2D or Circle Collider 2D
   
   **Add Scripts:**
   - Add Component → Search "PlayerController" → Add it
   - Add Component → Search "PlayerStats" → Add it
   - Add Component → Search "InventoryManager" → Add it
   - Optionally: Add "InventorySaveLoad" for saving
   
   **Configure Player Tag:**
   - At top of Inspector, click Tag dropdown → Select "Player"
   - (If "Player" tag doesn't exist, click "Add Tag", create it, then assign it)
   
   **Configure InventoryManager:**
   - In InventoryManager component, set:
   - **Inventory Size**: 30
   - **Hotbar Size**: 10

3. **Create Basic UI**
   - Canvas with EventSystem
   - Create ItemSlot prefab
   - Create Inventory Panel
   - Create Hotbar Panel

4. **Test**
   - Press Play
   - Press F1 to add test items
   - Press Tab to open inventory

📖 **For detailed instructions, see [INVENTORY_SETUP_GUIDE.md](INVENTORY_SETUP_GUIDE.md)**

---

## 🎮 Controls

| Action | Key | Description |
|--------|-----|-------------|
| **Open Inventory** | `Tab` or `I` | Toggle inventory panel |
| **Hotbar Select** | `1-9` | Quick select hotbar slots |
| **Cycle Hotbar** | `Mouse Wheel` | Scroll through hotbar |
| **Use Item** | `E` or `Left Click` | Use selected item |
| **Drop Item** | `Q` | Drop selected item |
| **Interact** | `F` | Interact with objects/NPCs |
| **Move** | `WASD` or `Arrows` | Move player |

### Debug Commands (when Debug Mode enabled in GameManager)
| Key | Action |
|-----|--------|
| `F1` | Add random test items |
| `F2` | Clear inventory |

---

## 💻 Code Examples

### Adding Items
```csharp
InventoryManager inventory = player.GetComponent<InventoryManager>();
inventory.AddItem(appleItem, 5); // Add 5 apples
```

### Checking Items
```csharp
bool hasApples = inventory.HasItem(appleItem, 10);
int appleCount = inventory.GetItemCount(appleItem);
```

### Using Items
```csharp
inventory.UseItem(slotIndex, player);
```

### Getting Items from Database
```csharp
ItemDatabase db = GameManager.Instance.GetItemDatabase();
Item potion = db.GetItemByName("Health Potion");
```

### Modifying Player Stats
```csharp
PlayerStats stats = player.GetComponent<PlayerStats>();
stats.RestoreHealth(20);
stats.UseStamina(10);
stats.ApplyBuff(1.5f, 1.0f, 10f); // Speed x1.5 for 10 seconds
```

📖 **For more examples, see [InventoryExamples.cs](Assets/Scripts/Examples/InventoryExamples.cs)**

---

## 🎨 Creating Content

### Creating a New Item

1. **Right-click in Project** → Create → Farming RPG → Items
2. **Choose item type**: Base Item, Consumable, Tool, or Seed
3. **Configure properties**:
   - Name, description
   - Icon sprite
   - Stack size
   - Prices
   - Type-specific properties

4. **Add to ItemDatabase**:
   - Open ItemDatabase asset
   - Add item to "All Items" list

### Creating a Crafting Recipe

1. **Right-click** → Create → Farming RPG → Crafting → Recipe
2. **Set result item** and quantity
3. **Add ingredients** (item + quantity)
4. **Set craft time** and required station

### Creating a Resource Node

1. **Create GameObject** with sprite
2. **Add Collider2D** (for interaction)
3. **Add ResourceNode script**
4. **Configure**:
   - Resource item to drop
   - Required tool type
   - Health and respawn time
5. **Set layer** to "Interactable"

### Creating a Storage Container

1. **Create GameObject** with sprite
2. **Add Collider2D**
3. **Add StorageContainer script**
4. **Configure** storage size and name
5. **Optionally add Animator** for open/close animations

---

## 🔧 Customization

### Adding New Item Types

1. Create new class extending `Item`:
```csharp
[CreateAssetMenu(fileName = "New Equipment", menuName = "Farming RPG/Items/Equipment")]
public class EquipmentItem : Item
{
    public int defense;
    public int armor;
    
    public override bool Use(GameObject user)
    {
        // Equip logic here
        return false; // Don't consume
    }
}
```

2. Use in ItemDatabase and inventory as normal

### Adding Custom Stats

Extend `PlayerStats.cs`:
```csharp
[Header("Custom Stats")]
[SerializeField] private int maxMana = 100;
private int currentMana = 100;

public void RestoreMana(int amount) { ... }
```

### Creating UI Themes

Modify colors in `ItemSlotUI`:
- Normal Color
- Highlight Color
- Selected Color

Apply custom sprites to UI panels for different themes.

---

## 🎯 Extending the System

### Recommended Next Steps

1. **Equipment System**
   - Equipment slots (head, body, weapon)
   - Stat bonuses from equipment
   - Visual character customization

2. **Farming System**
   - Tilling soil
   - Planting seeds
   - Watering crops
   - Harvest mechanic

3. **Combat System**
   - Enemy AI
   - Weapon attacks
   - Projectiles
   - Damage numbers

4. **NPC System**
   - Dialogue trees
   - Quest system
   - Relationship levels
   - Gift giving

5. **Building System**
   - Furniture placement
   - Building construction
   - Blueprint system

6. **Time System**
   - Day/night cycle
   - Seasons
   - Weather effects
   - Calendar

7. **Economy**
   - Shop UI
   - Dynamic prices
   - Trading system
   - Currency management

8. **Skills/Progression**
   - Experience system
   - Skill trees
   - Level-up rewards
   - Stat increases

---

## 📊 System Architecture

```
┌──────────────┐
│ GameManager  │ ← Singleton, manages ItemDatabase
└──────┬───────┘
       │
┌──────┴────────────────────────────────┐
│                                       │
│ ┌─────────────┐    ┌──────────────┐ │
│ │ItemDatabase │◄───┤    Items     │ │
│ └─────────────┘    └──────────────┘ │
│                                       │
└───────────────────────────────────────┘
                │
                ▼
    ┌───────────────────┐
    │      Player       │
    ├───────────────────┤
    │ • Controller      │
    │ • Stats           │
    │ • Inventory Mgr   │◄──── UI Components
    │ • Save/Load       │      └─ InventoryUI
    └───────────────────┘      └─ HotbarUI
                                └─ Tooltip
```

---

## 🤝 Integration with Other Systems

### Unity Input System
Replace `Input.GetKey` with new Input System:
```csharp
[SerializeField] private InputAction inventoryAction;
if (inventoryAction.triggered)
    ToggleInventory();
```

### Addressables
Load items asynchronously:
```csharp
Addressables.LoadAssetAsync<Item>("ItemAddress").Completed += handle => {
    Item item = handle.Result;
};
```

### Multiplayer (Netcode)
Add NetworkBehaviour and NetworkVariable:
```csharp
public class NetworkInventory : NetworkBehaviour
{
    NetworkList<int> itemIDs;
    NetworkList<int> quantities;
}
```

---

## ⚡ Performance Tips

1. **Object Pooling** - Pool ItemPickup GameObjects
2. **UI Optimization** - Only update visible slots
3. **Caching** - Cache GetComponent calls
4. **Event-Driven** - Use events instead of Update checks
5. **Lazy Loading** - Load items on-demand from database

---

## 🐛 Troubleshooting

### Common Issues

**Items not appearing in inventory**
- ✓ Check ItemDatabase is assigned in GameManager
- ✓ Verify items have icons
- ✓ Ensure items are added to database

**UI not responding**
- ✓ EventSystem exists in scene
- ✓ Canvas has GraphicRaycaster
- ✓ UI elements have raycast targets enabled

**Save/Load not working**
- ✓ ItemDatabase initialized before loading
- ✓ Item names match exactly (case-sensitive)
- ✓ Check permissions for save file location

**Player can't interact**
- ✓ Player has "Player" tag
- ✓ Interactable layer is set correctly
- ✓ Colliders are properly configured

---

## 📄 License

This code is provided as-is for game development purposes. You are free to use, modify, and distribute it in your projects (commercial or non-commercial).

---

## 🙏 Credits

Created for Unity 2D Farming/RPG games inspired by:
- Mad Island
- Stardew Valley
- Harvest Moon
- Graveyard Keeper

---

## 📞 Support

For questions or issues:
1. Check [INVENTORY_SETUP_GUIDE.md](INVENTORY_SETUP_GUIDE.md)
2. Review [QUICK_START.md](QUICK_START.md)
3. Examine [InventoryExamples.cs](Assets/Scripts/Examples/InventoryExamples.cs)

---

## 🔄 Version History

**v1.0.0** - Initial Release
- Complete inventory system
- UI components
- Player stats
- Item types (consumable, tool, seed)
- Save/load functionality
- Crafting recipes
- Resource gathering
- Storage containers
- Comprehensive documentation

---

Happy game development! 🎮✨

Want to show off what you build with this? I'd love to see it!
