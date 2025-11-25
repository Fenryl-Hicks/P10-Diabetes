using Microsoft.AspNetCore.Mvc;
using P10.Frontend.Models;
using P10.Frontend.Services;

public class PatientController : Controller
{
    private readonly IPatientApiClient _patientApiClient;

    public PatientController(IPatientApiClient patientApiClient)
    {
        _patientApiClient = patientApiClient;
    }

    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");

        var patients = await _patientApiClient.GetPatientsAsync(token);
        return View(patients);
    }

    public async Task<IActionResult> Details(int id)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");

        var patient = await _patientApiClient.GetPatientByIdAsync(id, token);
        if (patient == null)
            return NotFound();

        return View(patient);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PatientViewModel model)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");

        var success = await _patientApiClient.CreatePatientAsync(model, token);
        if (!success)
        {
            ModelState.AddModelError("", "Erreur lors de la création du patient");
            return View(model);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");

        var patient = await _patientApiClient.GetPatientByIdAsync(id, token);
        if (patient == null)
            return NotFound();

        return View(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PatientViewModel model)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");

        var success = await _patientApiClient.UpdatePatientAsync(model, token);
        if (!success)
        {
            ModelState.AddModelError("", "Erreur lors de la mise à jour du patient");
            return View(model);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");

        var success = await _patientApiClient.DeletePatientAsync(id, token);
        if (!success)
        {
            TempData["Error"] = "Erreur lors de la suppression du patient.";
        }

        return RedirectToAction("Index");
    }
}
