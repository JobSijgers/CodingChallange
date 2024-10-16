using System.Collections.Generic;
using Equipment.Equipments;
using UnityEngine;
using UnityEngine.Events;

namespace Equipment
{
    public interface IEquippable
    {
        public HashSet<EEquipmentType> EquipmentTypes { get; }
        public UnityEvent<string> UpdateUI { get; set; }

        public bool IsOfType(EEquipmentType type) => EquipmentTypes.Contains(type);
        //override this to make the equipment dependent on another equipment
        public object GetDependentEquipment();
        public Transform GetTransform();
        public void Equipped() {}
        public void Unequipped(){}
        public EReuseType Use(object dependentEquipment = null);
        public string GetEquipmentName();
    }
}