namespace ProjectPoo;

public enum TypePokemon
{
    Feu,
    Eau,
    Plante,
    Electrique,
    Roche,
    Vol
}

///////////////////////////////////////////// Classe abstraite
public abstract class Pokemon
{
    public string Nom { get; set; }
    public int PV { get; set; }
    public int PVMax { get; set; }
    public int Attaque { get; set; }
    public int Precision { get; set; }
    public TypePokemon Type { get; set; }

    public Pokemon(string nom, int pv, int attaque, int precision, TypePokemon type)
    {
        Nom = nom;
        PV = pv;
        Attaque = attaque;
        Precision = precision;
        Type = type;
        PVMax = pv;
    }
    
    public bool EstKO()
    {
        return PV <= 0;
    }
    
    public virtual void SubirDegats(int degats)
    {
        PV -= degats;
        Console.WriteLine($"{Nom} perd {degats} PV (reste {PV})");
    }
}

///////////////////////////////////////////// Classes Spécifiques

public class Salameche : Pokemon
{
    public Salameche() : base("Salamèche", 90, 22, 85, TypePokemon.Feu) { }
}

public class Reptincel : Pokemon
{
    public Reptincel() : base("Reptincel", 110, 26, 80, TypePokemon.Feu) { }
}

public class Feunard : Pokemon
{
    public Feunard() : base("Feunard", 100, 24, 90, TypePokemon.Feu) { }
}

public class Carapuce : Pokemon
{
    public Carapuce() : base("Carapuce", 100, 20, 90, TypePokemon.Eau) { }
}

public class Tortank : Pokemon
{
    public Tortank() : base("Tortank", 130, 25, 85, TypePokemon.Eau) { }
}

public class Lokhlass : Pokemon
{
    public Lokhlass() : base("Lokhlass", 140, 22, 80, TypePokemon.Eau) { }
}

public class Bulbizarre : Pokemon
{
    public Bulbizarre() : base("Bulbizarre", 95, 21, 90, TypePokemon.Plante) { }
}

public class Herbizarre : Pokemon
{
    public Herbizarre() : base("Herbizarre", 110, 23, 85, TypePokemon.Plante) { }
}

public class Florizarre : Pokemon
{
    public Florizarre() : base("Florizarre", 130, 25, 80, TypePokemon.Plante) { }
}

public class Pikachu : Pokemon
{
    public Pikachu() : base("Pikachu", 85, 24, 95, TypePokemon.Electrique) { }
}

public class Raichu : Pokemon
{
    public Raichu() : base("Raichu", 100, 28, 90, TypePokemon.Electrique) { }
}

public class Racaillou : Pokemon
{
    public Racaillou() : base("Racaillou", 120, 23, 75, TypePokemon.Roche) { }
}

public class Grolem : Pokemon
{
    public Grolem() : base("Grolem", 150, 30, 70, TypePokemon.Roche) { }
}

public class Roucool : Pokemon
{
    public Roucool() : base("Roucool", 80, 18, 95, TypePokemon.Vol) { }
}

public class Roucarnage : Pokemon
{
    public Roucarnage() : base("Roucarnage", 110, 26, 90, TypePokemon.Vol) { }
}