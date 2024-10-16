using System;
using TMPro;
using UnityEngine;

namespace Interaction
{
    
    public class RaycastUIPrompt : MonoBehaviour
    {
        [SerializeField] private string baseInteractPrompt = "Press E to";
        [SerializeField] private string baseEquipPrompt = "Press R to equip";
        
        [SerializeField] private TMP_Text equipPrompt;
        [SerializeField] private TMP_Text interactPrompt;
        
        public void ChangeEquipPromptState(bool state, string promptAddition)
        {
            equipPrompt.text = baseEquipPrompt + promptAddition;
            equipPrompt.gameObject.SetActive(state);
        }
        
        public void ChangeInteractPromptState(bool state, string promptAddition)
        {
            interactPrompt.text = baseInteractPrompt + promptAddition;
            interactPrompt.gameObject.SetActive(state);
        }
    }
}