using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using System.Linq;

namespace SonicShoes
{
    public class ModEntry : Mod
    {
        private static IDynamicGameAssetsApi apiDGA;
        private readonly int BuffUniqueID = 9999999;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += this.OnUpdated;
            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        }

        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            apiDGA = Helper.ModRegistry.GetApi<IDynamicGameAssetsApi>("spacechase0.DynamicGameAssets");

        }
        public void OnUpdated(object sender, UpdateTickedEventArgs e)
        {
            if (Context.IsWorldReady && apiDGA?.GetDGAItemId(Game1.player?.boots?.Value) == "lilico.SonicShoesDGA/SonicShoes")
            {
                //ignore in cutscenes
                if (Game1.eventUp || !Context.IsWorldReady)
                    return;

                // ignore if walking
                bool running = Game1.player.running;
                bool runEnabled = running || Game1.options.autoRun != Game1.isOneOfTheseKeysDown(Game1.GetKeyboardState(), Game1.options.runButton); // auto-run enabled and not holding walk button, or disabled and holding run button
                if (!runEnabled)
                    return;

                //this.Monitor.Log($"{Game1.player.Name} is wearing the Sonic Shoes. Activating buff...", LogLevel.Debug);
                int buffId = this.BuffUniqueID + 5;
                Buff buff = Game1.buffsDisplay.otherBuffs.FirstOrDefault(p => p.which == buffId);
                if (buff == null)
                {
                    //this.Monitor.Log($"{Game1.player.Name} has no current buffs...", LogLevel.Debug);
                    Game1.buffsDisplay.addOtherBuff(
                        buff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, speed: 3, 0, 0, minutesDuration: 1, source: "Sonic Shoes", displaySource: "Sonic Shoes") { which = buffId }
                        );
                }
                buff.millisecondsDuration = 50;
            }
        }
    }
}
