using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices
                .GetRequiredService<TomasosContext>();

            context.Database.Migrate();

            SeedProducts(context);
            SeedLinkProductIngredient(context);
            
            context.Dispose();
        }

        private static void SeedProducts(TomasosContext context)
        {
            if (context.Matratt.ToList().Count == 1)
            {
                context.Matratt.AddRange(
                    new Matratt()
                    {
                        MatrattNamn = "Hawaii",
                        Beskrivning = "Fruktig och perfekt",
                        Pris = 76,
                        MatrattTyp = 1
                    },
                    new Matratt()
                    {
                        MatrattNamn = "Vesuvio",
                        Beskrivning = "Lätt med få ingredienser",
                        Pris = 55,
                        MatrattTyp = 1
                    },
                    new Matratt()
                    {
                        MatrattNamn = "Capriccosa",
                        Beskrivning = "Färska champinjoner med frisk nordisk smak",
                        Pris = 79,
                        MatrattTyp = 1
                    },
                    new Matratt()
                    {
                        MatrattNamn = "Kebabpizza",
                        Beskrivning = "Kött i stora mängder med stark sås",
                        Pris = 99,
                        MatrattTyp = 1
                    },
                    new Matratt()
                    {
                        MatrattNamn = "Al tonno",
                        Beskrivning = "Fisk för fiskälskare",
                        Pris = 60,
                        MatrattTyp = 1
                    }
                );
                context.SaveChanges();
            }
        }

        private static void SeedLinkProductIngredient(TomasosContext context)
        {
            if (!context.MatrattProdukt.Any())
            {
                // ProductId
                var calzone = 1;
                var hawaiiP = 101;
                var vesuvioP = 102;
                var cappP = 103;
                var kebabP = 104;
                var tunaP = 105;

                //IngredientID 
                var ham = 1;
                var mozzarella = 2;
                var mushroom = 3;
                var tomato = 4;
                //var file = 5;
                var onion = 6;
                var tuna = 7;
                var pinapple = 8;
                //var banana = 9;
                //var curry = 10;
                var cheese = 11;
                var feferoni = 12;
                var kebabMeat = 13;
                //var kebabSauce = 14;
                var garlicSauce = 15;


                context.MatrattProdukt.AddRange(
                    new MatrattProdukt()
                    {
                        MatrattId = hawaiiP,
                        ProduktId = ham
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = hawaiiP,
                        ProduktId = tomato
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = hawaiiP,
                        ProduktId = pinapple
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = hawaiiP,
                        ProduktId = cheese
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = vesuvioP,
                        ProduktId = ham
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = vesuvioP,
                        ProduktId = cheese
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = vesuvioP,
                        ProduktId = tomato
                    }, new MatrattProdukt()
                    {
                        MatrattId = cappP,
                        ProduktId = ham
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = cappP,
                        ProduktId = cheese
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = cappP,
                        ProduktId = tomato
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = cappP,
                        ProduktId = mozzarella
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = kebabP,
                        ProduktId = tomato
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = kebabP,
                        ProduktId = garlicSauce
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = kebabP,
                        ProduktId = 14
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = kebabP,
                        ProduktId = kebabMeat
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = kebabP,
                        ProduktId = feferoni
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = kebabP,
                        ProduktId = cheese
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = kebabP,
                        ProduktId = onion
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = calzone,
                        ProduktId = ham
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = calzone,
                        ProduktId = mozzarella
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = calzone,
                        ProduktId = mushroom
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = calzone,
                        ProduktId = tomato
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = tunaP,
                        ProduktId = tomato
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = tunaP,
                        ProduktId = cheese
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = tunaP,
                        ProduktId = onion
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = tunaP,
                        ProduktId = tuna
                    }
                );
                context.SaveChanges();
            }
        }

    }
}