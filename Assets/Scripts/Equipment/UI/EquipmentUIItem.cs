using TMPro;
using UnityEngine;

namespace Equipment.UI
{
    public class EquipmentUIItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text equipmentName;
        [SerializeField] private TMP_Text equipmentDescription;
        
        public void SetEquipmentName(string name)
        {
            equipmentName.text = name;
        }

        public void SetEquipmentDescription(string description)
        {
            equipmentDescription.text = description;
        }
    }
}