using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonGoLib
{
    public class PowerUp
    {
        public Pokemon Pokemon { get; set; }
        public int CurrentCP { get { return Pokemon.CP(); }  }
        public int TargetCP { get; set; }
        public int PowerUpCount { get; set; }
        public int CandyCost { get; set; }
        public int StardustCost { get; set; }
        public int Evolutions { get; set; }

        public PowerUp(Pokemon mon, int target, int count, int candy, int stardust, int evolutions)
        {
            Pokemon = mon;
            TargetCP = target;
            PowerUpCount = count;
            CandyCost = candy;
            StardustCost = stardust;
            Evolutions = evolutions;
        }

        public string Report()
        {
            if (PowerUpCount == 0)
            {
                if (Evolutions == 0)
                {
                    return $"{Pokemon.Species.Name} at cp{CurrentCP} is ALREADY AT THIS CP";

                }
                else if (Evolutions == 1)
                {
                    return $"{Pokemon.Species.Name} at cp{CurrentCP} can reach cp{TargetCP} with ONE EVOLUTION";
                }
                else
                {
                    return $"{Pokemon.Species.Name} at cp{CurrentCP} can reach cp{TargetCP} with {Evolutions} EVOLUTIONS";

                }
            }
            else if (Evolutions == 0)
            {
                return $"{Pokemon.Species.Name} cp{CurrentCP} can reach cp{TargetCP} with {PowerUpCount} powerups, at a cost of {StardustCost} stardust and {CandyCost} candy";

            }
            else if (Evolutions == 1)
            {

                return $"{Pokemon.Species.Name} cp{CurrentCP} can reach cp{TargetCP} with ONE EVOLUTION AND {PowerUpCount} powerups, at a cost of {StardustCost} stardust and {CandyCost} candy (plus evolution candy)";

            }

            return $"{Pokemon.Species.Name} cp{CurrentCP} can reach cp{TargetCP} with {Evolutions} EVOLUTIONS AND {PowerUpCount} powerups, at a cost of {StardustCost} stardust and {CandyCost} candy (plus evolution candy)";
        }
    }
}
