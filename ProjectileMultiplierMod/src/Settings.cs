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

        // PLAYER
        [SettingPropertyBool("Player: Use Extreme Multiplier", Order = 0)]
        [SettingPropertyGroup("Player")]
        public bool PlayerUseExtreme { get; set; } = false;

        [SettingPropertyInteger("Player: Multiplier (Normal)", 1, 10, "0", Order = 1)]
        [SettingPropertyGroup("Player")]
        public int PlayerMultiplier { get; set; } = 10;

        [SettingPropertyInteger("Player: Multiplier (Extreme)", 1, 100, "0", Order = 2)]
        [SettingPropertyGroup("Player")]
        public int PlayerExtremeMultiplier { get; set; } = 100;

        // NPCs
        [SettingPropertyBool("NPCs: Use Extreme Multiplier", Order = 0)]
        [SettingPropertyGroup("NPCs")]
        public bool NpcUseExtreme { get; set; } = false;

        [SettingPropertyInteger("NPCs: Multiplier (Normal)", 1, 10, "0", Order = 1)]
        [SettingPropertyGroup("NPCs")]
        public int NpcMultiplier { get; set; } = 10;

        [SettingPropertyInteger("NPCs: Multiplier (Extreme)", 1, 100, "0", Order = 2)]
        [SettingPropertyGroup("NPCs")]
        public int NpcExtremeMultiplier { get; set; } = 100;

        [SettingPropertyInteger("NPCs: Hard Safety Cap", 1, 100, "0", Order = 3)]
        [SettingPropertyGroup("NPCs")]
        public int NpcHardCap { get; set; } = 25;

        // Accuracy / UX / Weapons
        [SettingPropertyBool("Slight Random Spread (off = Pinpoint)", Order = 0)]
        [SettingPropertyGroup("Accuracy & UX")]
        public bool SlightRandomSpread { get; set; } = true;

        [SettingPropertyBool("Show warning when Extreme is active (player)", Order = 1)]
        [SettingPropertyGroup("Accuracy & UX")]
        public bool ShowExtremeWarning { get; set; } = true;

        [SettingPropertyBool("Multiply thrown weapons too (experimental)", Order = 2)]
        [SettingPropertyGroup("Weapons")]
        public bool MultiplyThrown { get; set; } = false;

        // Helpers
        public int EffectivePlayer =>
            PlayerUseExtreme ? PlayerExtremeMultiplier : PlayerMultiplier;

        public int EffectiveNpc =>
            NpcUseExtreme ? NpcExtremeMultiplier : NpcMultiplier;
    }
}
