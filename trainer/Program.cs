using System;
using System.Collections.Generic;
using System.IO;

namespace trainer
{
    class Program
    {
        static void Main()
        {
            string file = "C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\mozgas.txt";

            int selectedOption = 1;

            // Menü megjelenítése
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Mozgásnapló kezelő");
                Console.WriteLine($"1. Hozzáadás{(selectedOption == 1 ? " <---" : "")}");
                Console.WriteLine($"2. Megtekintés{(selectedOption == 2 ? " <---" : "")}");
                Console.WriteLine($"3. Kilépés{(selectedOption == 3 ? " <---" : "")}");

                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.DownArrow)
                {
                    selectedOption = (selectedOption % 3) + 1;
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    selectedOption = (selectedOption + 1) % 3 + 1;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    switch (selectedOption)
                    {
                        case 1:
                            AddEntry(file);
                            break;
                        case 2:
                            ViewEntries(file);
                            Console.WriteLine("\nNyomj meg egy gombot a folytatáshoz...");
                            Console.ReadKey();
                            break;
                        case 3:
                            Environment.Exit(0);
                            break;
                    }
                }
            }
        }

        static void AddEntry(string filePath)
        {
            Console.Clear();
           
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

            Console.Write("Edzés típusa: ");
            string exerciseType = Console.ReadLine();

            Console.Write("Időtartam (percben): ");
            int duration;
            while (!int.TryParse(Console.ReadLine(), out duration) || duration <= 0)
            {
                Console.WriteLine("Hibás input. Kérlek adj meg egy érvényes pozitív egész számot.");
                Console.Write("Időtartam (percben): ");
            }

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

        static bool IsValidDate(string date)
        {
            DateTime dummy;
            return DateTime.TryParse(date, out dummy);
        }
    }
}
