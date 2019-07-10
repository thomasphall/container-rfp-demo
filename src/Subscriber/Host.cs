// ---------------------------------------------------------------------------------------------------------------
// <copyright file="Host.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// Copyright 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

using Common.Messaging;

namespace Subscriber
{
    internal class Host : HostBase<SubscriberModule>
    {
        private const string ENDPOINT_NAME = "Subscriber";

        public Host()
            : base(ENDPOINT_NAME, 10)
        {
        }

        protected override void StartTasks()
        {
            // Nothing to do.
        }

        protected override Task WaitForTasksToFinish()
        {
            // Nothing to do.
            return Task.CompletedTask;
        }
    }
}
