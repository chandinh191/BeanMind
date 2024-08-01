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
        public async static Task SeedGamesAsync(DbContext context)
        {
            var jsonString = @"
        [
            {
                'name': 'Nông trại vui vẻ - phép cộng và phép trừ hai số',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fhappy_farm.png?alt=media&token=ed307113-421f-4d74-bea2-e4d45fd18b8f',
                'itemStoreJson': '
                   [
                    {
                        ""id"": ""item001"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_002.png?alt=media&token=99427f48-54f4-41ec-81a5-598aff948ffd"",
                        ""price"": 5
                    },
                    {
                        ""id"": ""item002"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_003.png?alt=media&token=3b3b3b3b-3b3b-3b3b-3b3b-3b3b3b3b3b3b"",
                        ""price"": 7
                    },
                    {
                        ""id"": ""item003"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_004.png?alt=media&token=4b4b4b4b-4b4b-4b4b-4b4b-4b4b4b4b4b4b"",
                        ""price"": 8
                    },
                    {
                        ""id"": ""item004"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_005.png?alt=media&token=5b5b5b5b-5b5b-5b5b-5b5b-5b5b5b5b5b5b"",
                        ""price"": 6
                    },
                    {
                        ""id"": ""item005"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_006.png?alt=media&token=6b6b6b6b-6b6b-6b6b-6b6b-6b6b6b6b6b6b"",
                        ""price"": 10
                    },
                    {
                        ""id"": ""item006"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_007.png?alt=media&token=7b7b7b7b-7b7b-7b7b-7b7b-7b7b7b7b7b7b"",
                        ""price"": 12
                    },
                    {
                        ""id"": ""item007"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_008.png?alt=media&token=8b8b8b8b-8b8b-8b8b-8b8b-8b8b8b8b8b8b"",
                        ""price"": 15
                    },
                    {
                        ""id"": ""item008"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_009.png?alt=media&token=9b9b9b9b-9b9b-9b9b-9b9b-9b9b9b9b9b9b"",
                        ""price"": 20
                    },
                    {
                        ""id"": ""item009"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_010.png?alt=media&token=0b0b0b0b-0b0b-0b0b-0b0b-0b0b0b0b0b0b"",
                        ""price"": 9
                    },
                    {
                        ""id"": ""item010"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_011.png?alt=media&token=1b1b1b1b-1b1b-1b1b-1b1b-1b1b1b1b1b1b"",
                        ""price"": 18
                    },
                    {
                        ""id"": ""item011"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_012.png?alt=media&token=2b2b2b2b-2b2b-2b2b-2b2b-2b2b2b2b2b2b"",
                        ""price"": 22
                    },
                    {
                        ""id"": ""item012"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_013.png?alt=media&token=3b3b3b3b-3b3b-3b3b-3b3b-3b3b3b3b3b3b"",
                        ""price"": 17
                    },
                    {
                        ""id"": ""item013"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_014.png?alt=media&token=4b4b4b4b-4b4b-4b4b-4b4b-4b4b4b4b4b4b"",
                        ""price"": 11
                    },
                    {
                        ""id"": ""item014"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_015.png?alt=media&token=5b5b5b5b-5b5b-5b5b-5b5b-5b5b5b5b5b5b"",
                        ""price"": 14
                    },
                    {
                        ""id"": ""item015"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_016.png?alt=media&token=6b6b6b6b-6b6b-6b6b-6b6b-6b6b6b6b6b6b"",
                        ""price"": 19
                    },
                    {
                        ""id"": ""item016"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_017.png?alt=media&token=7b7b7b7b-7b7b-7b7b-7b7b-7b7b7b7b7b7b"",
                        ""price"": 23
                    },
                    {
                        ""id"": ""item017"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_018.png?alt=media&token=8b8b8b8b-8b8b-8b8b-8b8b-8b8b8b8b8b8b"",
                        ""price"": 13
                    },
                    {
                        ""id"": ""item018"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_019.png?alt=media&token=9b9b9b9b-9b9b-9b9b-9b9b-9b9b9b9b9b9b"",
                        ""price"": 16
                    },
                    {
                        ""id"": ""item019"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_020.png?alt=media&token=0b0b0b0b-0b0b-0b0b-0b0b-0b0b0b0b0b0b"",
                        ""price"": 21
                    },
                    {
                        ""id"": ""item020"",
                        ""image"": ""https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/item_game_images%2Fitem_store_021.png?alt=media&token=1b1b1b1b-1b1b-1b1b-1b1b-1b1b1b1b1b1b"",
                        ""price"": 25
                    }
                ]
                ',
                'animalJson': '',
                'id': 'a65534d6-b34c-43d1-e2f6-08dcb0b903bd',
                'isDeleted': false
            },
            {
                'name': 'Nông trại vui vẻ - phép cộng trừ nhân chia hai số',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fhappy_farm.png?alt=media&token=ed307113-421f-4d74-bea2-e4d45fd18b8f',
                'itemStoreJson': '',
                'animalJson': '',
                'id': '9400fa00-e27d-40a1-e2f7-08dcb0b903bd',
                'isDeleted': false
            },
            {
                'name': 'Khám phá đại dương - phép cộng trừ hai số',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Focean_adventure.png?alt=media&token=d9ada505-0862-4a8b-afe6-6a11358561bc',
                'itemStoreJson': '',
                'animalJson': '',
                'id': 'd9db0faa-49e7-488e-e2f8-08dcb0b903bd',
                'isDeleted': false
            },
            {
                'name': 'Khám phá đại dương - phép cộng trừ nhân chia hai số',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Focean_adventure.png?alt=media&token=d9ada505-0862-4a8b-afe6-6a11358561bc',
                'itemStoreJson': '',
                'animalJson': '',
                'id': '6d69ec97-28c8-4c34-e2f9-08dcb0b903bd',
                'isDeleted': false
            },
            {
                'name': 'Sắp xếp số từ 1 tới 100',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fnumber_sort.png?alt=media&token=6124bb15-ff34-47fc-a0ad-143463324dd1',
                'itemStoreJson': '',
                'animalJson': '',
                'id': '3e2e9eee-07bb-4548-e2fa-08dcb0b903bd',
                'isDeleted': false
            },
            {
                'name': 'Sắp xếp số từ 1 tới 1000',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fnumber_sort.png?alt=media&token=6124bb15-ff34-47fc-a0ad-143463324dd1',
                'itemStoreJson': '',
                'animalJson': '',
                'id': '6011f3e5-d1fd-439c-e2fb-08dcb0b903bd',
                'isDeleted': false
            },
            {
                'name': 'Lớn hơn, bé hơn hay bằng nhau',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fodd_and_even.png?alt=media&token=30686705-4ae5-433a-8af7-16a30938461c',
                'itemStoreJson': '',
                'animalJson': '',
                'id': 'b2b05dc0-d4d4-4dfb-e2fc-08dcb0b903bd',
                'isDeleted': false
            },
            {
                'name': 'Khám phá đại dương - học đếm từ 0 đến 5',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Focean_adventure.png?alt=media&token=d9ada505-0862-4a8b-afe6-6a11358561bc',
                'itemStoreJson': '',
                'animalJson': '',
                'id': '3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0',
                'isDeleted': false
            },
            {
                'name': 'Nông trại vui vẻ - học đếm từ 0 đến 5',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fhappy_farm.png?alt=media&token=ed307113-421f-4d74-bea2-e4d45fd18b8f',
                'itemStoreJson': '',
                'animalJson': '',
                'id': '49299e7c-fa16-45fd-84e4-1a725c118a9f',
                'isDeleted': false
            },
            {
                'name': 'Sắp xếp số từ 1 tới 10',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fnumber_sort.png?alt=media&token=6124bb15-ff34-47fc-a0ad-143463324dd1',
                'itemStoreJson': '',
                'animalJson': '',
                'id': 'ead13199-827d-4c48-5d08-08dcafad932c',
                'isDeleted': false
            },
            {
                'name': 'Trò chơi mua sắm',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fshopping.png?alt=media&token=6ba50614-1253-4b02-8906-e19979d9f614',
                'itemStoreJson': '',
                'animalJson': '',
                'id': 'c296495f-342e-4fd6-5d09-08dcafad932c',
                'isDeleted': false
            },
            {
                'name': 'Số lẻ và số chẵn',
                'description': 'Description Game',
                'image': 'https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/thumbnail_game_images%2Fodd_and_even.png?alt=media&token=30686705-4ae5-433a-8af7-16a30938461c',
                'itemStoreJson': '',
                'animalJson': '',
                'id': '59141c9e-7dd3-4c76-5d0a-08dcafad932c',
                'isDeleted': false
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
            }
            await context.SaveChangesAsync();
            //await SeedGameHistoryAsync(context);
        }
        public async static Task SeedGameHistoryAsync(ApplicationDbContext _context)
        {
            // GameHistories table
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("f9033f16-8f40-4cce-a687-ac1fac7712a7"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                Point = 10,
                Duration = 8
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("cb23cacd-7e73-4c32-a488-6ea58f1beacf"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Point = 14,
                Duration = 8
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("352a4f2a-13a7-4ac7-a92e-3cf900ac4425"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                Point = 95,
                Duration = 4
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("6c78454f-f667-4d4c-949a-d81aa082df44"),
                GameId = new Guid("3ae42c10-7dbe-4e71-a52c-c19c44e3c4a0"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Point = 34,
                Duration = 2
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("5d1addc2-56b0-4c8e-81ee-9049facac523"),
                GameId = new Guid("49299e7c-fa16-45fd-84e4-1a725c118a9f"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05a",
                Point = 34,
                Duration = 3
            });
            await _context.GameHistories.AddAsync(new GameHistory
            {
                Id = new Guid("2c8fb39d-1112-403e-8eb3-2252b21f3307"),
                GameId = new Guid("49299e7c-fa16-45fd-84e4-1a725c118a9f"),
                ApplicationUserId = "d7896275-9b79-4955-92f2-e1923b5fa05b",
                Point = 34,
                Duration = 1
            });
        }

        public async static Task SeedCourseChapterTopicAsync(ApplicationDbContext _context)
        {
            // Course table
            await _context.Courses.AddAsync(new Course
            {
                Id = new Guid("ceaf0f02-168d-4f69-975f-14a61d492886"),
                SubjectId = new Guid("14b76851-0f86-4dd2-a59c-ae45893c9578"),
                CourseLevelId = new Guid("b8fc90e5-a56f-4ac0-b6bb-cd3eea88d4a1"),
                ProgramTypeId = new Guid("8c368591-a7f0-4679-b059-31c22fa46c1c"),
                Title = "Đại số đại cương",
                Image = "https://static.vecteezy.com/system/resources/previews/013/400/591/non_2x/education-concept-with-cartoon-students-vector.jpg",
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
                Image = " https://static.vecteezy.com/system/resources/thumbnails/002/399/898/small_2x/education-concept-with-funny-characters-vector.jpg",
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
                Image = "https://c8.alamy.com/comp/M71DKY/vector-illustration-of-three-stick-kids-jumping-together-in-the-field-M71DKY.jpg",
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
                Image = "https://i.pinimg.com/736x/fa/3a/1a/fa3a1ac70a55ba27576e41d2335e253c.jpg",
                Description = "Tư duy logic là khả năng quan trọng giúp con người suy luận và giải quyết vấn đề một cách có cấu trúc và logic. Trong giáo dục, việc phát triển tư duy logic không chỉ là mục tiêu của quá trình học tập mà còn là công cụ quan trọng giúp học sinh phát triển kỹ năng suy nghĩ sâu sắc, phân tích thông tin và đưa ra nhận định có chất lượng. Thông qua việc thực hành giải quyết các bài toán phức tạp và tham gia vào các hoạt động nghiên cứu và phân tích, học sinh có cơ hội rèn luyện tư duy logic và áp dụng nó vào các tình huống thực tế, từ đó phát triển sự tự tin và thành công trong học tập và cuộc sống.",
                TotalSlot = 24,
            });
            // --------------
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
            // QuestionLevel table
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("26fb0c3c-2f79-4940-ac2c-6ef7ba427d92"),
                Title = "Dễ"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("871d2de9-cfca-4ed0-a9a9-658639d664df"),
                Title = "Trung bình"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("8abb4833-8443-4aab-b996-dc1eff84bd41"),
                Title = "Khá"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("8f45dab4-c1f8-4528-8ca4-ba5f682f847d"),
                Title = "Khó"
            });
            await _context.QuestionLevels.AddAsync(new QuestionLevel
            {
                Id = new Guid("ca44a423-5b69-4953-8e94-8e4b771bef19"),
                Title = "Cực khó"
            });
            // -----------
        }

        public async static Task SeedQuestionBankAsync(ApplicationDbContext _context)
        {
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
    }
}
