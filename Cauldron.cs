using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CauldronOfChance
{
    /// <summary>
    /// Holds all items/recipes/combinations
    /// </summary>
    public class Cauldron : IDisposable
    {
        #region buffs
        public List<int> buffList;
        public enum buffs
        {
            garlicOil1,
            garlicOil2,
            garlicOil3,

            debuffImmunity1,
            debuffImmunity2,
            debuffImmunity3,

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
            monsterMusk2,
            monsterMusk3,

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

        //List 1: Holds all combinations. Tuple: Type - Buff to add on to, Match2 - Value for 2 matches, Match3 - Value for 3 matches, Items - List of possible match items (Distinct)
        public List<(string Type, int Match2, int Match3, List<string> Items)> buffCombinations { get; set; }

        //List 1: Holds all the combinations. Tuple: Result - Resulting item, Items - 3 Items needed to create this recipe (at full chance)(Not Distinct)
        public List<(string Result, List<string> Items)> recipes { get; set; }

        //List 1: Holds all item combinatins. Tuple: Result - Resulting item, Items - List of possible match items (Distinct)
        public List<(string Result, List<string> Items)> itemCombinations { get; set; }

        object Caller { get; set; }

        public Cauldron(object caller)
        {
            this.Caller = caller;

            initializeBuffCombinations();
            initializeRecipes();
            initializeItemCombinations();
        }

        public void initializeBuffCombinations()
        {
            buffCombinations = new List<(string Type, int Match2, int Match3, List<string> Items)>();

            //TODO: => Add combinations here (e.g. all fish. Boni apply for: 2 matches, 3 matches. 3 matches give bigger boni)
        }

        public void initializeRecipes()
        {
            recipes = new List<(string Result, List<string> Items)>(); //TODO: Or add recipes with lower match3 (higher match1 tho) chance but more ingredients? => Combinations for items? Or handle that in a third list?

            //TODO: Add recipes here. 1 part recipe: 1% chance, 2 part recipe: 25% chance, full recipe: 75% chance? -> Set flag so only one check regardless of all possible recipes (decide if event happens)
        }

        //Prio 1 (After Other checks): Recipes. Prio 2: These Combinations (Same check as in recipes? Prolly its own tho). Check 3: Combinations => Drink Buff / Debuff
        public void initializeItemCombinations()
        {
            itemCombinations = new List<(string Result, List<string> Items)>();

            //TODO: Add item combinations here. 2 matches: 5% chance. 3 matches: 10% chance?
        }

        public void addToCauldron(string name, int value)
        {
            if (name.Equals("garlicOil"))
            {
                addToCauldron(name, "monsterMusk", value);
            }
            else if (name.Equals("monsterMusk"))
            {
                addToCauldron("garlicOil", name, value);
            }
            else if (name.Equals("debuffImmunity"))
            {
                addToCauldron(name, "", value);
            }
            else if (name.Equals("magneticRadius"))
            {
                addToCauldron(name + "Buff", "", value);
            }
            else
            {
                addToCauldron(name + "Buff", name + "Debuff", value);
            }
        }

        public void addToCauldron(string buff, string debuff, int value)
        {
            int buffIndex1 = -1;
            int buffIndex2 = -1;
            int buffIndex3 = -1;
            int debuffIndex1 = -1;
            int debuffIndex2 = -1;
            int debuffIndex3 = -1;

            if (buff != null && buff.Equals("") == false)
            {
                buffIndex1 = (int)Enum.Parse(typeof(buffs), buff + 1);
                buffIndex2 = (int)Enum.Parse(typeof(buffs), buff + 2);
                buffIndex3 = (int)Enum.Parse(typeof(buffs), buff + 3);
            }
            if (debuff != null && debuff.Equals("") == false)
            {
                debuffIndex1 = (int)Enum.Parse(typeof(debuffs), debuff + 1);
                debuffIndex2 = (int)Enum.Parse(typeof(debuffs), debuff + 2);
                debuffIndex3 = (int)Enum.Parse(typeof(debuffs), debuff + 3);
            }

            List<int> BuffList = new List<int>();
            List<int> DebuffList = new List<int>();

            if (Caller is CauldronMagic)
            {
                CauldronMagic cauldronMagic = Caller as CauldronMagic;
                BuffList = cauldronMagic.buffList;
                DebuffList = cauldronMagic.debuffList;
            }
            else if (Caller is Ingredient)
            {
                Ingredient ingredient = Caller as Ingredient;
                BuffList = ingredient.buffList;
                DebuffList = ingredient.debuffList;
            }

            if (value >= 3)
            {
                if (buff != null && buff.Equals("") == false)
                {
                    BuffList[buffIndex3] += 3;
                    BuffList[buffIndex2] += 2;
                    BuffList[buffIndex1] += 1;
                }

                if (debuff != null && debuff.Equals("") == false)
                {
                    DebuffList[debuffIndex2] += 1;
                    DebuffList[debuffIndex1] += 1;
                }
            }
            else if (value == 2)
            {
                if (buff != null && buff.Equals("") == false)
                {
                    BuffList[buffIndex2] += 3;
                    BuffList[buffIndex1] += 2;
                }

                if (debuff != null && debuff.Equals("") == false)
                {
                    DebuffList[debuffIndex1] += 1;
                }
            }
            else if (value == 1)
            {
                if (buff != null && buff.Equals("") == false)
                {
                    BuffList[buffIndex1] += 3;
                }

                if (debuff != null && debuff.Equals("") == false)
                {
                    DebuffList[debuffIndex1] += 1;
                }
            }
            else if (value == -1)
            {
                if (debuff != null && debuff.Equals("") == false)
                {
                    DebuffList[debuffIndex1] += 3;
                }

                if (buff != null && buff.Equals("") == false)
                {
                    BuffList[buffIndex1] += 1;
                }
            }
            else if (value == -2)
            {
                if (debuff != null && debuff.Equals("") == false)
                {
                    DebuffList[debuffIndex2] += 3;
                    DebuffList[debuffIndex1] += 2;
                }

                if (buff != null && buff.Equals("") == false)
                {
                    BuffList[buffIndex1] += 1;
                }
            }
            else if (value <= -3)
            {
                if (debuff != null && debuff.Equals("") == false)
                {
                    DebuffList[debuffIndex3] += 3;
                    DebuffList[debuffIndex2] += 2;
                    DebuffList[debuffIndex1] += 1;
                }

                if (buff != null && buff.Equals("") == false)
                {
                    BuffList[buffIndex2] += 1;
                    BuffList[buffIndex1] += 1;
                }
            }
        }

        public Ingredient getIngredient(Item item)
        {
            Ingredient Item;

            switch (item.Name)
            {
                case "Weeds":
                    Item = new Ingredient(item);
                    break;
                case "Stone":
                    Item = new Ingredient(item);
                    break;
                case "Wild Horseradish":
                    Item = new Ingredient(item);
                    break;
                case "Daffodil":
                    Item = new Ingredient(item);
                    break;
                case "Leek":
                    Item = new Ingredient(item);
                    break;
                case "Dandelion":
                    Item = new Ingredient(item);
                    break;
                case "Parsnip":
                    Item = new Ingredient(item);
                    break;
                case "Lumber":
                    Item = new Ingredient(item);
                    break;
                case "Emerald":
                    Item = new Ingredient(item);
                    break;
                case "Aquamarine":
                    Item = new Ingredient(item);
                    break;
                case "Ruby":
                    Item = new Ingredient(item);
                    break;
                case "Amethyst":
                    Item = new Ingredient(item);
                    break;
                case "Topaz":
                    Item = new Ingredient(item);
                    break;
                case "Jade":
                    Item = new Ingredient(item);
                    break;
                case "Diamond":
                    Item = new Ingredient(item);
                    break;
                case "Prismatic Shard":
                    Item = new Ingredient(item);
                    break;
                case "Cave Carrot":
                    Item = new Ingredient(item);
                    break;
                case "Quartz":
                    Item = new Ingredient(item);
                    break;
                case "Fire Quartz":
                    Item = new Ingredient(item);
                    break;
                case "Frozen Tear":
                    Item = new Ingredient(item);
                    break;
                case "Earth Crystal":
                    Item = new Ingredient(item);
                    break;
                case "Coconut":
                    Item = new Ingredient(item);
                    break;
                case "Cactus Fruit":
                    Item = new Ingredient(item);
                    break;
                case "Sap":
                    Item = new Ingredient(item);
                    break;
                case "Torch":
                    Item = new Ingredient(item);
                    break;
                case "Spirit Torch":
                    Item = new Ingredient(item);
                    break;
                case "Dwarf Scroll I":
                    Item = new Ingredient(item);
                    break;
                case "Dwarf Scroll II":
                    Item = new Ingredient(item);
                    break;
                case "Dwarf Scroll III":
                    Item = new Ingredient(item);
                    break;
                case "Dwarf Scroll IV":
                    Item = new Ingredient(item);
                    break;
                case "Chipped Amphora":
                    Item = new Ingredient(item);
                    break;
                case "Arrowhead":
                    Item = new Ingredient(item);
                    break;
                case "Ancient Doll":
                    Item = new Ingredient(item);
                    break;
                case "Elvish Jewelry":
                    Item = new Ingredient(item);
                    break;
                case "Chewing Stick":
                    Item = new Ingredient(item);
                    break;
                case "Ornamental Fan":
                    Item = new Ingredient(item);
                    break;
                case "Dinosaur Egg":
                    Item = new Ingredient(item);
                    break;
                case "Rare Disc":
                    Item = new Ingredient(item);
                    break;
                case "Ancient Sword":
                    Item = new Ingredient(item);
                    break;
                case "Rusty Spoon":
                    Item = new Ingredient(item);
                    break;
                case "Rusty Spur":
                    Item = new Ingredient(item);
                    break;
                case "Rusty Cog":
                    Item = new Ingredient(item);
                    break;
                case "Chicken Statue":
                    Item = new Ingredient(item);
                    break;
                case "Ancient Seed":
                    Item = new Ingredient(item);
                    break;
                case "Prehistoric Tool":
                    Item = new Ingredient(item);
                    break;
                case "Dried Starfish":
                    Item = new Ingredient(item);
                    break;
                case "Anchor":
                    Item = new Ingredient(item);
                    break;
                case "Glass Shards":
                    Item = new Ingredient(item);
                    break;
                case "Bone Flute":
                    Item = new Ingredient(item);
                    break;
                case "Prehistoric Handaxe":
                    Item = new Ingredient(item);
                    break;
                case "Dwarvish Helm":
                    Item = new Ingredient(item);
                    break;
                case "Dwarf Gadget":
                    Item = new Ingredient(item);
                    break;
                case "Ancient Drum":
                    Item = new Ingredient(item);
                    break;
                case "Golden Mask":
                    Item = new Ingredient(item);
                    break;
                case "Golden Relic":
                    Item = new Ingredient(item);
                    break;
                case "Strange Doll":
                    Item = new Ingredient(item);
                    break;
                case "Pufferfish":
                    Item = new Ingredient(item);
                    break;
                case "Anchovy":
                    Item = new Ingredient(item);
                    break;
                case "Tuna":
                    Item = new Ingredient(item);
                    break;
                case "Sardine":
                    Item = new Ingredient(item);
                    break;
                case "Bream":
                    Item = new Ingredient(item);
                    break;
                case "Largemouth Bass":
                    Item = new Ingredient(item);
                    break;
                case "Smallmouth Bass":
                    Item = new Ingredient(item);
                    break;
                case "Rainbow Trout":
                    Item = new Ingredient(item);
                    break;
                case "Salmon":
                    Item = new Ingredient(item);
                    break;
                case "Walleye":
                    Item = new Ingredient(item);
                    break;
                case "Perch":
                    Item = new Ingredient(item);
                    break;
                case "Carp":
                    Item = new Ingredient(item);
                    break;
                case "Catfish":
                    Item = new Ingredient(item);
                    break;
                case "Pike":
                    Item = new Ingredient(item);
                    break;
                case "Sunfish":
                    Item = new Ingredient(item);
                    break;
                case "Red Mullet":
                    Item = new Ingredient(item);
                    break;
                case "Herring":
                    Item = new Ingredient(item);
                    break;
                case "Eel":
                    Item = new Ingredient(item);
                    break;
                case "Octopus":
                    Item = new Ingredient(item);
                    break;
                case "Red Snapper":
                    Item = new Ingredient(item);
                    break;
                case "Squid":
                    Item = new Ingredient(item);
                    break;
                case "Seaweed":
                    Item = new Ingredient(item);
                    break;
                case "Green Algae":
                    Item = new Ingredient(item);
                    break;
                case "Sea Cucumber":
                    Item = new Ingredient(item);
                    break;
                case "Super Cucumber":
                    Item = new Ingredient(item);
                    break;
                case "Ghostfish":
                    Item = new Ingredient(item);
                    break;
                case "White Algae":
                    Item = new Ingredient(item);
                    break;
                case "Stonefish":
                    Item = new Ingredient(item);
                    break;
                case "Crimsonfish":
                    Item = new Ingredient(item);
                    break;
                case "Angler":
                    Item = new Ingredient(item);
                    break;
                case "Ice Pip":
                    Item = new Ingredient(item);
                    break;
                case "Lava Eel":
                    Item = new Ingredient(item);
                    break;
                case "Legend":
                    Item = new Ingredient(item);
                    break;
                case "Sandfish":
                    Item = new Ingredient(item);
                    break;
                case "Scorpion Carp":
                    Item = new Ingredient(item);
                    break;
                case "Treasure Chest":
                    Item = new Ingredient(item);
                    break;
                case "Joja Cola":
                    Item = new Ingredient(item);
                    break;
                case "Trash":
                    Item = new Ingredient(item);
                    break;
                case "Driftwood":
                    Item = new Ingredient(item);
                    break;
                case "Broken Glasses":
                    Item = new Ingredient(item);
                    break;
                case "Broken CD":
                    Item = new Ingredient(item);
                    break;
                case "Soggy Newspaper":
                    Item = new Ingredient(item);
                    break;
                case "Large Egg":
                    Item = new Ingredient(item);
                    break;
                case "Egg":
                    Item = new Ingredient(item);
                    break;
                case "Hay":
                    Item = new Ingredient(item);
                    break;
                case "Milk":
                    Item = new Ingredient(item);
                    break;
                case "Large Milk":
                    Item = new Ingredient(item);
                    break;
                case "Green Bean":
                    Item = new Ingredient(item);
                    break;
                case "Cauliflower":
                    Item = new Ingredient(item);
                    break;
                case "Ornate Necklace":
                    Item = new Ingredient(item);
                    break;
                case "Potato":
                    Item = new Ingredient(item);
                    break;
                case "Fried Egg":
                    Item = new Ingredient(item);
                    break;
                case "Omelet":
                    Item = new Ingredient(item);
                    break;
                case "Salad":
                    Item = new Ingredient(item);
                    break;
                case "Cheese Cauliflower":
                    Item = new Ingredient(item);
                    break;
                case "Baked Fish":
                    Item = new Ingredient(item);
                    break;
                case "Parsnip Soup":
                    Item = new Ingredient(item);
                    break;
                case "Vegetable Medley":
                    Item = new Ingredient(item);
                    break;
                case "Complete Breakfast":
                    Item = new Ingredient(item);
                    break;
                case "Fried Calamari":
                    Item = new Ingredient(item);
                    break;
                case "Strange Bun":
                    Item = new Ingredient(item);
                    break;
                case "Lucky Lunch":
                    Item = new Ingredient(item);
                    break;
                case "Fried Mushroom":
                    Item = new Ingredient(item);
                    break;
                case "Pizza":
                    Item = new Ingredient(item);
                    break;
                case "Bean Hotpot":
                    Item = new Ingredient(item);
                    break;
                case "Glazed Yams":
                    Item = new Ingredient(item);
                    break;
                case "Carp Surprise":
                    Item = new Ingredient(item);
                    break;
                case "Hashbrowns":
                    Item = new Ingredient(item);
                    break;
                case "Pancakes":
                    Item = new Ingredient(item);
                    break;
                case "Salmon Dinner":
                    Item = new Ingredient(item);
                    break;
                case "Fish Taco":
                    Item = new Ingredient(item);
                    break;
                case "Crispy Bass":
                    Item = new Ingredient(item);
                    break;
                case "Pepper Poppers":
                    Item = new Ingredient(item);
                    break;
                case "Bread":
                    Item = new Ingredient(item);
                    break;
                case "Tom Kha Soup":
                    Item = new Ingredient(item);
                    break;
                case "Trout Soup":
                    Item = new Ingredient(item);
                    break;
                case "Chocolate Cake":
                    Item = new Ingredient(item);
                    break;
                case "Pink Cake":
                    Item = new Ingredient(item);
                    break;
                case "Rhubarb Pie":
                    Item = new Ingredient(item);
                    break;
                case "Cookie":
                    Item = new Ingredient(item);
                    break;
                case "Spaghetti":
                    Item = new Ingredient(item);
                    break;
                case "Fried Eel":
                    Item = new Ingredient(item);
                    break;
                case "Spicy Eel":
                    Item = new Ingredient(item);
                    break;
                case "Sashimi":
                    Item = new Ingredient(item);
                    break;
                case "Maki Roll":
                    Item = new Ingredient(item);
                    break;
                case "Tortilla":
                    Item = new Ingredient(item);
                    break;
                case "Red Plate":
                    Item = new Ingredient(item);
                    break;
                case "Eggplant Parmesan":
                    Item = new Ingredient(item);
                    break;
                case "Rice Pudding":
                    Item = new Ingredient(item);
                    break;
                case "Ice Cream":
                    Item = new Ingredient(item);
                    break;
                case "Blueberry Tart":
                    Item = new Ingredient(item);
                    break;
                case "Autumn's Bounty":
                    Item = new Ingredient(item);
                    break;
                case "Pumpkin Soup":
                    Item = new Ingredient(item);
                    break;
                case "Super Meal":
                    Item = new Ingredient(item);
                    break;
                case "Cranberry Sauce":
                    Item = new Ingredient(item);
                    break;
                case "Stuffing":
                    Item = new Ingredient(item);
                    break;
                case "Farmer's Lunch":
                    Item = new Ingredient(item);
                    break;
                case "Survival Burger":
                    Item = new Ingredient(item);
                    break;
                case "Dish O' The Sea":
                    Item = new Ingredient(item);
                    break;
                case "Miner's Treat":
                    Item = new Ingredient(item);
                    break;
                case "Roots Platter":
                    Item = new Ingredient(item);
                    break;
                case "Sugar":
                    Item = new Ingredient(item);
                    break;
                case "Wheat Flour":
                    Item = new Ingredient(item);
                    break;
                case "Oil":
                    Item = new Ingredient(item);
                    break;
                case "Garlic":
                    Item = new Ingredient(item);
                    break;
                case "Kale":
                    Item = new Ingredient(item);
                    break;
                case "Tea Sapling":
                    Item = new Ingredient(item);
                    break;
                case "Rhubarb":
                    Item = new Ingredient(item);
                    break;
                case "Triple Shot Espresso":
                    Item = new Ingredient(item);
                    break;
                case "Melon":
                    Item = new Ingredient(item);
                    break;
                case "Tomato":
                    Item = new Ingredient(item);
                    break;
                case "Morel":
                    Item = new Ingredient(item);
                    break;
                case "Blueberry":
                    Item = new Ingredient(item);
                    break;
                case "Fiddlehead Fern":
                    Item = new Ingredient(item);
                    break;
                case "Hot Pepper":
                    Item = new Ingredient(item);
                    break;
                case "Warp Totem: Desert":
                    Item = new Ingredient(item);
                    break;
                case "Wheat":
                    Item = new Ingredient(item);
                    break;
                case "Radish":
                    Item = new Ingredient(item);
                    break;
                case "Seafoam Pudding":
                    Item = new Ingredient(item);
                    break;
                case "Red Cabbage":
                    Item = new Ingredient(item);
                    break;
                case "Flounder":
                    Item = new Ingredient(item);
                    break;
                case "Starfruit":
                    Item = new Ingredient(item);
                    break;
                case "Midnight Carp":
                    Item = new Ingredient(item);
                    break;
                case "Corn":
                    Item = new Ingredient(item);
                    break;
                case "Unmilled Rice":
                    Item = new Ingredient(item);
                    break;
                case "Eggplant":
                    Item = new Ingredient(item);
                    break;
                case "Rice Shoot":
                    Item = new Ingredient(item);
                    break;
                case "Artichoke":
                    Item = new Ingredient(item);
                    break;
                case "Artifact Trove":
                    Item = new Ingredient(item);
                    break;
                case "Pumpkin":
                    Item = new Ingredient(item);
                    break;
                case "Wilted Bouquet":
                    Item = new Ingredient(item);
                    break;
                case "Bok Choy":
                    Item = new Ingredient(item);
                    break;
                case "Magic Rock Candy":
                    Item = new Ingredient(item);
                    break;
                case "Yam":
                    Item = new Ingredient(item);
                    break;
                case "Chanterelle":
                    Item = new Ingredient(item);
                    break;
                case "Cranberries":
                    Item = new Ingredient(item);
                    break;
                case "Holly":
                    Item = new Ingredient(item);
                    break;
                case "Beet":
                    Item = new Ingredient(item);
                    break;
                case "Cherry Bomb":
                    Item = new Ingredient(item);
                    break;
                case "Bomb":
                    Item = new Ingredient(item);
                    break;
                case "Mega Bomb":
                    Item = new Ingredient(item);
                    break;
                case "Brick Floor":
                    Item = new Ingredient(item);
                    break;
                case "Twig":
                    Item = new Ingredient(item);
                    break;
                case "Salmonberry":
                    Item = new Ingredient(item);
                    break;
                case "Grass Starter":
                    Item = new Ingredient(item);
                    break;
                case "Hardwood Fence":
                    Item = new Ingredient(item);
                    break;
                case "Amaranth Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Amaranth":
                    Item = new Ingredient(item);
                    break;
                case "Grape Starter":
                    Item = new Ingredient(item);
                    break;
                case "Hops Starter":
                    Item = new Ingredient(item);
                    break;
                case "Pale Ale":
                    Item = new Ingredient(item);
                    break;
                case "Hops":
                    Item = new Ingredient(item);
                    break;
                case "Void Egg":
                    Item = new Ingredient(item);
                    break;
                case "Mayonnaise":
                    Item = new Ingredient(item);
                    break;
                case "Duck Mayonnaise":
                    Item = new Ingredient(item);
                    break;
                case "Void Mayonnaise":
                    Item = new Ingredient(item);
                    break;
                case "Acorn":
                    Item = new Ingredient(item);
                    break;
                case "Maple Seed":
                    Item = new Ingredient(item);
                    break;
                case "Pine Cone":
                    Item = new Ingredient(item);
                    break;
                case "Wood Fence":
                    Item = new Ingredient(item);
                    break;
                case "Stone Fence":
                    Item = new Ingredient(item);
                    break;
                case "Iron Fence":
                    Item = new Ingredient(item);
                    break;
                case "Gate":
                    Item = new Ingredient(item);
                    break;
                case "Wood Floor":
                    Item = new Ingredient(item);
                    break;
                case "Stone Floor":
                    Item = new Ingredient(item);
                    break;
                case "Clay":
                    Item = new Ingredient(item);
                    break;
                case "Weathered Floor":
                    Item = new Ingredient(item);
                    break;
                case "Crystal Floor":
                    Item = new Ingredient(item);
                    break;
                case "Copper Bar":
                    Item = new Ingredient(item);
                    break;
                case "Iron Bar":
                    Item = new Ingredient(item);
                    break;
                case "Gold Bar":
                    Item = new Ingredient(item);
                    break;
                case "Iridium Bar":
                    Item = new Ingredient(item);
                    break;
                case "Refined Quartz":
                    Item = new Ingredient(item);
                    break;
                case "Honey":
                    Item = new Ingredient(item);
                    break;
                case "Tea Set":
                    Item = new Ingredient(item);
                    break;
                case "Pickles":
                    Item = new Ingredient(item);
                    break;
                case "Jelly":
                    Item = new Ingredient(item);
                    break;
                case "Beer":
                    Item = new Ingredient(item);
                    break;
                case "Rare Seed":
                    Item = new Ingredient(item);
                    break;
                case "Wine":
                    Item = new Ingredient(item);
                    break;
                case "Energy Tonic":
                    Item = new Ingredient(item);
                    break;
                case "Juice":
                    Item = new Ingredient(item);
                    break;
                case "Muscle Remedy":
                    Item = new Ingredient(item);
                    break;
                case "Basic Fertilizer":
                    Item = new Ingredient(item);
                    break;
                case "Quality Fertilizer":
                    Item = new Ingredient(item);
                    break;
                case "Basic Retaining Soil":
                    Item = new Ingredient(item);
                    break;
                case "Quality Retaining Soil":
                    Item = new Ingredient(item);
                    break;
                case "Clam":
                    Item = new Ingredient(item);
                    break;
                case "Golden Pumpkin":
                    Item = new Ingredient(item);
                    break;
                case "Poppy":
                    Item = new Ingredient(item);
                    break;
                case "Copper Ore":
                    Item = new Ingredient(item);
                    break;
                case "Iron Ore":
                    Item = new Ingredient(item);
                    break;
                case "Coal":
                    Item = new Ingredient(item);
                    break;
                case "Gold Ore":
                    Item = new Ingredient(item);
                    break;
                case "Iridium Ore":
                    Item = new Ingredient(item);
                    break;
                case "Nautilus Shell":
                    Item = new Ingredient(item);
                    break;
                case "Coral":
                    Item = new Ingredient(item);
                    break;
                case "Rainbow Shell":
                    Item = new Ingredient(item);
                    break;
                case "Coffee":
                    Item = new Ingredient(item);
                    break;
                case "Spice Berry":
                    Item = new Ingredient(item);
                    break;
                case "Sea Urchin":
                    Item = new Ingredient(item);
                    break;
                case "Grape":
                    Item = new Ingredient(item);
                    break;
                case "Spring Onion":
                    Item = new Ingredient(item);
                    break;
                case "Strawberry":
                    Item = new Ingredient(item);
                    break;
                case "Straw Floor":
                    Item = new Ingredient(item);
                    break;
                case "Sweet Pea":
                    Item = new Ingredient(item);
                    break;
                case "Field Snack":
                    Item = new Ingredient(item);
                    break;
                case "Common Mushroom":
                    Item = new Ingredient(item);
                    break;
                case "Wood Path":
                    Item = new Ingredient(item);
                    break;
                case "Wild Plum":
                    Item = new Ingredient(item);
                    break;
                case "Gravel Path":
                    Item = new Ingredient(item);
                    break;
                case "Hazelnut":
                    Item = new Ingredient(item);
                    break;
                case "Crystal Path":
                    Item = new Ingredient(item);
                    break;
                case "Blackberry":
                    Item = new Ingredient(item);
                    break;
                case "Cobblestone Path":
                    Item = new Ingredient(item);
                    break;
                case "Winter Root":
                    Item = new Ingredient(item);
                    break;
                case "Blue Slime Egg":
                    Item = new Ingredient(item);
                    break;
                case "Crystal Fruit":
                    Item = new Ingredient(item);
                    break;
                case "Stepping Stone Path":
                    Item = new Ingredient(item);
                    break;
                case "Snow Yam":
                    Item = new Ingredient(item);
                    break;
                case "Sweet Gem Berry":
                    Item = new Ingredient(item);
                    break;
                case "Crocus":
                    Item = new Ingredient(item);
                    break;
                case "Vinegar":
                    Item = new Ingredient(item);
                    break;
                case "Red Mushroom":
                    Item = new Ingredient(item);
                    break;
                case "Sunflower":
                    Item = new Ingredient(item);
                    break;
                case "Purple Mushroom":
                    Item = new Ingredient(item);
                    break;
                case "Rice":
                    Item = new Ingredient(item);
                    break;
                case "Cheese":
                    Item = new Ingredient(item);
                    break;
                case "Fairy Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Goat Cheese":
                    Item = new Ingredient(item);
                    break;
                case "Tulip Bulb":
                    Item = new Ingredient(item);
                    break;
                case "Cloth":
                    Item = new Ingredient(item);
                    break;
                case "Jazz Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Truffle":
                    Item = new Ingredient(item);
                    break;
                case "Sunflower Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Truffle Oil":
                    Item = new Ingredient(item);
                    break;
                case "Coffee Bean":
                    Item = new Ingredient(item);
                    break;
                case "Stardrop":
                    Item = new Ingredient(item);
                    break;
                case "Goat Milk":
                    Item = new Ingredient(item);
                    break;
                case "Red Slime Egg":
                    Item = new Ingredient(item);
                    break;
                case "L. Goat Milk":
                    Item = new Ingredient(item);
                    break;
                case "Purple Slime Egg":
                    Item = new Ingredient(item);
                    break;
                case "Wool":
                    Item = new Ingredient(item);
                    break;
                case "Explosive Ammo":
                    Item = new Ingredient(item);
                    break;
                case "Duck Egg":
                    Item = new Ingredient(item);
                    break;
                case "Duck Feather":
                    Item = new Ingredient(item);
                    break;
                case "Caviar":
                    Item = new Ingredient(item);
                    break;
                case "Rabbit's Foot":
                    Item = new Ingredient(item);
                    break;
                case "Aged Roe":
                    Item = new Ingredient(item);
                    break;
                case "Stone Base":
                    Item = new Ingredient(item);
                    break;
                case "Poppy Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Ancient Fruit":
                    Item = new Ingredient(item);
                    break;
                case "Spangle Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Algae Soup":
                    Item = new Ingredient(item);
                    break;
                case "Pale Broth":
                    Item = new Ingredient(item);
                    break;
                case "Bouquet":
                    Item = new Ingredient(item);
                    break;
                case "Mead":
                    Item = new Ingredient(item);
                    break;
                case "Mermaid's Pendant":
                    Item = new Ingredient(item);
                    break;
                case "Decorative Pot":
                    Item = new Ingredient(item);
                    break;
                case "Drum Block":
                    Item = new Ingredient(item);
                    break;
                case "Flute Block":
                    Item = new Ingredient(item);
                    break;
                case "Speed-Gro":
                    Item = new Ingredient(item);
                    break;
                case "Deluxe Speed-Gro":
                    Item = new Ingredient(item);
                    break;
                case "Parsnip Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Bean Starter":
                    Item = new Ingredient(item);
                    break;
                case "Cauliflower Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Potato Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Garlic Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Kale Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Rhubarb Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Melon Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Tomato Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Blueberry Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Pepper Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Wheat Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Radish Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Red Cabbage Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Starfruit Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Corn Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Eggplant Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Artichoke Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Pumpkin Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Bok Choy Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Yam Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Cranberry Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Beet Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Spring Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Summer Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Fall Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Winter Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Ancient Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Geode":
                    Item = new Ingredient(item);
                    break;
                case "Frozen Geode":
                    Item = new Ingredient(item);
                    break;
                case "Magma Geode":
                    Item = new Ingredient(item);
                    break;
                case "Alamite":
                    Item = new Ingredient(item);
                    break;
                case "Bixite":
                    Item = new Ingredient(item);
                    break;
                case "Baryte":
                    Item = new Ingredient(item);
                    break;
                case "Aerinite":
                    Item = new Ingredient(item);
                    break;
                case "Calcite":
                    Item = new Ingredient(item);
                    break;
                case "Dolomite":
                    Item = new Ingredient(item);
                    break;
                case "Esperite":
                    Item = new Ingredient(item);
                    break;
                case "Fluorapatite":
                    Item = new Ingredient(item);
                    break;
                case "Geminite":
                    Item = new Ingredient(item);
                    break;
                case "Helvite":
                    Item = new Ingredient(item);
                    break;
                case "Jamborite":
                    Item = new Ingredient(item);
                    break;
                case "Jagoite":
                    Item = new Ingredient(item);
                    break;
                case "Kyanite":
                    Item = new Ingredient(item);
                    break;
                case "Lunarite":
                    Item = new Ingredient(item);
                    break;
                case "Malachite":
                    Item = new Ingredient(item);
                    break;
                case "Neptunite":
                    Item = new Ingredient(item);
                    break;
                case "Lemon Stone":
                    Item = new Ingredient(item);
                    break;
                case "Nekoite":
                    Item = new Ingredient(item);
                    break;
                case "Orpiment":
                    Item = new Ingredient(item);
                    break;
                case "Petrified Slime":
                    Item = new Ingredient(item);
                    break;
                case "Thunder Egg":
                    Item = new Ingredient(item);
                    break;
                case "Pyrite":
                    Item = new Ingredient(item);
                    break;
                case "Ocean Stone":
                    Item = new Ingredient(item);
                    break;
                case "Ghost Crystal":
                    Item = new Ingredient(item);
                    break;
                case "Tigerseye":
                    Item = new Ingredient(item);
                    break;
                case "Jasper":
                    Item = new Ingredient(item);
                    break;
                case "Opal":
                    Item = new Ingredient(item);
                    break;
                case "Fire Opal":
                    Item = new Ingredient(item);
                    break;
                case "Celestine":
                    Item = new Ingredient(item);
                    break;
                case "Marble":
                    Item = new Ingredient(item);
                    break;
                case "Sandstone":
                    Item = new Ingredient(item);
                    break;
                case "Granite":
                    Item = new Ingredient(item);
                    break;
                case "Basalt":
                    Item = new Ingredient(item);
                    break;
                case "Limestone":
                    Item = new Ingredient(item);
                    break;
                case "Soapstone":
                    Item = new Ingredient(item);
                    break;
                case "Hematite":
                    Item = new Ingredient(item);
                    break;
                case "Mudstone":
                    Item = new Ingredient(item);
                    break;
                case "Obsidian":
                    Item = new Ingredient(item);
                    break;
                case "Slate":
                    Item = new Ingredient(item);
                    break;
                case "Fairy Stone":
                    Item = new Ingredient(item);
                    break;
                case "Star Shards":
                    Item = new Ingredient(item);
                    break;
                case "Prehistoric Scapula":
                    Item = new Ingredient(item);
                    break;
                case "Prehistoric Tibia":
                    Item = new Ingredient(item);
                    break;
                case "Prehistoric Skull":
                    Item = new Ingredient(item);
                    break;
                case "Skeletal Hand":
                    Item = new Ingredient(item);
                    break;
                case "Prehistoric Rib":
                    Item = new Ingredient(item);
                    break;
                case "Prehistoric Vertebra":
                    Item = new Ingredient(item);
                    break;
                case "Skeletal Tail":
                    Item = new Ingredient(item);
                    break;
                case "Nautilus Fossil":
                    Item = new Ingredient(item);
                    break;
                case "Amphibian Fossil":
                    Item = new Ingredient(item);
                    break;
                case "Palm Fossil":
                    Item = new Ingredient(item);
                    break;
                case "Trilobite":
                    Item = new Ingredient(item);
                    break;
                case "Artifact Spot":
                    Item = new Ingredient(item);
                    break;
                case "Tulip":
                    Item = new Ingredient(item);
                    break;
                case "Summer Spangle":
                    Item = new Ingredient(item);
                    break;
                case "Fairy Rose":
                    Item = new Ingredient(item);
                    break;
                case "Blue Jazz":
                    Item = new Ingredient(item);
                    break;
                case "Sprinkler":
                    Item = new Ingredient(item);
                    break;
                case "Plum Pudding":
                    Item = new Ingredient(item);
                    break;
                case "Artichoke Dip":
                    Item = new Ingredient(item);
                    break;
                case "Stir Fry":
                    Item = new Ingredient(item);
                    break;
                case "Roasted Hazelnuts":
                    Item = new Ingredient(item);
                    break;
                case "Pumpkin Pie":
                    Item = new Ingredient(item);
                    break;
                case "Radish Salad":
                    Item = new Ingredient(item);
                    break;
                case "Fruit Salad":
                    Item = new Ingredient(item);
                    break;
                case "Blackberry Cobbler":
                    Item = new Ingredient(item);
                    break;
                case "Cranberry Candy":
                    Item = new Ingredient(item);
                    break;
                case "Apple":
                    Item = new Ingredient(item);
                    break;
                case "Green Tea":
                    Item = new Ingredient(item);
                    break;
                case "Bruschetta":
                    Item = new Ingredient(item);
                    break;
                case "Quality Sprinkler":
                    Item = new Ingredient(item);
                    break;
                case "Cherry Sapling":
                    Item = new Ingredient(item);
                    break;
                case "Apricot Sapling":
                    Item = new Ingredient(item);
                    break;
                case "Orange Sapling":
                    Item = new Ingredient(item);
                    break;
                case "Peach Sapling":
                    Item = new Ingredient(item);
                    break;
                case "Pomegranate Sapling":
                    Item = new Ingredient(item);
                    break;
                case "Apple Sapling":
                    Item = new Ingredient(item);
                    break;
                case "Apricot":
                    Item = new Ingredient(item);
                    break;
                case "Orange":
                    Item = new Ingredient(item);
                    break;
                case "Peach":
                    Item = new Ingredient(item);
                    break;
                case "Pomegranate":
                    Item = new Ingredient(item);
                    break;
                case "Cherry":
                    Item = new Ingredient(item);
                    break;
                case "Iridium Sprinkler":
                    Item = new Ingredient(item);
                    break;
                case "Coleslaw":
                    Item = new Ingredient(item);
                    break;
                case "Fiddlehead Risotto":
                    Item = new Ingredient(item);
                    break;
                case "Poppyseed Muffin":
                    Item = new Ingredient(item);
                    break;
                case "Green Slime Egg":
                    Item = new Ingredient(item);
                    break;
                case "Rain Totem":
                    Item = new Ingredient(item);
                    break;
                case "Mutant Carp":
                    Item = new Ingredient(item);
                    break;
                case "Bug Meat":
                    Item = new Ingredient(item);
                    break;
                case "Bait":
                    Item = new Ingredient(item);
                    break;
                case "Spinner":
                    Item = new Ingredient(item);
                    break;
                case "Dressed Spinner":
                    Item = new Ingredient(item);
                    break;
                case "Warp Totem: Farm":
                    Item = new Ingredient(item);
                    break;
                case "Warp Totem: Mountains":
                    Item = new Ingredient(item);
                    break;
                case "Warp Totem: Beach":
                    Item = new Ingredient(item);
                    break;
                case "Barbed Hook":
                    Item = new Ingredient(item);
                    break;
                case "Lead Bobber":
                    Item = new Ingredient(item);
                    break;
                case "Treasure Hunter":
                    Item = new Ingredient(item);
                    break;
                case "Trap Bobber":
                    Item = new Ingredient(item);
                    break;
                case "Cork Bobber":
                    Item = new Ingredient(item);
                    break;
                case "Sturgeon":
                    Item = new Ingredient(item);
                    break;
                case "Tiger Trout":
                    Item = new Ingredient(item);
                    break;
                case "Bullhead":
                    Item = new Ingredient(item);
                    break;
                case "Tilapia":
                    Item = new Ingredient(item);
                    break;
                case "Chub":
                    Item = new Ingredient(item);
                    break;
                case "Magnet":
                    Item = new Ingredient(item);
                    break;
                case "Dorado":
                    Item = new Ingredient(item);
                    break;
                case "Albacore":
                    Item = new Ingredient(item);
                    break;
                case "Shad":
                    Item = new Ingredient(item);
                    break;
                case "Lingcod":
                    Item = new Ingredient(item);
                    break;
                case "Halibut":
                    Item = new Ingredient(item);
                    break;
                case "Hardwood":
                    Item = new Ingredient(item);
                    break;
                case "Crab Pot":
                    Item = new Ingredient(item);
                    break;
                case "Lobster":
                    Item = new Ingredient(item);
                    break;
                case "Crayfish":
                    Item = new Ingredient(item);
                    break;
                case "Crab":
                    Item = new Ingredient(item);
                    break;
                case "Cockle":
                    Item = new Ingredient(item);
                    break;
                case "Mussel":
                    Item = new Ingredient(item);
                    break;
                case "Shrimp":
                    Item = new Ingredient(item);
                    break;
                case "Snail":
                    Item = new Ingredient(item);
                    break;
                case "Periwinkle":
                    Item = new Ingredient(item);
                    break;
                case "Oyster":
                    Item = new Ingredient(item);
                    break;
                case "Maple Syrup":
                    Item = new Ingredient(item);
                    break;
                case "Oak Resin":
                    Item = new Ingredient(item);
                    break;
                case "Pine Tar":
                    Item = new Ingredient(item);
                    break;
                case "Chowder":
                    Item = new Ingredient(item);
                    break;
                case "Fish Stew":
                    Item = new Ingredient(item);
                    break;
                case "Escargot":
                    Item = new Ingredient(item);
                    break;
                case "Lobster Bisque":
                    Item = new Ingredient(item);
                    break;
                case "Maple Bar":
                    Item = new Ingredient(item);
                    break;
                case "Crab Cakes":
                    Item = new Ingredient(item);
                    break;
                case "Shrimp Cocktail":
                    Item = new Ingredient(item);
                    break;
                case "Woodskip":
                    Item = new Ingredient(item);
                    break;
                case "Strawberry Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Jack-O-Lantern":
                    Item = new Ingredient(item);
                    break;
                case "Rotten Plant":
                    Item = new Ingredient(item);
                    break;
                case "Omni Geode":
                    Item = new Ingredient(item);
                    break;
                case "Slime":
                    Item = new Ingredient(item);
                    break;
                case "Bat Wing":
                    Item = new Ingredient(item);
                    break;
                case "Solar Essence":
                    Item = new Ingredient(item);
                    break;
                case "Void Essence":
                    Item = new Ingredient(item);
                    break;
                case "Mixed Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Fiber":
                    Item = new Ingredient(item);
                    break;
                case "Oil of Garlic":
                    Item = new Ingredient(item);
                    break;
                case "Life Elixir":
                    Item = new Ingredient(item);
                    break;
                case "Wild Bait":
                    Item = new Ingredient(item);
                    break;
                case "Glacierfish":
                    Item = new Ingredient(item);
                    break;
                case "Battery Pack":
                    Item = new Ingredient(item);
                    break;
                case "Void Salmon":
                    Item = new Ingredient(item);
                    break;
                case "Slimejack":
                    Item = new Ingredient(item);
                    break;
                case "Pearl":
                    Item = new Ingredient(item);
                    break;
                case "Midnight Squid":
                    Item = new Ingredient(item);
                    break;
                case "Spook Fish":
                    Item = new Ingredient(item);
                    break;
                case "Blobfish":
                    Item = new Ingredient(item);
                    break;
                case "Cactus Seeds":
                    Item = new Ingredient(item);
                    break;
                case "Iridium Milk":
                    Item = new Ingredient(item);
                    break;
                case "Tree Fertilizer":
                    Item = new Ingredient(item);
                    break;
                case "Dinosaur Mayonnaise":
                    Item = new Ingredient(item);
                    break;
                case "Void Ghost Pendant":
                    Item = new Ingredient(item);
                    break;
                case "Movie Ticket":
                    Item = new Ingredient(item);
                    break;
                case "Roe":
                    Item = new Ingredient(item);
                    break;
                case "Squid Ink":
                    Item = new Ingredient(item);
                    break;
                case "Tea Leaves":
                    Item = new Ingredient(item);
                    break;

                default:
                    Item = new Ingredient(item);
                    break;
            }
            return Item;
            //return new Ingredient(item, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        }

        #region disposeable support
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Cauldron()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion disposeable support
    }
}
