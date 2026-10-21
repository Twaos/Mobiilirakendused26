using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Storage;

namespace Mobiilirakendused;

public static class FailiHaldur
{
    private static readonly string FailiTee = Path.Combine(FileSystem.AppDataDirectory, "retseptid.txt");

    
    public static void SalvestaRetsept(Retsept retsept)
    {
        try
        {
            string rida = $"{retsept.Nimi};{retsept.Kategooria};{retsept.PildiLink}";
            File.AppendAllText(FailiTee, rida + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Viga salvestamisel: {ex.Message}");
        }
    }

    
    public static List<Retsept> LoeRetseptid()
    {
        var nimekiri = new List<Retsept>();

        if (File.Exists(FailiTee))
        {
            string[] read = File.ReadAllLines(FailiTee);
            foreach (string rida in read)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(rida)) continue; 

                    string[] osad = rida.Split(';');
                    
                    if (osad.Length >= 3)
                    {
                        nimekiri.Add(new Retsept
                        {
                            Nimi = osad[0],
                            Kategooria = osad[1],
                            PildiLink = osad[2]
                        });
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Viga: {ex.Message}");
                }
            }
        }
        return nimekiri;
    }
}