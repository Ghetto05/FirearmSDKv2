using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhettosFirearmSDKv2.Explosives;
using UnityEditor;

#if UNITY_EDITOR
namespace GhettosFirearmSDKv2
{
    [CustomEditor(typeof(ProjectileData))]
    public class ProjectileDataEditor : Editor
    {
        ProjectileData data;
        bool showSoundAndFlash = false;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Shared settings", EditorStyles.boldLabel);
            data = (ProjectileData)target;
            data.isInert = EditorGUILayout.Toggle("Inert cartridge", data.isInert);

            if (data.isInert)
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Additional information for ammo box:", GUILayout.Width(300));
            data.additionalInformation = EditorGUILayout.TextArea(data.additionalInformation);
            EditorGUILayout.EndHorizontal();

            data.accuracyMultiplier = EditorGUILayout.FloatField("Accuracy multiplier", data.accuracyMultiplier);
            data.recoil = EditorGUILayout.FloatField("Recoil", data.recoil);
            data.recoilUpwardsModifier = EditorGUILayout.FloatField("Recoil upwards torque", data.recoilUpwardsModifier);

            #region Sound and flash override
            showSoundAndFlash = EditorGUILayout.Foldout(showSoundAndFlash, "Sound and flash overrides", EditorStyles.foldoutHeader);
            if (showSoundAndFlash)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(1 * 20);
                data.allowPitchVariation = !EditorGUILayout.Toggle("Disallow pitch variation?", !data.allowPitchVariation);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(1 * 20);
                data.playFirearmDefaultMuzzleFlash = !EditorGUILayout.Toggle("Only play additional muzzle flash?", !data.playFirearmDefaultMuzzleFlash);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(1 * 20);
                data.alwaysSuppressed = EditorGUILayout.Toggle("Always suppressed?", data.alwaysSuppressed);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(1 * 20);
                data.overrideFireSounds = EditorGUILayout.Toggle("Override fire sounds?", data.overrideFireSounds);
                GUILayout.EndHorizontal();

                if (data.overrideFireSounds)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(1 * 20);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireSounds"), true);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(1 * 20);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("suppressedFireSounds"), true);
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(1 * 20);
                data.overrideMuzzleFlashLightColor = EditorGUILayout.Toggle("Override muzzle flash light color?", data.overrideMuzzleFlashLightColor);
                GUILayout.EndHorizontal();

                if (data.overrideMuzzleFlashLightColor)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(1 * 20);
                    data.muzzleFlashLightColorOne = EditorGUILayout.ColorField("Flash color one", data.muzzleFlashLightColorOne);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(1 * 20);
                    data.muzzleFlashLightColorTwo = EditorGUILayout.ColorField("Flash color two", data.muzzleFlashLightColorTwo);
                    GUILayout.EndHorizontal();
                }
            }
            #endregion

            EditorGUILayout.Space();
            data.isHitscan = EditorGUILayout.Toggle("Is hitscan?", data.isHitscan);

            if (data.isHitscan)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Hitscan settings", EditorStyles.boldLabel);
                data.projectileCount = EditorGUILayout.IntField("Projectile count", data.projectileCount);
                if (data.projectileCount > 1) data.projectileSpread = EditorGUILayout.FloatField("Projectile spread angle", data.projectileSpread);
                data.projectileRange = EditorGUILayout.FloatField("Projectile range", data.projectileRange);
                data.damagePerProjectile = EditorGUILayout.FloatField("Projectile damage", data.damagePerProjectile);
                data.forcePerProjectile = EditorGUILayout.FloatField("Projectile force", data.forcePerProjectile);
                data.slicesBodyParts = EditorGUILayout.Toggle("Slice off hit body parts?", data.slicesBodyParts);
                data.enoughToIncapitate = EditorGUILayout.Toggle("Strong enough to incapacitate?", data.enoughToIncapitate);
                data.lethalHeadshot = EditorGUILayout.Toggle("Is headshot guaranteed kill?", data.lethalHeadshot);
                data.forceDestabilize = EditorGUILayout.Toggle("Always destabilize hit creature?", data.forceDestabilize);
                data.fireDamage = EditorGUILayout.FloatField("Fire damage (100 to burn)", data.fireDamage);
                data.penetrationPower = (ProjectileData.PenetrationLevels)EditorGUILayout.EnumPopup("Penetration power", data.penetrationPower);

                EditorGUILayout.Space();

                data.knocksOutTemporarily = EditorGUILayout.Toggle("Knocks out temporarily?", data.knocksOutTemporarily);
                if (data.knocksOutTemporarily)
                {
                    data.temporaryKnockoutTime = EditorGUILayout.FloatField("Knockout time", data.temporaryKnockoutTime);
                    data.kockoutDelay = EditorGUILayout.FloatField("Kockout delay", data.kockoutDelay);
                }

                EditorGUILayout.Space();

                data.isExplosive = EditorGUILayout.Toggle("Detonate on impact?", data.isExplosive);
                if (data.isExplosive)
                {
                    data.explosiveData = (ExplosiveData)EditorGUILayout.ObjectField("Explosive data", data.explosiveData, typeof(ExplosiveData), true);
                    data.explosiveEffect = (ParticleSystem)EditorGUILayout.ObjectField("Explosive effect", data.explosiveEffect, typeof(ParticleSystem), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("explosiveSoundEffects"), true);
                }

                EditorGUILayout.Space();

                data.isElectrifying = EditorGUILayout.Toggle("Electrocute hit creature?", data.isElectrifying);
                if (data.isElectrifying)
                {
                    data.tasingForce = EditorGUILayout.FloatField("Electrocution force", data.tasingForce);
                    data.tasingDuration = EditorGUILayout.FloatField("Electrocution duration", data.tasingDuration);
                    data.playTasingEffect = EditorGUILayout.Toggle("Play effect?", data.playTasingEffect);
                }

                EditorGUILayout.Space();

                data.hasImpactEffect = EditorGUILayout.Toggle("Has ground impact effect?", data.hasImpactEffect);
                if (data.hasImpactEffect)
                {
                    data.customImpactEffectId = EditorGUILayout.TextField("Custom (ground) impact effect ID", data.customImpactEffectId);
                }

                EditorGUILayout.Space();

                data.hasBodyImpactEffect = EditorGUILayout.Toggle("Has ragdoll impact effect?", data.hasBodyImpactEffect);
                if (data.hasBodyImpactEffect)
                {
                    data.customRagdollImpactEffectId = EditorGUILayout.TextField("Custom ragdoll impact effect ID", data.customRagdollImpactEffectId);
                }

                EditorGUILayout.Space();

                data.drawsImpactDecal = EditorGUILayout.Toggle("Draw impact decal?", data.drawsImpactDecal);
                if (data.drawsImpactDecal)
                {
                    data.customImpactDecalId = EditorGUILayout.TextField("Custom impact decal ID", data.customImpactDecalId);
                }
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Physical projectile settings", EditorStyles.boldLabel);
                data.projectileItemId = EditorGUILayout.TextField("Projectile item ID", data.projectileItemId);
                data.muzzleVelocity = EditorGUILayout.FloatField("Muzzle velocity", data.muzzleVelocity);
                data.destroyTime = EditorGUILayout.FloatField("Auto destroy time", data.destroyTime);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif