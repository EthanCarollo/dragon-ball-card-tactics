using UnityEngine;
using UnityEditor;

public class CharacterDataUpdater : EditorWindow
{
    [MenuItem("Tools/Update Character Stats")]
    public static void UpdateAllCharacterStats()
    {
        Debug.Log("Starting character stats update...");
        
        UpdateCharacter("Moro", baseDamage: 175, maxHealth: 2000);
        UpdateCharacter("Piccolo", baseDamage: 80, maxHealth: 550);
        UpdateCharacter("Raditz", baseDamage: 75, maxHealth: 450);
        UpdateCharacter("Recoom", baseDamage: 90, maxHealth: 500, maxKi: 2);
        UpdateCharacter("Super Buu", baseDamage: 100, maxHealth: 800);
        UpdateCharacter("Toppo (God Power)", baseDamage: 100, maxHealth: 3500);
        UpdateCharacter("Trunks (Future SSJ Rage)", baseDamage: 95, maxHealth: 750);
        UpdateCharacter("Trunks (Future)", baseDamage: 75, maxHealth: 500);
        UpdateCharacter("Trunks (Mirai SSJ2)", baseDamage: 100, maxHealth: 675);
        UpdateCharacter("Trunks (Mirai)", baseDamage: 45, maxHealth: 450);
        UpdateCharacter("Trunks (Xeno SSG)", baseDamage: 110, maxHealth: 900, maxKi: 4);
        UpdateCharacter("Trunks (Xeno SSJ)", baseDamage: 85, maxHealth: 700, maxKi: 1);
        UpdateCharacter("Trunks (Xeno SSJ3)", baseDamage: 100, maxHealth: 1100, maxKi: 2);
        UpdateCharacter("Trunks (Xeno)", baseDamage: 65, maxHealth: 400, maxKi: 1);
        UpdateCharacter("Trunks (Young)", baseDamage: 50, maxHealth: 200, maxKi: 1);
        UpdateCharacter("Turles", baseDamage: 60, maxHealth: 320);
        UpdateCharacter("Turles (Metal)", baseDamage: 70, maxHealth: 450, maxKi: 2);
        UpdateCharacter("Turles (SSJ)", baseDamage: 80, maxHealth: 600);
        UpdateCharacter("Vegeta", baseDamage: 35, maxHealth: 350);
        UpdateCharacter("Vegeta (Namek)", baseDamage: 50, maxHealth: 225);
        UpdateCharacter("Vegeta (Buu Saga)", baseDamage: 75, maxHealth: 575, maxKi: 2);
        UpdateCharacter("Vegeta (Majin)", baseDamage: 95, maxHealth: 595, maxKi: 3);
        UpdateCharacter("Vegeta (SSB)", baseDamage: 105, maxHealth: 1200);
        UpdateCharacter("Vegeta (SSJ)", baseDamage: 70, maxHealth: 455);
        UpdateCharacter("Vegeta (Ultra Ego)", baseDamage: 120, maxHealth: 1600);
        UpdateCharacter("Vegito", baseDamage: 80, maxHealth: 875, maxKi: 4);
        UpdateCharacter("Whis", baseDamage: 60, maxHealth: 1000, maxKi: 4);
        UpdateCharacter("Yamcha", baseDamage: 20, maxHealth: 200);
        UpdateCharacter("Zamasu", baseDamage: 100, maxHealth: 290, maxKi: 3);
        UpdateCharacter("Zamasu (Fusion)", baseDamage: 115, maxHealth: 800, maxKi: 5);
        UpdateCharacter("Broly", baseDamage: 75, maxHealth: 340);
        UpdateCharacter("Goku (Ikari)", baseDamage: 235, maxHealth: 975, maxKi: 5);
        UpdateCharacter("Cooler (Metal)", baseDamage: 89, maxHealth: 780, maxKi: 3, folder: "Cooler (Metal)", name: "Cooler (Metal)");
        
        UpdateCharacter("Vermouth", maxKi: 6);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("=== Character stats update completed! ===");
    }

    private static void UpdateCharacter(string characterName, int? baseDamage = null, int? maxHealth = null, int? maxKi = null, string folder = null, string name = null)
    {
        if (folder == null)
        {
            folder = characterName;
        }
        
        if (name == null)
        {
            name = characterName;
        }
        
        string path = $"Assets/Resources/Character/Character/{folder}/{name}.asset";
        CharacterData character = AssetDatabase.LoadAssetAtPath<CharacterData>(path);
        
        if (character != null)
        {
            if (baseDamage.HasValue)
            {
                character.baseDamage = baseDamage.Value;
            }
            
            if (maxHealth.HasValue)
            {
                character.maxHealth = maxHealth.Value;
            }
            
            if (maxKi.HasValue)
            {
                character.maxKi = maxKi.Value;
            }
            
            EditorUtility.SetDirty(character);
            Debug.Log($"✓ Updated {characterName}: AD={character.baseDamage}, HP={character.maxHealth}, Mana={character.maxKi}");
        }
        else
        {
            Debug.LogWarning($"✗ Character not found at path: {path}");
        }
    }
}
