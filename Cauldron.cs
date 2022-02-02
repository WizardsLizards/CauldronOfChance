using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CauldronOfChance
{
    class Cauldron
    {
        public static Ingredient getIngredient(Item item)
        {
            return new Ingredient(item);
            //return new Ingredient(item, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        }
    }
}
