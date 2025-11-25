using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using session5demo.bl.DtoS.EmployeeDtoS;
using session5demo.bl.Sevices.EmployeeServices;
using session5demo.pl.ViewModels.Employeeupdatevm;

namespace session5demo.pl.Controllers.EmployeeController
{
[Authorize]


    public class EmployeeController:Controller
    {
        private readonly IemployeeServices s;
        private readonly ILogger l;
        private readonly IWebHostEnvironment w;

        public EmployeeController(IemployeeServices s,ILogger<EmployeeController> l,IWebHostEnvironment w)
        {
            this.s = s;
            this.l = l;
            this.w = w;
        }
        [HttpGet]
        public IActionResult Index(string? name)
        {
            var result = string.IsNullOrEmpty(name)
                ? s.getallservice()
                : s.searchbyname(name);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employeeupdatevm dto)
        {
            var emp = new createemployeedto()
            {
                Name = dto.Name,
                Age = dto.Age,
                Address = dto.Address,
                Gender = dto.Gender,
                deptid = dto.deptid,
                Email = dto.Email,
                EmployeeType = dto.EmployeeType,
                file=dto.file,
                
                

            };
            if (ModelState.IsValid)
            {
                try
                {
                    var res = s.addemployee(emp);
                    if (res > 0)
                    {
                        return RedirectToAction("index");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "not valid");

                        return View(dto);
                    }

                }
                catch(Exception ex)
                {
                    if (w.IsDevelopment())
                    {
                        l.LogError(ex.Message);
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }

          
                return View(dto);

            
        }
        [Authorize(Roles ="admin")]
        public IActionResult Edit(int ?id)
        {
            if (id is null) return BadRequest();
            var dto = s.getdetails(id.Value);
            if (dto is null) return NotFound();
            var emp = new Employeeupdatevm()
            {
                Id = dto.Id,
                Name = dto.Name,
                Age = dto.Age,
                Address = dto.Address,
                Gender = dto.Gender,
                deptid = dto.deptid,
                
                Email = dto.Email,
                EmployeeType = dto.EmployeeType,
                filename=dto.FileName,
               
                

            };
            return View(emp);
        }
        [HttpPost]

        public IActionResult Edit(Employeeupdatevm dto)
        {
            var emp = new updateemployeedto()
            {
                Id = dto.Id,
                Name = dto.Name,
                Age = dto.Age,
                Address = dto.Address,
                Gender = dto.Gender,
                deptid = dto.deptid,
                Email = dto.Email,
                EmployeeType = dto.EmployeeType,
                file=dto.file,
                filename=dto.filename
                
            };
            if (ModelState.IsValid)
            {
                try
                {
                    var res = s.updateemployee(emp);
                    if (res > 0)
                    {
                        return RedirectToAction("index");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "not valid");

                        return View(dto);
                    }

                }
                catch (Exception ex)
                {
                    if (w.IsDevelopment())
                    {
                        l.LogError(ex.Message);
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }


            return View(dto);


        }
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest();
            var res = s.getdetails(id.Value);
            if (res is null) return NotFound();
            return View(res);
        }
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var res = s.getdetails(id.Value);
            if (res is null) return NotFound();
            return View(res);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
           
            var res = s.deleteemployee(id);

            if (res > 0)
            {
                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }
    }
}
