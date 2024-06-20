﻿using EmployeeRequestTrackerApp.Exceptions;
using EmployeeRequestTrackerApp.Interfaces;
using EmployeeRequestTrackerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRequestTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase 
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [Authorize]
        [HttpGet("GetAllEmployees")]
        [ProducesResponseType(typeof(IList<Employee>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ErrorModel))]
        public async Task<ActionResult<IList<Employee>>> Get()
        {
            try
            {
                var employees = await _employeeService.GetEmployees();
                return Ok(employees.ToList());
            }
            catch (NoEmployeesFoundException nefe)
            {
                return NotFound(new ErrorModel(404, nefe.Message));
            }
        }
        [Authorize]
        [HttpPut("UpdateEmployeePhone")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ErrorModel))]
        public async Task<ActionResult<Employee>> Put([FromBody] int id, string phone)
        {
            Console.WriteLine(phone,id);
            try
            {
                var employee = await _employeeService.UpdateEmployeePhone(id, phone);
                return Ok(employee);
            }
            catch (NoSuchEmployeeException nsee)
            {
                return NotFound(new ErrorModel(404, nsee.Message));
            }
        }

        [Authorize(Roles = "User")]
        [Route("GetEmployeeByPhone")]
        [HttpPost]
        public async Task<ActionResult<Employee>> Get([FromBody] string phone)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByPhone(phone);
                return Ok(employee);
            }
            catch (NoSuchEmployeeException nefe)
            {
                return NotFound(nefe.Message);
            }
        }


    }
}
