﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PokemonGoLib
{
    public class CSVReader
    {
        public string FileName { get; set; }

        public string Error { get; set; }

        public event EventHandler<CSVReadProgress> ProgressChangedEvent;

        public CSVReader(string fileName)
        {
            FileName = fileName;
        }
        public List<Pokemon> ReadFile()
        {
            List<Pokemon> output = new List<Pokemon>();
            Error = "";
            var lines = File.ReadAllLines(FileName);

            int ancestorField = -1;
            int uniqueField = -1;
            int savedField = -1;
            int speciesNameField = -1;
            int atkField = -1;
            int defField = -1;
            int stamField = -1;
            int levelField = -1;

            var header = lines[0].Split(',');
            int index = 0;

            while (index < header.Length)
            {
                if (header[index] == "Ancestor?") { ancestorField = index; }
                if (header[index] == "Name") { speciesNameField = index; }
                if (header[index] == "Level") { levelField = index; }
                if (header[index] == "ØATT IV") { atkField = index; }
                if (header[index] == "ØDEF IV") { defField = index; }
                if (header[index] == "ØHP IV") { stamField = index; }
                if (header[index] == "Unique?") { uniqueField = index; }
                if (header[index] == "Saved") { savedField = index; }
                index++;
            }

            if (speciesNameField == -1)
            {
                Error = "Invalid CSV: Could not detect species name field!";
                return output;
            }
            if (levelField == -1 || atkField == -1 || defField == -1 || stamField == -1)
            {
                Error = "Invalid CSV: Could not detect level, Attack, Defense, or HP fields!";
                return output;
            }

            string speciesName;
            double level = 0;
            int atk = 0, def = 0, hp = 0;
            bool saved = true, appraised = true;

            for (int ii = 1; ii < lines.Length; ++ii)
            {
                var line = lines[ii].Split(',');

                if (ancestorField >= 0 && int.Parse(line[ancestorField]) == 1) { continue; }
                if (savedField >= 0)
                {
                    saved = int.Parse(line[savedField]) == 1;
                }else { saved = true; }
                if (uniqueField >= 0)
                {
                    appraised = int.Parse(line[uniqueField]) == 1;
                }else { appraised = true; }

                speciesName = line[speciesNameField];
                try
                {
                    level = double.Parse(line[levelField]);
                    atk = (int)double.Parse(line[atkField]);
                    def = (int)double.Parse(line[defField]);
                    hp = (int)double.Parse(line[stamField]);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error on line {ii}, could not parse one of the essential fields!");
                }

                var poke = new PokemonGoLib.Pokemon(speciesName, level, atk, def, hp, saved, appraised);
                output.Add(poke);
                ProgressChangedEvent?.Invoke(this, new CSVReadProgress { ReadLines = ii, TotalLines = lines.Length });
            }

            return output;
        }
    }

    public class CSVReadProgress : EventArgs
    {
        public int TotalLines { get; set; }
        public int ReadLines { get; set; }
    }
}
