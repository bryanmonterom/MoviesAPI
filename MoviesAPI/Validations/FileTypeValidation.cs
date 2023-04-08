using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Validations
{
    public class FileTypeValidation:ValidationAttribute
    {
        private readonly string[] fileTypes;

        public FileTypeValidation(string[] fileTypes)
        {
            this.fileTypes = fileTypes;
        }

        public FileTypeValidation(FileTypeGroup fileTypeGroup)
        {
            if (fileTypeGroup == FileTypeGroup.Image) {
                fileTypes = new string[] {"image/jpeg","image/png","image/gif" };
            }
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

            if (!fileTypes.Contains(formFile.ContentType)) {
                return new ValidationResult($"The file type should be one of the followings {string.Join(", ", fileTypes)}");
            }
            return ValidationResult.Success;


        }
    }
}
