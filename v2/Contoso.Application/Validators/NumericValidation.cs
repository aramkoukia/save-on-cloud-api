using System;
using SaveOnCloud.Core;
using LanguageExt;
using static LanguageExt.Prelude;

namespace SaveOnCloud
{
    public static partial class Validators
    {
        public static Func<decimal, Validation<Error, decimal>> AtLeast(decimal minimum) =>
            value => Optional(value)
                .Where(d => d >= minimum)
                .ToValidation<Error>($"Must be greater or equal to {minimum}");

        public static Func<decimal, Validation<Error, decimal>> AtMost(decimal max) =>
            value => Optional(value)
                .Where(d => d <= max)
                .ToValidation<Error>($"Must be less than or equal to {max}");
    }
}
