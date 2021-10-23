namespace QuestionnaireSystem.ORM.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        public Guid ID { get; set; }

        public Guid QuestionnaireID { get; set; }

        public int Number { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int Type { get; set; }

        [StringLength(1000)]
        public string QusetionOption { get; set; }

        public int IsCommon { get; set; }

        public bool IsMust { get; set; }
    }
}
