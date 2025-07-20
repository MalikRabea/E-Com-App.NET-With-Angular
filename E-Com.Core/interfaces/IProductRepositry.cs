using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Com.Core.DTO;
using E_Com.Core.Entites.Product;

namespace E_Com.Core.interfaces
{
    public interface IProductRepositry : IGenericRepositry<Product>
    {
        //for futuer
        Task<bool> AddAsync(AddProductDTO productDTO);
        Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO );
        Task DeleteAsync(Product product );



    }
}
