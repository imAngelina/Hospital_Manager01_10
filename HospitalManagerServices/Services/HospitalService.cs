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

        public async Task AddHospitalAsync(Hospital hospital)
        {
            new_context.Hospitals.Add(hospital);
            await new_context.SaveChangesAsync();
        }

        public async Task DeleteHospitalAsync(int id)
        {
            var hospital = await new_context.Hospitals.FindAsync(id);
            if(hospital == null)
            {
                hospital.IsDeleted = true;
                await new_context.SaveChangesAsync();
            }
        }

        public async Task<List<Hospital>> GetAllHospitalsAsync()
        {
            return await new_context.Hospitals.Where(x=>!x.IsDeleted).ToListAsync();
        }

        public async Task<Hospital?> GetHospitalByIdAsync(int id)
        {
            return await new_context.Hospitals.FindAsync(id);
        }

        public async Task UpdateHospitalAsync(Hospital hospital)
        {
            await new_context.SaveChangesAsync();
        }
    }
}
