namespace QuestionnaireSystem.ORM.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Answer")]
    public partial class Answer
    {
        public int ID { get; set; }

        public Guid QuestionID { get; set; }

        [StringLength(1000)]
        public string AnswerOption { get; set; }

        [StringLength(1000)]
        public string AnswerLetter { get; set; }

        public Guid PersonID { get; set; }
    }
}
