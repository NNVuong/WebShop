using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedObjects.Commons;
using SharedObjects.Models;
using SharedObjects.ValueObjects;
using SharedObjects.ViewModels;

namespace Service.Interfaces
{
    public interface ISalesService
    {
        Task<List<VSales>> GetAll();
    }
}