using UnityEngine;
using UnityEngine.Events;

namespace Equipment.UI
{
    public class EquipmentSlot : MonoBehaviour
    {
        [SerializeField] private EEquipmentType slotType;
        public IEquippable EquipmentInSlot { get; private set; }
        public bool IsOccupied { get; private set; } = false;
        public UnityEvent<string, EquipmentSlot> UpdateUI { get; } = new ();
        
        public void Equip(IEquippable equipment)
        {
            if (IsOccupied)
            {
                Debug.Log("Slot is already occupied");
                return;
            }

            EquipmentInSlot = equipment;
            AttachToSlot(equipment.GetTransform());
            IsOccupied = true;
            EquipmentInSlot.UpdateUI.AddListener(text => UpdateUI?.Invoke(text, this));
            EquipmentInSlot.Equipped();
        }
        
        public void Unequip()
        {
            if (!IsOccupied)
            {
                Debug.Log("Slot is already empty");
                return;
            }
            EquipmentInSlot.Unequipped();
            EquipmentInSlot.GetTransform().SetParent(null);
            EquipmentInSlot.UpdateUI.RemoveAllListeners();
            EquipmentInSlot = null;
            IsOccupied = false;
        }

        private void AttachToSlot(Transform equipment)
        {
            equipment.position = transform.position;
            equipment.rotation = transform.rotation;
            equipment.SetParent(transform);
        }
        
        public EEquipmentType GetSlotType()
        {
            return slotType;
        }
    }
}