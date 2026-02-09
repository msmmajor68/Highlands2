# Highlands 2: Phase 2 - Hotbar/Quickbar Implementation Guide

## Overview

This phase introduces a **professional-grade hotbar system** independent from the main inventory. Players can assign favorite items to 10 hotbar slots for quick access, similar to games like Skyrim, Diablo, and World of Warcraft.

**Phase 2 Completion Date:** February 8, 2026

---

## System Architecture

### Key Components

#### 1. **HotbarUI.cs** [Assets/Project/Scripts/Inventory/UI/HotbarUI.cs]
- Main hotbar controller
- Manages 10 independent hotbar slots
- Handles keyboard input (1-0, mouse wheel)
- Displays visual slot selection

#### 2. **HotbarSlot Class** (in InventoryManager.cs)
- Data structure for hotbar slot references
- Stores inventory slot index and cached item reference
- Automatically updates when inventory changes
- Properties: `inventorySlotIndex`, `item`, `HasItem`

#### 3. **ItemSlotUI.cs** (Updated) [Assets/Project/Scripts/Inventory/UI/ItemSlotUI.cs]
- Unified slot UI used for both inventory and hotbar
- Handles drag & drop for both systems
- Displays item icon, quantity (if stackable > 1)

#### 4. **InventoryUI.cs** (Updated) [Assets/Project/Scripts/Inventory/UI/InventoryUI.cs]
- Right-click handling to assign items to hotbar
- Methods to find items in hotbar and empty slots
- Integration with hotbar assignment

#### 5. **HotbarSlot.prefab** [Assets/Project/Prefabs/UI/HotbarSlot.prefab]
- Simplified slot prefab for hotbar display
- Fixed 70x70 size with 50x50 centered icon
- Dark gray background with green selection highlight

---

## Feature Breakdown

### Independent Hotbar Slots

**Unlike many games where the hotbar displays the first N inventory slots, this system is independent:**

- Hotbar slots are **separate from inventory slots**
- Items are **assigned/referenced**, not moved
- Sorting inventory doesn't affect hotbar assignments
- Same item can appear in both inventory and hotbar
- Hotbar slots update automatically when items are consumed

**Data Flow:**
```
Inventory Slot (contains item + quantity)
         ↓
    Hotbar Slot (references inventory slot index)
         ↓
    Hotbar UI Display (shows icon + quantity)
```

### Hotbar Controls

| Input | Action |
|-------|--------|
| **1-9** | Select hotbar slots 1-9 |
| **0** | Select hotbar slot 10 |
| **Mouse Wheel Up** | Cycle selection backwards |
| **Mouse Wheel Down** | Cycle selection forwards |
| **Left Click** (hotbar item) | Use selected item (when inventory closed) |
| **Right Click** (inventory item) | Assign item to first empty hotbar slot |
| **Right Click** (already assigned) | Remove from hotbar |

### Item Assignment

**Assign to Hotbar:**
1. Open inventory (I key)
2. Right-click an item
3. Item moves to first empty hotbar slot
4. Item remains in inventory

**Remove from Hotbar:**
1. Open inventory
2. Right-click the item again
3. Item is removed from hotbar
4. Item stays in inventory

**Auto-Update:**
- If item is consumed (used), hotbar slot clears automatically
- If inventory is sorted/reorganized, hotbar assignments persist
- Hotbar displays current item icon and quantity

---

## Implementation Details

### Hotbar Data Structure

```csharp
public class HotbarSlot
{
    public int inventorySlotIndex = -1;  // Reference to inventory
    public Item item = null;              // Cached reference
    
    public bool HasItem => inventorySlotIndex >= 0 && item != null;
    
    public void Clear()
    {
        inventorySlotIndex = -1;
        item = null;
    }
}
```

### Key Methods

#### InventoryManager.cs

```csharp
// Assign inventory slot to hotbar
public void AssignToHotbar(int inventoryIndex, int hotbarIndex)

// Remove item from hotbar
public void RemoveFromHotbar(int hotbarIndex)

// Get hotbar slot
public HotbarSlot GetHotbarSlot(int index)

// Use item from hotbar (consumes from inventory)
public bool UseHotbarItem(int hotbarIndex, GameObject user)

// Update hotbar when inventory changes
private void UpdateHotbarReferences(int changedInventoryIndex)
```

#### HotbarUI.cs

```csharp
// Handle number key and mouse wheel input
private void HandleInput()

// Select a hotbar slot
private void SelectSlot(int index)

// Cycle selection with mouse wheel
private void CycleHotbarSelection(int direction)

// Display hotbar slot
private void UpdateHotbarSlotDisplay(int hotbarIndex, HotbarSlot hotbarSlot)
```

#### InventoryUI.cs

```csharp
// Find if item is in hotbar
private int FindItemInHotbar(Item item)

// Find first empty hotbar slot
private int FindEmptyHotbarSlot()
```

---

## Item Dropping System

### Drop Mechanics

**How to Drop:**
1. Open inventory (I key)
2. **Shift + Click** item → drops 1 item
3. **Shift + Drag** item → drops entire stack

**Drop Behavior:**
- Items drop 1.5 units in front of player
- Small random offset prevents stacking
- 0.5 second pickup delay (prevents auto-re-pickup)
- Can pick up by walking over or right-clicking

### Drop Implementation

**InventoryManager.cs:**
```csharp
public void DropItem(int slotIndex, Vector3 dropPosition, int quantity = 1)
{
    // Remove from inventory
    // Spawn ItemPickup prefab in world
    // Apply random offset
    // Reset pickup delay
}
```

**ItemPickup.cs (Updated):**
- Added `pickupDelay` field (default: 0.5 seconds)
- `ResetPickupDelay()` called when item spawned
- Auto-pickup waits for delay to expire

---

## UI Layout

### Canvas Structure
```
InventoryCanvas
├── InventoryPanel (white pane with 30-slot grid)
│   └── SlotsContainer (GridLayoutGroup, 5 columns)
│       └── InventorySlot (Clone) x30
├── HotbarPanel (bottom of screen)
│   └── HotbarSlotsContainer (HorizontalLayoutGroup)
│       └── HotbarSlot (Clone) x10
└── HotbarUI (script controller)
```

### Hotbar Panel Settings
- **Anchors:** Bottom center (0.5, 0)
- **Position:** Centered X, 50 units from bottom
- **Size:** 800 × 100
- **Layout:** Horizontal with 10px spacing, 10px padding

### Slot Prefab (HotbarSlot)
- **Size:** 70 × 70 pixels
- **Icon:** 50 × 50 pixels (centered with padding)
- **Background:** Dark gray (0.3, 0.3, 0.3)
- **Selected:** Bright green (0, 1, 0)

---

## Events & Callbacks

### InventoryManager Events

```csharp
public event Action<int> OnInventoryChanged;           // Any inventory change
public event Action<int> OnHotbarChanged;              // Hotbar selected slot changed
public event Action<int, InventorySlot> OnSlotChanged; // Specific slot changed
public event Action<int, HotbarSlot> OnHotbarSlotChanged; // Hotbar slot changed
```

### Subscription Flow

**HotbarUI subscribes to:**
- `OnHotbarSlotChanged` → Update visual display
- `OnHotbarChanged` → Update selected slot highlight

**InventoryUI subscribes to:**
- `OnSlotChanged` → Update inventory grid visuals
- `OnInventoryChanged` → Refresh entire inventory

---

## Testing Checklist

### Basic Hotbar Functions
- [ ] Number keys 1-9, 0 select correct hotbar slot
- [ ] Mouse wheel cycles through 10 slots correctly
- [ ] Selected slot shows green highlight
- [ ] Other slots show dark gray background

### Item Assignment
- [ ] Right-click inventory item → moves to first empty hotbar slot
- [ ] Right-click again → removes from hotbar
- [ ] Item stays in inventory after assignment
- [ ] Multiple items assignable to different slots

### Item Usage
- [ ] Left-click uses selected hotbar item
- [ ] Inventory closes when item used
- [ ] Hotbar updates when item consumed
- [ ] Can't use items while inventory is open

### Item Dropping
- [ ] Shift+Click drops 1 item in front of player
- [ ] Shift+Drag drops entire stack
- [ ] Dropped item has 0.5s pickup delay
- [ ] Item disappears from inventory on drop
- [ ] Multiple dropped items get small offset (don't stack exactly)

### Inventory Changes
- [ ] Dragging items in inventory doesn't lose hotbar assignments
- [ ] Moving item between inventory slots keeps hotbar reference
- [ ] If hotbar-assigned item moved, hotbar follows it
- [ ] If hotbar-assigned item removed entirely, hotbar slot clears

---

## Performance Considerations

- **Hotbar Prefab Instantiation:** 10 slots created at startup (minimal overhead)
- **Event Subscriptions:** Cleaned up properly on MonoBehaviour destruction
- **Memory:** Each HotbarSlot stores only int + Item reference (negligible)
- **Update Frequency:** Input checked every frame, but only number keys/wheel (no polling)

---

## Known Limitations & Future Improvements

### Current Limitations
1. No keyboard remapping (hardcoded 1-0 keys)
2. No persistent hotbar save/load (resets on scene load)
3. No hotbar tooltips on hover
4. No animation for drop/pickup

### Potential Enhancements
1. **Persistence:** Save/load hotbar configuration
2. **Hotbar Preset:** Multiple hotbar sets (e.g., combat vs. gathering)
3. **Drag to Hotbar:** Drag items from inventory directly to hotbar slots
4. **Hotbar Tooltips:** Show item name/quantity on mouse over
5. **Keyboard Remapping:** Allow players to customize hotbar key bindings
6. **Middle-click:** Quick move to/from hotbar without right-clicking
7. **Cooldown Display:** Show ability cooldown on hotbar slots
8. **Quickcast:** Hold hotbar key to keep slot selected

---

## Code Statistics

| File | Lines | Purpose |
|------|-------|---------|
| HotbarUI.cs | 202 | Main hotbar controller |
| InventoryManager.cs | +80 | Hotbar data & methods |
| ItemSlotUI.cs | +20 | Drop on drag support |
| InventoryUI.cs | +60 | Item assignment logic |
| ItemPickup.cs | +15 | Pickup delay timer |
| HotbarSlot.prefab | 174 | Hotbar slot UI template |

---

## Integration with Existing Systems

### With Inventory
- Hotbar references inventory slots
- Right-click toggles assignment
- Both systems share same items

### With Item Usage
- `UseHotbarItem()` consumes from inventory
- Hotbar automatically updates
- Works with any consumable item

### With Item Dropping
- Drop creates ItemPickup in world
- Pickup delay prevents re-acquisition
- Integrates with auto-pickup system

---

## Developer Notes

### Important Variables
- **hotbarSize:** Set to 10 (can be changed in InventoryManager inspector)
- **pickupDelay:** 0.5 seconds (adjustable per ItemPickup)
- **dropPosition:** 1.5 units in front of player

### Debug Tips
- Check Console for hotbar assignment feedback
- Verify HotbarUI component attached to InventoryCanvas
- Ensure ItemSlotUI prefab is assigned to HotbarUI
- Verify InventoryManager has HotbarSize = 10

### Common Issues & Fixes

**Hotbar not showing:**
- Verify InventoryCanvas has HotbarUI component
- Check HotbarSlotsContainer is assigned correctly
- Ensure HotbarSlot prefab is assigned

**Items won't drop:**
- Assign a WorldPrefab to your TestItem
- Or ensure TestPickup exists as template in scene

**Items re-pickup instantly:**
- Adjust PickupDelay on ItemPickup script (increase from 0.5)

---

## Related Documentation

- **Phase 1:** [Character Generation & Animation](../Character/README.md)
- **Inventory System:** [Inventory Guide](./INVENTORY_SETUP_GUIDE.md)
- **Items:** [Item Creation Guide](./ITEM_CREATION.md)

---

**Status:** ✅ Complete - All core hotbar features implemented and tested.
