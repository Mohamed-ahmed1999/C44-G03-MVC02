﻿using GymManagementBLL.ViewModels.AnalyticsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interface
{
    public interface IAnalyticsService
    {
        AnalyticsViewModel GetAnalyticsData();
    }
}
