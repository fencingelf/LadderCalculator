using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonGoLib
{
    public class PowerUp
    {
        public string Name { get; set; }
        public int CurrentCP { get; set; }
        public int TargetCP { get; set; }
        public int PowerUpCount { get; set; }
        public int CandyCost { get; set; }
        public int StardustCost { get; set; }
        public int Evolutions { get; set; }

        public PowerUp(string name, int current, int target, int count, int candy, int stardust, int evolutions)
        {
            Name = name;
            CurrentCP = current;
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
                    return $"{Name} at cp{CurrentCP} is ALREADY AT THIS CP";

                }
                else if (Evolutions == 1)
                {
                    return $"{Name} at cp{CurrentCP} can reach cp{TargetCP} with ONE EVOLUTION";
                }
                else
                {
                    return $"{Name} at cp{CurrentCP} can reach cp{TargetCP} with {Evolutions} EVOLUTIONS";

                }
            }
            else if (Evolutions == 0)
            {
                return $"{Name} cp{CurrentCP} can reach cp{TargetCP} with {PowerUpCount} powerups, at a cost of {StardustCost} stardust and {CandyCost} candy";

            }
            else if (Evolutions == 1)
            {

                return $"{Name} cp{CurrentCP} can reach cp{TargetCP} with ONE EVOLUTION AND {PowerUpCount} powerups, at a cost of {StardustCost} stardust and {CandyCost} candy (plus evolution candy)";

            }

            return $"{Name} cp{CurrentCP} can reach cp{TargetCP} with {Evolutions} EVOLUTIONS AND {PowerUpCount} powerups, at a cost of {StardustCost} stardust and {CandyCost} candy (plus evolution candy)";
        }
    }
}
