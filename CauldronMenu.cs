using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Menus.InventoryMenu;
using static StardewValley.Menus.ItemGrabMenu;

namespace CauldronOfChance
{
    public class CauldronMenu : MenuWithInventory
    {
        public ClickableTextureComponent cauldronSlot1;
        public ClickableTextureComponent cauldronSlot2;
        public ClickableTextureComponent cauldronSlot3;

        private Item _ingredient1;
        private Item _ingredient2;
        private Item _ingredient3;

        public Item ingredient1
        {
            get
            {
                return _ingredient1;
            }
            set
            {
                _ingredient1 = value;
                checkForOkButton();
            }
        }
        public Item ingredient2
        {
            get
            {
                return _ingredient2;
            }
            set
            {
                _ingredient2 = value;
                checkForOkButton();
            }
        }
        public Item ingredient3
        {
            get
            {
                return _ingredient3;
            }
            set
            {
                _ingredient3 = value;
                checkForOkButton();
            }
        }

        public bool drawBackGround = true;
        public string message = "Add items into the Cauldron...";

        public InventoryMenu ItemsToGrabMenu { get; set; }
        public highlightThisItem highlightMethod { get; set; }

        //Animation on select? Could be cool
        private TemporaryAnimatedSprite poof { get; set; }
        //Needed to fill out empty slots...?
        protected List<TransferredItemSprite> _transferredItemSprites { get; set; } = new List<TransferredItemSprite>();

        public CauldronMenu()
            : base(okButton : false, trashCan : false)
        {
            //int xPositionOnScreen = base.xPositionOnScreen + 32;
            int xPositionOnScreen = base.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2;
            //int yPositionOnScreen = base.yPositionOnScreen;
            int yPositionOnScreen = base.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + 192 - 16;

            highlightMethod = highlightCauldronItems;

            this.ItemsToGrabMenu = new InventoryMenu(xPositionOnScreen, yPositionOnScreen, playerInventory: false, null, highlightMethod);
            ItemsToGrabMenu.highlightMethod = this.highlightMethod; //TODO: Why this not working? (Also not working for highlight non -> error in my draw method?)

            this.cauldronSlot1 = new ClickableTextureComponent("", new Rectangle(base.xPositionOnScreen + base.width / 2 - 48 - 92, base.yPositionOnScreen + base.height / 2 - 80 - 64, 96, 96), "", "", Game1.mouseCursors, new Rectangle(293, 360, 24, 24), 4f)
            {
                myID = 12598,
                region = 12598
            };
            this.cauldronSlot2 = new ClickableTextureComponent("", new Rectangle(base.xPositionOnScreen + base.width / 2 - 48, base.yPositionOnScreen + base.height / 2 - 80 - 64, 96, 96), "", "", Game1.mouseCursors, new Rectangle(293, 360, 24, 24), 4f)
            {
                myID = 12599,
                region = 12599
            };
            this.cauldronSlot3 = new ClickableTextureComponent("", new Rectangle(base.xPositionOnScreen + base.width / 2 - 48 + 92, base.yPositionOnScreen + base.height / 2 - 80 - 64, 96, 96), "", "", Game1.mouseCursors, new Rectangle(293, 360, 24, 24), 4f)
            {
                myID = 12600,
                region = 12600
            };

            exitFunction = delegate
            {
                cleanupCauldron();
            };
        }

        public override void draw(SpriteBatch b) //TODO: Rename
        {
            if (this.drawBackGround)
            {
                b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.uiViewport.Width, Game1.uiViewport.Height), Color.Black * 0.5f);
            }
            base.draw(b, drawUpperPortion: false, drawDescriptionArea: false);
            Game1.drawDialogueBox(Game1.uiViewport.Width / 2, this.ItemsToGrabMenu.yPositionOnScreen + this.ItemsToGrabMenu.height / 2, speaker: false, drawOnlyBox: false, this.message);

            this.cauldronSlot1.draw(b);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot1.bounds.X + -8, this.cauldronSlot1.bounds.Bottom - 100), new Rectangle(325, 448, 5, 14), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot1.bounds.X + 84, this.cauldronSlot1.bounds.Bottom - 100), new Rectangle(325, 448, 5, 14), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot1.bounds.X + -8, this.cauldronSlot1.bounds.Bottom - 44), new Rectangle(325, 452, 5, 13), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot1.bounds.X + 84, this.cauldronSlot1.bounds.Bottom - 44), new Rectangle(325, 452, 5, 13), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            if (ingredient1 != null)
            {
                ingredient1.drawInMenu(b, new Vector2(this.cauldronSlot1.bounds.X + 16, this.cauldronSlot1.bounds.Y + 16), 1f);
            }

            this.cauldronSlot2.draw(b);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot2.bounds.X + -8, this.cauldronSlot2.bounds.Bottom - 100), new Rectangle(325, 448, 5, 14), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot2.bounds.X + 84, this.cauldronSlot2.bounds.Bottom - 100), new Rectangle(325, 448, 5, 14), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot2.bounds.X + -8, this.cauldronSlot2.bounds.Bottom - 44), new Rectangle(325, 452, 5, 13), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot2.bounds.X + 84, this.cauldronSlot2.bounds.Bottom - 44), new Rectangle(325, 452, 5, 13), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            if (ingredient2 != null)
            {
                ingredient2.drawInMenu(b, new Vector2(this.cauldronSlot2.bounds.X + 16, this.cauldronSlot2.bounds.Y + 16), 1f);
            }

            this.cauldronSlot3.draw(b);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot3.bounds.X + -8, this.cauldronSlot3.bounds.Bottom - 100), new Rectangle(325, 448, 5, 14), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot3.bounds.X + 84, this.cauldronSlot3.bounds.Bottom - 100), new Rectangle(325, 448, 5, 14), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot3.bounds.X + -8, this.cauldronSlot3.bounds.Bottom - 44), new Rectangle(325, 452, 5, 13), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(this.cauldronSlot3.bounds.X + 84, this.cauldronSlot3.bounds.Bottom - 44), new Rectangle(325, 452, 5, 13), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            if (ingredient3 != null)
            {
                ingredient3.drawInMenu(b, new Vector2(this.cauldronSlot3.bounds.X + 16, this.cauldronSlot3.bounds.Y + 16), 1f);
            }

            //Hover item stuff?

            Game1.mouseCursorTransparency = 1f;
            base.drawMouse(b);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            #region add to slots
            if (this.isWithinBounds(x, y) && (ingredient1 == null || ingredient2 == null || ingredient3 == null))
            {
                Item selectedItem = null;
                //TODO: Get item & remove 1 from inventory

                foreach (ClickableComponent item in ItemsToGrabMenu.inventory)
                {
                    int slotNumber = Convert.ToInt32(item.name);
                    if (!item.containsPoint(x, y))
                    {
                        continue;
                    }
                    if(slotNumber >= Game1.player.Items.Count
                        || (Game1.player.Items[slotNumber] != null && !highlightMethod(Game1.player.Items[slotNumber])) //TODO: Own hightlight method
                        || slotNumber >= Game1.player.Items.Count
                        || Game1.player.Items[slotNumber] == null
                        //|| Game1.player.Items[slotNumber] is Tool //TODO: needed with hightlight method?
                        )
                    {
                        continue;
                    }

                    selectedItem =  Game1.player.Items[slotNumber].getOne();

                    if (Game1.player.Items[slotNumber].Stack > 1 
                        //&& Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[1]
                        //    {
                        //            new InputButton(Keys.LeftShift)
                        //    })
                        )
                    {
                        //selectedItem.Stack = (int)Math.Ceiling((double)Game1.player.Items[slotNumber].Stack / 2.0);
                        //Game1.player.Items[slotNumber].Stack = Game1.player.Items[slotNumber].Stack / 2;
                        selectedItem.Stack = 1;
                        Game1.player.Items[slotNumber].Stack = Game1.player.Items[slotNumber].Stack -1;
                    }
                    else if (Game1.player.Items[slotNumber].Stack == 1)
                    {
                        Game1.player.Items[slotNumber] = null;
                    }
                    else
                    {
                        Game1.player.Items[slotNumber].Stack--;
                    }
                    if (Game1.player.Items[slotNumber] != null && Game1.player.Items[slotNumber].Stack <= 0)
                    {
                        Game1.player.Items[slotNumber] = null;
                    }
                    if (playSound)
                    {
                        Game1.playSound(ItemsToGrabMenu.moveItemSound);
                    }
                    break;
                }

                if (ingredient1 == null)
                {
                    ingredient1 = selectedItem;
                }
                else if (ingredient2 == null)
                {
                    ingredient2 = selectedItem;
                }
                else if (ingredient3 == null)
                {
                    ingredient3 = selectedItem;
                }

                //??????????????????
                if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is ItemGrabMenu)
                {
                    //(Game1.activeClickableMenu as ItemGrabMenu).setSourceItem(this.sourceItem); //TODO: Just commented this out without thinkin
                    if (Game1.options.SnappyMenus)
                    {
                        (Game1.activeClickableMenu as ItemGrabMenu).currentlySnappedComponent = base.currentlySnappedComponent;
                        (Game1.activeClickableMenu as ItemGrabMenu).snapCursorToCurrentSnappedComponent();
                    }
                }
            }
            #endregion add to slots

            #region return from slots
            if (this.cauldronSlot1.containsPoint(x, y))
            {
                if (ingredient1 == null)
                {
                    return;
                }
                if (Game1.player.addItemToInventoryBool(ingredient1))
                {
                    Game1.playSound("coin");
                    if (Game1.player.ActiveObject != null)
                    {
                        //???
                        Game1.player.showCarrying();
                        Game1.player.Halt();
                    }
                }
                else
                {
                    Game1.player.currentLocation.debris.Add(new Debris(ingredient1, Game1.player.position));
                }
                ingredient1 = null;
                orderIngredients();
                return;
            }
            if (this.cauldronSlot2.containsPoint(x, y))
            {
                if (ingredient2 == null)
                {
                    return;
                }
                if (Game1.player.addItemToInventoryBool(ingredient2))
                {
                    Game1.playSound("coin");
                    if (Game1.player.ActiveObject != null)
                    {
                        //???
                        Game1.player.showCarrying();
                        Game1.player.Halt();
                    }
                }
                else
                {
                    Game1.player.currentLocation.debris.Add(new Debris(ingredient2, Game1.player.position));
                }
                ingredient2 = null;
                orderIngredients();
                return;
            }
            if (this.cauldronSlot3.containsPoint(x, y))
            {
                if (ingredient3 == null)
                {
                    return;
                }
                if (Game1.player.addItemToInventoryBool(ingredient3))
                {
                    Game1.playSound("coin");
                    if (Game1.player.ActiveObject != null)
                    {
                        //???
                        Game1.player.showCarrying();
                        Game1.player.Halt();
                    }
                }
                else
                {
                    Game1.player.currentLocation.debris.Add(new Debris(ingredient3, Game1.player.position));
                }
                ingredient3 = null;
                orderIngredients();
                return;
            }
            #endregion return from slots

            if(this.okButton != null && this.okButton.containsPoint(x, y))
            {
                if(ingredient1 != null || ingredient2 != null || ingredient3 != null)
                {
                    using (CauldronMagic cauldronMagic = new CauldronMagic())
                    {
                        cauldronMagic.UseCauldron(ingredient1, ingredient2, ingredient3);
                    }
                    Game1.exitActiveMenu();
                }
                else
                {
                    //TODO: emergency exit?
                }
            }

            #region copied from
            //if (this.shippingBin && this.lastShippedHolder.containsPoint(x, y))
            //{
            //    if (Game1.getFarm().lastItemShipped == null)
            //    {
            //        return;
            //    }
            //    Game1.getFarm().getShippingBin(Game1.player).Remove(Game1.getFarm().lastItemShipped);
            //    if (Game1.player.addItemToInventoryBool(Game1.getFarm().lastItemShipped))
            //    {
            //        Game1.playSound("coin");
            //        Game1.getFarm().lastItemShipped = null;
            //        if (Game1.player.ActiveObject != null)
            //        {
            //            Game1.player.showCarrying();
            //            Game1.player.Halt();
            //        }
            //    }
            //    else
            //    {
            //        Game1.getFarm().getShippingBin(Game1.player).Add(Game1.getFarm().lastItemShipped);
            //    }
            //    return;
            //}

            //else if ((this.reverseGrab || this.behaviorFunction != null) && this.isWithinBounds(x, y))
            //{
            //    this.behaviorFunction(base.heldItem, Game1.player);
            //    if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is ItemGrabMenu)
            //    {
            //        (Game1.activeClickableMenu as ItemGrabMenu).setSourceItem(this.sourceItem);
            //        if (Game1.options.SnappyMenus)
            //        {
            //            (Game1.activeClickableMenu as ItemGrabMenu).currentlySnappedComponent = base.currentlySnappedComponent;
            //            (Game1.activeClickableMenu as ItemGrabMenu).snapCursorToCurrentSnappedComponent();
            //        }
            //    }
            //    if (this.destroyItemOnClick)
            //    {
            //        base.heldItem = null;
            //        return;
            //    }
            //}
            #endregion copied from
        }

        public void checkForOkButton()
        {
            if(ingredient1 != null && ingredient2 != null && ingredient3 != null)
            {
                this.okButton = new ClickableTextureComponent(new Rectangle(base.xPositionOnScreen + base.width + 4, base.yPositionOnScreen + base.height - 192 - IClickableMenu.borderWidth, 64, 64), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), 1f)
                {
                    myID = 4857,
                    upNeighborID = 5948,
                    leftNeighborID = 12
                };
            }
            else
            {
                this.okButton = null;
            }
        }

        public void orderIngredients()
        {
            if (ingredient1 == null && ingredient2 != null)
            {
                ingredient1 = ingredient2;
                ingredient2 = null;
                orderIngredients();
            }
            if (ingredient2 == null && ingredient3 != null)
            {
                ingredient2 = ingredient3;
                ingredient3 = null;
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            this.receiveLeftClick(x, y, playSound);
        }

        public override void emergencyShutDown()
        {
            cleanupCauldron();

            base.emergencyShutDown();
        }

        public void cleanupCauldron()
        {
            #region return items from the cauldron
            if (ingredient1 != null)
            {
                if (Game1.player.addItemToInventoryBool(ingredient1))
                {
                    if (Game1.player.ActiveObject != null)
                    {
                        //???
                        Game1.player.showCarrying();
                        Game1.player.Halt();
                    }
                }
                else
                {
                    Game1.player.currentLocation.debris.Add(new Debris(ingredient1, Game1.player.position));
                }
                ingredient1 = null;
            }
            if (ingredient2 != null)
            {
                if (Game1.player.addItemToInventoryBool(ingredient2))
                {
                    if (Game1.player.ActiveObject != null)
                    {
                        //???
                        Game1.player.showCarrying();
                        Game1.player.Halt();
                    }
                }
                else
                {
                    Game1.player.currentLocation.debris.Add(new Debris(ingredient2, Game1.player.position));
                }
                ingredient2 = null;
            }
            if (ingredient3 != null)
            {
                if (Game1.player.addItemToInventoryBool(ingredient3))
                {
                    if (Game1.player.ActiveObject != null)
                    {
                        //???
                        Game1.player.showCarrying();
                        Game1.player.Halt();
                    }
                }
                else
                {
                    Game1.player.currentLocation.debris.Add(new Debris(ingredient3, Game1.player.position));
                }
                ingredient3 = null;
            }
            #endregion return items from the cauldron
        }
        public static bool highlightCauldronItems(Item item)
        {
            if (item is Tool
                || item is Boots //Also stuff with hats for recipes?
                || item is Clothing //Clothing could be interesting tho: Change colours?
                || item is Hat //And hats?
                || item is Ring //Ring could be interesting tho: Chance for enchantment
                || item is SpecialItem
                || item.isPlaceable()
                )
            {
                return false;
            }
            return true;
        }
    }
}
