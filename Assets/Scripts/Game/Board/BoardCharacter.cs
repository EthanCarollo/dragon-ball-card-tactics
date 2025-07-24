using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BoardCharacter : BoardObject
{
    public CharacterContainer character;
    public BoardCharacterState state;
    public Vector2 direction;
    // If the nextPosition is to negative infinity, it just don't have a next position at all
    public Vector2Int nextPosition = new Vector2Int(-1, -1);
    public bool isDying = false;

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
        
        if (!character.isPlayerCharacter)
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
        this.character = character;
        state = new DefaultCharacterState(this);

        if (isInstantiated) this.SetCharacterSlider();
        else Debug.LogWarning("Character Instantiated but not gameobject, cannot set slider.");
        
    }

    public void SetupCharacter(CharacterData character)
    {
        this.character.characterId = character.id;
        this.character.selectedUltimateAttack = 0;
        state = new DefaultCharacterState(this);
        this.SetCharacterSlider();
        
        // For refreshing the display of synergies etc..
        BoardGameUiManager.Instance.RefreshUI();
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
        SpriteRenderer renderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        renderer.material = new Material(ShadersDatabase.Instance.spriteMaterial);
    }

    public override void Update()
    {
        try
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = direction == Vector2.left;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        foreach (var passive in character.GetCharacterPassives())
        {
            passive.UpdatePassive(this);
        }

        character.UpdateEffect(this);
        state.Update();
    }

    public void Attack(int multiplicator, Particle particle, BoardCharacter target)
    {
        if (character.IsDead() || GetCharacterTarget() == null)
        {
            return;
        }
        if (particle != null && GetCharacterTarget().gameObject != null) {
            try {
                particle.StartParticle(GetCharacterTarget().gameObject.transform.position);
            } catch (Exception error){
                Debug.LogWarning("Error on starting particle," + error);
            }
        }

        foreach (var passive in character.GetCharacterPassives())
        {
            passive.HitCharacter(this, target);
        }

        if (target != null && target.character.IsDead() == false)
        {
            target.HitDamage(character.GetAttackDamage() * multiplicator);
            if(target.character.IsDead() == true)
            {
                foreach (var passive in character.GetCharacterPassives())
                {
                    passive.KilledAnEnemy(this, target);
                }
            }
        }
    }

    public void Dead()
    {
        state.Dead();
    }

    public void HitDamage(int damageAmount)
    {
        float damageReceived = 100 / (100 + character.GetArmor());
        int damageAmountCalculated = Mathf.FloorToInt(damageAmount * damageReceived);

        character.HitDamage(damageAmountCalculated, this);
        ParticleManager.Instance.ShowAttackNumber(this, damageAmountCalculated);
        
        SetCharacterSlider();
    }

    public void AddKi(int kiAmount)
    {
        this.character.AddKi(kiAmount);
        SetCharacterSlider();
    }

    public void Heal(int healAmount)
    {
        this.character.Heal(healAmount);
        SetCharacterSlider();
        ParticleManager.Instance.ShowHealNumber(this, healAmount);
    }

    public void SetCharacterSlider()
    {
        if (gameObject == null)
        {
            Debug.LogWarning("Gameobject isn't set :/ (called too early)");
            return;
        }
        var firstChild = gameObject.transform.GetChild(0);
        if (firstChild == null)
        {
            Debug.LogWarning("Character has no children :/ (called too early)");
            return;
        }
        var charPrefabScript = firstChild.GetComponent<CharacterPrefabScript>();
        if (charPrefabScript != null)
        {
            charPrefabScript.kiSlider.maxValue = this.character.GetCharacterMaxKi();
            charPrefabScript.kiSlider.value = this.character.actualKi;
            charPrefabScript.healthSlider.maxValue = this.character.GetCharacterMaxHealth();
            charPrefabScript.healthSlider.value = this.character.actualHealth;

            string effectContent = "";
            foreach (InGameEffect effect in character.activeEffects)
            {
                if(effectContent == "") {
                    effectContent = effect.effect.effectName;
                } else {
                    effectContent += "\n" + effect.effect.effectName;
                }
            }
            charPrefabScript.effectText.text = effectContent;
            foreach (Transform child in charPrefabScript.starContainer)
            {
                MonoBehaviour.Destroy(child.gameObject);
            }
            for (int i = 0; i < character.characterStar; i++)
            {
                var characterStar = new GameObject("CharacterStar");
                characterStar.AddComponent<Image>().sprite = charPrefabScript.starImage;
                characterStar.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.2f);
                characterStar.transform.SetParent(charPrefabScript.starContainer);
            }
        }
        else
        {
            Debug.LogWarning("No character prefab script on this object. (maybe it has been called too early ?)");
        }
        
        
        
    }

    public BoardAnimation actualAnimation;

    public bool isAnimating() { return actualAnimation != null; }
    
    public void PlayAnimation(BoardAnimation animation)
    {
        if (isInstantiated == false)
        {
            Debug.LogWarning("Cannot play animation if character is not isntantiated");
            return;
        }
        try
        {
            var characterScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
            characterScript.StopAllCoroutines();
            if (actualAnimation != null) actualAnimation.EndAnimation(this);
            characterScript.StartCoroutine(animation.PlayAnimationCoroutine(this));
        } 
        catch (Exception error)
        {
            Debug.LogError("Cannot run animation on character : " + character.GetCharacterData().characterName + "  " + error);
        }
    }

    public void PlayAnimation(BoardAnimation animation, Action onAnimationComplete = null)
    {
        if (isInstantiated == false)
        {
            Debug.LogWarning("Cannot play animation if character is not isntantiated");
            return;
        }
        
        try
        {
            var characterScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
            characterScript.StopAllCoroutines();
            if (actualAnimation != null) actualAnimation.EndAnimation(this);
            characterScript.StartCoroutine(PlayAnimationWithCallback(animation, onAnimationComplete));
        } 
        catch (Exception error)
        {
            Debug.LogError("Cannot run animation on character : " + character.GetCharacterData().characterName + "  " + error);
        }
    }

    public bool PlayAnimationIfNotRunning(BoardAnimation animation)
    {
        if (isInstantiated == false)
        {
            Debug.LogWarning("Cannot play animation if character is not isntantiated");
            return false;
        }
        
        try
        {
            if (!isAnimating())
            {
                gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().StartCoroutine(animation.PlayAnimationCoroutine(this));
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
            Debug.LogError("Cannot run animation on character : " + character.GetCharacterData().characterName);
            return false;
        }
    }

    private IEnumerator PlayAnimationWithCallback(BoardAnimation animation, Action onAnimationComplete)
    {
        yield return gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().StartCoroutine(animation.PlayAnimationCoroutine(this));
        onAnimationComplete?.Invoke();
    }

    public void LaunchKikoha() 
    {
        state.LaunchKikoha();
    }

    public void UpdateKikohaAdvancement(int percentage){
        state.UpdateKikohaAdvancement(percentage);
    }

    public int GetKikohaAdvancement(){
        return state.GetKikohaAdvancement();
    }

    public override BoardObject Clone()
    {
        // Reset the character container and clone it to a new BoardObject, but
        // we have the same gameobject than this boardcharacter, that can cause
        // some bugs
        var newCharacter = new CharacterContainer(character.characterId, character.characterPassives, character.characterStar, character.isPlayerCharacter, character.powerMultiplicator);
        BoardCharacter clonedCharacter = new BoardCharacter(newCharacter);
        clonedCharacter.SetGameObject(this.gameObject);
        clonedCharacter.SetBoard(this.board); 
        return clonedCharacter;
    }
}
