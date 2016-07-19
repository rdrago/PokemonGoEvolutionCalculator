using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoCalculator
{
    public class Pokemon
    {
        public string Name { get; set; }
        public ulong Amount { get; set; }
        public ulong Candies { get; set; }

        public ulong CandiesToEvolve { get; set; }
    }
}
