namespace ProjectPoo;

using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        Random rng = new Random();

        List<Func<Pokemon>> pokemons = new List<Func<Pokemon>>()
        {
            () => new Salameche(),
            () => new Carapuce(),
            () => new Bulbizarre(),
            () => new Pikachu(),
            () => new Racaillou(),
            () => new Roucool(),
            () => new Reptincel(),
            () => new Tortank(),
            () => new Herbizarre(),
            () => new Raichu(),
            () => new Grolem(),
            () => new Roucarnage(),
            () => new Feunard(),
            () => new Lokhlass(),
            () => new Florizarre()
        };

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("===== CHOISIS TON POKÉMON =====\n");
        Console.ResetColor();

        AfficherSelection(pokemons);

        int choix;
        Console.Write("\nTon choix : ");

        while (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > pokemons.Count)
        {
            Console.Write("Choix invalide, réessaie : ");
        }

        Pokemon joueur = pokemons[choix - 1]();

        int indexIA = rng.Next(pokemons.Count);
        Pokemon ennemi = pokemons[indexIA]();

        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Tu as choisi {joueur.Nom} !");
        Console.ResetColor();

        Thread.Sleep(1200);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"L'ennemi choisit {ennemi.Nom} !");
        Console.ResetColor();

        Thread.Sleep(1000);

        Combat.Duel(joueur, ennemi);
    }

    // 🔥 Affichage 3 cartes par ligne
    static void AfficherSelection(List<Func<Pokemon>> pokemons)
    {
        int cartesParLigne = 3;

        for (int i = 0; i < pokemons.Count; i += cartesParLigne)
        {
            var cartes = new List<string[]>();

            for (int j = 0; j < cartesParLigne && i + j < pokemons.Count; j++)
            {
                Pokemon p = pokemons[i + j]();
                cartes.Add(CreerCartePokemon(p, i + j + 1));
            }

            for (int ligne = 0; ligne < cartes[0].Length; ligne++)
            {
                foreach (var carte in cartes)
                {
                    Console.Write(carte[ligne].PadRight(28));
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }

    // 🧱 Création visuelle d'une carte
    static string[] CreerCartePokemon(Pokemon p, int index)
    {
        return new string[]
        {
            "==========================",
            $"[{index}] {p.Nom}",
            $"Type : {p.Type}",
            $"PV   : {p.PV}",
            $"ATK  : {p.Attaque}",
            $"PREC : {p.Precision}%",
            "=========================="
        };
    }
}