# Highlands 2: Phase 3 - Character UI Implementation Guide

## Overview

This phase introduces a two-part character UI system:

1. **Permanent In-Game HUD** - Always visible stats bars (Health, Stamina, Hunger)
2. **Character Sheet Window** - Optional detailed stats view (opened with C key)

**Phase 3 Completion Date:** February 8, 2026

---

## System Architecture

### Part 1: Permanent In-Game HUD

#### StatHUDUI.cs [Assets/Project/Scripts/UI/StatHUDUI.cs]

**Purpose:** Display core player stats as permanent UI elements
- Health bar with "Health: current/max" text
- Stamina bar with "Stamina: current/max" text
- Hunger bar with "Hunger: current/max" text

**Features:**
- Real-time updates via PlayerStats events
- Color-coded bars (red, yellow, green)
- Simple, clean layout suitable for gameplay
- Always visible in bottom-left or top-left corner

**Implementation:**
```csharp
public class StatHUDUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    
    // Subscribe to OnHealthChanged, OnStaminaChanged, OnHungerChanged
    // Update bar fill amounts and text displays
    // Clean up event subscriptions on destroy
}
```

**Methods:**
- `UpdateHealthDisplay(int current, int max)` - Updates health bar and text
- `UpdateStaminaDisplay(int current, int max)` - Updates stamina bar and text
- `UpdateHungerDisplay(int current, int max)` - Updates hunger bar and text

### Part 2: Character Sheet Window

#### CharacterSheetUI.cs [Assets/Project/Scripts/UI/CharacterSheetUI.cs]

**Purpose:** Detailed character information when viewer opens character window
- Opened with C key
- Toggleable overlay
- Shows detailed stats and buffs
- Can be closed with close button or C key again

**Features:**
- Toggle with C key
- Shows character name, level, experience
- Detailed stat display
- Active buff information
- Close button
- CanvasGroup for smooth show/hide

**Implementation:**
```csharp
public class CharacterSheetUI : MonoBehaviour
{
    [SerializeField] private CharacterSheetPanel panel;
    
    // Toggle with C key
    // Open() - show with full alpha
    // Close() - hide with 0 alpha
    // IsOpen property
}
```

**Methods:**
- `Toggle()` - Open if closed, close if open
- `Open()` - Display character sheet
- `Close()` - Hide character sheet
- `UpdateStatsDisplay(int, int)` - Refresh all stat displays

---

## Setup Instructions

### Part 1: StatHUDUI Setup

#### Step 1: Canvas Hierarchy
```
MainCanvas
├── HUDPanel (Top-Left, 200x200)
│   ├── HealthContainer
│   │   ├── HealthBackgroundBar (Image)
│   │   ├── HealthFillBar (Image - Red)
│   │   └── HealthText (Text)
│   ├── StaminaContainer
│   │   ├── StaminaBackgroundBar (Image)
│   │   ├── StaminaFillBar (Image - Yellow)
│   │   └── StaminaText (Text)
│   └── HungerContainer
│       ├── HungerBackgroundBar (Image)
│       ├── HungerFillBar (Image - Green)
│       └── HungerText (Text)
└── StatHUDUI (Script)
```

#### Step 2: Create Health Bar
1. Create Panel named "HUDPanel" as child of Canvas
   - Anchors: Top-Left (0, 1)
   - Position: (100, -50)
   - Size: (200, 200)
   - Background: Transparent or dark gray

2. Create Image named "HealthBackgroundBar" as child of HUDPanel
   - Anchors: Stretch horizontally, top
   - Size: (180, 20)
   - Color: Dark gray (0.2, 0.2, 0.2)

3. Create Image named "HealthFillBar" as child of HealthBackgroundBar
   - Anchors: Left (0, 0) to (0, 1)
   - Size: (180, 20)
   - Color: Red (1, 0, 0)
   - Image Type: Filled (Radial 360)

4. Create Text "HealthText" as child of HealthBackgroundBar
   - Text: "Health: 100/100"
   - Font Size: 14
   - Color: White
   - Alignment: Center

5. **Repeat Steps 2-4 for Stamina (Yellow #FFFF00) and Hunger (Green #00FF00)**

#### Step 3: Add StatHUDUI Script
1. Add `StatHUDUI.cs` script to HUDPanel or Canvas
2. Assign:
   - PlayerStats: [Find Player in scene]
   - Health Bar: HealthFillBar Image component
   - Health Text: HealthText Text component
   - Stamina Bar, Stamina Text (same structure)
   - Hunger Bar, Hunger Text (same structure)

3. Play scene and verify bars update when stats change

---

### Part 2: CharacterSheetUI Setup

#### Step 1: Character Sheet Panel
1. Create Panel named "CharacterSheetPanel" as child of Canvas
   - Anchors: Center (0.5, 0.5)
   - Position: (0, 0)
   - Size: (400, 500)
   - Background: Dark with slight transparency

2. Add CanvasGroup component
   - Initial Alpha: 0 (hidden)
   - Interactable: False
   - Blocks Raycasts: False

#### Step 2: Header Section
```
CharacterSheetPanel
├── HeaderPanel
│   ├── CharacterNameText ("Player")
│   ├── LevelText ("Level: 1")
│   └── ExperienceText ("Exp: 0/1000")
```

#### Step 3: Stats Section
```
CharacterSheetPanel
├── StatsPanel
│   ├── HealthContainer
│   │   ├── Label ("Health:")
│   │   └── HealthValueText ("100/100")
│   ├── StaminaContainer
│   │   ├── Label ("Stamina:")
│   │   └── StaminaValueText ("100/100")
│   ├── HungerContainer
│   │   ├── Label ("Hunger:")
│   │   └── HungerValueText ("100/100")
│   ├── DefenseContainer
│   │   ├── Label ("Defense:")
│   │   └── DefenseValueText ("0")
│   └── AttackContainer
│       ├── Label ("Attack:")
│       └── AttackValueText ("0")
```

#### Step 4: Buffs Section
```
CharacterSheetPanel
├── BuffsPanel
│   ├── BuffLabel ("Active Buffs:")
│   └── ActiveBuffsText ("No active buffs")
```

#### Step 5: Close Button
1. Create Button named "CloseButton"
2. Add Text child "Text" - "Close" or "X"
3. Position: Bottom-right of panel

#### Step 6: Add CharacterSheetUI Script
1. Add `CharacterSheetUI.cs` script to Canvas
2. Assign:
   - PlayerStats: [Find Player in scene]
   - Character Sheet Panel: [CanvasGroup component]
   - Close Button: [Button component]
   - All Text fields (Name, Level, Experience, Health, Stamina, etc.)

3. Play scene and test:
   - Press C to open/close
   - Verify all stats display correctly
   - Check that buttons work

---

## UI Layout Reference

### StatHUDUI - Top-Left Corner
```
┌─────────────────┐
│ Health: 100/100 │
│ ■■■■■■■■□□□□□□ │
├─────────────────┤
│ Stamina: 100/100│
│ ■■■■■■■■□□□□□□ │
├─────────────────┤
│ Hunger: 100/100 │
│ ■■■■■■■■□□□□□□ │
└─────────────────┘
```

### CharacterSheetUI - Center Screen (Hidden by default, C to toggle)
```
╔═══════════════════════════╗
║        CHARACTER          ║
║       Player - Lvl 1      ║
║     Exp: 0/1000           ║
╠═══════════════════════════╣
║ ATTRIBUTES                ║
║ Health:     100/100       ║
║ Stamina:    100/100       ║
║ Hunger:     100/100       ║
║ Defense:    0             ║
║ Attack:     0             ║
╠═══════════════════════════╣
║ BUFFS                     ║
║ No active buffs           ║
╠═══════════════════════════╣
║           [CLOSE]         ║
╚═══════════════════════════╝
```

---

## Testing Checklist

### StatHUDUI Tests
- [ ] HUD displays on screen
- [ ] Health bar shows correct value
- [ ] Stamina bar shows correct value
- [ ] Hunger bar shows correct value
- [ ] Text displays "Health: X/Y" format
- [ ] Bars update when stats change
- [ ] Bars are correct colors (red, yellow, green)
- [ ] HUD stays visible during gameplay

### CharacterSheetUI Tests
- [ ] Press C to open character sheet
- [ ] Character name displays
- [ ] Level shows as "Level: 1"
- [ ] Experience shows "0/1000"
- [ ] All stats display current values
- [ ] Active buffs text shows "No active buffs"
- [ ] Panel is centered on screen
- [ ] Close button closes panel
- [ ] Press C again closes panel
- [ ] Panel hidden at start (alpha 0)

### Integration Tests
- [ ] Both UIs exist simultaneously
- [ ] HUD visible with character sheet open
- [ ] Both update when stats change
- [ ] Use food from hotbar and see HUD update
- [ ] Use food and see character sheet update
- [ ] Take damage and see HUD bar decrease
- [ ] Gain hunger and see hunger bar increase

---

## Data Flow

### Stat Changes
```
PlayerStats.RestoreHealth() 
    ↓
OnHealthChanged event
    ↓
StatHUDUI.UpdateHealthDisplay()  [HUD updates]
CharacterSheetUI.UpdateStatsDisplay()  [Sheet updates]
```

### Connection
- **StatHUDUI** subscribes to: OnHealthChanged, OnStaminaChanged, OnHungerChanged
- **CharacterSheetUI** subscribes to: Same events (for update triggers)
- Both update independently based on event callbacks

---

## Ready for Phase 4: Consumables

With this system in place, we can now:
1. Create consumable items that modify stats
2. Use items from hotbar
3. **See the effects immediately in StatHUDUI**
4. Open character sheet with C to view detailed changes

Example flow:
- Player in-game sees HUD
- Player presses hotbar key 1 (has Apple)
- Apple is consumed → RestoreHunger(+20)
- PlayerStats.OnHungerChanged event fires
- StatHUDUI.UpdateHungerDisplay() called
- Hunger bar increases from 80 to 100
- Player can press C to see detailed stat view
- Player's hunger now 100/100 in character sheet

---

## Performance Notes

- **StatHUDUI:** Lightweight, only updates on stat change events
- **CharacterSheetUI:** Only active when panel is open
- **Event-driven:** No polling, true reactive update system
- **Memory:** Minimal - only text and image references

---

## Known Limitations & Future Enhancements

### Current Limitations
1. Character name hardcoded to "Player"
2. Level/Experience not yet implemented
3. Defense/Attack stats placeholder (will be populated by equipment in Phase 5)
4. No visual character portrait
5. No equipment display area

### Future Enhancements
1. **Character Portrait:** Add sprite/image of player
2. **Equipment Slots:** Show equipped items in sheet
3. **Detailed Buffs:** List all active buffs with duration
4. **Status Conditions:** Show if poisoned, blessed, etc.
5. **Stats from Equipment:** Calculate total stats including bonuses
6. **Advanced UI:** Tabbed interface (Stats, Equipment, Spells, etc.)

---

## Code Statistics

| File | Lines | Purpose |
|------|-------|---------|
| StatHUDUI.cs | 106 | Permanent in-game stat display |
| CharacterSheetUI.cs | 167 | Detailed character info window |

---

## Related Documentation

- **Phase 1:** Character & Animation
- **Phase 2:** Inventory & Hotbar
- **Phase 4:** Consumable Items (next - uses this system to show effects)
- **Phase 5:** Equipment System (will update Defense/Attack stats)

---

**Status:** ✅ Complete - Character UI foundation ready for consumables integration.
