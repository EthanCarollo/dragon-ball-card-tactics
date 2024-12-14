using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BoardCharacter : BoardObject
{
    public CharacterContainer character;
    public BoardCharacterState state;
    public bool isPlayerCharacter;
    public Vector2 direction;
    // If the nextPosition is to negative infinity, it just don't have a next position at all
    public Vector2Int nextPosition = new Vector2Int(-1, -1);
    public bool isAnimating = false;

    public bool isDying = false;
    public List<Effect> activeEffects = new List<Effect>();

    public BoardCharacter GetCharacterTarget()
    {
        if(state is AttackingCharacterState fightState){
            return fightState.characterTarget;
        }
        return null;
    }

    public BoardCharacter(CharacterContainer character, bool isPlayerCharacter)
    {
        SetupCharacter(character);
        this.isPlayerCharacter = isPlayerCharacter;
        if (!isPlayerCharacter)
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
        this.SetCharacterSlider();
    }

    public void SetupCharacter(CharacterData character)
    {
        this.character.characterId = character.id;
        this.character.selectedUltimateAttack = 0;
        state = new DefaultCharacterState(this);
        this.SetCharacterSlider();
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
        float deltaTime = Time.deltaTime;

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

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].UpdateEffect(deltaTime, this);

            // Retirer les effets terminés
            if (activeEffects[i].IsEffectFinished())
            {
                Debug.Log($"Effet {activeEffects[i].effectName} terminé pour {character.GetName()}");
                activeEffects.RemoveAt(i);
            }
        }
        state.Update();
    }

    public void Attack(int multiplicator, Particle particle, BoardCharacter target)
    {
        if (character.IsDead() || GetCharacterTarget() == null)
        {
            return;
        }
        if (particle != null) {
            particle.StartParticle(GetCharacterTarget().gameObject.transform.position);
        }

        foreach (var passive in character.GetCharacterPassives())
        {
            passive.HitCharacter(this, target);
        }

        if (target != null)
        {
            target.HitDamage(character.GetAttackDamage() * multiplicator);
        }
    }

    public bool CanKikoha()
    {
        return state.CanKikoha();
    }

    public void EndKikoha()
    {
        state.EndKikoha();
    }

    public void Dead()
    {
        state.Dead();
    }

    public void Transform(TransformAnimation animation)
    {
        state.Transform(animation);
    }

    public void ResetGameObjectPosition()
    {
        
    }

    public void HitDamage(int damageAmount)
    {
        this.character.actualHealth -= damageAmount;
        ParticleManager.Instance.ShowAttackNumber(this, damageAmount);
        if (this.character.actualHealth <= 0)
        {
            this.character.actualHealth = 0;
        }
        if (this.character.IsDead() && isDying == false)
        {
            this.Dead(); 
        }
        else if(this.character.IsDead() == false)
        {
            foreach (var passive in character.GetCharacterPassives())
            {
                passive.GetHit(damageAmount, this);
            }
        }
        SetCharacterSlider();
    }

    public void AddEffect(Effect newEffect)
    {
        Effect existingEffect = activeEffects.Find(effect => effect.effectName == newEffect.effectName);

        if (existingEffect != null)
        {
            // Si l'effet existe déjà, rafraîchir sa durée.
            existingEffect.effectDuration = newEffect.effectDuration;
            Debug.Log($"Effet {newEffect.effectName} rafraîchi pour {character.GetName()}. Nouvelle durée : {newEffect.effectDuration}s");
        }
        else
        {
            // Sinon, ajouter un nouvel effet.
            activeEffects.Add(newEffect.Clone());
            Debug.Log($"Effet {newEffect.effectName} ajouté à {character.GetName()}");
        }
    }

    public void AddKi(int kiAmount)
    {
        this.character.actualKi += kiAmount;
        if (this.character.actualKi > this.character.GetCharacterMaxKi())
        {
            this.character.actualKi = this.character.GetCharacterMaxKi();
        }
    }

    public void Heal(int healAmount)
    {
        ParticleManager.Instance.ShowHealNumber(this, healAmount);
        this.character.actualHealth += healAmount;
        if (this.character.actualHealth > this.character.GetCharacterMaxHealth())
        {
            this.character.actualHealth = this.character.GetCharacterMaxHealth();
        }
    }

    public void SetCharacterSlider()
    {
        try
        {
            var charPrefabScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
            charPrefabScript.kiSlider.maxValue = this.character.GetCharacterMaxKi();
            charPrefabScript.kiSlider.value = this.character.actualKi;
            charPrefabScript.healthSlider.maxValue = this.character.GetCharacterMaxHealth();
            charPrefabScript.healthSlider.value = this.character.actualHealth;
        }
        catch (Exception e)
        {
            Debug.Log("Cannot set character slider, charPrefabScript isn't probably set, its too early bro, " + e.ToString());
        }
        
    }

    public void PlayAnimation(BoardAnimation animation)
    {
        var characterScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
        characterScript.StopAllCoroutines();
        characterScript.StartCoroutine(animation.PlayAnimationCoroutine(this));
    }

    public void PlayAnimation(BoardAnimation animation, Action onAnimationComplete = null)
    {
        var characterScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
        characterScript.StopAllCoroutines();
        characterScript.StartCoroutine(PlayAnimationWithCallback(animation, onAnimationComplete));
    }

    private IEnumerator PlayAnimationWithCallback(BoardAnimation animation, Action onAnimationComplete)
    {
        yield return gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().StartCoroutine(animation.PlayAnimationCoroutine(this));
        onAnimationComplete?.Invoke();
    }

    public bool PlayAnimationIfNotRunning(BoardAnimation animation)
    {
        if (!isAnimating)
        {
            gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().StartCoroutine(animation.PlayAnimationCoroutine(this));
            return true;
        }
        else
        {
            return false;
        }
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
        // Reset the character container
        var newCharacter = new CharacterContainer(character.characterId, character.characterPassives, character.powerMultiplicator);
        BoardCharacter clonedCharacter = new BoardCharacter(newCharacter, isPlayerCharacter);
        clonedCharacter.SetGameObject(this.gameObject);
        clonedCharacter.SetBoard(this.board); 
        return clonedCharacter;
    }
}
