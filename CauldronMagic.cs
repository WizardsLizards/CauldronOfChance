﻿using StardewValley;
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
        StardewValley.Item resultingItem { get; set; }
        Random randomGenerator { get; set; }
        double playerLuck { get; set; }
        #endregion setup

        #region ingredients
        public Ingredient ingredient1 { get; set; }
        public Ingredient ingredient2 { get; set; }
        public Ingredient ingredient3 { get; set; }
        #endregion ingredients

        #region chances
        #region special
        public int crafting { get; set; } = 0;
        public int cooking { get; set; } = 0;
        public int randomItem { get; set; } = 0;
        public double butterflies { get; set; } = 0;
        public double boom { get; set; } = 0;
        public double cauldronLuck { get; set; } = 0;
        #endregion

        #region buffs
        public List<int> buffList;
        public enum buffs
        {
            garlicOil1,

            debuffImmunity1,

            farmingBuff1,
            farmingBuff2,
            farmingBuff3,

            miningBuff1,
            miningBuff2,
            miningBuff3,

            fishingBuff1,
            fishingBuff2,
            fishingBuff3,

            foragingBuff1,
            foragingBuff2,
            foragingBuff3,

            attackBuff1,
            attackBuff2,
            attackBuff3,

            defenseBuff1,
            defenseBuff2,
            defenseBuff3,

            maxEnergyBuff1,
            maxEnergyBuff2,
            maxEnergyBuff3,

            luckBuff1,
            luckBuff2,
            luckBuff3,

            magneticRadiusBuff1,
            magneticRadiusBuff2,
            magneticRadiusBuff3,

            speedBuff1,
            speedBuff2,
            speedBuff3
        }
        #endregion buffs

        #region debuffs
        public List<int> debuffList;
        public enum debuffs
        {
            monsterMusk1,

            farmingDebuff1,
            farmingDebuff2,
            farmingDebuff3,

            miningDebuff1,
            miningDebuff2,
            miningDebuff3,

            fishingDebuff1,
            fishingDebuff2,
            fishingDebuff3,

            foragingDebuff1,
            foragingDebuff2,
            foragingDebuff3,

            attackDebuff1,
            attackDebuff2,
            attackDebuff3,

            defenseDebuff1,
            defenseDebuff2,
            defenseDebuff3,

            maxEnergyDebuff1,
            maxEnergyDebuff2,
            maxEnergyDebuff3,

            luckDebuff1,
            luckDebuff2,
            luckDebuff3,

            speedDebuff1,
            speedDebuff2,
            speedDebuff3
        }
        #endregion debuffs
        #endregion chances
        #endregion properties

        #region constructors
        public CauldronMagic(Item ingredient1, Item ingredient2, Item ingredient3)
        {
            using (Cauldron Cauldron = new Cauldron(this))
            {
                this.ingredient1 = Cauldron.getIngredient(ingredient1);
                this.ingredient2 = Cauldron.getIngredient(ingredient2);
                this.ingredient3 = Cauldron.getIngredient(ingredient3);

                WizardHouse = Game1.locations.Where(x => x.Name.Equals("WizardHouse")).First();
                Wizard = Game1.getCharacterFromName("Wizard");
                resultingItem = new StardewValley.Object();
                randomGenerator = new Random();
                playerLuck = Game1.player.DailyLuck + getCauldronLuck();

                buffList = new List<int>();
                foreach (int buffIndex in Enum.GetValues(typeof(buffs)))
                {
                    buffList.Add(0);
                }
                debuffList = new List<int>();
                foreach (int debuffIndex in Enum.GetValues(typeof(debuffs)))
                {
                    debuffList.Add(0);
                }

                UseCauldron(Cauldron);
            }
        }
        #endregion constructors

        #region functions
        public void UseCauldron(Cauldron Cauldron)
        {
            #region determine effects
            findCombinations(Cauldron);
            #endregion determine effects

            #region determine actual effect
            determineResult(Cauldron);
            #endregion determine actual effect

            #region take effect
            //Drink buff
            if (effectType == 1)
            {
                Game1.player.canMove = false;
                ObjectPatches.IModHelper.Reflection.GetMethod(Game1.player, "performDrinkAnimation").Invoke(resultingItem);

                DelayedAction.delayedBehavior onDrink = delegate
                {
                    Game1.player.canMove = true;
                    Game1.activeClickableMenu = new DialogueBox("The magic of the Cauldron flows through you...");
                };

                DelayedAction.functionAfterDelay(onDrink, 1000);
            }
            //Drink debuff
            else if (effectType == 2)
            {
                Game1.player.canMove = false;
                ObjectPatches.IModHelper.Reflection.GetMethod(Game1.player, "performDrinkAnimation").Invoke(resultingItem);

                DelayedAction.delayedBehavior afterDrink = delegate
                {
                    Game1.player.canMove = true;
                    Game1.activeClickableMenu = new DialogueBox("An aweful taste fills your mouth...");
                };

                DelayedAction.delayedBehavior onDrink = delegate
                {
                    Game1.player.performPlayerEmote("sick");
                    DelayedAction.functionAfterDelay(afterDrink, 1000);
                };

                DelayedAction.functionAfterDelay(onDrink, 1500);
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
            else if (effectType == 4)
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
            //Get item
            else if (effectType == 5)
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
        public void findCombinations(Cauldron Cauldron)
        {
            List<string> itemNames = new List<string>()
            {
                ingredient1.item.Name,
                ingredient2.item.Name,
                ingredient3.item.Name
            }
            .Distinct().ToList();

            foreach((string Type, int Match2, int Match3, List<string> Items) tuple in Cauldron.buffCombinations)
            {
                int matches = itemNames.Intersect(tuple.Items).Count();

                if(matches == 2)
                {
                    Cauldron.addToCauldron(tuple.Type, tuple.Match2);
                }
                else if (matches >= 3)
                {
                    Cauldron.addToCauldron(tuple.Type, tuple.Match3);
                }
            }
        }
        #endregion chance values

        #region chance determination
        public void determineResult(Cauldron Cauldron)
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
            double recipeChance = CheckForRecipes(Cauldron);
            if (false)
            {
                effectType = 5;
                return;
            }

            //Check for other item drops? (Drop)
            double itemChance = CheckForItemCombinations(Cauldron);
            if (false)
            {
                effectType = 5;
                return;
            }

            //Check for cooking recipes (Drink)
            if (false)
            {
                effectType = 1;
                //Other text in the end (other buff building too tho...?)
                return;
            }

            //Check for other buffs (Drink)
            CheckForBuffCombinations(Cauldron);

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

            int minutesDuration = 20 * 60; //20 hours //TODO: Random duration? (5-20 min?)
            string source = "CauldronOfChance";
            string displaySource = "Cauldron";

            string deBuff = "";
            int value = -1;
            int type = 0;

            string description = "";

            if (buffRandom <= buffChance)
            {
                //Buff
                effectType = 1;

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
                            deBuff = "You've started to smell like... garlic?";
                            defaultBuff = 23;
                            break;
                        case (int)Ingredient.buffs.debuffImmunity1:
                            //description = "You feel as if nothing can slow you down.";
                            description += "(Debuff Immunity)";
                            deBuff = "You feel like nothing can slow you down today!";
                            defaultBuff = 28;
                            break;
                        case (int)Ingredient.buffs.farmingBuff1:
                            description += "(Farming +1)";
                            deBuff = "farming";
                            type = 1;
                            value = 1;
                            farming = 1;
                            break;
                        case (int)Ingredient.buffs.farmingBuff2:
                            description += "(Farming +2)";
                            deBuff = "farming";
                            type = 1;
                            value = 2;
                            farming = 2;
                            break;
                        case (int)Ingredient.buffs.farmingBuff3:
                            description += "(Farming +3)";
                            deBuff = "farming";
                            type = 1;
                            value = 3;
                            farming = 3;
                            break;
                        case (int)Ingredient.buffs.miningBuff1:
                            description += "(Mining +1)";
                            deBuff = "mining";
                            type = 1;
                            value = 1;
                            mining = 1;
                            break;
                        case (int)Ingredient.buffs.miningBuff2:
                            description += "(Mining +2)";
                            deBuff = "mining";
                            type = 1;
                            value = 2;
                            mining = 2;
                            break;
                        case (int)Ingredient.buffs.miningBuff3:
                            description += "(Mining +3)";
                            deBuff = "mining";
                            type = 1;
                            value = 3;
                            mining = 3;
                            break;
                        case (int)Ingredient.buffs.fishingBuff1:
                            description += "(Fishing +1)";
                            deBuff = "fishing";
                            type = 1;
                            value = 1;
                            fishing = 1;
                            break;
                        case (int)Ingredient.buffs.fishingBuff2:
                            description += "(Fishing +2)";
                            deBuff = "fishing";
                            type = 1;
                            value = 2;
                            fishing = 2;
                            break;
                        case (int)Ingredient.buffs.fishingBuff3:
                            description += "(Fishing +3)";
                            deBuff = "fishing";
                            type = 1;
                            value = 3;
                            fishing = 3;
                            break;
                        case (int)Ingredient.buffs.foragingBuff1:
                            description += "(Foraging +1)";
                            deBuff = "foraging";
                            type = 1;
                            value = 1;
                            foraging = 1;
                            break;
                        case (int)Ingredient.buffs.foragingBuff2:
                            description += "(Foraging +2)";
                            deBuff = "foraging";
                            type = 1;
                            value = 2;
                            foraging = 2;
                            break;
                        case (int)Ingredient.buffs.foragingBuff3:
                            description += "(Foraging +3)";
                            deBuff = "foraging";
                            type = 1;
                            value = 3;
                            foraging = 3;
                            break;
                        case (int)Ingredient.buffs.attackBuff1:
                            description += "(Attack +1)";
                            deBuff = "strong";
                            type = 2;
                            value = 1;
                            attack = 1;
                            break;
                        case (int)Ingredient.buffs.attackBuff2:
                            description += "(Attack +2)";
                            deBuff = "strong";
                            type = 2;
                            value = 2;
                            attack = 2;
                            break;
                        case (int)Ingredient.buffs.attackBuff3:
                            description += "(Attack +3)";
                            deBuff = "strong";
                            type = 2;
                            value = 3;
                            attack = 3;
                            break;
                        case (int)Ingredient.buffs.defenseBuff1:
                            description += "(Defense +1)";
                            deBuff = "resilient";
                            type = 2;
                            value = 1;
                            defense = 1;
                            break;
                        case (int)Ingredient.buffs.defenseBuff2:
                            description += "(Defense +2)";
                            deBuff = "resilient";
                            type = 2;
                            value = 2;
                            defense = 2;
                            break;
                        case (int)Ingredient.buffs.defenseBuff3:
                            description += "(Defense +3)";
                            deBuff = "resilient";
                            type = 2;
                            value = 3;
                            defense = 3;
                            break;
                        case (int)Ingredient.buffs.maxEnergyBuff1:
                            description += "(Max Stamina +10)";
                            deBuff = "vigorous";
                            type = 2;
                            value = 1;
                            maxStamina = 1;
                            break;
                        case (int)Ingredient.buffs.maxEnergyBuff2:
                            description += "(Max Stamina +20)";
                            deBuff = "vigorous";
                            type = 2;
                            value = 2;
                            maxStamina = 2;
                            break;
                        case (int)Ingredient.buffs.maxEnergyBuff3:
                            description += "(Max Stamina +30)";
                            deBuff = "vigorous";
                            type = 2;
                            value = 3;
                            maxStamina = 3;
                            break;
                        case (int)Ingredient.buffs.luckBuff1:
                            description += "(Luck +1)";
                            deBuff = "lucky";
                            type = 2;
                            value = 1;
                            luck = 1;
                            break;
                        case (int)Ingredient.buffs.luckBuff2:
                            description += "(Luck +2)";
                            deBuff = "lucky";
                            type = 2;
                            value = 2;
                            luck = 2;
                            break;
                        case (int)Ingredient.buffs.luckBuff3:
                            description += "(Luck +3)";
                            deBuff = "lucky";
                            type = 2;
                            value = 3;
                            luck = 3;
                            break;
                        case (int)Ingredient.buffs.magneticRadiusBuff1: //TODO: TOO MUCH?
                            description += "(Magnetic Radius +32)";
                            deBuff = "magnetic";
                            type = 2;
                            value = 1;
                            magneticRadius = 1;
                            break;
                        case (int)Ingredient.buffs.magneticRadiusBuff2:
                            description += "(Magnetic Radius +64)";
                            deBuff = "magnetic";
                            type = 2;
                            value = 2;
                            magneticRadius = 2;
                            break;
                        case (int)Ingredient.buffs.magneticRadiusBuff3:
                            description += "(Magnetic Radius + 96)";
                            deBuff = "magnetic";
                            type = 2;
                            value = 3;
                            magneticRadius = 3;
                            break;
                        case (int)Ingredient.buffs.speedBuff1:
                            description += "(Speed +1)";
                            deBuff = "fast";
                            type = 2;
                            value = 1;
                            speed = 1;
                            break;
                        case (int)Ingredient.buffs.speedBuff2:
                            description += "(Speed +2)";
                            deBuff = "fast";
                            type = 2;
                            value = 2;
                            speed = 2;
                            break;
                        case (int)Ingredient.buffs.speedBuff3:
                            description += "(Speed +3)";
                            deBuff = "fast";
                            type = 2;
                            value = 3;
                            speed = 3;
                            break;
                    }
                }
            }
            else
            {
                //Debuff
                effectType = 2;

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
                            deBuff = "A sweet stench emnates from you...";
                            defaultBuff = 24;
                            break;
                        case (int)Ingredient.debuffs.farmingDebuff1:
                            description += "(Farming -1)";
                            deBuff = "farming";
                            type = 1;
                            value = 1;
                            farming = -1;
                            break;
                        case (int)Ingredient.debuffs.farmingDebuff2:
                            description += "(Farming -2)";
                            deBuff = "farming";
                            type = 1;
                            value = 2;
                            farming = -2;
                            break;
                        case (int)Ingredient.debuffs.farmingDebuff3:
                            description += "(Farming -3)";
                            deBuff = "farming";
                            type = 1;
                            value = 3;
                            farming = -3;
                            break;
                        case (int)Ingredient.debuffs.miningDebuff1:
                            description += "(Mining -1)";
                            deBuff = "mining";
                            type = 1;
                            value = 1;
                            mining = -1;
                            break;
                        case (int)Ingredient.debuffs.miningDebuff2:
                            description += "(Mining -2)";
                            deBuff = "mining";
                            type = 1;
                            value = 2;
                            mining = -2;
                            break;
                        case (int)Ingredient.debuffs.miningDebuff3:
                            description += "(Mining -3)";
                            deBuff = "mining";
                            type = 1;
                            value = 3;
                            mining = -3;
                            break;
                        case (int)Ingredient.debuffs.fishingDebuff1:
                            description += "(Fishing -1)";
                            deBuff = "fishing";
                            type = 1;
                            value = 1;
                            fishing = -1;
                            break;
                        case (int)Ingredient.debuffs.fishingDebuff2:
                            description += "(Fishing -2)";
                            deBuff = "fishing";
                            type = 1;
                            value = 2;
                            fishing = -2;
                            break;
                        case (int)Ingredient.debuffs.fishingDebuff3:
                            description += "(Fishing -3)";
                            deBuff = "fishing";
                            type = 1;
                            value = 3;
                            fishing = -3;
                            break;
                        case (int)Ingredient.debuffs.foragingDebuff1:
                            description += "(Foraging -1)";
                            deBuff = "foraging";
                            type = 1;
                            value = 1;
                            foraging = -1;
                            break;
                        case (int)Ingredient.debuffs.foragingDebuff2:
                            description += "(Foraging -2)";
                            deBuff = "foraging";
                            type = 1;
                            value = 2;
                            foraging = -2;
                            break;
                        case (int)Ingredient.debuffs.foragingDebuff3:
                            description += "(Foraging -3)";
                            deBuff = "foraging";
                            type = 1;
                            value = 3;
                            foraging = -3;
                            break;
                        case (int)Ingredient.debuffs.attackDebuff1:
                            description += "(Attack -1)";
                            deBuff = "strong";
                            type = 2;
                            value = 1;
                            attack = -1;
                            break;
                        case (int)Ingredient.debuffs.attackDebuff2:
                            description += "(Attack -2)";
                            deBuff = "strong";
                            type = 2;
                            value = 2;
                            attack = -2;
                            break;
                        case (int)Ingredient.debuffs.attackDebuff3:
                            description += "(Attack -3)";
                            deBuff = "strong";
                            type = 2;
                            value = 3;
                            attack = -3;
                            break;
                        case (int)Ingredient.debuffs.defenseDebuff1:
                            description += "(Defense -1)";
                            deBuff = "resilient";
                            type = 2;
                            value = 1;
                            defense = -1;
                            break;
                        case (int)Ingredient.debuffs.defenseDebuff2:
                            description += "(Defense -2)";
                            deBuff = "resilient";
                            type = 2;
                            value = 2;
                            defense = -2;
                            break;
                        case (int)Ingredient.debuffs.defenseDebuff3:
                            description += "(Defense -3)";
                            deBuff = "resilient";
                            type = 2;
                            value = 3;
                            defense = -3;
                            break;
                        case (int)Ingredient.debuffs.maxEnergyDebuff1:
                            description += "(Max Stamina -10)";
                            deBuff = "vigorous";
                            type = 2;
                            value = 1;
                            maxStamina = -1 * 10;
                            break;
                        case (int)Ingredient.debuffs.maxEnergyDebuff2:
                            description += "(Max Stamina -20)";
                            deBuff = "vigorous";
                            type = 2;
                            value = 2;
                            maxStamina = -2 * 10;
                            break;
                        case (int)Ingredient.debuffs.maxEnergyDebuff3:
                            description += "(Max Stamina -30)";
                            deBuff = "vigorous";
                            type = 2;
                            value = 3;
                            maxStamina = -3 * 10;
                            break;
                        case (int)Ingredient.debuffs.luckDebuff1:
                            description += "(Luck -1)";
                            deBuff = "lucky";
                            type = 2;
                            value = 1;
                            luck = -1;
                            break;
                        case (int)Ingredient.debuffs.luckDebuff2:
                            description += "(Luck -2)";
                            deBuff = "lucky";
                            type = 2;
                            value = 2;
                            luck = -2;
                            break;
                        case (int)Ingredient.debuffs.luckDebuff3:
                            description += "(Luck -3)";
                            deBuff = "lucky";
                            type = 2;
                            value = 3;
                            luck = -3;
                            break;
                        case (int)Ingredient.debuffs.speedDebuff1:
                            description += "(Speed -1)";
                            deBuff = "fast";
                            type = 2;
                            value = 1;
                            speed = -1;
                            break;
                        case (int)Ingredient.debuffs.speedDebuff2:
                            description += "(Speed -2)";
                            deBuff = "fast";
                            type = 2;
                            value = 2;
                            speed = -2;
                            break;
                        case (int)Ingredient.debuffs.speedDebuff3:
                            description += "(Speed -3)";
                            deBuff = "fast";
                            type = 2;
                            value = 3;
                            speed = -3;
                            break;
                    }
                }
            }

            #region template
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
            #endregion template

            #region create buff
            string modifier = "";
            string modifierEnd = ".";
            string debuffModifier = "";

            if (effectType == 2)
            {
                debuffModifier = "don't ";
            }

            if (type == 0)
            {
                description = $"{deBuff}";
            }
            else if (type == 1)
            {
                if (value == 2)
                {
                    modifier = "quite ";
                    modifierEnd = "...";
                }
                else if (value == 3)
                {
                    modifier = "really ";
                    modifierEnd = "!!";
                }

                //description += $"You {debuffModifier}{modifier}feel like {deBuff} today{modifierEnd}";
                description = $"You {debuffModifier}{modifier}feel like {deBuff} today{modifierEnd}";
            }
            else if (type == 2)
            {
                if (value == 2)
                {
                    modifier = "quite ";
                    modifierEnd = "...";
                }
                else if (value == 3)
                {
                    modifier = "very ";
                    modifierEnd = "!!";
                }

                //description += $"You {debuffModifier}feel {modifier}{deBuff} today{modifierEnd}";
                description = $"You {debuffModifier}feel {modifier}{deBuff} today{modifierEnd}";
            }

            Buff buff = new Buff(farming, fishing, mining, digging, luck, foraging, crafting, maxStamina * 10, magneticRadius * 32, speed, defense, attack, minutesDuration, source, displaySource)
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
            #endregion create buff
        }

        public void CheckForBuffCombinations(Cauldron Cauldron)
        {

        }

        public double CheckForRecipes(Cauldron Cauldron)
        {
            //0-1/3 Parts: 0% Chance. 2/3 Parts: 25% Chance. 3/3 Parts: 75% Chance (Always top down. If full match always that. if no full match but multiple 25%: Same chance for everything)
            List<string> fullMatches = new List<string>();
            List<string> halfMatches = new List<string>();

            foreach((string Result, List<string> Items) recipe in Cauldron.recipes)
            {
                List<string> ingredients = new List<string>() { ingredient1.item.Name, ingredient2.item.Name, ingredient3.item.Name };
                int matches = 0;

                foreach(string Item in recipe.Items)
                {
                    if (ingredients.Contains(Item))
                    {
                        matches += 1;
                        ingredients.Remove(Item);
                    }
                }

                if(matches == 2)
                {
                    halfMatches.Add(recipe.Result);
                }
                else if (matches >= 3)
                {
                    fullMatches.Add(recipe.Result);
                }
            }

            if (fullMatches.Count > 0)
            {
                //Get item
                int index = randomGenerator.Next(0, fullMatches.Count - 1);

                resultingItem = Utility.fuzzyItemSearch(fullMatches[index]);

                return 0.75;
            }
            else if (halfMatches.Count > 0)
            {
                //Get item
                int index = randomGenerator.Next(0, halfMatches.Count - 1);

                resultingItem = Utility.fuzzyItemSearch(halfMatches[index]);

                return 0.25;
            }

            return 0;
        }

        public double CheckForItemCombinations(Cauldron Cauldron)
        {
            //2 Items Match: 5% Chance. 3 Items Match: 10% Chance (Does 3-Item match override 2-Item match tho? (Even a possibility?))
            // => Decide on item by chance: 10% items are added 3x to the pool, 5% items once. Once decided: can be 10% chance even for a 5% chance item, if theres a match
            double chance = 0;

            List<string> match2 = new List<string>();
            List<string> match3 = new List<string>();
            List<string> ingredients = new List<string>() { ingredient1.item.Name, ingredient2.item.Name, ingredient3.item.Name }.Distinct().ToList();

            foreach ((string Result, List<string> Items) itemCombination in Cauldron.itemCombinations)
            {
                int matches = ingredients.Intersect(itemCombination.Items).Count();

                if(matches == 2)
                {
                    match2.Add(itemCombination.Result);

                    if(chance < 0.05)
                    {
                        chance = 0.05;
                    }
                }
                else if (matches >= 3)
                {
                    match3.Add(itemCombination.Result);

                    if(chance < 0.1)
                    {
                        chance = 0.1;
                    }
                }
            }

            List<string> possibilities = new List<string>();

            foreach(string match in match2)
            {
                possibilities.Add(match);
            }
            foreach(string match in match3)
            {
                possibilities.Add(match);
                possibilities.Add(match);
                possibilities.Add(match);
            }

            if(possibilities.Count() > 0)
            {
                int index = randomGenerator.Next(0, possibilities.Count() - 1);

                resultingItem = Utility.fuzzyItemSearch(possibilities[index]);
            }

            return 0;
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