using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInventory", menuName = "Character/CharacterInventory")]
public class CharacterInventory : ScriptableObject
{
    public List<CharacterContainer> characters = new List<CharacterContainer>();
    
    // this is the index from the character array that is used for a fight
    public int[] selectedIndexCharacterForCampaign;

    private static CharacterInventory _instance;
    public static CharacterInventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<CharacterInventory>("CharacterInventory");
                if (_instance == null)
                {
                    Debug.LogError("CampaignDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }

    public void VerifyCharacterSelected()
    {
        for (int i = 0; i < selectedIndexCharacterForCampaign.Length; i++)
        {
            if (selectedIndexCharacterForCampaign[i] != -1)
            {
                if (characters[selectedIndexCharacterForCampaign[i]].IsDead())
                {
                    selectedIndexCharacterForCampaign[i] = -1;
                }
            }
        }
    }

    public void AddCharacter(CharacterData character)
    {
        characters.Add(new CharacterContainer(character.id, new List<CharacterPassive>(), 1, true));
    }
}

[Serializable]
public class CharacterContainer
{
    public event Action OnCharacterChanged;

    public void NotifyCharacterChanged()
    {
        OnCharacterChanged?.Invoke();
    }

    public float powerMultiplicator = 1;
    public int characterStar = 1;
    public int characterId;
    public int actualHealth;
    public int actualKi;
    public int selectedUltimateAttack = 0;
    public int maxCharacterStar = 5;
    public List<CharacterPassive> characterPassives = new List<CharacterPassive>();
    public bool isPlayerCharacter = true;
    
    public CharacterContainer(int characterId, List<CharacterPassive> characterPassives, int starNumber, bool isPlayerCharacter, float powerMultiplicator = 1)
    {
        this.characterPassives = characterPassives;
        this.powerMultiplicator = powerMultiplicator;
        this.characterId = characterId;
        this.actualHealth = GetCharacterMaxHealth();

        int additionalStars = Mathf.FloorToInt(powerMultiplicator - 1); // Ex: 3.0 → +2 étoiles
        float extraStarChance = powerMultiplicator - Mathf.Floor(powerMultiplicator);
        starNumber += additionalStars;
        if (UnityEngine.Random.value < extraStarChance && starNumber < 5)
        {
            starNumber++;
        }

        // S'assurer que starNumber reste entre 1 et 5
        this.characterStar = Mathf.Clamp(starNumber, 1, 5);

        this.isPlayerCharacter = isPlayerCharacter;
    }

    public bool CanAddStar(){
        return maxCharacterStar > characterStar;
    }

    public void AddStar(int starNumber = 1){
        if(CanAddStar() == false) return;
        characterStar += starNumber;
        this.actualHealth = this.GetCharacterMaxHealth();
        NotifyCharacterChanged();
        return;
    }
    
    public CharacterContainer(int characterId, int actualHealth, int actualKi, int starNumber, bool isPlayerCharacter)
    {
        this.characterId = characterId;
        this.actualHealth = actualHealth;
        this.actualKi = actualKi;
        this.characterStar = starNumber;
        this.isPlayerCharacter = isPlayerCharacter;
    }
    
    public CharacterData GetCharacterData()
    {
        return CharacterDatabase.Instance.GetCharacterById(characterId);
    }

    public SpecialAttack GetCharacterSpecialAttack()
    {
        return CharacterDatabase.Instance.GetCharacterById(characterId).specialAttackAnimation[selectedUltimateAttack];
    }
    
    public bool IsDead()
    {
        return actualHealth <= 0;
    }

    public CharacterPassive[] GetCharacterPassives()
    {
        return characterPassives.ToArray();
    }

    public int GetAttackDamage()
    {
        int totalAdditionalAttack = 0;

        if(GetCharacterPassives() != null)
        {
            foreach (var passive in GetCharacterPassives())
            {
                if(passive != null)
                {
                    totalAdditionalAttack += passive.AdditionalAttack(this);
                }
            }
        }

        // Synergy bonus part
        var activeBonuses = GetAllActiveBonuses();
        foreach (var bonus in activeBonuses)
        {
            if(bonus is SpecialCharacterBonus specialCharacterBonus){ 
                if(specialCharacterBonus.character == GetCharacterData()){
                    totalAdditionalAttack += bonus.attackBonus; 
                }
            } else {
                totalAdditionalAttack += bonus.attackBonus; 
            }  
        }

        foreach (var effect in activeEffects)
        {
            totalAdditionalAttack += effect.effect.attackBonus;
        }

        totalAdditionalAttack += (characterStar - 1) * 10;

        return Mathf.FloorToInt((GetCharacterData().baseDamage + totalAdditionalAttack)  * powerMultiplicator);
    }

    public string GetName()
    {
        return GetCharacterData().name;
    }
    public int GetArmor()
    {
        return Mathf.FloorToInt(GetCharacterData().baseArmor * powerMultiplicator);
    }
    public int GetSpeed()
    {
        return GetCharacterData().baseSpeed;
    }
    public int GetCharacterMaxHealth()
    {
        int maxHealth = Mathf.FloorToInt(GetCharacterData().maxHealth * powerMultiplicator);
        var activeBonuses = GetAllActiveBonuses();
        foreach (var bonus in activeBonuses)
        {
            if(bonus is SpecialCharacterBonus specialCharacterBonus){ 
                if(specialCharacterBonus.character == GetCharacterData()){
                    maxHealth += bonus.maxHpBonus;   
                }
            } else {
                maxHealth += bonus.maxHpBonus;   
            }
        }
        maxHealth += (characterStar - 1) * 40;
        return maxHealth;
    }
    public int GetCharacterMaxKi()
    {
        int maxKi = GetCharacterData().maxKi;
        maxKi -= (characterStar - 1) * 2;
        return maxKi;
    }
    public float GetAttackSpeed()
    {
        var attackSpeed = GetCharacterData().baseAttackSpeed  * powerMultiplicator;
        var activeBonuses = GetAllActiveBonuses();
        foreach (var bonus in activeBonuses)
        {
            if(bonus is SpecialCharacterBonus specialCharacterBonus){ 
                if(specialCharacterBonus.character == GetCharacterData()){
                    attackSpeed += bonus.attackSpeedBonus;  
                }
            } else {
                attackSpeed += bonus.attackSpeedBonus;  
            }
        }

        foreach (var effect in activeEffects)
        {
            attackSpeed += effect.effect.attackSpeedBonus;
        }
        attackSpeed += (float)((characterStar - 1) * 0.05);
        return attackSpeed;
    }
    public int GetCriticalChance()
    {
        var criticalChance = GetCharacterData().baseCriticalChance;
        var activeBonuses = GetAllActiveBonuses();
        foreach (var bonus in activeBonuses)
        {
            if(bonus is SpecialCharacterBonus specialCharacterBonus){ 
                if(specialCharacterBonus.character == GetCharacterData()){
                    criticalChance += bonus.criticalChanceBonus; 
                }
            } else {
                criticalChance += bonus.criticalChanceBonus; 
            }
        }
        criticalChance += (characterStar - 1) * 2;
        return criticalChance;
    }
    public int GetRange()
    {
        var range = GetCharacterData().baseRange;
        if(range > 1){
            range += characterStar - 1;
        }
        return range;
    }
    public Synergy[] GetSynergies()
    {
        return GetCharacterData().synergies;
    }
    public List<Bonus> GetAllActiveBonuses()
    {
        List<Bonus> bonusList = new List<Bonus>();
        var synergies = GetSynergies();
        if(synergies != null && synergies.Count() > 0){
            foreach (var synergie in synergies)
            {
                foreach (var tierBonus in synergie.GetActiveTierBonuses(isPlayerCharacter))
                {
                    foreach (var bonus in tierBonus.Bonuses)
                    {
                        bonusList.Add(bonus);   
                    }
                }
            }
        }
        return bonusList;
    }
    public int GetCharacterPower()
    {
        return 
            Mathf.FloorToInt(GetAttackDamage() * 3) +
            Mathf.FloorToInt(GetArmor() * 2) +
            GetCriticalChance() +
            Mathf.FloorToInt(GetCharacterMaxHealth() / 25) +
            Mathf.FloorToInt(GetAttackSpeed() * 60);
    }

    // Effect part
    public List<InGameEffect> activeEffects = new List<InGameEffect>();

    public void UpdateEffect(BoardCharacter boardCharacter){
        float deltaTime = Time.deltaTime;
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].UpdateEffect(deltaTime, boardCharacter);

            // Retirer les effets terminés
            if (activeEffects[i].IsEffectFinished())
            {
                Debug.Log($"Effet {activeEffects[i].effect.effectName} terminé pour {GetName()}");
                activeEffects.RemoveAt(i);
                NotifyCharacterChanged();
            }
        }
    }

    public void AddEffect(Effect newEffect)
    {
        InGameEffect existingEffect = activeEffects.Find(effect => effect.effect.effectName == newEffect.effectName);

        if (existingEffect != null)
        {
            // Si l'effet existe déjà, rafraîchir sa durée.
            existingEffect.effectDuration = newEffect.totalEffectDuration;
            Debug.Log($"Effet {newEffect.effectName} rafraîchi pour {GetName()}. Nouvelle durée : {newEffect.totalEffectDuration}s");
        }
        else
        {
            // Sinon, ajouter un nouvel effet.
            activeEffects.Add(new InGameEffect(newEffect));
            Debug.Log($"Effet {newEffect.effectName} ajouté à {GetName()}");
        }
        NotifyCharacterChanged();
    }

    public void AddPassive(CharacterPassive passive)
    {
        characterPassives.Add(passive);
        NotifyCharacterChanged();
    }

    public void HitDamage(int damageAmount, BoardCharacter characterGameInstance)
    {
        actualHealth -= damageAmount;
        if (actualHealth <= 0)
        {
            actualHealth = 0;
        }
        if (IsDead() && characterGameInstance.isDying == false)
        {
            characterGameInstance.Dead(); 
        } else if(IsDead() == false)
        {
            foreach (var passive in GetCharacterPassives())
            {
                passive.GetHit(damageAmount, characterGameInstance);
            }
        }
        NotifyCharacterChanged();
    }

    public void AddKi(int kiAmount)
    {
        actualKi += kiAmount;
        if (actualKi > GetCharacterMaxKi())
        {
            actualKi = GetCharacterMaxKi();
        }
        NotifyCharacterChanged();
    }

    public void Heal(int healAmount)
    {
        actualHealth += healAmount;
        if (actualHealth > GetCharacterMaxHealth())
        {
            actualHealth = GetCharacterMaxHealth();
        }
        NotifyCharacterChanged();
    }
}
