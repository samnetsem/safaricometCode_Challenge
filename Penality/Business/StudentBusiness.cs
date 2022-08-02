using Safari.DataAccess;
using Safari.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;


namespace Safari.Bussiness
{
    public class tblStudentBussiness
    {


        public bool Delete(int ID)
        {
            tblStudentDataAccess objtblStudentDataAccess = new tblStudentDataAccess();
            try
            {
                objtblStudentDataAccess.Delete(ID);
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public tblStudent GettblStudent(int ID)
        {
            tblStudentDataAccess objtblStudentDataAccess = new tblStudentDataAccess();

            try
            {
                return objtblStudentDataAccess.GetRecord(ID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public List<tblStudent> GettblStudents()
        {
            tblStudentDataAccess objtblStudentDataAccess = new tblStudentDataAccess();

            try
            {
                return objtblStudentDataAccess.GetList();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public bool Exists(int ID)
        {
            tblStudentDataAccess objtblStudentDataAccess = new tblStudentDataAccess();
            try
            {
                return objtblStudentDataAccess.Exists(ID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public bool UpdatetblStudent(tblStudent objtblStudent)
        {
            tblStudentDataAccess objtblStudentDataAccess = new tblStudentDataAccess();
            try
            {
                objtblStudentDataAccess.Update(objtblStudent);
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public bool InserttblStudent(tblStudent objtblStudent)
        {
            tblStudentDataAccess objtblStudentDataAccess = new tblStudentDataAccess();
            try
            {
                objtblStudentDataAccess.Insert(objtblStudent);
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


    }
}