using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace trainer
{
    class Program
    {
        static void Main()
        {
            // A mozgásnapló fájl elérési útja
            string file = "C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\mozgas.txt";

            // A kiválasztott menüopció tárolása
            int selectedOption = 1;

            // Menü megjelenítése és kezelése
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Mozgásnapló kezelő");
                Console.WriteLine($"1. Hozzáadás{(selectedOption == 1 ? " <---" : "")}");
                Console.WriteLine($"2. Megtekintés{(selectedOption == 2 ? " <---" : "")}");
                Console.WriteLine($"3. Edzéstípusra szűrés{(selectedOption == 3 ? " <---" : "")}");
                Console.WriteLine($"4. Statisztika{(selectedOption == 4 ? " <---" : "")}");
                Console.WriteLine($"5. Kilépés{(selectedOption == 5 ? " <---" : "")}");

                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.DownArrow)
                {
                    selectedOption = (selectedOption % 5) + 1;
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    selectedOption = (selectedOption + 3) % 5 + 1;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    // A kiválasztott menüopció végrehajtása
                    switch (selectedOption)
                    {
                        case 1:
                            AddEntry(file); // Új edzés hozzáadása
                            break;
                        case 2:
                            ViewEntries(file); // Edzésnapló megjelenítése
                            Console.WriteLine("\nNyomj meg egy gombot a folytatáshoz...");
                            Console.ReadKey();
                            break;
                        case 3:
                            FilterByExerciseType(file); // Edzéstípus alapján szűrt adatok megjelenítése
                            Console.WriteLine("\nNyomj meg egy gombot a folytatáshoz...");
                            Console.ReadKey();
                            break;
                        case 4:
                            DisplayStatistics(file); // Statisztika megjelenítése
                            Console.WriteLine("\nNyomj meg egy gombot a folytatáshoz...");
                            Console.ReadKey();
                            break;
                        case 5:
                            Environment.Exit(0); // Kilépés a programból
                            break;
                    }
                }
            }
        }

        // Új edzés hozzáadása a naplóhoz
        static void AddEntry(string filePath)
        {
            Console.Clear();

            // Dátum bekérése
            Console.Write("Dátum (éééé.hh.nn): ");
            string dateInput = Console.ReadLine();

            string[] dateParts = dateInput.Split('.');
            if (dateParts.Length != 3 || !IsValidDate(dateInput))
            {
                Console.WriteLine("Érvénytelen dátumformátum. A helyes formátum: éééé.hh.nn");
                Console.WriteLine("\nNyomj meg egy gombot a folytatáshoz...");
                Console.ReadKey();
                return;
            }

            string year = dateParts[0];
            string month = dateParts[1];
            string day = dateParts[2];

            // Edzés típusa bekérése
            Console.Write("Edzés típusa: ");
            string exerciseType = Console.ReadLine();

            // Időtartam bekérése
            Console.Write("Időtartam (percben): ");
            int duration;
            while (!int.TryParse(Console.ReadLine(), out duration) || duration <= 0)
            {
                Console.WriteLine("Hibás input. Kérlek adj meg egy érvényes pozitív egész számot.");
                Console.Write("Időtartam (percben): ");
            }

            // Új bejegyzés létrehozása és hozzáadása a fájlhoz
            string newEntry = $"{year}.{month.PadLeft(2, '0')}.{day.PadLeft(2, '0')}\t{exerciseType}\t{duration}";

            try
            {
                File.AppendAllText(filePath, newEntry + Environment.NewLine);
                Console.WriteLine("Adatok sikeresen hozzáadva.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt az adatok hozzáadása közben: {ex.Message}");
            }

            Console.WriteLine("\nNyomj meg egy gombot a folytatáshoz...");
            Console.ReadKey();
        }

        // Edzésnapló megjelenítése
        static void ViewEntries(string filePath)
        {
            Console.Clear();
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                Console.WriteLine("Dátum\t\tTípus\t\tIdőtartam (perc)");

                foreach (string line in lines)
                {
                    string[] parts = line.Split('\t');

                    if (parts.Length == 3)
                    {
                        string date = parts[0];
                        string exerciseType = parts[1];
                        int duration = int.Parse(parts[2]);

                        Console.WriteLine($"{date}\t\t{exerciseType}\t\t{duration}");
                    }
                    else
                    {
                        Console.WriteLine("Hibás formátum a következő sorban: " + line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }
        }

        // Edzéstípus alapján szűrt adatok megjelenítése
        static void FilterByExerciseType(string filePath)
        {
            Console.Clear();

            // Edzéstípus bekérése
            Console.Write("Edzéstípus: ");
            string exerciseTypeFilter = Console.ReadLine();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                Console.WriteLine($"Edzéstípusra szűrt adatok ({exerciseTypeFilter}):");
                Console.WriteLine("Dátum\t\tTípus\t\tIdőtartam (perc)");

                foreach (string line in lines)
                {
                    string[] parts = line.Split('\t');

                    if (parts.Length == 3 && parts[1].Equals(exerciseTypeFilter, StringComparison.OrdinalIgnoreCase))
                    {
                        string date = parts[0];
                        string exerciseType = parts[1];
                        int duration = int.Parse(parts[2]);

                        Console.WriteLine($"{date}\t\t{exerciseType}\t\t{duration}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }
        }

        // Statisztika megjelenítése
        static void DisplayStatistics(string filePath)
        {
            Console.Clear();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // Edzéstípusokhoz tartozó adatok gyűjtése
                Dictionary<string, List<int>> exerciseData = new Dictionary<string, List<int>>();

                foreach (string line in lines)
                {
                    string[] parts = line.Split('\t');

                    if (parts.Length == 3)
                    {
                        string exerciseType = parts[1];
                        int duration = int.Parse(parts[2]);

                        if (!exerciseData.ContainsKey(exerciseType))
                        {
                            exerciseData[exerciseType] = new List<int>();
                        }

                        exerciseData[exerciseType].Add(duration);
                    }
                }

                // Statisztika megjelenítése
                Console.WriteLine("Statisztika edzéstípusonként:");
                Console.WriteLine("Edzéstípus\tEdzések száma\tÖsszideje (perc)\tÁtlagos idő (perc)\tLeghosszabb edzés (perc)");

                foreach (var kvp in exerciseData)
                {
                    string exerciseType = kvp.Key;
                    int count = kvp.Value.Count;
                    int totalDuration = kvp.Value.Sum();
                    int averageDuration = (int)kvp.Value.Average();
                    int maxDuration = kvp.Value.Max();

                    Console.WriteLine($"{exerciseType}\t\t{count}\t\t{totalDuration}\t\t{averageDuration}\t\t\t{maxDuration}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt: {ex.Message}");
            }
        }

        // Dátum érvényességének ellenőrzése
        static bool IsValidDate(string date)
        {
            DateTime dummy;
            return DateTime.TryParse(date, out dummy);
        }
    }
}
