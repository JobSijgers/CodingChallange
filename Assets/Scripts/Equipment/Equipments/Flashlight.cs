using System.Collections.Generic;
using Interaction;
using UnityEngine;

namespace Equipment.Equipments
{
    public class Flashlight : EquipmentItem, IInteractable
    {
        public override HashSet<EEquipmentType> EquipmentTypes { get; } = new() { EEquipmentType.Hand };

        [SerializeField] private Light lightSource;
        private bool isOn;

        public void Interact() => ToggleLight();
        
        public override void Equipped()
        {
            base.Equipped();
            UpdateUI?.Invoke("Use to " + GetUsePrompt());
        }

        public override EReuseType Use(object dependentEquipment = null)
        {
            ToggleLight();
            return EReuseType.Keep;
        }

        private void ToggleLight()
        {
            isOn = !isOn;
            lightSource.enabled = isOn;
            UpdateUI?.Invoke("Use to " + GetUsePrompt());
        }
        
        public override string GetEquipmentName() => "Flashlight";
        public string GetInteractablePromptAddition() => GetUsePrompt();
        private string GetUsePrompt() => isOn ? "turn off" : "turn on";
    }
}