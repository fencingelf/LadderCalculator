using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoLib
{
    public class Species
    {
        public static Species BadSpecies = new Species { BaseAttack = 0, BaseDefense = 0, BaseStamina = 0, Name = "INVALID" };
        public int PokedexNumber { get; set; }
        public string Name { get; set; }
        public int BaseStamina { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefense { get; set; }

        public int CP(double level, int atk, int def, int hp)
        {
            var levelCPBonus = Data.Levels.ContainsKey(level) ? Data.Levels[level].CPMultiplier : 0;
            return (int)Math.Floor(((BaseAttack + atk) * Math.Sqrt(BaseDefense + def) * Math.Sqrt(BaseStamina + hp) *
            Math.Pow(levelCPBonus, 2)) / 10.0);
        }

        public bool CanBeCP(int cp, double startingLevel, int atk, int def, int hp, out int numberOfPowerups)
        {
            var currentCP = CP(startingLevel, atk, def, hp);
            if (currentCP > cp)
            {
                numberOfPowerups = 0;
                return false;
            }
            var levelCheck = startingLevel + 0.5;
            numberOfPowerups = 1;
            while (levelCheck < 40.5)
            {
                var testCP = CP(levelCheck, atk, def, hp);
                if (testCP == cp)
                {
                    return true;
                }
                else if (testCP > cp)
                {
                    return false;
                }
                numberOfPowerups++;
                levelCheck += 0.5;
            }
            return false;
        }

    }
}
