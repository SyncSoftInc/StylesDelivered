using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.ECP.Commands.Service;
using SyncSoft.ECP.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class ServiceController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IScheduler> _lazyScheduler = ObjectContainer.LazyResolve<IScheduler>();
        private IScheduler Scheduler => _lazyScheduler.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Trigger  -

        /// <summary>
        /// Trigger Job
        /// </summary>
        [HttpPost("api/service/job")]
        public async Task<string> TriggerJob(TriggerJobCommand cmd)
        {
            var jobKey = new JobKey(cmd.Name, cmd.GroupName);

            await Scheduler.TriggerJob(jobKey).ConfigureAwait(false);
            return MsgCodes.SUCCESS;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Get  -

        /// <summary>
        /// GetJobs
        /// </summary>
        [HttpGet("api/service/jobs")]
        public async Task<IList<JobDTO>> GetJobs()
        {
            var groupNames = await Scheduler.GetJobGroupNames().ConfigureAwait(false);
            var jobs = new List<JobDTO>();

            foreach (var groupName in groupNames)
            {
                var jobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)).ConfigureAwait(false);

                foreach (var jobKey in jobKeys)
                {
                    var jobDetail = await Scheduler.GetJobDetail(jobKey).ConfigureAwait(false);
                    var triggers = await Scheduler.GetTriggersOfJob(jobKey).ConfigureAwait(false);

                    var job = new JobDTO
                    {
                        Name = jobKey.Name,
                        GroupName = jobKey.Group,
                        TypeName = jobDetail.JobType.FullNameWithAssembly(),
                        TriggersCount = triggers.Count
                    };
                    jobs.Add(job);
                }
            }

            return jobs;
        }

        #endregion
    }
}
