using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("--- ĆWICZENIE 5.2: Inspekcja AppxBlockMap.xml ---");
        
        // Przygotowanie środowiska: symulacja rozpakowania archiwum ZIP (MSIX)
        Directory.CreateDirectory("msix-contents");
        string xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<AppxBlockMap>
  <File Name=""Lab09_PackagedApp.exe"">
    <Block Hash=""O8b1M...""/>
    <Block Hash=""P9c2N...""/>
    <Block Hash=""Q0d3O...""/>
  </File>
</AppxBlockMap>";
        await File.WriteAllTextAsync(Path.Combine("msix-contents", "AppxBlockMap.xml"), xmlContent);
        
        // Właściwa część zadania: Odczyt strumieniowy pliku z wnętrza pakietu
        var blockMapPath = Path.Combine("msix-contents", "AppxBlockMap.xml");
        
        await using var stream = new FileStream(blockMapPath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);

        int blockCount = 0;
        string? line;
        
        // Leniwa ewaluacja i strumieniowe czytanie linia po linii
        while ((line = await reader.ReadLineAsync()) is not null)
        {
            if (line.Contains("<Block")) 
            {
                blockCount++;
            }
        }

        Console.WriteLine($"[SUKCES] Odczytano strumieniowo liczbę bloków do aktualizacji: {blockCount}");
    }
}