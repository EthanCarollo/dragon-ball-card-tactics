using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GalaxyPanel : MonoBehaviour
{
    public GameObject planetPrefab; 
    public Transform galaxyParent;
    public SelectCharacterCampaign selectCharacterCampaign;

    public void Awake()
    {
        SetupGalaxyPanel();
    }

    public void SetupGalaxyPanel()
    {
        if (GameManager.Instance != null && GameManager.Instance.actualGalaxy != null)
        {
            var campaigns = GameManager.Instance.actualGalaxy.campaigns;
            RectTransform panelRect = GetComponent<RectTransform>();
            float panelWidth = panelRect.rect.width - 800;
            float panelHeight = panelRect.rect.height - 500;
            float minDistance = 350f; // Minimum distance between planets

            // List to store positions of instantiated planets
            List<Vector2> existingPositions = new List<Vector2>();

            foreach (var campaign in campaigns)
            {
                Vector2 randomPosition;
                bool positionIsValid;

                // Keep generating a position until it meets the minimum distance requirement
                do
                {
                    float randomX = UnityEngine.Random.Range(-panelWidth / 2, panelWidth / 2) - 300;
                    float randomY = UnityEngine.Random.Range(-panelHeight / 2, panelHeight / 2);
                    randomPosition = new Vector2(randomX, randomY);

                    positionIsValid = true;
                    foreach (var existingPosition in existingPositions)
                    {
                        if (Vector2.Distance(randomPosition, existingPosition) < minDistance)
                        {
                            positionIsValid = false;
                            break;
                        }
                    }
                } while (!positionIsValid);

                // Instantiate the planet at the valid position
                GameObject planet = Instantiate(planetPrefab, randomPosition, Quaternion.identity, galaxyParent);
                planet.GetComponent<CampaignButton>().campaign = campaign;

                if(campaign.planet != null){
                    planet.GetComponent<Image>().sprite = campaign.planet;
                }

                RectTransform planetRect = planet.GetComponent<RectTransform>();
                RectTransform planetRectTransform = planet.transform.GetChild(0).GetComponent<RectTransform>();

                if (randomPosition.x > 0)
                {
                    Vector3 anchoredPos = planetRectTransform.anchoredPosition;
                    planetRectTransform.anchoredPosition = new Vector3(-anchoredPos.x, anchoredPos.y, 0f);
                }

                planetRect.anchoredPosition = randomPosition;
                planetRect.localScale = Vector3.one;
                planet.name = "Planet_" + campaign.GetActualCampaign();

                // Store the position of the newly instantiated planet
                existingPositions.Add(randomPosition);
            }

        }
        else
        {
            Debug.LogWarning("GameManager ou actualGalaxy est nul.");
        }
    }
}