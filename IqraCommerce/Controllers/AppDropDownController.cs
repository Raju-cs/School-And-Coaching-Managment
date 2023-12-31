﻿using IqraBase.Data.Entities;
using IqraBase.Data.Models;
using IqraBase.Service;
using IqraBase.Setup.Models;
using IqraBase.Setup.Pages;
using IqraBase.Web.Controllers;
using IqraCommerce.DTOs;
using IqraCommerce.Helpers;
using IqraService.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IqraCommerce.Controllers
{
    public class AppDropDownController<TEntity, TModel> : IqraDropDownController<TEntity, TModel> 
        where TEntity : DropDownBaseEntity
        where TModel : DropDownBaseModel
    {
        public IqraCommerce.Services.AppBaseService<TEntity> __service;
        public AppDropDownController()
        {

        }

        public override ActionResult Index()
        {
            if (IsLoggedId) return View();

            return View();
        }

       

        public override async Task<ResponseList<Pagger<Dictionary<string, object>>>> Get([FromBody] Page page)
        {
            return await __service.Get(page);
        }

        public override ActionResult Create([FromForm] TModel recordToCreate)
        {
            return Json(__service.OnCreate(recordToCreate, appUser.Id, true));
        }

        public override ActionResult Edit([FromForm] TModel recordToUpdate)
        {
            return Json(__service.Update(recordToUpdate, appUser.Id));
        }

        virtual public JsonResult Remove([FromBody] DeleteDto deleteDto)
        {
            __service.Remove(deleteDto, appUser.Id);

            return Json(new { });
        }

    }
}
