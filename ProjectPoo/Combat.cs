namespace ProjectPoo;

public static class Combat
{
    static Random rng = new Random();

    public static void Duel(Pokemon joueur, Pokemon ennemi)
    {
        static void AfficherBarreVie(Pokemon p)
        {
            int tailleBarre = 40;

            double ratio = (double)p.PV / p.PVMax;
            int nbPleins = (int)(ratio * tailleBarre);
            int nbVides = tailleBarre - nbPleins;

            // Choix couleur selon PV
            if (ratio > 0.6)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (ratio > 0.3)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.Write($"{p.Nom} [");

            Console.Write(new string('#', nbPleins));
    
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string('-', nbVides));

            Console.ResetColor();

            Console.WriteLine($"] {p.PV}/{p.PVMax} PV");
        }
        
        static double GetMultiplicateur(TypePokemon attaquant, TypePokemon defenseur)
        {
            var table = new Dictionary<(TypePokemon, TypePokemon), double>
            {
                // 🔥 Feu
                { (TypePokemon.Feu, TypePokemon.Plante), 2 },
                { (TypePokemon.Feu, TypePokemon.Eau), 0.5 },
                { (TypePokemon.Feu, TypePokemon.Roche), 0.5 },

                // 💧 Eau
                { (TypePokemon.Eau, TypePokemon.Feu), 2 },
                { (TypePokemon.Eau, TypePokemon.Roche), 2 },
                { (TypePokemon.Eau, TypePokemon.Plante), 0.5 },

                // 🌿 Plante
                { (TypePokemon.Plante, TypePokemon.Eau), 2 },
                { (TypePokemon.Plante, TypePokemon.Roche), 2 },
                { (TypePokemon.Plante, TypePokemon.Feu), 0.5 },
                { (TypePokemon.Plante, TypePokemon.Vol), 0.5 },

                // ⚡ Electrique
                { (TypePokemon.Electrique, TypePokemon.Eau), 2 },
                { (TypePokemon.Electrique, TypePokemon.Vol), 2 },
                { (TypePokemon.Electrique, TypePokemon.Roche), 0.5 },

                // 🪨 Roche
                { (TypePokemon.Roche, TypePokemon.Feu), 2 },
                { (TypePokemon.Roche, TypePokemon.Vol), 2 },
                { (TypePokemon.Roche, TypePokemon.Eau), 0.5 },
                { (TypePokemon.Roche, TypePokemon.Plante), 0.5 },

                // 🌪️ Vol
                { (TypePokemon.Vol, TypePokemon.Plante), 2 },
                { (TypePokemon.Vol, TypePokemon.Electrique), 0.5 },
                { (TypePokemon.Vol, TypePokemon.Roche), 0.5 }
            };

            if (table.TryGetValue((attaquant, defenseur), out double multiplicateur))
                return multiplicateur;

            return 1; // neutre
        }
        
        static void ExecuterAction(Pokemon attaquant, Pokemon defenseur, int choix)
        {
            switch (choix)
            {
                case 1:
                    Attaque(attaquant, defenseur, 1.0, 90);
                    break;
                case 2:
                    Attaque(attaquant, defenseur, 1.3, 75);
                    break;
                case 3:
                    Attaque(attaquant, defenseur, 1.7, 55);
                    break;
                case 4:
                    Soin(attaquant);
                    break;
            }
        }
        
        static void Attaque(Pokemon attaquant, Pokemon defenseur, double coef, int precision)
        {
            int roll = rng.Next(1, 101);

            if (roll > precision)
            {
                Console.WriteLine("L'attaque rate !");
                return;
            }

            double multiplicateur = GetMultiplicateur(attaquant.Type, defenseur.Type);
            
            if (multiplicateur > 1)
                Console.WriteLine("C'est super efficace !");
            else if (multiplicateur < 1)
                Console.WriteLine("Ce n'est pas très efficace...");

            int degats = (int)(attaquant.Attaque * coef * multiplicateur);

            Console.WriteLine($"Ça touche ! ({multiplicateur}x)");
            defenseur.SubirDegats(degats);
        }
        
        static void Soin(Pokemon p)
        {
            int roll = rng.Next(1, 101);

            if (roll > 70)
            {
                Console.WriteLine("Le soin échoue !");
                return;
            }

            int heal = rng.Next(15, 30);
            p.PV += heal;

            Console.WriteLine($"{p.Nom} récupère {heal} PV !");
        }
        
        static int DemanderAction()
        {
            Console.WriteLine("\n1: Attaque classique");
            Console.WriteLine("2: Attaque osée");
            Console.WriteLine("3: Attaque risquée");
            Console.WriteLine("4: Soin");

            return int.Parse(Console.ReadLine());
        }
        
        //////////////////////////////////////////////////////////////// Boucle principale
        
        while (!joueur.EstKO() && !ennemi.EstKO())
        {
            Console.WriteLine($"\n{joueur.Nom} ({joueur.PV} PV) vs {ennemi.Nom} ({ennemi.PV} PV)");
            Console.WriteLine();
            AfficherBarreVie(joueur);
            AfficherBarreVie(ennemi);
            Thread.Sleep(1000);
                

            int choix = DemanderAction();
            Thread.Sleep(1000);

            ExecuterAction(joueur, ennemi, choix);
            Thread.Sleep(1500);

            if (ennemi.EstKO()) break;
            
            int choixIA = rng.Next(1, 5);
            Console.WriteLine($"\n{ennemi.Nom} joue...");
            ExecuterAction(ennemi, joueur, choixIA);
        }
        
        //////////////////////////////////////////////////////////////// Fin boucle principale
        static void AfficherResultat(Pokemon joueur)
        {
            Console.WriteLine();

            if (joueur.EstKO())
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("=======================================");
                Console.WriteLine("               DÉFAITE 💀              ");
                Console.WriteLine("=======================================");

                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("=======================================");
                Console.WriteLine("              VICTOIRE 🏆              ");
                Console.WriteLine("=======================================");

                Console.ResetColor();
            }
        }

        AfficherResultat(joueur);
    }
}
