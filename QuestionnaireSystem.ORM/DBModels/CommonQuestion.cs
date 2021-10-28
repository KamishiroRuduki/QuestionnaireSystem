namespace QuestionnaireSystem.ORM.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonQuestion")]
    public partial class CommonQuestion
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int Type { get; set; }

        [StringLength(1000)]
        public string QusetionOption { get; set; }

        public int IsChange { get; set; }

        public bool IsDel { get; set; }

        public int Number { get; set; }
    }
}
