using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using Hospital_Manager.ViewModels.Patient;
using HospitalManagerServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Manager.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService patientService;
        private readonly HospitalDbContext context;

        public PatientController(
            IPatientService patientService,
            HospitalDbContext context)
        {
            this.patientService = patientService;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await patientService.GetAllPatientsAsync();

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
                    DoctorName = string.Join(", ",
                        patient.DoctorsPatients.Select(x =>
                            x.Doctor.FirstName + " " + x.Doctor.LastName))
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var patient = await patientService.GetPatientWithDoctorsAsync(id);

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
                DoctorName = string.Join(", ",
                    patient.DoctorsPatients.Select(dp =>
                        dp.Doctor.FirstName + " " + dp.Doctor.LastName))
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDoctors();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                foreach (var doctorid in model.DosctorIds)
                {
                    patient.DoctorsPatients.Add(new DoctorPatient
                    {
                        DoctorId = doctorid
                    });
                }

                await patientService.AddPatientAsync(patient);

                return RedirectToAction(nameof(Index));
            }

            await LoadDoctors(model.DosctorIds);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var patient = await patientService.GetPatientWithDoctorsAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            var model = new PatientEditViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DosctorIds = patient.DoctorsPatients
                    .Select(dp => dp.DoctorId)
                    .ToList()
            };

            await LoadDoctors(model.DosctorIds);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PatientEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var patient = await patientService.GetPatientWithDoctorsAsync(id);

                if (patient == null)
                {
                    return NotFound();
                }

                patient.FirstName = model.FirstName;
                patient.LastName = model.LastName;
                patient.Email = model.Email;
                patient.PhoneNumber = model.PhoneNumber;

                await patientService.UpdatePatientAsync(
                    patient,
                    model.DosctorIds);

                return RedirectToAction(nameof(Index));
            }

            await LoadDoctors(model.DosctorIds);

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var patient = await patientService.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            var model = new PatientDeleteViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await patientService.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            await patientService.DeletePatientAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDoctors(List<int> selectedDoctors = null)
        {
            var doctors = await context.Doctors
                .Select(d => new
                {
                    d.Id,
                    FullName = d.FirstName + " " + d.LastName
                })
                .ToListAsync();

            ViewBag.Doctors = new MultiSelectList(
                doctors,
                "Id",
                "FullName",
                selectedDoctors);
        }
    }
}
