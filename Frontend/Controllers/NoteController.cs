using Microsoft.AspNetCore.Mvc;
using P10.Frontend.Models;
using P10.Frontend.Services;

public class NoteController : Controller
{
    private readonly INoteApiClient _noteApiClient;

    public NoteController(INoteApiClient noteApiClient)
    {
        _noteApiClient = noteApiClient;
    }

    public async Task<IActionResult> Index(int patientId)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");
        var notes = await _noteApiClient.GetNotesByPatientAsync(patientId, token);
        ViewBag.PatientId = patientId;
        return View(notes);
    }

    [HttpGet]
    public IActionResult Create(int patientId)
    {
        ViewBag.PatientId = patientId;
        return View(new NoteViewModel { PatientId = patientId });
    }

    [HttpPost]
    public async Task<IActionResult> Create(NoteViewModel model)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");
        var success = await _noteApiClient.CreateNoteAsync(model, token);
        if (!success)
        {
            ModelState.AddModelError("", "Erreur lors de la création de la note");
            ViewBag.PatientId = model.PatientId;
            return View(model);
        }
        return RedirectToAction("Index", new { patientId = model.PatientId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id, int patientId)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");
        var note = await _noteApiClient.GetNoteByIdAsync(id, token);
        if (note == null)
            return NotFound();
        ViewBag.PatientId = patientId;
        return View(note);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(NoteViewModel model)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");
        var success = await _noteApiClient.UpdateNoteAsync(model, token);
        if (!success)
        {
            ModelState.AddModelError("", "Erreur lors de la mise à jour de la note");
            ViewBag.PatientId = model.PatientId;
            return View(model);
        }
        return RedirectToAction("Index", new { patientId = model.PatientId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id, int patientId)
    {
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");
        var success = await _noteApiClient.DeleteNoteAsync(id, token);
        if (!success)
        {
            TempData["Error"] = "Erreur lors de la suppression de la note.";
        }
        return RedirectToAction("Index", new { patientId });
    }
}
