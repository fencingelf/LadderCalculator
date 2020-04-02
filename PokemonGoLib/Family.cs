using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PokemonGoLib
{
    public class Family
    {
        [Flags]
        public enum FamilyData
        {
            NotInFamily,
            IsBasic,
            IsFirstEvolution,
            IsSecondEvolution,
            HasFirstEvolution,
            HasSecondEvolution
        }
        public string Basic { get; set; }
        public List<string> FirstEvolution { get; set; }
        public int FirstEvolutionCandy { get; set; }
        public List<string> SecondEvolution { get; set; }
        public int SecondEvolutionCandy { get; set; }

        public FamilyData HasPokemon(string pokemonName)
        {
            if (pokemonName.Contains(Basic))
            {
                return FamilyData.IsBasic | FamilyData.HasFirstEvolution | (SecondEvolution.Count > 0 ? FamilyData.HasSecondEvolution : 0);
            }
            if (FirstEvolution.Any(n => pokemonName.Contains(n)))
            {
                return FamilyData.IsFirstEvolution | (SecondEvolution.Count > 0 ? FamilyData.HasSecondEvolution : 0);
            }
            if (SecondEvolution.Any(n=>pokemonName.Contains(n)))
            {
                return FamilyData.IsSecondEvolution;
            }
            return FamilyData.NotInFamily;
        }
    }
}
