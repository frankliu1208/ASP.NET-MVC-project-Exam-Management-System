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
    public partial class Paper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Paper()
        {
            this.Answer = new HashSet<Answer>();
            this.Topic = new HashSet<Topic>();
        }

        public int PaperID { get; set; }
        [Display(Name = "试卷名")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string PaperName { get; set; }
        [Display(Name = "试卷说明")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string PaperExplain { get; set; }
        [Display(Name = "试卷时长")]
        [Required(ErrorMessage = "{0}是必填项")]
        public int PaperTime { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Topic> Topic { get; set; }
    }
}
