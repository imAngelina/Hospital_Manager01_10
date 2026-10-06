using Data_Hospital_Manager;
using Hospital_Manager.ViewModels.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Manager.Controllers
{
    public class PatientController : Controller
    {
        private readonly HospitalDbContext context;

        public PatientController(HospitalDbContext context)
        {
            context = context;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await context.Patients.Include(x=>x.DoctorsPatients).ThenInclude(x=>x.Doctor).ToListAsync();
            var model = new List<PatientIndexViewModel>();

            foreach (var patient in patients)
            {
                model.Add(new PatientIndexViewModel
                {
                    Id = patient.Id,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Email = patient.Email,
                    PhoneNumber = patient.PhoneNumber,
                    DoctorName = string.Join(", ", patient.DoctorsPatients.Select(x=>x.Doctor.FirstName+" "+x.Doctor.LastName))
                });
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var patient = await context.Patients.Include(p => p.DoctorsPatients).ThenInclude(dp => dp.Doctor).FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            var model = new PatientDetailsViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DoctorName = string.Join(", ", patient.DoctorsPatients.Select(dp => dp.Doctor.FirstName + " " + dp.Doctor.LastName))
            };
            return View(model);
        }
        private async Task LoadDoctors(List<int> selectedDoctors = null)
        {
            var doctors = await context.Doctors.Select(d=> new
            {
                d.Id,
                FullName = d.FirstName+" "+d.LastName
            }).ToListAsync();

            ViewBag.Doctors = new MultiSelectList(doctors, "Id","FullName", selectedDoctors);
        }
    }
}
