using Equipment;
using Equipment.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class PlayerRaycaster : MonoBehaviour
    {
        [SerializeField] private float interactionDistance;
        [SerializeField] private LayerMask interactionLayer;
        [SerializeField] private Transform interactionOrigin;
        [SerializeField] private RaycastUIPrompt raycastUIPrompt;
        [SerializeField] private EquipmentSelectionMenu equipmentSelectionMenu;
        [SerializeField] private PlayerEquipmentManager equipmentManager;
        
        private PlayerInputActions playerInputActions;
        private InputAction interactAction;
        private InputAction equipAction;

        private IInteractable currentInteractable;
        private IEquippable currentEquippable;

        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
        }
        
        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        private void Update()
        {
            HandleRaycasting();
            UpdateUIPrompts();
        }

        private void HandleRaycasting()
        {
            currentInteractable = null;
            currentEquippable = null;
            if (!Physics.Raycast(interactionOrigin.position, interactionOrigin.forward, out RaycastHit hit,
                    interactionDistance, interactionLayer)) return;
            // Get the interactable and equippable components from the hit object
            hit.collider.TryGetComponent(out currentInteractable);
            hit.collider.TryGetComponent(out currentEquippable);
        }

        private void UpdateUIPrompts()
        {
            raycastUIPrompt.ChangeInteractPromptState(currentInteractable != null, currentInteractable?.GetInteractablePromptAddition());
            raycastUIPrompt.ChangeEquipPromptState(currentEquippable != null, currentEquippable?.GetEquipmentName());
        }

        private void Interact(InputAction.CallbackContext obj)
        {
            if (currentInteractable == null) return;
            currentInteractable.Interact();
        }

        private void ShowEquipmentSlotSelectionMenu(InputAction.CallbackContext obj)
        {
            if (currentEquippable == null) return;

            EquipmentSlot[] slots = equipmentManager.GetAvailableSlotsForSpecificEquipment(currentEquippable);
            if (slots == null || slots.Length == 0)
                return;
            equipmentSelectionMenu.ShowMenu(slots, EquipItem);
        }

        private void EquipItem(EquipmentSlot slot)
        {
            equipmentSelectionMenu.HideMenu();
            equipmentManager.EquipItem(slot, currentEquippable);
        }

        private void EnableInput()
        {
            interactAction = playerInputActions.Player.Interact;
            equipAction = playerInputActions.Player.Equip;
            interactAction.Enable();
            equipAction.Enable();
            interactAction.performed += Interact;
            equipAction.performed += ShowEquipmentSlotSelectionMenu;
        }

        private void DisableInput()
        {
            interactAction.Disable();
            equipAction.Disable();
            interactAction.performed -= Interact;
            equipAction.performed -= ShowEquipmentSlotSelectionMenu;
        }
    }
}