using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TAG.UI
{
    public class PickupPanel : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text itemDescription;
        private CanvasGroup panel;

        private void Awake()
        {
            GetComponents();
            panel.alpha = 0;
        }

        private void GetComponents()
        {
            panel = GetComponent<CanvasGroup>();
        }

        void OnEnable()
        {
            Pickup.pickupAdded += UpdatePanel;
        }

        private void OnDisable()
        {
            Pickup.pickupAdded -= UpdatePanel;
        }

        private void UpdatePanel(PickupData pickup)
        {
            itemName.text = pickup.displayName;
            itemImage.sprite = pickup.icon;
            itemDescription.text = pickup.description;
            StartCoroutine(ShowPickupPanel());
        }
        private IEnumerator ShowPickupPanel()
        {
            panel.alpha = 1;
            yield return new WaitForSeconds(3f);
            panel.alpha = 0;
        }
    }
}


