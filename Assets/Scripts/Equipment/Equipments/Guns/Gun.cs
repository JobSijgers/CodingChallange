using System.Collections.Generic;
using UnityEngine;

namespace Equipment.Equipments.Guns
{
    public class Gun : EquipmentItem
    {
        public override HashSet<EEquipmentType> EquipmentTypes { get; } = new() { EEquipmentType.Hand };
        public override object GetDependentEquipment() => null;

        [SerializeField] private int maxAmmo;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;

        private int currentAmmo;

        private void Start() => currentAmmo = maxAmmo;

        public override void Equipped()
        {
            base.Equipped();
            SendAmmoUpdate();
        }

        public void Reload()
        {
            SetAmmo(maxAmmo);
        }

        public override EReuseType Use(object dependentEquipment = null)
        {
            if (currentAmmo <= 0)
            {
                Debug.Log("Out of ammo");
                return EReuseType.Keep;
            }

            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            ChangeAmount(-1);
            return EReuseType.Keep;
        }

        private void ChangeAmount(int amount)
        {
            currentAmmo += amount;
            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;
            SendAmmoUpdate();
        }

        private void SetAmmo(int ammo)
        {
            currentAmmo = ammo;
            SendAmmoUpdate();
        }

        private void SendAmmoUpdate()
        {
            UpdateUI?.Invoke($"Ammo: {currentAmmo}/{maxAmmo}");
        }

        public override string GetEquipmentName() => "Gun";
    }
}