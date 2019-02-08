using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sanitize_legendary_card_list
{
    class Program
    {
        private const string InputDirectory = @"C:\Code\Git\legendary-complete-card-txt";
        private const string OutputDirectory = @"C:\Code\Git\Avery16282Generators\Avery16282Generator\Legendary\Data";

        private const string InputHeroesAndAlliesFileName = "2_heroes_and_allies.txt";
        private const string OutputHeroesAndAlliesFileName = InputHeroesAndAlliesFileName + "_Sanitized";
        private static string HeroesAndAlliesInputPath => Path.Combine(InputDirectory, InputHeroesAndAlliesFileName);
        private static string HeroesAndAlliesOutputPath => Path.Combine(OutputDirectory, OutputHeroesAndAlliesFileName);

        private const string OutputMastermindsAndCommandersFileName = InputMastermindsAndCommandersFileName + "_Sanitized";
        private const string InputMastermindsAndCommandersFileName = "4_masterminds_and_commanders.txt";
        private static string MastermindAndCommandersInputPath => Path.Combine(InputDirectory, InputMastermindsAndCommandersFileName);
        private static string MastermindAndCommandersOutputPath => Path.Combine(OutputDirectory, OutputMastermindsAndCommandersFileName);

        private const string OutputHenchmenAndBackupFileName = InputHenchmenAndBackupFileName + "_Sanitized";
        private const string InputHenchmenAndBackupFileName = "6_henchmen_and_backup.txt";
        private static string HenchmenAndBackupInputPath => Path.Combine(InputDirectory, InputHenchmenAndBackupFileName);
        private static string HenchmenAndBackupOutputPath => Path.Combine(OutputDirectory, OutputHenchmenAndBackupFileName);

        private const string InputVillainsAndAdversariesFileName = "5_villain_and_adversary_groups.txt";
        private const string OutputVillainsAndAdversariesFileName = InputVillainsAndAdversariesFileName + "_Sanitized";
        private static string VillainsAndAdversariesInputPath => Path.Combine(InputDirectory, InputVillainsAndAdversariesFileName);
        private static string VillainsAndAdversariesOutputPath => Path.Combine(OutputDirectory, OutputVillainsAndAdversariesFileName);



        static void Main(string[] args)
        {
            SanitizeHeroesAndAllies();
            SanitizeMastermindAndCommanders();
            SanitizeHenchmenAndBackup();
            SanitizeVillainsAndAllies();
        }

        private static void SanitizeVillainsAndAllies()
        {
            var thingsToRemove = new[]
            {
                "(jump to top)\r\n",
            };
            var text = File.ReadAllText(VillainsAndAdversariesInputPath);
            text = text.Substring(text.IndexOf('\n') + 5);
            text = Regex.Replace(text, @"\[[^\[]+\]", "");
            foreach (var thingToRemove in thingsToRemove)
            {
                text = text.Replace(thingToRemove, "");
            }

            File.WriteAllText(VillainsAndAdversariesOutputPath, text);
        }

        private static void SanitizeHenchmenAndBackup()
        {
            var thingsToRemove = new[]
            {
                "(jump to top)\r\n",
            };
            var text = File.ReadAllText(HenchmenAndBackupInputPath);
            text = text.Substring(text.IndexOf('\n') + 3);
            text = Regex.Replace(text, @"\[[^\[]+\]", "");
            foreach (var thingToRemove in thingsToRemove)
            {
                text = text.Replace(thingToRemove, "");
            }

            File.WriteAllText(HenchmenAndBackupOutputPath, text);
        }

        private static void SanitizeMastermindAndCommanders()
        {
            var thingsToRemove = new[]
            {
                "(jump to top)\r\n",
            };

            var text = File.ReadAllText(MastermindAndCommandersInputPath);
            text = text.Substring(text.IndexOf('\n') + 3);
            text = Regex.Replace(text, @"\[[^\[]+\]", "");

            foreach (var thingToRemove in thingsToRemove)
            {
                text = text.Replace(thingToRemove, "");
            }

            var removeToken = "REMOVETHIS";

            var lines = text.Split(new [] {Environment.NewLine}, StringSplitOptions.None).ToList();
            var isInEpicMastermind = false;
            for(var i = 0; i < lines.Count; i++)
            {
                if (isInEpicMastermind)
                {
                    if (string.IsNullOrWhiteSpace(lines[i]))
                        isInEpicMastermind = false;
                    else
                    {
                        lines[i] = "EPIC: " + lines[i];
                    }
                }
                else
                {
                    if (lines[i].StartsWith("Epic"))
                    {
                        isInEpicMastermind = true;
                        lines[i - 1] = removeToken;
                        lines[i] = removeToken;
                    }
                }
            }

            lines = lines.Where(line => line != removeToken).ToList();
            File.WriteAllLines(MastermindAndCommandersOutputPath, lines);
        }

        private static void SanitizeHeroesAndAllies()
        {
            var thingsToRemove = new[]
            {
                "(Note: 3D alt art has no flavor text.)\r\n",
                "Art contains a gun\r\n",
                "(jump to top)\r\n",
                "3D alt art flavor text: You won't like me when I'm angry.\r\n",
                "Art contains a gun, or at least muzzle flashes and bullet trajectory\r\n"
            };
            var text = File.ReadAllText(HeroesAndAlliesInputPath);
            text = text.Substring(text.IndexOf('\n') + 3);
            text = Regex.Replace(text, @"\[[^\[]+\]", "");
            foreach (var thingToRemove in thingsToRemove)
            {
                text = text.Replace(thingToRemove, "");
            }

            File.WriteAllText(HeroesAndAlliesOutputPath, text);
        }
    }
}
