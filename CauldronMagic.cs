using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CauldronOfChance
{
    public class CauldronMagic
    {
        public static int effectType = 0;

        #region properties
        #region setup
        GameLocation WizardHouse { get; set; }
        NPC Wizard { get; set; }
        StardewValley.Object resultingItem { get; set; }
        Random randomGenerator { get; set; }
        double playerLuck { get; set; }
        #endregion setup

        #region ingredients
        public Item ingredient1 { get; set; }
        public Item ingredient2 { get; set; }
        public Item ingredient3 { get; set; }
        #endregion ingredients

        #region chances
        #region special events
        public double butterflies { get; set; } = 0.01;
        public double boom { get; set; } = 0.01;
        #endregion special events

        #region special buffs
        public int garlicOil { get; set; } = 0;
        public int monsterMusk { get; set; } = 0;
        public int debuffImmunity { get; set; } = 0;
        #endregion special buffs

        #region recipes
        public int crafting { get; set; } = 0;
        public int cooking { get; set; } = 0;
        public int randomItem { get; set; } = 0;
        #endregion

        #region generic buffs
        public enum buffs
        {
            farmingBuff,
            miningBuff,
            fishingBuff,
            foragingBuff,
            attackBuff,
            defenseBuff,
            maxEnergyBuff,
            luckBuff,
            magneticRadiusBuff,
            speedBuff
        }

        #region old2
        //public List<int> buffList;
        //public enum buffs
        //{
        //    farmingBuff1,
        //    farmingBuff2,
        //    farmingBuff3,

        //    miningBuff1,
        //    miningBuff2,
        //    miningBuff3,

        //    fishingBuff1,
        //    fishingBuff2,
        //    fishingBuff3,

        //    foragingBuff1,
        //    foragingBuff2,
        //    foragingBuff3,

        //    attackBuff1,
        //    attackBuff2,
        //    attackBuff3,

        //    defenseBuff1,
        //    defenseBuff2,
        //    defenseBuff3,

        //    maxEnergyBuff1,
        //    maxEnergyBuff2,
        //    maxEnergyBuff3,

        //    luckBuff1,
        //    luckBuff2,
        //    luckBuff3,

        //    magneticRadiusBuff1,
        //    magneticRadiusBuff2,
        //    magneticRadiusBuff3,

        //    speedBuff1,
        //    speedBuff2,
        //    speedBuff3
        //}

        //public List<int> debuffList;
        //public enum debuffs
        //{
        //    farmingDebuff1,
        //    farmingDebuff2,
        //    farmingDebuff3,

        //    miningDebuff1,
        //    miningDebuff2,
        //    miningDebuff3,

        //    fishingDebuff1,
        //    fishingDebuff2,
        //    fishingDebuff3,

        //    foragingDebuff1,
        //    foragingDebuff2,
        //    foragingDebuff3,

        //    attackDebuff1,
        //    attackDebuff2,
        //    attackDebuff3,

        //    defenseDebuff1,
        //    defenseDebuff2,
        //    defenseDebuff3,

        //    maxEnergyDebuff1,
        //    maxEnergyDebuff2,
        //    maxEnergyDebuff3,

        //    luckDebuff1,
        //    luckDebuff2,
        //    luckDebuff3,

        //    magneticRadiusDebuff1,
        //    magneticRadiusDebuff2,
        //    magneticRadiusDebuff3,

        //    speedDebuff1,
        //    speedDebuff2,
        //    speedDebuff3
        //}
        #endregion old2
        #region old
        //public int farmingBuff1 { get; set; } = 0;
        //public int farmingBuff2 { get; set; } = 0;
        //public int farmingBuff3 { get; set; } = 0;
        //public int farmingDebuff1 { get; set; } = 0;
        //public int farmingDebuff2 { get; set; } = 0;
        //public int farmingDebuff3 { get; set; } = 0;

        //public int miningBuff1 { get; set; } = 0;
        //public int miningBuff2 { get; set; } = 0;
        //public int miningBuff3 { get; set; } = 0;
        //public int miningDebuff1 { get; set; } = 0;
        //public int miningDebuff2 { get; set; } = 0;
        //public int miningDebuff3 { get; set; } = 0;

        //public int fishingBuff1 { get; set; } = 0;
        //public int fishingBuff2 { get; set; } = 0;
        //public int fishingBuff3 { get; set; } = 0;
        //public int fishingDebuff1 { get; set; } = 0;
        //public int fishingDebuff2 { get; set; } = 0;
        //public int fishingDebuff3 { get; set; } = 0;

        //public int foragingBuff1 { get; set; } = 0;
        //public int foragingBuff2 { get; set; } = 0;
        //public int foragingBuff3 { get; set; } = 0;
        //public int foragingDebuff1 { get; set; } = 0;
        //public int foragingDebuff2 { get; set; } = 0;
        //public int foragingDebuff3 { get; set; } = 0;

        //public int attackBuff1 { get; set; } = 0;
        //public int attackBuff2 { get; set; } = 0;
        //public int attackBuff3 { get; set; } = 0;
        //public int attackDebuff1 { get; set; } = 0;
        //public int attackDebuff2 { get; set; } = 0;
        //public int attackDebuff3 { get; set; } = 0;

        //public int defenseBuff1 { get; set; } = 0;
        //public int defenseBuff2 { get; set; } = 0;
        //public int defenseBuff3 { get; set; } = 0;
        //public int defenseDebuff1 { get; set; } = 0;
        //public int defenseDebuff2 { get; set; } = 0;
        //public int defenseDebuff3 { get; set; } = 0;

        //public int maxEnergyBuff1 { get; set; } = 0;
        //public int maxEnergyBuff2 { get; set; } = 0;
        //public int maxEnergyBuff3 { get; set; } = 0;
        //public int maxEnergyDebuff1 { get; set; } = 0;
        //public int maxEnergyDebuff2 { get; set; } = 0;
        //public int maxEnergyDebuff3 { get; set; } = 0;

        //public int luckBuff1 { get; set; } = 0;
        //public int luckBuff2 { get; set; } = 0;
        //public int luckBuff3 { get; set; } = 0;
        //public int luckDebuff1 { get; set; } = 0;
        //public int luckDebuff2 { get; set; } = 0;
        //public int luckDebuff3 { get; set; } = 0;

        //public int magneticRadiusBuff1 { get; set; } = 0;
        //public int magneticRadiusBuff2 { get; set; } = 0;
        //public int magneticRadiusBuff3 { get; set; } = 0;
        //public int magneticRadiusDebuff1 { get; set; } = 0;
        //public int magneticRadiusDebuff2 { get; set; } = 0;
        //public int magneticRadiusDebuff3 { get; set; } = 0;

        //public int speedBuff1 { get; set; } = 0;
        //public int speedBuff2 { get; set; } = 0;
        //public int speedBuff3 { get; set; } = 0;
        //public int speedDebuff1 { get; set; } = 0;
        //public int speedDebuff2 { get; set; } = 0;
        //public int speedDebuff3 { get; set; } = 0;
        #endregion old
        #endregion generic buffs
        #endregion chances
        #endregion properties

        #region constructors
        public CauldronMagic(Item ingredient1, Item ingredient2, Item ingredient3)
        {
            this.ingredient1 = ingredient1;
            this.ingredient2 = ingredient2;
            this.ingredient3 = ingredient3;

            WizardHouse = Game1.locations.Where(x => x.Name.Equals("WizardHouse")).First();
            Wizard = Game1.getCharacterFromName("Wizard");
            resultingItem = new StardewValley.Object();
            randomGenerator = new Random();
            playerLuck = Game1.player.DailyLuck;

            //buffList = new List<int>();
            //foreach(int buffIndex in Enum.GetValues(typeof(buffs)))
            //{
            //    buffList.Add(0);
            //}
            //debuffList = new List<int>();
            //foreach(int debuffIndex in Enum.GetValues(typeof(debuffs)))
            //{
            //    debuffList.Add(0);
            //}

            UseCauldron();
        }
        #endregion constructors

        #region functions
        public void UseCauldron()
        {
            #region determine effects
            addChances(ingredient1);
            addChances(ingredient2);
            addChances(ingredient3);
            findCombinations();
            #endregion determine effects

            #region determine actual effect
            determineResult();
            #endregion determine actual effect

            #region take effect
            //Drink
            if (effectType == 1)
            {
                Game1.player.canMove = false;
                ObjectPatches.IModHelper.Reflection.GetMethod(Game1.player, "performDrinkAnimation").Invoke(resultingItem);

                DelayedAction.delayedBehavior onDrink = delegate
                {
                    Game1.player.canMove = true;
                    Game1.activeClickableMenu = new DialogueBox("You drink the potion and... suddenly you think that Expl0 is the coolest!!"); //TODO: Better text
                };

                DelayedAction.functionAfterDelay(onDrink, 1000);
            }
            //Get item
            else if (effectType == 2)
            {
                resultingItem = new StardewValley.Object(373, 1);
                Game1.player.currentLocation.debris.Add(new Debris(resultingItem, new Microsoft.Xna.Framework.Vector2(3 * 64f, 20 * 64f)));

                Game1.player.canMove = false;
                DelayedAction.delayedBehavior onDrop = delegate
                {
                    Game1.player.canMove = true;
                    Game1.activeClickableMenu = new DialogueBox($"A {resultingItem.DisplayName} emerges from the depths of the Cauldron.");
                };
                DelayedAction.functionAfterDelay(onDrop, 100);
            }
            //Butterflies!!!
            else if (effectType == 3)
            {
                WizardHouse.critters = new List<Critter>();
                for(int counter = 0; counter < 7; counter++)
                {
                    WizardHouse.critters.Add(new Butterfly(new Microsoft.Xna.Framework.Vector2(4, 20)));
                }
                Game1.player.changeFriendship(222, Wizard);
                Wizard.doEmote(NPC.heartEmote);

                Game1.player.canMove = false;
                DelayedAction.delayedBehavior onButterflies = delegate
                {
                    Game1.player.canMove = true;
                    Game1.activeClickableMenu = new DialogueBox("A pleasent smell fills the air...");
                };
                DelayedAction.functionAfterDelay(onButterflies, 100);
            }
            //BOOOM!!!!
            else
            {
                WizardHouse.explode(new Microsoft.Xna.Framework.Vector2(3, 20), 3, Game1.player);
                Game1.player.changeFriendship(-222, Wizard);
                Wizard.doEmote(NPC.angryEmote);

                Game1.player.canMove = false;
                DelayedAction.delayedBehavior onExplosion = delegate
                {
                    Game1.player.canMove = true;
                    Game1.activeClickableMenu = new DialogueBox("A vile stench fills the air...");
                };
                DelayedAction.functionAfterDelay(onExplosion, 100);
            }
            #endregion take effect

            //set player as "has used today"
        }

        #region helper
        #region recipes
        //public Item getRecipe()
        //{

        //}

        //public bool isRecipe()
        //{
        //    return getRecipe() != null;
        //}
        #endregion recipes

        #region chance values
        public void addChances(Item ingredient)
        {

        }
        public void findCombinations()
        {

        }
        #endregion chance values

        #region chance determination
        public void determineResult()
        {
            double butterboomChance = randomGenerator.NextDouble();
            //Chance for butterflies
            if (butterboomChance > 1 - butterflies - playerLuck)
            {
                effectType = 3;
                return;
            }
            //Chance for boom
            if (butterboomChance < boom - playerLuck)
            {
                effectType = 4;
                return;
            }

            //Check for crafting recipes (Drop)

            //Check for cooking recipes (Drink)

            //Check for other item drops? (Drop)

            //Check for other buffs (Drink)
            int buffChance = (int)(getBuffChance() + getBuffChance() * playerLuck);
            int debuffChance = (int)(getDebuffChance() - getDebuffChance() * playerLuck);
            int buffRandom = randomGenerator.Next(1, buffChance + debuffChance);
            if (buffRandom <= buffChance)
            {
                //Buff

            }
            else
            {
                //Debuff

            }
        }

        public int getBuffChance()
        {
            int buffChance = 0;

            foreach (int buffIndex in Enum.GetValues(typeof(buffs)))
            {
                buffChance += buffList[buffIndex];
            }

            return buffChance;
        }

        public int getDebuffChance()
        {
            int debuffChance = 0;

            foreach (int debuffIndex in Enum.GetValues(typeof(debuffs)))
            {
                debuffChance += debuffList[debuffIndex];
            }

            return debuffChance;
        }
        #endregion chance determination
        #endregion helper
        #endregion functions
    }
}