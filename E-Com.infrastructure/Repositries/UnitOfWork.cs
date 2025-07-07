using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Com.Core.interfaces;
using E_Com.infrastructure.Data;

namespace E_Com.infrastructure.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepositry CategoryRepositry{ get; }

        public IPhotoRepositry PhotoRepositry { get; }

        public IProductRepositry ProductRepositry { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CategoryRepositry =new CategoryRepositry(_context);
            PhotoRepositry =new PhotoRepositry(_context);
            ProductRepositry =new ProductRepositry(_context);
        }
    }
}
