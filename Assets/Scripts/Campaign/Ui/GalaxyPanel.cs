using UnityEngine;

public class GalaxyPanel : MonoBehaviour
{
    public GameObject planetPrefab; 

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
            float panelWidth = panelRect.rect.width - 500;
            float panelHeight = panelRect.rect.height - 500;

            foreach (var campaign in campaigns)
            {
                float randomX = Random.Range(-panelWidth / 2, panelWidth / 2);
                float randomY = Random.Range(-panelHeight / 2, panelHeight / 2);
                Vector2 randomPosition = new Vector2(randomX, randomY);

                GameObject planet = Instantiate(planetPrefab, randomPosition, Quaternion.identity, transform);
                planet.GetComponent<CampaignButton>().campaign = campaign;

                RectTransform planetRect = planet.GetComponent<RectTransform>();
                planetRect.anchoredPosition = randomPosition;
                planetRect.localScale = Vector3.one;
                planet.name = "Planet_" + campaign.GetActualCampaign();
            }
        }
        else
        {
            Debug.LogWarning("GameManager ou actualGalaxy est nul.");
        }
    }
}