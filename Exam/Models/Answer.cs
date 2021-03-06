//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exam.Models
{
    using System;
    using System.Collections.Generic;
    //
    using System.ComponentModel.DataAnnotations;
    public partial class Answer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Answer()
        {
            this.Detail = new HashSet<Detail>();
        }

        public int AnswerID { get; set; }

        [Display(Name = "Question ID")]
        [Required(ErrorMessage = "{0}is mandatory")]
        public int PaperID { get; set; }

        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "{0}is mandatory")]
        public int StuID { get; set; }

        [Display(Name = "Teacher ID")]
        public int TeacherID { get; set; }

        [Display(Name = "Scores")]
        [Required(ErrorMessage = "{0}is mandatory")]
        public int AnswerScore { get; set; }

        [Display(Name = "Testing time")]
        public Nullable<System.DateTime> AnswerTime { get; set; }

        [Display(Name = "Submission time")]
        public Nullable<System.DateTime> SubmitTime { get; set; }

        [Display(Name = "Reviewing time")]
        public Nullable<System.DateTime> BatchTime { get; set; }

        [Display(Name = "Status")]
        public int AnswerState { get; set; }



        public virtual Paper Paper { get; set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail> Detail { get; set; }
    }
}
