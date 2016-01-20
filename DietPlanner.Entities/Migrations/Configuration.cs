namespace DietPlanner.Entities.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DietPlanner.Entities.DietPlannerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DietPlanner.Entities.DietPlannerDbContext context)
        {
            // debug podczas konfiguracji seeda
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //   System.Diagnostics.Debugger.Launch();

            var userManager = new UserManager<AppUser>(new UserStore<AppUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string[] role = { "U¿ytkownik", "Administrator" };

            foreach (var rola in role)
            {
                if (!roleManager.RoleExists(rola))
                {
                    roleManager.Create(new IdentityRole(rola));
                }
            }

            List<RecipeCategory> rCtgs = new List<RecipeCategory>();
            rCtgs.Add(new RecipeCategory { CategoryName = "Dania z miêsem" });
            rCtgs.Add(new RecipeCategory { CategoryName = "Dania z rybami" });
            rCtgs.Add(new RecipeCategory { CategoryName = "Zupy" });
            rCtgs.Add(new RecipeCategory { CategoryName = "Ciasta i desery" });
            rCtgs.Add(new RecipeCategory { CategoryName = "Przystawki" });
            rCtgs.Add(new RecipeCategory { CategoryName = "Inne" });
            foreach (var item in rCtgs)
            {
                context.RecipeCategories.Add(item);
            }

            #region Jednostki

            Unit gram = new Unit
            {
                 Name = "gram", 
                 NamePlural1 = "gramy",
                 NamePlural2 = "gramów",
                 NameDecimal = "grama", 
                 Short = "g", 
                 BaseUnit = null, 
                 Ratio = null
            };
            Unit kilogram = new Unit
            {
                 Name = "kilogram", 
                 NamePlural1 = "kilogramy",
                 NamePlural2 = "kilogramów",
                 NameDecimal = "kilograma", 
                 Short = "kg", 
                 BaseUnit = gram, 
                 Ratio = new UnitRatio{ Base = 1000, Derived = 1}
            };
            Unit mililitr = new Unit
            { 
                 Name = "mililitr", 
                 NamePlural1 = "mililitry",
                 NamePlural2 = "mililitrów",
                 NameDecimal = "mililitra", 
                 Short = "ml", 
                 BaseUnit = gram, 
                 Ratio = null 
            };
            Unit sztuka = new Unit
            { 
                 Name = "sztuka", 
                 NamePlural1 = "sztuki",
                 NamePlural2 = "sztuk",
                 NameDecimal = "sztuki", 
                 Short = " szt.", 
                 BaseUnit = gram, 
                 Ratio = null 
            };
            Unit lyzeczka = new Unit
            { 
                 Name = "³y¿eczka",  
                 NamePlural1 = "³y¿eczki",
                 NamePlural2 = "³y¿eczek",
                 NameDecimal = "³y¿eczki", 
                 BaseUnit = mililitr, 
                 Ratio = new UnitRatio { Base = 5, Derived = 1 } 
            };
            Unit lyzka = new Unit
            { 
                 Name = "³y¿ka",  
                 NamePlural1 = "³y¿ki",
                 NamePlural2 = "³y¿ek",
                 NameDecimal = "³y¿ki", 
                 BaseUnit = mililitr, 
                 Ratio = new UnitRatio { Base = 15, Derived = 1 } 
            };
            Unit szklanka = new Unit
            { 
                 Name = "szklanka", 
                 NamePlural1 = "szklanki",
                 NamePlural2 = "szklanek",
                 NameDecimal = "szklanki", 
                 Short = " szkl.", 
                 BaseUnit = mililitr, 
                 Ratio = new UnitRatio { Base = 250, Derived = 1 } 
            };
            Unit litr = new Unit
            { 
                 Name = "litr", 
                 NamePlural1 = "litry",
                 NamePlural2 = "litrów",
                 NameDecimal = "litra",
                 Short = "l", 
                 BaseUnit = mililitr, 
                 Ratio = new UnitRatio { Base = 1000, Derived = 1 } 
            };
            Unit szczypta = new Unit
            { 
                 Name = "szczypta", 
                 NamePlural1 = "szczypty",
                 NamePlural2 = "szczypt",
                 NameDecimal = "szczypty", 
                 BaseUnit = gram, 
                 Ratio = null
            };
            Unit opakowanie = new Unit
            {
                Name = "opakowanie",
                NamePlural1 = "opakowania",
                NamePlural2 = "opakowañ",
                NameDecimal = "opakowania",
                BaseUnit = gram,
                Ratio = null
            };
            Unit plaster = new Unit
            { 
                 Name = "plaster", 
                 NamePlural1 = "plastry",
                 NamePlural2 = "plastrów",
                 NameDecimal = "plastra", 
                 BaseUnit = sztuka, 
                 Ratio = null
            };

            List<Unit> units = new List<Unit> 
            { 
                gram, 
                kilogram, 
                litr, 
                mililitr, 
                lyzeczka, 
                lyzka, 
                szklanka, 
                szczypta, 
                sztuka,
                plaster
            };

            foreach (var unit in units)
            {
                context.Units.Add(unit);
            }

            #endregion

            List<ProductCategory> ctgs = new List<ProductCategory>();

            #region Kategorie produktów

            ctgs.Add(new ProductCategory { CategoryName = "Owoce i warzywa"});   // [0]
            ctgs.Add(new ProductCategory { CategoryName = "Pieczywo"});
            ctgs.Add(new ProductCategory { CategoryName = "Nabia³"});
            ctgs.Add(new ProductCategory { CategoryName = "Miêso i ryby"});
            ctgs.Add(new ProductCategory { CategoryName = "Napoje"});
            ctgs.Add(new ProductCategory { CategoryName = "Artyku³y sypkie"});
            ctgs.Add(new ProductCategory { CategoryName = "S³odycze"});
            ctgs.Add(new ProductCategory { CategoryName = "Inne"});              // [7]

            ctgs.Add(new ProductCategory { CategoryName = "Woda", ParentCategory = ctgs[4]});
            ctgs.Add(new ProductCategory { CategoryName = "Soki", ParentCategory = ctgs[4]});
            ctgs.Add(new ProductCategory { CategoryName = "Napoje gazowane", ParentCategory = ctgs[4]});
            ctgs.Add(new ProductCategory { CategoryName = "Alkohol", ParentCategory = ctgs[4]});
            ctgs.Add(new ProductCategory { CategoryName = "Inne", ParentCategory = ctgs[4]});     // [12]

            ctgs.Add(new ProductCategory { CategoryName = "Owoce", ParentCategory = ctgs[0]});
            ctgs.Add(new ProductCategory { CategoryName = "Warzywa", ParentCategory = ctgs[0]});
            ctgs.Add(new ProductCategory { CategoryName = "Grzyby", ParentCategory = ctgs[0]});
            ctgs.Add(new ProductCategory { CategoryName = "Zio³a i kie³ki", ParentCategory = ctgs[0]});
            ctgs.Add(new ProductCategory { CategoryName = "Bakalie", ParentCategory = ctgs[0]});
            ctgs.Add(new ProductCategory { CategoryName = "Inne", ParentCategory = ctgs[0]});     // [18]

            ctgs.Add(new ProductCategory { CategoryName = "Chleb", ParentCategory = ctgs[1]});
            ctgs.Add(new ProductCategory { CategoryName = "Bu³ki", ParentCategory = ctgs[1]});
            ctgs.Add(new ProductCategory { CategoryName = "Pieczywo pe³noziarniste", ParentCategory = ctgs[1]});
            ctgs.Add(new ProductCategory { CategoryName = "Wyroby cukiernicze", ParentCategory = ctgs[1]});
            ctgs.Add(new ProductCategory { CategoryName = "Inne", ParentCategory = ctgs[1]});     // [23]

            ctgs.Add(new ProductCategory { CategoryName = "Mleko", ParentCategory = ctgs[2]});
            ctgs.Add(new ProductCategory { CategoryName = "Jogurty", ParentCategory = ctgs[2]});
            ctgs.Add(new ProductCategory { CategoryName = "Œmietany", ParentCategory = ctgs[2]});
            ctgs.Add(new ProductCategory { CategoryName = "Kefiry i maœlanki", ParentCategory = ctgs[2]});
            ctgs.Add(new ProductCategory { CategoryName = "Jajka", ParentCategory = ctgs[2]});
            ctgs.Add(new ProductCategory { CategoryName = "Sery", ParentCategory = ctgs[2]});
            ctgs.Add(new ProductCategory { CategoryName = "T³uszcze do smarowania", ParentCategory = ctgs[2]});
            ctgs.Add(new ProductCategory { CategoryName = "Inne", ParentCategory = ctgs[2]});     // [31]

            ctgs.Add(new ProductCategory { CategoryName = "Drób", ParentCategory = ctgs[3]});
            ctgs.Add(new ProductCategory { CategoryName = "Wo³owina", ParentCategory = ctgs[3]});
            ctgs.Add(new ProductCategory { CategoryName = "Cielêcina", ParentCategory = ctgs[3]});
            ctgs.Add(new ProductCategory { CategoryName = "Jagniêcina", ParentCategory = ctgs[3]});
            ctgs.Add(new ProductCategory { CategoryName = "Wieprzowina", ParentCategory = ctgs[3]});
            ctgs.Add(new ProductCategory { CategoryName = "Ryby", ParentCategory = ctgs[3]});
            ctgs.Add(new ProductCategory { CategoryName = "Wêdliny", ParentCategory = ctgs[3]});
            ctgs.Add(new ProductCategory { CategoryName = "Inne", ParentCategory = ctgs[3]});     // [39]

            ctgs.Add(new ProductCategory { CategoryName = "Ry¿", ParentCategory = ctgs[5]});
            ctgs.Add(new ProductCategory { CategoryName = "Makaron", ParentCategory = ctgs[5]});
            ctgs.Add(new ProductCategory { CategoryName = "Kawa", ParentCategory = ctgs[5]});
            ctgs.Add(new ProductCategory { CategoryName = "Herbata", ParentCategory = ctgs[5]});
            ctgs.Add(new ProductCategory { CategoryName = "Kasza", ParentCategory = ctgs[5]});
            ctgs.Add(new ProductCategory { CategoryName = "Do wypieków", ParentCategory = ctgs[5]});
            ctgs.Add(new ProductCategory { CategoryName = "Inne", ParentCategory = ctgs[5]});     // [46]

            ctgs.Add(new ProductCategory { CategoryName = "Czekolady", ParentCategory = ctgs[6]});
            ctgs.Add(new ProductCategory { CategoryName = "Batony", ParentCategory = ctgs[6]});
            ctgs.Add(new ProductCategory { CategoryName = "Ciastka", ParentCategory = ctgs[6]});
            ctgs.Add(new ProductCategory { CategoryName = "Cukierki i ¿elki", ParentCategory = ctgs[6]});
            ctgs.Add(new ProductCategory { CategoryName = "Inne", ParentCategory = ctgs[6]});     // [51]

            ctgs.Add(new ProductCategory { CategoryName = "Piwo", ParentCategory = ctgs[11]});
            ctgs.Add(new ProductCategory { CategoryName = "Wino", ParentCategory = ctgs[11]});
            ctgs.Add(new ProductCategory { CategoryName = "Wódki", ParentCategory = ctgs[11]});
            ctgs.Add(new ProductCategory { CategoryName = "Inne", ParentCategory = ctgs[11]});

            #endregion

            foreach (var ctg in ctgs)
            {
                if (ctg.ParentCategory != null)
                {
                    if (ctg.ParentCategory.ChildCategories == null)
                    {
                        ctg.ParentCategory.ChildCategories = new List<ProductCategory>();
                    }
                    ctg.ParentCategory.ChildCategories.Add(ctg);
                }
            }

            List<Product> products = new List<Product>();

            #region produkty

            Product jablko = new Product
            {
                Name = "Jab³ko, surowe, ze skórk¹, œrednie",
                ProductCategory = ctgs[13],
                DefaultUnit = sztuka,

                Water = 85.56M,
                Protein = 0.26M,
                Carbohydrate = 13.81M,
                Fat = 0.17M,
                Fiber = 2.4M,
                Sugar = 10.39M,
                SaturatedFat = 0.028M,
                Cholesterol = null,
                GlutenFree = null,

                Verified = true
            };

            products.Add(jablko);

            Product ziemniak = new Product
            {
                Name = "Ziemniak, bez skórki, gotowany w nieosolonej wodzie",
                ProductCategory = ctgs[14],
                DefaultUnit = kilogram,

                Water = 77.46M,
                Protein = 1.71M,
                Carbohydrate = 20.01M,
                Fat = 0.10M,
                Fiber = 1.8M,
                Sugar = 0.89M,
                SaturatedFat = 0.026M,

                GlutenFree = null,
                Verified = true
            };

            products.Add(ziemniak);

            products.Add(new Product
            {
                Name = "Fasola czerwona, nasiona, bez zalewy",
                ProductCategory = ctgs[14],
                DefaultUnit = gram,

                Water = 11.75M,
                Protein = 22.53M,
                Carbohydrate = 61.26M,
                Fat = 1.06M,
                Fiber = 15.2M,
                Sugar = 2.1M,
                SaturatedFat = 0.154M,

                GlutenFree = true,
                Verified = true
            });

            products.Add(new Product
            {
                Name = "Fasola bia³a, nasiona, gotowana w nieosolonej wodzie",
                ProductCategory = ctgs[14],
                DefaultUnit = gram,

                Water = 63.08M,
                Protein = 9.73M,
                Carbohydrate = 25.09M,
                Fat = 0.35M,
                Fiber = 6.3M,
                Sugar = 0.34M,
                SaturatedFat = 0.091M,

                GlutenFree = true,
                Verified = true
            });

            products.Add(new Product
            {
                Name = "Fasola mung, kie³ki, surowa",
                ProductCategory = ctgs[16],
                DefaultUnit = gram,

                Water = 90.40M,
                Protein = 3.04M,
                Carbohydrate = 5.94M,
                Fat = 0.18M,
                Fiber = 1.8M,
                Sugar = 4.13M,
                SaturatedFat = 0.046M,

                GlutenFree = true,
                Verified = true
            });

            products.Add(new Product
            {
                Name = "Fasola szparagowa, str¹czki, surowa",
                ProductCategory = ctgs[14],
                DefaultUnit = gram,

                Water = 90.32M,
                Protein = 1.83M,
                Carbohydrate = 6.97M,
                Fat = 0.22M,
                Fiber = 2.7M,
                Sugar = 3.26M,
                SaturatedFat = 0.05M,

                GlutenFree = true,
                Verified = true
            });

            Product awokado = new Product
            {
                Name = "Awokado",
                ProductCategory = ctgs[13],
                DefaultUnit = sztuka,

                Water = 73.23M,
                Protein = 2.00M,
                Carbohydrate = 8.53M,
                Fat = 14.66M,
                Fiber = 6.7M,
                Sugar = 0.66M,
                SaturatedFat = 2.126M,
                Cholesterol = null,

                GlutenFree = null,
                Verified = true
            };

            products.Add(awokado);

            products.Add(new Product
            {
                Name = "Ry¿ bia³y, gotowany",
                ProductCategory = ctgs[40],
                DefaultUnit = gram,

                Water = 76.63M,
                Protein = 2.02M,
                Carbohydrate = 21.09M,
                Fat = 0.19M,
                Fiber = 1.0M,
                Sugar = 0.05M,
                SaturatedFat = 0.039M,
                Cholesterol = null,

                GlutenFree = false,
                Verified = true
            });

            products.Add(new Product
            {
                Name = "Kasza jaglana, surowa",
                ProductCategory = ctgs[44],
                DefaultUnit = gram,

                Water = 8.67M,
                Protein = 10.5M,
                Carbohydrate = 68.41M,
                Fat = 2.9M,
                Fiber = 3.5M,
                Sugar = 0.12M,
                SaturatedFat = 0.723M,
                Cholesterol = null,
                Salt = 0.013M,

                GlutenFree = true,
                Verified = true
            });

            products.Add(new Product
            {
                Name = "Kasza jaglana, gotowana",
                ProductCategory = ctgs[44],
                DefaultUnit = gram,

                Water = 71.41M,
                Protein = 3.51M,
                Carbohydrate = 23.67M,
                Fat = 1.0M,
                Fiber = 1.3M,
                Sugar = 0.13M,
                SaturatedFat = 0.172M,
                Cholesterol = null,

                GlutenFree = true,
                Verified = true
            });

            products.Add(new Product
            {
                Name = "Kurczak, pierœ, surowy",
                ProductCategory = ctgs[32],
                DefaultUnit = gram,

                Water = 73.90M,
                Protein = 22.50M,
                Carbohydrate = 0,
                Fat = 2.62M,
                Fiber = 0,
                Sugar = 0,
                SaturatedFat = 0.563M,
                Cholesterol = 0.073M,

                GlutenFree = null,
                Verified = true
            });

            products.Add(new Product
            {
                Name = "£osoœ norweski, surowy",
                ProductCategory = ctgs[37],
                DefaultUnit = gram,

                Water = 68.50M,
                Protein = 19.84M,
                Carbohydrate = 0,
                Fat = 6.34M,
                Fiber = 0,
                Sugar = null,
                SaturatedFat = 0.981M,
                Cholesterol = 0.055M,

                GlutenFree = null,
                Verified = true
            });

            Product jajko1 = new Product
            {
                Name = "Jajo kurze, surowe, ma³e",
                ProductCategory = ctgs[28],
                DefaultUnit = sztuka,

                Water = 76.15M,
                Protein = 12.56M,
                Carbohydrate = 0.72M,
                Fat = 9.51M,
                Fiber = 0,
                Sugar = 0.37M,
                SaturatedFat = 3.126M,
                Cholesterol = 0.372M,

                GlutenFree = null,
                Verified = true
            };

            Product jajko2 = new Product
            {
                Name = "Jajo kurze, surowe, œrednie",
                ProductCategory = ctgs[28],
                DefaultUnit = sztuka,

                Water = 76.15M,
                Protein = 12.56M,
                Carbohydrate = 0.72M,
                Fat = 9.51M,
                Fiber = 0,
                Sugar = 0.37M,
                SaturatedFat = 3.126M,
                Cholesterol = 0.372M,

                GlutenFree = null,
                Verified = true
            };

            Product jajko3 = new Product
            {
                Name = "Jajo kurze, surowe, du¿e",
                ProductCategory = ctgs[28],
                DefaultUnit = sztuka,

                Water = 76.15M,
                Protein = 12.56M,
                Carbohydrate = 0.72M,
                Fat = 9.51M,
                Fiber = 0,
                Sugar = 0.37M,
                SaturatedFat = 3.126M,
                Cholesterol = 0.372M,

                GlutenFree = null,
                Verified = true
            };

            products.Add(jajko1);
            products.Add(jajko2);
            products.Add(jajko3);

            Product bialkoJajka = new Product
            {
                Name = "Jajo kurze, bia³ko, surowe, œwie¿e",
                ProductCategory = ctgs[28],
                DefaultUnit = mililitr,

                Water = 87.57M,
                Protein = 10.90M,
                Carbohydrate = 0.73M,
                Fat = 0.17M,
                Fiber = 0,
                Sugar = 0.71M,
                SaturatedFat = null,
                Cholesterol = 0,

                GlutenFree = null,
                Verified = true
            };

            products.Add(bialkoJajka);

            Product brokul = new Product
            {
                Name = "Broku³, g³ówka, surowy",
                ProductCategory = ctgs[14],
                DefaultUnit = sztuka,

                Water = 89.30M,
                Protein = 2.82M,
                Carbohydrate = 6.64M,
                Fat = 0.37M,
                Fiber = 2.6M,
                Sugar = 1.70M,
                SaturatedFat = 0.039M,
                Cholesterol = null,

                GlutenFree = null,
                Verified = true
            };

            products.Add(brokul);

            Product mleko = new Product
            {
                Name = "Mleko, 2% zawartoœci t³uszczu",
                ProductCategory = ctgs[24], 
                DefaultUnit = mililitr,

                Water = 89.21M,
                Protein = 3.30M,
                Carbohydrate = 4.0M,
                Fat = 1.98M,
                Fiber = 0,
                Sugar = 4.8M,
                SaturatedFat = 1.257M,
                Cholesterol = 0.008M,
                Salt = 0.1M,

                GlutenFree = null,
                Verified = true
            };

            products.Add(mleko);

            Product wino = new Product
            {
                Name = "Wino czerwone, Cabernet Sauvignon",
                ProductCategory = ctgs[53], 
                DefaultUnit = mililitr,

                Water = 86.56M,
                Protein = 0.07M,
                Carbohydrate = 2.60M,
                Fat = 0,
                Fiber = 0,
                Sugar = 1.0M,
                SaturatedFat = 0,
                Cholesterol = null,
                Alcohol = 10.60M,

                GlutenFree = null,
                Verified = true
            };

            products.Add(wino);

            products.Add(new Product
            {
                Name = "Haribo ¯elki Z³ote Misie",
                ProductCategory = ctgs[50], 
                DefaultUnit = gram,

                Water = 1,
                Protein = 0,
                Carbohydrate = 98.90M,
                Fat = 0,
                Fiber = 0.1M,
                Sugar = 59,
                SaturatedFat = 0,
                Cholesterol = null,
                GlutenFree = null,

                Verified = true
            });

            products.Add(new Product
            {
                Name = "Ser wiejski, serek wiejski, naturalny",
                ProductCategory = ctgs[29], 
                DefaultUnit = gram,

                Protein = 13.0M,
                Fat = 5.0M,
                SaturatedFat = 3.5M,
                Carbohydrate = 2.0M,
                Sugar = 1.5M,
                Salt = 0.7M,
                GlutenFree = null,

                Verified = true
            });

            products.Add(new Product
            {
                Name = "Ser pleœniowy, brie, naturalny",
                ProductCategory = ctgs[29],
                DefaultUnit = gram,

                Protein = 13.0M,
                Fat = 31.0M,
                SaturatedFat = 22.0M,
                Carbohydrate = 1.3M,
                Sugar = 0.01M,
                Salt = 1.7M,
                GlutenFree = null,

                Verified = true
            });

            Product olej = new Product
            {
                Name = "Olej s³onecznikowy",
                ProductCategory = ctgs[7],
                DefaultUnit = mililitr,

                Protein = 0.0M,
                Fat = 100.0M,
                SaturatedFat = 11.0M,
                Carbohydrate = 0.0M,
                Salt = 0.0M,
                GlutenFree = null,

                Verified = true
            };

            products.Add(olej);

            Product oliwa = new Product
            {
                Name = "Oliwa z oliwek, z pierwszego t³oczenia",
                ProductCategory = ctgs[7],
                DefaultUnit = mililitr,

                Protein = 0.0M,
                Fat = 91.60M,
                SaturatedFat = 13.0M,
                Carbohydrate = 0.0M,
                Sugar = 0.0M,
                Salt = 0.0M,
                GlutenFree = null,

                Verified = true
            };
            
            products.Add(oliwa);

            products.Add(new Product
            {
                Name = "Ry¿ jaœminowy, surowy",
                ProductCategory = ctgs[40], 
                DefaultUnit = gram,

                Protein = 6.7M,
                Fat = 0.7M,
                SaturatedFat = 0.2M,
                Carbohydrate = 78.9M,
                Sugar = 0.2M,
                Fiber = 2.4M,
                Salt = 0.02M,
                GlutenFree = false,

                Verified = true
            });

            Product kakao = new Product
            {
                Name = "Kakao ciemne, obni¿ona zawartoœæ t³uszczu",
                ProductCategory = ctgs[46],
                DefaultUnit = gram,

                Fat = 10.5M,
                SaturatedFat = 6.4M,
                Carbohydrate = 13,
                Sugar = 0.5M,
                Fiber = null,
                Protein = 23.5M,
                Salt = 0.86M,
                GlutenFree = null,

                Verified = true
            };

            products.Add(kakao);

            Product sok = new Product
            {
                Name = "Sok pomarañczowy",
                ProductCategory = ctgs[9], 
                DefaultUnit = mililitr,

                Protein = 0.70M,
                Fat = 0.20M,
                SaturatedFat = 0.024M,
                Carbohydrate = 10.4M,
                Sugar = 8.4M,
                Fiber = 0.2M,
                Water = 88.3M,
                Salt = null,
                GlutenFree = null,

                Verified = true
            };

            products.Add(sok);

            Product banan = new Product
            {
                Name = "Banan, bez skórki, d³ugi, surowy, œredni",
                ProductCategory = ctgs[13],
                DefaultUnit = sztuka,

                Protein = 1.09M,
                Fat = 0.33M,
                SaturatedFat = 0.112M,
                Carbohydrate = 22.84M,
                Sugar = 12.23M,
                Fiber = 2.6M,
                Water = 74.91M,
                Salt = null,
                GlutenFree = null,

                Verified = true
            };

            products.Add(banan);

            products.Add(new Product
            {
                Name = "Miêso mielone wo³owe",
                ProductCategory = ctgs[33], 
                DefaultUnit = gram,

                Protein = 22.00M,
                Fat = 5.50M,
                SaturatedFat = null,
                Carbohydrate = 0.10M,
                Sugar = null,
                Fiber = null,
                Water = null,
                Salt = null,
                GlutenFree = null,

                Verified = true
            });

            products.Add(new Product
            {
                Name = "Wieprzowina karkówka",
                ProductCategory = ctgs[36],
                DefaultUnit = gram,

                Protein = 16.50M,
                Fat = 23.20M,
                SaturatedFat = null,
                Carbohydrate = 0.0M,
                Sugar = null,
                Fiber = null,
                Water = null,
                Salt = null,
                GlutenFree = null,

                Verified = true
            });

            #endregion

            foreach (var item in products)
            {
                if (item.ProductCategory.CategoryMembers == null)
                {
                    item.ProductCategory.CategoryMembers = new List<Product>();
                }
                item.ProductCategory.CategoryMembers.Add(item);
                context.Products.Add(item);
            }

            foreach (var item in ctgs)
            {
                context.ProductCategories.Add(item);
            }

            #region Miary produktów

            List<ProductUnitUnitRatio> ratios = new List<ProductUnitUnitRatio>
            {
                new ProductUnitUnitRatio
                { 
	                Product = jablko, 
	                Unit = sztuka, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 185, 
		                Derived = 1 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = awokado, 
	                Unit = sztuka, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 201, 
		                Derived = 1 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = ziemniak, 
	                Unit = sztuka, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 167, 
		                Derived = 1 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = jajko1, 
	                Unit = sztuka, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 38, 
		                Derived = 1 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = jajko2, 
	                Unit = sztuka, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 44, 
		                Derived = 1 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = jajko3, 
	                Unit = sztuka, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 50, 
		                Derived = 1 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = bialkoJajka, 
	                Unit = mililitr, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 93, 
		                Derived = 100 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = brokul, 
	                Unit = sztuka, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 608, 
		                Derived = 1 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = banan, 
	                Unit = sztuka, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 118, 
		                Derived = 1 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = mleko, 
	                Unit = mililitr, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 103, 
		                Derived = 100 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = wino, 
	                Unit = mililitr, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 97, 
		                Derived = 100 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = olej, 
	                Unit = mililitr, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 89, 
		                Derived = 100 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = oliwa, 
	                Unit = mililitr, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 91, 
		                Derived = 100 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = kakao, 
	                Unit = mililitr, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 100, 
		                Derived = 212 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = sok, 
	                Unit = mililitr, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 100, 
		                Derived = 96 
	                } 
                },
                new ProductUnitUnitRatio
                { 
	                Product = banan, 
	                Unit = sztuka, 
	                Ratio = new UnitRatio 
	                { 
		                Base = 118, 
		                Derived = 1 
	                } 
                }
            };

            foreach (var ratio in ratios)
            {
                context.ProductUnitUnitRatios.Add(ratio);
            }

            #endregion

            string email = "admin@dietplanner.pl";
            var user = new AppUser
            {
                UserName = "admin",
                Email = email,
                PasswordHash = userManager.PasswordHasher.HashPassword("pass"),
                BirthYear = 1993,
                Heigth = 183,
                Weight = 78,
            };
            userManager.Create(user);
            user = userManager.FindByEmail(email);
            userManager.AddToRole(user.Id, "Administrator");
        }
    }
}
