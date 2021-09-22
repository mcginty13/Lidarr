using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Indexers.Soulseek
{
    public class SoulseekSettingsValidator : AbstractValidator<SoulseekSettings>
    {
        public SoulseekSettingsValidator()
        {
            RuleFor(c => c).Custom((c, context) =>
            {
                if (c.Categories.Empty())
                {
                    context.AddFailure("'Categories' must be provided");
                }
            });

            RuleFor(c => c.Username).NotEmpty();
            RuleFor(c => c.Password).NotEmpty();
        }
    }

    public class SoulseekSettings : IIndexerSettings
    {
        private static readonly SoulseekSettingsValidator Validator = new SoulseekSettingsValidator();

        public SoulseekSettings()
        {
            ApiPath = "/";
            BaseUrl = "";
            Token = "";
            Categories = new[] { 1, 1 };
        }

        public string BaseUrl { get; set; }

        public string ApiPath { get; set; }

        public string Token { get; set; }

        [FieldDefinition(0, Label = "Categories", HelpText = "Comma Separated list, leave blank to disable standard/daily shows")]
        public IEnumerable<int> Categories { get; set; }

        [FieldDefinition(1, Label = "Username")]
        public string Username { get; set; }

        [FieldDefinition(2, Label = "Password", Type = FieldType.Password)]
        public string Password { get; set; }

        [FieldDefinition(3, Type = FieldType.Number, Label = "Early Download Limit", Unit = "days", HelpText = "Time before release date Lidarr will download from this indexer, empty is no limit", Advanced = true)]
        public int? EarlyReleaseLimit { get; set; }

        public virtual NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
