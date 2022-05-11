
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIEquipmentSlot
{
        [SerializeField] private GameObject unequippedIcon;
        [SerializeField] private GameObject equipmentIcon;
        [SerializeField] private EquipmentType equipmentType;

        public GameObject UnequippedIcon
        {
                get => unequippedIcon;
                set => unequippedIcon = value;
        }

        public GameObject EquipmentIcon
        {
                get => equipmentIcon;
                set => equipmentIcon = value;
        }

        public EquipmentType EquipmentType
        {
                get => equipmentType;
                set => equipmentType = value;
        }
}
