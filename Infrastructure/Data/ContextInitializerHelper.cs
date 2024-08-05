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
                Id = new Guid("b8fc90e5-a56f-4ac0-b6bb-cd3eea88d4a1"),
                Title = "Tiền Tiểu Học",
                Description = "Chương trình Toán tư duy cấp Tiền Tiểu Học tập trung vào việc phát triển khả năng tư duy và suy luận logic của trẻ nhỏ thông qua các hoạt động học tập vui nhộn và sáng tạo. Trẻ sẽ học các khái niệm cơ bản về số đếm, hình dạng và mẫu hình học, đồng thời phát triển kỹ năng giải quyết vấn đề qua các trò chơi và bài toán đơn giản liên quan đến cuộc sống hàng ngày."
            });
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
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("b8fc90e5-a56f-4ac0-b6bb-cd3eea88d4a1"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Đại số đại cương",
                ImageURL = "https://static.vecteezy.com/system/resources/previews/013/400/591/non_2x/education-concept-with-cartoon-students-vector.jpg",
                Description = "Đại số là một phần quan trọng của toán học, tập trung vào việc nghiên cứu và giải quyết các vấn đề liên quan đến biểu thức, phương trình và hệ phương trình. Trong đại số, học sinh học về cách tạo ra và giải quyết các biểu thức và phương trình để tìm ra giá trị của các biến số. Điều này có thể bao gồm cả các khái niệm như phép cộng, phép trừ, phép nhân, phép chia, cũng như các phương pháp giải các hệ phương trình.",
                TotalSlot = 20,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Hình học đại cương",
                ImageURL = "https://static.vecteezy.com/system/resources/thumbnails/002/399/898/small_2x/education-concept-with-funny-characters-vector.jpg",
                Description = "Hình học nghiên cứu về các hình học cơ bản như hình vuông, hình tròn, tam giác và các hình khác, cũng như các phép biến đổi hình học như tịnh tiến, quay và phản xạ. Học sinh được giáo dục về cách tính diện tích, chu vi và khám phá các tính chất đặc biệt của các hình học này.",
                TotalSlot = 30,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("9e735658-0002-45c9-a6d5-1a579dba49ee"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Số học ứng dụng",
                ImageURL = "https://c8.alamy.com/comp/M71DKY/vector-illustration-of-three-stick-kids-jumping-together-in-the-field-M71DKY.jpg",
                Description = "Số học là nền tảng của toán học, tập trung vào việc nghiên cứu và hiểu về các số và phép tính. Trong số học, học sinh học cách thực hiện các phép toán cơ bản như cộng, trừ, nhân, chia, cũng như các khái niệm như số nguyên tố, bội số chung nhỏ nhất và cách áp dụng chúng vào các bài toán thực tế.",
                TotalSlot = 20,
            });
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("3f6798e9-eed3-40db-ae92-7dd0da9a6435"),
                ProgramTypeId = new Guid("96cd2a6c-1013-4bb9-8927-047fc25e0402"),
                Title = "Tư duy Logic",
                ImageURL = "https://i.pinimg.com/736x/fa/3a/1a/fa3a1ac70a55ba27576e41d2335e253c.jpg",
                Description = "Tư duy logic là khả năng quan trọng giúp con người suy luận và giải quyết vấn đề một cách có cấu trúc và logic. Trong giáo dục, việc phát triển tư duy logic không chỉ là mục tiêu của quá trình học tập mà còn là công cụ quan trọng giúp học sinh phát triển kỹ năng suy nghĩ sâu sắc, phân tích thông tin và đưa ra nhận định có chất lượng. Thông qua việc thực hành giải quyết các bài toán phức tạp và tham gia vào các hoạt động nghiên cứu và phân tích, học sinh có cơ hội rèn luyện tư duy logic và áp dụng nó vào các tình huống thực tế, từ đó phát triển sự tự tin và thành công trong học tập và cuộc sống.",
                TotalSlot = 24,
            });
            // --------------
            // TeachingSlot table
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("3efbbaca-4aa1-45f2-98a0-12fbc2399185"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 3,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("811c17cb-527e-4658-9db4-454fdeeca3ef"),
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 5,
                Title = "TeachingSlot Tilte",
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("0471144b-8ed3-4e78-b032-d5ca3c5ddfa5"),
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 6,
                Title = "TeachingSlot Tilte",
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            //-----
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("c608dd61-9076-4181-9786-6c6b211b0bbd"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 3,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("5ee277ed-5b88-4c4f-8ba6-07aac526ce60"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 4,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("eeb6c8b1-3d26-4e3d-bf09-3c01b44d15ac"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 7,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
            });
            //------
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("60913325-34df-4a96-956c-2a32b0fde16d"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 3,
                CourseId = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("b6459368-0bc0-4632-9e73-c1009c5832c8"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 5,
                CourseId = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("ce616e03-1b16-4920-8fed-822f3e274ad6"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 6,
                CourseId = new Guid("6c215522-0925-4f86-b0fd-72f746ca9fd6"),
            });
            //------
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("a4f96e29-4944-4e5a-af44-8cee69660c3d"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 3,
                CourseId = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("bbd02525-16fa-46da-834b-e37f92ff4fb5"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 3,
                CourseId = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("1229415f-16fd-4e1b-b568-2218421da13a"),
                Title = "TeachingSlot Tilte",
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 3,
                CourseId = new Guid("c2ad8bc5-d7ef-4639-87b2-d251854656a1"),
            });
            // ------------
            // Chapter table
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Biểu thức đại số",
                Description = "Giới thiệu về các biểu thức đại số, bao gồm các phép toán cơ bản như cộng, trừ, nhân và chia biểu thức.",
            });
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("a4562cbc-b8f8-4bca-b537-77d7b5e4eacc"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Phương trình đại số cơ bản",
                Description = "Học cách giải các phương trình đơn giản bằng cách tìm giá trị của biến số.",
            });
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("d0f9feee-92c2-4f01-9fc7-4c801c3202c7"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Hệ phương trình đại số",
                Description = "Giải quyết các bài toán liên quan đến hệ phương trình đại số bằng các phương pháp như phương pháp loại trừ hoặc phương pháp thế.",
            });
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("6b760186-9678-4e66-81f1-cb3aefe56e9f"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Bất phương trình và hệ bất phương trình",
                Description = "Nghiên cứu về cách giải và hiểu về các bất phương trình và hệ bất phương trình trong đại số.",
            });
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("3f15e8a2-247e-4d79-85f4-d46a73f7782b"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                Title = "Kiểm tra thành tựu",
                Description = "Kiểm tra chung kiến thức về chủ đề Đại số",
            });
            // ------------
            // Topic table
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("6d8c088e-bc7e-409f-8c79-31066d6df42e"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Biểu thức đại số cơ bản",
                Description = "Giới thiệu về cấu trúc của biểu thức đại số, bao gồm các phép toán cơ bản như cộng, trừ, nhân và chia.",
            });
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("b78c510e-f524-4a78-a8c1-58c22063e0a0"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Rút gọn biểu thức",
                Description = "Học cách rút gọn và đơn giản hóa các biểu thức đại số bằng cách sử dụng các quy tắc phù hợp.",
            });
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("59ee169a-c77f-431b-9cf6-b2c36ab3a4fe"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Phân tích và định giá trị của biểu thức",
                Description = "Phân tích cấu trúc và tính chất của các biểu thức đại số và tìm giá trị của chúng cho các giá trị cụ thể của biến số.",
            });
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("afb89987-0405-4641-b595-b634f422cbed"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Biểu thức đại số có mũ và căn bậc hai",
                Description = "Nghiên cứu về các biểu thức chứa mũ và căn bậc hai, bao gồm các quy tắc biến đổi và tính toán.",
            });
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("3829a480-2d22-44da-82c1-38da6fd0a6c9"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Biểu thức đại số đa biến",
                Description = "Khám phá về cách xử lý và giải quyết các biểu thức đại số có nhiều hơn một biến số.",
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("b4f46040-35ea-43f3-a586-0816f17b219b"),
                ChapterId = new Guid("71d018c0-c040-4116-808f-2c3ae70d9ae9"),
                Title = "Ứng dụng của biểu thức đại số",
                Description = "Áp dụng kiến thức về biểu thức đại số vào các bài toán thực tế trong các lĩnh vực như kinh tế, khoa học, và kỹ thuật.",
            });
            // -----------
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
                Id = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b1",
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
                Status = Domain.Enums.EnrollmentStatus.OnGoing,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("d0d67119-944a-4ac7-a6a9-e888f50bf05c"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b0",
                Status = Domain.Enums.EnrollmentStatus.Complete,
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
                new Guid("b78c510e-f524-4a78-a8c1-58c22063e0a0"),
                new Guid("59ee169a-c77f-431b-9cf6-b2c36ab3a4fe"),
                new Guid("afb89987-0405-4641-b595-b634f422cbed"),
                new Guid("3829a480-2d22-44da-82c1-38da6fd0a6c9"),
                new Guid("b4f46040-35ea-43f3-a586-0816f17b219b")
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
                        Content = "Câu trả lời thứ " + j + " cho câu hỏi " + i + " ",
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
            for (int i = 10;i< 30; i++)
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
            for (int i = 10; i < 40; i++)
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
