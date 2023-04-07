﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileManager fileManager;
        private readonly string container = "actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileManager fileManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileManager = fileManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertPaginationParameters(queryable, paginationDTO.RecordsQuantityPerPage);
            var entities = await queryable.Paginate(paginationDTO).ToListAsync();


            return mapper.Map<List<ActorDTO>>(entities);
        }

        [HttpGet("{id}", Name = "getActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var entity = await context.Actors.FirstOrDefaultAsync(a => a.Id == id);
            if (entity is null)
            {
                return NotFound();
            }
            return mapper.Map<ActorDTO>(entity);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var entity = mapper.Map<Actor>(actorCreationDTO);

            if (actorCreationDTO.Photo is not null)
            {
                using (var memoryStream = new MemoryStream())
                {

                    await actorCreationDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreationDTO.Photo.FileName);
                    entity.Photo = await fileManager.SaveFile(content, extension, container, actorCreationDTO.Photo.ContentType);
                }
            }
            context.Add(entity);
            await context.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(entity);
            return new CreatedAtRouteResult("getActor", new { id = entity.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
        {

            var actorDB = await context.Actors.FirstOrDefaultAsync(a => a.Id == id);

            if (actorDB is null)
            {
                return NotFound();
            }

            actorDB = mapper.Map(actorCreationDTO, actorDB);
            if (actorCreationDTO.Photo is not null)
            {
                using (var memoryStream = new MemoryStream())
                {

                    await actorCreationDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreationDTO.Photo.FileName);
                    actorDB.Photo = await fileManager.EditFile(content, extension, container, actorCreationDTO.Photo.ContentType, actorDB.Photo);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {

            if (patchDocument is null)
            {
                return BadRequest();
            }

            var entity = await context.Actors.FirstOrDefaultAsync(a => a.Id == id);

            if (entity is null)
            {

                return NotFound();
            }

            var entityDTO = mapper.Map<ActorPatchDTO>(entity);
            patchDocument.ApplyTo(entityDTO, ModelState);

            var isValid = TryValidateModel(entityDTO);
            if (!isValid)
            {

                return BadRequest(ModelState);
            }

            mapper.Map(entityDTO, entity);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Actors.AnyAsync(actor => actor.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Actor { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
