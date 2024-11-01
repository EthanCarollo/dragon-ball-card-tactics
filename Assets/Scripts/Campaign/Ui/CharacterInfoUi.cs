using System;
using UnityEngine;

public class CharacterInfoUi : MonoBehaviour
{
    public GameObject characterInfoButtonPrefab;

    public void Start()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (CharacterContainer character in GameManager.Instance.characterInventory.characters)
        {
            var go = Instantiate(characterInfoButtonPrefab, this.transform);
            go.GetComponent<CharacterInfoButton>().Setup(character);
        }
    }
}
