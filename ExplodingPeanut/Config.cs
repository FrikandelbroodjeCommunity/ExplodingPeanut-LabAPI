using System.ComponentModel;

namespace ExplodingPeanut;

public class Config
{
    [Description("The chance SCP-173 will explode when it dies (0 = never, 100 = always)")]
    public float ExplodeChance { get; set; } = 100;

    [Description("The fuse time the grenade on SCP-173 will be spawned with")]
    public float FuseTime { get; set; } = 0f;
}