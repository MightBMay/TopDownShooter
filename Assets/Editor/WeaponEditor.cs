using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


    [CustomEditor(typeof(Inventory))]
    public class WeaponEditor : Editor
    {
        public Inventory script;
        private List<bool> showWeaponProperties = new List<bool>();

        private void OnEnable()
        {
             script = (Inventory)target;
        }

        public override void OnInspectorGUI()
        {
            Inventory script = (Inventory)target;
            base.OnInspectorGUI();
            for (int i = 0; i < script.weapons.Count; i++)
            {
                Weapon weapon = script.weapons[i];
                if (i >= showWeaponProperties.Count)
                    showWeaponProperties.Add(false);
                showWeaponProperties[i] = EditorGUILayout.Foldout(showWeaponProperties[i], weapon.Name);
                if (showWeaponProperties[i])
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    if (weapon is RangedWeapon)
                    {
                        RangedWeapon rangedWeapon = (RangedWeapon)weapon;
                        rangedWeapon.Name = EditorGUILayout.TextField("Name", rangedWeapon.Name);
                        rangedWeapon.damageFlat = EditorGUILayout.FloatField("Flat Damage", rangedWeapon.damageFlat);
                        rangedWeapon.attackRate = EditorGUILayout.FloatField("Attack Rate", rangedWeapon.attackRate);
                        rangedWeapon.curMag = EditorGUILayout.IntField("Current Ammo", rangedWeapon.curMag);
                        rangedWeapon.storedAmmo = EditorGUILayout.IntField("Stored Ammo", rangedWeapon.storedAmmo);
                        rangedWeapon.magSize = EditorGUILayout.IntField("Mag Size", rangedWeapon.magSize);
                        rangedWeapon.reloadSpeed = EditorGUILayout.FloatField("Reload Speed", rangedWeapon.reloadSpeed);
                        rangedWeapon.bulletDespawnTimer = EditorGUILayout.FloatField("Bullet Despawn Time", rangedWeapon.bulletDespawnTimer);
                        rangedWeapon.bulletSpeed = EditorGUILayout.FloatField("Bullet Speed", rangedWeapon.bulletSpeed);
                        rangedWeapon.lightColour = EditorGUILayout.ColorField("Light Colour", rangedWeapon.lightColour);
                        rangedWeapon.lightRange = EditorGUILayout.FloatField("Light Range", rangedWeapon.lightRange);
                        rangedWeapon.lightIntensity = EditorGUILayout.FloatField("Light Intensity", rangedWeapon.lightIntensity);

                    }
                    else if (weapon is MeleeWeapon)
                    {
                        MeleeWeapon meleeWeapon = (MeleeWeapon)weapon;
                        meleeWeapon.Name = EditorGUILayout.TextField("Name", meleeWeapon.Name);
                        meleeWeapon.damageFlat = EditorGUILayout.FloatField("Flat Damage", meleeWeapon.damageFlat);
                        meleeWeapon.attackRate = EditorGUILayout.FloatField("Attack Rate", meleeWeapon.attackRate);
                        meleeWeapon.attackRange = EditorGUILayout.FloatField("Attack Range", meleeWeapon.attackRange);
                        meleeWeapon.attackArc = EditorGUILayout.FloatField("Attack Arc", meleeWeapon.attackArc);

                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
    }


