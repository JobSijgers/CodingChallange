using System.Collections.Generic;

namespace Equipment.Equipments
{
    public class Hat : EquipmentItem
    {
        public override HashSet<EEquipmentType> EquipmentTypes { get; } =
            new() { EEquipmentType.Hand, EEquipmentType.Head };

        public override void Equipped()
        {
            base.Equipped();
            UpdateUI?.Invoke("Hat");
        }

        public override EReuseType Use(object dependentEquipment = null)
        {
            return EReuseType.Keep;
        }
        public override string GetEquipmentName() => "Hat";
    }
}