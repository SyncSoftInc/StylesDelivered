using MongoDB.Driver;
using SyncSoft.App.MongoDB;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.DTO.User;

namespace SyncSoft.StylesDelivered.MongoDB
{
    public class StylesDeliveredDB : MongoDatabase, IStylesDeliveredDB
    {
        public IMongoCollection<ProductItemDTO> ProductItems { get; private set; }
        public IMongoCollection<UserDTO> Users { get; private set; }

        public StylesDeliveredDB(string connStrOrName) : base(connStrOrName)
        {
        }

        protected override void Init()
        {
            ////////// ProductItems
            ProductItems = DB.GetCollection<ProductItemDTO>(nameof(ProductItems));
            // 索引
            ProductItems.Indexes.CreateOne(new CreateIndexModel<ProductItemDTO>(Builders<ProductItemDTO>.IndexKeys.Descending(x => x.CreatedOnUtc)));
            // ProductItemDTO
            ProductItems.Indexes.CreateOne(new CreateIndexModel<ProductItemDTO>(Builders<ProductItemDTO>.IndexKeys.Ascending(x => x.ItemNo), new CreateIndexOptions
            {
                Unique = true
            }));

            ////////// Users
            Users = DB.GetCollection<UserDTO>(nameof(Users));
            // 索引
            // ProductItemDTO
            Users.Indexes.CreateOne(new CreateIndexModel<UserDTO>(Builders<UserDTO>.IndexKeys.Ascending(x => x.ID), new CreateIndexOptions
            {
                Unique = true
            }));
        }
    }
}
