﻿using System.Collections.Generic;
using UnityEngine;

namespace WeaponPack.SO
{
    [CreateAssetMenu(fileName = "new Weapon", menuName = "Custom/Weapon")]
    public class SoWeapon : ScriptableObject
    {
        [SerializeField] private string weaponName;
        [SerializeField, TextArea] private string weaponDescription;
        [SerializeField] private Sprite weaponSprite;
        [SerializeField] private bool oneTimeSpawnLogic = false;
        [SerializeField] private float cooldown;
        [SerializeField] private GameObject weaponLogicPrefab;

        [SerializeField] private List<WeaponStatPair> weaponStartingStats;
        [SerializeField] private List<UpgradeWeaponStats> weaponUpgradeStats;

        public string WeaponName => weaponName;
        public string WeaponDescription => weaponDescription;
        public float Cooldown => cooldown;
        public bool OneTimeSpawnLogic => oneTimeSpawnLogic;
        public GameObject WeaponLogicPrefab => weaponLogicPrefab;
        public Sprite WeaponSprite => weaponSprite;
        public List<WeaponStatPair> WeaponStartingStats => weaponStartingStats;
        public List<UpgradeWeaponStats> WeaponUpgradeStats => weaponUpgradeStats;
    }
}