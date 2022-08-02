using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MotiSectorAPI.ActionFilters;
using MotiSectorAPI.Business;
using MotiSectorAPI.DataAccess;
using MotiSectorAPI.Model;
using MotiSectorAPI.Repository;
using Safari.Bussiness;
using Safari.Domain;


namespace MotiSectorAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizationRequired]
    [MotiSectorAPIExceptionFilter]
    public class StudentController : ApiController
    {
        [HttpPut]
        [Route("Api/StudentUpdate/{id:int}")]
        public bool Put(int id, [FromBody] tblStudent pmt)
        {
            string ErrorMessage = "";
            int ErrorCode = 200;

            Result r = new Result();

            int studentId = 0;
            try
            {
                studentId = Convert.ToInt32(pmt.ID);
            }
            catch
            {
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Student. doesn't exist"),
                    ReasonPhrase = "Student No. doesn't exist",
                    StatusCode = HttpStatusCode.SwitchingProtocols
                };
                //ErrorMessage = "Character is not allowed in Invoice number";
                //ErrorCode = (int)HttpStatusCode.Ambiguous;
                //ErrorMessage = "Invoice No. doesn't exist";
                //ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                throw new HttpResponseException(msg);
            }

            if (id != pmt.ID)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("student number in path param is different form request body"),
                    ReasonPhrase = "student number in path param is different form request body",
                    StatusCode = HttpStatusCode.Redirect
                };
                //ErrorMessage = "Invoice number in path param is different form request body";
                //ErrorCode = (int)HttpStatusCode.Redirect;
                throw new HttpResponseException(msg);
            }



            tblStudent objStud = new tblStudent();
            tblStudentBussiness objbuss = new tblStudentBussiness();
            var result = objbuss.UpdatetblStudent(pmt);
            if(objbuss.UpdatetblStudent(pmt))
            {
                r.ErrorCode = 200;
                r.ErrorMessage = string.Empty;
                r.IsSaved = true;
                
            }
            
            else
            {
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(r.ErrorMessage),
                    ReasonPhrase = r.ErrorMessage,
                    StatusCode = (HttpStatusCode)r.ErrorCode
                };

                throw new HttpResponseException(msg);
            }
            return true;
        }



        [HttpPost]
        [Route("Api/StudentInsert/{id:int}")]
        public bool Post(int id, [FromBody] tblStudent pmt)
        {
            string ErrorMessage = "";
            int ErrorCode = 200;

            Result r = new Result();
            tblStudent objStud = new tblStudent();
            tblStudentBussiness objbuss = new tblStudentBussiness();
            int studentId = 0;
            try
            {
                studentId = Convert.ToInt32(pmt.ID);
            }
            catch
            {
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Student. doesn't exist"),
                    ReasonPhrase = "Student No. doesn't exist",
                    StatusCode = HttpStatusCode.SwitchingProtocols
                };
                //ErrorMessage = "Character is not allowed in Invoice number";
                //ErrorCode = (int)HttpStatusCode.Ambiguous;
                //ErrorMessage = "Invoice No. doesn't exist";
                //ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                throw new HttpResponseException(msg);
            }

            if (id != pmt.ID)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("student number in path param is different form request body"),
                    ReasonPhrase = "student number in path param is different form request body",
                    StatusCode = HttpStatusCode.Redirect
                };
                //ErrorMessage = "Invoice number in path param is different form request body";
                //ErrorCode = (int)HttpStatusCode.Redirect;
                throw new HttpResponseException(msg);
            }

            if (objbuss.Exists(pmt.ID))
            {
                r.ErrorCode = (int)HttpStatusCode.UpgradeRequired;
                r.ErrorMessage = "The Student Id has already been used!";
                r.IsSaved = false;          

                return false;
               
            }

           
            var result = objbuss.InserttblStudent(pmt);
            if (objbuss.InserttblStudent(pmt))
            {
                r.ErrorCode = 200;
                r.ErrorMessage = string.Empty;
                r.IsSaved = true;

            }

            else
            {
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(r.ErrorMessage),
                    ReasonPhrase = r.ErrorMessage,
                    StatusCode = (HttpStatusCode)r.ErrorCode
                };

                throw new HttpResponseException(msg);
            }
            return true;
        }

        /// <summary>
        ///     Gets a penalty record by payment order number (InvoiceNo) including Business Registration name
        /// </summary>
        /// <param name="InvoiceNo" - a Unique Payment Order Number for a penalty record.></param>
        /// <returns>A JSON Object representing an offense record  </returns>
        [Route("Api/GetStudent/{Id}")]
        public HttpResponseMessage GetStudent(string Id)
        {
            string ErrorMessage = "";
            int ErrorCode = 200;
            try
            {
                Result r = new Result();

                
                try
                {
                    int studentId = Convert.ToInt32(Id);
                }
                catch
                {
                    var msg = new HttpResponseMessage();
                    ErrorMessage = "Student No. doesn't exist";
                    ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                    //ErrorMessage = "Character is not allowed in Invoice number";
                    //ErrorCode = (int)HttpStatusCode.Ambiguous;
                    throw new HttpResponseException(msg);
                }
                var objStudent = new tblStudentBussiness();
                var studentDetail = objStudent.GettblStudent(Convert.ToInt32(Id));

             
                if (r.ErrorCode == 200)
                {
                    r.ErrorCode = 200;
                    r.ErrorMessage = string.Empty;
                    r.IsSaved = true;
                   
                }
                if (r.ErrorCode == 101)
                {
                    ErrorMessage = "Student Id. doesn't exist";
                    ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                    var msg = new HttpResponseMessage();
                    throw new HttpResponseException(msg);
                }
                var response = Request.CreateResponse(HttpStatusCode.OK, studentDetail);
                return response;
            }
            catch
            {
                var msg = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage),
                    ReasonPhrase = ErrorMessage,
                    StatusCode = (HttpStatusCode)ErrorCode
                };
                throw new HttpResponseException(msg);
            }
        }


        /// <summary>
        ///     Gets a penalty record by payment order number (InvoiceNo) including Business Registration name for ET-Sweach
        /// </summary>
        /// <param name="InvoiceNo" - a Unique Payment Order Number for a MOTI Transaction.></param>
        /// <param name="ETSRequestID" - a Unique Request ID Generated From ETSwitch.></param>
        /// <returns>A JSON Object representing an offense record  </returns>
        [Route("Api/GetStudentList")]
        public HttpResponseMessage GetStudentList(string InvoiceNo, string ETSRequestID)
        {
            string ErrorMessage = "";
            int ErrorCode = 200;
            try
            {
                Result r = new Result();
                try
                {
                    
                }
                catch
                {
                    var msg = new HttpResponseMessage();
                    //ErrorMessage = "Character is not allowed in Invoice number";
                    //ErrorCode = (int)HttpStatusCode.Ambiguous;
                    ErrorMessage = "";
                    ErrorCode = (int)HttpStatusCode.SwitchingProtocols;

                    throw new HttpResponseException(msg);
                }
                var objStudent = new tblStudentBussiness();
                var studentDetail = objStudent.GettblStudents();
                if (r.ErrorCode == 200)
                {
                    
                        ErrorMessage = "There is no student In the list!";
                        ErrorCode = (int)HttpStatusCode.Forbidden;
                        var msg = new HttpResponseMessage();
                        throw new HttpResponseException(msg);
                    
                }
                
                var response = Request.CreateResponse(HttpStatusCode.OK, studentDetail);
                return response;
            }
            catch
            {
                var msg = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage),
                    ReasonPhrase = ErrorMessage,
                    StatusCode = (HttpStatusCode)ErrorCode
                };
                throw new HttpResponseException(msg);
            }
        }

        [HttpDelete]
        [Route("Api/DeleteStudent/{Id}")]
        public HttpResponseMessage DeleteStudent(string Id)
        {
            string ErrorMessage = "";
            int ErrorCode = 200;
            try
            {
                Result r = new Result();


                try
                {
                    int studentId = Convert.ToInt32(Id);
                }
                catch
                {
                    var msg = new HttpResponseMessage();
                    ErrorMessage = "Student No. doesn't exist";
                    ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                    //ErrorMessage = "Character is not allowed in Invoice number";
                    //ErrorCode = (int)HttpStatusCode.Ambiguous;
                    throw new HttpResponseException(msg);
                }
                var objStudent = new tblStudentBussiness();
                var studentDetail = objStudent.Delete(Convert.ToInt32(Id));


                if (r.ErrorCode == 200)
                {
                    r.ErrorCode = 200;
                    r.ErrorMessage = string.Empty;
                    r.IsSaved = true;

                }
                if (r.ErrorCode == 101)
                {
                    ErrorMessage = "Student Id. doesn't exist";
                    ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                    var msg = new HttpResponseMessage();
                    throw new HttpResponseException(msg);
                }
                var response = Request.CreateResponse(HttpStatusCode.OK, studentDetail);
                return response;
            }
            catch
            {
                var msg = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage),
                    ReasonPhrase = ErrorMessage,
                    StatusCode = (HttpStatusCode)ErrorCode
                };
                throw new HttpResponseException(msg);
            }
        }
    }
}