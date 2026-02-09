# 🎮 Quick Start Checklist

## ✅ Essential Setup (Do these first!)

### 1. Scene Setup
- [ ] Create Canvas with EventSystem
- [ ] Create GameManager GameObject
- [ ] Setup Player GameObject with all required components

### 2. Create Assets
- [ ] Create ItemDatabase (Right-click → Create → Farming RPG → Item Database)
- [ ] Create at least 3-5 test items (food, tools, resources)
- [ ] Assign items to ItemDatabase

### 3. UI Setup
- [ ] Create ItemSlot prefab with ItemSlotUI script
- [ ] Create Inventory Panel with InventoryUI script
- [ ] Create Hotbar Panel with HotbarUI script
- [ ] Create Tooltip panel with ItemTooltip script

### 4. Assign References
- [ ] GameManager → ItemDatabase
- [ ] Player → All required components
- [ ] InventoryUI → InventoryManager, SlotPrefab, Container
- [ ] HotbarUI → InventoryManager, SlotPrefab, Container
- [ ] ItemTooltip → All text elements

### 5. Test
- [ ] Press Play
- [ ] Press F1 to add test items
- [ ] Press Tab to open inventory
- [ ] Drag items between slots
- [ ] Right-click to use items
- [ ] Use number keys to select hotbar

---

## 🎨 Recommended UI Layout

```
Canvas
├── InventoryPanel (center, hidden by default)
│   ├── Background (Image - semi-transparent black)
│   ├── TitleText ("Inventory")
│   └── SlotsContainer (Grid Layout 10x3)
│
├── HotbarPanel (bottom-center, always visible)
│   └── SlotsContainer (Horizontal Layout, 10 slots)
│
└── ItemTooltip (follows mouse, hidden by default)
    ├── ItemNameText
    ├── ItemTypeText
    ├── DescriptionText
    └── PriceText
```

---

## 📦 Component Requirements

### Player GameObject needs:
```
☑ Tag: "Player"
☑ Rigidbody2D (Gravity Scale = 0)
☑ Collider2D (for interactions)
☑ PlayerController
☑ PlayerStats  
☑ InventoryManager
☑ InventorySaveLoad (optional)
```

### ItemSlot Prefab needs:
```
☑ ItemSlotUI script
☑ Background Image
☑ ItemIcon Image (child)
☑ QuantityText (child, TextMeshPro)
☑ Highlight Image (child, optional)
```

---

## 🔧 Common Configuration Values

### InventoryManager
- Inventory Size: **30** (standard)
- Hotbar Size: **10** (1-9 keys + 0)

### ItemSlotUI
- Icon Size: **48x48** to **64x64**
- Padding: **8px**

### Grid Layout Group
- Cell Size: **64x64**
- Spacing: **8, 8**
- Columns: **10**

### Item Settings
- Stack Size (consumables): **99**
- Stack Size (tools): **1** (not stackable)
- Stack Size (resources): **99** or **999**

---

## 🎯 Test Items to Create

Create these items for testing:

### Consumables:
1. **Apple** - Restores 20 health
2. **Bread** - Restores 30 hunger
3. **Health Potion** - Restores 50 health

### Tools:
1. **Wooden Axe** - Chop trees, Power: 1
2. **Stone Pickaxe** - Mine rocks, Power: 2
3. **Iron Sword** - Combat, Damage: 15

### Resources:
1. **Wood** - Building material
2. **Stone** - Building material  
3. **Gold Coin** - Currency

### Seeds:
1. **Wheat Seeds** - Grows in Spring/Summer
2. **Carrot Seeds** - Grows in Spring/Fall

---

## ⌨️ Default Controls Reference

| Action | Key | Description |
|--------|-----|-------------|
| Open Inventory | Tab / I | Toggle inventory panel |
| Select Hotbar | 1-9 | Quick select slots 1-9 |
| Cycle Hotbar | Mouse Wheel | Scroll through hotbar |
| Use Item | E / Left Click | Use selected item |
| Drop Item | Q | Drop selected item |
| Interact | F | Talk to NPCs, harvest resources |
| Move | WASD / Arrows | Move player |

---

## 🐛 Quick Troubleshooting

### Items not showing?
→ Check ItemDatabase is assigned in GameManager
→ Verify items have icons assigned

### Can't open inventory?
→ Check InventoryUI script is attached
→ Verify inventoryPanel reference is set

### UI not clickable?
→ Ensure EventSystem exists in scene
→ Check Canvas has GraphicRaycaster

### Items not stacking?
→ Set item.isStackable = true
→ Set item.maxStackSize > 1

---

## 📈 Next Features to Implement

After basic inventory works:

1. **Crafting UI** - Visual crafting menu
2. **Storage Chests** - Separate inventories
3. **Equipment Slots** - Weapons, armor
4. **Stats UI** - Health/stamina/hunger bars
5. **Shop System** - Buy/sell items
6. **Quest System** - Track objectives
7. **Farming System** - Plant and grow crops
8. **Combat System** - Use weapons

---

## 💡 Pro Tips

1. **Test incrementally** - Get one feature working before adding more
2. **Use Debug.Log** - Add logs to track what's happening
3. **Save often** - Use Ctrl+S to save scenes/assets
4. **Organize folders** - Keep items, prefabs, scripts organized
5. **Use prefab variants** - Create variants for similar items
6. **Version control** - Use Git to track changes

---

Ready to start? Follow the checklist above! ✨
