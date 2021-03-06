﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class ActorViewModel
    {
        public int ActorId { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Biography { get; set; }
        public CountryViewModel Country { get; set; }
        public int SelectedCountry { get; set; }

        public ActorViewModel() { }

        public ActorViewModel(Actor actor)
        {
            ActorId = actor.ActorId;
            FullName = actor.FullName;
            BirthDate = actor.BirthDate;
            Biography = actor.Biography;
            Country = new CountryViewModel(actor.Country);
            SelectedCountry = actor.Country.CountryId;
        }
    }
}