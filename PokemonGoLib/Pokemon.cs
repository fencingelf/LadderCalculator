﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoLib
{
    public class Pokemon
    {
        public Species Species { get; set; }
        public int AttackBonus { get; set; }
        public int DefenseBonus { get; set; }
        public int StaminaBonus { get; set; }
        public double CurrentLevel { get; set; }

        public Pokemon(string speciesName, double level, int atk, int def, int stam)
        {
            AttackBonus = atk;
            DefenseBonus = def;
            StaminaBonus = stam;
            CurrentLevel = level;
            if (Data.SpeciesData.ContainsKey(speciesName))
            {
                Species = Data.SpeciesData[speciesName];
            }
            else
            {
                var speciesNameTmp = speciesName.Replace("Normal", "").Replace("Purified", "").Replace("Shadow", "").Replace("Galarian", "").Trim();

                if (speciesNameTmp.Contains("Alola"))
                {
                    speciesNameTmp = speciesNameTmp.Replace("Alolan", "").Trim();
                    speciesNameTmp = "Alolan " + speciesNameTmp;

                }

                // Species-specific hacks
                if (speciesNameTmp.Contains("Red Stripe") || speciesNameTmp.Contains("Blue Stripe"))
                {
                    speciesNameTmp = "Basculin";
                }
                if (speciesNameTmp.Contains("Overcast") || speciesNameTmp.Contains("Sunshine"))
                {
                    speciesNameTmp = "Cherrim";
                }
                if (speciesNameTmp.Contains("West Sea") || speciesNameTmp.Contains("East Sea"))
                {
                    speciesNameTmp = speciesNameTmp.Replace("West Sea", "").Replace("East Sea", "").Trim();
                }

                if (Data.SpeciesData.ContainsKey(speciesNameTmp))
                {
                    Species = Data.SpeciesData[speciesNameTmp];
                }
            }
            if (Species == null)
            {
                Species = Species.BadSpecies;
            }
        }

        public int CP()
        {
            return Species.CP(CurrentLevel, AttackBonus, DefenseBonus, StaminaBonus);
        }

        public int CP(double level)
        {
            return Species.CP(level, AttackBonus, DefenseBonus, StaminaBonus);
        }

        public List<PowerUp> AllCPs()
        {
            List<PowerUp> ret = new List<PowerUp>();
            double level = CurrentLevel;
            int stardust = 0;
            int candy = 0;

            List<Species> evo1 = new List<Species>();
            List<Species> evo2 = new List<Species>();

            foreach (var fam in Data.Families)
            {
                if (fam.Basic == Species.Name)
                {
                    if (fam.FirstEvolution != null)
                    {
                        foreach (var evo in fam.FirstEvolution)
                            evo1.Add(Data.SpeciesData[evo]);
                    }
                    if (fam.SecondEvolution != null)
                    {
                        foreach (var evo in fam.SecondEvolution)
                            evo2.Add(Data.SpeciesData[evo]);
                    }
                }
                if (fam.FirstEvolution.Contains(Species.Name))
                {
                    if (fam.SecondEvolution != null)
                    {
                        foreach (var evo in fam.SecondEvolution)
                            evo1.Add(Data.SpeciesData[evo]);
                    }
                }
            }

            int levelUps = 0;
            var currentCP = CP(level);
            while (level < 40.5)
            {
                ret.Add(new PowerUp(Species.Name, currentCP, CP(level), levelUps, candy, stardust, 0));
                foreach (var evo in evo1)
                {
                    ret.Add(new PowerUp(Species.Name, currentCP, evo.CP(level, AttackBonus, DefenseBonus, StaminaBonus), levelUps, candy, stardust, 1));
                }
                foreach (var evo in evo2)
                {
                    ret.Add(new PowerUp(Species.Name, currentCP,  evo.CP(level, AttackBonus, DefenseBonus, StaminaBonus), levelUps, candy, stardust, 2));
                }

                candy += Data.Levels[level].Candy;
                stardust += Data.Levels[level].Stardust;
                level += 0.5;
                levelUps++;
            }

            return ret;
        }

        public bool CanBeCP(int cp, out int numberOfPowerups, out string evolveName)
        {
            evolveName = "";
            if (Species.CanBeCP(cp, CurrentLevel, AttackBonus, DefenseBonus, StaminaBonus, out numberOfPowerups))
            {
                return true;
            }

            var familyData = Family.FamilyData.NotInFamily;
            var fam = Data.Families.FirstOrDefault(f => (familyData = f.HasPokemon(Species.Name)) != Family.FamilyData.NotInFamily);

            if (familyData.HasFlag(Family.FamilyData.HasFirstEvolution))
            {
                foreach (var evo in fam.FirstEvolution)
                    if (Data.SpeciesData[evo].CanBeCP(cp, CurrentLevel, AttackBonus, DefenseBonus, StaminaBonus, out numberOfPowerups))
                    {
                        evolveName = evo;
                        return true;
                    }
            }
            if (familyData.HasFlag(Family.FamilyData.HasSecondEvolution))
            {
                foreach (var evo in fam.SecondEvolution)
                    if (Data.SpeciesData[evo].CanBeCP(cp, CurrentLevel, AttackBonus, DefenseBonus, StaminaBonus, out numberOfPowerups))
                    {
                        evolveName = evo;
                        return true;
                    }
            }

            return false;
        }
    }
}