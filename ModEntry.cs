using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Linq;

namespace CauldronOfChance
{
    public class ModEntry : StardewModdingAPI.Mod
    {
        //TODO: Once per day
        public override void Entry(IModHelper IHelper)
        {
            #region Setup
            var csHarmony = new Harmony(this.ModManifest.UniqueID);

            ObjectPatches.Initialize(this.Monitor, IHelper);
            #endregion Setup

            #region Harmony Patches
            csHarmony.Patch(
               original: AccessTools.Method(typeof(GameLocation), "checkAction"),
               prefix: new HarmonyMethod(typeof(ObjectPatches), nameof(ObjectPatches.checkAction_Prefix))
            );
            #endregion Harmony Patches

            #region Events
            IHelper.Events.GameLoop.SaveLoaded += onSaveLoaded;
            IHelper.Events.Input.ButtonPressed += onButtonPressed;
            #endregion Events
        }

        private void onButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (Game1.currentLocation != null && Game1.currentLocation.Name.Equals("WizardHouse"))
            {
                var x = Game1.currentLocation;
            }

            if (e.Button.IsActionButton())
            {
                //Game1.addHUDMessage(new HUDMessage("Tile: X: " + e.Cursor.Tile.X + "Y: " + e.Cursor.Tile.Y));
                //Game1.addHUDMessage(new HUDMessage("ScreenPixels: X: " + e.Cursor.ScreenPixels.X + "Y: " + e.Cursor.ScreenPixels.Y));
                //Game1.addHUDMessage(new HUDMessage("AbsolutePixels: X: " + e.Cursor.AbsolutePixels.X + "Y: " + e.Cursor.AbsolutePixels.Y));
                //Game1.addHUDMessage(new HUDMessage(Game1.player.DailyLuck.ToString()));
            }
        }

        private void onSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            try
            {
                GameLocation WizardHouse = Game1.locations.Where(x => x.Name.Equals("WizardHouse")).First();
                WizardHouse.setTileProperty(2, 20, "Buildings", "Action", "CauldronOfChance");
                WizardHouse.setTileProperty(3, 20, "Buildings", "Action", "CauldronOfChance");
                WizardHouse.setTileProperty(4, 20, "Buildings", "Action", "CauldronOfChance");
                WizardHouse.setTileProperty(2, 21, "Buildings", "Action", "CauldronOfChance");
                WizardHouse.setTileProperty(3, 21, "Buildings", "Action", "CauldronOfChance");
                WizardHouse.setTileProperty(4, 21, "Buildings", "Action", "CauldronOfChance");
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"Could not add TileProperties to the Wizards Cauldron:\n{ex}", LogLevel.Error);
            }
        }
    }
}
