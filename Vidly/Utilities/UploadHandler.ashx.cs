using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vidly.Utilities
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public  class  UploadHandler : IHttpHandler
    {

        public  void ProcessRequest(HttpContext context)
        {
            //check whether client has uploaded a file or not 
           if(context.Request.Files.Count > 0)
            {
                //if files selected by the user then get them
                HttpFileCollection files = context.Request.Files;
                //traverse through each file and save it destination folder
                for (int i =0; i<files.Count; i++)
                {
                    //since our machine will be acting as client and server 
                    //so inorder see Progress bar we need introduce some latency
                    //Delete the below line when deployed on production enviornment
                    //System.Threading.Thread.Sleep(1000);

                    //Get the file to upload 
                    HttpPostedFile file = files[i];
                    //Construct file path along with fileName
                    //Upload folder is in project root so used ~ /
                    string fileName = context
                                      .Server
                                      .MapPath("~/Upload/" + System.IO.Path.GetFileName(file.FileName));
                    //finally save the file 
                    file.SaveAs(fileName);
                }
            }
        }

        public  bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}