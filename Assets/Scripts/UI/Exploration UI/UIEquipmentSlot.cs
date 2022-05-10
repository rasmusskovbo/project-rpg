
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIEquipmentSlot
{
        [SerializeField] private Image unequippedIcon;
        [SerializeField] private Image equipmentIcon;
        [SerializeField] private EquipmentType equipmentType;

        public Image UnequippedIcon
        {
                get => unequippedIcon;
                set => unequippedIcon = value;
        }

        public Image EquipmentIcon
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
