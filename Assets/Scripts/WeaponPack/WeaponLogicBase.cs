﻿using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using PlayerPack;
using UnityEngine;
using WeaponPack.Enums;
using WeaponPack.SO;

namespace WeaponPack
{
    public abstract class WeaponLogicBase : MonoBehaviour
    {
        public SoWeapon Weapon => _weapon;
        public int Level => _level;
        
        protected SoWeapon _weapon;

        protected List<WeaponStatPair> _realWeaponStats = new();

        protected int Damage => (int)_realWeaponStats.FirstOrDefault(s => s.statType == EWeaponStat.Damage)!.statValue;
        protected float Speed => _realWeaponStats.FirstOrDefault(s => s.statType == EWeaponStat.Speed)!.statValue;
        protected int ProjectileCount => (int)_realWeaponStats.FirstOrDefault(s => s.statType == EWeaponStat.ProjectilesCount)!.statValue;
        
        protected Vector2 PlayerPos => GameManager.Instance.CurrentPlayer.transform.position;
        protected Transform PlayerTransform => GameManager.Instance.CurrentPlayer.transform;

        protected float Cooldown => _weapon.Cooldown * (1 - _realWeaponStats.FirstOrDefault(s => s.statType == EWeaponStat.CooldownReduction)!.statValue);
        private float _timer = 0;
        
        protected int _level = 0;

        protected bool spawned = false;
        
        public void Setup(SoWeapon weapon)
        {
            _weapon = weapon;
            _realWeaponStats = weapon.WeaponStartingStats;
            PlayerWeaponry.OnWeaponLevelUp += OnLevelUp;
        }

        private void OnDisable()
        {
            PlayerWeaponry.OnWeaponLevelUp -= OnLevelUp;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < Cooldown || (spawned && _weapon.OneTimeSpawnLogic)) return;

            _timer = 0;
            UseWeapon();
        }

        protected abstract void UseWeapon();

        private void OnLevelUp(string weaponName)
        {
            if (weaponName != _weapon.WeaponName) return;

            _level++;
            IncrementStats();
        }

        private void IncrementStats()
        {
            var stats = _weapon.WeaponStartingStats;
            foreach (var stat in GetCurrentUpgradeStats())
            {
                var statTuple = stats.FirstOrDefault(s => s.statType == stat.statType);
                if(statTuple == default) continue;

                if (stat.isPercentage) statTuple.statValue *= 1 + stat.statValue;
                else statTuple.statValue += stat.statValue;
            }
        }

        private List<WeaponStatPair> GetCurrentUpgradeStats()
        {
            for (var i = _weapon.WeaponUpgradeStats.Count; i > 0; i--)
            {
                if (_level % i == 0) return _weapon.WeaponUpgradeStats[i].levelStats;
            }

            return new List<WeaponStatPair>();
        }
    }
}