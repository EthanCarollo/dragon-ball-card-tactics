# Project structure

## Runtime layout

```text
Assets/
├── Scenes/              # Runtime scenes included in Build Settings
├── Scripts/
│   ├── Data/             # ScriptableObject databases and persistence
│   ├── Game/             # Board, cards, characters, combat and game UI
│   ├── MainMenu/         # Main menu and wiki UI
│   ├── Scenes/           # Scene-loading components
│   └── Utils/            # Small cross-domain utilities
├── Resources/            # Runtime bootstrap databases and content catalogs
├── Prefabs/              # Reusable UI, card, board and history prefabs
├── Art/                  # Project-owned visual and audio source assets
├── Tests/                # EditMode gameplay and progression tests
├── Editor/               # Unity Editor-only tools and validators
└── ThirdParty/           # Dependencies that are not installed as packages

Packages/
├── com.coffee.ui-effect/ # Embedded UIEffect package used by the game
└── com.bezi.sidekick/    # Embedded editor integration
```

The two runtime scenes are `Assets/Scenes/MainMenuScene.unity` and
`Assets/Scenes/FightScene.unity`. The authoritative list is the Unity Build
Settings, not a hard-coded list in an editor script.

## Data ownership

- `CardDatabase`, `CharacterDatabase`, `FightDatabase` and the other runtime
  databases are the entry points for game content.
- Character, card and fight assets may keep display names with spaces or
  parentheses. Code should use database IDs or object references for identity,
  never a display-name-derived path.
- Any database refresh or ID normalization must be followed by the serialized
  reference validator in `Tools/Project`.

## Naming conventions

- C# types and public methods: `PascalCase`.
- Private fields and locals: `camelCase`.
- Unity-facing UI folders use `UI`; new folders should not introduce `Ui`.
- New technical asset identifiers should use stable lowercase slugs. Display
  names remain serialized data and are free to be localized or reformatted.
- Avoid `Test`, `New`, `Copy`, `Old` and `Default` in production asset names.
  Existing files with those names must be reference-audited before removal.

## Dependency rules

- Project gameplay code belongs under `Assets/Scripts`; editor-only code belongs
  under `Assets/Editor` or an editor-only assembly.
- Package-owned files stay under `Packages` and project-owned UIEffect samples
  stay under `Assets/ThirdParty/Samples/UIEffect`.
- Do not copy package runtime scripts into `Assets/Scripts`.
- New dependencies should be added to `Packages/manifest.json` when a package
  version exists; `Assets/ThirdParty` is for vendored libraries without a
  suitable package installation.

## Safe maintenance workflow

1. Change one structural concern at a time.
2. Run `git diff --check`.
3. Open Unity in batch mode to force import and compilation.
4. Run the EditMode tests.
5. Run `Tools/Project/Validate Serialized References` and
   `Tools/Project/Validate Runtime Data`.
6. Restore any editor-generated changes that are not intentional, especially
   generated TextMesh Pro assets.
7. Commit with an Angular/Conventional Commit message.

## Deferred migrations

The following are intentionally not automatic refactors:

- Moving the large `Resources` catalog to Addressables.
- Introducing runtime assembly definitions while LeanTween remains in the
  default Unity assembly.
- Removing `Test`/`New`/`Default` assets without a complete reference audit.

These changes affect asset loading or Unity assembly boundaries and should be
done as isolated migrations with a dedicated verification pass.
