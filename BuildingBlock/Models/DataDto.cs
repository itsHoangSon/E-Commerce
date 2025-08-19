using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Models
{
    public class SimpleResponseDto
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public SimpleResponseDto() { }
        public SimpleResponseDto(CreatorModel model)
        {
            Id = model.Id;
            Code = model.Username;
            Name = model.DisplayName;
        }
    }
    public class AuditableResponseDto
    {

        public AuditableResponseDto(DataModel model)
        {
            Id = model.Id;
            Informations = model.Informations;
            Warnings = model.Warnings;
            Errors = model.Errors;

            Creator = model.Creator == null ? null : new SimpleResponseDto(model.Creator);
            CreatedAt = model.CreatedAt;
            LastModifiedAt = model.LastModifiedAt;
        }
        public long Id { get; set; }
        public SimpleResponseDto? Creator { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public Dictionary<string, string> Informations { get; set; }
        public Dictionary<string, string> Warnings { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }

    public class DataRequestDto
    {
        public long Id { get; set; }

    }
}
