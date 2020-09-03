﻿using System;
using System.Collections.Generic;
namespace Auriculoterapia.Api.Service
{
    public interface IService<T>
    {
        void Save(T entity);
        IEnumerable<T> FindAll();

    }
}
