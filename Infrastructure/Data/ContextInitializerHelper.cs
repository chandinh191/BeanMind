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
                ContentURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fcac_so_1_den_10.pdf?alt=media&token=0310819c-a0f8-4d00-aef5-3864b50a38da",
                Price = 200,
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fcourse1.png?alt=media&token=4293c282-2182-45f9-89b9-9378022274fc",
                Description = "Mở đầu chương trình Toán 1 Kết Nối Tri Thức, các em sẽ tìm hiểu về Các số từ 0 đến 10. Gồm các bài học có tóm tắt lý thuyết, cung cấp các bài tập minh họa để các em ôn tập và củng cố kiến thức đã học. Bên cạnh đó, hệ thống hỏi đáp sẽ giúp các em giải đáp các thắc mắc sau khi học bài. Mời các em xem chi tiết bài học.",
                TotalSlot = 8,
            });
            _context.SaveChanges();
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

            // ---------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------

            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 150,
                ContentURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fhinh%20hoc.pdf?alt=media&token=0649e7de-2bbd-47fc-aebb-213cc3c8b742",
                Title = "Làm quen với một số hình phẳng",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fcourse2.png?alt=media&token=6a85663e-21c2-42a8-9fde-ee324d2323c3",
                Description = "Nhận biết được một số hình vuông, hình tròn, hình tam giác, hình chữ nhật là nội dung các em sẽ tìm hiểu ở Chương: Làm quen với một số hình phẳng của môn Toán 1 Kết Nối Tri thức. Bài học được BeanMind biên soạn với các phần tóm tắt lý thuyết, bài tập minh họa và giúp các em chuẩn bị bài học thật tốt và luyện tập, đánh giá năng lực của bản thân. Hệ thống hỏi đáp sẽ giúp các em giải quyết nhiều câu hỏi khó nhanh chóng, hiệu quả. Các em xem nội dung bài học ngay bên dưới.",
                TotalSlot = 5,
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

            // Adding a new chapter to the course
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("d8b99e0e-1f4f-4a5b-8bcf-bd3f6d5b9d5e"),
                Order = 4,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                Title = "Ứng dụng thực tế của các hình học",
                Description = "Bài giảng này tập trung vào việc giúp học sinh nhận diện và áp dụng các hình học cơ bản như hình tròn, hình tam giác, hình vuông, và hình chữ nhật vào các tình huống thực tế. Các em sẽ được hướng dẫn cách nhận diện các hình học này trong môi trường xung quanh, từ đó giúp phát triển khả năng tư duy không gian và ứng dụng toán học vào cuộc sống hàng ngày."
            });

            // Adding topics to the new chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("7f2a9d3e-cf5d-4ec5-97f2-0954f994ca78"),
                ChapterId = new Guid("d8b99e0e-1f4f-4a5b-8bcf-bd3f6d5b9d5e"),
                Order = 1,
                Title = "Nhận diện các hình trong đời sống",
                Description = "Trong bài học này, học sinh sẽ học cách nhận diện và phân loại các hình học cơ bản như hình tròn, hình tam giác, hình vuông và hình chữ nhật trong các vật thể và cảnh vật hàng ngày. Bằng cách quan sát xung quanh, các em sẽ phát hiện các hình học này trong các đồ vật quen thuộc như bánh xe, cửa sổ, và mái nhà."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("c1eb5dcb-9b91-4db5-bf3f-b5fcff1c3c7e"),
                ChapterId = new Guid("d8b99e0e-1f4f-4a5b-8bcf-bd3f6d5b9d5e"),
                Order = 2,
                Title = "Ứng dụng các hình học trong giải toán",
                Description = "Bài học này giúp học sinh học cách áp dụng kiến thức về các hình học cơ bản để giải quyết các bài toán và vấn đề thực tế. Học sinh sẽ thực hành việc sử dụng hình học để phân tích các vấn đề, như tính diện tích của các hình cơ bản và giải các bài toán liên quan đến hình học trong cuộc sống hàng ngày."
            });

            // Adding the fifth chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("a9b77f21-4d4e-45f8-8b7f-82f12d5b64fa"),
                Order = 5,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                Title = "Vẽ và tô màu các hình đơn giản",
                Description = "Bài học này giúp các em học sinh lớp 1 học cách vẽ và tô màu các hình đơn giản như hình tròn, hình tam giác, hình vuông và hình chữ nhật. Các em sẽ được hướng dẫn từng bước để cầm bút đúng cách, vẽ các hình cơ bản và sau đó tô màu cho các hình vẽ của mình, giúp phát triển kỹ năng vận động tinh và sự sáng tạo."
            });

            // Adding topics to the fifth chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("5ecfa8be-df16-4d1b-8b5e-0f91bc5c6815"),
                ChapterId = new Guid("a9b77f21-4d4e-45f8-8b7f-82f12d5b64fa"),
                Order = 1,
                Title = "Vẽ các hình học cơ bản",
                Description = "Trong phần này, học sinh sẽ học cách vẽ các hình học cơ bản như hình tròn, hình tam giác, hình vuông và hình chữ nhật theo các bước đơn giản. Các em sẽ được thực hành vẽ các hình này từ các mẫu hình có sẵn và sau đó tự tạo ra các hình của riêng mình, giúp củng cố kỹ năng vẽ và nhận diện hình học."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("bd8ef469-8d44-4d4c-a06f-5f6c0be9f94c"),
                ChapterId = new Guid("a9b77f21-4d4e-45f8-8b7f-82f12d5b64fa"),
                Order = 2,
                Title = "Tô màu các hình học",
                Description = "Học sinh sẽ thực hành tô màu cho các hình học đã vẽ, giúp phát triển khả năng sáng tạo và kỹ năng vận động tinh. Bài học sẽ hướng dẫn các em cách lựa chọn màu sắc phù hợp, tô màu đều và cẩn thận, từ đó tạo ra những hình vẽ đẹp mắt và độc đáo."
            });

            // Adding the sixth chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("b2d0c5c6-9587-4ae3-9c7c-69f96e5910f9"),
                Order = 6,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                Title = "Nhận biết hình học trong thực tế",
                Description = "Bài học này giúp các em học sinh lớp 1 nhận biết các hình học cơ bản trong đời sống hàng ngày, chẳng hạn như hình tròn trong bánh xe, hình vuông trong ô cửa, và hình tam giác trong mái nhà. Các em sẽ tham gia vào các hoạt động quan sát và phân loại để kết nối kiến thức hình học với các đối tượng thực tế xung quanh."
            });

            // Adding topics to the sixth chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("2e3f08b3-5c6b-4f70-b0d8-4b7e3d6d65b8"),
                ChapterId = new Guid("b2d0c5c6-9587-4ae3-9c7c-69f96e5910f9"),
                Order = 1,
                Title = "Tìm các hình học xung quanh em",
                Description = "Học sinh sẽ học cách tìm và nhận diện các hình học cơ bản xung quanh mình trong các môi trường khác nhau, chẳng hạn như ở nhà, trường học và ngoài trời. Bài học sẽ khuyến khích các em quan sát kỹ lưỡng và ghi chép các hình học mà các em nhìn thấy."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("0f66d8a7-7e94-411b-8df5-6aafdffcb52a"),
                ChapterId = new Guid("b2d0c5c6-9587-4ae3-9c7c-69f96e5910f9"),
                Order = 2,
                Title = "Thực hành nhận diện hình học",
                Description = "Học sinh sẽ tham gia vào các hoạt động thực hành nhận diện và phân loại các hình học cơ bản từ các vật dụng xung quanh. Bài học sẽ bao gồm các trò chơi và bài tập tương tác để giúp các em phân loại và nhận diện các hình học một cách chính xác và thú vị."
            });

            // ---------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------

            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 200,
                Title = "Phép cộng, phép trừ (không nhớ) trong phạm vi 100",
                ContentURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fcong_tru_trong_pham_vi_100.pdf?alt=media&token=90722fe3-39e2-4bdb-a07b-6cbabfba2e02",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fcourse3.png?alt=media&token=cc0a0bf6-31cc-49ea-8605-bf4275945292",
                Description = "Phép cộng, phép trừ (không nhớ) trong phạm vi 100 là một trong những chương học quan trọng của chương trình Toán 1 Kết Nối Tri Thức. BeanMind đã biên soạn chi tiết lý thuyết cần nhớ, bài tập minh họa, giúp các em học sinh nắm vững nội dung như phép cộng, trừ số có hai chữ số cho số có hai chữ số, phép cộng, trừ số có hai chữ số cho số có một chữ số. Sau đây mời quý phụ huynh và các em học sinh cùng tham khảo.",
                TotalSlot = 8,
            });
            // Adding the first chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("bdc7f6c3-4ec9-43af-b7b2-cb3f2f82f832"),
                Order = 1,
                CourseId = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
                Title = "Giới thiệu phép cộng và phép trừ",
                Description = "Bài học này sẽ giới thiệu các khái niệm cơ bản về phép cộng và phép trừ trong phạm vi 100. Học sinh sẽ tìm hiểu cách thực hiện phép cộng và phép trừ với các số đơn giản mà không cần nhớ. Qua các ví dụ minh họa trực quan và bài tập thực hành, các em sẽ nắm vững cách sử dụng các phép toán cơ bản này để giải quyết các bài toán đơn giản. Bài học cũng sẽ bao gồm các phương pháp và chiến lược để giúp học sinh làm quen và tự tin hơn với các phép cộng và phép trừ."
            });

            // Adding topics to the first chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("46b4f1d3-8fb7-4cf4-897a-dff92a267f63"),
                ChapterId = new Guid("bdc7f6c3-4ec9-43af-b7b2-cb3f2f82f832"),
                Order = 1,
                Title = "Khái niệm cơ bản về phép cộng",
                Description = "Trong bài học này, học sinh sẽ học cách thực hiện phép cộng các số có hai chữ số mà không cần nhớ. Các em sẽ được cung cấp các ví dụ minh họa cụ thể, hướng dẫn từng bước về cách cộng hai số, cùng với các bài tập thực hành với các số đơn giản. Bài học cũng sẽ bao gồm các trò chơi toán học để củng cố kiến thức và giúp học sinh thấy được ứng dụng thực tiễn của phép cộng trong cuộc sống hàng ngày."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("75f3e5fc-7fdf-42d3-9c1b-ea3a646f591e"),
                ChapterId = new Guid("bdc7f6c3-4ec9-43af-b7b2-cb3f2f82f832"),
                Order = 2,
                Title = "Khái niệm cơ bản về phép trừ",
                Description = "Học sinh sẽ học cách thực hiện phép trừ các số có hai chữ số mà không cần nhớ trong bài học này. Các em sẽ được hướng dẫn chi tiết về các bước thực hiện phép trừ, cùng với các ví dụ và bài tập thực hành để củng cố kiến thức. Bài học cũng sẽ bao gồm các hoạt động tương tác và trò chơi để giúp học sinh nắm vững kỹ năng trừ và áp dụng chúng vào các tình huống thực tế."
            });

            // Adding the second chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("17f90b49-5e56-4b8a-9e7d-c75bdb6d8c37"),
                Order = 2,
                CourseId = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
                Title = "Luyện tập cộng và trừ các số trong phạm vi 100",
                Description = "Bài học này cung cấp nhiều bài tập luyện tập phép cộng và phép trừ với các số trong phạm vi 100. Học sinh sẽ thực hành qua các bài tập có độ khó tăng dần, giúp các em củng cố và nâng cao kỹ năng phép toán cơ bản. Bài học cũng bao gồm các trò chơi tương tác và hoạt động nhóm để khuyến khích học sinh làm việc cùng nhau và học tập một cách vui vẻ và hiệu quả."
            });

            // Adding topics to the second chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("a1a7e03f-5c11-4f1d-b5f4-3e788a5886ab"),
                ChapterId = new Guid("17f90b49-5e56-4b8a-9e7d-c75bdb6d8c37"),
                Order = 1,
                Title = "Bài tập cộng cơ bản",
                Description = "Trong phần này, học sinh sẽ thực hành các bài tập về phép cộng không nhớ với các số trong phạm vi 100. Các bài tập sẽ được thiết kế để phù hợp với nhiều cấp độ kỹ năng khác nhau, bao gồm cả bài tập cá nhân và bài tập nhóm. Mục tiêu là giúp học sinh làm quen với các phép cộng khác nhau và cải thiện khả năng giải toán của mình qua việc thực hành liên tục."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("35fa7f50-22cf-4e26-9e9e-cd2de0f3c5db"),
                ChapterId = new Guid("17f90b49-5e56-4b8a-9e7d-c75bdb6d8c37"),
                Order = 2,
                Title = "Bài tập trừ cơ bản",
                Description = "Bài tập trừ cơ bản sẽ giúp học sinh làm quen với các phép trừ không nhớ trong phạm vi 100. Các bài tập sẽ được thiết kế để phù hợp với nhiều cấp độ khác nhau, bao gồm bài tập cá nhân và nhóm. Học sinh sẽ được thực hành các phép trừ khác nhau, cải thiện khả năng giải toán và hiểu rõ hơn về quy trình thực hiện phép trừ."
            });

            // Adding the third chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("b1eafbf6-303e-486d-b5d4-5193e9378a2a"),
                Order = 3,
                CourseId = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
                Title = "Áp dụng phép cộng và trừ trong các tình huống thực tế",
                Description = "Bài học này giúp học sinh áp dụng kiến thức về phép cộng và phép trừ vào các tình huống thực tế như mua sắm, phân chia đồ vật, và các tình huống hàng ngày khác. Học sinh sẽ được hướng dẫn cách sử dụng các phép toán để giải quyết các bài toán thực tế, từ việc tính tổng chi phí khi mua sắm đến việc phân chia đồ vật. Bài học cũng bao gồm các hoạt động thực tiễn để giúp học sinh hiểu rõ hơn về ứng dụng của toán học trong cuộc sống."
            });

            // Adding topics to the third chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("74f5e643-9795-41e2-b33b-758d7032e4f3"),
                ChapterId = new Guid("b1eafbf6-303e-486d-b5d4-5193e9378a2a"),
                Order = 1,
                Title = "Tính toán trong mua sắm",
                Description = "Học sinh sẽ học cách áp dụng phép cộng và phép trừ khi mua sắm. Các bài học sẽ bao gồm các tình huống thực tế như tính toán tổng chi phí của nhiều mặt hàng hoặc số tiền còn lại sau khi mua hàng. Bài học sẽ sử dụng các ví dụ thực tế và các bài tập ứng dụng để giúp học sinh hiểu rõ hơn cách sử dụng phép toán trong các tình huống mua sắm thực tế, từ đó phát triển kỹ năng tính toán trong đời sống hàng ngày."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("e5f7b8c1-917f-48a2-983f-b621e1c89736"),
                ChapterId = new Guid("b1eafbf6-303e-486d-b5d4-5193e9378a2a"),
                Order = 2,
                Title = "Tính toán khi phân chia đồ vật",
                Description = "Học sinh sẽ thực hành phân chia đồ vật thành các nhóm bằng cách sử dụng phép cộng và phép trừ. Các bài tập sẽ giúp học sinh hiểu cách chia sẻ và phân loại đồ vật trong các tình huống cụ thể, như phân chia đồ chơi cho các bạn cùng lớp hoặc chia sẻ bánh kẹo trong một bữa tiệc. Bài học sẽ bao gồm các hoạt động thực tế và bài tập để học sinh có thể áp dụng kiến thức vào các tình huống thực tế."
            });

            // Adding the fourth chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("11dabe72-3157-49f1-9d2e-6386a1f2b19c"),
                Order = 4,
                CourseId = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
                Title = "Thực hành với các trò chơi và hoạt động tương tác",
                Description = "Bài học này sẽ sử dụng các trò chơi và hoạt động tương tác để giúp học sinh luyện tập phép cộng và phép trừ. Các trò chơi sẽ được thiết kế để khuyến khích học sinh làm việc nhóm và học tập vui vẻ, từ việc giải các bài toán qua các trò chơi toán học đến các hoạt động nhóm. Mục tiêu là tạo môi trường học tập năng động và thú vị, giúp học sinh củng cố kiến thức và phát triển kỹ năng giải toán một cách hiệu quả."
            });

            // Adding topics to the fourth chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("a8e8e5cb-0d65-44e7-8c2a-4d57e2e67322"),
                ChapterId = new Guid("11dabe72-3157-49f1-9d2e-6386a1f2b19c"),
                Order = 1,
                Title = "Trò chơi toán học",
                Description = "Học sinh sẽ tham gia vào các trò chơi toán học thú vị để thực hành phép cộng và phép trừ. Các trò chơi được thiết kế để giúp học sinh phát triển kỹ năng giải toán qua các hoạt động giải trí, bao gồm các trò chơi cá nhân và nhóm. Bài học này nhằm mục đích làm cho việc học toán trở nên vui vẻ và hấp dẫn, đồng thời khuyến khích học sinh tự tin hơn trong việc giải quyết các bài toán."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("1f12ec3c-94b3-41e4-b2d4-e6af318f2355"),
                ChapterId = new Guid("11dabe72-3157-49f1-9d2e-6386a1f2b19c"),
                Order = 2,
                Title = "Hoạt động nhóm với phép cộng và trừ",
                Description = "Học sinh sẽ thực hiện các hoạt động nhóm để giải quyết các bài toán về phép cộng và phép trừ. Các hoạt động nhóm bao gồm giải các bài toán theo nhóm, thi đua giải toán, và các trò chơi nhóm. Mục tiêu của bài học là khuyến khích học sinh hợp tác, làm việc nhóm, và cùng nhau giải quyết vấn đề. Các hoạt động này không chỉ giúp học sinh củng cố kiến thức toán học mà còn phát triển kỹ năng làm việc nhóm và giao tiếp."
            });

            // Adding the fifth chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("874dd7d2-9bc6-48da-bf50-1070b6343202"),
                Order = 5,
                CourseId = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
                Title = "Kiểm tra và đánh giá",
                Description = "Bài học cuối cùng của khóa học sẽ bao gồm một bài kiểm tra để đánh giá khả năng của học sinh về phép cộng và phép trừ trong phạm vi 100. Bài kiểm tra sẽ đánh giá sự hiểu biết của học sinh về các khái niệm cơ bản và khả năng áp dụng chúng vào các tình huống thực tế. Sau khi kiểm tra, học sinh sẽ nhận được phản hồi chi tiết để cải thiện kỹ năng của mình. Bài học cũng sẽ bao gồm các hoạt động phân tích kết quả để giúp học sinh nhận ra điểm mạnh và điểm cần cải thiện."
            });

            // Adding topics to the fifth chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("4c5f3b70-1ad2-4c94-8994-00e4e3e379a7"),
                ChapterId = new Guid("874dd7d2-9bc6-48da-bf50-1070b6343202"),
                Order = 1,
                Title = "Bài kiểm tra tổng hợp",
                Description = "Học sinh sẽ thực hiện một bài kiểm tra tổng hợp để đánh giá kiến thức về phép cộng và phép trừ trong phạm vi 100. Bài kiểm tra sẽ bao gồm các câu hỏi đa dạng, từ các bài toán cơ bản đến các bài toán ứng dụng thực tiễn. Mục tiêu là giúp học sinh kiểm tra sự hiểu biết toàn diện về các khái niệm đã học và khả năng áp dụng chúng vào các tình huống thực tế."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("10cbeb5e-b6e2-4374-9150-10d5f67e46b1"),
                ChapterId = new Guid("874dd7d2-9bc6-48da-bf50-1070b6343202"),
                Order = 2,
                Title = "Phân tích và cải thiện kỹ năng",
                Description = "Sau khi hoàn thành bài kiểm tra, học sinh sẽ phân tích kết quả và nhận phản hồi chi tiết để cải thiện kỹ năng. Bài học sẽ bao gồm các hoạt động phân tích kết quả bài kiểm tra, nhận xét về các điểm mạnh và điểm yếu, và lập kế hoạch để cải thiện các kỹ năng còn thiếu. Các hoạt động này nhằm mục đích giúp học sinh tự đánh giá và phát triển một kế hoạch học tập để tiếp tục nâng cao khả năng giải toán của mình."
            });

            // ---------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------

            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), //Toans
                CourseLevelId = new Guid("8a7b78a9-d209-473e-a133-919479d61d5c"), //Lop 1
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"), //BGD
                Price = 200,
                ContentURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fthoigian_gio_va_lich.pdf?alt=media&token=f6748ba7-851b-4594-8e00-d47e5324d1fb",
                Title = "Thời gian, giờ và lịch",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fcourse%204.png?alt=media&token=b284d40a-5b62-4661-a87f-67840aed6986",
                Description = "Nhận biết được thời gian trên đồng hồ, xem được ngày tháng trên lịch là nội dung các em sẽ tìm hiểu ở Chương: Thời gian, giờ và lịch của môn Toán 1 Kết Nối Tri thức. Bài học được BeanMind biên soạn với các phần tóm tắt lý thuyết, bài tập minh họa và giúp các em chuẩn bị bài học thật tốt và luyện tập, đánh giá năng lực của bản thân. Hệ thống hỏi đáp sẽ giúp các em giải quyết nhiều câu hỏi khó nhanh chóng, hiệu quả. Các em xem nội dung bài học ngay bên dưới.",
                TotalSlot = 8,
            });
            // Adding the first chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("b5f96c58-6d7a-4b1f-b3e5-7c4b945e7f26"),
                Order = 1,
                CourseId = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
                Title = "Giới thiệu về thời gian",
                Description = "Chương đầu tiên của khóa học sẽ giúp học sinh hiểu rõ về khái niệm thời gian, bao gồm cách sử dụng đồng hồ để đọc giờ và phân biệt giữa các phần của đồng hồ như kim giờ, kim phút và kim giây. Học sinh sẽ học cách xác định thời gian trong ngày, cách đọc thời gian trên đồng hồ và sự khác biệt giữa các đơn vị đo thời gian như giờ và phút. Chương này cung cấp các ví dụ thực tế và bài tập thực hành để giúp các em nắm vững các khái niệm cơ bản về thời gian, từ đó áp dụng chúng vào cuộc sống hàng ngày một cách hiệu quả."
            });

            // Adding topics to the first chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("e02d4f3b-84cb-4e47-a6b1-1a0d2c21434f"),
                ChapterId = new Guid("b5f96c58-6d7a-4b1f-b3e5-7c4b945e7f26"),
                Order = 1,
                Title = "Đọc giờ trên đồng hồ",
                Description = "Trong bài học này, học sinh sẽ tìm hiểu về cách đọc giờ trên đồng hồ cơ bản, bao gồm việc xác định kim giờ, kim phút, và kim giây. Học sinh sẽ học cách đọc các mốc thời gian quan trọng trong ngày như giờ, phút, và giây, và cách chuyển đổi giữa các mốc thời gian khác nhau. Bài học sẽ bao gồm các ví dụ minh họa chi tiết và bài tập thực hành để giúp học sinh làm quen với việc đọc giờ chính xác và hiệu quả."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("b66b9a9f-61ef-49b5-8a0e-c8f9d6a91737"),
                ChapterId = new Guid("b5f96c58-6d7a-4b1f-b3e5-7c4b945e7f26"),
                Order = 2,
                Title = "Phân biệt giờ và phút",
                Description = "Bài học này sẽ giúp học sinh phân biệt rõ ràng giữa giờ và phút, và cách sử dụng đồng hồ để đo lường thời gian. Học sinh sẽ học cách xác định và ghi nhớ các mốc thời gian quan trọng trong ngày, và các khái niệm về sự khác biệt giữa giờ và phút. Bài học sẽ bao gồm các ví dụ cụ thể và các bài tập để học sinh có thể thực hành và củng cố kiến thức của mình về cách đo lường thời gian."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("d49f827f-9c8d-4707-b431-f3de8a9b8f73"),
                ChapterId = new Guid("b5f96c58-6d7a-4b1f-b3e5-7c4b945e7f26"),
                Order = 3,
                Title = "Thực hành đo thời gian",
                Description = "Trong bài học này, học sinh sẽ thực hành đo thời gian bằng cách sử dụng đồng hồ và các công cụ đo thời gian khác. Học sinh sẽ tham gia vào các hoạt động thực tế như đo thời gian cho các sự kiện hàng ngày và ghi lại kết quả. Bài học sẽ bao gồm các bài tập thực hành và các bài kiểm tra để giúp học sinh áp dụng kiến thức của mình vào các tình huống thực tế và nâng cao khả năng đo thời gian của mình."
            });

            // Adding the second chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("a8e4f712-572f-4b3e-a75f-7b5f03b83c8f"),
                Order = 2,
                CourseId = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
                Title = "Lịch và ngày tháng",
                Description = "Chương này tập trung vào việc hiểu và sử dụng lịch và ngày tháng trong cuộc sống hàng ngày. Học sinh sẽ được giới thiệu về cấu trúc của lịch, bao gồm các thành phần như ngày, tháng, và năm. Các em sẽ học cách đọc và hiểu ngày tháng trên lịch, xác định các ngày trong tuần, và cách lên kế hoạch cho các sự kiện dựa trên lịch. Bài học cũng bao gồm các bài tập thực hành và các hoạt động để giúp học sinh nắm vững cách sử dụng lịch một cách hiệu quả."
            });

            // Adding topics to the second chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("f7b14a8e-205a-45dc-8e3e-d60a457f01e6"),
                ChapterId = new Guid("a8e4f712-572f-4b3e-a75f-7b5f03b83c8f"),
                Order = 1,
                Title = "Hiểu về lịch",
                Description = "Bài học này giúp học sinh hiểu cấu trúc và các phần của lịch, bao gồm ngày, tháng, và năm. Học sinh sẽ tìm hiểu cách sử dụng lịch để theo dõi thời gian và các sự kiện quan trọng, và cách xác định các ngày trong tuần và các tháng trong năm. Bài học sẽ cung cấp các ví dụ cụ thể và bài tập thực hành để học sinh nắm vững kiến thức về lịch và cách áp dụng nó vào cuộc sống hàng ngày."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("3c7a4f9b-6bfb-4c54-a1c8-3f07e4b41bfc"),
                ChapterId = new Guid("a8e4f712-572f-4b3e-a75f-7b5f03b83c8f"),
                Order = 2,
                Title = "Đọc ngày tháng trên lịch",
                Description = "Trong bài học này, học sinh sẽ học cách đọc và hiểu ngày tháng trên lịch. Các em sẽ học cách xác định các ngày trong tháng, các tuần trong tháng, và các tháng trong năm. Bài học sẽ bao gồm các hoạt động thực hành để giúp học sinh nắm vững cách đọc ngày tháng và áp dụng nó vào việc lập kế hoạch cho các hoạt động trong cuộc sống."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("f4e5b6a3-8f4a-4f59-911e-0b6f18d934a6"),
                ChapterId = new Guid("a8e4f712-572f-4b3e-a75f-7b5f03b83c8f"),
                Order = 3,
                Title = "Lên kế hoạch với lịch",
                Description = "Bài học này giúp học sinh học cách sử dụng lịch để lên kế hoạch cho các sự kiện và hoạt động trong tương lai. Các em sẽ tìm hiểu cách tổ chức các hoạt động hàng ngày, lập kế hoạch cho các sự kiện quan trọng như sinh nhật và lễ hội, và theo dõi các hoạt động dựa trên lịch. Bài học sẽ bao gồm các bài tập thực hành và các hoạt động để giúp học sinh áp dụng kiến thức về lịch vào việc lập kế hoạch hiệu quả."
            });

            // Adding the third chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("e1c9d6f7-1d69-4f46-b55e-4e6c5c2b973b"),
                Order = 3,
                CourseId = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
                Title = "So sánh thời gian",
                Description = "Chương này giúp học sinh phát triển kỹ năng so sánh thời gian bằng cách sử dụng đồng hồ và lịch. Học sinh sẽ học cách so sánh các khoảng thời gian khác nhau, xác định thời gian sớm hơn hoặc muộn hơn, và áp dụng các kỹ năng so sánh vào các tình huống thực tế. Bài học sẽ bao gồm các ví dụ cụ thể và bài tập thực hành để giúp học sinh nắm vững kỹ năng so sánh thời gian và áp dụng nó vào việc lập kế hoạch và tổ chức các hoạt động."
            });

            // Adding topics to the third chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("a1b9f0e8-4b6c-4b2b-933d-09a3dff7f5d4"),
                ChapterId = new Guid("e1c9d6f7-1d69-4f46-b55e-4e6c5c2b973b"),
                Order = 1,
                Title = "So sánh thời gian giữa các hoạt động",
                Description = "Bài học này giúp học sinh học cách so sánh thời gian giữa các hoạt động khác nhau trong ngày. Các em sẽ tìm hiểu cách xác định thời gian bắt đầu và kết thúc của các hoạt động, và so sánh thời gian giữa các hoạt động để lên kế hoạch cho các hoạt động tiếp theo. Bài học sẽ bao gồm các bài tập thực hành và ví dụ cụ thể để giúp học sinh áp dụng kỹ năng so sánh thời gian vào việc tổ chức các hoạt động hàng ngày."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("8b2e7f9b-cd12-4d52-89f1-f8e9b5fbd51d"),
                ChapterId = new Guid("e1c9d6f7-1d69-4f46-b55e-4e6c5c2b973b"),
                Order = 2,
                Title = "So sánh thời gian và lịch",
                Description = "Trong bài học này, học sinh sẽ học cách so sánh thời gian trên đồng hồ với ngày tháng trên lịch. Các em sẽ tìm hiểu cách so sánh các khoảng thời gian khác nhau, xác định các mốc thời gian quan trọng trong ngày và trong tuần, và áp dụng kỹ năng so sánh vào các tình huống thực tế. Bài học sẽ bao gồm các hoạt động thực hành và bài tập để giúp học sinh nắm vững kỹ năng so sánh thời gian và lịch."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("5d1b0f6c-5c6e-4b77-9e0f-bb9c15a42b61"),
                ChapterId = new Guid("e1c9d6f7-1d69-4f46-b55e-4e6c5c2b973b"),
                Order = 3,
                Title = "Áp dụng so sánh thời gian vào thực tế",
                Description = "Trong bài học này, học sinh sẽ học cách áp dụng kỹ năng so sánh thời gian vào các tình huống thực tế. Các em sẽ thực hiện các bài tập về việc lập kế hoạch cho các hoạt động và sự kiện dựa trên việc so sánh thời gian và ngày tháng. Bài học sẽ bao gồm các ví dụ thực tế và hoạt động để giúp học sinh áp dụng kiến thức của mình vào cuộc sống hàng ngày."
            });

            // Adding the fourth chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("c9b25a3f-29dc-4d59-b9c4-917b2a0cf3d6"),
                Order = 4,
                CourseId = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
                Title = "Ứng dụng thời gian trong cuộc sống",
                Description = "Chương này giúp học sinh hiểu cách áp dụng các khái niệm về thời gian vào cuộc sống hàng ngày. Các em sẽ tìm hiểu về cách sử dụng thời gian để tổ chức các hoạt động trong ngày, lên kế hoạch cho các sự kiện và quản lý thời gian hiệu quả. Học sinh sẽ được giới thiệu về các công cụ quản lý thời gian, cách lập kế hoạch cho các sự kiện và hoạt động dựa trên thời gian, và cách duy trì sự cân bằng giữa các hoạt động học tập và vui chơi. Bài học sẽ bao gồm các ví dụ thực tế, bài tập và hoạt động để giúp học sinh áp dụng kiến thức về thời gian vào việc lập kế hoạch và tổ chức các hoạt động hàng ngày."
            });

            // Adding topics to the fourth chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("e4b8c06e-2a9b-4c7e-b4f0-2c87b6b2f6d9"),
                ChapterId = new Guid("c9b25a3f-29dc-4d59-b9c4-917b2a0cf3d6"),
                Order = 1,
                Title = "Quản lý thời gian cá nhân",
                Description = "Bài học này giúp học sinh học cách quản lý thời gian cá nhân hiệu quả. Các em sẽ tìm hiểu các kỹ năng tổ chức thời gian, lập kế hoạch cho các hoạt động hàng ngày, và cách duy trì sự cân bằng giữa học tập và giải trí. Bài học sẽ bao gồm các ví dụ thực tế và bài tập để giúp học sinh áp dụng các kỹ năng quản lý thời gian vào cuộc sống hàng ngày."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("e93f6d6b-d586-4a7b-bf0e-df5d227db8b9"),
                ChapterId = new Guid("c9b25a3f-29dc-4d59-b9c4-917b2a0cf3d6"),
                Order = 2,
                Title = "Lên kế hoạch cho sự kiện",
                Description = "Trong bài học này, học sinh sẽ học cách lập kế hoạch cho các sự kiện quan trọng như sinh nhật, lễ hội, và các hoạt động ngoài trời. Các em sẽ tìm hiểu cách tổ chức thời gian và tài nguyên để đảm bảo sự kiện diễn ra thành công. Bài học sẽ bao gồm các bài tập thực hành và ví dụ cụ thể để giúp học sinh áp dụng kiến thức về lập kế hoạch vào việc tổ chức các sự kiện."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("c89b5d62-cf46-4e3d-bf9a-0b77d7b6248a"),
                ChapterId = new Guid("c9b25a3f-29dc-4d59-b9c4-917b2a0cf3d6"),
                Order = 3,
                Title = "Duy trì sự cân bằng trong cuộc sống",
                Description = "Bài học này giúp học sinh học cách duy trì sự cân bằng giữa học tập, vui chơi, và các hoạt động khác. Các em sẽ tìm hiểu về tầm quan trọng của việc quản lý thời gian hiệu quả và cách duy trì sự cân bằng giữa các hoạt động hàng ngày. Bài học sẽ bao gồm các ví dụ thực tế và các hoạt động để giúp học sinh áp dụng kiến thức vào việc duy trì sự cân bằng trong cuộc sống của mình."
            });

            // ---------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------
 
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"), //Lop 2
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Price = 200,
                Title = "Ôn tập và bổ sung",
                ContentURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fon_tap_lop_1_ky_1.pdf?alt=media&token=66ad5131-8065-421c-980c-4492819ed4d7",
                ImageURL = "https://infinitylearn.com/surge/wp-content/uploads/2021/12/MicrosoftTeams-image-58.jpg",
                Description = "Mở đầu chương trình Toán 2 Kết Nối Tri Thức, các em sẽ tìm hiểu về Chủ đề 1 : Ôn tập và bổ sung. Gồm các bài học có tóm tắt lý thuyết, cung cấp các bài tập minh họa để các em ôn tập và củng cố kiến thức đã học. Bên cạnh đó, hệ thống hỏi đáp sẽ giúp các em giải đáp các thắc mắc sau khi học bài. Mời các em xem chi tiết bài học.",
                TotalSlot = 10,
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

            // ---------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"), // Toan
                CourseLevelId = new Guid("dd885d8d-0ea4-4c19-9b06-5e02bb44e7bb"), //Lop 2
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),//BGD
                Price = 250,
                ContentURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fhinh%20hoc.pdf?alt=media&token=0649e7de-2bbd-47fc-aebb-213cc3c8b742",
                Title = "Làm quen với khối lượng, dung tích",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/course_img%2Fcourse5.png?alt=media&token=a5d09f6d-086d-4b51-ad94-2729feb6d45b",
                Description = "Đến với nội dung Chủ đề 3 : Làm quen với khối lượng, dung tích của chương trình Toán 2 Kết Nối Tri Thức, các em sẽ được học các bài như: Lít, ki-lô-gam, khối lượng và đơn vị đo khối lượng . Bên cạnh đó, các em còn được thử sức với các bài tập minh họa cuối mỗi bài học nhằm đánh giá năng lực bản thân sau khi học bài. Hệ thống hỏi đáp cuối bài sẽ giải đáp các thắc mắc của các em trong quá trình học. Mời các em theo dõi nội dung chi tiết bên dưới!",
                TotalSlot = 5,
            });
            // Adding the first chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("e8f8b8e4-fc25-4efc-946b-14c744d17b29"),
                Order = 1,
                CourseId = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
                Title = "Khái niệm cơ bản về khối lượng và dung tích",
                Description = "Bài học đầu tiên của chúng ta sẽ giúp các em làm quen với các khái niệm cơ bản về khối lượng và dung tích. Các em sẽ học cách phân biệt khối lượng và dung tích, cũng như các đơn vị đo lường cơ bản như lít và ki-lô-gam. Bài học sẽ bao gồm các ví dụ minh họa rõ ràng và các bài tập thực hành giúp các em nắm vững lý thuyết và ứng dụng các khái niệm vào thực tế."
            });

            // Adding topics to the first chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("c1f5e7a9-372f-4d69-a1d8-5f8c79b8c9e0"),
                ChapterId = new Guid("e8f8b8e4-fc25-4efc-946b-14c744d17b29"),
                Order = 1,
                Title = "Khái niệm về khối lượng",
                Description = "Trong bài học này, các em sẽ tìm hiểu về khối lượng - một thuộc tính quan trọng của vật thể, phản ánh độ nặng của nó. Các em sẽ học các đơn vị đo lường khối lượng như ki-lô-gam và gram, và cách chuyển đổi giữa các đơn vị này. Bài học sẽ cung cấp các ví dụ thực tế về việc sử dụng khối lượng trong cuộc sống hàng ngày và các bài tập giúp các em làm quen với các phép toán cơ bản liên quan đến khối lượng."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("c7e4d38c-b55a-4e9f-8f87-e5a3ef7bfa89"),
                ChapterId = new Guid("e8f8b8e4-fc25-4efc-946b-14c744d17b29"),
                Order = 2,
                Title = "Khái niệm về dung tích",
                Description = "Bài học này sẽ giúp các em hiểu về dung tích, là lượng không gian bên trong một vật chứa. Các em sẽ học về các đơn vị đo lường dung tích như lít và mili-lít, và cách sử dụng chúng để đo lường các chất lỏng. Các ví dụ minh họa sẽ bao gồm việc đo dung tích của các vật chứa thông dụng trong nhà bếp và các bài tập giúp các em thực hành và làm quen với khái niệm dung tích."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("d5e1f27d-2f6d-4e55-bb3d-3456e7a46f7c"),
                ChapterId = new Guid("e8f8b8e4-fc25-4efc-946b-14c744d17b29"),
                Order = 3,
                Title = "So sánh khối lượng và dung tích",
                Description = "Trong bài học này, các em sẽ học cách so sánh khối lượng và dung tích giữa các vật thể. Các em sẽ tìm hiểu sự khác biệt giữa việc đo lường khối lượng và đo lường dung tích, và cách so sánh chúng trong các tình huống thực tế. Bài học sẽ cung cấp các bài tập so sánh thực tế và các trò chơi tương tác để giúp các em củng cố kiến thức và nâng cao khả năng so sánh các đơn vị đo lường."
            });

            // Adding the second chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("d3d6745e-05d8-4736-8aeb-45d8d325fa87"),
                Order = 2,
                CourseId = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
                Title = "Đơn vị đo khối lượng và dung tích",
                Description = "Bài học này tập trung vào việc tìm hiểu các đơn vị đo khối lượng và dung tích. Các em sẽ học về các đơn vị đo lường như lít, mili-lít, ki-lô-gam và gram, và cách sử dụng chúng trong các phép đo. Bài học sẽ bao gồm các hoạt động thực hành đo khối lượng và dung tích của các vật thể khác nhau, giúp các em làm quen với việc sử dụng các đơn vị đo lường trong thực tế."
            });

            // Adding topics to the second chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("8a47a5a1-007c-4b78-8b09-6d7b6ae56d85"),
                ChapterId = new Guid("d3d6745e-05d8-4736-8aeb-45d8d325fa87"),
                Order = 1,
                Title = "Đơn vị đo khối lượng: Ki-lô-gam và gram",
                Description = "Trong bài học này, các em sẽ tìm hiểu chi tiết về đơn vị đo khối lượng như ki-lô-gam và gram. Các em sẽ học cách sử dụng các đơn vị này để đo khối lượng của các vật thể và thực hành chuyển đổi giữa ki-lô-gam và gram. Bài học sẽ bao gồm các ví dụ minh họa và bài tập thực hành để các em làm quen với việc đo lường khối lượng trong cuộc sống hàng ngày."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("d6c6d1b8-4b91-4b4a-8b6b-5b94c124d6a5"),
                ChapterId = new Guid("d3d6745e-05d8-4736-8aeb-45d8d325fa87"),
                Order = 2,
                Title = "Đơn vị đo dung tích: Lít và mili-lít",
                Description = "Bài học này sẽ giúp các em hiểu về các đơn vị đo dung tích như lít và mili-lít. Các em sẽ học cách đo dung tích của các chất lỏng và thực hành chuyển đổi giữa lít và mili-lít. Bài học sẽ bao gồm các ví dụ minh họa và bài tập thực hành giúp các em làm quen với việc đo lường dung tích trong các tình huống thực tế, chẳng hạn như đo lượng nước trong một chai hoặc lượng sữa trong một hộp."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("0b9f6d34-fd4d-4f5d-bd6c-9c53b94f23d9"),
                ChapterId = new Guid("d3d6745e-05d8-4736-8aeb-45d8d325fa87"),
                Order = 3,
                Title = "Chuyển đổi đơn vị đo khối lượng và dung tích",
                Description = "Trong bài học này, các em sẽ học cách chuyển đổi giữa các đơn vị đo khối lượng và dung tích. Các em sẽ thực hành chuyển đổi giữa ki-lô-gam và gram, lít và mili-lít qua các bài tập thực hành và ví dụ cụ thể. Bài học sẽ giúp các em củng cố khả năng làm việc với các đơn vị đo lường và ứng dụng chúng vào các tình huống thực tế."
            });

            // Adding the third chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("f6c7e09a-bb65-4c35-bd9e-9e9b1b530b2d"),
                Order = 3,
                CourseId = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
                Title = "Tính toán với khối lượng và dung tích",
                Description = "Bài học này sẽ giúp các em áp dụng các kiến thức đã học về khối lượng và dung tích vào các bài toán tính toán. Các em sẽ thực hành các phép toán cơ bản như cộng và trừ với các đơn vị đo lường, và giải quyết các bài toán thực tiễn liên quan đến khối lượng và dung tích. Bài học sẽ bao gồm các ví dụ cụ thể và bài tập thực hành giúp các em nâng cao kỹ năng tính toán và giải quyết vấn đề."
            });

            // Adding topics to the third chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("14d0e16d-1e2d-4c84-97d3-2a97baf5f45e"),
                ChapterId = new Guid("f6c7e09a-bb65-4c35-bd9e-9e9b1b530b2d"),
                Order = 1,
                Title = "Cộng và trừ khối lượng",
                Description = "Bài học này sẽ giúp các em thực hành các phép toán cộng và trừ với các đơn vị đo khối lượng. Các em sẽ làm quen với việc cộng và trừ các giá trị khối lượng và thực hành giải quyết các bài toán thực tế liên quan đến khối lượng. Bài học sẽ cung cấp các ví dụ và bài tập để các em củng cố khả năng thực hiện các phép toán với đơn vị đo khối lượng."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("d9e2c7b1-09a8-49d8-890f-5f6c8d5c1f12"),
                ChapterId = new Guid("f6c7e09a-bb65-4c35-bd9e-9e9b1b530b2d"),
                Order = 2,
                Title = "Cộng và trừ dung tích",
                Description = "Trong bài học này, các em sẽ thực hành các phép toán cộng và trừ với các đơn vị đo dung tích. Các em sẽ học cách cộng và trừ các giá trị dung tích và giải quyết các bài toán thực tiễn liên quan đến dung tích. Bài học sẽ bao gồm các ví dụ minh họa và bài tập thực hành giúp các em làm quen với các phép toán liên quan đến dung tích."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("912a3f1e-c3d9-46ba-8a47-48a2c643c3dc"),
                ChapterId = new Guid("f6c7e09a-bb65-4c35-bd9e-9e9b1b530b2d"),
                Order = 3,
                Title = "Bài toán thực tiễn về khối lượng và dung tích",
                Description = "Bài học này sẽ đưa các em vào các bài toán thực tiễn liên quan đến khối lượng và dung tích. Các em sẽ giải quyết các tình huống thực tế, chẳng hạn như tính toán lượng thực phẩm cần mua hoặc lượng nước cần chuẩn bị cho một bữa tiệc. Bài học sẽ bao gồm các bài tập và ví dụ giúp các em áp dụng kiến thức đã học vào các tình huống thực tế."
            });

            // Adding the fourth chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("b2f38e25-68fc-4e2e-a1e0-2d4e7c4b8d79"),
                Order = 4,
                CourseId = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
                Title = "Ứng dụng khối lượng và dung tích trong cuộc sống hàng ngày",
                Description = "Bài học này sẽ giúp các em nhận diện và ứng dụng các kiến thức về khối lượng và dung tích vào cuộc sống hàng ngày. Các em sẽ học cách sử dụng các đơn vị đo lường trong các tình huống thực tiễn, như mua sắm, nấu ăn, và các hoạt động khác. Bài học sẽ bao gồm các hoạt động thực hành và bài tập giúp các em hiểu rõ hơn về việc sử dụng khối lượng và dung tích trong thực tế."
            });

            // Adding topics to the fourth chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("b8f4d2e5-3f0d-4bde-9c4b-7e8d5f01b8d9"),
                ChapterId = new Guid("b2f38e25-68fc-4e2e-a1e0-2d4e7c4b8d79"),
                Order = 1,
                Title = "Mua sắm với khối lượng và dung tích",
                Description = "Bài học này sẽ giúp các em hiểu cách sử dụng khối lượng và dung tích khi mua sắm các sản phẩm. Các em sẽ học cách đọc nhãn sản phẩm, nhận diện các đơn vị đo lường và tính toán số lượng cần thiết khi mua sắm. Bài học sẽ bao gồm các ví dụ thực tế và bài tập để các em làm quen với việc áp dụng khối lượng và dung tích trong các tình huống mua sắm."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("c5e8f5c1-6a3b-4e09-bb2d-82e6f5b2e9d7"),
                ChapterId = new Guid("b2f38e25-68fc-4e2e-a1e0-2d4e7c4b8d79"),
                Order = 2,
                Title = "Nấu ăn và đo lường khối lượng, dung tích",
                Description = "Trong bài học này, các em sẽ học cách đo lường khối lượng và dung tích trong quá trình nấu ăn. Các em sẽ tìm hiểu cách sử dụng các công cụ đo lường như cốc đo lít và cân điện tử để đo lượng nguyên liệu. Bài học sẽ bao gồm các ví dụ minh họa và bài tập thực hành giúp các em nắm vững cách sử dụng khối lượng và dung tích trong việc chuẩn bị các món ăn."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("a7e9b3f4-2b0d-4b5d-8b1b-4c7e9e3d6f7a"),
                ChapterId = new Guid("b2f38e25-68fc-4e2e-a1e0-2d4e7c4b8d79"),
                Order = 3,
                Title = "Ứng dụng trong các trò chơi và hoạt động",
                Description = "Bài học này sẽ khám phá cách khối lượng và dung tích được áp dụng trong các trò chơi và hoạt động hàng ngày. Các em sẽ học cách sử dụng các đơn vị đo lường trong các trò chơi như đo lường lượng nước trong một bể bơi hoặc khối lượng của các vật phẩm trong một trò chơi. Bài học sẽ bao gồm các ví dụ và bài tập thực hành để các em thấy rõ sự ứng dụng của khối lượng và dung tích trong các hoạt động giải trí."
            });

            // Adding the fifth chapter
            await _context.Chapters.AddAsync(new Chapter
            {
                Id = new Guid("a5f2d8d6-7a9c-4e7c-8d4e-58a9b1c8c2d8"),
                Order = 5,
                CourseId = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
                Title = "Tổng kết và đánh giá",
                Description = "Bài học cuối cùng sẽ tổng kết các kiến thức về khối lượng và dung tích mà các em đã học. Các em sẽ làm bài kiểm tra tổng hợp để đánh giá khả năng của mình về các khái niệm cơ bản. Bài học cũng sẽ cung cấp phản hồi chi tiết để các em hiểu rõ hơn về điểm mạnh và các lĩnh vực cần cải thiện. Các hoạt động phân tích kết quả sẽ giúp các em nhận ra các khía cạnh cần chú ý để nâng cao kỹ năng của mình."
            });

            // Adding topics to the fifth chapter
            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("7e9d4f67-8e2d-4d83-bf93-8d2c0b1a9a8d"),
                ChapterId = new Guid("a5f2d8d6-7a9c-4e7c-8d4e-58a9b1c8c2d8"),
                Order = 1,
                Title = "Bài kiểm tra tổng hợp",
                Description = "Học sinh sẽ thực hiện một bài kiểm tra tổng hợp để đánh giá kiến thức về khối lượng và dung tích. Bài kiểm tra sẽ bao gồm các câu hỏi từ cơ bản đến nâng cao, giúp các em kiểm tra khả năng hiểu biết và áp dụng các khái niệm đã học vào các tình huống thực tế. Bài kiểm tra cũng sẽ giúp các em nhận diện những lĩnh vực cần cải thiện và củng cố kiến thức."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("c6b1d3e8-3b9c-4f77-bc8a-64f1b9c8e9a3"),
                ChapterId = new Guid("a5f2d8d6-7a9c-4e7c-8d4e-58a9b1c8c2d8"),
                Order = 2,
                Title = "Phân tích kết quả bài kiểm tra",
                Description = "Trong bài học này, các em sẽ phân tích kết quả bài kiểm tra tổng hợp. Các em sẽ học cách đọc và hiểu các phản hồi, nhận diện các điểm mạnh và điểm cần cải thiện. Bài học sẽ bao gồm các hoạt động phân tích chi tiết và các bài tập giúp các em hiểu rõ hơn về cách cải thiện kỹ năng của mình."
            });

            await _context.Topics.AddAsync(new Topic
            {
                Id = new Guid("e8f3a9b7-1a5d-45c5-bc4a-2f6e9d4c7b8c"),
                ChapterId = new Guid("a5f2d8d6-7a9c-4e7c-8d4e-58a9b1c8c2d8"),
                Order = 3,
                Title = "Lời khuyên để nâng cao kỹ năng",
                Description = "Bài học này sẽ cung cấp các lời khuyên và chiến lược để các em nâng cao kỹ năng về khối lượng và dung tích. Các em sẽ học các phương pháp học tập hiệu quả, cách thực hành thêm để cải thiện kỹ năng, và các nguồn tài liệu bổ sung để tiếp tục học hỏi. Bài học sẽ giúp các em tự tin hơn trong việc áp dụng kiến thức và nâng cao khả năng giải quyết vấn đề."
            });

            _context.SaveChanges();
            // --------------
            // TeachingSlot table
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("3efbbaca-4aa1-45f2-98a0-12fbc2399185"),
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 1,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("50199a05-0c0c-45ef-8b98-570e592fc903"),
                StartTime = "2 pm",
                EndTime = "4 pm",
                DayIndex = 2,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("811c17cb-527e-4658-9db4-454fdeeca3ef"),
                StartTime = "8 am",
                EndTime = "10 am",
                DayIndex = 3,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("ec198adf-2a3e-42d9-af87-37ee9a819f99"),
                StartTime = "10 am",
                EndTime = "12 am",
                DayIndex = 4,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("0471144b-8ed3-4e78-b032-d5ca3c5ddfa5"),
                StartTime = "1 pm",
                EndTime = "3 pm",
                DayIndex = 5,
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
            });
            //--------------
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("c608dd61-9076-4181-9786-6c6b211b0bbd"),
                StartTime = "3 pm",
                EndTime = "5 pm",
                DayIndex = 2,
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
                DayIndex = 6,
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
            });
            //------
            //--------------
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("48bdd659-d67a-4c66-ae75-f76fdf68dbcc"),
                StartTime = "7 am",
                EndTime = "10 am",
                DayIndex = 1,
                CourseId = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("fd332db2-1d09-45f7-8bae-2ec6bad1106e"),
                StartTime = "7 am",
                EndTime = "10 am",
                DayIndex = 3,
                CourseId = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("1e83df4c-5d28-4398-8e1d-c48a9391270e"),
                StartTime = "7 am",
                EndTime = "10 am",
                DayIndex = 5,
                CourseId = new Guid("1a1de7fd-d3c5-4bb3-8c51-fb3b02c44f16"),
            });
            //------
            //--------------
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("dd2d17d0-70d4-46da-a148-4e3a2aa4cba2"),
                StartTime = "2 pm",
                EndTime = "4 pm",
                DayIndex = 1,
                CourseId = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("aeb43bb4-bc8f-496c-a77d-6d1a1d34b098"),
                StartTime = "2 pm",
                EndTime = "4 pm",
                DayIndex = 2,
                CourseId = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("95c00c79-35a8-436a-b878-aa3d4bb1c4af"),
                StartTime = "2 pm",
                EndTime = "4 pm",
                DayIndex = 3,
                CourseId = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("306be03a-041e-478e-99d5-52e3480561d9"),
                StartTime = "2 pm",
                EndTime = "4 pm",
                DayIndex = 4,
                CourseId = new Guid("bd186368-c35a-4f16-9214-c1acdbfce054"),
            });
            //------
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("60913325-34df-4a96-956c-2a32b0fde16d"),
                StartTime = "2 pm",
                EndTime = "3 pm",
                DayIndex = 1,
                CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("b6459368-0bc0-4632-9e73-c1009c5832c8"),
                StartTime = "2 pm",
                EndTime = "3 pm",
                DayIndex = 2,
                CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("ce616e03-1b16-4920-8fed-822f3e274ad6"),
                StartTime = "2 pm",
                EndTime = "3 pm",
                DayIndex = 3,
                CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
            });
            //------
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("a3cc3e31-c26c-4953-838b-3f457946a026"),
                StartTime = "5 pm",
                EndTime = "7 pm",
                DayIndex = 2,
                CourseId = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("b0c5f33a-7654-4bc0-9c02-e4ab526cfebb"),
                StartTime = "5 pm",
                EndTime = "7 pm",
                DayIndex = 4,
                CourseId = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
            });
            await _context.TeachingSlots.AddAsync(new TeachingSlot
            {
                Id = new Guid("22f8f7ff-ab4d-4047-a237-c77d495c7d0f"),
                StartTime = "5 pm",
                EndTime = "7 pm",
                DayIndex = 6,
                CourseId = new Guid("d96fa6e2-ba64-4639-b0bb-d95c4f6b40d1"),
            });
            _context.SaveChanges();

        }
        public async static Task Seed_ChapterGame_Async(ApplicationDbContext _context)
        {
            await _context.ChapterGames.AddAsync(new ChapterGame
            {
                Id = new Guid("85a5ae0b-af59-4a4b-a48a-beee2a7b485b"),
                GameId = new Guid("ead13199-827d-4c48-5d08-08dcafad932c"),
                ChapterId = new Guid("2110f3bc-0170-4bbc-a3be-09823e310e43")
            });
            await _context.ChapterGames.AddAsync(new ChapterGame
            {
                Id = new Guid("1a61520b-7ee4-473c-961d-96dbb905b881"),
                GameId = new Guid("49299e7c-fa16-45fd-84e4-1a725c118a9f"),
                ChapterId = new Guid("3f15e8a2-247e-4d79-85f4-d46a73f7782b")
            });
            await _context.ChapterGames.AddAsync(new ChapterGame
            {
                Id = new Guid("4cff627d-4336-49f3-ba9e-4b41302164e5"),
                GameId = new Guid("134a1896-37a0-481c-76b8-08dcb1f69dd7"),
                ChapterId = new Guid("17f90b49-5e56-4b8a-9e7d-c75bdb6d8c37")
            });
            await _context.ChapterGames.AddAsync(new ChapterGame
            {
                Id = new Guid("a7b438e6-9457-4f1d-922c-85fb87d56381"),
                GameId = new Guid("3e2e9eee-07bb-4548-e2fa-08dcb0b903bd"),
                ChapterId = new Guid("11dabe72-3157-49f1-9d2e-6386a1f2b19c")
            });
            await _context.ChapterGames.AddAsync(new ChapterGame
            {
                Id = new Guid("95721ca2-44a8-4c70-8198-80a3ea62172b"),
                GameId = new Guid("c296495f-342e-4fd6-5d09-08dcafad932c"),
                ChapterId = new Guid("11dabe72-3157-49f1-9d2e-6386a1f2b19c")
            });
        }

        public async static Task Seed_Session_Enrollment_Participant_Teachable_Async(ApplicationDbContext _context)
        {
            // Session table
           /* await _context.Sessions.AddAsync(new Session
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
            });*/
            // -----------
            // Enrollment table
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("091e0476-0b32-412b-9437-e3990a6aa529"),
                CourseId = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b0",
                Status = Domain.Enums.EnrollmentStatus.OnGoing,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("877ddfd8-2ec2-445c-aeaf-1a51a6a40cd5"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b1",
                Status = Domain.Enums.EnrollmentStatus.OnGoing,
            });
            await _context.Enrollments.AddAsync(new Enrollment
            {
                Id = new Guid("d0d67119-944a-4ac7-a6a9-e888f50bf05c"),
                CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                ApplicationUserId = "954b8b1b-1b5f-42f6-9e27-4aa65cc7e7b2",
                Status = Domain.Enums.EnrollmentStatus.OnGoing,
            });
            // -----------
            // Participant table
           /* await _context.Participants.AddAsync(new Participant
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
            });*/
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
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea43",
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("ff7a0187-c8a6-478b-aa1e-91d2e6867111"),
                CourseId = new Guid("555a0815-d0b8-4975-8e1c-245d7acbab45"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea44",
            });
            await _context.Teachables.AddAsync(new Teachable
            {
                Id = new Guid("a1c89cfb-12e7-4c28-aa2d-fe4a684c463a"),
                CourseId = new Guid("471519c0-673d-40c0-b094-2014fe96848d"),
                ApplicationUserId = "8e02b95e-6491-4eaf-a75a-06dae6e1ea44",
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

            //Guid questionLevel = questionLevels.ElementAt(random.Next(questionLevels.Count)).Key;
            //Guid topicId = topics[random.Next(topics.Count)];

            var questionsAndAnswers = new List<(string question, List<(string answer, bool isCorrect)>)>
            {
                ("Kết quả của 7 + 2 là ", new List<(string, bool)>
                {
                    ("9", true), ("8", false), ("10", false), ("7", false)
                }),
                ("Kết quả của 6 - 3 là ", new List<(string, bool)>
                {
                    ("3", true), ("4", false), ("2", false), ("1", false)
                }),
                ("Kết quả của 4 + 4 là ", new List<(string, bool)>
                {
                    ("8", true), ("7", false), ("6", false), ("9", false)
                }),
                ("Kết quả của 5 + 3 là ", new List<(string, bool)>
                {
                    ("8", true), ("7", false), ("6", false), ("9", false)
                }),
                ("Kết quả của 9 - 2 là ", new List<(string, bool)>
                {
                    ("7", true), ("8", false), ("6", false), ("5", false)
                }),
                ("Kết quả của 10 - 5 là ", new List<(string, bool)>
                {
                    ("5", true), ("6", false), ("4", false), ("3", false)
                }),
                ("Kết quả của 8 + 1 là ", new List<(string, bool)>
                {
                    ("9", true), ("7", false), ("8", false), ("10", false)
                }),
                ("Kết quả của 2 + 6 là ", new List<(string, bool)>
                {
                    ("8", true), ("7", false), ("6", false), ("5", false)
                }),
                ("Kết quả của 4 + 3 là ", new List<(string, bool)>
                {
                    ("7", true), ("6", false), ("8", false), ("5", false)
                }),
                ("Kết quả của 10 - 7 là ", new List<(string, bool)>
                {
                    ("3", true), ("4", false), ("5", false), ("2", false)
                })
            };
            foreach (var (question, answers) in questionsAndAnswers)
            {
                var newQuestion = new Question
                {
                    Id = Guid.NewGuid(),
                    Content = question,
                    TopicId = topics[random.Next(topics.Count)],
                    QuestionLevelId = questionLevels.ElementAt(random.Next(2)).Key
                };
                await _context.Questions.AddAsync(newQuestion);

                foreach (var (answerContent, isCorrect) in answers)
                {
                    await _context.QuestionAnswers.AddAsync(new QuestionAnswer
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = newQuestion.Id,
                        Content = answerContent,
                        IsCorrect = isCorrect
                    });
                }
            }
            await _context.SaveChangesAsync();

            var questionsAndAnswers2 = new List<(string question, List<(string answer, bool isCorrect)>)>
            {
                ("Có bao nhiêu số có một chữ số:", new List<(string, bool)>
                {
                    ("10", false), ("9", true), ("8", false), ("90", false)
                }),
                ("Số liền trước số lớn nhất có một chữ số là số:", new List<(string, bool)>
                {
                    ("8", true), ("9", false), ("10", false), ("11", false)
                }),
                ("Số liền sau số lớn nhất có hai chữ số là số:", new List<(string, bool)>
                {
                    ("10", false), ("9", false), ("99", false), ("100", true)
                }),
                ("Số ở giữa số 25 và 27 là số:", new List<(string, bool)>
                {
                    ("28", false), ("24", false), ("26", true), ("29", false)
                }),
                ("Kết quả của phép tính 56 + 13 – 30 =…..", new List<(string, bool)>
                {
                    ("29", false), ("39", true), ("49", false), ("59", false)
                }),
                ("Số điền vào chỗ chấm trong phép tính ……….+15 – 20 = 37 là:", new List<(string, bool)>
                {
                    ("37", false), ("40", false), ("42", true), ("45", false)
                }),
                ("Nhà bà có tất cả 64 quả bưởi và na, trong đó số quả na là 24, vậy số quả bưởi là:", new List<(string, bool)>
                {
                    ("88 quả", false), ("40 quả", true), ("24 quả", false), ("20 quả", false)
                }),
                ("Số 45 là số liền sau số:", new List<(string, bool)>
                {
                    ("40", false), ("44", true), ("46", false), ("50", false)
                }),
                ("Hà có 35 lá cờ, Hà cho An 5 lá cờ và cho Lan 10 lá cờ, số lá cờ Hà còn lại:", new List<(string, bool)>
                {
                    ("30 lá", false), ("25 lá", true), ("20 lá", false), ("15 lá", false)
                }),
                ("Số liền sau số bé nhất có hai chữ số là:", new List<(string, bool)>
                {
                    ("9", false), ("10", false), ("11", true), ("12", false)
                }),
                ("Dãy số nào sau đây được xếp theo thứ tự từ bé đến lớn:", new List<(string, bool)>
                {
                    ("95; 83; 65; 52; 20", false), ("25; 30; 42; 86; 60", false), ("24; 32; 65; 82; 90", true), ("12; 15; 42; 52; 25", false)
                }),
                ("Hình tam giác là hình có:", new List<(string, bool)>
                {
                    ("2 cạnh", false), ("3 cạnh", true), ("4 cạnh", false), ("5 cạnh", false)
                }),
                ("Hôm nay là thứ năm ngày 8 thì hôm kia là ngày:", new List<(string, bool)>
                {
                    ("Thứ bảy ngày 10", false), ("Thứ ba ngày 10", false), ("Thứ ba ngày 6", true), ("Thứ tư ngày 7", false)
                }),
                ("Đoạn thẳng AB dài 18 cm, đoạn thẳng BC dài 25 cm, vậy đoạn thẳng BC ngắn hơn đoạn thẳng AB:", new List<(string, bool)>
                {
                    ("Đúng", false), ("Sai", true), ("Không chắc chắn", false), ("Cả hai đều sai", false)
                }),
                ("Có tất cả bao nhiêu số tròn chục có hai chữ số:", new List<(string, bool)>
                {
                    ("9", false), ("10", true), ("90", false), ("100", false)
                }),
                ("Số 65 là số ở giữa của:", new List<(string, bool)>
                {
                    ("60 và 65", false), ("64 và 66", true), ("65 và 70", false), ("61 và 62", false)
                }),
                ("Số tròn chục ở giữa số 35 và 45 là:", new List<(string, bool)>
                {
                    ("30", false), ("40", true), ("50", false), ("20", false)
                }),
                ("Số liền trước số bé nhất có hai chữ số là số:", new List<(string, bool)>
                {
                    ("11", false), ("10", false), ("9", true), ("8", false)
                }),
                ("Dãy số nào sau đây xếp theo thứ tự từ lớn đến bé:", new List<(string, bool)>
                {
                    ("90; 95; 80; 35; 65", false), ("95; 80; 62; 50; 20", true), ("20; 50; 62; 80; 95", false), ("55; 23; 35; 20; 10", false)
                }),
                ("Số lớn nhất có hai chữ số là:", new List<(string, bool)>
                {
                    ("98", false), ("99", true), ("100", false), ("10", false)
                }),
                ("Kết quả nào của phép tính 55 – 42 +22 = ………bé hơn số nào:", new List<(string, bool)>
                {
                    ("30", false), ("35", false), ("40", true), ("45", false)
                }),
                ("Đoạn thẳng BC dài 14cm, đoạn thẳng CD dài hơn đoạn thẳng BC 3cm, vậy đoạn thẳng CD dài là:", new List<(string, bool)>
                {
                    ("11", false), ("11cm", false), ("17", false), ("17cm", true)
                }),
                ("Số mà có số liền trước là số 20 là:", new List<(string, bool)>
                {
                    ("18", false), ("19", true), ("21", false), ("22", false)
                }),
                ("Năm nay anh 10 tuổi, anh hơn em 4 tuổi, vậy tuổi của em là:", new List<(string, bool)>
                {
                    ("14 tuổi", false), ("4 tuổi", false), ("6 tuổi", true), ("8 tuổi", false)
                }),
                ("Lớp 1A có 55 bạn, trong đó có 30 bạn nam, vậy số bạn nữ của lớp 1A là:", new List<(string, bool)>
                {
                    ("85 bạn", false), ("25 bạn", true), ("20 bạn", false), ("30 bạn", false)
                }),
                ("Kết quả của phép tính 85 – 24 – 40 =……", new List<(string, bool)>
                {
                    ("31", true), ("21", false), ("11", false), ("10", false)
                }),
                ("Nam có 12 bút chì, Thành có 13 bút chì, Văn có 14 bút chì. Vậy số bút chì có tất cả là:", new List<(string, bool)>
                {
                    ("29 bút", false), ("39 bút", true), ("49 bút", false), ("59 bút", false)
                }),
                ("Số lớn hơn 62 và nhỏ hơn 64 là số:", new List<(string, bool)>
                {
                    ("60", false), ("61", false), ("62", false), ("63", true)
                }),
                ("Một tuần có:", new List<(string, bool)>
                {
                    ("5 ngày", false), ("6 ngày", false), ("7 ngày", true), ("8 ngày", false)
                }),
                ("Số thích hợp điền vào chỗ chấm của phép tính ……….- 12 - 35 = 21 là:", new List<(string, bool)>
                {
                    ("88", false), ("78", true), ("68", false), ("58", false)
                })
            };

            foreach (var (question, answers) in questionsAndAnswers2)
            {
                var newQuestion = new Question
                {
                    Id = Guid.NewGuid(),
                    Content = question,
                    TopicId = topics[random.Next(topics.Count)],
                    QuestionLevelId = questionLevels.ElementAt(random.Next(2,questionLevels.Count)).Key
                };
                await _context.Questions.AddAsync(newQuestion);

                foreach (var (answerContent, isCorrect) in answers)
                {
                    await _context.QuestionAnswers.AddAsync(new QuestionAnswer
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = newQuestion.Id,
                        Content = answerContent,
                        IsCorrect = isCorrect
                    });
                }
            }
            await _context.SaveChangesAsync();

            var questionsAndAnswers3 = new List<(string question, List<(string answer, bool isCorrect)>)>
            {
                ("Số nhỏ nhất có hai chữ số là:", new List<(string, bool)>
                {
                    ("10", true), ("11", false), ("9", false), ("12", false)
                }),
                ("5 x 3 = ?", new List<(string, bool)>
                {
                    ("15", true), ("20", false), ("10", false), ("5", false)
                }),
                ("Kết quả của phép tính 9 - 4 là:", new List<(string, bool)>
                {
                    ("5", true), ("4", false), ("3", false), ("6", false)
                }),
                ("Hình vuông có bao nhiêu cạnh:", new List<(string, bool)>
                {
                    ("3", false), ("4", true), ("5", false), ("6", false)
                }),
                ("Trong các số sau, số nào là số chẵn:", new List<(string, bool)>
                {
                    ("7", false), ("4", true), ("9", false), ("5", false)
                }),
                ("Kết quả của phép nhân 2 x 6 là:", new List<(string, bool)>
                {
                    ("10", false), ("12", true), ("8", false), ("14", false)
                }),
                ("Số nào lớn hơn 18 nhưng nhỏ hơn 22:", new List<(string, bool)>
                {
                    ("19", true), ("18", false), ("20", false), ("22", false)
                }),
                ("3 x 4 = ?", new List<(string, bool)>
                {
                    ("7", false), ("12", true), ("9", false), ("10", false)
                }),
                ("Kết quả của phép tính 15 + 5 là:", new List<(string, bool)>
                {
                    ("20", true), ("21", false), ("19", false), ("18", false)
                }),
                ("Số 6 + số nào bằng 10:", new List<(string, bool)>
                {
                    ("4", true), ("5", false), ("3", false), ("6", false)
                }),
                ("Có bao nhiêu số lẻ từ 1 đến 9:", new List<(string, bool)>
                {
                    ("4", false), ("5", true), ("3", false), ("6", false)
                }),
                ("Số liền sau số 12 là:", new List<(string, bool)>
                {
                    ("11", false), ("13", true), ("14", false), ("10", false)
                }),
                ("Kết quả của phép chia 20 ÷ 5 là:", new List<(string, bool)>
                {
                    ("4", true), ("5", false), ("3", false), ("6", false)
                }),
                ("1 tuần có mấy ngày:", new List<(string, bool)>
                {
                    ("5 ngày", false), ("6 ngày", false), ("7 ngày", true), ("8 ngày", false)
                }),
                ("Hình tam giác có bao nhiêu cạnh:", new List<(string, bool)>
                {
                    ("2", false), ("3", true), ("4", false), ("5", false)
                }),
                ("Số lớn nhất có một chữ số là:", new List<(string, bool)>
                {
                    ("9", true), ("8", false), ("10", false), ("7", false)
                }),
                ("10 - 2 = ?", new List<(string, bool)>
                {
                    ("8", true), ("7", false), ("6", false), ("9", false)
                }),
                ("Số nào là số lẻ:", new List<(string, bool)>
                {
                    ("4", false), ("8", false), ("5", true), ("2", false)
                }),
                ("Kết quả của phép tính 6 + 4 là:", new List<(string, bool)>
                {
                    ("10", true), ("11", false), ("9", false), ("8", false)
                }),
                ("Số nào là số nguyên tố:", new List<(string, bool)>
                {
                    ("4", false), ("9", false), ("7", true), ("6", false)
                }),
                ("5 x 5 = ?", new List<(string, bool)>
                {
                    ("25", true), ("20", false), ("15", false), ("10", false)
                }),
                ("Số nhỏ nhất có một chữ số là:", new List<(string, bool)>
                {
                    ("0", true), ("1", false), ("2", false), ("3", false)
                }),
                ("Kết quả của phép chia 16 ÷ 4 là:", new List<(string, bool)>
                {
                    ("4", true), ("3", false), ("5", false), ("6", false)
                }),
                ("Hình chữ nhật có bao nhiêu góc vuông:", new List<(string, bool)>
                {
                    ("2", false), ("3", false), ("4", true), ("5", false)
                }),
                ("Kết quả của phép nhân 7 x 2 là:", new List<(string, bool)>
                {
                    ("14", true), ("12", false), ("10", false), ("16", false)
                }),
                ("Số lớn hơn 33 nhưng nhỏ hơn 35 là:", new List<(string, bool)>
                {
                    ("32", false), ("33", false), ("34", true), ("35", false)
                }),
                ("Kết quả của phép trừ 12 - 7 là:", new List<(string, bool)>
                {
                    ("5", true), ("4", false), ("3", false), ("2", false)
                }),
                ("1 ngày có bao nhiêu giờ:", new List<(string, bool)>
                {
                    ("12 giờ", false), ("24 giờ", true), ("36 giờ", false), ("48 giờ", false)
                }),
                ("Số nào nhỏ hơn 15 nhưng lớn hơn 10:", new List<(string, bool)>
                {
                    ("12", true), ("15", false), ("10", false), ("9", false)
                }),
                ("Kết quả của phép cộng 8 + 7 là:", new List<(string, bool)>
                {
                    ("15", true), ("14", false), ("13", false), ("12", false)
                })
            };
            foreach (var (question, answers) in questionsAndAnswers3)
            {
                var newQuestion = new Question
                {
                    Id = Guid.NewGuid(),
                    Content = question,
                    TopicId = topics[random.Next(topics.Count)],
                    QuestionLevelId = questionLevels.ElementAt(random.Next(questionLevels.Count)).Key
                };
                await _context.Questions.AddAsync(newQuestion);

                foreach (var (answerContent, isCorrect) in answers)
                {
                    await _context.QuestionAnswers.AddAsync(new QuestionAnswer
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = newQuestion.Id,
                        Content = answerContent,
                        IsCorrect = isCorrect
                    });
                }
            }
            await _context.SaveChangesAsync();

        }

        public async static Task Seed_Worksheet_Async(ApplicationDbContext _context)
        {
            // WorksheetTemplate table
            await _context.WorksheetTemplates.AddAsync(new WorksheetTemplate
            {
                Id = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                Title = "Mẫu kiểm tra khó",
                CourseId = new Guid("CEAF0F02-168D-4F69-975F-14A61D492886"),
                Classification = 0
            });
            // LevelTemplateRelation table
            await _context.LevelTemplateRelations.AddAsync(new LevelTemplateRelation
            {
                Id = new Guid("a054bf47-6783-4cbe-ac05-02b5b22b84ec"),
                QuestionLevelId = new Guid("871D2DE9-CFCA-4ED0-A9A9-658639D664DF"), //Trung bình
                WorksheetTemplateId = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                NoQuestions = 2
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
                Id = new Guid("340161ca-a711-4d41-bd28-464b26d7511b"),
                QuestionLevelId = new Guid("ca44a423-5b69-4953-8e94-8e4b771bef19"), //cuc Khó
                WorksheetTemplateId = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                NoQuestions = 5
            });
            await _context.LevelTemplateRelations.AddAsync(new LevelTemplateRelation
            {
                Id = new Guid("de30e2e6-a203-494d-a8ed-8a279f36f25c"),
                QuestionLevelId = new Guid("26FB0C3C-2F79-4940-AC2C-6EF7BA427D92"), //Dễ
                WorksheetTemplateId = new Guid("721f378c-69fd-4f9d-ba93-38fa2db2044f"),
                NoQuestions = 2
            });
            // WorksheetTemplate table
            await _context.WorksheetTemplates.AddAsync(new WorksheetTemplate
            {
                Id = new Guid("3ea235ca-52e0-470e-83b7-0c3e8126a7d2"),
                Title = "Mẫu kiểm tra dễ",
                CourseId = new Guid("CEAF0F02-168D-4F69-975F-14A61D492886"),
                Classification = 0
            });
            // LevelTemplateRelation table
            await _context.LevelTemplateRelations.AddAsync(new LevelTemplateRelation
            {
                Id = new Guid("48f561e1-bede-428a-8785-c32c90dff954"),
                QuestionLevelId = new Guid("871D2DE9-CFCA-4ED0-A9A9-658639D664DF"), //Trung bình
                WorksheetTemplateId = new Guid("3ea235ca-52e0-470e-83b7-0c3e8126a7d2"),
                NoQuestions = 3
            });
            await _context.LevelTemplateRelations.AddAsync(new LevelTemplateRelation
            {
                Id = new Guid("8cbf9190-0794-4f7e-bc6e-9ce7e946f9f1"),
                QuestionLevelId = new Guid("8F45DAB4-C1F8-4528-8CA4-BA5F682F847D"), //Khó
                WorksheetTemplateId = new Guid("3ea235ca-52e0-470e-83b7-0c3e8126a7d2"),
                NoQuestions = 1
            });
            await _context.LevelTemplateRelations.AddAsync(new LevelTemplateRelation
            {
                Id = new Guid("de30280a-346e-433f-89b4-1b932b618ee6"),
                QuestionLevelId = new Guid("26FB0C3C-2F79-4940-AC2C-6EF7BA427D92"), //Dễ
                WorksheetTemplateId = new Guid("3ea235ca-52e0-470e-83b7-0c3e8126a7d2"),
                NoQuestions = 6
            });
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
