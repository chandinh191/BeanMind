using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Infrastructure.Data.ApplicationDbContextInitialiser;

namespace Infrastructure.Data
{
    public static class ContextInitializerHelper
    {
        public async static Task Seed_Game_Async(DbContext context)
        {
            var jsonString = @"
            [
          {
            ""name"": ""Cộng trừ hai số"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fnumber_sort.png?alt=media&token=6124bb15-ff34-47fc-a0ad-143463324dd1"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""134a1896-37a0-481c-76b8-08dcb1f69dd7"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Cộng trừ nhân chia ba số"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fnumber_sort.png?alt=media&token=6124bb15-ff34-47fc-a0ad-143463324dd1"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""d9ebd726-6f2b-4609-76b9-08dcb1f69dd7"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Cộng trừ nhân chia bốn số"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fnumber_sort.png?alt=media&token=6124bb15-ff34-47fc-a0ad-143463324dd1"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""2395575f-74e2-4809-76ba-08dcb1f69dd7"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Nông trại vui vẻ - phép cộng và phép trừ hai số"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fhappy_farm.png?alt=media&token=ed307113-421f-4d74-bea2-e4d45fd18b8f"",
            ""itemStoreJson"": ""\r\n                   [\r\n                    {\r\n                        \""id\"": \""item001\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_002.png?alt=media&token=99427f48-54f4-41ec-81a5-598aff948ffd\"",\r\n                        \""price\"": 5\r\n                    },\r\n                    {\r\n                        \""id\"": \""item002\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_003.png?alt=media&token=3b3b3b3b-3b3b-3b3b-3b3b-3b3b3b3b3b3b\"",\r\n                        \""price\"": 7\r\n                    },\r\n                    {\r\n                        \""id\"": \""item003\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_004.png?alt=media&token=4b4b4b4b-4b4b-4b4b-4b4b-4b4b4b4b4b4b\"",\r\n                        \""price\"": 8\r\n                    },\r\n                    {\r\n                        \""id\"": \""item004\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_005.png?alt=media&token=5b5b5b5b-5b5b-5b5b-5b5b-5b5b5b5b5b5b\"",\r\n                        \""price\"": 6\r\n                    },\r\n                    {\r\n                        \""id\"": \""item005\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_006.png?alt=media&token=6b6b6b6b-6b6b-6b6b-6b6b-6b6b6b6b6b6b\"",\r\n                        \""price\"": 10\r\n                    },\r\n                    {\r\n                        \""id\"": \""item006\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_007.png?alt=media&token=7b7b7b7b-7b7b-7b7b-7b7b-7b7b7b7b7b7b\"",\r\n                        \""price\"": 12\r\n                    },\r\n                    {\r\n                        \""id\"": \""item007\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_008.png?alt=media&token=8b8b8b8b-8b8b-8b8b-8b8b-8b8b8b8b8b8b\"",\r\n                        \""price\"": 15\r\n                    },\r\n                    {\r\n                        \""id\"": \""item008\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_009.png?alt=media&token=9b9b9b9b-9b9b-9b9b-9b9b-9b9b9b9b9b9b\"",\r\n                        \""price\"": 20\r\n                    },\r\n                    {\r\n                        \""id\"": \""item009\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_010.png?alt=media&token=0b0b0b0b-0b0b-0b0b-0b0b-0b0b0b0b0b0b\"",\r\n                        \""price\"": 9\r\n                    },\r\n                    {\r\n                        \""id\"": \""item010\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_011.png?alt=media&token=1b1b1b1b-1b1b-1b1b-1b1b-1b1b1b1b1b1b\"",\r\n                        \""price\"": 18\r\n                    },\r\n                    {\r\n                        \""id\"": \""item011\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_012.png?alt=media&token=2b2b2b2b-2b2b-2b2b-2b2b-2b2b2b2b2b2b\"",\r\n                        \""price\"": 22\r\n                    },\r\n                    {\r\n                        \""id\"": \""item012\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_013.png?alt=media&token=3b3b3b3b-3b3b-3b3b-3b3b-3b3b3b3b3b3b\"",\r\n                        \""price\"": 17\r\n                    },\r\n                    {\r\n                        \""id\"": \""item013\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_014.png?alt=media&token=4b4b4b4b-4b4b-4b4b-4b4b-4b4b4b4b4b4b\"",\r\n                        \""price\"": 11\r\n                    },\r\n                    {\r\n                        \""id\"": \""item014\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_015.png?alt=media&token=5b5b5b5b-5b5b-5b5b-5b5b-5b5b5b5b5b5b\"",\r\n                        \""price\"": 14\r\n                    },\r\n                    {\r\n                        \""id\"": \""item015\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_016.png?alt=media&token=6b6b6b6b-6b6b-6b6b-6b6b-6b6b6b6b6b6b\"",\r\n                        \""price\"": 19\r\n                    },\r\n                    {\r\n                        \""id\"": \""item016\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_017.png?alt=media&token=7b7b7b7b-7b7b-7b7b-7b7b-7b7b7b7b7b7b\"",\r\n                        \""price\"": 23\r\n                    },\r\n                    {\r\n                        \""id\"": \""item017\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_018.png?alt=media&token=8b8b8b8b-8b8b-8b8b-8b8b-8b8b8b8b8b8b\"",\r\n                        \""price\"": 13\r\n                    },\r\n                    {\r\n                        \""id\"": \""item018\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_019.png?alt=media&token=9b9b9b9b-9b9b-9b9b-9b9b-9b9b9b9b9b9b\"",\r\n                        \""price\"": 16\r\n                    },\r\n                    {\r\n                        \""id\"": \""item019\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_020.png?alt=media&token=0b0b0b0b-0b0b-0b0b-0b0b-0b0b0b0b0b0b\"",\r\n                        \""price\"": 21\r\n                    },\r\n                    {\r\n                        \""id\"": \""item020\"",\r\n                        \""image\"": \""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_021.png?alt=media&token=1b1b1b1b-1b1b-1b1b-1b1b-1b1b1b1b1b1b\"",\r\n                        \""price\"": 25\r\n                    }\r\n                ]\r\n                "",
            ""animalJson"": """",
            ""id"": ""a65534d6-b34c-43d1-e2f6-08dcb0b903bd"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Nông trại vui vẻ - phép cộng trừ nhân chia hai số"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fhappy_farm.png?alt=media&token=ed307113-421f-4d74-bea2-e4d45fd18b8f"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""9400fa00-e27d-40a1-e2f7-08dcb0b903bd"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Khám phá đại dương - phép cộng trừ hai số"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Focean_adventure.png?alt=media&token=d9ada505-0862-4a8b-afe6-6a11358561bc"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""d9db0faa-49e7-488e-e2f8-08dcb0b903bd"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Khám phá đại dương - phép cộng trừ nhân chia hai số"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Focean_adventure.png?alt=media&token=d9ada505-0862-4a8b-afe6-6a11358561bc"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""6d69ec97-28c8-4c34-e2f9-08dcb0b903bd"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Sắp xếp số từ 1 tới 100"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fnumber_sort.png?alt=media&token=6124bb15-ff34-47fc-a0ad-143463324dd1"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""3e2e9eee-07bb-4548-e2fa-08dcb0b903bd"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Sắp xếp số từ 1 tới 1000"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fnumber_sort.png?alt=media&token=6124bb15-ff34-47fc-a0ad-143463324dd1"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""6011f3e5-d1fd-439c-e2fb-08dcb0b903bd"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Lớn hơn, bé hơn hay bằng nhau"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fodd_and_even.png?alt=media&token=30686705-4ae5-433a-8af7-16a30938461c"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""b2b05dc0-d4d4-4dfb-e2fc-08dcb0b903bd"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Khám phá đại dương - học đếm từ 0 đến 5"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Focean_adventure.png?alt=media&token=d9ada505-0862-4a8b-afe6-6a11358561bc"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Nông trại vui vẻ - học đếm từ 0 đến 5"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fhappy_farm.png?alt=media&token=ed307113-421f-4d74-bea2-e4d45fd18b8f"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""49299e7c-fa16-45fd-84e4-1a725c118a9f"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Sắp xếp số từ 1 tới 10"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fnumber_sort.png?alt=media&token=6124bb15-ff34-47fc-a0ad-143463324dd1"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""ead13199-827d-4c48-5d08-08dcafad932c"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Trò chơi mua sắm"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fshopping.png?alt=media&token=6ba50614-1253-4b02-8906-e19979d9f614"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""c296495f-342e-4fd6-5d09-08dcafad932c"",
            ""isDeleted"": false
          },
          {
            ""name"": ""Số lẻ và số chẵn"",
            ""description"": ""Description Game"",
            ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fodd_and_even.png?alt=media&token=30686705-4ae5-433a-8af7-16a30938461c"",
            ""itemStoreJson"": """",
            ""animalJson"": """",
            ""id"": ""59141c9e-7dd3-4c76-5d0a-08dcafad932c"",
            ""isDeleted"": false
          }
        ]";

            var games = JsonConvert.DeserializeObject<List<GameSeed>>(jsonString);
            foreach (var game in games)
            {
                await context.Set<Game>().AddAsync(new Game
                {
                    Id = game.Id,
                    Name = game.Name,
                    Description = game.Description,
                    Image = game.Image,
                    ItemStoreJson = game.ItemStoreJson,
                    AnimalJson = game.AnimalJson,
                    IsDeleted = game.IsDeleted
                });
                await context.SaveChangesAsync();
            }
            //await context.SaveChangesAsync();
            //await SeedGameHistoryAsync(context);
        }

        public async static Task Seed_GameHistory_Async(ApplicationDbContext _context)
        {
            // GameHistories table
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("f9033f16-8f40-4cce-a687-ac1fac7712a7"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b1",
                Point = 10,
                Duration = 8
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("cb23cacd-7e73-4c32-a488-6ea58f1beacf"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b2",
                Point = 14,
                Duration = 8
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("352a4f2a-13a7-4ac7-a92e-3cf900ac4425"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b2",
                Point = 95,
                Duration = 4
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("6c78454f-f667-4d4c-949a-d81aa082df44"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b1",
                Point = 34,
                Duration = 2
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("5d1addc2-56b0-4c8e-81ee-9049facac523"),
                GameId = new Guid("49299e7c-fa16-45fd-84e4-1a725c118a9f"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b0",
                Point = 34,
                Duration = 3
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("2c8fb39d-1112-403e-8eb3-2252b21f3307"),
                GameId = new Guid("49299e7c-fa16-45fd-84e4-1a725c118a9f"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b3",
                Point = 34,
                Duration = 1
            });
        }

        public async static Task Seed_Subject_CourseLevel_ProgamType_Async(ApplicationDbContext _context)
        {
            // Subject table
            await _context.Subjects.AddAsync(new Subject
            {
                Id = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                Title = "Toán",
                Description = "Môn Toán trẻ em giúp phát triển kỹ năng toán học cơ bản cho trẻ qua các hoạt động và trò chơi thú vị. Mục tiêu là tạo nền tảng vững chắc cho sự hiểu biết và tự tin của trẻ khi tiếp cận với toán học."
            });
            await _context.Subjects.AddAsync(new Subject
            {
                Id = new Guid("d7896275-9b79-4955-92f2-e1923b5fa05f"),
                Title = "Khoa học",
                Description = "Môn Khoa học là một phần quan trọng của chương trình giáo dục, giúp học sinh hiểu về thế giới xung quanh thông qua việc nghiên cứu và khám phá các hiện tượng tự nhiên và khoa học. Trong môn này, học sinh được khuyến khích tìm hiểu về các nguyên lý cơ bản của khoa học thông qua các thí nghiệm, quan sát và thảo luận. Mục tiêu của môn Khoa học là khơi dậy sự tò mò, tạo ra nền tảng kiến thức vững chắc và phát triển kỹ năng tư duy logic và phân tích cho học sinh, từ đó giúp họ hiểu biết sâu hơn về thế giới và thúc đẩy sự phát triển cá nhân và xã hội.",
            });
            await _context.Subjects.AddAsync(new Subject
            {
                Id = new Guid("16179b1e-335c-4594-bf15-156e799aa0c5"),
                Title = "Tiếng Việt",
                Description = "Môn Tiếng Việt là một môn học nhằm phát triển các kỹ năng ngôn ngữ như đọc, viết, nghe và nói bằng tiếng Việt. Học sinh sẽ được học về ngữ pháp, từ vựng, chính tả, và cấu trúc câu. Ngoài ra, môn học còn giúp học sinh hiểu sâu hơn về văn hóa, lịch sử và văn học Việt Nam thông qua các tác phẩm văn học, bài báo và các tài liệu khác. Môn Tiếng Việt góp phần nâng cao khả năng giao tiếp và tư duy ngôn ngữ của học sinh.",
            });
            await _context.Subjects.AddAsync(new Subject
            {
                Id = new Guid("80b9f97d-6281-4893-be0c-ae41f8ad8444"),
                Title = "Tiếng Anh",
                Description = "Môn Tiếng Anh là một môn học tập trung vào việc phát triển các kỹ năng ngôn ngữ như đọc, viết, nghe và nói bằng tiếng Anh. Học sinh sẽ học về ngữ pháp, từ vựng, chính tả và cấu trúc câu. Ngoài ra, môn học còn giúp học sinh hiểu rõ hơn về văn hóa, lịch sử và văn học của các nước nói tiếng Anh thông qua các tác phẩm văn học, bài báo và các tài liệu khác. Môn Tiếng Anh nhằm nâng cao khả năng giao tiếp và tư duy ngôn ngữ của học sinh.",
            });
            // ------------------
            // Program type table
            await _context.ProgramTypes.AddAsync(new ProgramType
            {
                Id = new Guid("ed464990-9599-4d6c-99d9-b0ff18be795a"),
                Title = "Toán Cambridge",
                Description = "Chương trình Toán Cambridge dành cho học sinh tiểu học tập trung vào việc phát triển hiểu biết khái niệm, kỹ năng giải quyết vấn đề và lý luận toán học. Học sinh sử dụng các vật dụng học tập như khối đếm và dải phân số để hiểu rõ hơn các khái niệm trừu tượng. Bài toán được đặt trong ngữ cảnh thực tế để làm cho việc học trở nên gần gũi và dễ hiểu hơn. Khuyến khích học sinh giải thích quá trình suy nghĩ và lý luận toán học giúp nâng cao kỹ năng trình bày và suy luận logic. Các bài tập ngắn và các thử thách có giới hạn thời gian được sử dụng để cải thiện sự lưu loát và chính xác trong tính toán."
            });
            await _context.ProgramTypes.AddAsync(new ProgramType
            {
                Id = new Guid("96cd2a6c-1013-4bb9-8927-047fc25e0402"),
                Title = "Toán Tư Duy",
                Description = "Toán tư duy là phương pháp giảng dạy giúp học sinh phát triển khả năng suy luận logic, phân tích và giải quyết vấn đề thông qua các bài toán sáng tạo và thực tế. Thay vì chỉ tập trung vào việc học thuộc lòng các công thức, toán tư duy khuyến khích học sinh hiểu sâu các khái niệm toán học và áp dụng chúng vào các tình huống khác nhau. Phương pháp này sử dụng các bài toán mở, câu hỏi kích thích tư duy và các hoạt động nhóm để thúc đẩy sự tương tác và hợp tác giữa các học sinh. Kết quả là, học sinh không chỉ nắm vững kiến thức toán học mà còn phát triển kỹ năng tư duy phản biện và sáng tạo, chuẩn bị tốt hơn cho việc học tập và ứng dụng toán học trong cuộc sống hàng ngày.",
            });
            await _context.ProgramTypes.AddAsync(new ProgramType
            {
                Id = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Toán Bộ Giáo Dục",
                Description = "Chương trình Toán học của Bộ Giáo dục và Đào tạo Việt Nam dành cho học sinh tiểu học tập trung vào việc xây dựng nền tảng vững chắc về các khái niệm toán học cơ bản như số học, hình học, đo lường và đại số đơn giản. Học sinh được khuyến khích sử dụng các vật dụng học tập và phương pháp trực quan như khối đếm, bảng biểu và sơ đồ để hiểu rõ hơn về các khái niệm trừu tượng. Bên cạnh đó, chương trình cũng chú trọng đến việc phát triển kỹ năng giải quyết vấn đề thông qua các bài toán thực tế và các hoạt động nhóm, giúp học sinh rèn luyện khả năng tư duy logic và sáng tạo. Việc thực hành thường xuyên và liên tục được ưu tiên nhằm nâng cao sự chính xác và lưu loát trong các phép tính toán học. Chương trình còn khuyến khích sự tham gia của phụ huynh trong quá trình học tập của con em, tạo môi trường học tập tích cực và hỗ trợ tối đa cho học sinh.",
            });
            // ------------------
            // CourseLevel table
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"),
                Title = "Lớp 1",
                Description = "Chương trình Toán cho học sinh lớp 1 tập trung vào việc xây dựng nền tảng toán học vững chắc với các khái niệm số học cơ bản như số đếm, phép cộng và phép trừ. Các bài học sử dụng phương pháp học tập trực quan và thực hành để giúp học sinh hiểu sâu hơn về các khái niệm toán học, kích thích sự hứng thú và tích cực trong việc học toán qua các trò chơi và bài toán thực tế."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"),
                Title = "Lớp 2",
                Description = "Chương trình Toán cho học sinh lớp 2 tiếp tục củng cố nền tảng toán học cơ bản với các phép cộng, trừ và các mẫu hình học đơn giản. Các bài học sử dụng phương pháp học tập trực quan, thực hành và trò chơi để phát triển kỹ năng giải quyết vấn đề và tư duy logic của học sinh, chuẩn bị cho những thách thức toán học phức tạp hơn."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("3f6798e9-eed3-40db-ae92-7dd0da9a6435"),
                Title = "Lớp 3",
                Description = "Chương trình Toán cho học sinh lớp 3 tập trung vào việc mở rộng kiến thức số học với các phép nhân, chia không dư, và các khái niệm hình học như đo đạc đơn vị và diện tích. Học sinh sẽ thực hành và áp dụng kiến thức vào các bài toán thực tế, phát triển kỹ năng tư duy logic và giải quyết vấn đề qua các phương pháp giảng dạy linh hoạt và học tập theo nhóm."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("6e4cf1a0-6e6b-4b08-a76c-11d3c51d3bea"),
                Title = "Lớp 4",
                Description = "Chương trình Toán cho học sinh lớp 4 tập trung vào việc củng cố và mở rộng kiến thức số học, bao gồm các phép nhân, chia không dư và các khái niệm hình học như diện tích và thể tích. Học sinh sẽ tiếp tục phát triển kỹ năng giải quyết bài toán phức tạp, sử dụng các phương pháp học tập trực quan, thực hành và học tập theo nhóm để tăng cường sự hiểu biết và tích cực trong học tập toán học."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("9e735658-0002-45c9-a6d5-1a579dba49ee"),
                Title = "Lớp 5",
                Description = "Chương trình Toán cho học sinh lớp 5 tập trung vào việc mở rộng kiến thức số học, hình học và đại số. Học sinh sẽ học về phép nhân, chia với dư, tỉ lệ, phần trăm, và các khái niệm đại số đơn giản. Họ cũng sẽ nghiên cứu về diện tích, thể tích và các hình học không gian. Chương trình giúp học sinh phát triển kỹ năng giải quyết vấn đề, phân tích và tự tin áp dụng kiến thức toán học vào các tình huống thực tế."
            });
            await _context.CourseLevels.AddAsync(new CourseLevel
            {
                Id = new Guid("b7b9648d-5e45-4f96-a22a-cfe447ed9b33"),
                Title = "Lớp nâng cao",
                Description = "Chương trình Toán nâng cao dành cho học sinh có năng lực và đam mê Toán học, giúp họ phát triển kiến thức và kỹ năng ở mức độ cao hơn. Học sinh sẽ được học các khái niệm nâng cao trong đại số, hình học, và xác suất thống kê. Chương trình còn bao gồm các chủ đề toán học hiện đại và ứng dụng thực tiễn, nhằm rèn luyện tư duy logic, khả năng phân tích và giải quyết vấn đề phức tạp. Mục tiêu là chuẩn bị cho học sinh những nền tảng vững chắc để tiếp cận các kỳ thi Toán học quốc gia và quốc tế, cũng như các môn học Toán ở cấp độ cao hơn."
            });

            // ------------------
        }

        public async static Task Seed_Course_TeachingSlot_Chapter_Topic_Async(ApplicationDbContext _context)
        {
            // Course table
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Title = "Các số từ 0 đến 10",
                Price = 200,
                ImageURL = "https://lh3.googleusercontent.com/cUU3mc0ZhuccrJt8PSkcIicKVG5I6WXqJoCMaldiuDH1XCMbKEKUxAzJxpRITwcqXzXe6wxofg1aRrAOyBc9R--m85Q5K8myK2KvEChDa3xUdyKwI5xIjOBM1VNuVHE4dBt5tAqX",
                Description = "Mở đầu chương trình Toán 1 Kết Nối Tri Thức, các em sẽ tìm hiểu về Các số từ 0 đến 10. Gồm các bài học có tóm tắt lý thuyết, cung cấp các bài tập minh họa để các em ôn tập và củng cố kiến thức đã học. Bên cạnh đó, hệ thống hỏi đáp sẽ giúp các em giải đáp các thắc mắc sau khi học bài. Mời các em xem chi tiết bài học.",
                TotalSlot = 20,
            });
            // Chapter table
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Order = 1,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Các số 0, 1, 2, 3, 4, 5",
                Description = "BeanMind xin giới thiệu đến các em học sinh lớp 1 nội dung bài Các số 0, 1, 2, 3, 4, 5. Bài giảng được biên soạn đầy đủ và chi tiết, đồng thời được trình bày một các logic, khoa học sẽ giúp các em ôn tập và củng cố kiến thức về phép chia số có hai chữ số.",
            });
                // Topic table
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("6d8c088e-bc7e-409f-8c79-31066d6df42e"),
                    ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                    Order = 1,
                    Title = "Đọc, đếm, viết được từ 0 đến 5, vị trí của các số trong dãy số.",
                    Description = "Tiết học này tập trung vào việc phát triển kỹ năng cơ bản về số học dành cho học sinh lớp 1, với các nội dung chính như sau: (1) Giới thiệu và nhận biết các số từ 0 đến 5: Học sinh sẽ học cách đọc tên, viết và đếm các số trong khoảng này. (2) Phân tích vị trí của từng số trong dãy số tự nhiên: Học sinh sẽ học cách xác định vị trí tương đối của một số trong dãy số, bao gồm số liền trước và số liền sau, nhằm hình thành tư duy tuần tự và khả năng so sánh. (3) Các bài tập thực hành sẽ bao gồm việc đọc và viết các số, so sánh và sắp xếp các số theo thứ tự tăng dần hoặc giảm dần. (4) Cuối cùng, học sinh sẽ tham gia vào các hoạt động tương tác nhằm củng cố khả năng nhận thức số học qua việc sử dụng các công cụ trực quan và bài tập phân loại số, nhằm phát triển tư duy logic và khả năng nhận biết mô hình số học trong giai đoạn đầu."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("59ee169a-c77f-431b-9cf6-b2c36ab3a4fe"),
                    ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                    Order = 2,
                    Title = "So sánh số 0 với các số trong phạm vi 5",
                    Description = "Trong tiết học này, học sinh sẽ học cách so sánh số 0 với các số khác trong phạm vi từ 1 đến 5. Bài học sẽ bắt đầu bằng việc nhắc lại các khái niệm cơ bản về số 0, như là số khởi đầu trong dãy số tự nhiên và đại diện cho sự không có lượng. Học sinh sẽ được hướng dẫn để hiểu rằng số 0 luôn nhỏ hơn bất kỳ số nào khác trong phạm vi từ 1 đến 5. Qua các bài tập thực hành, học sinh sẽ so sánh số 0 với từng số trong dãy số, sử dụng các dấu hiệu so sánh (<, =, >), và từ đó rút ra kết luận về mối quan hệ giữa số 0 và các số còn lại. Tiết học sẽ kết thúc với các bài tập củng cố và các trò chơi tương tác để giúp học sinh ghi nhớ kiến thức."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("afb89987-0405-4641-b595-b634f422cbed"),
                    ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                    Order = 3,
                    Title = "Đếm xuôi và đếm ngược các số từ 0 đến 5 và thứ tự của các số đó.",
                    Description = "Trong tiết học này, học sinh sẽ học cách đếm xuôi và đếm ngược các số từ 0 đến 5, cùng với việc xác định thứ tự của các số trong cả hai chiều. Bài học sẽ bắt đầu với việc đếm xuôi từ 0 đến 5, giúp học sinh nắm bắt thứ tự tự nhiên của các số. Sau đó, học sinh sẽ học cách đếm ngược từ 5 về 0, qua đó phát triển khả năng tư duy đảo ngược và củng cố kiến thức về vị trí tương đối của các số trong dãy. Các bài tập sẽ bao gồm việc sắp xếp các số theo thứ tự tăng dần và giảm dần, so sánh các cặp số và xác định số liền trước, liền sau trong cả hai chiều. Cuối cùng, học sinh sẽ tham gia vào các hoạt động thực hành để củng cố kỹ năng đếm và nhận biết thứ tự số học."

                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("3829a480-2d22-44da-82c1-38da6fd0a6c9"),
                    ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                    Order = 4,
                    Title = "Đọc số lượng đồ vật trong mỗi nhóm",
                    Description = "Trong tiết học này, học sinh sẽ học cách đếm và đọc số lượng đồ vật trong mỗi nhóm. Các em sẽ được hướng dẫn cách xác định số lượng cụ thể của các đồ vật được sắp xếp thành từng nhóm nhỏ, từ đó đọc ra số lượng đúng. Bài học sẽ bao gồm các bài tập thực hành như đếm số lượng đồ vật trong các hình ảnh minh họa hoặc vật dụng thực tế trong lớp học. Sau đó, học sinh sẽ tham gia vào một trò chơi tương tác, nơi các em sẽ đếm và xác định số lượng đồ vật trong các nhóm khác nhau trong thời gian giới hạn, giúp củng cố kỹ năng đếm và đọc số lượng một cách nhanh chóng và chính xác."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("73558a53-e910-470a-a261-e42d803508e9"),
                    ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                    Order = 5,
                    Title = "Đếm theo thứ tự các số",
                    Description = "Tiết học này sẽ hướng dẫn học sinh cách đếm theo thứ tự các số từ 0 trở lên. Học sinh sẽ bắt đầu bằng việc đếm các số liên tiếp theo thứ tự tăng dần, qua đó nắm vững cách sắp xếp thứ tự tự nhiên của các số. Bài học sẽ bao gồm các bài tập yêu cầu học sinh đếm và sắp xếp các số theo đúng thứ tự, cũng như nhận biết các mẫu số học trong dãy số. Các hoạt động thực hành sẽ giúp học sinh củng cố kỹ năng đếm và nhận diện sự liên tục trong các số, đồng thời phát triển tư duy logic thông qua các bài tập sắp xếp và đếm số theo thứ tự."

                });
                // -----------
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("a4562cbc-b8f8-4bca-b537-77d7b5e4eacc"),
                Order = 2,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Các số 6, 7, 8, 9, 10",
                Description = "Nhằm giúp các em học sinh có thêm nhiều tài liệu tham khảo hữu ích cho môn Toán 1, BeanMind đã biên soạn và tổng hợp nội dung bài Các số 6, 7, 8, 9, 10. Tài liệu được biên soạn với đầy đủ các dạng Toán và các bài tập minh họa có hướng dẫn giải chi tiết. Mời các em cùng tham khảo.",
            });
                // Topic table
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("105cf4c3-4a0a-4a37-ac5b-a6825dad8f20"),
                    ChapterId = new Guid("a4562cbc-b8f8-4bca-b537-77d7b5e4eacc"),
                    Order = 1,
                    Title = "Đọc, đếm được từ 6 đến 10, vị trí của các số trong dãy số",
                    Description = "Trong tiết học này, học sinh sẽ được hướng dẫn cách đọc và đếm các số từ 6 đến 10. Bài học sẽ tập trung vào việc giúp các em nắm vững cách đọc chính xác các số này, cũng như đếm số lượng đồ vật tương ứng. Ngoài ra, học sinh sẽ tìm hiểu về vị trí của từng số trong dãy số tự nhiên, bao gồm việc xác định số liền trước và liền sau, từ đó hiểu rõ hơn về thứ tự và mối quan hệ giữa các số trong phạm vi từ 6 đến 10. Các bài tập thực hành sẽ củng cố kỹ năng đọc, đếm và so sánh các số, giúp học sinh phát triển tư duy số học một cách toàn diện."

                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("c92e3f5a-672d-4f9f-abb1-21dd0f6f0de2"),
                    ChapterId = new Guid("a4562cbc-b8f8-4bca-b537-77d7b5e4eacc"),
                    Order = 2,
                    Title = "So sánh số trong phạm vi 10",
                    Description = "Trong tiết học này, học sinh sẽ học cách so sánh các số trong phạm vi từ 0 đến 10. Bài học sẽ bắt đầu với việc ôn lại các khái niệm cơ bản về số học và cách xác định giá trị của mỗi số. Sau đó, học sinh sẽ thực hành so sánh các cặp số bằng cách sử dụng các dấu hiệu so sánh như lớn hơn, nhỏ hơn và bằng (=, >, <). Tiết học cũng sẽ cung cấp các bài tập thực hành, trong đó học sinh sẽ cần sắp xếp các số theo thứ tự tăng dần và giảm dần, cũng như giải các bài toán so sánh đơn giản. Mục tiêu là giúp học sinh nắm vững kỹ năng so sánh số, từ đó củng cố kiến thức nền tảng về số học."

                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("274e92f2-617d-467c-8d49-3712f15d9f24"),
                    ChapterId = new Guid("a4562cbc-b8f8-4bca-b537-77d7b5e4eacc"),
                    Order = 3,
                    Title = "Đếm số lượng đồ vật, đồ vật",
                    Description = "Tiết học này tập trung vào việc giúp học sinh đếm số lượng đồ vật trong các nhóm khác nhau và phân biệt các loại đồ vật. Bài học sẽ bao gồm việc hướng dẫn học sinh cách đếm số lượng đồ vật trong các hình ảnh minh họa hoặc trong môi trường thực tế, đồng thời phân loại và xác định từng loại đồ vật trong nhóm. Các bài tập thực hành sẽ yêu cầu học sinh đếm chính xác số lượng đồ vật, nhận diện và phân loại chúng theo loại và số lượng. Tiết học cũng sẽ kết thúc với một trò chơi tương tác, nơi học sinh sẽ tham gia vào các hoạt động đếm và phân loại đồ vật trong thời gian giới hạn, giúp củng cố kỹ năng đếm và phân biệt các đồ vật."

                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("1899112e-3c10-4ed7-a0cc-9fdf85173288"),
                    ChapterId = new Guid("a4562cbc-b8f8-4bca-b537-77d7b5e4eacc"),
                    Order = 4,
                    Title = "Luyện tập",
                    Description = "Tiết học này được thiết kế để củng cố và kiểm tra các kỹ năng đã học qua các hoạt động luyện tập. Học sinh sẽ tham gia vào một trò chơi tương tác giúp nâng cao khả năng áp dụng kiến thức về đếm, so sánh số và phân loại đồ vật. Bài học sẽ kết hợp với việc làm các worksheet, trong đó học sinh sẽ thực hiện các bài tập thực hành để ôn tập và áp dụng các khái niệm đã học. Trò chơi và worksheet sẽ bao gồm các bài tập đa dạng, từ việc đếm số lượng, so sánh các số, đến phân loại đồ vật, giúp học sinh củng cố và kiểm tra kiến thức một cách vui vẻ và hiệu quả."
                });
            // -----------
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("d0f9feee-92c2-4f01-9fc7-4c801c3202c7"),
                Order = 3,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Nhiều hơn, ít hơn, bằng nhau",
                Description = "Các em học sinh đang tìm kiếm tài liệu tổng hợp kiến thức về Nhiều hơn, ít hơn, bằng nhau. Hãy tham khảo ngay bài giảng dưới đây của BeanMind biên soạn với những lý thuyết về cùng với các dạng toán cơ bản thường gặp. Sau đây mời các em cùng tham khảo.",
            });
                // Topic table
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("551bd7b1-fe4b-421b-962e-ee16c4f4d59c"),
                    ChapterId = new Guid("d0f9feee-92c2-4f01-9fc7-4c801c3202c7"),
                    Order = 1,
                    Title = "So sánh số lượng, sử dụng từ “bằng” và dấu “ =” để so sánh các số.",
                    Description = "Trong tiết học này, học sinh sẽ học cách so sánh số lượng đồ vật và các số bằng cách sử dụng từ 'bằng' và dấu '='. Bài học sẽ bao gồm việc giải thích khái niệm số lượng 'bằng nhau' và cách sử dụng dấu '=' để biểu thị sự bằng nhau trong các bài toán. Học sinh sẽ thực hành so sánh số lượng đồ vật trong các nhóm khác nhau, xác định khi nào số lượng là bằng nhau và khi nào không phải. Các bài tập sẽ bao gồm việc sắp xếp và so sánh các số hoặc nhóm đồ vật, và điền vào chỗ trống với dấu '=' khi các số là bằng nhau. Cuối cùng, học sinh sẽ thực hiện các hoạt động tương tác để củng cố khả năng nhận diện và sử dụng dấu '=' trong các tình huống khác nhau."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("47207357-0623-432c-af51-b4a26a090482"),
                    ChapterId = new Guid("d0f9feee-92c2-4f01-9fc7-4c801c3202c7"),
                    Order = 2,
                    Title = "Nhận biết được khái niệm nhiều hơn, ít hơn, hơn, kém thông qua hình ảnh và các đồ vật.",
                    Description = "Tiết học này tập trung vào việc giúp học sinh nhận biết và hiểu các khái niệm 'nhiều hơn', 'ít hơn', 'hơn', và 'kém' thông qua việc sử dụng hình ảnh và đồ vật thực tế. Bài học sẽ bắt đầu bằng việc giới thiệu các khái niệm cơ bản liên quan đến số lượng và sự so sánh. Học sinh sẽ được hướng dẫn để nhận diện và phân biệt giữa các nhóm đồ vật dựa trên số lượng của chúng, sử dụng các từ ngữ như 'nhiều hơn', 'ít hơn', 'hơn', và 'kém'. Các bài tập sẽ bao gồm việc so sánh số lượng đồ vật trong các hình ảnh minh họa, xác định sự khác biệt về số lượng, và thực hành các hoạt động tương tác để củng cố sự hiểu biết về các khái niệm này."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("bc8dffca-5a5f-463e-ae25-0a8573fc5142"),
                    ChapterId = new Guid("d0f9feee-92c2-4f01-9fc7-4c801c3202c7"),
                    Order = 3,
                    Title = "Cách so sánh 1-1 giữa hai đại lượng để xác định đại lượng nhiều hơn, ít hơn.",
                    Description = "Tiết học này sẽ hướng dẫn học sinh cách so sánh 1-1 giữa hai đại lượng để xác định đại lượng nào nhiều hơn hoặc ít hơn. Bài học sẽ bắt đầu bằng việc giải thích khái niệm so sánh 1-1, trong đó mỗi phần của đại lượng này được so sánh với mỗi phần của đại lượng kia. Học sinh sẽ thực hành việc sử dụng các công cụ trực quan như hình ảnh hoặc đồ vật để so sánh số lượng của hai nhóm đồ vật. Bài học sẽ bao gồm các hoạt động như đặt các nhóm đồ vật cạnh nhau và đếm số lượng từng nhóm để xác định nhóm nào nhiều hơn hoặc ít hơn. Các bài tập cũng sẽ yêu cầu học sinh sử dụng phương pháp so sánh này để giải quyết các bài toán thực tế, giúp củng cố khả năng phân tích và so sánh số lượng."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("1da26c74-2dff-428a-a60c-c69da767d90d"),
                    ChapterId = new Guid("d0f9feee-92c2-4f01-9fc7-4c801c3202c7"),
                    Order = 4,
                    Title = "Luyện tập",
                    Description = "Tiết học này cung cấp cơ hội cho học sinh luyện tập và củng cố kiến thức đã học thông qua các hoạt động thực hành. Học sinh sẽ tham gia vào trò chơi tương tác, giúp nâng cao kỹ năng so sánh đại lượng, nhận diện các khái niệm 'nhiều hơn', 'ít hơn', và 'bằng nhau'. Bài học cũng sẽ bao gồm việc hoàn thành các worksheet với các bài tập đa dạng, từ việc so sánh số lượng đến việc áp dụng các khái niệm trong các tình huống thực tế. Các hoạt động này được thiết kế để giúp học sinh ôn tập và kiểm tra kiến thức một cách vui vẻ và hiệu quả, đồng thời củng cố kỹ năng giải quyết vấn đề và phân tích số học."
                });
            // -----------
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("6b760186-9678-4e66-81f1-cb3aefe56e9f"),
                Order = 4,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Các dạng toán cho so sánh số",
                Description = "Sau đây mời các em học sinh lớp 1 cùng tìm hiểu về So sánh số. Bài giảng dưới đây đã được BeanMind biên soạn khái quát lý thuyết cần nhớ, đồng thời có các bài tập được tổng hợp đầy đủ các dạng toán liên quan giúp các em dễ dàng nắm được kiến thức trọng tâm của bài.",
            });
                // Topic table
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("683bf283-fea6-407a-a930-171003611689"),
                    ChapterId = new Guid("6b760186-9678-4e66-81f1-cb3aefe56e9f"),
                    Order = 1,
                    Title = "Điền dấu thích hợp vào chỗ trống",
                    Description = "Tiết học này tập trung vào việc luyện tập cách điền dấu thích hợp vào chỗ trống để so sánh các số. Học sinh sẽ hoàn thành các worksheet với các bài tập yêu cầu điền vào các ô trống dấu so sánh như '<', '>', hoặc '=' để hoàn thành các câu so sánh giữa các số. Các bài tập sẽ giúp học sinh củng cố khả năng nhận diện và sử dụng các dấu so sánh trong các tình huống khác nhau, đồng thời nâng cao sự chính xác trong việc so sánh số lượng. Cuối tiết học, học sinh sẽ kiểm tra lại các bài làm của mình để đảm bảo đã hiểu và áp dụng đúng các khái niệm so sánh số học."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("16c30517-73e2-46fa-8ae6-faea81c9e9bf"),
                    ChapterId = new Guid("6b760186-9678-4e66-81f1-cb3aefe56e9f"),
                    Order = 2,
                    Title = "Sắp xếp các số theo thứ tự tăng dần hoặc giảm dần.",
                    Description = "Tiết học này giúp học sinh rèn luyện kỹ năng sắp xếp các số theo thứ tự tăng dần hoặc giảm dần thông qua trò chơi tương tác. Học sinh sẽ tham gia vào các trò chơi vui nhộn, trong đó các em sẽ phải sắp xếp một dãy số từ nhỏ nhất đến lớn nhất hoặc ngược lại, từ lớn nhất đến nhỏ nhất. Các trò chơi sẽ sử dụng các công cụ trực quan và các bài tập có tính thử thách để giúp học sinh củng cố khả năng phân tích và tổ chức các số theo thứ tự đúng. Mục tiêu là giúp học sinh nắm vững khái niệm về thứ tự số học và cải thiện kỹ năng sắp xếp số một cách hiệu quả và thú vị."
                });
                // -----------
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("3f15e8a2-247e-4d79-85f4-d46a73f7782b"),
                Order = 5,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Mấy và mấy",
                Description = "Để giúp các em học sinh lớp 1 học hiệu quả môn Toán, đội ngũ BeanMind đã biên soạn và tổng hợp nội dung bài Mấy và mấy. Tài liệu gồm kiến thức cần nhớ và các dạng Toán của bài giúp các em học tập và củng cố thật tốt kiến thức. Mời các em cùng tham khảo.",
            });
                // Topic table
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("de94cf5e-f9ab-4b0c-b5b8-329c280cd8bc"),
                    ChapterId = new Guid("3f15e8a2-247e-4d79-85f4-d46a73f7782b"),
                    Order = 1,
                    Title = "Hệ thống lại kiến thức",
                    Description = "Tiết học này nhằm mục đích hệ thống lại và củng cố các kiến thức đã học trong bài 'Mấy và mấy'. Học sinh sẽ xem xét lại các khái niệm chính, các dạng toán và các kỹ thuật đã được giảng dạy trong các bài học trước. Bài học sẽ bao gồm việc giải quyết các bài tập tổng hợp, giúp học sinh ôn tập và áp dụng kiến thức vào các tình huống thực tế. Học sinh sẽ thực hiện các hoạt động nhóm và cá nhân để củng cố hiểu biết, đồng thời tham gia vào các bài kiểm tra ngắn để đánh giá mức độ nắm vững kiến thức. Mục tiêu là giúp học sinh xây dựng nền tảng vững chắc và tự tin khi đối mặt với các bài toán liên quan."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("0be5a40b-4058-47fd-b2f1-3c0cf9f268df"),
                    ChapterId = new Guid("3f15e8a2-247e-4d79-85f4-d46a73f7782b"),
                    Order = 2,
                    Title = "Vận dụng và nâng cao",
                    Description = "Tiết học này tập trung vào việc vận dụng và nâng cao kiến thức đã học qua các worksheet khó. Học sinh sẽ thực hiện các bài tập nâng cao, yêu cầu áp dụng các khái niệm và kỹ năng toán học đã học vào các tình huống phức tạp hơn. Các worksheet sẽ bao gồm các bài toán đòi hỏi sự phân tích sâu, giải quyết vấn đề và tư duy phản xạ. Bài học nhằm giúp học sinh phát triển khả năng tư duy logic, khả năng giải quyết các bài toán khó và củng cố kiến thức một cách toàn diện. Học sinh cũng sẽ có cơ hội thảo luận và giải quyết các bài toán thử thách cùng bạn bè để nâng cao kỹ năng và hiểu biết."
                });
                // -----------
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("2110f3bc-0170-4bbc-a3be-09823e310e43"),
                Order = 6,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Luyện tập chung",
                Description = "Nhằm giúp các em học sinh có thêm nhiều tài liệu tham khảo hữu ích cho môn Toán 1, BeanMind đã biên soạn và tổng hợp nội dung bài Luyện tập chung. Tài liệu được biên soạn với đầy đủ các dạng Toán và các bài tập minh họa có hướng dẫn giải chi tiết. Mời các em cùng tham khảo.",
            });

            // ------------   
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 150,
                Title = "Làm quen với một số hình phẳng",
                ImageURL = "https://static.vecteezy.com/system/resources/thumbnails/002/399/898/small_2x/education-concept-with-funny-characters-vector.jpg",
                Description = "Nhận biết được một số hình vuông, hình tròn, hình tam giác, hình chữ nhật là nội dung các em sẽ tìm hiểu ở Chương: Làm quen với một số hình phẳng của môn Toán 1 Kết Nối Tri thức. Bài học được BeanMind biên soạn với các phần tóm tắt lý thuyết, bài tập minh họa và giúp các em chuẩn bị bài học thật tốt và luyện tập, đánh giá năng lực của bản thân. Hệ thống hỏi đáp sẽ giúp các em giải quyết nhiều câu hỏi khó nhanh chóng, hiệu quả. Các em xem nội dung bài học ngay bên dưới.",
                TotalSlot = 30,
            });
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("66b60dd0-020b-4a09-afc1-80a11d878b60"),
                Order = 1,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                Title = "Hình vuông, hình tròn, hình tam giác, hình chữ nhật",
                Description = "Nội dung bài Hình vuông, hình tròn, hình tam giác, hình chữ nhật gồm các kiến thức cần nắm và bài tập minh họa có hướng dẫn giải chi tiết, đã được BeanMind biên soạn đầy đủ và chi tiết.",
            });
                // Topic table
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("658253c2-663a-406e-a2a0-de0063d3bfd7"),
                    ChapterId = new Guid("66b60dd0-020b-4a09-afc1-80a11d878b60"),
                    Order = 1,
                    Title = "Nhận biết được các hình đã học như hình tròn, hình tam giác, hình vuông, hình chữ nhật",
                    Description = "Trong tiết học này, học sinh sẽ học cách nhận biết và phân biệt các hình học cơ bản như hình tròn, hình tam giác, hình vuông và hình chữ nhật. Bài học sẽ cung cấp các ví dụ minh họa về các hình học này trong đời sống hàng ngày và trong các bài tập. Học sinh sẽ thực hành nhận diện các hình từ các hình ảnh và vật thể thực tế, đồng thời hoàn thành các bài tập giúp củng cố kiến thức về đặc điểm và tính chất của từng loại hình. Các hoạt động bao gồm việc vẽ, nhận diện và phân loại các hình theo đúng tiêu chí. Mục tiêu là giúp học sinh nắm vững khái niệm về các hình học cơ bản và áp dụng chúng vào các bài tập thực tế."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("fd1b8abd-66fc-426f-9bc2-be0aef000cb7"),
                    ChapterId = new Guid("66b60dd0-020b-4a09-afc1-80a11d878b60"),
                    Order = 2,
                    Title = "Đọc đúng tên với hình tương ứng",
                    Description = "Tiết học này tập trung vào việc giúp học sinh đọc và nhận diện đúng tên các hình học cơ bản như hình tròn, hình tam giác, hình vuông, và hình chữ nhật. Học sinh sẽ được cung cấp các hình ảnh của các hình học khác nhau và cần phải chọn tên chính xác của từng hình từ danh sách các lựa chọn. Bài học sẽ bao gồm các hoạt động thực hành, trong đó học sinh sẽ ghép các hình với tên của chúng và giải quyết các bài tập tương tác để củng cố sự hiểu biết. Mục tiêu là giúp học sinh cải thiện kỹ năng nhận diện và đọc tên các hình học một cách chính xác, đồng thời phát triển khả năng phân loại hình học."
                });
                // -----------
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("e7947d3b-599c-4aac-95d3-92f7b62161f3"),
                Order = 2,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                Title = "Thực hành lắp ghép, xếp hình",
                Description = "Bài giảng này bao gồm chi tiết các dạng Toán, bên cạnh đó sử dụng các bài tập minh hoạ kèm theo lời giải chi tiết cho các em tham khảo, rèn luyện kỹ năng giải Toán 1. Mời các em học sinh cùng tham khảo.",
            });
                // Topic table
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("10f32cf1-b1d2-43a9-a35e-25299ef714b8"),
                    ChapterId = new Guid("e7947d3b-599c-4aac-95d3-92f7b62161f3"),
                    Order = 1,
                    Title = "Phân biệt được các hình đã học như hình tròn, hình tam giác, hình vuông, hình chữ nhật",
                    Description = "Tiết học này giúp học sinh phân biệt và nhận diện các hình học cơ bản mà các em đã học, bao gồm hình tròn, hình tam giác, hình vuông, và hình chữ nhật. Học sinh sẽ thực hành việc nhận diện các hình trong các bài tập xếp hình và lắp ghép, từ đó củng cố khả năng phân biệt hình học dựa trên đặc điểm và tính chất của chúng. Các bài tập sẽ bao gồm việc lắp ráp các hình từ các mảnh ghép khác nhau và xếp các hình theo đúng thứ tự, giúp học sinh hiểu rõ hơn về cấu trúc và đặc điểm của từng loại hình. Mục tiêu là giúp học sinh nâng cao kỹ năng nhận diện và phân biệt các hình học một cách chính xác và hiệu quả."
                });
                await _context.Topics.AddAsync(new Topic
                {
                    Id = new Guid("9134b560-a23b-428b-8e9e-341206da2939"),
                    ChapterId = new Guid("e7947d3b-599c-4aac-95d3-92f7b62161f3"),
                    Order = 2,
                    Title = "Nắm được các thao tác đơn giản khi xếp, ghép các hình đơn lẻ thành một hình tổng hợp theo yêu cầu",
                    Description = "Tiết học này tập trung vào việc giúp học sinh nắm vững các thao tác cơ bản khi xếp và ghép các hình đơn lẻ để tạo thành một hình tổng hợp theo yêu cầu. Học sinh sẽ thực hành việc sử dụng các mảnh hình học khác nhau để lắp ráp và tạo thành các hình mới, học cách xác định vị trí và cách sắp xếp các mảnh ghép để hoàn thành hình tổng hợp. Bài học bao gồm các bài tập thực hành với hướng dẫn chi tiết để giúp học sinh hiểu và áp dụng các kỹ thuật ghép hình, từ đó phát triển khả năng tư duy không gian và giải quyết vấn đề. Mục tiêu là giúp học sinh cải thiện kỹ năng lắp ghép và xếp hình một cách chính xác và sáng tạo."
                });
                // -----------
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("5b0f0e59-60b5-47bf-aa81-fd84405ffc54"),
                Order = 3,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                Title = "Luyện tập chung",
                Description = "Bài học Luyện tập chung bao gồm kiến thức cần nhớ và các dạng Toán liên quan được BeanMind tóm tắt một cách chi tiết,",
            });


            // ------------

            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 200,
                Title = "Phép cộng, phép trừ trong phạm vi 10",
                ImageURL = "https://img.loigiaihay.com/picture/2022/0921/bai-2-tr49.JPG",
                Description = "Đến với nội dung Phép cộng, phép trừ trong phạm vi 10 của chương trình Toán 1 Kết Nối Tri Thức, các em sẽ được học hỏi thêm các kiến thức mới về phép toán cộng, trừ trong phạm vi 10. Bên cạnh đó, các em còn được thử sức với các bài tập minh họa cuối mỗi bài học nhằm đánh giá năng lực bản thân sau khi học bài. Hệ thống hỏi đáp cuối bài sẽ giải đáp các thắc mắc của các em trong quá trình học. Mời các em theo dõi nội dung chi tiết bên dưới!",
                TotalSlot = 25,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 100,
                Title = "Làm quen với một số hình khối",
                ImageURL = "https://vnmedia2.monkeyuni.net/upload/web/storage_web/21-08-2023_09:39:28_toan-lop-1-khoi-lap-phuong-0.jpg",
                Description = "Đến với Chương: Làm quen với một số hình khối của chương trình Toán 1 Kết Nối Tri Thức các em sẽ được tiềm hiểu về khối lập phương, khối hợp chữ nhật. Nhằm giúp các em học tập thật tốt và nắm vững kiến thức trọng tâm trong bài, Hoc247 đã biên soạn các bài tập minh họa sau phần tóm tắt lý thuyết. Chúc các em học tập tốt. Nếu có thắc mắc cần giải quyết, hãy bình luận ở phần hỏi đáp cuối bài học.",
                TotalSlot = 24,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("59466844-48d3-4556-a7e7-0422ce299190"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 200,
                Title = "Ôn tập học kì 1",
                ImageURL = "https://play-lh.googleusercontent.com/GndUEl04bwSNJKEPAlRB-8AKfnb5X0oZN_clNmgwBRZl2MOPQpFRCJL_6rFxioj_Gg",
                Description = "Mời các em đến với nội dung Ôn tập học kì 1​​ của chương trình Toán 1 Kết nối tri thức do Hoc247 biên soạn dưới đây. Ở chương này các em học sinh sẽ được ôn tập lại đầy đủ các kiến thức đã học, bên cạnh đó còn có các bài tập minh họa có hướng dẫn giải chi tiết, giúp các em có thể tự luyện tập, đối chiếu đáp án, đánh giá năng lực bản thân sau khi học bài. Hệ thống hỏi đáp sẽ giúp các em giải quyết các thắc mắc liên quan đến bài học.",
                TotalSlot = 24,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("9cea4c8e-2114-4508-a000-c1c743eaa55b"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 200,
                Title = "Các số đến 100",
                ImageURL = "https://booktoan.com/wp-content/uploads/2023/07/unnamed-file-447.png",
                Description = "BeanMind xin giới thiệu các em học sinh Chương: Các số đến 100 môn Toán 1 sách Kết Nối Tri Thức. Nội dung đầy đủ bao gồm tóm tắt lý thuyết, các bài tập SGK và các bài tập minh họa có hướng dẫn giải chi tiết. Hi vọng bài học sẽ giúp các em nắm vững kiến thức và ghi nhớ được các số có hai chữ số, so sánh số có hai chữ số và các phép toán liên quan đến chương. Nếu có bất kì thắc mắc gì về chủ đề này, các em vui lòng bình luận ở mục hỏi đáp để BeanMind hỗ trợ.",
                TotalSlot = 18,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("85e6ab9f-557d-435f-b51c-60124dbc33cc"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 300,
                Title = "Độ dài và đo độ dài",
                ImageURL = "https://img.loigiaihay.com/picture/2022/1107/bai-1-tr115.PNG",
                Description = "Đến với chương Độ dài và đo độ dài Toán 1 Kết Nối Tri Thức các em học sinh sẽ thực hành ước lượng và đo độ dài, biết so sánh các vật dài hơn, ngắn hơn,....BeanMind đã tóm tắt chi tiết các kiến thức cần nhớ, các dạng bài tập và các bài tập minh họa có hướng dẫn giải chi tiết, giúp các em dễ dàng nắm vững được kiến thức mới. Mời quý phụ huynh và các em học sinh cùng tham khảo!",
                TotalSlot = 35,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 200,
                Title = "Phép cộng, phép trừ (không nhớ) trong phạm vi 100",
                ImageURL = "https://vietjack.com/giai-bai-tap-toan-1/images/phep-tru-trong-pham-vi-100-tru-khong-nho-trang-158-1.PNG",
                Description = "Phép cộng, phép trừ (không nhớ) trong phạm vi 100 là một trong những chương học quan trọng của chương trình Toán 1 Kết Nối Tri Thức. BeanMind đã biên soạn chi tiết lý thuyết cần nhớ, bài tập minh họa, giúp các em học sinh nắm vững nội dung như phép cộng, trừ số có hai chữ số cho số có hai chữ số, phép cộng, trừ số có hai chữ số cho số có một chữ số. Sau đây mời quý phụ huynh và các em học sinh cùng tham khảo.",
                TotalSlot = 22,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 200,
                Title = "Thời gian, giờ và lịch",
                ImageURL = "https://www.vietjack.com/toan-1-ket-noi/images/bai-34-xem-gio-dung-tren-dong-ho-5.png",
                Description = "Nhận biết được thời gian trên đồng hồ, xem được ngày tháng trên lịch là nội dung các em sẽ tìm hiểu ở Chương: Thời gian, giờ và lịch của môn Toán 1 Kết Nối Tri thức. Bài học được BeanMind biên soạn với các phần tóm tắt lý thuyết, bài tập minh họa và giúp các em chuẩn bị bài học thật tốt và luyện tập, đánh giá năng lực của bản thân. Hệ thống hỏi đáp sẽ giúp các em giải quyết nhiều câu hỏi khó nhanh chóng, hiệu quả. Các em xem nội dung bài học ngay bên dưới.",
                TotalSlot = 15,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("d1830d8f-2259-43a4-b667-ac35f390a1bc"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 200,
                Title = "Ôn tập cuối năm",
                ImageURL = "https://mir-s3-cdn-cf.behance.net/projects/404/d2c356205497513.Y3JvcCwxMDMzLDgwOCw2MSw2OTM.png",
                Description = "Đến với Ôn tập cuối năm của chương trình Toán 1 Kết Nối Tri Thức đã được BeanMind biên soạn chi tiết các kiến thức cần nhớ và các bài tập minh họa có hướng dẫn giải chi tiết, nhầm giúp các em học tập thật tốt môn Toán 1. Đồng thời đây cũng là tài liệu tham khảo cho các phụ huynh và các giáo viên trong quá trình dạy học. Mời các em học sinh cùng các bậc phụ huynh tham khảo.",
                TotalSlot = 12,
            });
            //
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"), //Lop 2
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Price = 200,
                Title = "Ôn tập và bổ sung",
                ImageURL = "https://infinitylearn.com/surge/wp-content/uploads/2021/12/MicrosoftTeams-image-58.jpg",
                Description = "Mở đầu chương trình Toán 2 Kết Nối Tri Thức, các em sẽ tìm hiểu về Chủ đề 1 : Ôn tập và bổ sung. Gồm các bài học có tóm tắt lý thuyết, cung cấp các bài tập minh họa để các em ôn tập và củng cố kiến thức đã học. Bên cạnh đó, hệ thống hỏi đáp sẽ giúp các em giải đáp các thắc mắc sau khi học bài. Mời các em xem chi tiết bài học.",
                TotalSlot = 20,
            });
                // -----------
                await _context.Chapters.AddAsync(new Chapter
                {
                    Id = new Guid("02f812c2-aeff-45b6-892b-f8548dd1d9b0"),
                    Order = 1,
                    CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                    Title = "Ôn tập các số đến 100",
                    Description = "Bài giảng Cộng, trừ các số tròn chục là một trong những bài học quan trọng trong chương trình Toán 2 sách Kết nối tri thức. BeanMind đã biên soạn chi tiết về lý thuyết cần nhớ và bài tập minh hoạ, giúp các em học sinh nắm được nội dung về các số đến 100 và các dạng toán của nó. Sau đây mời quý phụ huynh và các em học sinh cùng tham khảo.",
                });
                    // Topic table
                    await _context.Topics.AddAsync(new Topic
                    {
                        Id = new Guid("09716311-e60e-4eee-a820-ea95681fe2db"),
                        ChapterId = new Guid("02f812c2-aeff-45b6-892b-f8548dd1d9b0"),
                        Order = 1,
                        Title = "Đọc số",
                        Description = "- Biết đọc, viết, đếm, so sánh các số đến 100 - Nhận biết thứ tự các trong phạm vi các số đến 100 - Vận dụng thứ tự các trong phạm vi các số đến 100, dự đoán quy luật, hoàn thành dãy số"
                    });
                    await _context.Topics.AddAsync(new Topic
                    {
                        Id = new Guid("5778d0ce-c9b5-4257-8008-04afc6d75b91"),
                        ChapterId = new Guid("02f812c2-aeff-45b6-892b-f8548dd1d9b0"),
                        Order = 2,
                        Title = "Chục và đơn vị",
                        Description = "- Nhận biết tên gọi chục, đơn vị, quan hệ giữa chục và đơn vị. - Sử dụng các thuật ngữ chục, đơn vị khi lập số và phân tích số. - Đếm, đọc, viết số phân tích cấu tạo của số - Phân biệt được số chục với số đơn vị. - 10 đơn vị bằng một chục - 1 chục bằng 10 đơn vị - Biết thực hiện phép tính cộn số tròn chục với số có một chữ số."
                    });
                    await _context.Topics.AddAsync(new Topic
                    {
                        Id = new Guid("76c4a31b-a5cc-4e78-94b1-1e44b4b4164e"),
                        ChapterId = new Guid("02f812c2-aeff-45b6-892b-f8548dd1d9b0"),
                        Order = 3,
                        Title = "So sánh các số trong phạm vi 100",
                        Description = "- Biết dựa vào cấu tạo số để so sánh hai hoặc nhiều số. + Hai số có cùng chữ số hàng chục thì số nào có hàng đơn vị lớn hơn sẽ lớn hơn. + Hai số khác chữ số hàng chục thì số nào có hàng chục lớn hơn sẽ lớn hơn. - Xác định số lớn hơn, số bé hơn trong một dãy số có hai chữ số. - So sánh các số rồi chọn số có giá trị lớn nhất hoặc bé nhất trong dãy số đó."
                    });
                    // -----------
                await _context.Chapters.AddAsync(new Chapter
                {
                    Id = new Guid("a2472712-f2d9-41cc-aa31-24a1bc27c279"),
                    Order = 2,
                    CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                    Title = "Tia số. Số liền trước, số liền sau",
                    Description = "Dưới đây là phần tóm tắt lý thuyết và các bài tập minh họa có hướng dẫn giải chi tiết của bài Tia số. Số liền trước, số liền sau được BeanMind biên soạn chi tiết về kiến thức cân nhớ, giúp các em học sinh lớp 2 dễ dàng nắm vững kiến thức quan trọng của bài. Mời các em cùng tham khảo.",
                });
                    // Topic table
                    await _context.Topics.AddAsync(new Topic
                    {
                        Id = new Guid("d49aa131-73a5-4aaa-b142-0aad75eca92d"),
                        ChapterId = new Guid("a2472712-f2d9-41cc-aa31-24a1bc27c279"),
                        Order = 1,
                        Title = "Lý thuyết cần nhớ",
                        Description = "Nhận biết được tia số: Trên tia số - Số 0 ở vạch đầu tiên, là số bé nhất - Mỗi số lớn hơn số bên trái nó và bé hơn số bên phải nó. * Nhận biết số liền trước, số liền sau"
                    });
                    await _context.Topics.AddAsync(new Topic
                    {
                        Id = new Guid("d1d55d76-8e51-4fef-b01b-1711cb303534"),
                        ChapterId = new Guid("a2472712-f2d9-41cc-aa31-24a1bc27c279"),
                        Order = 2,
                        Title = "Bài tập",
                        Description = "Xác định các khoảng cách được chia trên tia số, số đơn vị cách đều rồi đếm và điền các số tương ứng."
                    });
                    await _context.Topics.AddAsync(new Topic
                    {
                        Id = new Guid("cb4bc21f-0fdb-4539-a179-9f1520d9ee86"),
                        ChapterId = new Guid("a2472712-f2d9-41cc-aa31-24a1bc27c279"),
                        Order = 3,
                        Title = "Luyện tập",
                        Description = "Qua bài học này giúp các con: - Biết thế nào là tia số, số liền trước, số liền sau - Vận dụng thực hiện giải các bài toán liên quan đến tia số"
                    });
                    // -----------
                await _context.Chapters.AddAsync(new Chapter
                {
                    Id = new Guid("67f25718-3555-462a-b2cc-63829621c06a"),
                    Order = 3,
                    CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                    Title = "Các thành phần của phép cộng, phép trừ",
                    Description = "Bài học Các thành phần của phép cộng, phép trừ chương trình Toán lớp 2 sách Kết nối tri thức được BeanMind biên soạn nhằm giúp các em học sinh học tập tốt môn Toán lớp 2. Đồng thời đây cũng là tài liệu tham khảo cho các phụ huynh và giáo viên trong quá trình dạy học. Mời các em học sinh cùng các bậc phụ huynh tham khảo.",
                });
                await _context.Chapters.AddAsync(new Chapter
                {
                    Id = new Guid("9e317fb6-1d8f-415c-95c6-4e72e5a40acb"),
                    Order = 4,
                    CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                    Title = "Hơn, kém nhau bao nhiêu?",
                    Description = "Bài giảng Hơn, kém nhau bao nhiêu? bên dưới đây được BeanMind biên soạn chi tiết lý thuyết cần nhớ, sử dụng các bài tập minh hoạ kèm theo hướng dẫn giải chi tiết, dành cho các em học sinh lớp 2 tham khảo, giúp các em học sinh rèn luyện giải môn Toán lớp 2. Mời các em học sinh cùng các bậc phụ huynh tham khảo.",
                });
                await _context.Chapters.AddAsync(new Chapter
                {
                    Id = new Guid("1f3b8cad-5ef0-45a5-babb-5e49d1bf40c8"),
                    Order = 5,
                    CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                    Title = "Ôn tập phép cộng, phép trừ (không nhớ) trong phạm vi 100",
                    Description = "Bài học sau đây gồm chi tiết các dạng Toán về phép cộng, phép trừ (không nhớ) trong phạm vi 100, đồng thời sử dụng các bài tập minh hoạ kèm theo hướng dẫn giải chi tiết, dành cho các em học sinh lớp 2 tham khảo, giúp các em học sinh rèn luyện giải môn Toán lớp 2. Mời các em học sinh cùng các bậc phụ huynh tham khảo.",
                });
                await _context.Chapters.AddAsync(new Chapter
                {
                    Id = new Guid("648c486b-6f2d-4721-b588-54978b92ecfb"),
                    Order = 6,
                    CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                    Title = "Luyện tập chung",
                    Description = "Việc học các kỹ năng giải Toán khi vào lớp 2 là rất quan trọng. Vậy giải Toán như thế nào để phù hợp với tất cả các học sinh, các em có thể tự đọc các kiến thức và tự làm các ví dụ minh họa để nâng cao các kỹ năng giải Toán lớp 2 của mình thêm hiệu quả. Sau đây là một ví dụ minh họa về bài lý thuyết Bài 6 Luyện tập chung sách Kết nối tri thức, mời các em cùng tham khảo.",
                });

            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("ab8c3b06-9ae7-408f-a923-1fb9564ba181"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"), //Lop 2
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Price = 250,
                Title = "Phép cộng, phép trừ trong phạm vi 20",
                ImageURL = "https://mightymath.edu.vn/uploads/pictures/634f768b165b7822016ef8e0/content_bang-tru-trong-pham-vi-20-lop-2-moi.jpg",
                Description = "Đến với nội dung Chủ đề 2: Phép cộng, phép trừ trong phạm vi 20 của chương trình Toán 2 Kết Nối Tri Thức, các em sẽ được học hỏi thêm các kiến thức mới về bảng cộng, bảng trừ qua 10, bài toán về thêm, bớt một số đơn vị,.... Bên cạnh đó, các em còn được thử sức với các bài tập minh họa cuối mỗi bài học nhằm đánh giá năng lực bản thân sau khi học bài. Hệ thống hỏi đáp cuối bài sẽ giải đáp các thắc mắc của các em trong quá trình học. Mời các em theo dõi nội dung chi tiết bên dưới!",
                TotalSlot = 30,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"), //Lop 2
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Price = 250,
                Title = "Làm quen với khối lượng, dung tích",
                ImageURL = "https://pomath.vn/wp-content/uploads/2023/09/toan-lop-2-lam-quen-voi-khoi-luong-va-dung-tich-4-1.jpg",
                Description = "Đến với nội dung Chủ đề 3 : Làm quen với khối lượng, dung tích của chương trình Toán 2 Kết Nối Tri Thức, các em sẽ được học các bài như: Lít, ki-lô-gam, khối lượng và đơn vị đo khối lượng . Bên cạnh đó, các em còn được thử sức với các bài tập minh họa cuối mỗi bài học nhằm đánh giá năng lực bản thân sau khi học bài. Hệ thống hỏi đáp cuối bài sẽ giải đáp các thắc mắc của các em trong quá trình học. Mời các em theo dõi nội dung chi tiết bên dưới!",
                TotalSlot = 15,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("6dc0d9fd-3b29-4e9e-a766-f7aade0ae97a"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"), //Lop 2
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Price = 250,
                Title = "Phép cộng, phép trừ (có nhớ) trong phạm vi 100",
                ImageURL = "https://img.loigiaihay.com/picture/question_lgh/2021_41/1622174452-cech.jpg",
                Description = "Mời các em đến với nội dung Chủ đề 4 : Phép cộng, phép trừ (có nhớ) trong phạm vi 100​​ của chương trình Toán 2 Kết nối tri thức do Benamind biên soạn dưới đây. Ở chương này các em học sinh sẽ được ôn tập lại đầy đủ các kiến thức đã học, bên cạnh đó còn có các bài tập minh họa có hướng dẫn giải chi tiết, giúp các em có thể tự luyện tập, đối chiếu đáp án, đánh giá năng lực bản thân sau khi học bài. Hệ thống hỏi đáp sẽ giúp các em giải quyết các thắc mắc liên quan đến bài học.",
                TotalSlot = 25,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("23c317ba-e081-4ed3-83d8-3c9c52a269d8"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"), //Lop 2
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Price = 250,
                Title = "Làm quen với hình phẳng",
                ImageURL = "https://vnmedia2.monkeyuni.net/upload/web/storage_web/09-06-2022_18:01:39_toan-lop-2-lam-quen-voi-hinh-phang.jpg",
                Description = "Nhận biết được điểm, đoạn thẳng, đường thẳng, đường cong, ba điểm thẳng hàng, đường gấp khúc, hình tứ giác là nội dung các em sẽ tìm hiểu ở Chủ đề 5 : Làm quen với hình phẳng của môn Toán 2 Kết Nối Tri thức. Bài học được BeanMind biên soạn với các phần tóm tắt lý thuyết, bài tập minh họa và giúp các em chuẩn bị bài học thật tốt và luyện tập, đánh giá năng lực của bản thân. Hệ thống hỏi đáp sẽ giúp các em giải quyết nhiều câu hỏi khó nhanh chóng, hiệu quả. Các em xem nội dung bài học ngay bên dưới.",
                TotalSlot = 18,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("c6e8223d-441e-4be2-a541-d33cb55fc6ff"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"), //Lop 2
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Price = 250,
                Title = "Ngày - giờ, giờ - phút, ngày - tháng",
                ImageURL = "https://vnmedia2.monkeyuni.net/upload/web/storage_web/28-03-2022_12:35:29_toan-lop-2-ngay-gio.jpg",
                Description = "BeanMind xin giới thiệu các em học sinh Chủ đề 6 : Ngày - giờ, giờ - phút, ngày - tháng môn Toán 2 sách Kết Nối Tri Thức. Nội dung đầy đủ bao gồm tóm tắt lý thuyết, các bài tập SGK và các bài tập minh họa có hướng dẫn giải chi tiết. Hi vọng bài học sẽ giúp các em nắm vững kiến thức và ghi nhớ được các số có hai chữ số, so sánh số có hai chữ số và các phép toán liên quan đến chương. Nếu có bất kì thắc mắc gì về chủ đề này, các em vui lòng bình luận ở mục hỏi đáp để BeanMind  hỗ trợ.",
                TotalSlot = 22,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("904224da-5ee9-4ce1-9ec8-9e87497cafec"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"), //Lop 2
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Price = 250,
                Title = "Ôn tập Học kì 1",
                ImageURL = "https://i.pinimg.com/736x/51/7e/89/517e890cd1d9606b78ccbdd08f0fdf1d.jpg",
                Description = "Ôn tập Học kì 1 Toán 2 Kết Nối Tri Thức các em học sinh sẽ Ôn tập hình phẳng, Ôn tập đo lường, Ôn tập phép cộng, phép trừ trong phạm vi 20, 100. BeanMind đã tóm tắt chi tiết các kiến thức cần nhớ, các dạng bài tập và các bài tập minh họa có hướng dẫn giải chi tiết, giúp các em dễ dàng nắm vững được kiến thức mới. Mời quý phụ huynh và các em học sinh cùng tham khảo!",
                TotalSlot = 35,
            });

            // --------------
            // TeachingSlot table
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("3efbbaca-4aa1-45f2-98a0-12fbc2399185"),
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 3,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("811c17cb-527e-4658-9db4-454fdeeca3ef"),
                StartTime = "8 am",
                EndTime = "10 am",
                DayIndex = 5,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("0471144b-8ed3-4e78-b032-d5ca3c5ddfa5"),
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 4,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            //-----
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("c608dd61-9076-4181-9786-6c6b211b0bbd"),
                StartTime = "3 pm",
                EndTime = "5 pm",
                DayIndex = 3,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("5ee277ed-5b88-4c4f-8ba6-07aac526ce60"),
                StartTime = "10 am",
                EndTime = "12 am",
                DayIndex = 4,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("eeb6c8b1-3d26-4e3d-bf09-3c01b44d15ac"),
                StartTime = "10 am",
                EndTime = "12 am",
                DayIndex = 1,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
            });
            //------
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("60913325-34df-4a96-956c-2a32b0fde16d"),
                StartTime = "2 pm",
                EndTime = "3 pm",
                DayIndex = 3,
                CourseId = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("b6459368-0bc0-4632-9e73-c1009c5832c8"),
                StartTime = "2 pm",
                EndTime = "3 pm",
                DayIndex = 5,
                CourseId = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("ce616e03-1b16-4920-8fed-822f3e274ad6"),
                StartTime = "2 pm",
                EndTime = "3 pm",
                DayIndex = 6,
                CourseId = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
            });
            //------
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("a4f96e29-4944-4e5a-af44-8cee69660c3d"),
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 0,
                CourseId = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("bbd02525-16fa-46da-834b-e37f92ff4fb5"),
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 2,
                CourseId = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("1229415f-16fd-4e1b-b568-2218421da13a"),
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 4,
                CourseId = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
            });
            // ------------
        }

        public async static Task Seed_Session_Enrollment_Participant_Teachable_Async(ApplicationDbContext _context)
        {
            // Session table
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("c8f560be-8762-4cb6-bc1f-ad64f3dac67e"),
                TeachingSlotId = new Guid("0471144b-8ed3-4e78-b032-d5ca3c5ddfa5"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea40",
                Date = DateTime.Parse("08/05/2024")
            });
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("92a70117-01f5-41c2-805a-bcacddc872c1"),
                TeachingSlotId = new Guid("811c17cb-527e-4658-9db4-454fdeeca3ef"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea40",
                Date = DateTime.Parse("08/02/2024")
            });
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("501aad6e-40e9-4a4e-ba0f-247e1c7f97a0"),
                TeachingSlotId = new Guid("811c17cb-527e-4658-9db4-454fdeeca3ef"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea41",
                Date = DateTime.Parse("08/01/2024")
            });
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("d4390f9a-f21a-404f-8fdc-5d4b132bb2f3"),
                TeachingSlotId = new Guid("3efbbaca-4aa1-45f2-98a0-12fbc2399185"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea43",
                Date = DateTime.Parse("08/04/2024")
            });
            await _context.Sessions.AddAsync(new Session
            {
                Id = new Guid("26a7510c-0d5b-4b4b-9775-9578d01120b9"),
                TeachingSlotId = new Guid("0471144b-8ed3-4e78-b032-d5ca3c5ddfa5"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea44",
                Date = DateTime.Parse("08/03/2024")
            });
            // -----------
            // Enrollment table
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("a17c4640-6f6a-4fa3-9c96-f8758de0ccc7"),
                CourseId = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b0",
                Status = Domain.Enums.EnrollmentStatus.Complete,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b1",
                Status = Domain.Enums.EnrollmentStatus.Complete,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("540fe297-72b3-40cf-bb2e-ce9d3d5cfce7"),
                CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b0",
                Status = Domain.Enums.EnrollmentStatus.OnGoing,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("2e90ca3c-8296-4bc7-adf4-8e8d4b363616"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b0",
                Status = Domain.Enums.EnrollmentStatus.OnGoing,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("877ddfd8-2ec2-445c-aeaf-1a51a6a40cd5"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b2",
                Status = Domain.Enums.EnrollmentStatus.OnGoing,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("5e0e13d9-ec68-4de8-8456-e6882071eb89"),
                CourseId = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b2",
                Status = Domain.Enums.EnrollmentStatus.Complete,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("d0d67119-944a-4ac7-a6a9-e888f50bf05c"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b0",
                Status = Domain.Enums.EnrollmentStatus.OnGoing,
            });
            // -----------
            // Participant table
            await _context.Participants.AddAsync(new Participant
            {
                Id = new Guid("4e21ac1d-1c74-440d-8208-df31fd60aff4"),
                EnrollmentId = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                SessionId = new Guid("c8f560be-8762-4cb6-bc1f-ad64f3dac67e"),
                IsPresent = true,
            });
            await _context.Participants.AddAsync(new Participant
            {
                Id = new Guid("99a17610-466b-403c-b161-0fd728b41dac"),
                EnrollmentId = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                SessionId = new Guid("92a70117-01f5-41c2-805a-bcacddc872c1"),
                IsPresent = true,
            });
            // -----------
            // Teachable table
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("82cc29db-7118-40ee-b989-7fff95cc3469"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea40",
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("871c0c1a-63fe-42f6-87c6-6eb599ee9526"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea41",
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("d828cbec-50e4-498d-8385-63ed13a8a558"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea42",
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("054d8c0c-9037-45d0-84bc-17b1bdc2f28b"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea41",
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("ff7a0187-c8a6-478b-aa1e-91d2e6867111"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea43",
            });
            // -----------
        }

        public async static Task Seed_QuestionBank_Async(ApplicationDbContext _context)
        {
            // QuestionLevel table
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("26fb0c3c-2f79-4940-ac2c-6ef7ba427d92"),
                Title = "Dễ",
                Description = "Dễ"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("871d2de9-cfca-4ed0-a9a9-658639d664df"),
                Title = "Trung bình",
                Description = "Trung bình"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("8abb4833-8443-4aab-b996-dc1eff84bd41"),
                Title = "Khá",
                Description = "Khá"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("8f45dab4-c1f8-4528-8ca4-ba5f682f847d"),
                Title = "Khó",
                Description = "Khó"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("ca44a423-5b69-4953-8e94-8e4b771bef19"),
                Title = "Cực khó",
                Description = "Cực khó"
            });
            // -----------
            var questionLevels = new Dictionary<Guid, string>
            {
                { new Guid("26fb0c3c-2f79-4940-ac2c-6ef7ba427d92"), "Dễ" },
                { new Guid("871d2de9-cfca-4ed0-a9a9-658639d664df"), "Trung bình" },
                { new Guid("8abb4833-8443-4aab-b996-dc1eff84bd41"), "Khá" },
                { new Guid("8f45dab4-c1f8-4528-8ca4-ba5f682f847d"), "Khó" },
                { new Guid("ca44a423-5b69-4953-8e94-8e4b771bef19"), "Cực khó" }
            };
            var topics = new List<Guid>
            {
                new Guid("6d8c088e-bc7e-409f-8c79-31066d6df42e"),
                new Guid("59ee169a-c77f-431b-9cf6-b2c36ab3a4fe"),
                new Guid("afb89987-0405-4641-b595-b634f422cbed"),
                new Guid("3829a480-2d22-44da-82c1-38da6fd0a6c9"),
                new Guid("73558a53-e910-470a-a261-e42d803508e9")
            };
            Random random = new Random();
            int totalGenerate = 100; //100 questions
            for (int i = 0; i < totalGenerate; i++)
            {
                Guid questionLevel = questionLevels.ElementAt(random.Next(questionLevels.Count)).Key;
                string questionLevelName = questionLevels.ElementAt(random.Next(questionLevels.Count)).Value;
                Guid topicId = topics[random.Next(topics.Count)];

                //Seeding question
                var question = new Question
                {
                    Id = new Guid(),
                    Content = "(Seeding question for testing) Câu hỏi thứ " + (i+1).ToString() +" - " + questionLevelName + " : ",
                    TopicId = topicId,
                    QuestionLevelId = questionLevel
                };
                await _context.Questions.AddAsync(question);

                //Seeding question answers
                for (int j = 0; j < 4; j++) {
                    await _context.QuestionAnswers.AddAsync(new QuestionAnswer
                    {
                        Id = new Guid(),
                        QuestionId = question.Id,
                        Content = "Câu trả lời thứ " + j + " cho câu hỏi " + i + " " + ((i % 4 == j) ? "true" : "false"),
                        IsCorrect = (i%4 == j) ? true : false,
                    });
                }
            }
        }

        public async static Task Seed_Worksheet_Async(ApplicationDbContext _context)
        {
            // WorksheetTemplate table
            await _context.WorksheetTemplates.AddAsync(new WorksheetTemplate
            {
                Id = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                Title = "Mẫu kiểm tra 1",
                CourseId = new Guid("CEAF0F02-168D-4F69-975F-14A61D492886"),
                Classification = 0
            });
            // LevelTemplateRelation table
            await _context.LevelTemplateRelations.AddAsync(new LevelTemplateRelation
            {
                Id = new Guid("a054bf47-6783-4cbe-ac05-02b5b22b84ec"),
                QuestionLevelId = new Guid("871D2DE9-CFCA-4ED0-A9A9-658639D664DF"), //Trung bình
                WorksheetTemplateId = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                NoQuestions = 10
            });
            await _context.LevelTemplateRelations.AddAsync(new LevelTemplateRelation
            {
                Id = new Guid("b2679983-24f2-4643-8ec6-316a5e670de9"),
                QuestionLevelId = new Guid("8F45DAB4-C1F8-4528-8CA4-BA5F682F847D"), //Khó
                WorksheetTemplateId = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                NoQuestions = 5
            });
            await _context.LevelTemplateRelations.AddAsync(new LevelTemplateRelation
            {
                Id = new Guid("de30e2e6-a203-494d-a8ed-8a279f36f25c"),
                QuestionLevelId = new Guid("26FB0C3C-2F79-4940-AC2C-6EF7BA427D92"), //Dễ
                WorksheetTemplateId = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                NoQuestions = 10
            });
            // Worksheet table
            await _context.Worksheets.AddAsync(new Worksheet
            {
                Id = new Guid("89b67173-669d-443f-ba87-79b8791c6238"),
                WorksheetTemplateId = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                Title = "Bài kiểm tra 1",
                Description = "10 trung bình, 10 dễ, 5 khó"
            });
            await _context.Worksheets.AddAsync(new Worksheet
            {
                Id = new Guid("6834d36d-b09d-4812-8aa9-7001ce6f9833"),
                WorksheetTemplateId = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                Title = "Bài kiểm tra 2",
                Description = "10 trung bình, 10 dễ, 5 khó"
            });
            var questions = _context.Questions.AsQueryable().ToList();
            Random random = new Random();
            for (int i = 10;i< 20; i++)
            {
                // Select a random question from the list
                var question = questions[random.Next(questions.Count)];
                await _context.WorksheetQuestions.AddAsync(new WorksheetQuestion
                {
                    Id = new Guid("23528d26-7db3-444d-9e9d-9a11948f7b" + i.ToString()),                    
                    QuestionId = question.Id,
                    WorksheetId = new Guid("89b67173-669d-443f-ba87-79b8791c6238"),
                });
            }
            for (int i = 10; i < 20; i++)
            {
                // Select a random question from the list
                var question = questions[random.Next(questions.Count)];
                await _context.WorksheetQuestions.AddAsync(new WorksheetQuestion
                {
                    Id = new Guid("858d6e8c-0887-4fea-99ce-390c5ad73c" + i.ToString()),
                    QuestionId = question.Id,
                    WorksheetId = new Guid("6834d36d-b09d-4812-8aa9-7001ce6f9833"),
                });
            }
        }

        public class GameSeed
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
            public string ItemStoreJson { get; set; }
            public string AnimalJson { get; set; }
            public Guid Id { get; set; }
            public bool IsDeleted { get; set; }
        }

        public static DateOnly GetRandomDateOnly()
        {
            Random random = new Random();
            DateOnly start = new DateOnly(2000, 1, 1);
            int range = (DateTime.Today - start.ToDateTime(TimeOnly.MinValue)).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
