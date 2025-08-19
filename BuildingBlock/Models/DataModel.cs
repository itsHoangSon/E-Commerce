using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Models
{
    public class CreatorModel
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? DisplayName { get; set; }
    }
    public class DataModel
    {
        public bool IsValidated
        {
            get
            {
                if (Errors != null && Errors.Count > 0) return false;
                else return true;
            }
        }

        public long Id { get; set; }

        public DateTime? CreatedAt { get; set; }
        public long? CreatorId { get; set; }
        public CreatorModel? Creator { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public long? LastModifierId { get; set; }
        public Dictionary<string, string> Informations { get; private set; }
        public Dictionary<string, string> Warnings { get; private set; }
        public Dictionary<string, string> Errors { get; private set; }

        public void AddInformation(string key, string value)
        {
            if (Informations == null) Informations = new Dictionary<string, string>();

            if (Informations.ContainsKey(key))
            {
                if (!Informations[key].Contains(value))
                    Informations[key] += value;
            }
            else
                Informations.Add(key, value);
        }

        public void AddWarning(string key, string value)
        {
            if (Warnings == null) Warnings = new Dictionary<string, string>();

            if (Warnings.ContainsKey(key))
            {
                if (!Warnings[key].Contains(value))
                    Warnings[key] += value;
            }
            else
                Warnings.Add(key, value);
        }

        public void AddError(string key, string value)
        {
            if (Errors == null) Errors = new Dictionary<string, string>();

            if (Errors.ContainsKey(key))
            {
                if (!Errors[key].Contains(value))
                    Errors[key] += value;
            }
            else
                Errors.Add(key, value);
        }
    }
}