﻿using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Product
{
    public interface IProductDF
    {
        Task<ProductDTO> GetProductAsync(string asin);
        Task<PagedList<ProductDTO>> GetProductsAsync(GetProductsQuery query);
    }
}
