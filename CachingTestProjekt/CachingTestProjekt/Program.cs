using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;



// Doku: https://docs.microsoft.com/de-de/aspnet/core/performance/caching/memory?view=aspnetcore-2.1

namespace CachingTestProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cache registrieren
            var provider = new ServiceCollection()
                .AddMemoryCache()
                //.AddSingleton<MyMemoryCache>() // Ggf. Cache konfigurieren
                .BuildServiceProvider();

            // Und holen
            var cache = provider.GetService<IMemoryCache>();
            
            // Object im Cache speichern
            Person p = new Person {Id = Guid.NewGuid(), Vorname = "John", Nachname = "Doe"};
            createEntry(cache, p.Id, p);

            // Und versuchen zu holen
            Person getFromCache;
            if (cache.TryGetValue(p.Id, out getFromCache))
            {
                Console.WriteLine("Person gefunden");
            }
            else
            {
                Console.WriteLine("Person nicht gefunden");
            }
            if (cache.TryGetValue(Guid.NewGuid(), out getFromCache))
            {
                Console.WriteLine("Person gefunden");
            }
            else
            {
                Console.WriteLine("Person nicht gefunden");
            }

            Console.WriteLine("Hello World!");
        }

        private static ICacheEntry createEntry(IMemoryCache cache, object key, object value)
        {
            using (var entry = cache.CreateEntry(key))
            {
                entry.Value = value;
                //entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
                entry.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(10);
                return entry;
            }
        }
        
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string Vorname  { get; set; } 
        public string Nachname { get; set; }
    }

    public class MyMemoryCache
    {
        public MemoryCache Cache { get; set; }
        public MyMemoryCache()
        {
            Cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 1024
            });
        }
    }
}
