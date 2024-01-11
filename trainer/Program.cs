namespace trainer
{
    class Program
    {
        static void Main()
        {
            string file = "F:\\iskola\\11\\ikt\\mentes\\mozgas.txt";

            try
            {
                // Beolvassuk az állomány sorait
                string[] lines = File.ReadAllLines(file);

                // Adatok feldolgozása és kiírása
                foreach (string line in lines)
                {
                    string[] parts = line.Split('\t'); // Tabulátorral tagolt sor szétválasztása

                    if (parts.Length == 3) // Ellenőrzés, hogy minden szükséges adat elérhető-e
                    {
                        string date = parts[0];
                        string exerciseType = parts[1];
                        int duration = int.Parse(parts[2]);

                        // Adatok feldolgozása, például kiírás
                        Console.WriteLine($"Dátum: {date}, Típus: {exerciseType}, Időtartam: {duration} perc");
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
    }
}
