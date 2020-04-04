using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LadderCalculator
{
    public partial class Form1 : Form
    {
        private List<PokemonGoLib.Pokemon> LoadedPokemon { get; set; }
        private Dictionary<int, List<PokemonGoLib.PowerUp>> ResultCache { get; set; }
        public Form1()
        {
            LoadedPokemon = new List<PokemonGoLib.Pokemon>();
            ResultCache = new Dictionary<int, List<PokemonGoLib.PowerUp>>();
            InitializeComponent();
        }

        private void UpdateDisplay()
        {

            LoadingForm loadingForm = new LoadingForm((int)numericUpDown3.Value, "Updating Display");
            loadingForm.Show();
            treeView1.Nodes.Clear();
            TreeNode baseNode = new TreeNode("CP");
            for (int cp = (int)numericUpDown2.Value; cp < (int)numericUpDown3.Value; ++cp)
            {
                TreeNode newNode = new TreeNode(cp.ToString());

                if(ResultCache.ContainsKey(cp))
                {
                    foreach( var result in ResultCache[cp])
                    {
                        if(result.PowerUpCount <= numericUpDown1.Value)
                        {
                            if (result.Pokemon.Saved || !onlySavedCheckBox.Checked)
                            {
                                if (result.Pokemon.Appraised || !onlyUniqueCheckBox.Checked)
                                {
                                    newNode.Nodes.Add(result.Report());
                                }
                            }
                        }
                    }
                }

                baseNode.Nodes.Add(newNode);

                loadingForm.UpdateProgress(cp, "Updating Display");
            }
            treeView1.Nodes.Add(baseNode);
            loadingForm.Close();
        }

        private void AnalyzePokemon()
        {
            ResultCache.Clear();
            LoadingForm loadingForm = new LoadingForm(LoadedPokemon.Count, "Calculating CP Coverage");
            loadingForm.Show();
            var ii = 0;
            foreach (var poke in LoadedPokemon)
            {
                var currentCP = poke.CP();
                var cps = poke.AllCPs();

                foreach (var cp in cps)
                {
                    if (!ResultCache.ContainsKey(cp.TargetCP))
                    {
                        ResultCache[cp.TargetCP] = new List<PokemonGoLib.PowerUp>();
                    }
                    ResultCache[cp.TargetCP].Add(cp);
                }
                loadingForm.UpdateProgress(++ii, "Calculating CP Coverage");
            }
            loadingForm.Close();
            UpdateDisplay();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                PokemonGoLib.CSVReader reader = new PokemonGoLib.CSVReader(openFileDialog1.FileName);
                reader.ProgressChangedEvent += Reader_ProgressChangedEvent;
                LoadedPokemon = reader.ReadFile();
                LoadingForm?.Close();
                LoadingForm = null;
                AnalyzePokemon();
            }
        }

        private static LoadingForm LoadingForm { get; set; }

        private void Reader_ProgressChangedEvent(object sender, PokemonGoLib.CSVReadProgress e)
        {
            if (LoadingForm == null)
            {
                LoadingForm = new LoadingForm(e.TotalLines, "Reading CSV File");
                LoadingForm.Show();
            }
            LoadingForm.UpdateProgress(e.ReadLines, "Reading CSV File...");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AnalyzePokemon();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void onlySavedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void onlyUniqueCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if(numericUpDown2.Value > numericUpDown3.Value)
            {
                numericUpDown3.Value = numericUpDown2.Value;
            }
            UpdateDisplay();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if(numericUpDown3.Value < numericUpDown2.Value)
            {
                numericUpDown2.Value = numericUpDown3.Value;
            }
            UpdateDisplay();
        }
    }
}
