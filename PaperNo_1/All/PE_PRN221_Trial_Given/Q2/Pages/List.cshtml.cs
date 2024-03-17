using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Q2.Models;
using Q2.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Q2.Pages
{
    public class ListModel : PageModel
    {
        private readonly PRN221_TrialContext _context;

        public ListModel(PRN221_TrialContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IFormFile FileInput { get; set; }

        [BindProperty]
        public string Search { get; set; }

        public List<Service> ListService { get; set; }
        public XmlDataModel XmlData { get; set; }


        public IActionResult OnGet(string search)
        {
            int year = 2022;
            int month = 3;

            ListService = _context.Services
                .Include(x => x.EmployeeNavigation)
                .Where(x => x.Year == year && x.Month == month)
                .ToList();
            Search = search;

            if (!string.IsNullOrEmpty(search))
            {
                ListService = ListService.Where(x => x.RoomTitle == search).ToList();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (FileInput == null || FileInput.Length <= 0)
            {
                return RedirectToPage("List");
            }

            using (var memoryStream = new MemoryStream())
            {
                FileInput.CopyTo(memoryStream);
                memoryStream.Position = 0;

                var serializer = new XmlSerializer(typeof(XmlDataModel));
                XmlData = (XmlDataModel)serializer.Deserialize(memoryStream);

                try
                {
                    if (XmlData != null)
                    {
                        foreach (var item in XmlData.Services)
                        {
                            Service s = new Service()
                            {
                                Amount = item.Amount,
                                FeeType = item.FeeType,
                                Id = item.Id,
                                RoomTitle = item.RoomTitle,
                                Month = (byte)item.Month,
                                Year = item.Year,
                                PaymentDate = DateTime.Parse(item.PaymentDate),
                                Employee = int.Parse(item.Employee)

                            };
                            _context.Add(item);
                            _context.SaveChanges();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return RedirectToPage("List");
        }
    }
    [XmlRoot("ArrayOfService")]
    public class XmlDataModel
    {
        [XmlElement("Service")]
        public ServiceModel[] Services { get; set; }
    }

    public class ServiceModel
    {
        public int Id { get; set; }
        public string RoomTitle { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }
        public string PaymentDate { get; set; }
        public string Employee { get; set; }
    }
}
