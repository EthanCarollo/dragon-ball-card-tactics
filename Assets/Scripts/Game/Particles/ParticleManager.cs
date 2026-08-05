using System;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
        public static ParticleManager Instance;
        public void Awake()
        {
                if (Instance != null && Instance != this)
                {
                        Destroy(gameObject);
                        return;
                }

                Instance = this;
        }

        private void OnDestroy()
        {
                if (Instance == this)
                {
                        Instance = null;
                }
        }

        public void InstantiateParticle(Vector3 position, GameObject prefab)
        {
                if (prefab != null)
                {
                        Instantiate(prefab, position, Quaternion.identity);
                }
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
                if (boardCharacter == null || boardCharacter.gameObject == null || SpriteDatabase.Instance == null ||
                    SpriteDatabase.Instance.numbers == null || SpriteDatabase.Instance.numbers.Length < 10)
                {
                        return;
                }

                Sprite[] particles = SpriteDatabase.Instance.numbers;
                Transform boardTransform = boardCharacter.gameObject.transform;
                string numberString = Mathf.Abs(number).ToString();
                float xOffset = 0f;

                float randomX = UnityEngine.Random.Range(-0.2f, 0.2f);
                float randomY = UnityEngine.Random.Range(0f, 1.5f);
                var position = boardTransform.position + new Vector3(randomX, randomY, 0);

                foreach (char digitChar in numberString)
                {
                        int digit = digitChar - '0';
                        if (digit < 0 || digit >= particles.Length || particles[digit] == null)
                        {
                                continue;
                        }

                        GameObject digitObject = new GameObject("DamageDigit");
                        SpriteRenderer spriteRenderer = digitObject.AddComponent<SpriteRenderer>();
                        spriteRenderer.color = color;
                        spriteRenderer.sortingOrder = 10;
                        spriteRenderer.sprite = particles[digit];

                        digitObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        digitObject.transform.position = position + new Vector3(xOffset, 0, 0);
                        digitObject.transform.SetParent(boardTransform);

                        LeanTween.alpha(digitObject.gameObject, 0f, 0.5f).setDelay(0.6f);
                        LeanTween.moveX(digitObject.gameObject, digitObject.transform.position.x + 0.4f, 1.2f)
                                .setOnComplete(() => Destroy(digitObject.gameObject))
                                .setEase(LeanTweenType.easeOutCirc);

                        xOffset += 0.35f;
                }
        }

}
