using System.Collections.Generic;
using System.Threading.Tasks;
using Bijector.Infrastructure.Repositories;
using Bijector.Workflows.Executor;
using Bijector.Workflows.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bijector.Workflows.Controllers
{
    [Route("Workflows")]
    public class WorkflowsController : Controller
    {
        private readonly IWorkflowExecutor executor;

        private readonly IRepository<Workflow> workflows;

        public WorkflowsController(IWorkflowExecutor executor, IRepository<Workflow> workflows)
        {
            this.executor = executor;
            this.workflows = workflows;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateWorkflow()
        {
            return Accepted();
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<string> GetAll()
        {
            var accountId = int.Parse(User.Identity.Name);
            var list = await workflows.FilterAsync(w => w.AccountId == accountId);
            return JsonConvert.SerializeObject(list);
        }

        [Authorize]
        [HttpGet("Generate")]
        public async Task<IActionResult> GenerateSimpleWorkflow()
        {
            var accountId = int.Parse(User.Identity.Name);
            var workflow = new Workflow();
            workflow.AccountId = accountId;
            workflow.State = WorkflowState.NonExecuted;
            
            var workflowNode1 = new TimeStartWorkflowNode(System.DateTimeOffset.Now.AddSeconds(30));
            workflowNode1.ServiceName = "Bijector Workflows";

            var command = new Bijector.Workflows.Messages.Commands.RenameDriveEntity(0, "1ucttWbsmfAhKccKYkNXMSY3ucFe8-G6h", $"{System.DateTimeOffset.Now.Minute}");
            var workflowNode2 = new CommandWorkflowNode(command);
            workflowNode2.ServiceName = "Bijector GDrive";

            workflow.WorkflowNodes = new List<IWorkflowNode>
            {
                workflowNode1,
                workflowNode2
            };
            await workflows.AddAsync(workflow);
            return new JsonResult(workflow);
        }
    }
}