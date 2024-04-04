using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RumbApp.Models.Domain.Comments;
using RumbApp.Models.Requests.Comment;
using RumbApp.Models.Requests.Comments;
using RumbApp.Services;
using RumbApp.Services.Interfaces;
using RumbApp.Web.Controllers;
using RumbApp.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RumbApp.Web.Api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentApiController : BaseApiController
    {
        private ICommentService _service = null;
        private IAuthenticationService<int> _authService = null;

        public CommentApiController(ICommentService service, IAuthenticationService<int> authService, ILogger<CommentApiController> logger) : base(logger) 
        {
            _service = service;
            _authService = authService;
        }
        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(CommentAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);

                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(CommentUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                 int userId = _authService.GetCurrentUserId();
                response = new SuccessResponse();
                _service.Update(model, userId);
                 
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }


        [HttpPut("delete/{id:int}")]
        public ActionResult<SuccessResponse> Delete(int Id)
        {
            _service.Delete(Id);
            SuccessResponse response = new SuccessResponse();
            return Ok(response);
        }

        [HttpGet("{EntityId:int}")]
        public ActionResult<ItemsResponse<Comment>> GetByEntityId(int EntityId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
               List<Comment> list = _service.GetByEntityId(EntityId);
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found");
                }
                else
                {
                    response = new ItemsResponse<Comment> { Items = list };
                }
            }
            catch (SqlException sqlEx)
            {
                code = 500;
                response = new ErrorResponse($"SqlException Errors: {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());
            }
            catch (ArgumentException argEx)
            {
                code = 500;
                response = new ErrorResponse($"ArgumentException Errors: {argEx.Message}");
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Generic Errors: {ex.Message}");
                base.Logger.LogError(ex.ToString());
            };
            return StatusCode(code, response);
        }

    }
}
