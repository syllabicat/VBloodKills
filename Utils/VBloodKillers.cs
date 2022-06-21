using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wetstone.API;
using ProjectM;

namespace VBloodKills.Utils
{
    public class VBloodKillers
    {
        public static Dictionary<int, HashSet<string>> vbloodKills = new();

        public static void AddKiller(int vblood, string killerCharacterName)
        {
            if (!vbloodKills.ContainsKey(vblood))
            {
                vbloodKills[vblood] = new HashSet<string>();
            }
            vbloodKills[vblood].Add(killerCharacterName);
        }
        public static void RemoveKillers(int vblood)
        {
            vbloodKills[vblood] = new HashSet<string>();
        }

        public static List<string> GetKillers(int vblood)
        {
            return vbloodKills[vblood].ToList();
        }

        public static void SendAnnouncementMessage(int vblood)
        {
            var message = GetAnnouncementMessage(vblood);
            if (message != null)
            {
                ServerChatUtils.SendSystemMessageToAllClients(VWorld.Server.EntityManager, message);
                RemoveKillers(vblood);
            }
        }

        public static string GetAnnouncementMessage(int vblood)
        {
            var killers = GetKillers(vblood);
            var vbloodLabel = VBloodLabels.PrefabToLabel[vblood];
            var sbKillersLabel = new StringBuilder();
            if (killers.Count == 0) return null;
            if (killers.Count == 1)
            {
                sbKillersLabel.Append(ChatColors.Yellow(killers[0]));
            }
            if (killers.Count == 2)
            {
                sbKillersLabel.Append($"{ChatColors.Yellow(killers[0])} and {ChatColors.Yellow(killers[1])}");
            }
            if (killers.Count > 2)
            {
                for (int i = 0; i < killers.Count; i++)
                {
                    if (i == killers.Count - 1)
                    {
                        sbKillersLabel.Append($"and {ChatColors.Yellow(killers[i])}");
                    }
                    else
                    {
                        sbKillersLabel.Append($"{ChatColors.Yellow(killers[i])}, ");
                    }
                }
            }
            return ChatColors.Green($"Congratulations to {sbKillersLabel} for beating {ChatColors.Red(vbloodLabel)}!");
        }
    }
}
