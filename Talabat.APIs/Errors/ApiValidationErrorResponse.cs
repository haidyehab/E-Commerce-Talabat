namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorResponse :ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }//this is Reference
        public ApiValidationErrorResponse():base(400) //here I do chain ctor of parent
        {
            Errors = new List<string>();  
        }
    }
}
