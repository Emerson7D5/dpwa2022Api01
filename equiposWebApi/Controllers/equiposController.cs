using equiposWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace equiposWebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        //Configurar mi variable de conexion al contexto db
        private readonly prestamosContext _contexto;

        public equiposController(prestamosContext miContexto)
        {
            _contexto = miContexto;
        }

        [HttpGet]
        [Route("api/equipos/")]
        public IActionResult Get()
        {
            IEnumerable<equipos> listadoEquipo = (from e in _contexto.equipos
                                                  select e);

            if (listadoEquipo.Count() > 0)
            {
                return Ok(listadoEquipo);
            }

            return NotFound();

        }

        [HttpGet]
        [Route("api/equipos/{idUsuario}")]
        public IActionResult Get(int idUsuario)
        {
            equipos unEquipo = (from e in _contexto.equipos
                                where e.id_equipos == idUsuario
                                select e
                               ).FirstOrDefault();

            if (unEquipo != null)
            {
                return Ok(unEquipo);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("api/equipos")]
        public IActionResult guardarEquipo([FromBody] equipos equipoNuevo)
        {
            try
            {
                _contexto.equipos.Add(equipoNuevo);
                _contexto.SaveChanges();
                return Ok(equipoNuevo);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/equipos")]
        public IActionResult updateEquipo([FromBody] equipos equipoAModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            equipos equipoExiste = (from e in _contexto.equipos
                                    where e.id_equipos == equipoAModificar.id_equipos
                                    select e).FirstOrDefault();

            if (equipoExiste is null)
            {
                //Si no existte el registro retornamos un 404
                return NotFound();
            }

            //Si existe, modificamos los campos necesarios.
            equipoExiste.nombre = equipoAModificar.nombre;
            equipoExiste.descripcion = equipoAModificar.descripcion;

            //Guadarmos los cambios en el contexto y base de datos
            _contexto.Entry(equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(equipoExiste);

        }




    }
}
