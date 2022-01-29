using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;

namespace CauldronOfChance
{
    class ObjectPatches
    {
        public static IMonitor IMonitor;
        public static IModHelper IModHelper;

        public static void Initialize(IMonitor IMonitor, IModHelper IHelper)
        {
            ObjectPatches.IMonitor = IMonitor;
            ObjectPatches.IModHelper = IHelper;
        }

        public static bool checkAction_Prefix(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
        {
            try
            {
                if (who.currentLocation.Name.Equals("WizardHouse"))
                {
                    string property = Game1.currentLocation.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Action", "Buildings");
                    if (property != null && property.Equals("CauldronOfChance"))
                    {
                        //TODO: Check that player hasnt already used cauldron today
                        //Game1.activeClickableMenu = new ItemGrabMenu(null, reverseGrab: true, showReceivingMenu: false, Utility.highlightLuauSoupItems, clickToAddItemToLuauSoup, Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1719"), null, snapToBottom: false, canBeExitedWithKey: true, playRightClickSound: true, allowRightClick: true, showOrganizeButton: false, 0, null, -1, this);
                        Game1.activeClickableMenu = new CauldronMenu();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                IMonitor.Log($"Failed in {nameof(checkAction_Prefix)}:\n{ex}", LogLevel.Error);
                return true;
            }
        }
    }
}
