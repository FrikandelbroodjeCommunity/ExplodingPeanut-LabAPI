using System;
using InventorySystem;
using InventorySystem.Items.ThrowableProjectiles;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using Mirror;
using PlayerRoles;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;
using ThrowableItem = InventorySystem.Items.ThrowableProjectiles.ThrowableItem;
using TimedGrenadePickup = InventorySystem.Items.ThrowableProjectiles.TimedGrenadePickup;

namespace ExplodingPeanut;

public class Plugin : Plugin<Config>
{
    public override string Name => "ExplodingPeanut";
    public override string Description => "Peanut will explode when killed ";
    public override string Author => "gamendegamer321";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    private static readonly Random Random = new();

    private static ThrowableItem Template
    {
        get
        {
            if (_template == null)
            {
                InventoryItemLoader.TryGetItem(ItemType.GrenadeHE, out _template);
            }

            return _template;
        }
    }

    private static ThrowableItem _template;

    public override void Enable()
    {
        PlayerEvents.Death += OnPlayerDied;
    }

    public override void Disable()
    {
        PlayerEvents.Death -= OnPlayerDied;
    }

    private void OnPlayerDied(PlayerDeathEventArgs ev)
    {
        if (ev.OldRole != RoleTypeId.Scp173 || Random.Next(100) >= Config.ExplodeChance || Template == null) return;

        var grenade = Object.Instantiate(Template.Projectile, ev.OldPosition, Quaternion.identity);
        if (grenade is TimeGrenade time)
        {
            time._fuseTime = Config.FuseTime;
        }

        NetworkServer.Spawn(grenade.gameObject);
        grenade.ServerActivate();
    }
}