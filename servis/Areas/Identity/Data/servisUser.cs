﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace servis.Areas.Identity.Data;

// Add profile data for application users by adding properties to the servisUser class
public class servisUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Year { get; set; }
    public string? Phone { get; set; }
    public int ModelID { get; set; }

}

