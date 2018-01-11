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

            //ProduktID ProduktNamn
            //1   Skinka
            //2   Mozzarella
            //3   Champinjoner
            //4   Tomat
            //5   Fläskfile
            //6   Lök
            //7   Tonfisk
            //8   Ananas
            //9   Banan
            //10  Curry
            //11  Ost

            if (!context.MatrattProdukt.Any())
            {
                context.MatrattProdukt.AddRange(
                    new MatrattProdukt()
                    {
                        MatrattId = 2,
                        ProduktId = 1
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 2,
                        ProduktId = 4
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 2,
                        ProduktId = 8
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 2,
                        ProduktId = 11
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 3,
                        ProduktId = 1
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 3,
                        ProduktId = 11
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 3,
                        ProduktId = 4
                    }, new MatrattProdukt()
                    {
                        MatrattId = 4,
                        ProduktId = 1
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 4,
                        ProduktId = 11
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 4,
                        ProduktId = 4
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 4,
                        ProduktId = 2
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 5,
                        ProduktId = 4
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 5,
                        ProduktId = 15
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 5,
                        ProduktId = 14
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 5,
                        ProduktId = 13
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 5,
                        ProduktId = 12
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 5,
                        ProduktId = 11
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 5,
                        ProduktId = 6
                    }, 
                    new MatrattProdukt()
                    {
                        MatrattId = 1,
                        ProduktId = 1
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 1,
                        ProduktId = 2
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 1,
                        ProduktId = 3
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 1,
                        ProduktId = 4
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 6,
                        ProduktId = 4
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 6,
                        ProduktId = 11
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 6,
                        ProduktId = 6
                    },
                    new MatrattProdukt()
                    {
                        MatrattId = 6,
                        ProduktId = 7
                    }

                );
            }
        }
    }
}