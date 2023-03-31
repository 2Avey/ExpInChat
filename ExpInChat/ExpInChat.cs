using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace ExpInChat
{
    public class ExpInChat : RocketPlugin
    {
        public static ExpInChat Instance;

        public Dictionary <CSteamID, uint> Experiances = new Dictionary <CSteamID, uint> ();

        protected override void Load()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerUpdateExperience += onPlayerUpdateExperience;
            U.Events.OnPlayerConnected += onPlayerConnected;
        }
        protected override void Unload()
        {
            Instance = null;
            UnturnedPlayerEvents.OnPlayerUpdateExperience -= onPlayerUpdateExperience;
            U.Events.OnPlayerConnected -= onPlayerConnected;
        }


        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"give_exp_player","Получено {0} опыта. Всего {1}"},
            {"lose_exp_player","Потеряно {0} опыта. Всего {1}"}
        };


        private void onPlayerUpdateExperience(UnturnedPlayer player, uint expirience)
        {
            if (Experiances.ContainsKey(player.CSteamID))
            {
                uint LastExperiances = Experiances[player.CSteamID];
                if (expirience > LastExperiances)
                {
                    uint GetExperiance = expirience - LastExperiances;
                    Experiances[player.CSteamID] = expirience;
                    UnturnedChat.Say(player, ExpInChat.Instance.Translate("give_exp_player", GetExperiance, expirience), Color.cyan);
                }
                else
                {
                    uint GetExperiance = LastExperiances - expirience;
                    Experiances[player.CSteamID] = expirience;
                    UnturnedChat.Say(player, ExpInChat.Instance.Translate("lose_exp_player", GetExperiance, expirience), Color.red);
                }
            }
        }
        private void onPlayerConnected (UnturnedPlayer player)
        {
            if (Experiances.ContainsKey(player.CSteamID) == false)
            {
                Experiances.Add(player.CSteamID, player.Experience);
            }
        }
    }
}