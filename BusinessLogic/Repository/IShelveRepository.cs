﻿using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IShelveRepository:IRepository<Shelve>
    {
        Shelve? GetByName(string name);
    }
}
