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
        SetupCharacter(character);
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
        this.SetCharacterSlider();
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

        if (target != null)
        {
            target.HitDamage(character.GetAttackDamage() * multiplicator);
        }
    }

    public void Dead()
    {
        state.Dead();
    }

    public void HitDamage(int damageAmount)
    {
        this.character.HitDamage(damageAmount, this);
        ParticleManager.Instance.ShowAttackNumber(this, damageAmount);
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
        try
        {
            var charPrefabScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
            charPrefabScript.kiSlider.maxValue = this.character.GetCharacterMaxKi();
            charPrefabScript.kiSlider.value = this.character.actualKi;
            charPrefabScript.healthSlider.maxValue = this.character.GetCharacterMaxHealth();
            charPrefabScript.healthSlider.value = this.character.actualHealth;

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
        catch (Exception e)
        {
            Debug.Log("Cannot set character slider, charPrefabScript isn't probably set, its too early bro, " + e.ToString());
        }
        
    }

    public BoardAnimation actualAnimation;

    public bool isAnimating() { return actualAnimation != null; }
    
    public void PlayAnimation(BoardAnimation animation)
    {
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
        // Reset the character container
        var newCharacter = new CharacterContainer(character.characterId, character.characterPassives, character.characterStar, character.isPlayerCharacter, character.powerMultiplicator);
        BoardCharacter clonedCharacter = new BoardCharacter(newCharacter);
        clonedCharacter.SetGameObject(this.gameObject);
        clonedCharacter.SetBoard(this.board); 
        return clonedCharacter;
    }
}
