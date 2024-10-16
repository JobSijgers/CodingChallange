using System;
using System.Collections.Generic;
using Equipment.Equipments;
using Equipment.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Equipment
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        //slots for hands separate for input
        [SerializeField] private EquipmentSlot leftHand, rightHand;
        [SerializeField] private EquipmentSlot[] otherEquipmentSlots;
        [SerializeField] private EquipmentSelectionMenu equipmentSelectionMenu;

        private PlayerInputActions playerInputActions;
        private InputAction leftHandAction, rightHandAction, dropAction;

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

        /// <summary>
        /// Tries to use the equipment in the equipment slot, does nothing if the slot is empty or the dependent equipment is not equipped
        /// </summary>
        /// <param name="equipmentSlot">Slot to use</param>
        private void TryUseHand(EquipmentSlot equipmentSlot)
        {
            if (IsSlotEmpty(equipmentSlot))
                return;

            IEquippable equipment = equipmentSlot.EquipmentInSlot;

            if (IsEquipmentIndependent(equipment))
            {
                UseEquipment(equipmentSlot, equipment);
                return;
            }

            EquipmentSlot dependentEquipment = GetEquipmentOfType(equipment.GetDependentEquipment());
            if (dependentEquipment == null)
                return;

            UseEquipment(equipmentSlot, equipment, dependentEquipment);
        }

        private static bool IsSlotEmpty(EquipmentSlot equipmentSlot)
        {
            return equipmentSlot == null || equipmentSlot.EquipmentInSlot == null;
        }

        private static bool IsEquipmentIndependent(IEquippable equipment)
        {
            return equipment.GetDependentEquipment() == null;
        }

        private void UseEquipment(EquipmentSlot equipmentSlot, IEquippable equipment,
            EquipmentSlot dependentEquipment = null)
        {
            EReuseType reuseType = dependentEquipment == null
                ? equipment.Use()
                : equipment.Use(dependentEquipment.EquipmentInSlot);
            HandleSlotReuse(equipmentSlot, reuseType);
        }

        private void HandleSlotReuse(EquipmentSlot equipmentSlot, EReuseType reuseType)
        {
            switch (reuseType)
            {
                case EReuseType.Keep:
                    HandleKeep(equipmentSlot);
                    break;

                case EReuseType.Unequip:
                    HandleUnequip(equipmentSlot);
                    break;

                case EReuseType.Destroy:
                    HandleDestroy(equipmentSlot);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleKeep(EquipmentSlot equipmentSlot)
        {
            // Currently does nothing
        }

        private void HandleUnequip(EquipmentSlot equipmentSlot)
        {
            equipmentSlot.Unequip();
        }

        private void HandleDestroy(EquipmentSlot equipmentSlot)
        {
            GameObject itemToDestroy = equipmentSlot.EquipmentInSlot.GetTransform().gameObject;
            equipmentSlot.Unequip();
            Destroy(itemToDestroy);
        }

        /// <summary>
        /// if the slot is empty equip the item
        /// </summary>
        /// <param name="equipmentSlot">Slot to equip item to</param>
        /// <param name="equipment">Equipment to snap to slot</param>
        public void EquipItem(EquipmentSlot equipmentSlot, IEquippable equipment)
        {
            if (!equipmentSlot.IsOccupied)
            {
                equipmentSlot.Equip(equipment);
            }
        }

        // Returns all the slots that are free that can equip the equipment
        public EquipmentSlot[] GetAvailableSlotsForSpecificEquipment(IEquippable equipment)
        {
            return GetSlots(slot => !slot.IsOccupied && equipment.IsOfType(slot.GetSlotType()));
        }

        //Returns all slots that are occupied
        private EquipmentSlot[] GetAllOccupiedSlots()
        {
            return GetSlots(slot => slot.IsOccupied);
        }

        // Gets all the slots
        public EquipmentSlot[] GetAllSlots()
        {
            return GetSlots(slot => true);
        }

        private bool HasAnyEquipment()
        {
            return GetSlots(slot => slot.IsOccupied) != null;
        }

        private EquipmentSlot GetEquipmentOfType(object type)
        {
            EquipmentSlot[] slots = GetSlots(slot => slot.IsOccupied && slot.EquipmentInSlot.GetType() == (Type)type);
            if (slots == null || slots.Length == 0)
                return null;

            return slots[0];
        }

        // Gets all the slots that satisfy the predicate
        private EquipmentSlot[] GetSlots(Func<EquipmentSlot, bool> predicate)
        {
            List<EquipmentSlot> slots = new();

            if (predicate(leftHand))
                slots.Add(leftHand);
            if (predicate(rightHand))
                slots.Add(rightHand);

            foreach (EquipmentSlot equipmentSlot in otherEquipmentSlots)
            {
                if (equipmentSlot == null)
                    continue;
                if (predicate(equipmentSlot))
                    slots.Add(equipmentSlot);
            }

            return slots.Count == 0 ? null : slots.ToArray();
        }

        private void OpenDropMenu(InputAction.CallbackContext obj)
        {
            if (!HasAnyEquipment())
                return;
            equipmentSelectionMenu.ShowMenu(GetAllOccupiedSlots(), DropItem);
        }

        private void DropItem(EquipmentSlot slotToDrop)
        {
            equipmentSelectionMenu.HideMenu();
            slotToDrop.Unequip();
        }

        private void EnableInput()
        {
            //get input actions
            leftHandAction = playerInputActions.Player.UseLeft;
            rightHandAction = playerInputActions.Player.UseRight;
            dropAction = playerInputActions.Player.Drop;
            //enable input actions
            leftHandAction.Enable();
            rightHandAction.Enable();
            dropAction.Enable();
            //subscribe to input events
            playerInputActions.Player.UseLeft.performed += context => TryUseHand(leftHand);
            playerInputActions.Player.UseRight.performed += context => TryUseHand(rightHand);
            playerInputActions.Player.Drop.performed += OpenDropMenu;
        }

        private void DisableInput()
        {
            //disable input actions
            leftHandAction.Disable();
            rightHandAction.Disable();
            dropAction.Disable();
            //unsubscribe from input events
            playerInputActions.Player.UseLeft.performed -= context => TryUseHand(leftHand);
            playerInputActions.Player.UseRight.performed -= context => TryUseHand(rightHand);
            playerInputActions.Player.Drop.performed -= OpenDropMenu;
        }
    }
}