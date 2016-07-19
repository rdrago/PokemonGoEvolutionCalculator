using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PokemonGoCalculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly private TimeSpan TimeToEvolve = TimeSpan.FromSeconds(30);

        public MainPage()
        {
            this.InitializeComponent();
            PokemonList.ItemsSource = new object[]
            {
                new Pokemon { Name = "Pidgey", CandiesToEvolve = 12},
                new Pokemon { Name = "Weedle", CandiesToEvolve = 12 },
                new Pokemon { Name = "Caterpie", CandiesToEvolve = 12 }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan totalTime = new TimeSpan();
            var exp = 0;
            List<EvolutionResult> results = new List<EvolutionResult>();

            foreach (var item in PokemonList.Items)
            {
                var pokemon = (Pokemon)item;
                var result = NumberOfEvolutions(pokemon);
                totalTime += result.TimeTaken;
                exp += (result.EvolveCount * 1000);
                results.Add(result);
            }

            Time.Text = $"{totalTime.ToString()}";
            EXP.Text = $"{exp} EXP";
            ResultList.ItemsSource = results;
        }

        public class EvolutionResult
        {
            public string Name { get; set; }
            public int EvolveCount { get; set; }
            public int TransferCount { get; set; }
            public TimeSpan TimeTaken { get; set; }
        }

        private EvolutionResult NumberOfEvolutions(Pokemon pokemon)
        {
            var amount = pokemon.Amount;
            var candies = pokemon.Candies;
            var candiesToEvolve = pokemon.CandiesToEvolve;

            var evolveCount = 0;
            var transferCount = 0;

            // Evolve w/o transfer
            while (((candies / candiesToEvolve) != 0) && 
                   (amount != 0))
            {
                --amount;                   // Evolve a pokemon
                candies -= candiesToEvolve; // Consume candies
                ++candies;                  // Gaining 1 candy from evolution
                ++evolveCount;
            }

            // Transfer and evolve
            while (((candies + amount) >= (candiesToEvolve + 1)) &&  // Still have enough candies
                   (amount != 0))                                    // Still have pokemons
            {
                // Transfer till enough to evolve
                while (candies < candiesToEvolve)
                {
                    --amount;
                    ++candies;
                    ++transferCount;
                }

                // Evolve
                --amount;                   // Evolve a pokemon
                candies -= candiesToEvolve; // Consume candies
                ++candies;                  // Gaining 1 candy from evolution
                ++evolveCount;  
            }

            var timeTaken = TimeSpan.FromSeconds(evolveCount * TimeToEvolve.TotalSeconds);

            return new EvolutionResult
            {
                Name = pokemon.Name,
                EvolveCount = evolveCount,
                TransferCount = transferCount,
                TimeTaken = timeTaken
            };
        }
    }
}
