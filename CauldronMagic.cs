using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CauldronOfChance
{
    public class CauldronMagic : IDisposable
    {
        #region constructors
        public CauldronMagic()
        {

        }
        #endregion constructors

        #region functions
        public void UseCauldron(Item ingredient1, Item ingredient2, Item ingredient3)
        {
            Game1.addHUDMessage(new HUDMessage("Ingredient 1: " + ingredient1 + "Ingredient2 : " + ingredient2 + "Ingredient 3: " + ingredient3));

        }
        #endregion functions

        #region disposable support
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
        // ~CauldronMagic()
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
        #endregion disposably support
    }
}
