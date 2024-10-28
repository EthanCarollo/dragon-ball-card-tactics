
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignDatabase", menuName = "Campaign/CampaignDatabase")]
public class CampaignDatabase: ScriptableObject
{
        private static CampaignDatabase _instance;
        
        public Campaign[] campaigns;

        public static CampaignDatabase Instance
        {
                get
                {
                        if (_instance == null)
                        {
                                _instance = Resources.Load<CampaignDatabase>("CampaignDatabase");
                                if (_instance == null)
                                {
                                        Debug.LogError("CampaignDatabase instance not found in Resources folder!");
                                }
                        }
                        return _instance;
                }
        }
}