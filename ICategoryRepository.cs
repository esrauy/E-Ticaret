﻿using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.DataAccessLayer.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {      
        List<Category> GetPopularCategories();
        Category GetByIdWidthProducts(int categoryId);
        void DeleteFromCategory (int productId, int categoryId);
    }
}
