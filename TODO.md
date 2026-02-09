# Highlands 2 - Development Progress Tracker

**Last Updated:** February 8, 2026

---

## Phase 1: Character & Animation ✅ COMPLETE

- [x] Player sprite sheet setup
- [x] 8-directional animation states (idle & walk)
- [x] Animator controller with parameter-based transitions
- [x] PlayerController with directional input
- [x] PlayerStats component for character data
- [x] Smooth animation blending between directions
- [x] Fixed lockout of diagonal movement

**Documentation:** See [README.md](README.md)

---

## Phase 2: Inventory System ✅ COMPLETE

### Core Inventory
- [x] 30-slot inventory grid (5x6)
- [x] InventoryManager backend with slot management
- [x] Item scriptable objects (Item.asset)
- [x] Inventory UI display with icons
- [x] Item quantity display for stackable items

### Hotbar/Quickbar (Independent)
- [x] 10-slot hotbar UI at bottom of screen
- [x] HotbarSlot class for independent slot references
- [x] Number key selection (1-9, 0)
- [x] Mouse wheel cycling through hotbar
- [x] Green highlight for selected hotbar slot
- [x] Right-click inventory items to assign to hotbar
- [x] Right-click again to remove from hotbar

### Interaction Features
- [x] Drag & drop between inventory slots
- [x] Drag & drop between inventory and hotbar
- [x] Shift+Click to drop single item
- [x] Shift+Drag to drop entire stack
- [x] Left-click hotbar to use item (when inventory closed)
- [x] Hotbar auto-updates when items consumed

### Item Spawning & Pickup
- [x] ItemPickup prefab with bob animation
- [x] Random position offset to prevent stacking
- [x] 0.5 second pickup delay after drop
- [x] Auto-pickup when walking over items
- [x] Inventory integration with drop system

**Documentation:** See [INVENTORY_SETUP_GUIDE.md](INVENTORY_SETUP_GUIDE.md) and [HOTBAR_QUICKBAR_GUIDE.md](HOTBAR_QUICKBAR_GUIDE.md)

---

## Phase 3: Character Stats UI Window ✅ COMPLETE

Two-part character UI system:

### Part 1: Permanent In-Game HUD ✅
- [x] Create StatHUDUI component
- [x] Display Health bar with text (current/max)
- [x] Display Stamina bar with text (current/max)
- [x] Display Hunger bar with text (current/max)
- [x] Real-time updates on stat changes
- [x] Color-coded bars (red, yellow, green)
- [x] Subscribe to PlayerStats events
- [x] Clean up event subscriptions

### Part 2: Character Sheet Window ✅
- [x] Create CharacterSheetUI component
- [x] Toggle with C key
- [x] Show character name and level
- [x] Display experience/level info
- [x] Show all core stats (Health, Stamina, Hunger)
- [x] Show secondary stats placeholders (Defense, Attack)
- [x] Display active buffs
- [x] Close button functionality
- [x] CanvasGroup for smooth show/hide
- [x] Subscribe to stat change events

### Testing ✅
- [x] HUD displays health/stamina/hunger bars
- [x] Character sheet opens with C key
- [x] Character sheet closes with C key or button
- [x] All stats display correctly
- [x] Real-time updates when stats change
- [x] Both systems work simultaneously

**Documentation:** See [CHARACTER_UI_GUIDE.md](CHARACTER_UI_GUIDE.md)

---

## Phase 4: Consumable Items 🔄 IN PLANNING

Items that can be used/consumed to restore stats or apply effects:

### Consumable Types
- [ ] **Food** (health restoration, short duration)
- [ ] **Potions** (instant effect, ability to stack)
- [ ] **Poisons** (negative effects on enemies)
- [ ] **Scrolls** (temporary buff effects)

### Implementation Tasks
- [ ] Create Consumable class extending Item
- [ ] Add `Use(GameObject target)` override
- [ ] Implement stat restoration logic
- [ ] Add effect duration system (if needed)
- [ ] Create test consumable items (apple, health potion, etc.)
- [ ] Add consumable icons/sprites
- [ ] Test usage through inventory and hotbar
- [ ] Add consumption feedback (UI notification, sound effect)

### Testing
- [ ] Right-click to use consumable
- [ ] Hotbar key to use consumable
- [ ] Stats update when consumed
- [ ] Item quantity decreases
- [ ] Hotbar slot clears when last item consumed
- [ ] Effects apply correctly (health +50, etc.)

---

## Phase 5: Equipment System 🔄 IN PLANNING

Wearable items that provide stat bonuses:

### Equipment Slots
- [ ] Add equipment slot UI (Head, Chest, Hands, Feet, Weapon, Offhand)
- [ ] Create Equipment class extending Item
- [ ] Stat modification system
- [ ] Visual preview on player character

### Equipment Types
- [ ] **Armor** (head, chest, hands, feet with defense stat)
- [ ] **Weapons** (damage, attack speed stats)
- [ ] **Accessories** (rings, amulets with mixed stats)

### Implementation Tasks
- [ ] Add equipment inventory parallel to main inventory
- [ ] Left-click to equip/unequip
- [ ] Show stat bonuses in tooltip
- [ ] Apply stat modifiers to PlayerStats
- [ ] Remove stat modifiers when unequipped
- [ ] Prevent equipping same item twice
- [ ] Create test equipment items

### Testing
- [ ] Equip/unequip from inventory
- [ ] Stats update when equipped
- [ ] Equip slot blocks duplicate items
- [ ] Visual feedback on character (optional)
- [ ] Swapping equipment works smoothly

---

## Phase 6: Item Tooltips & UI Polish 🔄 IN PLANNING

Enhanced item information display:

### Tooltip System
- [ ] Create Tooltip UI prefab (panel with text)
- [ ] Show on hover (inventory items)
- [ ] Display item name, quantity, rarity
- [ ] Show consumable effects (e.g., "+50 Health")
- [ ] Show equipment stats (e.g., "+5 Defense")
- [ ] Tooltip disappears on mouse leave

### Additional Polish
- [ ] Item rarity color coding (common, uncommon, rare, epic, legendary)
- [ ] Drag cursor changes based on item type
- [ ] Equipped item indicator in inventory
- [ ] Quantity below 2 shows "1" instead of blank
- [ ] Sound effects for interactions (drop, pickup, equip)

### Implementation
- [ ] Add Item.Description field
- [ ] Create TooltipUI component
- [ ] Subscribe to ItemSlotUI hover events
- [ ] Format tooltip text with item data
- [ ] Test tooltip positioning (stays on screen)

---

## Phase 7: Advanced Inventory Features 🔄 IN PLANNING

Quality-of-life improvements:

### Stack Splitting
- [ ] Ctrl+Click to split stacks in half
- [ ] Drag split stack to new slot
- [ ] Split UI to show quantity input

### Sorting & Organization
- [ ] Sort inventory button (by type/rarity/name)
- [ ] Organize hotbar button
- [ ] Destroy duplicate stacks option

### Item Management
- [ ] Right-click item to destroy/sell
- [ ] Confirmation dialog for destruction
- [ ] Destroy all of type option
- [ ] Mark items as favorite/protected

---

## Phase 8: Storage & Containers 🔄 IN PLANNING

Chest/barrel/locker systems for storage:

### Container System
- [ ] Create Container scriptable object
- [ ] ContainerUI for viewing container inventory
- [ ] Open/close container UI
- [ ] Drag & drop between player inventory and container

### Container Types
- [ ] Chests (generic 50-slot storage)
- [ ] Barrels (themed storage, 25-slot)
- [ ] Lockers (small, 10-slot)
- [ ] Shops (read-only merchant inventory)

### Implementation
- [ ] PlaceableContainer component for world objects
- [ ] Open interaction (E key or click)
- [ ] Save/load container contents
- [ ] Drop limit (weight system if added)

### Testing
- [ ] Open multiple containers
- [ ] Drag items to/from containers
- [ ] Container contents persist after close
- [ ] Remove container deletes contents safely

---

## Phase 9: Crafting System 🔄 IN PLANNING

Recipe-based item creation:

### Recipe System
- [ ] Create Recipe scriptable object
- [ ] Define recipe inputs (ingredients)
- [ ] Define recipe outputs (result + quantity)
- [ ] Recipe difficulty/level requirement

### Crafting UI
- [ ] Crafting menu to browse recipes
- [ ] Show required ingredients
- [ ] Check if player has ingredients
- [ ] Craft button (creates item, removes ingredients)

### Implementation
- [ ] RecipeManager to track known recipes
- [ ] Crafting bench/workbench in world
- [ ] Learn recipe from NPC or loot
- [ ] Recipe book UI
- [ ] Craft confirmation dialog

### Testing
- [ ] Craft item with valid ingredients
- [ ] Can't craft without ingredients
- [ ] Output items added to inventory
- [ ] Ingredients consumed

---

## Phase A: NPC Trading & Commerce 🔄 IN PLANNING

NPC merchant interactions:

### Trading System
- [ ] NPC shop UI (buy/sell tabs)
- [ ] NPC inventory (separate from player)
- [ ] Buy items from NPC
- [ ] Sell items to NPC

### Economy
- [ ] Item value property
- [ ] NPC buy price (discount)
- [ ] NPC sell price (markup)
- [ ] Player gold/currency system
- [ ] Gold display in UI

### Implementation
- [ ] NPCMerchant component
- [ ] ShopUI prefab
- [ ] Dialogue trigger to open shop
- [ ] Save NPC inventory state

### Testing
- [ ] Open shop UI
- [ ] Buy item (gold decreases, item added)
- [ ] Sell item (gold increases, item removed)
- [ ] NPC inventory limits
- [ ] Shop prices are correct

---

## Phase B: Dialogue System 🔄 IN PLANNING

NPC conversation system:

- [ ] Dialogue tree data structure
- [ ] DialogueUI for displaying conversations
- [ ] Player choice branching
- [ ] NPC response variations
- [ ] Quest integration hook
- [ ] Save dialogue state

---

## Phase C: Quest System 🔄 IN PLANNING

Mission/objective tracking:

- [ ] Quest data structure (objectives, rewards)
- [ ] QuestManager for quest tracking
- [ ] Quest UI log
- [ ] Track quest progress
- [ ] Turn in quest for rewards
- [ ] Quest markers in world

---

## Phase D: Save/Load System 🔄 IN PLANNING

Game persistence:

- [ ] Save player state (position, stats)
- [ ] Save inventory contents
- [ ] Save hotbar assignments
- [ ] Save equipment
- [ ] Save container contents
- [ ] Save quest progress
- [ ] Save/Load UI

---

## Priority Order

**High Priority (Next):**
1. Character Stats UI (Phase 3) - foundation for consumables/equipment
2. Consumable items (Phase 4) - foundation for item usage
3. Equipment system (Phase 5) - stat system expansion
4. Item tooltips (Phase 6) - UX improvement

**Medium Priority:**
5. Stack splitting (Phase 7) - inventory QoL
6. Containers/Storage (Phase 8) - world building

**Lower Priority:**
7. Crafting (Phase 9) - game depth
8. NPC Trading (Phase A) - economy
9. Dialogue (Phase B) - character interaction
10. Quests (Phase C) - quest log
11. Save/Load (Phase D) - persistence

---

## Statistics

| Category | Completed | In Progress | To Do | Total |
|----------|-----------|-------------|-------|-------|
| Core Systems | 7 | 0 | 3 | 10 |
| Inventory | 13 | 0 | 2 | 15 |
| Items | 7 | 0 | 27 | 34 |
| UI | 10 | 0 | 12 | 22 |
| **TOTAL** | **37** | **0** | **44** | **81** |

---

## Notes

- All Phase 2 systems tested and working
- **Phase 3 (Character UI) complete** - both HUD and character sheet UIs ready
- Foundation now in place to test consumables with visible stat changes
- Ready to begin Phase 4 (Consumables) when user confirms
- Documentation updated for completed phases
- No blocking issues or technical debt

**Last Session:** February 8, 2026 - Phase 3 (Character UI) completed

---

## How to Update This Document

- ✅ Mark tasks when code is written and tested
- ☐ Uncheck if issues are found
- 🔄 Move tasks to "IN PLANNING" when beginning phase
- Link to documentation when phase completes
- Update statistics at bottom

---

**Current Focus:** Awaiting direction on next phase (likely Phase 3: Consumables)
