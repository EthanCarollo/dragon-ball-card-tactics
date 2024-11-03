using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGalaxy", menuName = "Galaxy/Galaxy")]
public class Galaxy : ScriptableObject
{
        public string galaxyName;
        public string galaxyDescription;
        public CampaignContainer[] campaigns;
}

[Serializable]
public class CampaignContainer
{
        public string campaignName;
        public Campaign[] campaign;
        public int actualCampaign = 0;

        public Campaign GetActualCampaign()
        {
                return campaign[actualCampaign];
        }
}