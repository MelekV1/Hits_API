using FluentValidation;
using HitsAPI.api.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitsAPI.api.Validators
{
    public class SaveMusicResourceValidator : AbstractValidator<SaveMusicResource>
    {
        public SaveMusicResourceValidator()
        {
            RuleFor(m => m.Name)
            .NotEmpty()
            .MaximumLength(50);

            RuleFor(m => m.ArtistId)
            .NotEmpty()
            .WithMessage("'Artist Id' must not be 0.");
        }
    }
}
