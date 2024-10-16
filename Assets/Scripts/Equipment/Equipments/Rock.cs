using System.Collections.Generic;
using UnityEngine;

namespace Equipment.Equipments
{
    public class Rock : EquipmentItem
    {
        public override HashSet<EEquipmentType> EquipmentTypes { get; } = new() { EEquipmentType.Hand };

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float throwForce = 10f;

        public override void Unequipped()
        {
            base.Unequipped();
            rb.isKinematic = false;
        }
        
        public override void Equipped()
        {
            base.Equipped();
            rb.isKinematic = true;
            UpdateUI?.Invoke("Use to throw");
        }
        
        public override EReuseType Use(object dependentEquipment = null)
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            return EReuseType.Unequip;
        }
        
        public override string GetEquipmentName() => "Rock";
    }
}