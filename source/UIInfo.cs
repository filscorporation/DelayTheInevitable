using Steel;

namespace SteelCustom
{
    public class UIInfo : ScriptComponent
    {
        public string TooltipInfo;
        private Entity tooltip;

        public override void OnMouseEnterUI()
        {
            float height = Math.Clamp(TooltipInfo.Length / 24 * 40, 40, 800);
            tooltip = UITooltip.ShowTooltip(TooltipInfo, height);
        }

        public override void OnMouseExitUI()
        {
            UITooltip.HideTooltip(tooltip);
        }
    }
}