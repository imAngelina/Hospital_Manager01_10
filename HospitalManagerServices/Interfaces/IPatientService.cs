using Data_Hospital_Manager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagerServices.Interfaces
{
    public interface IPatientService
    {
        Task<List<Patient>> GetAllPatientsAsync();
        Task<Patient?> GetPatientByIdAsync(int id);
        Task<Patient?> GetPatientWithDoctorsAsync(int id);
        Task AddPatientAsync(Hospital hospital);
        Task UpdatePatientAsync(Hospital hospital, List<int> doctorIds);
        Task DeletePatientAsync(int id);
    }
}
