using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity._1.Data;
using ContosoUniversity._1.Models;

namespace ContosoUniversity._1.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;
        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string MidSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<Student> Students { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;


            {
                // using System;
                NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                DateSort = sortOrder == "Date" ? "date_desc" : "Date";
                MidSort = String.IsNullOrEmpty(sortOrder) ? "mid_desc" : "";
                if (searchString != null)
                {
                    pageIndex = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                CurrentFilter = searchString;

                IQueryable<Student> studentsIQ = from s in _context.Students
                                                 select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                           || s.FirstMidName.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                        break;
                    case "mid_desc":
                        studentsIQ = studentsIQ.OrderByDescending(s => s.FirstMidName);
                        break;
                    case "Date":
                        studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                        break;
                    case "date_desc":
                        studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                        break;
                    default:
                        studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                        break;
                }

                int pageSize = 3;
                Students = await PaginatedList<Student>.CreateAsync(
                    studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize); ;
            }
        }
    }
}
    

