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
        public Ingredient ingredient1 { get; set; }
        public Ingredient ingredient2 { get; set; }
        public Ingredient ingredient3 { get; set; }
        #endregion ingredients

        #region chances
        #region recipes
        public int crafting { get; set; } = 0;
        public int cooking { get; set; } = 0;
        public int randomItem { get; set; } = 0;
        #endregion
        #endregion chances
        #endregion properties

        #region constructors
        public CauldronMagic(Item ingredient1, Item ingredient2, Item ingredient3)
        {
            this.ingredient1 = Cauldron.getIngredient(ingredient1);
            this.ingredient2 = Cauldron.getIngredient(ingredient2);
            this.ingredient3 = Cauldron.getIngredient(ingredient3);

            WizardHouse = Game1.locations.Where(x => x.Name.Equals("WizardHouse")).First();
            Wizard = Game1.getCharacterFromName("Wizard");
            resultingItem = new StardewValley.Object();
            randomGenerator = new Random();
            playerLuck = Game1.player.DailyLuck + getCauldronLuck();

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
            findCombinations();
            #endregion determine effects

            #region determine actual effect
            determineResult();
            #endregion determine actual effect

            //For buffs: energy *10, magnetic *32(?)

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
        public void findCombinations()
        {

        }
        #endregion chance values

        #region chance determination
        public void determineResult()
        {
            double butterboomChance = randomGenerator.NextDouble();
            //Chance for butterflies
            if (butterboomChance > 1 - getButterflies() - playerLuck)
            {
                effectType = 3;
                return;
            }
            //Chance for boom
            if (butterboomChance < getBoom() - playerLuck)
            {
                effectType = 4;
                return;
            }

            //Check for crafting recipes (Drop)
            if (false)
            {
                effectType = 2;
                return;
            }

            //Check for cooking recipes (Drink)
            if (false)
            {
                effectType = 1;
                return;
            }

            //Check for other item drops? (Drop)
            if (false)
            {
                effectType = 2;
                return;
            }

            //Check for other buffs (Drink)
            int buffChance = (int)(getBuffChance() + getBuffChance() * playerLuck);
            int debuffChance = (int)(getDebuffChance() - getDebuffChance() * playerLuck);

            if(buffChance < 1)
            {
                buffChance = 1;
            }
            if(debuffChance < 1)
            {
                debuffChance = 1;
            }

            int buffRandom = randomGenerator.Next(1, buffChance + debuffChance);

            int farming = 0;
            int fishing = 0;
            int mining = 0;
            int digging = 0;
            int luck = 0;
            int foraging = 0;
            int crafting = 0;
            int maxStamina = 0;
            int magneticRadius = 0;
            int speed = 0;
            int defense = 0;
            int attack = 0;

            int defaultBuff = -1;

            int minutesDuration = 20 * 60; //20 hours
            string source = "CauldronOfChance";
            string displaySource = "Cauldron";
            //TODO: Hints instead of buff display? ("You dont feel like fishing, you feel like farming, etc.),
            string description = "The magic of the Cauldron flows through you. "; //TODO: Change for debuff/special buffs?
            //string description = "You're feeling "; //TODO: Change for debuff/special buffs?

            effectType = 1;

            if (buffRandom <= buffChance)
            {
                //Buff
                int maxBuffs = getBuffChance();

                if (getDebuffChance() < 5)
                {
                    maxBuffs += Enum.GetValues(typeof(Ingredient.buffs)).Length;
                }

                int buffRandomCounter = randomGenerator.Next(1, maxBuffs);

                List<int> buffs = getAllBuffs();

                int finalBuffIndex = -1;

                foreach (int buffIndex in Enum.GetValues(typeof(Ingredient.buffs)))
                {
                    if (buffs[buffIndex] > 0)
                    {
                        buffRandomCounter -= buffs[buffIndex];

                        if(buffRandomCounter < 0)
                        {
                            finalBuffIndex = buffIndex;
                            break;
                        }
                    }
                }

                if(finalBuffIndex != -1)
                {
                    switch (finalBuffIndex)
                    {
                        case (int)Ingredient.buffs.garlicOil1: //TODO: Re-add 2 and 3 with the same effect for more chances / higher value tho?
                            //description = "You've started to smell... weird.";
                            description += "(Garlic Oil)";
                            defaultBuff = 23;
                            break;
                        case (int)Ingredient.buffs.debuffImmunity1:
                            //description = "You feel as if nothing can slow you down.";
                            description += "(Debuff Immunity)";
                            defaultBuff = 28;
                            break;
                        case (int)Ingredient.buffs.farmingBuff1:
                            description += "(Farming +1)";
                            farming = 1;
                            break;
                        case (int)Ingredient.buffs.farmingBuff2:
                            description += "(Farming +2)";
                            farming = 2;
                            break;
                        case (int)Ingredient.buffs.farmingBuff3:
                            description += "(Farming +3)";
                            farming = 3;
                            break;
                        case (int)Ingredient.buffs.miningBuff1:
                            description += "(Mining +1)";
                            mining = 1;
                            break;
                        case (int)Ingredient.buffs.miningBuff2:
                            description += "(Mining +2)";
                            mining = 2;
                            break;
                        case (int)Ingredient.buffs.miningBuff3:
                            description += "(Mining +3)";
                            mining = 3;
                            break;
                        case (int)Ingredient.buffs.fishingBuff1:
                            description += "(Fishing +1)";
                            fishing = 1;
                            break;
                        case (int)Ingredient.buffs.fishingBuff2:
                            description += "(Fishing +2)";
                            fishing = 2;
                            break;
                        case (int)Ingredient.buffs.fishingBuff3:
                            description += "(Fishing +3)";
                            fishing = 3;
                            break;
                        case (int)Ingredient.buffs.foragingBuff1:
                            description += "(Foraging +1)";
                            foraging = 1;
                            break;
                        case (int)Ingredient.buffs.foragingBuff2:
                            description += "(Foraging +2)";
                            foraging = 2;
                            break;
                        case (int)Ingredient.buffs.foragingBuff3:
                            description += "(Foraging +3)";
                            foraging = 3;
                            break;
                        case (int)Ingredient.buffs.attackBuff1:
                            description += "(Attack +1)";
                            attack = 1;
                            break;
                        case (int)Ingredient.buffs.attackBuff2:
                            description += "(Attack +2)";
                            attack = 2;
                            break;
                        case (int)Ingredient.buffs.attackBuff3:
                            description += "(Attack +3)";
                            attack = 3;
                            break;
                        case (int)Ingredient.buffs.defenseBuff1:
                            description += "(Defense +1)";
                            defense = 1;
                            break;
                        case (int)Ingredient.buffs.defenseBuff2:
                            description += "(Defense +2)";
                            defense = 2;
                            break;
                        case (int)Ingredient.buffs.defenseBuff3:
                            description += "(Defense +3)";
                            defense = 3;
                            break;
                        case (int)Ingredient.buffs.maxEnergyBuff1:
                            description += "(Max Stamina +10)";
                            maxStamina = 1 * 10;
                            break;
                        case (int)Ingredient.buffs.maxEnergyBuff2:
                            description += "(Max Stamina +20)";
                            maxStamina = 2 * 10;
                            break;
                        case (int)Ingredient.buffs.maxEnergyBuff3:
                            description += "(Max Stamina +30)";
                            maxStamina = 3 * 10;
                            break;
                        case (int)Ingredient.buffs.luckBuff1:
                            description += "(Luck +1)";
                            luck = 1;
                            break;
                        case (int)Ingredient.buffs.luckBuff2:
                            description += "(Luck +2)";
                            luck = 2;
                            break;
                        case (int)Ingredient.buffs.luckBuff3:
                            description += "(Luck +3)";
                            luck = 3;
                            break;
                        case (int)Ingredient.buffs.magneticRadiusBuff1: //TODO: TOO MUCH?
                            description += "(Magnetic Radius +32)";
                            magneticRadius = 1 * 32;
                            break;
                        case (int)Ingredient.buffs.magneticRadiusBuff2:
                            description += "(Magnetic Radius +64)";
                            magneticRadius = 2 * 32;
                            break;
                        case (int)Ingredient.buffs.magneticRadiusBuff3:
                            description += "(Magnetic Radius + 96)";
                            magneticRadius = 3 * 32;
                            break;
                        case (int)Ingredient.buffs.speedBuff1:
                            description += "(Speed +1)";
                            speed = 1;
                            break;
                        case (int)Ingredient.buffs.speedBuff2:
                            description += "(Speed +2)";
                            speed = 2;
                            break;
                        case (int)Ingredient.buffs.speedBuff3:
                            description += "(Speed +3)";
                            speed = 3;
                            break;
                    }
                }
            }
            else
            {
                //Debuff
                int maxDebuffs = getDebuffChance();

                if(getBuffChance() < 5)
                {
                    maxDebuffs += Enum.GetValues(typeof(Ingredient.debuffs)).Length;
                }

                int debuffRandomCounter = randomGenerator.Next(1, maxDebuffs);

                List<int> debuffs = getAllDebuffs();

                int finalDebuffIndex = -1;

                foreach (int debuffIndex in Enum.GetValues(typeof(Ingredient.debuffs)))
                {
                    if (debuffs[debuffIndex] > 0)
                    {
                        debuffRandomCounter -= debuffs[debuffIndex];

                        if (debuffRandomCounter < 0)
                        {
                            finalDebuffIndex = debuffIndex;
                            break;
                        }
                    }
                }

                if(finalDebuffIndex != -1)
                {
                    switch (finalDebuffIndex)
                    {
                        case (int)Ingredient.debuffs.monsterMusk1:
                            description += "(Monster Musk)";
                            defaultBuff = 24;
                            break;
                        case (int)Ingredient.debuffs.farmingDebuff1:
                            description += "(Farming -1)";
                            farming = -1;
                            break;
                        case (int)Ingredient.debuffs.farmingDebuff2:
                            description += "(Farming -2)";
                            farming = -2;
                            break;
                        case (int)Ingredient.debuffs.farmingDebuff3:
                            description += "(Farming -3)";
                            farming = -3;
                            break;
                        case (int)Ingredient.debuffs.miningDebuff1:
                            description += "(Mining -1)";
                            mining = -1;
                            break;
                        case (int)Ingredient.debuffs.miningDebuff2:
                            description += "(Mining -2)";
                            mining = -2;
                            break;
                        case (int)Ingredient.debuffs.miningDebuff3:
                            description += "(Mining -3)";
                            mining = -3;
                            break;
                        case (int)Ingredient.debuffs.fishingDebuff1:
                            description += "(Fishing -1)";
                            fishing = -1;
                            break;
                        case (int)Ingredient.debuffs.fishingDebuff2:
                            description += "(Fishing -2)";
                            fishing = -2;
                            break;
                        case (int)Ingredient.debuffs.fishingDebuff3:
                            description += "(Fishing -3)";
                            fishing = -3;
                            break;
                        case (int)Ingredient.debuffs.foragingDebuff1:
                            description += "(Foraging -1)";
                            foraging = -1;
                            break;
                        case (int)Ingredient.debuffs.foragingDebuff2:
                            description += "(Foraging -2)";
                            foraging = -2;
                            break;
                        case (int)Ingredient.debuffs.foragingDebuff3:
                            description += "(Foraging -3)";
                            foraging = -3;
                            break;
                        case (int)Ingredient.debuffs.attackDebuff1:
                            description += "(Attack -1)";
                            attack = -1;
                            break;
                        case (int)Ingredient.debuffs.attackDebuff2:
                            description += "(Attack -2)";
                            attack = -2;
                            break;
                        case (int)Ingredient.debuffs.attackDebuff3:
                            description += "(Attack -3)";
                            attack = -3;
                            break;
                        case (int)Ingredient.debuffs.defenseDebuff1:
                            description += "(Defense -1)";
                            defense = -1;
                            break;
                        case (int)Ingredient.debuffs.defenseDebuff2:
                            description += "(Defense -2)";
                            defense = -2;
                            break;
                        case (int)Ingredient.debuffs.defenseDebuff3:
                            description += "(Defense -3)";
                            defense = -3;
                            break;
                        case (int)Ingredient.debuffs.maxEnergyDebuff1:
                            description += "(Max Stamina -10)";
                            maxStamina = -1 * 10;
                            break;
                        case (int)Ingredient.debuffs.maxEnergyDebuff2:
                            description += "(Max Stamina -20)";
                            maxStamina = -2 * 10;
                            break;
                        case (int)Ingredient.debuffs.maxEnergyDebuff3:
                            description += "(Max Stamina -30)";
                            maxStamina = -3 * 10;
                            break;
                        case (int)Ingredient.debuffs.luckDebuff1:
                            description += "(Luck -1)";
                            luck = -1;
                            break;
                        case (int)Ingredient.debuffs.luckDebuff2:
                            description += "(Luck -2)";
                            luck = -2;
                            break;
                        case (int)Ingredient.debuffs.luckDebuff3:
                            description += "(Luck -3)";
                            luck = -3;
                            break;
                        case (int)Ingredient.debuffs.speedDebuff1:
                            description += "(Speed -1)";
                            speed = -1;
                            break;
                        case (int)Ingredient.debuffs.speedDebuff2:
                            description += "(Speed -2)";
                            speed = -2;
                            break;
                        case (int)Ingredient.debuffs.speedDebuff3:
                            description += "(Speed -3)";
                            speed = -3;
                            break;
                    }
                }
            }

            //Game1.buffsDisplay.addOtherBuff(
            //    new(0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        minutesDuration: 1,
            //        source: "<internal buff name>",
            //        displaySource: ModEntry.ModHelper.Translation.Get("<what should appear in game as the source>"))
            //    {
            //        which = myBuffId,
            //        sheetIndex = < index of the buff icon >,
            //        glow = <if player should glow set to something other than Color.White >,
            //        millisecondsDuration = < here you set the actual duration >,
            //        description = ModEntry.ModHelper.Translation.Get("<should appear in game as the description>")
            //    }
            //);

            Buff buff = new Buff(farming, fishing, mining, digging, luck, foraging, crafting, maxStamina, magneticRadius, speed, defense, attack, minutesDuration, source, displaySource)
            {
                sheetIndex = 17,
                //millisecondsDuration = < here you set the actual duration >,
                description = description
            };

            if(defaultBuff != -1)
            {
                buff.which = defaultBuff;
            }
            

            Game1.buffsDisplay.addOtherBuff(buff);
        }

        public int getBuffChance()
        {
            return ingredient1.getBuffChance() + ingredient2.getBuffChance() + ingredient3.getBuffChance();
        }

        public int getDebuffChance()
        {
            return ingredient1.getDebuffChance() + ingredient2.getDebuffChance() + ingredient3.getDebuffChance();
        }

        public double getButterflies()
        {
            return ingredient1.butterflies + ingredient2.butterflies + ingredient3.butterflies + 0.01;
        }

        public double getBoom()
        {
            return ingredient1.boom + ingredient2.boom + ingredient3.boom + 0.01;
        }

        public double getCauldronLuck()
        {
            return ingredient1.cauldronLuck + ingredient2.cauldronLuck + ingredient3.cauldronLuck;
        }

        public List<int> getAllBuffs()
        {
            int defaultAdder = 0;

            if(getDebuffChance() < 5)
            {
                defaultAdder = 1;
            }

            List<int> buffList = new List<int>();
            foreach (int buffIndex in Enum.GetValues(typeof(Ingredient.buffs)))
            {
                buffList.Add(ingredient1.buffList[buffIndex] + ingredient2.buffList[buffIndex] + ingredient3.buffList[buffIndex] + defaultAdder);
            }
            return buffList;
        }

        public List<int> getAllDebuffs()
        {
            int defaultAdder = 0;

            if (getBuffChance() < 5)
            {
                defaultAdder = 1;
            }

            List<int> debuffList = new List<int>();
            foreach (int debuffIndex in Enum.GetValues(typeof(Ingredient.debuffs)))
            {
                debuffList.Add(ingredient1.debuffList[debuffIndex] + ingredient2.debuffList[debuffIndex] + ingredient3.debuffList[debuffIndex] + defaultAdder);
            }
            return debuffList;
        }
        #endregion chance determination
        #endregion helper
        #endregion functions
    }
}