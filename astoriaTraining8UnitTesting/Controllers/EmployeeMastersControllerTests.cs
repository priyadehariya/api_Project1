using Microsoft.VisualStudio.TestTools.UnitTesting;
using astoriaTrainingAPI_.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using astoriaTrainingAPI_.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace astoriaTrainingAPI_.Controllers.Tests
{
    [TestClass()]
    public class EmployeeMastersControllerTests
    {
        private readonly astoriaTraining80Context _context;
        public EmployeeMastersControllerTests()
        {
            var optionBuilder = new DbContextOptionsBuilder<astoriaTraining80Context>( );
           // optionBuilder.UseSqlServer("Server=priyadb.database.windows.net;Database=astoriaTraining8.0LT2022;User Id=PriyaSQL;Password=Priya@123;");
            optionBuilder.UseSqlServer("Server=ASTORIA-LT102;Database=astoriaTraining8.0LT2022;User Id=sa;Password=Pass123;");
            _context = new astoriaTraining80Context(optionBuilder.Options);
        }

        #region Unit Test Method of GetAllEmployees API

        [TestMethod()]
        public void GetEmployees_MatchCount_ReturnsOk_Test()
        {
            //Arrange

            int expectedAllEmployeeCount = 6 ;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetEmployeeMasterCount();

            //Assert
            Assert.AreEqual(expectedAllEmployeeCount, apiResult);
        }

        [TestMethod()]
        public void Test_GetEmployees_Returns_OkResult()
        {
            //Arrange

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetEmployeeMasterList();

            //Assert
            Assert.IsInstanceOfType(apiResult.Result.Result ,typeof(OkResult));
        }



        [TestMethod()]
        public void GetEmployees_ReturnsNoContent_Test()
        {
            //Arrange
            var noContentResult = typeof(NoContentResult);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetEmployees();
            var resultType = apiResult.Result.Result;
            //Assert
            Assert.AreEqual(resultType, noContentResult);
        }
        #endregion

        #region Unit TestMethod of GetEmployeeById API

        [TestMethod()]
        public void GetEmployeeMaster_ValidEmployeeKey_MatchCount_Test()
        {
            //Arrange
            long empKeyInput = 43;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeMaster(empKeyInput);
            var resultList = ((OkObjectResult)result.Result.Result).Value as EmployeeMaster;
            //Assert
           
            Assert.AreEqual(empKeyInput, resultList.EmployeeKey);
        }

        [TestMethod()]
        public void GetEmployeeMaster_ValidEmployeeKey_ReturnOkResult_Test()
        {
            //Arrange

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeMasterList();
            //  Assert

            Assert.IsInstanceOfType(result.Result.Result,typeof(OkResult));
        }

        [TestMethod()]
        public void GetEmployeeMaster_InValidEmpKey_ReturnNotFoundResult_Test()
        {
            //Arrange
            long empKeyInput = 13;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeMaster(empKeyInput);
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(NotFoundResult));
        }


        #endregion
        #region Test Method of PostEmployeeMaster API

        [TestMethod()]
        public void PostEmployeeMaster_ValidData_ReturnOKResult_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL-216";
            objEmployee.EmpFirstName = "Gaurav";
            objEmployee.EmpLastName = "dehariya";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpCompanyId = 3;
            objEmployee.EmpDesignationId = 2;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);
            var employeeResult = result.Result;
            var resultObject = employeeResult;
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void PostEmployeeMaster_DuplicateEmpID_ReturnConflict_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 20021;
            objEmployee.EmployeeId = "ATIL-198";
            objEmployee.EmpFirstName = "vijji";
            objEmployee.EmpLastName = " P ";
            objEmployee.EmpGender = "Female";
            objEmployee.EmpCompanyId = 3;
            objEmployee.EmpDesignationId = 1;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(StatusCodeResult));
        }

        [TestMethod()]
        public void PostEmployeeMaster_EmptyColumn_ReturnBadRequestResult_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "";
            objEmployee.EmpFirstName = " ";
            objEmployee.EmpLastName = " ";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpCompanyId = 1;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PostEmployeeMaster_InvalidEmpIdLength_ReturnBadRequestResult_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL-1986677789898767454356557";
            objEmployee.EmpFirstName = "Priyanshu";
            objEmployee.EmpLastName = "Alawa";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpCompanyId = 1;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.EmpJoiningDate = DateTime.Now.Date;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PostEmployeeMaster_InvalidDate_ReturnBadRequest_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL-10";
            objEmployee.EmpFirstName = "Priyansh";
            objEmployee.EmpLastName = "Alawa";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpCompanyId = 1;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.EmpJoiningDate = new DateTime (2022,11,12);
            objEmployee.EmpResignationDate = new DateTime(2022,11,11);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(BadRequestObjectResult));
        }

        #endregion
        #region Test Method of GetAllcompanies method API

        [TestMethod()]
        public void GetCompanyMaster_MatchCount_ReturnsOk_Test()
        {
            //Arrange
            int expectedAllCompanyCount = 3;

            //Action
            var objCompanyMasterController = new EmployeeMastersController(_context);
            var apiResult = objCompanyMasterController.GetCompanyMaster();
            var resultList = apiResult.Result.Value as List<CompanyMaster>;
            int resultCount = resultList.Count;

            //Assert
            Assert.AreEqual(expectedAllCompanyCount, resultCount);
        }

        [TestMethod()]
        public void GetCompanyMaster_ReturnsNoContent_Test()
        {
            //Arrange
            var noContentResult = typeof(NoContentResult);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetCompanyMaster();
            var resultType = apiResult.Result.Result;
            //Assert
            Assert.AreEqual(resultType, noContentResult);
        }

        #endregion

        #region Test Method of GetAlldesignation method API

        [TestMethod()]
        public void GetDesignationMaster_MatchCount_ReturnsOk_Test()
        {
            //Arrange
            int expectedAllDesignationCount = 5;

            //Action
            var objDesignationMasterController = new EmployeeMastersController(_context);
            var apiResult = objDesignationMasterController.GetDesignationMaster();
            var resultList = apiResult.Result.Value as List<DesignationMaster>;
            int resultCount = resultList.Count;

            //Assert
            Assert.AreEqual(expectedAllDesignationCount, resultCount);
        }

        [TestMethod()]
        public void GetDesignationMaster_ReturnsNoContent_Test()
        {
            //Arrange
            var noContentResult = typeof(NoContentResult);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetDesignationMaster();
            var resultType = apiResult.Result.Result;
            //Assert
            Assert.AreEqual(resultType, noContentResult);
        }

        #endregion

        #region Test Method of PutEmployeeMaster API
        [TestMethod()]
        public void PutEmployeeMaster_ValidInput_ReturnOkResult_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 43;
            objEmployee.EmployeeId = "ATIL-204";
            objEmployee.EmpFirstName = "Priya";
            objEmployee.EmpLastName = "Dehariya";
            objEmployee.EmpGender = "Female";
            objEmployee.EmpCompanyId = 1;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.CreationDate = DateTime.Now;
            objEmployee.EmpJoiningDate = DateTime.Now;
        
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey,objEmployee);
            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_InvalidLength_ReturnBadRequest_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 3;
            objEmployee.EmployeeId = "ATIL-1986677789898767454356557hg";
            objEmployee.EmpFirstName = "Priyanshu";
            objEmployee.EmpLastName = "Alawa";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpCompanyId = 1;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.CreationDate = DateTime.Now;
            objEmployee.EmpJoiningDate = DateTime.Now;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);
            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_EmptyInput_ReturnBadRequest_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 43;
            objEmployee.EmployeeId = "";
            objEmployee.EmpFirstName = "a";
            objEmployee.EmpLastName = "b";
            objEmployee.EmpGender = "Female";
            objEmployee.EmpCompanyId = 1;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.CreationDate = DateTime.Now;
            objEmployee.EmpJoiningDate = DateTime.Now;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);
            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_DuplicateInput_ReturnConflict_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 43;
            objEmployee.EmployeeId = "ATIL-200";
            objEmployee.EmpFirstName = "Priya";
            objEmployee.EmpLastName = "Dehariya";
            objEmployee.EmpGender = "Female";
            objEmployee.EmpCompanyId = 1;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.CreationDate = DateTime.Now;
            objEmployee.EmpJoiningDate = DateTime.Now;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);
            //Assert
          
            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_InValidEmployeeKey_ReturnNotFound_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 1733;
            objEmployee.EmployeeId = "ATIL-200";
            objEmployee.EmpFirstName = "Priya";
            objEmployee.EmpLastName = "Dehariya";
            objEmployee.EmpGender = "Female";
            objEmployee.EmpCompanyId = 1;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.CreationDate = DateTime.Now;
            objEmployee.EmpJoiningDate = DateTime.Now;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);
            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
        [TestMethod()]
        public void PutEmployeeMaster_InvalidDate_ReturnBadRequest_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeKey = 43;
            objEmployee.EmployeeId = "ATIL-200";
            objEmployee.EmpFirstName = "Priya";
            objEmployee.EmpLastName = "Dehariya";
            objEmployee.EmpGender = "Female";
            objEmployee.EmpCompanyId = 1;
            objEmployee.EmpDesignationId = 4;
            objEmployee.EmpHourlySalaryRate = 500;
            objEmployee.CreationDate = DateTime.Now;
            objEmployee.EmpJoiningDate = new DateTime(2022,11,12);
            objEmployee.EmpResignationDate = new DateTime(2022,11,11);
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);
            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }
        #endregion
        #region Test Method of DeleteEmployee method API

        [TestMethod()]
        public void DeleteEmployeeMaster_InValidInput_ReturnsNotFound_Test()
        {
            //Arrange
            long empKeyInput = 1;
            //Action
            var objDeleteEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objDeleteEmployeeMasterController.DeleteEmployeeMaster(empKeyInput);
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(NotFoundResult));
        }
        [TestMethod()]
        public void DeleteEmployeeMaster_ValidInput_ReturnsOkResult_Test()
        {
            //Arrange
            long empKeyInput = 30019;
            //Action
            var objDeleteEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objDeleteEmployeeMasterController.DeleteEmployeeMaster(empKeyInput);
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(OkObjectResult));
        }
        [TestMethod()]
        public void DeleteEmployeeMaster_CheckEmployeeInUse_ReturnsStatus409Conflict_Test()
        {
            //Arrange
            long empKeyInput = 43;
            //Action
            var objDeleteEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objDeleteEmployeeMasterController.DeleteEmployeeMaster(empKeyInput);
            var empResult = result.Result.Value;
            //Assert
            Assert.IsInstanceOfType(result.Result.Result,typeof(ConflictObjectResult));
        }
        #endregion

        [TestMethod()]
        public void PostUserInfo_validDate_ReturnOkObject_Test()
        {
            //Arrange
            var objEmployee = new UserInfo();
            objEmployee.UserId = 2;
            objEmployee.FirstName = "megha";
            objEmployee.LastName = "Ahirwar";
            objEmployee.UserName = "megha123";
            objEmployee.EmailId = "megha@gmail.com";
            objEmployee.Password = "1234";
            objEmployee.CreationDate = new DateTime(2022, 11, 12);
            //Action
            var objUserInfoesController = new UserInfoesController(_context);
            var result = objUserInfoesController.PostUserInfo(objEmployee);
            var employeeResult = result.Result;
            var resultObject = employeeResult;
            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(OkObjectResult));
        }

    }
}