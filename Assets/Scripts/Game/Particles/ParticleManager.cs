using System;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
        public static ParticleManager Instance;
        public void Awake()
        {
                // TODO : make this particle manager singleton better
                Instance = this;
        }

        public void InstantiateParticle(Vector3 position, GameObject prefab)
        {
                Instantiate(prefab, position, Quaternion.identity);
        }

        public void ShowAttackNumber(BoardCharacter boardCharacter, int number)
        {
                ShowNumber(boardCharacter, number, Color.white);
        }

        public void ShowHealNumber(BoardCharacter boardCharacter, int number)
        {
                ShowNumber(boardCharacter, number, Color.green);
        }

        public void ShowNumber(BoardCharacter boardCharacter, int number, Color color)
        {
                try
                {
                        Sprite[] particles = SpriteDatabase.Instance.numbers;
                        Transform boardTransform = boardCharacter.gameObject.transform;
                        string numberString = number.ToString();
                        float xOffset = 0f;

                        float randomX = UnityEngine.Random.Range(-0.2f, 0.2f);
                        float randomY = UnityEngine.Random.Range(0f, 1.5f);
                        var position = boardTransform.position + new Vector3(randomX, randomY, 0);

                        foreach (char digitChar in numberString)
                        {
                                int digit = int.Parse(digitChar.ToString());
                                GameObject digitObject = new GameObject("DamageDigit");
                                SpriteRenderer spriteRenderer = digitObject.AddComponent<SpriteRenderer>();
                                spriteRenderer.color = color;
                                spriteRenderer.sortingOrder = 10;
                                spriteRenderer.sprite = particles[digit];

                                // Scale down the size of the number
                                digitObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                                // Randomize position within the given range
                                digitObject.transform.position = position + new Vector3(xOffset, 0, 0);
                                digitObject.transform.SetParent(boardTransform);

                                // Tween settings
                                LeanTween.alpha(digitObject.gameObject, 0f, 0.5f).setDelay(0.6f);
                                LeanTween.moveX(digitObject.gameObject, digitObject.transform.position.x + 0.4f, 1.2f).setOnComplete(
                                () => { Destroy(digitObject.gameObject); }).setEase(LeanTweenType.easeOutCirc);

                                xOffset += 0.35f; 
                        }
                }
                catch (Exception exception)
                {
                        Debug.LogError("Got an error in ParticleManager, " + exception.ToString());
                }
                
        }

}