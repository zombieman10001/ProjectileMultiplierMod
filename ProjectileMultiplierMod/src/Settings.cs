using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace ProjectileMultiplierMod
{
    public class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "ProjectileMultiplierMod_Settings_v2";
        public override string DisplayName => "Projectile Multiplier";
        public override string FolderName => "ProjectileMultiplierMod";
        public override string FormatType => "json2";

        [SettingPropertyBool("Player: Use Extreme Multiplier", Order = 0, RequireRestart = false, HintText = "Enable extreme multiplier for player projectiles")]
        [SettingPropertyGroup("Player")]
        public bool PlayerUseExtreme { get; set; } = false;

        [SettingPropertyInteger("Player: Multiplier (Normal)", 1, 10, "0", Order = 1, RequireRestart = false, HintText = "Number of projectiles to spawn (1-10)")]
        [SettingPropertyGroup("Player")]
        public int PlayerMultiplier { get; set; } = 3;

        [SettingPropertyInteger("Player: Multiplier (Extreme)", 1, 100, "0", Order = 2, RequireRestart = false, HintText = "Extreme number of projectiles (1-100). Use with caution!")]
        [SettingPropertyGroup("Player")]
        public int PlayerExtremeMultiplier { get; set; } = 50;

        [SettingPropertyBool("NPCs: Use Extreme Multiplier", Order = 0, RequireRestart = false, HintText = "Enable extreme multiplier for NPC projectiles")]
        [SettingPropertyGroup("NPCs")]
        public bool NpcUseExtreme { get; set; } = false;

        [SettingPropertyInteger("NPCs: Multiplier (Normal)", 1, 10, "0", Order = 1, RequireRestart = false, HintText = "Number of projectiles for NPCs (1-10)")]
        [SettingPropertyGroup("NPCs")]
        public int NpcMultiplier { get; set; } = 3;

        [SettingPropertyInteger("NPCs: Multiplier (Extreme)", 1, 100, "0", Order = 2, RequireRestart = false, HintText = "Extreme number of projectiles for NPCs (1-100)")]
        [SettingPropertyGroup("NPCs")]
        public int NpcExtremeMultiplier { get; set; } = 50;

        [SettingPropertyInteger("NPCs: Hard Safety Cap", 1, 100, "0", Order = 3, RequireRestart = false, HintText = "Maximum projectiles for NPCs regardless of other settings")]
        [SettingPropertyGroup("NPCs")]
        public int NpcHardCap { get; set; } = 25;

        [SettingPropertyBool("Slight Random Spread", Order = 0, RequireRestart = false, HintText = "Add slight random spread to projectiles. Disable for pinpoint accuracy.")]
        [SettingPropertyGroup("Accuracy & UX")]
        public bool SlightRandomSpread { get; set; } = true;

        [SettingPropertyBool("Show Extreme Warning", Order = 1, RequireRestart = false, HintText = "Display a warning message when extreme multiplier is active")]
        [SettingPropertyGroup("Accuracy & UX")]
        public bool ShowExtremeWarning { get; set; } = true;

        [SettingPropertyBool("Multiply Thrown Weapons", Order = 2, RequireRestart = false, HintText = "Apply multiplier to thrown weapons (experimental feature)")]
        [SettingPropertyGroup("Weapons")]
        public bool MultiplyThrown { get; set; } = false;

        public int EffectivePlayer =>
            PlayerUseExtreme ? PlayerExtremeMultiplier : PlayerMultiplier;

        public int EffectiveNpc =>
            NpcUseExtreme ? NpcExtremeMultiplier : NpcMultiplier;
    }
}
