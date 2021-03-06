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
    using System.Web.Mvc;

    public partial class Material
    {
        public int MId { get; set; }
        [Required, Display(Name = "Title")]
        public string Title { get; set; }
        [Required, Display(Name = "Type")]
        public string Type { get; set; }
        [Required, Display(Name = "Course Id")]
        public int CId { get; set; }
        [Required, Display(Name = "File Path")]
        public string FilePath { get; set; }

        public static IEnumerable<SelectListItem> GetTypeList()
        {
            IList<SelectListItem> status = new List<SelectListItem>
            {
                new SelectListItem() {Text="Assignment", Value="Assignments"},
                new SelectListItem() {Text="Notes", Value="Notes"},
                new SelectListItem() {Text="Past Paper", Value="PastPapers"},
                new SelectListItem() {Text="Readings", Value="Readings"}
            };
            return status;
        }
        public virtual Course Course { get; set; }
    }
}
