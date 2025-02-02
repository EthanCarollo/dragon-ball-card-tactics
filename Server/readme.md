# Game Patching System

This repository contains a patching system for our game, which allows us to automatically download and apply updates to the game client. The patching system supports both Windows and macOS platforms.

## Patching for macOS

When preparing a patch for macOS, follow these instructions to generate the appropriate `.zip` file:

### Steps to Patch for macOS:

1. **Build Your Game**:
   - Build your game for macOS in Unity or another engine. Ensure that the game is working correctly on macOS.
   
2. **Prepare the Contents Folder**:
   - After building the `.app` bundle, navigate to the `Contents` folder inside the `.app` bundle. For example: `MyGame.app/Contents`.

3. **Compress the Contents Folder**:
   - **Important**: Only compress the contents inside the `Contents` folder, NOT the entire `.app` package. This ensures that only the game’s executable and necessary files are included in the patch.
   
   - On macOS, you can compress the contents using the following command:

     ```bash
     zip -r patch_mac.zip *  # Make sure you're inside the Contents directory
     ```

     This command will create a zip file named `patch_mac.zip` containing all the required files for your patch.

4. **Upload the Patch**:
   - Place the generated `patch_mac.zip` file inside the `public` folder of this repository.
   - The patch file will be served when requested by the patching system.

### Patching for Windows

For Windows, the steps are similar but you'll need to ensure that the entire build is compatible with Windows. Once you’ve packaged the game for Windows, follow these steps:

1. **Build Your Game for Windows**:
   - Use Unity or your game engine to export the game for Windows.

2. **Compress the Windows Build**:
   - Compress the necessary game files into `patch_windows.zip`.

     For example:

     ```bash
     zip -r patch_windows.zip *  # Compress the necessary game files
     ```

3. **Upload the Patch**:
   - Place the generated `patch_windows.zip` file inside the `public` folder of this repository.
   - The patch file will be served when requested by the patching system.

## How the Patching System Works

1. **Version Checking**:
   - The patching system checks the current version of the game using the `version.txt` file. This file should be placed in the `public` directory and contain the current version of the game (e.g., `1.0.0`).
   
2. **Downloading the Patch**:
   - The patching system compares the local version with the remote version found in `version.txt`. If a new version is available, it downloads the appropriate patch file based on the platform:
     - For macOS: The system will download `patch_mac.zip`.
     - For Windows: The system will download `patch_windows.zip`.
   
3. **Applying the Patch**:
   - Once the patch is downloaded, the system extracts the patch file and overwrites the necessary game files in the application directory.
   
4. **Restarting the Game**:
   - After the patch is applied, the game is restarted automatically to apply the changes.

## Notes

- **Platform-Specific Patches**: Ensure you create and upload a separate patch for each platform (Windows and macOS).
- **Permissions on macOS**: After applying a patch on macOS, make sure that the game’s executable has execute permissions. The patching system will attempt to set the correct permissions automatically.
- **Testing**: Always test the patching system before releasing it to users to ensure that updates are applied correctly on all platforms.

---

Happy patching! 🎮
