using System.ComponentModel.DataAnnotations;
namespace RMS.Data.Validators;

public class UrlResource : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext ctx){
        String url = (string)value;

        if (url != null && !UrlResourceExists(url)){
            return new ValidationResult("Url resource does not exist");
        }//if
        return ValidationResult.Success;
    }//validation result

    private bool UrlResourceExists(string Url){
        using(var http = new HttpClient()){
            var result = http.SendAsync(new HttpRequestMessage(HttpMethod.Head, Url)).Result;
            return result.IsSuccessStatusCode;
        }//checking html
    }//url resource exists
}//urlresource

