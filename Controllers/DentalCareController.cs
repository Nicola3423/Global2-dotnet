using Microsoft.AspNetCore.Mvc;

namespace Sessions_app.Controllers
{
    public class DentalCareController : Controller
    {
        public IActionResult DentalCare()
        {
            return View();
        }
    }
}
