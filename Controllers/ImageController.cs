using Image_CRUD_Postman.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Image_CRUD_Postman.Controllers
{
    public class ImageController : ApiController
    {
        Image_CRUDEntities1 entities = new Image_CRUDEntities1();
        [HttpGet]
        //[Route("api/Image/GetAllImage")]

        public HttpResponseMessage GetAllImage()
        {

            var files = entities.Image_POSTMAN;
            return Request.CreateResponse(HttpStatusCode.OK, files);
        }

        [HttpPost]
        //[Route("api/Image/InsertImage")]
        public HttpResponseMessage InsertImage()
        {
            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Check if Request contains File.
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //Read the File data from Request.Form collection.
            HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

            //Fetch the File Name.
            string fileName = Path.GetFileName(postedFile.FileName);

            //Set the File Path.
            string filePath = @"E:\Image\" + fileName;

            //Save the File in Folder.
            postedFile.SaveAs(filePath);

            //Insert the File to Database Table.
            Image_POSTMAN file = new Image_POSTMAN
            {
                Name = fileName,
                Type = filePath

            };
            entities.Image_POSTMAN.Add(file);
            entities.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Data insert successfully");

        }


        [HttpDelete]
        //[Route("api/Image")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {

                Image_CRUDEntities1 entities = new Image_CRUDEntities1();
                Image_POSTMAN file = entities.Image_POSTMAN.ToList().Find(p => p.Id == id);

                string pic = file.Name;
                string path = @"E:\Image\" + file;
                FileInfo ff = new FileInfo(path);

                if (file != null)
                {
                    entities.Image_POSTMAN.Remove(file);
                    entities.SaveChanges();

                }
                return Request.CreateResponse(HttpStatusCode.OK, "data delete sucessfully");
            }

            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not delete");
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateImage(int id)
        {
            try
             {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];
                Image_POSTMAN putid = entities.Image_POSTMAN.Where(x => x.Id == id).FirstOrDefault();
                string file = putid.Name;
                string path = @"E:\Image\" + file;
                FileInfo ff = new FileInfo(path);
                if (ff.Exists)
                {
                    ff.Delete();
                    Image_POSTMAN tblupl = entities.Image_POSTMAN.Find(id)
;
                    tblupl.Id = id;
                    tblupl.Name = Path.GetFileName(postedFile.FileName);
                    tblupl.Type = postedFile.ContentType;
                    string filePath = @"E:\Image\" + tblupl.Name;
                    postedFile.SaveAs(filePath);
                    entities.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "updated sucessfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not updated");
            }
        }
    }
}
