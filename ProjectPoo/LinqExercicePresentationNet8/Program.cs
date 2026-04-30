using DataSources;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


//1
var allAlbums = ListAlbumsData.ListAlbums;

var formattedAlbums = allAlbums
    .Select(album => $"Album n°{album.AlbumId} : {album.Title}")
    .ToList();

formattedAlbums.ForEach(Console.WriteLine);

//2

Console.WriteLine("Quel est votre recherche ?");
string recherche = Console.ReadLine();

var resultats = allAlbums
    .Where(album => album.Title.Contains(recherche, StringComparison.InvariantCultureIgnoreCase))
    .Select(album => $"Album n°{album.AlbumId} : {album.Title}")
    .ToList();

resultats.ForEach(Console.WriteLine);