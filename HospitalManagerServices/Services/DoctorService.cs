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
    public class DoctorService:IDoctorService
    {
        private readonly HospitalDbContext new_context;

        public DoctorService(HospitalDbContext context)
        {
            new_context = context;
        }
        public async Task AddDoctorAsync(Doctor doctor)
        {
            new_context.Doctors.Add(doctor);
            await new_context.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await new_context.Doctors.Include(x=>x.DoctorsPatients).FirstOrDefaultAsync(x=>x.Id==id);
            if (doctor != null)
            {
                new_context.DoctorsPatients.RemoveRange(doctor.DoctorsPatients);
                new_context.Doctors.Remove(doctor);
                await new_context.SaveChangesAsync();
            }
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await new_context.Doctors.Include(x=>x.Hospital).ToListAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await new_context.Doctors.FindAsync(id);
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            await new_context.SaveChangesAsync();
        }
        public async Task<Doctor?> GetDoctorDetailsAsync(int id)
        {
            return await new_context.Doctors.Include(x=>x.Hospital).Include(x=>x.DoctorsPatients).ThenInclude(x=>x.Patient).FirstOrDefaultAsync(x=>x.Id==id);
        }

       
    }
}
