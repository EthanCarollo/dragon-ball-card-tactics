using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BoardCharacter : BoardObject
{
    public CharacterContainer character;
    public BoardCharacterState state;
    public Vector2 direction;
    // If the nextPosition is to negative infinity, it just don't have a next position at all
    public Vector2Int nextPosition = new Vector2Int(-1, -1);
    public bool isDying = false;

    private int displayedStarCount = -1;
    private string displayedEffectContent;
    [NonSerialized] private CharacterPrefabScript characterPrefabScript;

    public BoardCharacter GetCharacterTarget()
    {
        if(state is AttackingCharacterState fightState){
            return fightState.characterTarget;
        }
        return null;
    }

    public BoardCharacter(CharacterContainer character)
    {
        this.character = character;
        state = new DefaultCharacterState(this);
        
        if (character == null || !character.isPlayerCharacter)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }
    }

    public void SetupCharacter(CharacterContainer character)
    {
        if (character == null)
        {
            Debug.LogWarning("Cannot setup a board character with null character data.");
            return;
        }

        this.character = character;
        state = new DefaultCharacterState(this);

        if (isInstantiated) this.SetCharacterSlider();
        else Debug.LogWarning("Character Instantiated but not gameobject, cannot set slider.");
        
    }

    public void SetupCharacter(CharacterData character)
    {
        if (this.character == null || character == null)
        {
            Debug.LogWarning("Cannot change a board character from null character data.");
            return;
        }

        this.character.characterId = character.id;
        this.character.selectedUltimateAttack = 0;
        state = new DefaultCharacterState(this);
        this.SetCharacterSlider();
        
        // For refreshing the display of synergies etc..
        BoardGameUiManager.Instance?.RefreshUI();
    }

    public void UpdateState(BoardCharacterState newState)
    {
        state = newState;
    }

    public override void UpdateUi()
    {
        
    }

    public void ResetCharacterShader()
    {
        var prefabScript = GetCharacterPrefabScript();
        if (prefabScript?.spriteRenderer == null || ShadersDatabase.Instance == null ||
            ShadersDatabase.Instance.spriteMaterial == null)
        {
            return;
        }

        prefabScript.spriteRenderer.material = new Material(ShadersDatabase.Instance.spriteMaterial);
    }

    public override void Update()
    {
        var prefabScript = GetCharacterPrefabScript();
        if (prefabScript?.spriteRenderer != null)
        {
            prefabScript.spriteRenderer.flipX = direction == Vector2.left;
        }

        if (character == null)
        {
            return;
        }

        foreach (var passive in character.GetCharacterPassives())
        {
            passive?.UpdatePassive(this);
        }

        character.UpdateEffect(this);
        state?.Update();
    }

    public void Attack(int multiplicator, Particle particle, BoardCharacter target)
    {
        if (character == null || character.IsDead() || target?.character == null || target.character.IsDead())
        {
            return;
        }
        if (particle != null && target.gameObject != null) {
            try {
                particle.StartParticle(target.gameObject.transform.position);
            } catch (Exception error){
                Debug.LogWarning("Error on starting particle," + error);
            }
        }

        foreach (var passive in character.GetCharacterPassives())
        {
            passive?.HitCharacter(this, target);
        }

        target.HitDamage(character.GetAttackDamage() * multiplicator);
        if(target.character.IsDead() == true)
        {
            foreach (var passive in character.GetCharacterPassives())
            {
                passive?.KilledAnEnemy(this, target);
            }
        }
    }

    public void Dead()
    {
        ReleaseBoardPosition();
        state?.Dead();
    }

    public void HitDamage(int damageAmount)
    {
        if (character == null)
        {
            return;
        }

        float damageReceived = 100 / (100 + character.GetArmor());
        int damageAmountCalculated = Mathf.FloorToInt(damageAmount * damageReceived);

        character.HitDamage(damageAmountCalculated, this);
        ParticleManager.Instance?.ShowAttackNumber(this, damageAmountCalculated);
        
        SetCharacterSlider();
    }

    public void AddKi(int kiAmount)
    {
        if (character == null)
        {
            return;
        }

        character.AddKi(kiAmount);
        SetCharacterSlider();
    }

    public void Heal(int healAmount)
    {
        if (character == null)
        {
            return;
        }

        character.Heal(healAmount);
        SetCharacterSlider();
        ParticleManager.Instance?.ShowHealNumber(this, healAmount);
    }

    public void SetCharacterSlider()
    {
        if (character == null)
        {
            return;
        }

        var prefabScript = GetCharacterPrefabScript();
        if (prefabScript == null)
        {
            return;
        }

        if (prefabScript.kiSlider != null)
        {
            prefabScript.kiSlider.maxValue = character.GetCharacterMaxKi();
            prefabScript.kiSlider.value = character.actualKi;
        }
        if (prefabScript.healthSlider != null)
        {
            prefabScript.healthSlider.maxValue = character.GetCharacterMaxHealth();
            prefabScript.healthSlider.value = character.actualHealth;
        }

        var effectBuilder = new StringBuilder();
        foreach (InGameEffect effect in character.activeEffects ?? new List<InGameEffect>())
        {
            if (effect?.effect == null)
            {
                continue;
            }

            if (effectBuilder.Length > 0)
            {
                effectBuilder.Append('\n');
            }
            effectBuilder.Append(effect.effect.effectName);
        }

        string effectContent = effectBuilder.ToString();
        if (displayedEffectContent != effectContent)
        {
            if (prefabScript.effectText != null)
            {
                prefabScript.effectText.text = effectContent;
            }
            displayedEffectContent = effectContent;
        }

        if (displayedStarCount == character.characterStar || prefabScript.starContainer == null)
        {
            return;
        }

        foreach (Transform child in prefabScript.starContainer)
        {
            MonoBehaviour.Destroy(child.gameObject);
        }

        if (prefabScript.starImage != null)
        {
            for (int i = 0; i < character.characterStar; i++)
            {
                var characterStar = new GameObject("CharacterStar");
                characterStar.AddComponent<Image>().sprite = prefabScript.starImage;
                characterStar.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.2f);
                characterStar.transform.SetParent(prefabScript.starContainer);
            }
        }

        displayedStarCount = character.characterStar;
    }

    public BoardAnimation actualAnimation;

    public bool isAnimating() { return actualAnimation != null; }
    
    public void PlayAnimation(BoardAnimation animation)
    {
        if (!isInstantiated || animation == null)
        {
            return;
        }
        try
        {
            var characterScript = GetCharacterPrefabScript();
            if (characterScript == null)
            {
                return;
            }

            characterScript.StopAllCoroutines();
            if (actualAnimation != null) actualAnimation.EndAnimation(this);
            characterScript.StartCoroutine(animation.PlayAnimationCoroutine(this));
        } 
        catch (Exception error)
        {
            Debug.LogError("Cannot run animation on character : " + GetCharacterDisplayName() + "  " + error);
        }
    }

    public bool PlayAnimation(BoardAnimation animation, Action onAnimationComplete = null)
    {
        if (!isInstantiated || animation == null)
        {
            return false;
        }
        
        try
        {
            var characterScript = GetCharacterPrefabScript();
            if (characterScript == null)
            {
                return false;
            }

            characterScript.StopAllCoroutines();
            if (actualAnimation != null) actualAnimation.EndAnimation(this);
            characterScript.StartCoroutine(PlayAnimationWithCallback(animation, onAnimationComplete));
            return true;
        } 
        catch (Exception error)
        {
            Debug.LogError("Cannot run animation on character : " + GetCharacterDisplayName() + "  " + error);
            return false;
        }
    }

    public bool PlayAnimationIfNotRunning(BoardAnimation animation)
    {
        if (!isInstantiated || animation == null)
        {
            return false;
        }
        
        try
        {
            if (!isAnimating())
            {
                var characterScript = GetCharacterPrefabScript();
                if (characterScript == null)
                {
                    return false;
                }

                characterScript.StartCoroutine(animation.PlayAnimationCoroutine(this));
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception error)
        {
            Debug.LogError(error);
            Debug.LogError("Cannot run animation on character : " + GetCharacterDisplayName());
            return false;
        }
    }

    private IEnumerator PlayAnimationWithCallback(BoardAnimation animation, Action onAnimationComplete)
    {
        var characterScript = GetCharacterPrefabScript();
        if (characterScript == null)
        {
            yield break;
        }

        yield return characterScript.StartCoroutine(animation.PlayAnimationCoroutine(this));
        onAnimationComplete?.Invoke();
    }

    public void LaunchKikoha() 
    {
        state?.LaunchKikoha();
    }

    public void UpdateKikohaAdvancement(int percentage){
        state?.UpdateKikohaAdvancement(percentage);
    }

    public int GetKikohaAdvancement(){
        return state == null ? 0 : state.GetKikohaAdvancement();
    }

    public override BoardObject Clone()
    {
        if (character == null)
        {
            return null;
        }

        return new BoardCharacter(character.Clone())
        {
            direction = direction
        };
    }

    public void ResetUiCache()
    {
        displayedStarCount = -1;
        displayedEffectContent = null;
    }

    public void SetCharacterPrefabScript(CharacterPrefabScript prefabScript)
    {
        characterPrefabScript = prefabScript;
    }

    public CharacterPrefabScript GetCharacterPrefabScript()
    {
        if (characterPrefabScript != null)
        {
            return characterPrefabScript;
        }

        if (gameObject == null || gameObject.transform.childCount == 0)
        {
            return null;
        }

        characterPrefabScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
        return characterPrefabScript;
    }

    private string GetCharacterDisplayName()
    {
        return character?.GetCharacterData()?.characterName ?? "<missing>";
    }
}
