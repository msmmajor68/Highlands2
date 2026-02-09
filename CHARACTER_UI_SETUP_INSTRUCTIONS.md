# Character UI Setup - Step-by-Step Instructions

## Color Format Notes

**In Unity Inspector (GUI):** Use **Hex values** like `#333333B3`
- Paste directly into the color picker's hex field
- Easiest for visual editing

**In C# Code:** Use **RGBA (0-1 format)** like `new Color(0.2f, 0.2f, 0.2f, 0.7f)`
- This is the industry standard for Unity scripting
- 0.0 = 0% (black), 1.0 = 100% (white)
- Values between 0-1 represent percentages of color saturation

**Conversion:**
- Hex `#33` = Decimal `51` = Code `0.2` (51 ÷ 255 = 0.2)
- Hex `#FF` = Decimal `255` = Code `1.0` (255 ÷ 255 = 1.0)

Throughout this guide, colors show **Hex first (for GUI)** then **code format (for reference)**.

---

## Part 1: Permanent In-Game HUD (StatHUDUI)

### Step 1.1: Create HUD Panel Container
1. In Hierarchy, right-click on **InventoryCanvas** (or main Canvas)
2. Select **UI > Panel - Image**
3. Rename to **HUDPanel**
4. With HUDPanel selected:
   - **Rect Transform Settings:**
     - Anchors: Top-Left (preset)
     - Position X: 100, Y: -50
     - Width: 250, Height: 180
   - **Image Component:**
     - Color: **#333333B3** (or code: 0.2, 0.2, 0.2, 0.7) - Dark semi-transparent
   - **Layout Element (add if needed):**
     - Preferred Width: 250
     - Preferred Height: 180

### Step 1.2: Create Health Bar Container
1. Inside HUDPanel, create a new Panel: Right-click HUDPanel > **UI > Panel - Image**
2. Rename to **HealthContainer**
3. Settings:
   - Anchors: Top-Stretch (stretch horizontally)
   - Position Y: 0 (top)
   - Height: 30
   - Image Color: **#1A1A1ACC** (or code: 0.1, 0.1, 0.1, 0.8)

### Step 1.3: Create Health Bar Background
1. Inside HealthContainer, create **UI > Image**
2. Rename to **HealthBarBackground**
3. Settings:
   - Anchors: Stretch both
   - Width/Height: Auto (fills container)
   - Color: **#4D0D0DFF** (or code: 0.3, 0.05, 0.05, 1) - Dark red

### Step 1.4: Create Health Bar Fill
1. Inside HealthBarBackground, create **UI > Image**
2. Rename to **HealthBarFill**
3. Settings:
   - Anchors: Left (0, 0.5) - starts from left, centered vertically
   - Width: 220 (fill amount will modify this)
   - Height: 20
   - X Position: 5, Y: 0
   - Color: **#FF0000FF** (or code: 1, 0, 0, 1) - Bright red
   - **Image Component:**
     - Image Type: **Filled**
     - Fill Method: **Horizontal**
     - Origin X: **Left**

### Step 1.5: Create Health Text
1. Inside HealthContainer, create **UI > TextMeshPro - Text**
2. Rename to **HealthText**
3. Settings:
   - Anchors: Center
   - Text: "Health: 100/100"
   - Font Size: 18
   - Color: White **#FFFFFFFF** (or code: 1, 1, 1, 1)
   - Alignment: Center-Middle
   - Width: 200, Height: 30

### Step 1.6: Duplicate for Stamina
1. **Right-click HealthContainer > Copy**
2. Right-click HUDPanel > Paste as Child
3. Rename to **StaminaContainer**
4. Position Y: -40 (below health)
5. Select **StaminaBarFill** (the fill inside)
   - Color: **#FFFF00FF** (or code: 1, 1, 0, 1) - Bright yellow
   - Change parent Image (StaminaBarBackground) to yellow: **#4D4D00FF** (or code: 0.3, 0.3, 0, 1)
6. Select **StaminaText**
   - Text: "Stamina: 100/100"

### Step 1.7: Duplicate for Hunger
1. **Right-click StaminaContainer > Copy**
2. Right-click HUDPanel > Paste as Child
3. Rename to **HungerContainer**
4. Position Y: -80 (below stamina)
5. Select **HungerBarFill** (the fill inside)
   - Color: **#00FF00FF** (or code: 0, 1, 0, 1) - Bright green
   - Change parent Image (HungerBarBackground) to green: **#004D00FF** (or code: 0, 0.3, 0, 1)
6. Select **HungerText**
   - Text: "Hunger: 100/100"

### Step 1.8: Add StatHUDUI Script
1. Select **HUDPanel**
2. In Inspector, click **Add Component**
3. Search for **StatHUDUI**
4. Add the component
5. **Drag and assign:**
   - Player Stats: Drag from Hierarchy or use Find
   - Health Bar: Drag **HealthBarFill** Image component
   - Health Text: Drag **HealthText** TextMeshPro component
   - Stamina Bar: Drag **StaminaBarFill** Image component
   - Stamina Text: Drag **StaminaText** TextMeshPro component
   - Hunger Bar: Drag **HungerBarFill** Image component
   - Hunger Text: Drag **HungerText** TextMeshPro component

### Step 1.9: Test HUD
1. Play the game
2. Check HUD appears in top-left
3. Verify bars show 100/100 for all stats
4. In Console, type: `find player` to debug if PlayerStats not found

---

## Part 2: Character Sheet Window (CharacterSheetUI)

### Step 2.1: Create Main Character Sheet Panel
1. In Hierarchy, right-click **InventoryCanvas**
2. Select **UI > Panel - Image**
3. Rename to **CharacterSheetPanel**
4. Settings:
   - Anchors: Center
   - Position: X: 0, Y: 0
   - Size: Width: 450, Height: 550
   - Image Color: **#262626F2** (or code: 0.15, 0.15, 0.15, 0.95) - Dark gray, opaque

### Step 2.2: Add CanvasGroup
1. With CharacterSheetPanel selected
2. In Inspector, click **Add Component**
3. Search for **CanvasGroup**
4. Initially set:
   - Alpha: 0 (hidden)
   - Interactable: OFF (unchecked)
   - Blocks Raycasts: OFF (unchecked)

### Step 2.3: Create Header Section
1. Inside CharacterSheetPanel, create **UI > Panel - Image**
2. Rename to **HeaderPanel**
3. Settings:
   - Anchors: Top-Stretch
   - Height: 80
   - Image Color: **#1A1A4DFF** (or code: 0.1, 0.1, 0.3, 1) - Dark blue header

#### Add Header Text Elements:
1. Inside HeaderPanel, create **UI > TextMeshPro - Text**
   - Rename to **CharacterNameText**
   - Text: "Player"
   - Font Size: 24, Bold
   - Position: Y: -10
   - Width: 400

2. Duplicate HeaderPanel's text area, rename to **LevelText**
   - Text: "Level: 1"
   - Font Size: 16
   - Position: Y: -35

3. Duplicate LevelText, rename to **ExperienceText**
   - Text: "Exp: 0/1000"
   - Font Size: 14
   - Position: Y: -55

### Step 2.4: Create Stats Section
1. Inside CharacterSheetPanel, create **UI > Panel - Image**
2. Rename to **StatsPanel**
3. Settings:
   - Anchors: Stretch both
   - Top: 90, Bottom: 100
   - Image Color: Transparent or **#33333380** (or code: 0.2, 0.2, 0.2, 0.5)
   - Add **Vertical Layout Group:**
     - Preferred Height: 300
     - Child Force Expand Height: ON
     - Child Force Expand Width: ON

### Step 2.5: Create Stat Entries
For each stat (Health, Stamina, Hunger, Defense, Attack), create this structure:

**Health Row:**
1. Inside StatsPanel, create **UI > Panel - Image**
   - Rename to **HealthRow**
   - Height: 40
   - Color: Transparent or **#401A1A4D** (or code: 0.25, 0.1, 0.1, 0.3)
   - Add **Horizontal Layout Group**

2. Inside HealthRow, create **UI > TextMeshPro - Text**
   - Rename to **HealthLabel**
   - Text: "Health:"
   - Font Size: 16
   - Fixed Width: 120

3. Inside HealthRow, create **UI > TextMeshPro - Text**
   - Rename to **HealthValueText**
   - Text: "100 / 100"
   - Font Size: 16
   - Color: Light green

**Stamina Row:** (Duplicate HealthRow)
   - Change HealthLabel text to "Stamina:"
   - Change HealthValueText to **StaminaValueText** with text "100 / 100"

**Hunger Row:** (Duplicate StaminaRow)
   - Change to "Hunger:" and **HungerValueText** with "100 / 100"

**Defense Row:** (Duplicate HungerRow)
   - Change to "Defense:" and **DefenseValueText** with "0"

**Attack Row:** (Duplicate DefenseRow)
   - Change to "Attack:" and **AttackValueText** with "0"

### Step 2.6: Create Buffs Section
1. Inside CharacterSheetPanel (below StatsPanel), create **UI > Panel - Image**
2. Rename to **BuffsPanel**
3. Settings:
   - Anchors: Stretch horizontally, bottom
   - Height: 80
   - Color: **#26332680** (or code: 0.15, 0.2, 0.15, 0.5)

#### Add Buffs Text:
1. Inside BuffsPanel, create **UI > TextMeshPro - Text**
   - Rename to **BuffsLabel**
   - Text: "Active Buffs:"
   - Font Size: 14
   - Position: Top-left with padding

2. Inside BuffsPanel, create **UI > TextMeshPro - Text**
   - Rename to **ActiveBuffsText**
   - Text: "No active buffs"
   - Font Size: 14
   - Color: Yellow **#FFFF00FF** (or code: 1, 1, 0, 1)

### Step 2.7: Create Close Button
1. Inside CharacterSheetPanel (bottom area), create **UI > Button - TextMeshPro**
2. Rename to **CloseButton**
3. Settings:
   - Anchors: Bottom-Center
   - Position Y: 20
   - Size: Width: 100, Height: 40
   - Button Color: **#4D1A1AFF** (or code: 0.3, 0.1, 0.1, 1) - Dark red
   - Text: "Close"
   - Font Size: 16

### Step 2.8: Add CharacterSheetUI Script
1. Select **InventoryCanvas** (or root canvas)
2. In Inspector, click **Add Component**
3. Search for **CharacterSheetUI**
4. Add the component
5. **Drag and assign:**
   - Player Stats: Find or drag Player from Hierarchy
   - Character Sheet Panel: Drag **CharacterSheetPanel > CanvasGroup**
   - Close Button: Drag **CloseButton** Button component
   - Character Name Text: Drag **CharacterNameText**
   - Level Text: Drag **LevelText**
   - Experience Text: Drag **ExperienceText**
   - Health Value Text: Drag **HealthValueText**
   - Stamina Value Text: Drag **StaminaValueText**
   - Hunger Value Text: Drag **HungerValueText**
   - Defense Value Text: Drag **DefenseValueText**
   - Attack Value Text: Drag **AttackValueText**
   - Active Buffs Text: Drag **ActiveBuffsText**

---

## Testing the UI

### Test Sequence:
1. **Play the game**
2. **Check HUD appears** in top-left corner with all bars at 100
3. **Press C key** - Character sheet should fade in and appear centered
4. **Verify character sheet shows:**
   - Player name "Player"
   - Level 1
   - All stats at 100/100 (or 0 for Defense/Attack)
   - "No active buffs" text
5. **Press C key again** - Character sheet should fade out
6. **Console test:**
   ```csharp
   // In debug console while playing:
   // Find player and adjust stats:
   var player = FindObjectOfType<PlayerStats>();
   player.RestoreHealth(10);  // Health should increase in HUD
   player.RestoreHunger(-20); // Hunger should decrease in HUD
   ```

### Troubleshooting:

| Issue | Solution |
|-------|----------|
| HUD doesn't appear | Check HUDPanel CanvasGroup, verify StatHUDUI script assigned |
| Bars don't fill | Verify Image Type is "Filled" and Fill Method is "Horizontal" |
| Character sheet won't open | Check C key not already used, verify CharacterSheetUI on Canvas |
| Stats show wrong values | Verify all TextMeshPro fields assigned in inspector |
| Bars scale incorrectly | Check RectTransform anchors are correct (Top-Left for HUD, Left for bars) |

### Quick Fix Checklist:
- [ ] All Image components have correct colors assigned
- [ ] TextMeshPro components have font size 14-24
- [ ] CanvasGroup on CharacterSheetPanel initially Alpha = 0
- [ ] Close button click event connected to CloseButton.onClick
- [ ] Both StatHUDUI and CharacterSheetUI scripts added to Canvas
- [ ] PlayerStats component found on Player GameObject

---

## Hierarchy Summary

When complete, your hierarchy should look like:
```
Canvas (InventoryCanvas)
├── InventoryPanel (existing)
├── HotbarPanel (existing)
├── HUDPanel (NEW - StatHUDUI script here)
│   ├── HealthContainer
│   │   ├── HealthBarBackground
│   │   │   └── HealthBarFill (Image)
│   │   └── HealthText
│   ├── StaminaContainer
│   │   ├── StaminaBarBackground
│   │   │   └── StaminaBarFill (Image)
│   │   └── StaminaText
│   └── HungerContainer
│       ├── HungerBarBackground
│       │   └── HungerBarFill (Image)
│       └── HungerText
├── CharacterSheetPanel (NEW - CanvasGroup + CharacterSheetUI on parent)
│   ├── HeaderPanel
│   │   ├── CharacterNameText
│   │   ├── LevelText
│   │   └── ExperienceText
│   ├── StatsPanel
│   │   ├── HealthRow
│   │   ├── StaminaRow
│   │   ├── HungerRow
│   │   ├── DefenseRow
│   │   └── AttackRow
│   ├── BuffsPanel
│   │   ├── BuffsLabel
│   │   └── ActiveBuffsText
│   └── CloseButton
└── InventoryUI (script - existing)
```

---

## Next Steps

Once UI is set up and working:
1. Verify both UIs appear and function correctly
2. Test pressing C to toggle character sheet
3. Test that HUD updates in real-time
4. **Then move to Phase 4: Consumable Items**
   - Create food/potion items
   - Add Use() functionality to consume and restore stats
   - See effects immediately in HUD!

