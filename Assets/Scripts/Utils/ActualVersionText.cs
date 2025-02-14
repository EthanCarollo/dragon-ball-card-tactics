using System;
using TMPro;
using UnityEngine;

public class ActualVersionText : MonoBehaviour
{
        public void Start()
        {
                this.GetComponent<TextMeshProUGUI>().text = string.Format("v{0}", Application.version);
        }
}