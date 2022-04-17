using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WisdomPetMedicine.Pet.Api.ApplicationServices;
using WisdomPetMedicine.Pet.Api.Commands;
using Dapper;
using System.Linq;

namespace WisdomPetMedicine.Pet.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetQueryController : ControllerBase
    {
        private readonly PetApplicationService petApplicationService;
        private readonly ILogger<PetController> logger;
        private readonly IConfiguration _configuration;

        public PetQueryController(PetApplicationService petApplicationService,
                             ILogger<PetController> logger,
                             IConfiguration configuration)
        {
            this.petApplicationService = petApplicationService;
            this.logger = logger;
            this._configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                string sql = @"SELECT p.Name_Value as Name,
                            p.Breed_Value as Breed,
                            Sex = 
                            CASE p.SexOfPet_Value
                              WHEN 0 THEN 'Male'
                              WHEN 1 THEN 'Female'
                            END,
                            p.Color_Value as Color,
                            p.DateOfBirth_Value as DateOfBirth,
                            p.Species_Value as Species
                            FROM Pets p";
                using var connection = new SqlConnection(_configuration.GetConnectionString("Pet"));
                var result = (await connection.QueryAsync(sql)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}