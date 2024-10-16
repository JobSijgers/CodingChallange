using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Equipment.UI
{
    public class EquipmentSelectionMenu : MonoBehaviour
    {
        [SerializeField] private EquipmentSelectButton selectButtonPrefab;
        [SerializeField] private Transform buttonParent;
        [SerializeField] private GameObject background;
        
        private readonly List<EquipmentSelectButton> activeButtons = new();
        private bool isMenuActive;
        
        
        public void ShowMenu(EquipmentSlot[] slots, UnityAction<EquipmentSlot> selected)
        {
            if (isMenuActive)
                return;
            Pause();
            CreateSelectionButtons(slots, selected);
            isMenuActive = true;
            background.SetActive(true);
        }

        public void HideMenu()
        {
            Unpause();
            DestroySelectionButtons();
            isMenuActive = false;
            background.SetActive(false);
        }

        private void CreateSelectionButtons(EquipmentSlot[] slots, UnityAction<EquipmentSlot> selected)
        {
            foreach (EquipmentSlot slot in slots)
            {
                EquipmentSelectButton button = Instantiate(selectButtonPrefab, buttonParent);
                button.Setup(slot, selected);
                activeButtons.Add(button);
            }
        }

        private void DestroySelectionButtons()
        {
            foreach (EquipmentSelectButton button in activeButtons)
            {
                Destroy(button.gameObject);
            }

            activeButtons.Clear();
        }

        private void Pause()
        {
            //if you want to deploy this use a better way to pause the game but for now this will do
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Unpause()
        {
            //if you want to deploy this use a better way to pause the game but for now this will do
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}