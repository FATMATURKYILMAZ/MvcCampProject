﻿using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationsRules
{
    public class WriterValidator : AbstractValidator<Writer>
    {
        public WriterValidator()
        {
            RuleFor(x => x.WriterName).NotEmpty().WithMessage("Yazar Adını boş geçemezsiniz.");
            RuleFor(x => x.WriterSurName).NotEmpty().WithMessage("Yazar soyadını boş geçemezsiniz.");
            RuleFor(x => x.WriterAbout).NotEmpty().WithMessage("Hakkında kısmını boş geçemezsiniz.");
            RuleFor(x => x.WriterTitle).NotEmpty().WithMessage("Unvan kısmını boş geçemezsiniz.");

            RuleFor(x => x.WriterAbout).Must(x => x != null && x.ToLower().Contains("a"))
            .WithMessage("Hakkında kısmında mutlaka 'a' harfi bulunmalıdır.");
            RuleFor(x => x.WriterSurName).MinimumLength(2).WithMessage("Lütfen en az 3 karakter girişi yapın");
            RuleFor(x => x.WriterSurName).MaximumLength(50).WithMessage("Lütfen 50'den fazla karakter girişi yapmayınız");
        }
    }
}
