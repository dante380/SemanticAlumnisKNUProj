using System.ComponentModel.DataAnnotations;

namespace SemanticKNUProj.Model
{
    public class AlumniModel
    {
        public AlumniModel()
        {
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    }
}
