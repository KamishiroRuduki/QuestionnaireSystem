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

        public Guid QuestionnaireID { get; set; }

        public Guid QuestionID { get; set; }

        public int? Option1 { get; set; }

        public int? Option2 { get; set; }

        public int? Option3 { get; set; }

        public int? Option4 { get; set; }

        public int? Option5 { get; set; }

        public int? Option6 { get; set; }

        public int? Option7 { get; set; }

        public int? Option8 { get; set; }

        public int? Option9 { get; set; }

        public int? Option10 { get; set; }

        public int OptionAll { get; set; }
    }
}
