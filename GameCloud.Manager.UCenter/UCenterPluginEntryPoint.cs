﻿using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GameCloud.Database;
using GameCloud.Database.Adapters;
using GameCloud.Manager.Contract;
using GameCloud.Manager.Contract.Attributes;
using GameCloud.Manager.Contract.Requests;
using GameCloud.Manager.Contract.Responses;
using GameCloud.Manager.UCenter.Models;
using GameCloud.UCenter.Common.MEF;
using GameCloud.UCenter.Database;
using GameCloud.UCenter.Database.Entities;
using MongoDB.Driver;

namespace GameCloud.Manager.UCenter
{
    [PluginMetadata(Name = "ucenter", DisplayName = "UCenter管理平台", Description = "This is a demo plugin.")]
    [PluginCategoryMetadata(Name = "events", DisplayName = "事件查看", Description = "This is demo collection 1")]
    [PluginCategoryMetadata(Name = "player-analytics", DisplayName = "玩家分析", Description = "This is demo collection 1")]
    [PluginCategoryMetadata(Name = "online-analytics", DisplayName = "在线分析", Description = "This is demo collection 2")]
    public class UCenterPluginEntryPoint : PluginEntryPoint
    {
        private readonly ExportProvider exportProvider;
        private readonly UCenterDatabaseContext database;

        public UCenterPluginEntryPoint()
        {
            this.exportProvider = CompositionContainerFactory.Create();
            var dbSettings = this.exportProvider.GetExportedValue<DatabaseContextSettings>();
            dbSettings.ConnectionString = this.Configuration.GetSettingValue<string>("ConnectionString");
            dbSettings.DatabaseName = this.Configuration.GetSettingValue<string>("DatabaseName");

            this.database = this.exportProvider.GetExportedValue<UCenterDatabaseContext>();
        }

        [PluginItemMetadata(Name = "app-search", DisplayName = "App管理", Type = PluginItemType.List)]
        public async Task<PluginPaginationResponse<AppEntity>> GetApps(PluginRequestInfo request)
        {
            Expression<Func<AppEntity, bool>> filter = null;
            string keyword = request.GetParameterValue<string>("keyword");
            int page = request.GetParameterValue<int>("page", 1);
            int count = request.GetParameterValue<int>("pageSize", 10);
            if (!string.IsNullOrEmpty(keyword))
            {
                filter = a => a.Name.Contains(keyword);
            }

            var total = await this.database.Apps.CountAsync(filter, CancellationToken.None);

            IQueryable<AppEntity> queryable = this.database.Apps.Collection.AsQueryable();
            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            var result = queryable.Skip((page - 1) * count).Take(count).ToList();

            // todo: add orderby support.
            var model = new PluginPaginationResponse<AppEntity>
            {
                Page = page,
                PageSize = count,
                Raws = result,
                Total = total
            };

            return model;
        }

        [PluginItemMetadata(Name = "account-event-search", Category = "events", DisplayName = "玩家事件", Type = PluginItemType.List)]
        public async Task<PluginPaginationResponse<AccountEventEntity>> GetAccountEvents(PluginRequestInfo request)
        {
            //Expression<Func<AccountEventEntity, bool>> filter = null;
            //string keyword = request.GetParameterValue<string>("keyword");
            //int page = request.GetParameterValue<int>("page", 1);
            //int count = request.GetParameterValue<int>("pageSize", 10);
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    filter = a => a.AccountName.Contains(keyword);
            //}

            //var total = await this.database.AccountEvents.CountAsync(filter, CancellationToken.None);

            //IQueryable<AccountEventEntity> queryable = this.database.AccountEvents.Collection.AsQueryable();
            //if (filter != null)
            //{
            //    queryable = queryable.Where(filter);
            //}

            //var result = queryable.Skip((page - 1) * count).Take(count).ToList();

            //// todo: add orderby support.
            //var model = new PluginPaginationResponse<AccountEventEntity>
            //{
            //    Page = page,
            //    PageSize = count,
            //    Raws = result,
            //    Total = total
            //};

            //return model;
            return null;
        }

        [PluginItemMetadata(Name = "error-event-search", Category = "events", DisplayName = "错误事件", Type = PluginItemType.List)]
        public async Task<PluginPaginationResponse<ErrorEventEntity>> GetErrorEvents(PluginRequestInfo request)
        {
            //Expression<Func<ErrorEventEntity, bool>> filter = null;
            //string keyword = request.GetParameterValue<string>("keyword");
            //int page = request.GetParameterValue<int>("page", 1);
            //int count = request.GetParameterValue<int>("pageSize", 10);
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    filter = a => a.AccountName.Contains(keyword);
            //}

            //var total = await this.database.ErrorEvents.CountAsync(filter, CancellationToken.None);

            //IQueryable<ErrorEventEntity> queryable = this.database.ErrorEvents.Collection.AsQueryable();
            //if (filter != null)
            //{
            //    queryable = queryable.Where(filter);
            //}

            //var result = queryable.Skip((page - 1) * count).Take(count).ToList();

            //// todo: add orderby support.
            //var model = new PluginPaginationResponse<ErrorEventEntity>
            //{
            //    Page = page,
            //    PageSize = count,
            //    Raws = result,
            //    Total = total
            //};

            //return model;
            return null;
        }

        [PluginItemMetadata(Name = "account-search", DisplayName = "账号管理", Type = PluginItemType.List)]
        public async Task<PluginPaginationResponse<AccountEntity>> GetAccounts(PluginRequestInfo request)
        {
            var accountId = request.GetParameterValue<string>("accountId");
            var keyword = request.GetParameterValue<string>("keyword");
            var page = request.GetParameterValue<int>("page", 1);
            var count = request.GetParameterValue<int>("pageSize", 10);

            Expression<Func<AccountEntity, bool>> filter = null;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter = a => a.AccountName.Contains(keyword)
                    || a.Email.Contains(keyword)
                    || a.Phone.Contains(keyword);
            }

            var total = await this.database.Accounts.CountAsync(filter, CancellationToken.None);

            IQueryable<AccountEntity> queryable = this.database.Accounts.Collection.AsQueryable();
            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            var result = queryable.Skip((page - 1) * count).Take(count).ToList();

            // todo: add orderby support.
            var model = new PluginPaginationResponse<AccountEntity>
            {
                Page = page,
                PageSize = count,
                Raws = result,
                Total = total
            };

            return model;
        }

        [PluginItemMetadata(Name = "realtime-glance", DisplayName = "实时状况", Type = PluginItemType.Report)]
        public Task<UCenterStatisticsInfo> GetRealtimeGlance(PluginRequestInfo request)
        {
            return Task.FromResult<UCenterStatisticsInfo>(this.CreateSampleStatisticsInfo());
        }

        [PluginItemMetadata(Name = "new-users", Category = "player-analytics", DisplayName = "新增玩家", Type = PluginItemType.Report)]
        public Task<UCenterStatisticsInfo> GetNewUsers(PluginRequestInfo request)
        {
            return Task.FromResult<UCenterStatisticsInfo>(this.CreateSampleStatisticsInfo());
        }

        [PluginItemMetadata(Name = "active-users", Category = "player-analytics", DisplayName = "活跃玩家", Type = PluginItemType.Report)]
        public Task<UCenterStatisticsInfo> GetActiveUsers(PluginRequestInfo request)
        {
            return Task.FromResult<UCenterStatisticsInfo>(this.CreateSampleStatisticsInfo());
        }

        [PluginItemMetadata(Name = "stay-statistics", Category = "player-analytics", DisplayName = "留存统计", Type = PluginItemType.Report)]
        public Task<UCenterStatisticsInfo> GetStayStatistics(PluginRequestInfo request)
        {
            return Task.FromResult<UCenterStatisticsInfo>(this.CreateSampleStatisticsInfo());
        }

        [PluginItemMetadata(Name = "lost-statistics", Category = "player-analytics", DisplayName = "流失统计", Type = PluginItemType.Report)]
        public Task<UCenterStatisticsInfo> GetLostStatistics(PluginRequestInfo request)
        {
            return Task.FromResult<UCenterStatisticsInfo>(this.CreateSampleStatisticsInfo());
        }

        [PluginItemMetadata(Name = "online-analytics", Category = "online-analytics", DisplayName = "在线分析", Type = PluginItemType.Report)]
        public Task<UCenterStatisticsInfo> GetOnlineAnalytics(PluginRequestInfo request)
        {
            return Task.FromResult<UCenterStatisticsInfo>(this.CreateSampleStatisticsInfo());
        }

        [PluginItemMetadata(Name = "online-behaviour", Category = "online-behaviour", DisplayName = "在线习惯", Type = PluginItemType.Report)]
        public Task<UCenterStatisticsInfo> GetOnlineBehaviour(PluginRequestInfo request)
        {
            return Task.FromResult<UCenterStatisticsInfo>(this.CreateSampleStatisticsInfo());
        }

        private UCenterStatisticsInfo CreateSampleStatisticsInfo()
        {
            UCenterStatisticsInfo info = new UCenterStatisticsInfo();
            info.FirstPlay = new StatisticsData()
            {
                Labels = new string[] { "6PM", "7PM", "8PM", "9PM", "10PM", "11PM", "12PM" },
                Datas = this.RandomNumbers(7),
            };

            info.NewUserSex = new StatisticsData()
            {
                Labels = new string[] { "男", "女" },
                Datas = this.RandomNumbers(2),
            };

            info.ActiveUserSex = new StatisticsData()
            {
                Labels = new string[] { "男", "女" },
                Datas = this.RandomNumbers(2),
            };

            info.NewUserAge = new StatisticsData()
            {
                Labels = new string[] { "20-30", "30-40", "40-50", "50-60" },
                Datas = new double[] { 55, 5, 15, 15, 5 }
            };

            info.ActiveUserAge = new StatisticsData()
            {
                Labels = new string[] { "20-30", "30-40", "40-50", "50-60" },
                Datas = new double[] { 55, 5, 15, 15, 5 }
            };

            info.HourActiveDevices = new StatisticsData()
            {
                Labels = ParallelEnumerable.Range(0, 24).Select(i => i.ToString()).ToArray(),
                Series = new string[] { "小时设备激活" },
                Datas = this.RandomNumbers(24),
            };

            info.HourNewUsers = new StatisticsData()
            {
                Labels = ParallelEnumerable.Range(0, 24).Select(i => i.ToString()).ToArray(),
                Series = new string[] { "小时新增用户" },
                Datas = this.RandomNumbers(24),
            };

            info.HourNewDevices = new StatisticsData()
            {
                Labels = ParallelEnumerable.Range(0, 24).Select(i => i.ToString()).ToArray(),
                Series = new string[] { "小时新增设备" },
                Datas = this.RandomNumbers(24),
            };

            info.HourDAU = new StatisticsData()
            {
                Labels = ParallelEnumerable.Range(0, 24).Select(i => i.ToString()).ToArray(),
                Series = new string[] { "小时DAU" },
                Datas = this.RandomNumbers(24),
            };

            info.HourWAU = new StatisticsData()
            {
                Labels = ParallelEnumerable.Range(0, 24).Select(i => i.ToString()).ToArray(),
                Series = new string[] { "小时WAU" },
                Datas = this.RandomNumbers(24),
            };

            info.HourMAU = new StatisticsData()
            {
                Labels = ParallelEnumerable.Range(0, 24).Select(i => i.ToString()).ToArray(),
                Series = new string[] { "小时MAU" },
                Datas = this.RandomNumbers(24),
            };

            return info;
        }

        private double[] RandomNumbers(int length)
        {
            Random random = new Random();
            return new int[length].Select(i => Math.Floor(random.NextDouble() * 10000) / 100).ToArray();
        }
    }
}
