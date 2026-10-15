using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using HospitalManagerServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagerServices.Services
{
    public class HospitalService:IHospitalService
    {
        private readonly HospitalDbContext new_context;

        public HospitalService(HospitalDbContext context)
        {
            new_context=context;
        }

        public Task AddHospitalAsync(Hospital hospital)
        {
            throw new NotImplementedException();
        }

        public Task DeleteHospitalAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Hospital>> GetAllHospitalsAsync()
        {
            return await new_context.Hospitals.Where(x=>!x.IsDeleted).ToListAsync();
        }

        public Task<Hospital?> GetHospitalByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateHospitalAsync(Hospital hospital)
        {
            throw new NotImplementedException();
        }
    }
}
