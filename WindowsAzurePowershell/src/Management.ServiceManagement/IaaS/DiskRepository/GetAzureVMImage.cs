﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.ServiceManagement.IaaS.DiskRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using Samples.WindowsAzure.ServiceManagement;
    using Model;
    using Cmdlets.Common;

    [Cmdlet(VerbsCommon.Get, "AzureVMImage"), OutputType(typeof(IEnumerable<OSImageContext>))]
    public class GetAzureVMImage : CloudBaseCmdlet<IServiceManagement>
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, Mandatory = false, HelpMessage = "Name of the image in the image library.")]
        [ValidateNotNullOrEmpty]
        public string ImageName
        {
            get;
            set;
        }

        protected override void OnProcessRecord()
        {
            Func<Operation, IEnumerable<OSImage>, object> func = (operation, images) => images.Select(d => new OSImageContext
            {
                OperationId = operation.OperationTrackingId,
                OperationDescription = CommandRuntime.ToString(),
                OperationStatus = operation.Status,
                AffinityGroup = d.AffinityGroup,
                Category = d.Category,
                Label = d.Label,
                Location = d.Location,
                MediaLink = d.MediaLink,
                ImageName = d.Name,
                OS = d.OS,
                LogicalSizeInGB = d.LogicalSizeInGB
            });
            if (!string.IsNullOrEmpty(this.ImageName))
            {
                ExecuteClientActionInOCS(
                    null,
                    CommandRuntime.ToString(),
                    s => this.Channel.GetOSImage(s, this.ImageName),
                    WaitForOperation,
                    (operation, image) => func(operation, new[] { image }));
            }
            else
            {
                ExecuteClientActionInOCS(
                    null,
                    CommandRuntime.ToString(),
                    s => this.Channel.ListOSImages(s),
                    WaitForOperation,
                    (operation, images) => func(operation, images));

            }
        }
    }
}