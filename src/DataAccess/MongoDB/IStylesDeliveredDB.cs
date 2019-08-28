using MongoDB.Driver;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.DTO.User;

namespace SyncSoft.StylesDelivered.MongoDB
{
    public interface IStylesDeliveredDB
    {
        IMongoCollection<ProductItemDTO> ProductItems { get; }
        IMongoCollection<UserDTO> Users { get; }
    }
}
