using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WrestlingInfo.API.Models.Validation;

public class DoubleRangeAttribute : ValidationAttribute {
	public double[] AllowableValues { get; set; } = [];

	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
		if (AllowableValues.Contains((double)(value ?? throw new ArgumentNullException(nameof(value))))) {
			return ValidationResult.Success;
		}

		string msg;
		if (AllowableValues.Length > 0) {
			msg = "Please enter one of the allowable values:";
			bool first = true;
			foreach (double doubleValue in AllowableValues) {
				if (!first) {
					msg += ", ";
				} else {
					first = false;
				}
				msg += doubleValue;
			}
		} else {
			msg = "No allowable values found.";
		}

		return new ValidationResult(msg);
	}
}