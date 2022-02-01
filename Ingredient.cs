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
        public double butterflies { get; set; } = 0.01;
        public double boom { get; set; } = 0.01;
        #endregion special events

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

            magneticRadiusDebuff1,
            magneticRadiusDebuff2,
            magneticRadiusDebuff3,

            speedDebuff1,
            speedDebuff2,
            speedDebuff3
        }
        #endregion debuffs

        public Ingredient (Item item, int bufferfliesChance = 0, int boomChance = 0, int garlicOil = 0, int monsterMusk = 0, int debuffImmunity = 0,
            int farming = 0, int mining = 0, int fishing = 0, int foragin = 0, int attack = 0, int defense = 0, int maxEnergy = 0, int luck = 0, int magneticRadius = 0, int speed = 0)
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

            addToCauldron(nameof(garlicOil), garlicOil);
            addToCauldron(nameof(monsterMusk), monsterMusk);
            addToCauldron(nameof(debuffImmunity), debuffImmunity);
            addToCauldron(nameof(farming), farming);
            addToCauldron(nameof(mining), mining);
            addToCauldron(nameof(fishing), fishing);
            addToCauldron(nameof(foragin), foragin);
            addToCauldron(nameof(attack), attack);
            addToCauldron(nameof(defense), defense);
            addToCauldron(nameof(maxEnergy), maxEnergy);
            addToCauldron(nameof(luck), luck);
            addToCauldron(nameof(magneticRadius), magneticRadius);
            addToCauldron(nameof(speed), speed);
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
                buffIndex2 = (int)Enum.Parse(typeof(buffs), buff + 2);
                buffIndex3 = (int)Enum.Parse(typeof(buffs), buff + 3);
            }
            if(debuff != null && debuff.Equals("") == false)
            {
                debuffIndex1 = (int)Enum.Parse(typeof(buffs), debuff + 1);
                debuffIndex2 = (int)Enum.Parse(typeof(buffs), debuff + 2);
                debuffIndex3 = (int)Enum.Parse(typeof(buffs), debuff + 3);
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