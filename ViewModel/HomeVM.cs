using WebFrontToBack.Models;

namespace WebFrontToBack.ViewModel
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Category> Categories { get; set; }
        public List<Service> Services { get; set; }
        public List<RecentWork> RecentWorks { get; set; }
    }
}
