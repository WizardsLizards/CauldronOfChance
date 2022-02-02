using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CauldronOfChance
{
    public class Ingredient
    {
        Item item;

        #region special events
        public double butterflies { get; set; } = 0;
        public double boom { get; set; } = 0;
        public int cooking { get; set; } = 0;
        public double cauldronLuck { get; set; } = 0;
        #endregion special events

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

        public Ingredient (Item item, int bufferfliesChance = 0, int boomChance = 0, int garlicOil = 0, int monsterMusk = 0, int debuffImmunity = 0,
            int farming = 0, int mining = 0, int fishing = 0, int foraging = 0, int attack = 0, int defense = 0, int maxEnergy = 0, int luck = 0, int magneticRadius = 0, int speed = 0, int cauldronLuck = 0)
        {
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

            this.item = item;

            if (bufferfliesChance > 0)
            {
                this.butterflies += 0.1 * bufferfliesChance;
            }
            if (boomChance > 0)
            {
                this.boom += 0.1 * boomChance;
            }

            this.cauldronLuck += cauldronLuck * 0.1;

            addToCauldron(nameof(garlicOil), garlicOil);
            addToCauldron(nameof(monsterMusk), monsterMusk);
            addToCauldron(nameof(debuffImmunity), debuffImmunity);
            addToCauldron(nameof(farming), farming);
            addToCauldron(nameof(mining), mining);
            addToCauldron(nameof(fishing), fishing);
            addToCauldron(nameof(foraging), foraging);
            addToCauldron(nameof(attack), attack);
            addToCauldron(nameof(defense), defense);
            addToCauldron(nameof(maxEnergy), maxEnergy);
            addToCauldron(nameof(luck), luck);
            addToCauldron(nameof(magneticRadius), magneticRadius);
            addToCauldron(nameof(speed), speed);

            addForCategory();
            addForFoodBuffs();
            addForQuality();
            addForEdibility();
            addForMoneyValue();
            addForCooking();
        }

        public void addForCategory()
        {
            switch (item.Category)
            {
                case StardewValley.Object.artisanGoodsCategory:
                    addToCauldron("farming", 1);
                    break;
                case StardewValley.Object.baitCategory:
                    addToCauldron("fishing", 1);
                    break;
                case StardewValley.Object.CookingCategory:
                    addToCauldron("maxEnergy", 1);
                    break;
                case StardewValley.Object.CraftingCategory:
                    addToCauldron("defense", 1);
                    break;
                case StardewValley.Object.EggCategory:
                    addToCauldron("farming", 1);
                    break;
                case StardewValley.Object.fertilizerCategory:
                    addToCauldron("farming", 1);
                    break;
                case StardewValley.Object.FishCategory:
                    addToCauldron("fishing", 1);
                    break;
                case StardewValley.Object.flowersCategory:
                    addToCauldron("foraging", 1);
                    break;
                case StardewValley.Object.FruitsCategory:
                    addToCauldron("farming", 1);
                    break;
                case StardewValley.Object.GemCategory:
                    addToCauldron("mining", 1);
                    break;
                case StardewValley.Object.GreensCategory:
                    addToCauldron("foraging", 1);
                    break;
                case StardewValley.Object.ingredientsCategory:
                    addToCauldron("maxEnergy", 1);
                    break;
                case StardewValley.Object.junkCategory:
                    this.boom += 0.2;
                    break;
                case StardewValley.Object.MilkCategory:
                    addToCauldron("farming", 1);
                    break;
                case StardewValley.Object.mineralsCategory:
                    addToCauldron("mining", 1);
                    break;
                case StardewValley.Object.monsterLootCategory:
                    addToCauldron("attack", 1);
                    break;
                case StardewValley.Object.SeedsCategory:
                    addToCauldron("farming", 1);
                    break;
                case StardewValley.Object.syrupCategory:
                    addToCauldron("farming", 1);
                    break;
                case StardewValley.Object.tackleCategory:
                    addToCauldron("fishing", 1);
                    break;
                case StardewValley.Object.VegetableCategory:
                    addToCauldron("farming", 1);
                    break;
            }
        }

        public void addForFoodBuffs()
        {
            if(item is StardewValley.Object)
            {
                StardewValley.Object csObject = item as StardewValley.Object;

                string[] objectDescription = Game1.objectInformation[csObject.ParentSheetIndex].Split('/');

                if (Convert.ToInt32(objectDescription[2]) > 0)
                {
                    string[] whatToBuff = (string[])((objectDescription.Length > 7) ? ((object)objectDescription[7].Split(' ')) : ((object)new string[12]
                    {
                        "0", "0", "0", "0", "0", "0", "0", "0", "0", "0",
                        "0", "0"
                    }));

                    csObject.ModifyItemBuffs(whatToBuff);

                    //Buff buff = new Buff(Convert.ToInt32(whatToBuff[0]), Convert.ToInt32(whatToBuff[1]), Convert.ToInt32(whatToBuff[2]), Convert.ToInt32(whatToBuff[3]), Convert.ToInt32(whatToBuff[4]), Convert.ToInt32(whatToBuff[5]), Convert.ToInt32(whatToBuff[6]), Convert.ToInt32(whatToBuff[7]), Convert.ToInt32(whatToBuff[8]), Convert.ToInt32(whatToBuff[9]), Convert.ToInt32(whatToBuff[10]), (whatToBuff.Length > 11) ? Convert.ToInt32(whatToBuff[11]) : 0, duration, objectDescription[0], objectDescription[4]);

                    addToCauldron("farming", Convert.ToInt32(whatToBuff[0]));
                    addToCauldron("mining", Convert.ToInt32(whatToBuff[2]));
                    addToCauldron("fishing", Convert.ToInt32(whatToBuff[1]));
                    addToCauldron("foraging", Convert.ToInt32(whatToBuff[5]));
                    addToCauldron("attack", (whatToBuff.Length > 11) ? Convert.ToInt32(whatToBuff[11]) : 0);
                    addToCauldron("defense", Convert.ToInt32(whatToBuff[10]));
                    addToCauldron("maxEnergy", Convert.ToInt32(whatToBuff[7]) / 10);
                    addToCauldron("luck", Convert.ToInt32(whatToBuff[4]));
                    addToCauldron("magneticRadius", Convert.ToInt32(whatToBuff[8]) / 32);
                    addToCauldron("speed", Convert.ToInt32(whatToBuff[9]));
                }
            }
        }

        public void addForQuality()
        {
            if (item is StardewValley.Object)
            {
                StardewValley.Object csObject = item as StardewValley.Object;

                cauldronLuck += csObject.Quality * 0.05;
            }
        }

        public void addForEdibility()
        {
            if (item is StardewValley.Object)
            {
                StardewValley.Object csObject = item as StardewValley.Object;

                if(csObject.Edibility > -300)
                {
                    cauldronLuck += (csObject.Edibility / 100) * 0.05;
                }
            }
        }
        
        public void addForMoneyValue()
        {
            cauldronLuck += (item.salePrice() / 1000) * 0.02;
        }

        public void addForCooking()
        {
            //TODO
        }

        public void addToCauldron(string name, int value)
        {
            if (name.Equals("garlicOil"))
            {
                addToCauldron(name, "monsterMusk", value > 0 ? 1 : 0);
            }
            else if (name.Equals("monsterMusk"))
            {
                addToCauldron("garlicOil", name, value > 0 ? 1 : 0);
            }
            else if (name.Equals("debuffImmunity"))
            {
                addToCauldron(name, "", value > 0 ? 1 : 0);
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

            if(buff != null && buff.Equals("") == false)
            {
                buffIndex1 = (int)Enum.Parse(typeof(buffs), buff + 1);

                if(value >= 2)
                {
                    buffIndex2 = (int)Enum.Parse(typeof(buffs), buff + 2);
                }

                if(value >= 3)
                {
                    buffIndex3 = (int)Enum.Parse(typeof(buffs), buff + 3);
                }
            }
            if(debuff != null && debuff.Equals("") == false)
            {
                debuffIndex1 = (int)Enum.Parse(typeof(debuffs), debuff + 1);

                if (value >= 2)
                {
                    debuffIndex2 = (int)Enum.Parse(typeof(debuffs), debuff + 2);
                }

                if (value >= 3)
                {
                    debuffIndex3 = (int)Enum.Parse(typeof(debuffs), debuff + 3);
                }
            }

            if (value >= 3)
            {
                if (buff != null && buff.Equals("") == false)
                {
                    buffList[buffIndex3] += 3;
                    buffList[buffIndex2] += 2;
                    buffList[buffIndex1] += 1;
                }

                if (debuff != null && debuff.Equals("") == false)
                {
                    debuffList[debuffIndex2] += 1;
                    debuffList[debuffIndex1] += 1;
                }
            }
            else if (value == 2)
            {
                if (buff != null && buff.Equals("") == false)
                {
                    buffList[buffIndex2] += 3;
                    buffList[buffIndex1] += 2;
                }

                if (debuff != null && debuff.Equals("") == false)
                {
                    debuffList[debuffIndex1] += 1;
                }
            }
            else if (value == 1)
            {
                if (buff != null && buff.Equals("") == false)
                {
                    buffList[buffIndex1] += 3;
                }

                if (debuff != null && debuff.Equals("") == false)
                {
                    debuffList[debuffIndex1] += 1;
                }
            }
            else if (value == -1)
            {
                if (debuff != null && debuff.Equals("") == false)
                {
                    debuffList[debuffIndex1] += 3;
                }

                if (buff != null && buff.Equals("") == false)
                {
                    buffList[buffIndex1] += 1;
                }
            }
            else if (value == -2)
            {
                if (debuff != null && debuff.Equals("") == false)
                {
                    debuffList[debuffIndex2] += 3;
                    debuffList[debuffIndex1] += 2;
                }

                if (buff != null && buff.Equals("") == false)
                {
                    buffList[buffIndex1] += 1;
                }
            }
            else if (value <= -3)
            {
                if (debuff != null && debuff.Equals("") == false)
                {
                    debuffList[debuffIndex3] += 3;
                    debuffList[debuffIndex2] += 2;
                    debuffList[debuffIndex1] += 1;
                }

                if (buff != null && buff.Equals("") == false)
                {
                    buffList[buffIndex2] += 1;
                    buffList[buffIndex1] += 1;
                }
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
    }
}