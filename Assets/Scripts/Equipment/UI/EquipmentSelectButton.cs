using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Equipment.UI
{
    public class EquipmentSelectButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText;
        
        public void Setup(EquipmentSlot slot, UnityAction<EquipmentSlot> select)
        {
            buttonText.text = slot.name;
            button.onClick.AddListener(() => select(slot));
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}