using Exiled.API.Interfaces;
using System.ComponentModel;

namespace LookToMute
{
    public class Config : IConfig
    {
        [Description("Setting plugin enabled or not")]
        public bool IsEnabled { get; set; } = true;
        [Description("Setting debug enabled or not")]
        public bool Debug { get; set; } = false;
        [Description("Distance for mute player you're looking")]
        public float MaxDistance { get; set; } = 10;
        [Description("Radius of sphere. High value may cause incorrect behaviour as well as too low value")]
        public float Radius { get; set; } = 0.15f;
    }
}
