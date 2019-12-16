using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bijector.Infrastructure.Repositories;
using Bijector.Infrastructure.Types;
using Bijector.Workflows.Executor;
using Bijector.Workflows.Messages.Commands;
using Bijector.Workflows.Messages.Events;
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var accountId = int.Parse(User.Identity.Name);
            var list = await workflows.FilterAsync(w => w.AccountId == accountId);
            return new JsonResult(list);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateWorkflow([FromBody]GenerateWorkflow command)
        {
            var accountId = int.Parse(User.Identity.Name);  

            var workflow = new Workflow();
            workflow.AccountId = accountId;
            workflow.Name = command.Name;
            workflow.Description = command.Description;
            workflow.State = WorkflowState.NonExecuted;                        
            workflow.WorkflowNodes = new List<IWorkflowNode>();

            foreach (var node in command.WorkflowNodes)
            {
                var nodeType = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                   from type in asm.GetTypes()
                   where type.IsClass && type.Name == node.Type
                   select type).Single();
                var actionType = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                   from type in asm.GetTypes()
                   where type.IsClass && type.Name == node.ActionType
                   select type).Single();
                
                dynamic workflowNode = nodeType.GetConstructor( new Type[]{} ).Invoke(null);
                workflowNode.Id = node.Id;
                workflowNode.ServiceName = node.ServiceName;
                workflowNode.Action = JsonConvert.DeserializeObject(node.ActionJson, actionType);
                
                workflow.WorkflowNodes.Add(workflowNode);
            }            

            await workflows.AddAsync(workflow);
            return Accepted();
        }

        [Authorize]
        [HttpPost("ForceStart")]
        public async Task<IActionResult> ForceStart(int id)
        {
            var context = new BaseContext(id, int.Parse(User.Identity.Name), "Bijector Workflows", "Bijector Workflows");
            var @event = new ForceStartEvent();
            if(await executor.Handle(@event, context))
                return Accepted();
            return BadRequest();
        }

        [Authorize]
        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateWorkflow([FromBody]UpdateWorkflow command)
        {
            var accountId = int.Parse(User.Identity.Name);  
            
            var oldWorkflow = await workflows.GetByIdAsync(command.Id);
            if(oldWorkflow == null || oldWorkflow.AccountId != accountId)
                return BadRequest();

            var workflow = new Workflow();
            workflow.Id = command.Id;
            workflow.AccountId = accountId;
            workflow.Name = command.Name;
            workflow.Description = command.Description;
            workflow.State = WorkflowState.NonExecuted;                        
            workflow.WorkflowNodes = new List<IWorkflowNode>();

            foreach (var node in command.WorkflowNodes)
            {
                var nodeType = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                   from type in asm.GetTypes()
                   where type.IsClass && type.Name == node.Type
                   select type).Single();
                var actionType = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                   from type in asm.GetTypes()
                   where type.IsClass && type.Name == node.ActionType
                   select type).Single();
                
                dynamic workflowNode = nodeType.GetConstructor( new Type[]{} ).Invoke(null);
                workflowNode.Id = node.Id;
                workflowNode.ServiceName = node.ServiceName;
                workflowNode.Action = JsonConvert.DeserializeObject(node.ActionJson, actionType);
                
                workflow.WorkflowNodes.Add(workflowNode);
            }            

            await workflows.UpdateAsync(workflow.Id, workflow);
            return Accepted();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteWorkflow(int id)
        {
            var accountId = int.Parse(User.Identity.Name);  
            
            var oldWorkflow = await workflows.GetByIdAsync(id);
            if(oldWorkflow == null || oldWorkflow.AccountId != accountId)
                return BadRequest();

            await workflows.RemoveAsync(id);
            return Accepted();
        }
    }
}