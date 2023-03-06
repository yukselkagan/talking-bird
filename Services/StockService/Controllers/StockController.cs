using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockService.Services;
using System.Net.Http.Headers;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        public StockController()
        {
        }


        [HttpPost("profile-image")]
        public ActionResult UploadProfileImage()
        {
            try
            {
                var file = Request.Form.Files[0];

                var folderName = Path.Combine("Stock", "ProfileImages");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileExtension = Path.GetExtension(fileName);
                    if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg")
                    {
                        string createdUniqueNamae = GeneralService.CreateUniqueName();
                        fileName = createdUniqueNamae + fileExtension;
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        //publish to service - userId and var fileName
                        return Ok();
                    }
                    else
                    {
                        throw new Exception("Wrong file type");
                    }
                }
                else
                {
                    throw new Exception("Empty file");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }





    }
}
