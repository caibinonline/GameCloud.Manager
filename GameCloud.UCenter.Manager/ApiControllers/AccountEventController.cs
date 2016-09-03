﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GameCloud.Database.Adapters;
using GameCloud.UCenter.Common.Settings;
using GameCloud.UCenter.Database;
using GameCloud.UCenter.Database.Entities;
using GameCloud.UCenter.Web.Common.Modes;
using MongoDB.Driver;

namespace GameCloud.UCenter.Manager.ApiControllers
{
    /// <summary>
    /// Provide a controller for users.
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/accountEvents")]
    public class AccountEventsController : ApiControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventsController" /> class.
        /// </summary>
        /// <param name="database">Indicating the database context.</param>
        /// <param name="settings">Indicating the settings.</param>
        [ImportingConstructor]
        public AccountEventsController(UCenterDatabaseContext database, Settings settings)
            : base(database, settings)
        {
        }

        /// <summary>
        /// Get user list.
        /// </summary>
        /// <param name="token">Indicating the cancellation token.</param>
        /// <param name="keyword">Indicating the keyword.</param>
        /// <param name="orderby">Indicating the order by name.</param>
        /// <param name="page">Indicating the page number.</param>
        /// <param name="count">Indicating the count.</param>
        /// <returns>Async return user list.</returns>
        public async Task<PaginationResponse<AccountEventEntity>> Get(
            CancellationToken token,
            [FromUri] string keyword = null,
            [FromUri] string orderby = null,
            [FromUri] int page = 1,
            [FromUri] int count = 1000)
        {
            //Expression<Func<AccountEventEntity, bool>> filter = null;

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    filter = a => a.AccountName.Contains(keyword);
            //}

            //var total = await this.Database.AccountEvents.CountAsync(filter, token);

            //IQueryable<AccountEventEntity> queryable = this.Database.AccountEvents.Collection.AsQueryable();
            //if (filter != null)
            //{
            //    queryable = queryable.Where(filter);
            //}
            //queryable = queryable.OrderByDescending(a => a.CreatedTime);

            //var result = queryable.Skip((page - 1) * count).Take(count).ToList();

            //// todo: add orderby support.
            //var model = new PaginationResponse<AccountEventEntity>
            //{
            //    Page = page,
            //    PageSize = count,
            //    Raws = result,
            //    Total = total
            //};

            //return model;
            return null;
        }

        /// <summary>
        /// Get single user details.
        /// </summary>
        /// <param name="id">Indicating the user id.</param>
        /// <param name="token">Indicating the cancellation token.</param>
        /// <returns>Async return user details.</returns>
        public async Task<AccountEventEntity> Get(string id, CancellationToken token)
        {
            //var result = await this.Database.AccountEvents.GetSingleAsync(id, token);

            //return result;
            return null;
        }
    }
}
