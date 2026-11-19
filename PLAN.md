# Starfox 3D — Project plan (Jun Zhe Xia & Daiwei Chen)

Guide document: architecture, task split and checklist. All code is written by us.

Repo layout:
```
VJ_2026-2027_Starfox/      <- git repo root (GitHub: Lexter356/VJ_2026-2027_Starfox)
├── PLAN.md, info.txt      <- docs
├── Starfox/               <- Unity project (Unity 6000.6.9f1, URP)  -> becomes "Projecte/" in the zip
└── Binari/                <- builds (git-ignored)
```

---

## 0. Setup

1. Both use **Unity 6000.6.9f1** exactly (check the lab PCs too).
2. Windows: **Smart App Control must be OFF**, otherwise Windows blocks Unity's compiled scripts.
3. Project lives on the **Windows disk**, never inside WSL.
4. Settings: Asset Serialization = Force Text, Version Control = Visible Meta Files, Active Input Handling = Input System (new).
5. **Merge rule:** never both edit the same `.unity` scene at once. Each person works in their own test
   scene or in prefabs; only one person touches `Level.unity` per day. Pull before opening Unity, commit
   + push when you close it.

---

## 1. Architecture (what each script is responsible for)

Write these yourselves — this is the "what", not the "how".

| Script | Responsibility | Key ideas to research |
|---|---|---|
| `RailMover` | Moves an empty "rail" object forward along +Z at constant speed. Camera and player are **children** of it. | `Transform.Translate`, `Time.deltaTime` |
| `PlayerController` | WASD moves the ship in **local X/Y** inside a clamped rectangle; ship tilts (bank/pitch) toward movement. | `Mathf.Clamp`, `Quaternion.Slerp`, local vs world position |
| `PlayerAim` | Mouse position → ray from camera → point at fixed distance ahead; ship nose looks at it. Draw a crosshair (UI) there. | `Camera.ScreenPointToRay`, `Transform.LookAt` |
| `PlayerWeapons` | LMB: laser (fire rate, level 1/2/3 = single/double/blue). RMB: bomb (limited count, explodes with area damage). | object pooling, `Physics.OverlapSphere` |
| `BarrelRoll` | Double-tap A/D (or Q/E) → 360° roll over ~0.4 s, invulnerable during it, cooldown. | coroutines, flag read by `Health` |
| `Health` (shared) | HP, `TakeDamage`, invulnerability, death event. Used by player, enemies, boss. | C# `event`/`UnityEvent` |
| `PlayerState` | Shield, lives, bombs, laser level. Resets laser level on death. | |
| `GameManager` (singleton) | Lives, current checkpoint, respawn, game over / win, cheat keys **0–3**, ESC → menu. | `SceneManager`, `DontDestroyOnLoad` |
| `Checkpoint` | Trigger on the rail path: stores rail Z position + plays feedback. | `OnTriggerEnter` |
| `Spawner` / level layout | Enemies activate when the rail reaches their Z (avoid everything running from the start). | distance check vs rail Z |
| `EnemyBase` + 3 enemies | e.g. **Turret** (static, aims & shoots), **Swooper** (flies a curve across screen), **Kamikaze** (charges player). | inheritance, `Vector3.MoveTowards` |
| `Boss` | Phases (HP thresholds), weak points, attack patterns. Rail stops at boss arena. | state machine (enum + switch) |
| `Pickup` | Supply Ring (+shield), Laser Upgrade, + 3rd (e.g. Bomb +1 or extra life). Dropped by some enemies. Spins/bobs. | `Instantiate` on enemy death |
| `Hazard` | Buildings/obstacles: collide → damage + knockback + camera shake. Moving obstacles (doors, rotating bars). | colliders, `OnCollisionEnter` |
| `HUD` | Shield bar, lives, bombs, boss HP bar. | uGUI + TextMeshPro, or UI Toolkit |
| `AudioManager` | Music per scene + SFX one-shots. | `AudioSource.PlayOneShot` |
| `CameraShake`, `HitFlash` | Game feel. | |

Scenes: `MainMenu`, `Level`, `Credits` (+ optional `GameOver`/`Victory` panels inside Level).

Level ≈ 5 min: at rail speed *v*, length = v × 300 s (e.g. 40 u/s → ~12 000 units). Checkpoints at ~1/3 and ~2/3, boss at the end.

---

## 2. Sprints (≈ 1 week each)

| Sprint | Jun Zhe | Daiwei | Done when… |
|---|---|---|---|
| **1. Core flight** | RailMover, PlayerController, PlayerAim, crosshair | Greybox level (cubes) 0–2 min, Trello board | Fly the rail with WASD + mouse aim |
| **2. Combat** | PlayerWeapons (laser + bomb), BarrelRoll | Health, EnemyBase + Turret + Swooper | Shoot and kill 2 enemy types, take damage |
| **3. Game loop** | GameManager, checkpoints, respawn, cheat keys 0–3 | 3rd enemy, pickups (3), hazards/obstacles | Lose life → respawn at checkpoint; game over → menu |
| **4. Boss + UI** | Boss | HUD, MainMenu, Credits, pause/ESC | Full run start → boss → win |
| **5. Art & feel** | Particles (explosions, hits, pickups), camera shake, death/respawn transitions | Models (Blockbench/MagicaVoxel), skybox, music + SFX | No entity "just disappears" |
| **6. Ship it** | Bug hunt, build, demo.avi (OBS + HandBrake ≤150 MB) | Memòria | Zip ready |

**Keep evidence for the report as you go:** Trello screenshots per sprint, weekly meeting notes
(date, decisions, who does what), git log graph, Gantt chart.

---

## 3. Requirements checklist (from the PDF)

### Basic (4 pts) — missing basics = 0
- [ ] Level ~5 min, similar to Starfox 64
- [ ] 2 checkpoints, respawn there if destroyed
- [ ] Decorative elements (buildings…), some collidable → damage
- [ ] Boss at the end, must be defeated to win
- [ ] Shield points; 0 → lose a life → respawn at last checkpoint
- [ ] Rail shooter: movement limited in X/Y around a straight Z rail
- [ ] Mouse aims, WASD moves
- [ ] LMB lasers, RMB bombs
- [ ] 3 enemies with different behaviours
- [ ] 3 pickups dropped by enemies: Supply Ring (+shield), Laser Upgrade (lost on death), +1 own
- [ ] A couple of obstacles to dodge
- [ ] Barrel roll with brief invulnerability
- [ ] HUD: shield, lives, bombs
- [ ] Return to main menu from game
- [ ] 3 screens: menu, play, credits
- [ ] Sound and music
- [ ] Keys 0/1/2/3 → start, checkpoint 1, checkpoint 2, boss

### Interactivity & art (4 pts)
- [ ] Every interaction has sound + visual (particles/animation): hits, pickups, deaths, checkpoints
- [ ] No instant disappear / instant level reset (fade, explosion, respawn animation)
- [ ] Game feel: banking, screen shake, hit-stop, muzzle flash, speed lines
- [ ] Consistent art style everywhere (pick one: low-poly / voxel)

### Memòria (2 pts) — see PDF section 3
- [ ] Title page (FIB + UPC logos), index, page numbers, justified text, ≤1.5 spacing, no blank pages
- [ ] Every figure numbered, captioned, referenced in text
- [ ] Sections: El Joc · Descripció (instructions, window diagram, flow chart, every entity with image) · Metodologia · Conclusions · Bibliografia (with one-line description each — **list every tutorial used**)

### Delivery `NomCognoms.zip`
- [ ] `info.txt` (names) · `memoria.pdf` · `Binari/` (build) · `Projecte/` (**without `Library/`**, unused store assets removed) · `demo.avi` (~1 min, ≤150 MB)

---

## 4. Rules to remember
- Tutorials/courses OK for **learning**, must be cited in the bibliography. **No copied code.**
- Every bug found by the professors costs points → keep a bug list, test with cheat keys.
