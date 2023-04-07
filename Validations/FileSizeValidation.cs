using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Validations
{
    public class FileSizeValidation : ValidationAttribute
    {
        private readonly int maxSizeInMb;

        public FileSizeValidation(int maxSizeInMb)
        {
            this.maxSizeInMb = maxSizeInMb;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {

                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;
            if (formFile is null)
            {

                return ValidationResult.Success;
            }

            if (formFile.Length > maxSizeInMb * 1024 * 1024)
            {
                return new ValidationResult($"The size of the file cannot be greater than {maxSizeInMb} mb ");
            }
            return ValidationResult.Success;
        }

    }
}
