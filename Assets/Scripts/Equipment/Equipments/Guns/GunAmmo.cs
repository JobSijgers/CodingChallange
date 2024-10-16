using System.Collections.Generic;

namespace Equipment.Equipments.Guns
{
    public class GunAmmo : EquipmentItem
    {
        public override HashSet<EEquipmentType> EquipmentTypes { get; } = new() { EEquipmentType.Hand};
        public override object GetDependentEquipment() => typeof(Gun);

        public override void Equipped()
        {
            base.Equipped();
            UpdateUI?.Invoke("Use to reload gun");
        }

        public override EReuseType Use(object dependentEquipment = null)
        {
            if (dependentEquipment == null)
                return EReuseType.Keep;
            // Cast dependentEquipment to Gun and call Reload method
            Gun gun = dependentEquipment as Gun;
            if (gun != null)
            {
                gun.Reload();
            }
            return EReuseType.Destroy;
        }
        
        public override string GetEquipmentName() => "Gun ammo";
    }
}