using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Mbs.Numerics.RandomGenerators;
using Mbs.Numerics.RandomGenerators.FractionalBrownianMotion;

namespace Mbs.Trading.Data.Generators.FractionalBrownianMotion
{
    /// <summary>
    /// The input parameters for the fractional Brownian motion generator.
    /// </summary>
    public class FractionalBrownianMotionParameters : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the amplitude of the fractional Brownian motion, should be positive.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [DefaultValue(DefaultParameterValues.FbmAmplitude)]
        [Range(0.0, double.MaxValue)]
        public double Amplitude { get; set; } = DefaultParameterValues.FbmAmplitude;

        /// <summary>
        /// Gets or sets the value corresponding to the minimum of the fractional Brownian motion, should be positive.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [DefaultValue(DefaultParameterValues.FbmMinimalValue)]
        [Range(0.0, double.MaxValue)]
        public double MinimalValue { get; set; } = DefaultParameterValues.FbmMinimalValue;

        /// <summary>
        /// Gets or sets the value of the Hurst exponent of the fractal Brownian motion, H∈[0, 1].
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [DefaultValue(DefaultParameterValues.FbmHurstExponent)]
        [Range(0.0, 1.0)]
        public double HurstExponent { get; set; } = DefaultParameterValues.FbmHurstExponent;

        /// <summary>
        /// Gets or sets the fractional Brownian motion algorithm.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [EnumDataType(typeof(FractionalBrownianMotionAlgorithm), ErrorMessage = ErrorMessages.FieldEnumValueInvalid)]
        [DefaultValue(DefaultParameterValues.FbmAlgorithm)]
        public FractionalBrownianMotionAlgorithm Algorithm { get; set; } = DefaultParameterValues.FbmAlgorithm;

        /// <summary>
        /// Gets or sets the kind of a Gaussian distribution random generator.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [EnumDataType(typeof(NormalRandomGeneratorKind), ErrorMessage = ErrorMessages.FieldEnumValueInvalid)]
        [DefaultValue(DefaultParameterValues.FbmNormalRandomGeneratorKind)]
        public NormalRandomGeneratorKind NormalRandomGeneratorKind { get; set; } = DefaultParameterValues.FbmNormalRandomGeneratorKind;

        /// <summary>
        /// Gets or sets the kind of a uniform random generator optionally associated with the Gaussian random generator.
        /// Used only by ZigguratColinGreen and BoxMuller Gaussian generators.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [EnumDataType(typeof(UniformRandomGeneratorKind), ErrorMessage = ErrorMessages.FieldEnumValueInvalid)]
        [DefaultValue(DefaultParameterValues.FbmAssociatedUniformRandomGeneratorKind)]
        public UniformRandomGeneratorKind AssociatedUniformRandomGeneratorKind { get; set; } = DefaultParameterValues.FbmAssociatedUniformRandomGeneratorKind;

        /// <summary>
        /// Gets or sets the seed of a random generator.
        /// If ZigguratColinGreen or BoxMuller Gaussian generator is used, the seed will be applied to the associated uniform generator.
        /// Otherwise, it will be applied to the Gaussian generator.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.FieldIsRequired, AllowEmptyStrings = false)]
        [DefaultValue(DefaultParameterValues.FbmSeed)]
        public int Seed { get; set; } = DefaultParameterValues.FbmSeed;

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
