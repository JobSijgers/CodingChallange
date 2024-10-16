using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Equipment.Equipments
{
    public abstract class EquipmentItem : MonoBehaviour, IEquippable
    {
        public abstract HashSet<EEquipmentType> EquipmentTypes { get; }
        public UnityEvent<string> UpdateUI { get; set; } = new();

        public virtual object GetDependentEquipment() => null;
        public Transform GetTransform() => transform;
        public abstract EReuseType Use(object dependentEquipment = null);

        public virtual void Equipped() { }
        public virtual void Unequipped() => UpdateUI?.Invoke("Empty slot");
        
        public abstract string GetEquipmentName();
    }
}