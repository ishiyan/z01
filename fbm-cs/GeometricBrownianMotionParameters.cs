using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Mbs.Numerics.RandomGenerators;

namespace Mbs.Trading.Data.Generators.GeometricBrownianMotion
{
    /// <summary>
    /// The input parameters for the geometric Brownian motion generator.
    /// </summary>
    public class GeometricBrownianMotionParameters : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the amplitude of the geometric Brownian motion, should be positive.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [DefaultValue(DefaultParameterValues.GbmAmplitude)]
        [Range(0.0, double.MaxValue)]
        public double Amplitude { get; set; } = DefaultParameterValues.GbmAmplitude;

        /// <summary>
        /// Gets or sets the value corresponding to the minimum of the geometric Brownian motion, should be positive.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [DefaultValue(DefaultParameterValues.GbmMinimalValue)]
        [Range(0.0, double.MaxValue)]
        public double MinimalValue { get; set; } = DefaultParameterValues.GbmMinimalValue;

        /// <summary>
        /// Gets or sets the value of the drift (the expected return), μ, of the geometric Brownian motion.
        /// The model assumes the value will "drift" up by the expected return, being shocked (added or subtracted) by a random shock.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [DefaultValue(DefaultParameterValues.GbmDrift)]
        public double Drift { get; set; } = DefaultParameterValues.GbmDrift;

        /// <summary>
        /// Gets or sets the value of the volatility (standard deviation), σ, of the geometric Brownian motion.
        /// At each step, the drift will be shocked (added or subtracted) by this value multiplied by a normal random number.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [DefaultValue(DefaultParameterValues.GbmVolatility)]
        public double Volatility { get; set; } = DefaultParameterValues.GbmVolatility;

        /// <summary>
        /// Gets or sets the kind of a Gaussian distribution random generator.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [EnumDataType(typeof(NormalRandomGeneratorKind), ErrorMessage = ErrorMessages.FieldEnumValueInvalid)]
        [DefaultValue(DefaultParameterValues.GbmNormalRandomGeneratorKind)]
        public NormalRandomGeneratorKind NormalRandomGeneratorKind { get; set; } = DefaultParameterValues.GbmNormalRandomGeneratorKind;

        /// <summary>
        /// Gets or sets the kind of a uniform random generator optionally associated with the Gaussian random generator.
        /// Used only by ZigguratColinGreen and BoxMuller Gaussian generators.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [EnumDataType(typeof(UniformRandomGeneratorKind), ErrorMessage = ErrorMessages.FieldEnumValueInvalid)]
        [DefaultValue(DefaultParameterValues.GbmAssociatedUniformRandomGeneratorKind)]
        public UniformRandomGeneratorKind AssociatedUniformRandomGeneratorKind { get; set; } = DefaultParameterValues.GbmAssociatedUniformRandomGeneratorKind;

        /// <summary>
        /// Gets or sets the seed of a random generator.
        /// If ZigguratColinGreen or BoxMuller Gaussian generator is used, the seed will be applied to the associated uniform generator.
        /// Otherwise, it will be applied to the Gaussian generator.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [DefaultValue(DefaultParameterValues.GbmSeed)]
        public int Seed { get; set; } = DefaultParameterValues.GbmSeed;

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
