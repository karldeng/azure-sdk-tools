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

using System.Management.Automation;
using Microsoft.WindowsAzure.Commands.Service.Gateway;
using Microsoft.WindowsAzure.Commands.ServiceManagement.Model;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.WindowsAzure.Management.Network.Models;
using Microsoft.WindowsAzure.ServiceManagement;

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.IaaS
{
    [Cmdlet(VerbsCommon.Set, "AzureVNetGatewayKey"), OutputType(typeof(SharedKeyContext))]
    public class SetAzureVNetGatewayKey : ServiceManagementBaseCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The virtual network name.")]
        [ValidateNotNullOrEmpty]
        public string VNetName
        {
            get;
            set;
        }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The local network site name.")]
        [ValidateNotNullOrEmpty]
        public string LocalNetworkSiteName
        {
            get;
            set;
        }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The shared key that will be used on the Azure Gateway and the customer's VPN device.")]
        [ValidateNotNullOrEmpty]
        public string SharedKey
        {
            get;
            set;
        }

        protected override void OnProcessRecord()
        {
            ServiceManagementProfile.Initialize();

            this.ExecuteClientActionNewSM(
                null,
                this.CommandRuntime.ToString(),
                () => this.NetworkClient.Gateways.SetSharedKey(
                            this.VNetName,
                            this.LocalNetworkSiteName,
                            new GatewaySetSharedKeyParameters()
                            {
                                Value = SharedKey,
                            }));
        }
    }
}
