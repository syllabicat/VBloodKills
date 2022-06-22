using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using Wetstone.API;
using System.Collections.Generic;
using System;

namespace VBloodKills.Patches
{
    [HarmonyPatch]
    public class VBloodSystem_Patch
    {
        /**
         * Each player has its own VBloodConsumed event.
         * This delay is used to gather all the VBLoodConsumed events before sending the system message.
         */
        private const double SendMessageDelay = 2;
        private static bool checkKiller = false;
        private static Dictionary<int, DateTime> lastKillerUpdate = new();

        [HarmonyPatch(typeof(VBloodSystem), nameof(VBloodSystem.OnUpdate))]
        [HarmonyPrefix]
        public static void OnUpdate_Prefix(VBloodSystem __instance)
        {
            if (__instance._eventList.Length > 0)
            {
                foreach (var e in __instance._eventList)
                {
                    if (VWorld.Server.EntityManager.HasComponent<PlayerCharacter>(e.Target))
                    {
                        var player = VWorld.Server.EntityManager.GetComponentData<PlayerCharacter>(e.Target);
                        var user = VWorld.Server.EntityManager.GetComponentData<User>(player.UserEntity._Entity);
                        var vblood = e.Source.GuidHash;
                        Utils.VBloodKillers.AddKiller(e.Source.GuidHash, user.CharacterName.ToString());
                        lastKillerUpdate[vblood] = DateTime.Now;
                        checkKiller = true;
                    }
                }
            }
            else if (checkKiller)
            {
                var didSkip = false;
                foreach (KeyValuePair<int, DateTime> kvp in lastKillerUpdate)
                {
                    var lastUpdateTime = kvp.Value;
                    if (DateTime.Now - lastUpdateTime < TimeSpan.FromSeconds(SendMessageDelay))
                    {
                        didSkip = true;
                        continue;
                    }
                    Utils.VBloodKillers.SendAnnouncementMessage(kvp.Key);
                }
                checkKiller = didSkip;
            }
        }
    }
}
