﻿using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        Warehouse? GetByName(string name);
    }
}
