using System.Diagnostics;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

long tempsSequentiel = ConversionSequentielle();
long tempsParallele = ConversionParallele();

Console.WriteLine($"\n--- Résultats ---");
Console.WriteLine($"Séquentiel : {tempsSequentiel}ms");
Console.WriteLine($"Parallèle  : {tempsParallele}ms");

long ConversionSequentielle()
{
    var sw = Stopwatch.StartNew();

    string projetRacine = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string inputFolder = Path.Combine(projetRacine, "PicturesInput");
    string outputFolder = Path.Combine(projetRacine, "PicturesOutput");

    string[] extensions = [".jpg", ".jpeg", ".png"];

    // On récupère les images qui sont au format dans la liste "extensions"
    List<string> imageFiles = Directory.GetFiles(inputFolder)
        .Where(f => extensions.Contains(Path.GetExtension(f).ToLower()))
        .ToList();

    // On défini les HAUTEURS d'images souhaitées
    List<(int? Hauteur, string Dossier)> resolutions =
    [
        (480,  "480p"),
        (720,  "720p"),
        (1080, "1080p"),
        (null, "original"),
    ];

    // On crée les dossiers de sortie avec le nom correspondant à la résolution désirée de sortie
    foreach (var resolution in resolutions)
    {
        Directory.CreateDirectory(Path.Combine(outputFolder, resolution.Dossier));
    }

    // Traitement de chaque image
    foreach (string fichier in imageFiles)
    {
        string nomFichier = Path.GetFileNameWithoutExtension(fichier);
        Console.WriteLine($"Traitement : {fichier}");

        foreach (var resolution in resolutions)
        {
            using Image image = Image.Load(fichier);

            if (resolution.Hauteur.HasValue)
            {
                int nouvelleHauteur = resolution.Hauteur.Value;
                int nouvelleLargeur = (int)((double)image.Width / image.Height * nouvelleHauteur);
                image.Mutate(x => x.Resize(nouvelleLargeur, nouvelleHauteur));
            }

            string cheminSortie = Path.Combine(outputFolder, resolution.Dossier, $"{nomFichier}.webp");
            image.Save(cheminSortie, new WebpEncoder());
            Console.WriteLine($"  -> {cheminSortie} ({image.Width}x{image.Height})");
        }
    }

    Console.WriteLine($"\n{imageFiles.Count} image(s) converties.");
    return sw.ElapsedMilliseconds;
}

long ConversionParallele()
{
    var sw = Stopwatch.StartNew();

    string projetRacine = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string inputFolder = Path.Combine(projetRacine, "PicturesInput");
    string outputFolder = Path.Combine(projetRacine, "PicturesOutput");

    string[] extensions = [".jpg", ".jpeg", ".png"];

    // On récupère les images qui sont au format dans la liste "extensions"
    List<string> imageFiles = Directory.GetFiles(inputFolder)
        .Where(f => extensions.Contains(Path.GetExtension(f).ToLower()))
        .ToList();

    // On défini les HAUTEURS d'images souhaitées
    List<(int? Hauteur, string Dossier)> resolutions =
    [
        (480,  "480p"),
        (720,  "720p"),
        (1080, "1080p"),
        (null, "original"),
    ];

    // On crée les dossiers de sortie avec le nom correspondant à la résolution désirée de sortie
    foreach (var resolution in resolutions)
    {
        Directory.CreateDirectory(Path.Combine(outputFolder, resolution.Dossier));
    }

    int totalConverties = 0;

    object lockObj = new object();

    var options = new ParallelOptions
    {
        // 8 Threads
        MaxDegreeOfParallelism = 8
    };

    // Traitement de chaque image
    Parallel.For(0, imageFiles.Count, options,

        () => 0,

        (i, state, localCount) =>
        {
            string fichier = imageFiles[i];
            string nomFichier = Path.GetFileNameWithoutExtension(fichier);
            Console.WriteLine($"Traitement : {fichier}");

            foreach (var resolution in resolutions)
            {
                using Image image = Image.Load(fichier);

                if (resolution.Hauteur.HasValue)
                {
                    int nouvelleHauteur = resolution.Hauteur.Value;
                    int nouvelleLargeur = (int)((double)image.Width / image.Height * nouvelleHauteur);
                    image.Mutate(x => x.Resize(nouvelleLargeur, nouvelleHauteur));
                }

                string cheminSortie = Path.Combine(outputFolder, resolution.Dossier, $"{nomFichier}.webp");
                image.Save(cheminSortie, new WebpEncoder());
                Console.WriteLine($"  -> {cheminSortie} ({image.Width}x{image.Height})");
            }

            return localCount + 1;
        },

        localCount =>
        {
            lock (lockObj)
            {
                totalConverties += localCount;
            }
        });

    Console.WriteLine($"\n{totalConverties} image(s) converties.");
    return sw.ElapsedMilliseconds;
}
