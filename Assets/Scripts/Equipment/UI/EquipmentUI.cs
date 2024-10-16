using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Equipment.UI
{
    public class EquipmentUI : MonoBehaviour
    {
        [SerializeField] private EquipmentUIItem equipmentUIItemPrefab;
        [SerializeField] private Transform equipmentUIParent;
        [SerializeField] private PlayerEquipmentManager equipmentManager;
        
        private readonly Dictionary<EquipmentSlot, EquipmentUIItem> equipmentSlotToUIItem = new();

        private void Start()
        {
            SetupUI(equipmentManager.GetAllSlots());
        }

        private void OnDestroy()
        {
            foreach (KeyValuePair<EquipmentSlot, EquipmentUIItem> pair in equipmentSlotToUIItem)
            {
                pair.Key.UpdateUI.RemoveListener(UpdateUI);
            }
        }

        private void SetupUI(EquipmentSlot[] allSlots)
        {
            foreach (EquipmentSlot equipmentSlot in allSlots)
            {
                if (equipmentSlot == null)
                    continue;
                CreateNewItem(equipmentSlot);
                BindItemToSlot(equipmentSlot.UpdateUI);
            }
        }

        private void CreateNewItem(EquipmentSlot equipmentSlot)
        {
            EquipmentUIItem equipmentUIItem = Instantiate(equipmentUIItemPrefab, equipmentUIParent);
            equipmentSlotToUIItem.Add(equipmentSlot, equipmentUIItem);
            equipmentUIItem.SetEquipmentName(equipmentSlot.name);
            equipmentUIItem.SetEquipmentDescription("Empty Slot");
        }

        private void BindItemToSlot(UnityEvent<string, EquipmentSlot> updateEvent)
        {
            updateEvent.AddListener(UpdateUI);
        }
        
        private void UpdateUI(string description, EquipmentSlot equipmentSlot)
        {
            equipmentSlotToUIItem[equipmentSlot].SetEquipmentDescription(description);
        }
    }
}