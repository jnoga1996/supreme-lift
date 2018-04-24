﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Abstract;
using Microsoft.AspNet.Identity;

namespace WebApplication.Controllers
{
    [Authorize]
    public class RecordController : Controller
    {
        private readonly IDataRepository _repository;
        private User _user;

        public RecordController(IDataRepository repository)
        {
            _repository = repository;
        }

        private void SetUser()
        {
            string userName = HttpContext.User.Identity.GetUserName();
            _user = _repository.FindUser(userName);
        }

        private SelectList PopulateExercisesSelectList()
        {
            var data = from e in _repository.GetExercises()
                select new
                {
                    Id = e.ExerciseId,
                    Name = e.Name
                };

            return new SelectList(data, "Id", "Name");
        }

        // GET: Record
        public ActionResult Index()
        {
            SetUser();
            var records = _repository.GetRecords();
            return View(records);
        }

        public ActionResult Get()
        {
            SetUser();
            var currentUserRecords = _repository.GetRecords(_user);

            return View(currentUserRecords);
        }

        public ActionResult Create()
        {
            SetUser();
            ViewBag.ExercisesList = PopulateExercisesSelectList();

            return View();
        }

        [HttpPost]
        public ActionResult Create(Record record)
        {
            SetUser();
            record.Fk_UserId = _user.UserId;
            record.User = _user;
            record.Exercise = _repository.FindExercise((int)record.Fk_ExerciseId);

            if (ModelState.IsValid)
            {
                _repository.AddRecord(record);
                return RedirectToAction("Get");
            }

            return View(record);
        }

        public ActionResult Edit(int id)
        {
            SetUser();
            Record recordToEdit = _repository.FindRecord(id);
            ViewBag.ExercisesList = PopulateExercisesSelectList();

            if (recordToEdit == null || recordToEdit.User != _user)
            {
                return HttpNotFound("Invalid record id.");
            }

            return View(recordToEdit);
        }

        [HttpPost]
        public ActionResult Edit(Record record)
        {
            SetUser();
            record.Fk_UserId = _user.UserId;
            record.User = _user;
            record.Exercise = _repository.FindExercise((int)record.Fk_ExerciseId);

            if (ModelState.IsValid)
            {
                _repository.AddRecord(record);
                return RedirectToAction("Get");
            }

            return View(record);
        }
    }
}