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
        }
    }
}
