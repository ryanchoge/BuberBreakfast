﻿using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuberBreakfast.Controllers;


     
    public class BreakfastsController : ApiController
    {
        private readonly IBreakfastService _breakfastService;
        public BreakfastsController(IBreakfastService breakfastService)
        {
            _breakfastService = breakfastService;
        }
        // POST api/<BreakfastsController>
        [HttpPost]
        public IActionResult CreateBreakfast(CreateBreakfastRequest request)
        {
  
            ErrorOr<Breakfast> requestToBreakfastResult =  Breakfast.From(request);
                 if(requestToBreakfastResult.IsError){
                    return Problem(requestToBreakfastResult.Errors);
                 }
                 var breakfast = requestToBreakfastResult.Value;
            //TODO: Save breakfast to a database
            ErrorOr<Created> createBreakfastResult =  _breakfastService.CreateBreakfast(breakfast);
            return createBreakfastResult.Match(created => CreatedAsGetBreakfast(breakfast),errors => Problem(errors));
            // var response = new BreakfastResponse(breakfast.Id, breakfast.Name, breakfast.Description, breakfast.StartDateTime, breakfast.EndDateTime, breakfast.LastModifiedTime, breakfast.Savory, breakfast.Sweet);
          
        }
        // GET: api/<BreakfastsController>
        [HttpGet("{id:guid}")]
         public IActionResult GetBreakfast(Guid id)
        {
           ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);
           return getBreakfastResult.Match(breakfast => Ok(MapBreakfastResponse(breakfast)), errors => Problem(errors));
        //    if(getBreakfastResult.IsError && getBreakfastResult.FirstError == Errors.Breakfast.NotFound)
        //    {
        //     return NotFound();
        //    }
        //    var breakfast = getBreakfastResult.Value;
        //     var response = new BreakfastResponse(breakfast.Id, breakfast.Name, breakfast.Description, breakfast.StartDateTime, breakfast.EndDateTime, breakfast.LastModifiedTime, breakfast.Savory, breakfast.Sweet);
           // return Ok(response);
        }
  

        // PUT api/<BreakfastsController>/5
        [HttpPut("{id:guid}")]
        public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
        {
            ErrorOr<Breakfast> requestToBreakfastResult  = Breakfast.From(id,request);
            if(requestToBreakfastResult.IsError){
                return Problem(requestToBreakfastResult.Errors);

            }
            var breakfast = requestToBreakfastResult.Value;
            
            ErrorOr<UpsertedBreakfast> upsertBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);
            //TODO Return 201 if a new breakfast was created
            return upsertBreakfastResult.Match(upserted =>upserted.IsNewlyCreated ? CreatedAsGetBreakfast(breakfast) : NoContent(), errors => Problem(errors));
        }

        // DELETE api/<BreakfastsController>/5
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteBreakfast(Guid id)
        { 
            ErrorOr<Deleted> deletedBreakfastResult= _breakfastService.DeleteBreakfast(id);
            return deletedBreakfastResult.Match(deleted => NoContent(),errors => Problem(errors));
        }


        private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast){
            return new BreakfastResponse(breakfast.Id, breakfast.Name, breakfast.Description, breakfast.StartDateTime, breakfast.EndDateTime, breakfast.LastModifiedTime, breakfast.Savory, breakfast.Sweet);
        }
        private CreatedAtActionResult CreatedAsGetBreakfast(Breakfast breakfast)
        {
            return CreatedAtAction(actionName : nameof(GetBreakfast), routeValues : new { id = breakfast.Id}, value: MapBreakfastResponse(breakfast));
        }
    }
