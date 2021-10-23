namespace QuestionnaireSystem.ORM.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Option")]
    public partial class Option
    {
        public int ID { get; set; }

        public Guid QuestionID { get; set; }

        [StringLength(50)]
        public string Option1 { get; set; }

        [StringLength(50)]
        public string Option2 { get; set; }

        [StringLength(50)]
        public string Option3 { get; set; }

        [StringLength(50)]
        public string Option4 { get; set; }

        [StringLength(50)]
        public string Option5 { get; set; }

        [StringLength(50)]
        public string Option6 { get; set; }

        [StringLength(50)]
        public string Option7 { get; set; }

        [StringLength(50)]
        public string Option8 { get; set; }

        [StringLength(50)]
        public string Option9 { get; set; }

        [StringLength(50)]
        public string Option10 { get; set; }

        public int OptionAll { get; set; }
    }
}
