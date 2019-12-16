using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bijector.Infrastructure.Queues;
using Bijector.Infrastructure.Repositories;
using Bijector.Infrastructure.Types;
using Bijector.Infrastructure.Types.Messages;
using Bijector.Workflows.Models;
using Newtonsoft.Json.Linq;

namespace Bijector.Workflows.Executor
{
    public class WorkflowExecutor : IWorkflowExecutor
    {
        private readonly IRepository<Workflow> workflowsRepository;

        private readonly IPublisher publisher;

        public WorkflowExecutor(IRepository<Workflow> workflowsRepository, IPublisher publisher)
        {
            this.workflowsRepository = workflowsRepository;
            this.publisher = publisher;
        }

        public async Task<Workflow> GetWorkflow(int id)
        {
            return await workflowsRepository.GetByIdAsync(id);
        }

        public async Task<bool> Handle(IEvent @event, IContext context)
        {
            if(context.Id == -1)
            {
                await AttemtStart(@event, context);
                return true;
            }                

            var workflow = await workflowsRepository.GetByIdAsync(context.Id);
            if(workflow == null)
                return false;
            var node = workflow.WorkflowNodes.Single(w => w.Id == workflow.CurrentNodeId);
            
            var index = workflow.WorkflowNodes.IndexOf(node);
            var nextIndex = 0;
            if(workflow.WorkflowNodes.Count > index + 1)
            {
                nextIndex++;
            }                
            var nextNodeId = workflow.WorkflowNodes[nextIndex].Id;
            
            int? nextId = null;
            JObject nextParameters = null;
            (nextId, nextParameters) =  node.HandleRequest(@event, nextNodeId);

            if(nextId.HasValue)
            {                
                workflow.CurrentNodeId = nextId.Value;
                                
                var nextNode = workflow.WorkflowNodes.Single(w => w.Id == workflow.CurrentNodeId);
                                
                if(workflow.WorkflowNodes.IndexOf(nextNode) == 0)
                {
                    workflow.State = WorkflowState.Success;
                }
                else
                {
                    var newContext = new BaseContext(context.Id, workflow.AccountId, "Bijector Workflows", nextNode.ServiceName);
                    await nextNode.Execute(newContext,publisher,nextParameters);                                       
                }
                await workflowsRepository.UpdateAsync(context.Id, workflow);
                return true;             
            }                       
            else
            {
                workflow.State = WorkflowState.Failed;
                await workflowsRepository.UpdateAsync(context.Id, workflow);
                return false;
            }            
        }

        private async Task AttemtStart(IEvent @event, IContext context)
        {
            IEnumerable<Workflow> workflows = null;
            if(context.UserId == -1)
                workflows = await workflowsRepository.FilterAsync(w => (w.State == WorkflowState.NonExecuted || w.State == WorkflowState.Success));
            else
                workflows = await workflowsRepository.FilterAsync(w => (w.State == WorkflowState.NonExecuted || w.State == WorkflowState.Success) && w.AccountId == context.UserId);
            
            foreach (var workflow in workflows)
            {
                var startNode = (workflow.WorkflowNodes.Single(w => w.Id == workflow.CurrentNodeId) as IStartWorkflowNode);
                var nextNodeId = workflow.WorkflowNodes[1].Id;            
                int? nextId = null;
                JObject nextParameters = null;
                (nextId, nextParameters) =  startNode.HandleRequest(@event, nextNodeId);
                if(nextId.HasValue)
                {
                    workflow.CurrentNodeId = nextId.Value;
                    workflow.State = WorkflowState.Running;
                    var nextNode = workflow.WorkflowNodes.Single(w => w.Id == workflow.CurrentNodeId);
                    var newContext = new BaseContext(workflow.Id, workflow.AccountId, "Bijector Workflows", nextNode.ServiceName);
                    await nextNode.Execute(newContext,publisher,nextParameters);                    
                }
                await workflowsRepository.UpdateAsync(workflow.Id, workflow);
            }
        }
    }
}