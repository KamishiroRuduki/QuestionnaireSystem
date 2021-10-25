namespace QuestionnaireSystem.ORM.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Static")]
    public partial class Static
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public Guid QuestionnaireID { get; set; }

        public Guid QuestionID { get; set; }

        [Required]
        [StringLength(100)]
        public string QuestionOption { get; set; }

        public int Sum { get; set; }
    }
}
