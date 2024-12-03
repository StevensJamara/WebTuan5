using Microsoft.AspNetCore.Mvc;

namespace BT.Controllers
{
    public class uploadPicture : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public uploadPicture(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                var filePath = Path.Combine(uploadsFolder, file.Name);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return RedirectToAction("Index");
            }
            return BadRequest("No file specified or file is empty");
        }
    }
}
