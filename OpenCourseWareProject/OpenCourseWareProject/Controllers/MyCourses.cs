//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OpenCourseWareProject.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class MyCourses
    {
        public int Id { get; set; }
        [Required, Display(Name = "Course Id")]
        public int CId { get; set; }
        [Required, Display(Name = "User Id")]
        public string UserId { get; set; }
    }
}
