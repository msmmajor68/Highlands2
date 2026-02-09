# 🎮 Farming/RPG Game - Inventory System Setup Guide

## 📋 Overview
This is a complete inventory and item system for a 2D Farming/RPG game similar to Mad Island. The system includes:

- ✅ Flexible item system with multiple item types (consumables, tools, seeds, resources)
- ✅ Inventory management with slots and stacking
- ✅ UI system with drag-and-drop support
- ✅ Hotbar system with quick item access
- ✅ Player stats (health, stamina, hunger)
- ✅ Item pickup and drop systems
- ✅ Save/Load functionality
- ✅ Crafting system
- ✅ Resource gathering system
- ✅ Item tooltips

---

## 🏗️ Setup Instructions

### Step 1: Create the GameManager
1. Create an empty GameObject in your scene called "GameManager"
2. Add the `GameManager` script component to it
3. Create an ItemDatabase asset: Right-click in Project → Create → Farming RPG → Item Database
4. Assign the ItemDatabase to the GameManager

### Step 2: Setup the Player
1. Create or select your Player GameObject
2. Ensure it has the tag "Player"
3. Add these components:
   - `Rigidbody2D` (Gravity Scale = 0, for top-down movement)
   - `PlayerController`
   - `PlayerStats`
   - `InventoryManager`
   - `InventorySaveLoad` (optional, for save/load)

4. Configure InventoryManager:
   - Inventory Size: 30 (or your preferred size)
   - Hotbar Size: 10

### Step 3: Create the Inventory UI

#### A. Create the Main Inventory Panel
1. Create a Canvas (Right-click → UI → Canvas)
2. Set Canvas Scaler to "Scale With Screen Size" (Reference: 1920x1080)
3. Create a Panel under Canvas called "InventoryPanel"
4. Inside InventoryPanel, create:
   - Background Image
   - Title Text (e.g., "Inventory")
   - Slots Container (Grid Layout Group)

5. Configure Grid Layout Group:
   - Cell Size: 64x64
   - Spacing: 8, 8
   - Constraint: Fixed Column Count = 10 (or your preference)

#### B. Create Item Slot Prefab
1. Create a Button or Panel called "ItemSlot"
2. Add these child objects:
   - Background (Image)
   - ItemIcon (Image) - disable by default
   - QuantityText (TextMeshPro) - disable by default
   - Highlight (Image, optional) - for hover effect

3. Add `ItemSlotUI` script to ItemSlot
4. Assign references in ItemSlotUI:
   - Item Icon → ItemIcon Image
   - Quantity Text → QuantityText
   - Background Image → Background
   - Highlight Image → Highlight (optional)

5. Save as Prefab in Project

#### C. Setup Inventory UI Controller
1. Add `InventoryUI` script to InventoryPanel
2. Assign references:
   - Inventory Manager → Player's InventoryManager
   - Inventory Panel → The panel GameObject
   - Slots Container → Grid Layout container
   - Slot Prefab → Your ItemSlot prefab

### Step 4: Create the Hotbar UI

1. Create a Panel called "HotbarPanel" in Canvas
2. Position at bottom-center of screen
3. Add Horizontal Layout Group
4. Add `HotbarUI` script
5. Assign:
   - Inventory Manager → Player's InventoryManager
   - Hotbar Container → Panel with layout group
   - Hotbar Slot Prefab → Same ItemSlot prefab or create a variant

### Step 5: Create Item Tooltip

1. Create a Panel called "ItemTooltip" in Canvas
2. Add these child TextMeshPro elements:
   - ItemName (bold, larger)
   - ItemType
   - Description
   - Price

3. Add `ItemTooltip` script to the panel
4. Assign all text references
5. Add CanvasGroup component
6. Set initial alpha to 0

### Step 6: Create Items

#### Creating Different Item Types:

**Basic Item:**
- Right-click → Create → Farming RPG → Items → Base Item
- Set name, description, icon, type, prices

**Consumable (Food/Potions):**
- Right-click → Create → Farming RPG → Items → Consumable
- Set health/stamina/hunger restoration
- Set buff effects if needed

**Tool (Axe, Pickaxe, Weapon):**
- Right-click → Create → Farming RPG → Items → Tool
- Set tool type, power, durability
- For weapons, set damage and range

**Seed:**
- Right-click → Create → Farming RPG → Items → Seed
- Set growth time, yield
- Assign what crop it produces
- Set which seasons it grows in

#### Add Items to Database:
1. Open your ItemDatabase asset
2. Expand "All Items" list
3. Add all your created items

---

## 🎮 Controls

### Inventory:
- **Tab** or **I** - Open/Close Inventory
- **Left Click** - Select item
- **Right Click** - Use item
- **Drag & Drop** - Move items between slots

### Hotbar:
- **1-9 Keys** - Quick select hotbar slots
- **Mouse Scroll** - Cycle through hotbar
- **Q** - Drop selected item
- **E** or **Left Mouse** - Use selected item

### Interaction:
- **F** - Interact with objects/NPCs

### Debug Commands (when Debug Mode enabled):
- **F1** - Add test items
- **F2** - Clear inventory

---

## 🛠️ Creating Resource Nodes

### Trees, Rocks, Plants:
1. Create a GameObject in scene
2. Add SpriteRenderer with the resource sprite
3. Add Collider2D (for interaction detection)
4. Add `ResourceNode` script
5. Configure:
   - Resource Item → What item it gives
   - Min/Max Yield → How many items
   - Required Tool → What tool is needed
   - Min Tool Power → Minimum tool level
   - Max Health → How many hits to deplete
   - Respawn Time → Seconds to respawn (0 = never)

6. Set Layer to "Interactable" (create if needed)
7. Make sure PlayerController's "Interactable Layer" includes this layer

---

## 🔨 Creating Crafting Recipes

1. Right-click → Create → Farming RPG → Crafting → Recipe
2. Set recipe name and description
3. Set Result Item and quantity
4. Add ingredients (item + quantity)
5. Set craft time
6. Set required crafting station (if any)

To use crafting, you'll need to create a Crafting UI and Crafting Station scripts that utilize the recipes.

---

## 📦 Item Pickup System

### For Dropped/World Items:
1. Create a GameObject with SpriteRenderer
2. Add Collider2D (set as Trigger)
3. Add Rigidbody2D (Kinematic)
4. Add `ItemPickup` script
5. Assign the item and quantity
6. Set pickup range and auto-pickup preference

Items dropped by player will auto-create this prefab if you assign "worldPrefab" to items.

---

## 💾 Save/Load System

The inventory automatically saves:
- Every 60 seconds (configurable)
- When application quits

Save location: `Application.persistentDataPath/inventory_save.json`

### Manual Save/Load:
```csharp
InventorySaveLoad saveLoad = player.GetComponent<InventorySaveLoad>();
saveLoad.SaveInventory(); // Manual save
saveLoad.LoadInventory(); // Manual load
saveLoad.DeleteSave(); // Delete save file
```

---

## 🎨 Customization Tips

### Item Rarity System:
Add to Item.cs:
```csharp
public enum ItemRarity { Common, Uncommon, Rare, Epic, Legendary }
public ItemRarity rarity;
```

### Equipment Slots:
Create an EquipmentManager with specific slots (Head, Body, Weapon, etc.)

### Quest Items:
Quest items can be marked as "Quest" type and not sellable/droppable

### Farming System:
Use SeedItem with a FarmingSystem that spawns cropPrefab at tilled soil positions

### NPC Trading:
Create a ShopUI that uses item buy/sell prices

---

## 📝 Code Structure

```
Assets/Scripts/
├── Core/
│   └── GameManager.cs
├── Inventory/
│   ├── Item.cs (base class)
│   ├── ConsumableItem.cs
│   ├── ToolItem.cs
│   ├── SeedItem.cs
│   ├── ItemType.cs
│   ├── InventorySlot.cs
│   ├── InventoryManager.cs
│   ├── ItemDatabase.cs
│   └── InventorySaveLoad.cs
├── Inventory/UI/
│   ├── ItemSlotUI.cs
│   ├── InventoryUI.cs
│   ├── HotbarUI.cs
│   └── ItemTooltip.cs
├── Player/
│   ├── PlayerController.cs
│   └── PlayerStats.cs
├── Items/
│   └── ItemPickup.cs
├── Crafting/
│   └── CraftingRecipe.cs
└── Interactables/
    └── ResourceNode.cs
```

---

## 🚀 Next Steps

### Expand Your Game:
1. **Farming System** - Tilled soil, planting, watering, growth cycles
2. **Combat System** - Enemy AI, health bars, damage system
3. **NPC System** - Dialogue, quests, relationships
4. **Building System** - Place furniture, construct buildings
5. **Time System** - Day/night cycle, seasons, weather
6. **Cooking System** - Combine foods for better buffs
7. **Fishing System** - Mini-game, different fish types
8. **Animal Husbandry** - Chickens, cows, pigs
9. **Mining System** - Underground caves, ores
10. **Skill System** - Level up farming, combat, crafting skills

---

## 📚 Reference

### Adding Items Programmatically:
```csharp
InventoryManager inventory = player.GetComponent<InventoryManager>();
Item item = itemDatabase.GetItemByName("Apple");
inventory.AddItem(item, 5);
```

### Checking Item Count:
```csharp
int appleCount = inventory.GetItemCount(appleItem);
bool hasApples = inventory.HasItem(appleItem, 3);
```

### Using Items:
```csharp
inventory.UseItem(slotIndex, player);
```

---

## ⚠️ Common Issues

**Items not showing in inventory:**
- Check if ItemDatabase is assigned in GameManager
- Verify items are added to the database
- Check if icons are assigned to items

**Can't pick up items:**
- Verify Player has "Player" tag
- Check if PlayerController has InventoryManager
- Verify ItemPickup has item assigned

**UI not responding:**
- Check EventSystem exists in scene
- Verify Canvas has GraphicRaycaster
- Check UI elements have CanvasGroup or Image component for raycasting

**Save/Load not working:**
- ItemDatabase must be initialized before loading
- Item names in save file must match item names in database exactly

---

## 🎯 Tips for Mad Island Style Game

To match Mad Island's gameplay:
1. **Survival Mechanics** - Implement hunger/thirst decay
2. **Base Building** - Expandable inventory system for building materials
3. **Multiple Hotbars** - Create weapon hotbar + tool hotbar
4. **Quick Crafting** - Craft from inventory without opening menu
5. **Resource Weight** - Add weight limit to inventory
6. **Storage Chests** - Create separate inventory for storage containers
7. **NPCs Join Party** - NPC followers with their own inventories

Good luck with your game development! 🎮✨
