using System;
using TMPro;
using UnityEngine;

public class ActualVersionText : MonoBehaviour
{
        public void Start()
        {
                var versionText = GetComponent<TextMeshProUGUI>();
                if (versionText != null)
                {
                        versionText.text = string.Format("v{0}", Application.version);
                }
        }
}
